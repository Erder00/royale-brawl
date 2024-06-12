namespace Supercell.Laser.Logic.Message.Club
{
    public class JoinAllianceMessage : GameMessage
    {
        public long AllianceId;

        public override void Decode()
        {
            AllianceId = Stream.ReadLong();
        }

        public override int GetMessageType()
        {
            return 14305;
        }

        public override int GetServiceNodeType()
        {
            return 11;
        }
    }
}
