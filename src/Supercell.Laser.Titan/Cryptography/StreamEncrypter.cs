namespace Supercell.Laser.Titan.Cryptography
{
    public abstract class StreamEncrypter
    {
        public abstract int Decrypt(byte[] input, byte[] output, int length);
        public abstract int Encrypt(byte[] input, byte[] output, int length);
        public abstract int GetEncryptionOverhead();
    }
}
