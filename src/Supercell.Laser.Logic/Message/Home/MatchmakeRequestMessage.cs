namespace Supercell.Laser.Logic.Message.Home
{
    public class MatchmakeRequestMessage : GameMessage
    {
        public MatchmakeRequestMessage() : base()
        {
            ;
        }

        public int CharacterInstanceId;
        public int EventSlot;

        public override void Decode()
        {
            Stream.ReadVInt();
            CharacterInstanceId = Stream.ReadVInt();

            Stream.ReadVInt();
            EventSlot = Stream.ReadVInt();

            if (EventSlot > 3) EventSlot = 1;
        }

        public override int GetMessageType()
        {
            return 14103;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
