namespace Supercell.Laser.Logic.Message.Team
{
    using Supercell.Laser.Logic.Helper;

    public class TeamGameStartingMessage : GameMessage
    {
        public int Slot;
        public int LocationId;

        public override void Encode()
        {
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            ByteStreamHelper.WriteDataReference(Stream, LocationId);
        }

        public override int GetMessageType()
        {
            return 24130;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
