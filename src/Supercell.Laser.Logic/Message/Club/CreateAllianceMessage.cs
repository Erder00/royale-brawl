namespace Supercell.Laser.Logic.Message.Club
{
    using Supercell.Laser.Logic.Helper;

    public class CreateAllianceMessage : GameMessage
    {
        public string Name;
        public string Description;
        public int BadgeId;
        public int Type;
        public int RequiredTrophies;

        public override void Decode()
        {
            Name = Stream.ReadString();
            Description = Stream.ReadString();
            BadgeId = ByteStreamHelper.ReadDataReference(Stream);
            ByteStreamHelper.ReadDataReference(Stream);
            Type = Stream.ReadVInt();
            RequiredTrophies = Stream.ReadVInt();
        }

        public override int GetMessageType()
        {
            return 14301;
        }

        public override int GetServiceNodeType()
        {
            return 11;
        }
    }
}
