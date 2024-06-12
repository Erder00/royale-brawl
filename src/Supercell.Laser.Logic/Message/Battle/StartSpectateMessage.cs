namespace Supercell.Laser.Logic.Message.Battle
{
    public class StartSpectateMessage : GameMessage
    {
        public long AccountId;

        public override void Decode()
        {
            AccountId = Stream.ReadLong();
        }

        public override int GetMessageType()
        {
            return 14104;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
