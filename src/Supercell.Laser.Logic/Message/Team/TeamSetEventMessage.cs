namespace Supercell.Laser.Logic.Message.Team
{
    public class TeamSetEventMessage : GameMessage
    {
        public int EventSlot { get; set; }

        public override void Decode()
        {
            base.Decode();

            Stream.ReadVInt();
            EventSlot = Stream.ReadVInt();
        }

        public override int GetMessageType()
        {
            return 14362;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
