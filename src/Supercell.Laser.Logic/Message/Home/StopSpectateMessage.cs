namespace Supercell.Laser.Logic.Message.Home
{
    public class StopSpectateMessage : GameMessage
    {
        public override int GetMessageType()
        {
            return 14107;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
