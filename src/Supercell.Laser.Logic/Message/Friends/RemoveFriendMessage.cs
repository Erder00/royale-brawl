namespace Supercell.Laser.Logic.Message.Friends
{
    public class RemoveFriendMessage : FriendAvatarBaseMessage
    {
        public override int GetMessageType()
        {
            return 10506;
        }

        public override int GetServiceNodeType()
        {
            return 3;
        }
    }
}
