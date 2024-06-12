namespace Supercell.Laser.Logic.Message
{
    using Supercell.Laser.Titan.DataStream;

    public abstract class GameMessage
    {
        protected readonly ByteStream Stream;
        private int _version;

        public GameMessage()
        {
            Stream = new ByteStream(10);
        }

        public virtual void Encode()
        {
            ;
        }

        public virtual void Decode()
        {
            ;
        }

        public abstract int GetMessageType();
        public abstract int GetServiceNodeType();

        public void SetVersion(int version)
        {
            _version = version;
        }

        public int GetVersion()
        {
            return _version;
        }

        public ByteStream GetByteStream()
        {
            return Stream;
        }

        public byte[] GetMessageBytes()
        {
            return Stream.GetByteArray();
        }

        public int GetEncodingLength()
        {
            return Stream.GetOffset();
        }
    }
}
