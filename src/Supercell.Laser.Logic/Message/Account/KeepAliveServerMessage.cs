namespace Supercell.Laser.Logic.Message.Account
{
    public class KeepAliveServerMessage : GameMessage
    {
        public override int GetMessageType()
        {
            return 20108;
        }

        public override int GetServiceNodeType()
        {
            return 1;
        }
    }
}
