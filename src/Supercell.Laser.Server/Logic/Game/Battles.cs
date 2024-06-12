namespace Supercell.Laser.Server.Logic.Game
{
    using Supercell.Laser.Logic.Battle;
    using System.Collections.Concurrent;

    public static class Battles
    {
        private static long m_battleIdCounter;
        private static ConcurrentDictionary<long, BattleMode> m_battles;

        public static void Init()
        {
            m_battles = new ConcurrentDictionary<long, BattleMode>();
            m_battleIdCounter = 0;

            new Thread(Update).Start();
        }

        public static void Update()
        {
            while (true)
            {
                foreach (BattleMode battle in m_battles.Values.ToArray())
                {
                    if (battle.IsGameOver)
                    {
                        m_battles.Remove(battle.Id, out _);
                    }
                }
                Thread.Sleep(1000);
            }
        }

        public static long Add(BattleMode battle)
        {
            long id = ++m_battleIdCounter;
            m_battles[id] = battle;
            return id;
        }

        public static BattleMode Get(long id)
        {
            if (!m_battles.ContainsKey(id)) return null;
            return m_battles[id];
        }
    }
}
