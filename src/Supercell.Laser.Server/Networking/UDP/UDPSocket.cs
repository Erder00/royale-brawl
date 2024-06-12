namespace Supercell.Laser.Server.Networking
{
    using Supercell.Laser.Logic.Battle;
    using Supercell.Laser.Logic.Message;
    using Supercell.Laser.Logic.Message.Battle;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Server.Message;
    using System.Net;
    using System.Linq;
    using System.Threading.Tasks;
    using Supercell.Laser.Logic.Battle.Input;

    public class UDPSocket
    {
        public readonly long SessionId;
        private EndPoint EndPoint;

        public BattleMode Battle;
        public bool IsConnected => EndPoint != null;

        public Connection TCPConnection;
        public bool IsSpectator;

        public UDPSocket(long sessionId)
        {
            SessionId = sessionId;
        }

        public void SetEndPoint(EndPoint endPoint)
        {
            EndPoint = endPoint;
        }

        public void SendMessage(GameMessage message)
        {
            if (message.GetEncodingLength() == 0) message.Encode();

            ByteStream stream = new ByteStream(10);
            stream.WriteLong(SessionId);
            stream.WriteShort(0);
            stream.WriteVInt(message.GetMessageType());
            stream.WriteVInt(message.GetEncodingLength());
            stream.WriteBytesWithoutLength(message.GetMessageBytes(), message.GetEncodingLength());

            UDPGateway.SendTo(stream.GetByteArray(), 0, stream.GetOffset(), EndPoint);
        }

        public void ProcessReceive(ByteStream stream)
        {
            // async!
            Task.Run(() =>
            {
                int type = stream.ReadVInt();
                int length = stream.ReadVInt();
                byte[] data = stream.ReadBytes(length, 2000);

                GameMessage message = MessageFactory.Instance.CreateMessageByType(type);
                if (message != null)
                {
                    message.GetByteStream().SetByteArray(data, length);
                    message.Decode();
                    HandleMessage(message);
                }
            });
        }

        private void HandleMessage(GameMessage message)
        {
            if (message.GetMessageType() == 10555)
            {
                ClientInputMessage clientInputMessage = (ClientInputMessage)message;
                while (clientInputMessage.Inputs.TryDequeue(out ClientInput clientInput))
                {
                    if (!IsSpectator)
                        Battle.AddClientInput(clientInput, SessionId);
                    else
                        Battle.HandleSpectatorInput(clientInput, SessionId);
                }
            }
        }
    }
}
