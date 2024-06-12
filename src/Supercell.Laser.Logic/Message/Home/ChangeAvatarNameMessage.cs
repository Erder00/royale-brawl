namespace Supercell.Laser.Logic.Message.Home
{
    public class ChangeAvatarNameMessage : GameMessage
    {
        public string Name;

        public ChangeAvatarNameMessage() : base()
        {
            ;
        }

        public override void Decode()
        {
            Name = Stream.ReadString();
        }

        public override int GetMessageType()
        {
            return 10212;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
