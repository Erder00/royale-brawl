namespace Supercell.Laser.Logic.Message.Home
{
    public class ChronosEventSeenMessage : GameMessage
    {
        public int Unknown { get; set; }

        public override void Decode()
        {
            Unknown = Stream.ReadVInt();
        }

        public override int GetMessageType()
        {
            return 14166;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
