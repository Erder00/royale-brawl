namespace Supercell.Laser.Logic.Message.Club
{
    using Supercell.Laser.Logic.Stream.Entry;

    public class AllianceStreamEntryMessage : GameMessage
    {
        public AllianceStreamEntry Entry;

        public override void Encode()
        {
            Entry.Encode(Stream);
        }

        public override int GetMessageType()
        {
            return 24312;
        }

        public override int GetServiceNodeType()
        {
            return 11;
        }
    }
}
