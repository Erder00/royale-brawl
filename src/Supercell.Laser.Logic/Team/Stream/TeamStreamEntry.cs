namespace Supercell.Laser.Logic.Team.Stream
{
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Math;

    public abstract class TeamStreamEntry
    {
        public LogicLong Id { get; set; }
        public LogicLong AccountId { get; set; }
        public string Name { get; set; }

        public virtual void Encode(ByteStream encoder)
        {
            encoder.WriteVInt(this.GetStreamEntryType());
            encoder.WriteVLong(Id);
            encoder.WriteVLong(AccountId);
            encoder.WriteString(Name);
            encoder.WriteVInt(0);
            encoder.WriteVInt(0);
            encoder.WriteVInt(0);
        }

        public abstract int GetStreamEntryType();
    }
}
