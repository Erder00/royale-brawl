namespace Supercell.Laser.Logic.Message.Team.Stream
{
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Titan.Math;

    public class TeamPremadeChatMessage : GameMessage
    {
        public TeamPremadeChatMessage()
        {
            TargetId = -1;
        }

        public int MessageDataId { get; set; }
        public LogicLong TargetId { get; set; }
        public int Unknown1 { get; set; }
        public int Unknown2 { get; set; }

        public override void Decode()
        {
            base.Decode();

            MessageDataId = ByteStreamHelper.ReadDataReference(Stream);
            bool v2 = Stream.ReadBoolean();
            if (v2)
                TargetId = Stream.ReadLong();

            Unknown1 = Stream.ReadVInt();
            Unknown2 = Stream.ReadVInt();

            if (!Stream.IsAtEnd())
                Stream.ReadVInt();
        }

        public override int GetMessageType()
        {
            return 14369;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
