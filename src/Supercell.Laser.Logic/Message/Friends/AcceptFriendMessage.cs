namespace Supercell.Laser.Logic.Message.Friends
{
    public class AcceptFriendMessage : FriendAvatarBaseMessage
    {
        public override int GetMessageType()
        {
            return 10501;
        }

        public override int GetServiceNodeType()
        {
            return 3;
        }
    }
}
