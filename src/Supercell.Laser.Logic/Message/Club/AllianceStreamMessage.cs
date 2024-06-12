namespace Supercell.Laser.Logic.Message.Club
{
    using Supercell.Laser.Logic.Stream.Entry;

    public class AllianceStreamMessage : GameMessage
    {
        public AllianceStreamEntry[] Entries;

        public override void Encode()
        {
            if (Entries == null)
            {
                Stream.WriteVInt(-1);
                return;
            }

            Stream.WriteVInt(Entries.Length);
            foreach (var entry in Entries)
            {
                entry.Encode(Stream);
            }
        }

        public override int GetMessageType()
        {
            return 24311;
        }

        public override int GetServiceNodeType()
        {
            return 11;
        }
    }
}
