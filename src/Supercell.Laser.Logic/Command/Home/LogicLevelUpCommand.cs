namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Titan.DataStream;

    public class LogicLevelUpCommand : Command
    {
        private int CharacterId;

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            CharacterId = ByteStreamHelper.ReadDataReference(stream);
        }

        public override int Execute(HomeMode homeMode)
        {
            Hero hero = homeMode.Avatar.GetHero(CharacterId);
            if (hero == null) return -1;

            if (hero.PowerLevel > Hero.UpgradeCostTable.Length) return -2;
            if (hero.PowerPoints < Hero.UpgradePowerPointsTable[hero.PowerLevel]) return -3;

            if (!homeMode.Avatar.UseGold(Hero.UpgradeCostTable[hero.PowerLevel])) return -4;

            hero.PowerLevel++;

            return 0;
        }

        public override int GetCommandType()
        {
            return 520;
        }
    }
}
