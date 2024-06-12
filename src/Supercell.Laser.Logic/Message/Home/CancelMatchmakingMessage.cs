namespace Supercell.Laser.Logic.Message.Home
{
    public class CancelMatchmakingMessage : GameMessage
    {
        public override int GetMessageType()
        {
            return 14106;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
