namespace Supercell.Laser.Logic.Message.Friends
{
    using Supercell.Laser.Logic.Friends;

    public class FriendListMessage : GameMessage
    {
        public Friend[] Friends;

        public override void Encode()
        {
            Stream.WriteInt(0);
            Stream.WriteBoolean(true);
            Stream.WriteInt(Friends.Length);
            foreach (Friend friend in Friends)
            {
                friend.Encode(Stream);
            }
        }

        public override int GetMessageType()
        {
            return 20105;
        }

        public override int GetServiceNodeType()
        {
            return 3;
        }
    }
}
