namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Titan.DataStream;

    public class LogicPurchaseBrawlPassCommand : Command
    {
        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            stream.ReadVInt();
            stream.ReadBoolean();
        }

        public override int Execute(HomeMode homeMode)
        {
            if (homeMode.Avatar.UseDiamonds(169))
            {
                homeMode.Home.HasPremiumPass = true;
                return 0;
            }

            return -1;
        }

        public override int GetCommandType()
        {
            return 534;
        }
    }
}
