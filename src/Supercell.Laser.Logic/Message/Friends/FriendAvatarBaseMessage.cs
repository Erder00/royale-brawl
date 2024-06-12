namespace Supercell.Laser.Logic.Message.Friends
{
    public abstract class FriendAvatarBaseMessage : GameMessage
    {
        public long AvatarId;

        public override void Decode()
        {
            AvatarId = Stream.ReadLong();
        }
    }
}
