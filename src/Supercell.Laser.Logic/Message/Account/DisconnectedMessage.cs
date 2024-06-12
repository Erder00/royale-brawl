namespace Supercell.Laser.Logic.Message.Account
{
    public class DisconnectedMessage : GameMessage
    {
        public int Reason { get; set; }

        public override void Encode()
        {
            Stream.WriteInt(Reason);
        }

        public override void Decode()
        {
            Reason = Stream.ReadInt();
        }

        public override int GetMessageType()
        {
            return 25892;
        }

        public override int GetServiceNodeType()
        {
            return 1;
        }
    }
}
