namespace Supercell.Laser.Logic.Message.Home
{
    public class OutOfSyncMessage : GameMessage
    {
        public override void Encode()
        {
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
        }

        public override int GetMessageType()
        {
            return 24104;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
