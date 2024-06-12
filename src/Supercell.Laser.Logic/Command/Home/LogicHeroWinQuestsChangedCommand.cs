namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Quest;
    using Supercell.Laser.Titan.DataStream;

    public class LogicHeroWinQuestsChangedCommand : Command
    {
        public Quests Quests { get; set; }

        public override void Encode(ByteStream stream)
        {
            if (stream.WriteBoolean(Quests != null))
            {
                Quests.Encode(stream);
            }

            stream.WriteVInt(0);
            base.Encode(stream);
        }

        public override int Execute(HomeMode homeMode)
        {
            return 0;
        }

        public override int GetCommandType()
        {
            return 220;
        }
    }
}
