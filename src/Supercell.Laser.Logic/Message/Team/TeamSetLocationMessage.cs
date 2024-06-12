namespace Supercell.Laser.Logic.Message.Team
{
    public class TeamSetLocationMessage : GameMessage
    {
        public override int GetMessageType()
        {
            return 14363;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
