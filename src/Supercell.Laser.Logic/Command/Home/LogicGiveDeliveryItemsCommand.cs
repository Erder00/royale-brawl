namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Gatcha;
    using Supercell.Laser.Titan.DataStream;

    public class LogicGiveDeliveryItemsCommand : Command
    {
        public readonly List<DeliveryUnit> DeliveryUnits;
        public int RewardTrackType { get; set; }
        public int RewardForRank { get; set; }

        public LogicGiveDeliveryItemsCommand() : base()
        {
            DeliveryUnits = new List<DeliveryUnit>();
        }

        public override void Encode(ByteStream stream)
        {
            stream.WriteVInt(0);

            stream.WriteVInt(DeliveryUnits.Count);
            foreach (DeliveryUnit unit in DeliveryUnits)
            {
                unit.Encode(stream);
            }

            stream.WriteBoolean(false);

            stream.WriteVInt(RewardTrackType);
            stream.WriteVInt(RewardForRank);
            stream.WriteVInt(0);

            stream.WriteVInt(1);
            base.Encode(stream);
        }

        public override int Execute(HomeMode homeMode)
        {
            foreach (DeliveryUnit unit in DeliveryUnits)
            {
                foreach (GatchaDrop drop in unit.GetDrops())
                {
                    drop.DoDrop(homeMode);
                }
            }

            return 0;
        }

        public override int GetCommandType()
        {
            return 203;
        }
    }
}
