namespace Supercell.Laser.Logic.Message.Friends
{
    using Supercell.Laser.Logic.Friends;

    public class FriendOnlineStatusEntryMessage : GameMessage
    {
        public long AvatarId;
        public int PlayerStatus;

        public override void Encode()
        {
            Stream.WriteLong(AvatarId);
            if (Stream.WriteBoolean(PlayerStatus >= 0))
            {
                Stream.WriteLong(AvatarId);
                Stream.WriteVInt(PlayerStatus);
                Stream.WriteVInt(0);
                Stream.WriteBoolean(false);
                Stream.WriteBoolean(false);
            }
        }

        public override int GetMessageType()
        {
            return 24555;
        }

        public override int GetServiceNodeType()
        {
            return 3;
        }
    }
}
