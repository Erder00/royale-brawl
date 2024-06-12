namespace Supercell.Laser.Logic.Message.Home
{
    public class MatchMakingCancelledMessage : GameMessage
    {
        public override int GetMessageType()
        {
            return 20406;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
