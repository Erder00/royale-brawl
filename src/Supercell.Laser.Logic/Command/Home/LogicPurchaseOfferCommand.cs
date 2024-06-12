namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Titan.DataStream;

    public class LogicPurchaseOfferCommand : Command
    {
        public int OfferIndex;
        public int Unknown;

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);

            OfferIndex = stream.ReadVInt();
            Unknown = ByteStreamHelper.ReadDataReference(stream);
        }

        public override int Execute(HomeMode homeMode)
        {
            if (!CanExecute(homeMode)) return -1;

            homeMode.Home.PurchaseOffer(OfferIndex);

            return 0;
        }

        public override int GetCommandType()
        {
            return 519;
        }
    }
}
