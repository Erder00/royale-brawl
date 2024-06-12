namespace Supercell.Laser.Logic.Team.Stream
{
    using Supercell.Laser.Titan.DataStream;

    public class TeamCreatedStreamEntry : TeamStreamEntry
    {
        public long TeamId;

        public override void Encode(ByteStream encoder)
        {
            base.Encode(encoder);
            encoder.WriteLong(TeamId);
            encoder.WriteVInt(0);
            encoder.WriteVInt(0);
            encoder.WriteVInt(0);
        }

        public override int GetStreamEntryType()
        {
            return 77;
        }
    }
}
