namespace Supercell.Laser.Logic.Message.Club
{
    public class AskForAllianceDataMessage : GameMessage
    {
        public long AllianceId;

        public override void Decode()
        {
            AllianceId = Stream.ReadLong();
        }

        public override int GetMessageType()
        {
            return 14302;
        }

        public override int GetServiceNodeType()
        {
            return 11;
        }
    }
}
