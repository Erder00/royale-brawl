namespace Supercell.Laser.Logic.Message.Team
{
    public class TeamInviteMessage : GameMessage
    {
        public long AvatarId;
        public int Team;

        public override void Decode()
        {
            AvatarId = Stream.ReadVLong();
            Team = Stream.ReadVInt();
        }

        public override int GetMessageType()
        {
            return 14365;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
