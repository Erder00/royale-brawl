namespace Supercell.Laser.Logic.Message.Friends
{
    using Supercell.Laser.Logic.Friends;

    public class FriendListUpdateMessage : GameMessage
    {
        public Friend Entry;

        public override void Encode()
        {
            Stream.WriteBoolean(true);
            Entry.Encode(Stream);
        }

        public override int GetMessageType()
        {
            return 20106;
        }

        public override int GetServiceNodeType()
        {
            return 3;
        }
    }
}
