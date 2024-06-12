namespace Supercell.Laser.Logic.Message.Team
{
    using Supercell.Laser.Logic.Friends;

    public class TeamInvitationMessage : GameMessage
    {
        public int Unknown;
        public long TeamId;
        public Friend FriendEntry;

        public override void Encode()
        {
            Stream.WriteVInt(Unknown);
            Stream.WriteLong(TeamId);
            FriendEntry.Encode(Stream);
        }

        public override int GetMessageType()
        {
            return 24589;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
