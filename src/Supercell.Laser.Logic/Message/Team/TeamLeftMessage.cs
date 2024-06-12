namespace Supercell.Laser.Logic.Message.Team
{
    public class TeamLeftMessage : GameMessage
    {
        public int Reason;

        public override void Encode()
        {
            Stream.WriteInt(Reason);
        }

        public override int GetMessageType()
        {
            return 24125;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
