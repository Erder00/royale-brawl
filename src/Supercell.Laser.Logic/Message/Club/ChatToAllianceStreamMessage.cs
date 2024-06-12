namespace Supercell.Laser.Logic.Message.Club
{
    public class ChatToAllianceStreamMessage : GameMessage
    {
        public string Message;

        public override void Decode()
        {
            Message = Stream.ReadString();
        }

        public override int GetMessageType()
        {
            return 14315;
        }

        public override int GetServiceNodeType()
        {
            return 11;
        }
    }
}
