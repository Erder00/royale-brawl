namespace Supercell.Laser.Logic.Message.Club
{
    using Supercell.Laser.Logic.Helper;

    public class ChangeAllianceSettingsMessage : GameMessage
    {
        public string Description;
        public int BadgeId;
        public int RequiredTrophies;

        public override void Decode()
        {
            Description = Stream.ReadString();

            BadgeId = ByteStreamHelper.ReadDataReference(Stream);
            ByteStreamHelper.ReadDataReference(Stream); // Region CSV

            Stream.ReadVInt(); // Club Type
            RequiredTrophies = Stream.ReadVInt();
        }

        public override int GetMessageType()
        {
            return 14316;
        }

        public override int GetServiceNodeType()
        {
            return 11;
        }
    }
}
