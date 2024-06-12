namespace Supercell.Laser.Logic.Message.Security
{
    public class ServerHelloMessage : GameMessage
    {
        private byte[] _serverHelloToken;

        public ServerHelloMessage() : base()
        {
            _serverHelloToken = new byte[24];
        }

        public override void Encode()
        {
            Stream.WriteBytes(_serverHelloToken, 24);
        }

        public void SetServerHelloToken(byte[] token)
        {
            _serverHelloToken = token;
        }

        public byte[] RemoveServerHelloToken()
        {
            byte[] token = _serverHelloToken;
            _serverHelloToken = null;
            return token;
        }

        public override int GetMessageType()
        {
            return 20100;
        }

        public override int GetServiceNodeType()
        {
            return 1;
        }
    }
}
