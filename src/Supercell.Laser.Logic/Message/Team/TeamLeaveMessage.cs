namespace Supercell.Laser.Logic.Message.Team
{
    public class TeamLeaveMessage : GameMessage
    {
        public override int GetMessageType()
        {
            return 14353;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
