namespace Supercell.Laser.Logic.Message.Friends
{
    public class AskForFriendListMessage : GameMessage
    {
        public override int GetMessageType()
        {
            return 10504;
        }

        public override int GetServiceNodeType()
        {
            return 3;
        }
    }
}
