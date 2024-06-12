namespace Supercell.Laser.Logic.Message.Home
{
    public class GoHomeMessage : GameMessage
    {
        public override int GetMessageType()
        {
            return 14101;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
