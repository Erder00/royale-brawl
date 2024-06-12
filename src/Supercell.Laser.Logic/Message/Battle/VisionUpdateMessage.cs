namespace Supercell.Laser.Logic.Message.Battle
{
    using Supercell.Laser.Titan.DataStream;

    public class VisionUpdateMessage : GameMessage
    {
        public int Tick;
        public int HandledInputs;
        public int Viewers;
        public BitStream VisionBitStream;

        public VisionUpdateMessage() : base()
        {
            ;
        }

        public override void Encode()
        {
            Stream.WriteVInt(Tick);
            Stream.WriteVInt(HandledInputs);
            Stream.WriteVInt(0); // unknown
            Stream.WriteVInt(Viewers);
            Stream.WriteBoolean(false);

            Stream.WriteBytes(VisionBitStream.GetByteArray(), VisionBitStream.GetLength());
        }

        public override int GetMessageType()
        {
            return 24109;
        }

        public override int GetServiceNodeType()
        {
            return 4;
        }
    }
}
