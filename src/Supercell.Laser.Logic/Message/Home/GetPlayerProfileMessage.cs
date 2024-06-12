namespace Supercell.Laser.Logic.Message.Home
{
    public class GetPlayerProfileMessage : GameMessage
    {
        public long AccountId;

        public GetPlayerProfileMessage() : base()
        {
            ;
        }

        public override void Decode()
        {
            AccountId = Stream.ReadLong();
        }

        public override int GetMessageType()
        {
            return 14113;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
