namespace Supercell.Laser.Logic.Message.Home
{
    public class GoHomeFromOfflinePractiseMessage : GameMessage
    {
        public override int GetMessageType()
        {
            return 14109;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
