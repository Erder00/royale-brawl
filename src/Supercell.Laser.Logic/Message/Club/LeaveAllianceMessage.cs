namespace Supercell.Laser.Logic.Message.Club
{
    public class LeaveAllianceMessage : GameMessage
    {
        public override int GetMessageType()
        {
            return 14308;
        }

        public override int GetServiceNodeType()
        {
            return 11;
        }
    }
}
