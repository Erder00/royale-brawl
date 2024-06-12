namespace Supercell.Laser.Logic.Message.Udp
{
    public class UdpConnectionInfoMessage : GameMessage
    {
        public int ServerPort;
        public string ServerAddress;

        public long SessionId;

        public UdpConnectionInfoMessage() : base()
        {
            ;
        }

        public override void Encode()
        {
            Stream.WriteVInt(ServerPort);
            Stream.WriteString(ServerAddress);

            Stream.WriteInt(10);
            Stream.WriteLong(SessionId);
            Stream.WriteShort(0);

            Stream.WriteInt(0); // nonce
        }

        public override int GetMessageType()
        {
            return 24112;
        }

        public override int GetServiceNodeType()
        {
            return 27;
        }
    }
}
