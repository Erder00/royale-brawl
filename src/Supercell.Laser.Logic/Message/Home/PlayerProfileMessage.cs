namespace Supercell.Laser.Logic.Message.Home
{
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Club;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home.Structures;

    public class PlayerProfileMessage : GameMessage
    {
        public Profile Profile;
        public AllianceHeader AllianceHeader;
        public AllianceRole AllianceRole;

        public PlayerProfileMessage() : base()
        {
            ;
        }

        public override void Encode()
        {
            Profile.Encode(Stream);

            if (Stream.WriteBoolean(AllianceHeader != null))
            {
                AllianceHeader.Encode(Stream);
            }

            if (AllianceRole > AllianceRole.None)
                ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(25, (int)AllianceRole));
            else
                Stream.WriteVInt(0);
        }

        public override int GetMessageType()
        {
            return 24113;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
