namespace Supercell.Laser.Logic.Message.Team
{
    public class TeamMemberStatusMessage : GameMessage
    {
        public int Status;

        public override void Decode()
        {
            Status = Stream.ReadVInt();
        }

        public override int GetMessageType()
        {
            return 14361;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
