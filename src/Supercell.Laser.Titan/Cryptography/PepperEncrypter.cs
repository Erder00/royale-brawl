namespace Supercell.Laser.Titan.Cryptography
{
    using static Supercell.Laser.Titan.Library.TweetNaCl;

    public class PepperEncrypter : StreamEncrypter
    {
        private byte[] Key;
        private byte[] Nonce;

        public PepperEncrypter(byte[] key, byte[] nonce)
        {
            Key = key;
            Nonce = nonce;
        }

        public override int Decrypt(byte[] input, byte[] output, int length)
        {
            NextNonce(Nonce);

            try
            {
                byte[] res = crypto_secretbox_xsalsa19poly1305_tweet_open(input, Nonce, Key);
                Buffer.BlockCopy(res, 0, output, 0, res.Length);
            }
            catch (InvalidCipherTextException)
            {
                return -1;
            }
            return 0;
        }

        public override int Encrypt(byte[] input, byte[] output, int length)
        {
            NextNonce(Nonce);

            byte[] res = crypto_secretbox_xsalsa19poly1305_tweet(input, Nonce, Key);
            Buffer.BlockCopy(res, 0, output, 0, res.Length);
            return 0;
        }

        public void NextNonce(byte[] nonce)
        {
            int timesToIncrease = 3;
            for (int j = 0; j < timesToIncrease; j++)
            {
                ushort c = 1;
                for (UInt32 i = 0; i < nonce.Length; i++)
                {
                    c += (ushort)nonce[i];
                    nonce[i] = (byte)c;
                    c >>= 8;
                }
            }
        }

        public override int GetEncryptionOverhead()
        {
            return 16;
        }
    }
}
