namespace Supercell.Laser.Logic.Message.Team
{
    public class TeamInvitationResponseMessage : GameMessage
    {
        public int Response;
        public long TeamId;
        public bool MutePlayer;

        public override void Decode()
        {
            Response = Stream.ReadVInt();
            TeamId = Stream.ReadLong();
            MutePlayer = Stream.ReadBoolean();
        }

        public override int GetMessageType()
        {
            return 14479;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
