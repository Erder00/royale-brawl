namespace Supercell.Laser.Logic.Message.Udp
{
    public class ClientInfoMessage : GameMessage
    {
        public override int GetMessageType()
        {
            return 10177;
        }

        public override int GetServiceNodeType()
        {
            return 27;
        }
    }
}
