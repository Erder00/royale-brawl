namespace Supercell.Laser.Logic.Home
{
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Command;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Home.Gatcha;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Logic.Listener;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Titan.Math;

    public class HomeMode
    {
        public const int UNLOCKABLE_HEROES_COUNT = 38;

        public readonly LogicGameListener GameListener;

        public ClientHome Home;
        public ClientAvatar Avatar;

        public Action<int> CharacterChanged;

        public HomeMode(ClientHome home, ClientAvatar avatar, LogicGameListener gameListener)
        {
            Home = home;
            Avatar = avatar;

            Home.HomeMode = this;
            Avatar.HomeMode = this;

            GameListener = gameListener;
        }

        public static HomeMode LoadHomeState(LogicGameListener gameListener, ClientHome home, ClientAvatar avatar, EventData[] events)
        {
            home.Events = events;

            HomeMode homeMode = new HomeMode(home, avatar, gameListener);
            homeMode.Enter(DateTime.UtcNow);

            return homeMode;
        }

        private bool GetRandomBrawlerForGatcha(Random rand, DeliveryUnit unit)
        {
            int brawlersCount = UNLOCKABLE_HEROES_COUNT;
            int brawlerId = -1;

            bool done = false;
            int attempts = 0;
            while (!done && attempts < 25)
            {
                attempts++;
                brawlerId = GlobalId.CreateGlobalId(16, rand.Next(0, brawlersCount));

                CharacterData data = DataTables.Get(DataType.Character).GetDataByGlobalId<CharacterData>(brawlerId);
                done = !data.Disabled && !Avatar.HasHero(brawlerId);

                if (done)
                {
                    CardData card = DataTables.Get(DataType.Card).GetData<CardData>(data.Name + "_unlock");
                    done = card.Rarity != "common";
                    if (done)
                    {
                        done = card.Name != "Blower_unlock";
                        if (done)
                        {
                            if (card.Rarity == "epic")
                            {
                                if (Avatar.RollsSinceGoodDrop > 6)
                                {
                                    Avatar.RollsSinceGoodDrop = 0;
                                    done = true;
                                }
                                else
                                {
                                    done = Avatar.RollsSinceGoodDrop > rand.Next(1000);
                                    if (done) Avatar.RollsSinceGoodDrop = 0;
                                }
                            }
                            else if (card.Rarity == "mega_epic")
                            {
                                if (Avatar.RollsSinceGoodDrop > 25)
                                {
                                    Avatar.RollsSinceGoodDrop = 0;
                                    done = true;
                                }
                                else
                                {
                                    done = Avatar.RollsSinceGoodDrop > rand.Next(1000);
                                    if (done) Avatar.RollsSinceGoodDrop = 0;
                                }
                            }
                            else if (card.Rarity == "legendary")
                            {
                                if (Avatar.RollsSinceGoodDrop > 100)
                                {
                                    Avatar.RollsSinceGoodDrop = 0;
                                    done = true;
                                }
                                else
                                {
                                    done = Avatar.RollsSinceGoodDrop > rand.Next(1000);
                                    if (done) Avatar.RollsSinceGoodDrop = 0;
                                }
                            }
                        }
                    }
                }

                if (done)
                {
                    GatchaDrop drop = new GatchaDrop(1);
                    drop.DataGlobalId = brawlerId;
                    drop.Count = 1;
                    unit.AddDrop(drop);
                    break;
                }
            }

            return done;
        }

        public void SimulateGatcha(DeliveryUnit unit)
        {
            Avatar.RollsSinceGoodDrop++;
            Random rand = new Random();

            int unlockedBrawlersCount = Avatar.GetUnlockedHeroesCount();

            List<int> powerPoints = new List<int>();
            if (unit.Type == 10)
            {
                int brawlerChance = 10;
                if (unlockedBrawlersCount < 4)
                {
                    brawlerChance = 70;
                }
                else if (unlockedBrawlersCount < 6)
                {
                    brawlerChance = 40;
                }

                bool isBrawler = rand.Next(0, 100) < brawlerChance;
                if (isBrawler)
                {
                    isBrawler = GetRandomBrawlerForGatcha(rand, unit);
                }

                if (!isBrawler)
                {
                    GatchaDrop coins = new GatchaDrop(7);
                    coins.Count = rand.Next(15, 40);
                    unit.AddDrop(coins);

                    for (int i = 0; i < 2; i++)
                    {
                        List<Hero> unlockedHeroes = Avatar.Heroes;
                        bool heroValid = false;
                        int generateAttempts = 0;
                        int idx = -1;
                        while (!heroValid && generateAttempts < 10)
                        {
                            generateAttempts++;
                            idx = rand.Next(unlockedHeroes.Count);
                            heroValid = unlockedHeroes[idx].PowerPoints < 1410;
                            if (heroValid)
                            {
                                GatchaDrop drop = new GatchaDrop(6);
                                drop.DataGlobalId = unlockedHeroes[idx].CharacterId;
                                if (powerPoints.Contains(drop.DataGlobalId))
                                {
                                    continue;
                                }
                                powerPoints.Add(drop.DataGlobalId);
                                drop.Count = LogicMath.Min(rand.Next(5, 25), (1410 - unlockedHeroes[idx].PowerPoints));
                                unit.AddDrop(drop);
                            }
                        }
                    }
                }
            }
            else if (unit.Type == 11)
            {
                GatchaDrop coins = new GatchaDrop(7);
                coins.Count = rand.Next(322, 600);
                unit.AddDrop(coins);

                for (int i = 0; i < 5; i++)
                {
                    List<Hero> unlockedHeroes = Avatar.Heroes;
                    bool heroValid = false;
                    int generateAttempts = 0;
                    int idx = -1;
                    while (!heroValid && generateAttempts < 10)
                    {
                        generateAttempts++;
                        idx = rand.Next(unlockedHeroes.Count);
                        heroValid = unlockedHeroes[idx].PowerPoints < 1410;
                        if (heroValid)
                        {
                            GatchaDrop drop = new GatchaDrop(6);
                            drop.DataGlobalId = unlockedHeroes[idx].CharacterId;
                            if (powerPoints.Contains(drop.DataGlobalId))
                            {
                                continue;
                            }
                            powerPoints.Add(drop.DataGlobalId);
                            drop.Count = LogicMath.Min(rand.Next(40, 80), (1410 - unlockedHeroes[idx].PowerPoints));
                            unit.AddDrop(drop);
                        }
                    }
                }

                int brawlerChance = 20;
                if (unlockedBrawlersCount < 4)
                {
                    brawlerChance = 70;
                }
                else if (unlockedBrawlersCount < 6)
                {
                    brawlerChance = 40;
                }

                bool isBrawler = rand.Next(0, 100) < brawlerChance;
                if (isBrawler)
                {
                    isBrawler = GetRandomBrawlerForGatcha(rand, unit);
                }
            }
            else if (unit.Type == 12)
            {
                GatchaDrop coins = new GatchaDrop(7);
                coins.Count = rand.Next(60, 250);
                unit.AddDrop(coins);

                for (int i = 0; i < 3; i++)
                {
                    List<Hero> unlockedHeroes = Avatar.Heroes;
                    bool heroValid = false;
                    int generateAttempts = 0;
                    int idx = -1;
                    while (!heroValid && generateAttempts < 10)
                    {
                        generateAttempts++;
                        idx = rand.Next(unlockedHeroes.Count);
                        heroValid = unlockedHeroes[idx].PowerPoints < 1410;
                        if (heroValid)
                        {
                            GatchaDrop drop = new GatchaDrop(6);
                            drop.DataGlobalId = unlockedHeroes[idx].CharacterId;
                            if (powerPoints.Contains(drop.DataGlobalId))
                            {
                                continue;
                            }
                            powerPoints.Add(drop.DataGlobalId);
                            drop.Count = LogicMath.Min(rand.Next(10, 50), (1410 - unlockedHeroes[idx].PowerPoints));
                            unit.AddDrop(drop);
                        }
                    }
                }

                int brawlerChance = 20;
                if (unlockedBrawlersCount < 4)
                {
                    brawlerChance = 70;
                }
                else if (unlockedBrawlersCount < 6)
                {
                    brawlerChance = 40;
                }
                bool isBrawler = rand.Next(0, 100) < brawlerChance; // brawler = 25% chance.
                if (isBrawler)
                {
                    isBrawler = GetRandomBrawlerForGatcha(rand, unit);
                }
            }

            bool bonus = rand.Next(0, 100) < 50;
            if (bonus) // add gems bonus
            {
                int count = rand.Next(2, 7) + 1;
                GatchaDrop drop = new GatchaDrop(8);
                drop.Count = count;
                unit.AddDrop(drop);
            }
        }

        public void Enter(DateTime dateTime)
        {
            Home.HomeVisited();
        }

        public void ClientTurnReceived(int tick, int checksum, List<Command> commands)
        {
            foreach (Command command in commands)
            {
                if (command.Execute(this) != 0)
                {
                    OutOfSyncMessage outOfSync = new OutOfSyncMessage();
                    GameListener.SendMessage(outOfSync);
                }
            }
            Home.Tick();
        }
    }
}
