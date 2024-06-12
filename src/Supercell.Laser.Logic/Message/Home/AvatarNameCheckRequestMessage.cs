namespace Supercell.Laser.Logic.Message.Home
{
    public class AvatarNameCheckRequestMessage : GameMessage
    {
        public string Name { get; set; }

        public override void Decode()
        {
            base.Decode();

            Name = Stream.ReadString();
        }

        public override int GetMessageType()
        {
            return 14600;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
