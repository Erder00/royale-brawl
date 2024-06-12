namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Titan.DataStream;

    public class LogicPurchaseBrawlPassProgressCommand : Command
    {
        public int Unknown { get; set; }

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            Unknown = stream.ReadVInt();
        }

        public override int Execute(HomeMode homeMode)
        {
            return -1;
        }

        public override int GetCommandType()
        {
            return 536;
        }
    }
}
