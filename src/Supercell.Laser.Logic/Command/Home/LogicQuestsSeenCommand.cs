namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Quest;

    public class LogicQuestsSeenCommand : Command
    {
        public override int Execute(HomeMode homeMode)
        {
            foreach (Quest quest in homeMode.Home.Quests.QuestList.ToArray())
            {
                quest.QuestSeen = true;
            }

            return 0;
        }

        public override int GetCommandType()
        {
            return 533;
        }
    }
}
