namespace Supercell.Laser.Logic.Message.Account
{
    public class ShutdownStartedMessage : GameMessage
    {
        public override void Encode()
        {
            Stream.WriteInt(0);
        }

        public override int GetMessageType()
        {
            return 20161;
        }

        public override int GetServiceNodeType()
        {
            return 1;
        }
    }
}
