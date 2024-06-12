namespace Supercell.Laser.Logic.Message.Home
{
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Home;

    public class OwnHomeDataMessage : GameMessage
    {
        public OwnHomeDataMessage() : base()
        {
            ;
        }

        public ClientHome Home;
        public ClientAvatar Avatar;

        public override void Encode()
        {
            Home.Encode(Stream);
            Avatar.Encode(Stream);
            
            Stream.WriteVInt(1337);
        }

        public override int GetMessageType()
        {
            return 24101;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
