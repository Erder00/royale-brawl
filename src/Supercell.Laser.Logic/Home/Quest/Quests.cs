namespace Supercell.Laser.Logic.Home.Quest
{
    using Newtonsoft.Json;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Titan.DataStream;

    public class Quests
    {
        private static readonly int[] ALLOWED_MODES = { 0, 3, 6 };
        private static readonly int[] GOAL_TABLE = { 40000, 50000, 60000, 80000, 100000 };

        [JsonProperty("quest_list")]
        public List<Quest> QuestList;

        public Quests()
        {
            QuestList = new List<Quest>();
        }

        public void AddRandomQuests(List<Hero> unlockedHeroes, int count)
        {
            List<int> characters = unlockedHeroes.Select(x => x.CharacterId).ToList();

            Random rand = new Random();

            for (int i = 0; i < count; i++)
            {
                Quest quest = new Quest();

                quest.MissionType = rand.Next(1, 5);

                bool forCharacter = rand.Next(120) > 40;
                if (forCharacter)
                {
                    quest.CharacterId = characters[rand.Next(0, characters.Count)];
                    quest.GameModeVariation = -1;
                }
                else
                {
                    quest.GameModeVariation = ALLOWED_MODES[rand.Next(0, ALLOWED_MODES.Length)];
                }

                switch (quest.MissionType)
                {
                    case 1:
                        quest.QuestGoal = rand.Next(6, 13);
                        quest.Reward = quest.QuestGoal >= 10 ? 500 : 250;
                        break;
                    case 2:
                        quest.QuestGoal = rand.Next(12, 31);
                        quest.Reward = quest.QuestGoal >= 20 ? 500 : 250;
                        break;
                    case 3:
                    case 4:
                        quest.QuestGoal = GOAL_TABLE[rand.Next(0, GOAL_TABLE.Length)];
                        quest.Reward = quest.QuestGoal >= 80000 ? 500 : 250;
                        break;
                }

                QuestList.Add(quest);
            }
        }

        public List<Quest> UpdateQuestsProgress(int gameModeVariation, int characterId, int kills, int damage, int heals, ClientHome home)
        {
            List<Quest> completed = new List<Quest>();
            List<Quest> progressive = new List<Quest>();

            foreach (Quest quest in QuestList.ToArray())
            {
                if ((quest.GameModeVariation == gameModeVariation || quest.GameModeVariation == -1) 
                    && (quest.CharacterId == characterId || quest.CharacterId == 0))
                {
                    if (quest.MissionType == 1)
                    {
                        var progress = quest.Clone();
                        progress.Progress = 1;

                        quest.CurrentGoal += 1;
                        if (quest.CurrentGoal >= quest.QuestGoal)
                        {
                            completed.Add(quest);
                        }

                        progressive.Add(progress);
                    }
                    else if (quest.MissionType == 2)
                    {
                        var progress = quest.Clone();
                        progress.Progress = kills;

                        quest.CurrentGoal += kills;
                        if (quest.CurrentGoal >= quest.QuestGoal)
                        {
                            completed.Add(quest);
                        }

                        progressive.Add(progress);
                    }
                    else if (quest.MissionType == 3)
                    {
                        var progress = quest.Clone();
                        progress.Progress = damage;

                        quest.CurrentGoal += damage;
                        if (quest.CurrentGoal >= quest.QuestGoal)
                        {
                            completed.Add(quest);
                        }

                        progressive.Add(progress);
                    }
                    else if (quest.MissionType == 4)
                    {
                        var progress = quest.Clone();
                        progress.Progress = heals;
                        if (progress.CurrentGoal + progress.Progress > progress.QuestGoal)
                        {
                            progress.Progress = progress.QuestGoal - progress.CurrentGoal;
                        }

                        quest.CurrentGoal += heals;
                        if (quest.CurrentGoal >= quest.QuestGoal)
                        {
                            completed.Add(quest);
                        }

                        progressive.Add(progress);
                    }
                }
            }

            foreach (Quest quest in completed)
            {
                QuestList.Remove(quest);
                home.TokenReward += quest.Reward;
                home.BrawlPassTokens += quest.Reward;
            }

            return progressive;
        }

        public void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteVInt(QuestList.Count);
            foreach (Quest quest in QuestList.ToArray())
            {
                quest.Encode(encoder);
            }
        }
    }
}
