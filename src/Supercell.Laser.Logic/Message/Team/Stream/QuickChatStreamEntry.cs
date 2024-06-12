namespace Supercell.Laser.Logic.Message.Team.Stream
{
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Team.Stream;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Math;

    public class QuickChatStreamEntry : TeamStreamEntry
    {
        public int MessageDataId { get; set; }
        public LogicLong TargetId { get; set; }
        public string TargetPlayerName { get; set; }

        public int Unknown1 { get; set; }
        public int Unknown2 { get; set; }

        public override void Encode(ByteStream encoder)
        {
            base.Encode(encoder);

            ByteStreamHelper.WriteDataReference(encoder, MessageDataId);

            if (encoder.WriteBoolean(TargetId > 0))
                encoder.WriteLong(TargetId);
            encoder.WriteString(TargetPlayerName);
            encoder.WriteVInt(Unknown1);
            encoder.WriteVInt(Unknown2);
        }

        public override int GetStreamEntryType()
        {
            return 8;
        }
    }
}
