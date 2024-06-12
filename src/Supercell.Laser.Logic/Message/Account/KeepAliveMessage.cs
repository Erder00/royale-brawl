namespace Supercell.Laser.Logic.Message.Account
{
    public class KeepAliveMessage : GameMessage
    {
        public override int GetMessageType()
        {
            return 10108;
        }

        public override int GetServiceNodeType()
        {
            return 1;
        }
    }
}
