namespace Supercell.Laser.Logic.Message.Account
{
    public class ClientCapabilitiesMessage : GameMessage
    {
        public int Ping;

        public override void Encode()
        {
            Stream.WriteVInt(Ping);
        }

        public override void Decode()
        {
            Ping = Stream.ReadVInt();
        }

        public override int GetMessageType()
        {
            return 10107;
        }

        public override int GetServiceNodeType()
        {
            return 1;
        }
    }
}
