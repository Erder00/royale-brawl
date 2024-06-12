using Supercell.Laser.Server.Networking.Security;
using Supercell.Laser.Titan.Library.Blake;

namespace Supercell.Laser.Server.Networking
{
    using Supercell.Laser.Logic.Message;
    using Supercell.Laser.Server.Message;
    using Supercell.Laser.Titan.Cryptography;
    using Supercell.Laser.Titan.Library;
    using Supercell.Laser.Titan.Math;
    using System.Linq;

    public class Messaging
    {
        public byte[] SessionToken { get; }

        private Connection Connection;
        private int PepperState;

        private StreamEncrypter Encrypter;
        private StreamEncrypter Decrypter;

        private MessageFactory MessageFactory;

        private byte[] RNonce;
        private byte[] SNonce;
        private byte[] SecretKey;

        public int Seed { get; set; }

        public Messaging(Connection connection)
        {
            Connection = connection;
            MessageFactory = MessageFactory.Instance;

            PepperState = 2;

            SessionToken = new byte[24];
            SNonce = new byte[24];
            SecretKey = new byte[32];

            TweetNaCl.RandomBytes(SessionToken);
            TweetNaCl.RandomBytes(SNonce);
            TweetNaCl.RandomBytes(SecretKey);
        }

        public void Send(GameMessage message)
        {
            if (message.GetMessageType() == 20100)
            {
                EncryptAndWrite(message);
            }
            else
            {
                Processor.Send(Connection, message);
            }
        }

        public void EncryptAndWrite(GameMessage message)
        {
            if (message.GetEncodingLength() == 0) message.Encode();

            byte[] payload = new byte[message.GetEncodingLength()];
            Buffer.BlockCopy(message.GetMessageBytes(), 0, payload, 0, payload.Length);

            int messageType = message.GetMessageType();
            int version = message.GetVersion();

            switch (PepperState)
            {
                case 4:
                    payload = SendPepperLoginResponse(payload);
                    break;
                case 5:
                    byte[] encrypted = new byte[payload.Length + Encrypter.GetEncryptionOverhead()];
                    Encrypter.Encrypt(payload, encrypted, payload.Length);
                    payload = encrypted;
                    break;
            }

            byte[] stream = new byte[payload.Length + 7];

            int length = payload.Length;

            stream[0] = (byte)(messageType >> 8);
            stream[1] = (byte)(messageType);
            stream[2] = (byte)(length >> 16);
            stream[3] = (byte)(length >> 8);
            stream[4] = (byte)(length);
            stream[5] = (byte)(version >> 8);
            stream[6] = (byte)(version);

            Buffer.BlockCopy(payload, 0, stream, 7, payload.Length);
            Connection.Write(stream);
        }

        public int OnReceive()
        {
            long position = Connection.Memory.Position;
            Connection.Memory.Position = 0;

            byte[] headerBuffer = new byte[7];
            Connection.Memory.Read(headerBuffer, 0, 7);

            // Messaging::readHeader inling? yes.
            int type = headerBuffer[0] << 8 | headerBuffer[1];
            int length = headerBuffer[2] << 16 | headerBuffer[3] << 8 | headerBuffer[4];
            int version = headerBuffer[5] << 8 | headerBuffer[6];

            byte[] payload = new byte[length];
            if (Connection.Memory.Read(payload, 0, length) < length)
            { // Packet still not received
                Connection.Memory.Position = position;
                return 0;
            }

            if (this.ReadNewMessage(type, length, version, payload) != 0)
            {
                return -1;
            }

            byte[] all = Connection.Memory.ToArray();
            byte[] buffer = all.Skip(length + 7).ToArray();

            Connection.Memory = new MemoryStream();
            Connection.Memory.Write(buffer, 0, buffer.Length);

            if (buffer.Length >= 7) OnReceive();
            return 0;
        }

        private int ReadNewMessage(int type, int length, int version, byte[] payload)
        {
            switch (PepperState)
            {
                case 2:
                    if (type == 10100) PepperState = 3;
                    else return -1;
                    break;
                case 3:
                    if (type != 10101) return -1;
                    payload = HandlePepperLogin(payload);
                    if (payload == null) return -1;
                    break;
                case 5:
                    byte[] decrypted = new byte[length - Decrypter.GetEncryptionOverhead()];
                    int result = Decrypter.Decrypt(payload, decrypted, length);
                    payload = decrypted;
                    if (result != 0) return -1;
                    break;
            }

            GameMessage message = MessageFactory.CreateMessageByType(type);
            if (message != null)
            {
                message.GetByteStream().SetByteArray(payload, payload.Length);
                message.Decode();
                if (message.GetMessageType() == 10100)
                {
                    Connection.MessageManager.ReceiveMessage(message);
                }
                else
                {
                    Processor.Receive(Connection, message);
                }
            }
            else
            {
                Logger.Print("Ignoring message of unknown type " + type);
            }

            return 0;
        }

        private byte[] client_pk;
        private byte[] client_sk;

        private byte[] HandlePepperLogin(byte[] payload)
        {
            try
            {
                client_pk = payload.Take(32).ToArray();

                client_sk = new byte[32];
                LogicRandom rand = new LogicRandom(Seed);

                int c = 0;
                for (int i = 0; i < 12; i++)
                {
                    c = rand.Rand(256);
                }

                for (int i = 0; i < 32; i++)
                {
                    client_sk[i] = (byte)(rand.Rand(256) ^ c);
                }

                if (!TweetNaCl.CryptoScalarmultBase(client_sk).SequenceEqual(client_pk))
                {
                    Logger.Print("Cryptography error. 10101 public key is invalid!");
                    return null;
                } 

                Blake2BHasher hasher = new Blake2BHasher();
                hasher.Update(client_pk);
                hasher.Update(PepperKey.SERVER_PK);
                byte[] nonce = hasher.Finish();

                byte[] decrypted = TweetNaCl.CryptoBoxOpen(payload.Skip(32).ToArray(), nonce, PepperKey.SERVER_PK, client_sk);

                byte[] session_token = decrypted.Take(24).ToArray();
                if (!session_token.SequenceEqual(SessionToken))
                {
                    Logger.Print("Cryptography error. Session token is invalid!");
                    return null;
                }

                PepperState = 4;

                RNonce = decrypted.Skip(24).Take(24).ToArray();

                return decrypted.Skip(48).ToArray();
            } catch (TweetNaCl.InvalidCipherTextException)
            {
                Console.WriteLine("Failed to decrypt 10101");
                return null;
            }
        }

        private byte[] SendPepperLoginResponse(byte[] payload)
        {
            byte[] packet = new byte[payload.Length + 32 + 24];

            Buffer.BlockCopy(SNonce, 0, packet, 0, 24);
            Buffer.BlockCopy(SecretKey, 0, packet, 24, 32);
            Buffer.BlockCopy(payload, 0, packet, 24 + 32, payload.Length);
            
            Blake2BHasher hasher = new Blake2BHasher();
            hasher.Update(RNonce);
            hasher.Update(client_pk);
            hasher.Update(PepperKey.SERVER_PK);
            byte[] nonce = hasher.Finish();

            byte[] encrypted = TweetNaCl.CryptoBox(packet, nonce, PepperKey.SERVER_PK, client_sk);

            PepperState = 5;

            Decrypter = new PepperEncrypter(SecretKey, RNonce);
            Encrypter = new PepperEncrypter(SecretKey, SNonce);

            return encrypted;
        }
    }
}
