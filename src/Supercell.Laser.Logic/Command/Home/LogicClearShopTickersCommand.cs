namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Items;

    public class LogicClearShopTickersCommand : Command
    {
        public override int Execute(HomeMode homeMode)
        {
            OfferBundle[] bundles = homeMode.Home.OfferBundles.ToArray();
            foreach (OfferBundle bundle in bundles)
            {
                bundle.State = 2;
            }

            return 0;
        }

        public override int GetCommandType()
        {
            return 515;
        }
    }
}
