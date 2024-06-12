namespace Supercell.Laser.Logic.Command.Avatar
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Titan.DataStream;

    public class LogicDiamondsAddedCommand : Command
    {
        public int Amount;

        public override void Encode(ByteStream stream)
        {
            base.Encode(stream);
        }

        public override int Execute(HomeMode homeMode)
        {
            homeMode.Avatar.AddDiamonds(Amount);
            return 0;
        }

        public override int GetCommandType()
        {
            return 202;
        }
    }
}
