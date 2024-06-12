namespace Supercell.Laser.Logic.Team.Stream
{
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Math;

    public class ChatStreamEntry : TeamStreamEntry
    {
        public string Message { get; set; }

        public override void Encode(ByteStream encoder)
        {
            base.Encode(encoder);

            encoder.WriteString(Message);
        }

        public override int GetStreamEntryType()
        {
            return 2;
        }
    }
}
