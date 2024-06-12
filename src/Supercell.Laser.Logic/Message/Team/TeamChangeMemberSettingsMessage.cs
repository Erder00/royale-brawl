namespace Supercell.Laser.Logic.Message.Team
{
    using Supercell.Laser.Logic.Helper;

    public class TeamChangeMemberSettingsMessage : GameMessage
    {
        public int CharacterId;
        public int SkinId;

        public override void Decode()
        {
            CharacterId = ByteStreamHelper.ReadDataReference(Stream);
            SkinId = ByteStreamHelper.ReadDataReference(Stream);
        }

        public override int GetMessageType()
        {
            return 14354;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
