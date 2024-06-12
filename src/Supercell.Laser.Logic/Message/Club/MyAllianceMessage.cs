namespace Supercell.Laser.Logic.Message.Club
{
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Club;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Helper;

    public class MyAllianceMessage : GameMessage
    {
        public int OnlineMembers;
        public AllianceRole Role;
        public AllianceHeader AllianceHeader { get; set; }

        public override void Encode()
        {
            Stream.WriteVInt(OnlineMembers);
            if (Stream.WriteBoolean(AllianceHeader != null))
            {
                ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(25, (int)Role));
                AllianceHeader.Encode(Stream);
            }
        }

        public override int GetMessageType()
        {
            return 24399;
        }

        public override int GetServiceNodeType()
        {
            return 11;
        }
    }
}
