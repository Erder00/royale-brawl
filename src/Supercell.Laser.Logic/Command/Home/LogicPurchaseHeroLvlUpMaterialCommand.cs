namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Titan.DataStream;

    public class LogicPurchaseHeroLvlUpMaterialCommand : Command
    {
        private int PackIndex;

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            PackIndex = stream.ReadVInt();
        }

        public override int Execute(HomeMode homeMode)
        {
            if (PackIndex < 0 || PackIndex > ClientHome.GoldPacksAmount.Length) return -1;

            if (!homeMode.Avatar.UseDiamonds(ClientHome.GoldPacksPrice[PackIndex])) return -2;

            homeMode.Avatar.AddGold(ClientHome.GoldPacksAmount[PackIndex]);

            return 0;
        }

        public override int GetCommandType()
        {
            return 521;
        }
    }
}
