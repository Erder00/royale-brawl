namespace Supercell.Laser.Server.Database.Cache
{
    using Supercell.Laser.Logic.Club;
    using System.Diagnostics.CodeAnalysis;

    public static class AllianceCache
    {
        private static Dictionary<long, Alliance> CachedAlliances;
        private static Thread _thread;

        public static bool Started = true;

        public static int Count
        {
            get
            {
                return CachedAlliances.Count;
            }
        }

        public static void Init()
        {
            CachedAlliances = new Dictionary<long, Alliance>();
            _thread = new Thread(Update);
            _thread.Start();
        }

        private static void Update()
        {
            while (Started)
            {
                SaveAll();
                Thread.Sleep(1000 * 30);
            }
        }

        public static void SaveAll()
        {
            try
            {
                foreach (var alliance in CachedAlliances.Values)
                {
                    try
                    {
                        Alliances.Save(alliance);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Unhandled exception while saving account: " + ex.Message + " trace: " + ex.StackTrace);
                    }
                }
            }
            catch (Exception) { }
        }

        public static bool IsAllianceCached(long id)
        {
            return CachedAlliances.ContainsKey(id);
        }

        public static Alliance GetAlliance(long id)
        {
            if (CachedAlliances.ContainsKey(id))
            {
                return CachedAlliances[id];
            }
            return null;
        }

        public static void Cache(Alliance alliance)
        {
            if (!CachedAlliances.ContainsKey(alliance.Id))
            {
                CachedAlliances.Add(alliance.Id, alliance);
            }
            else
            {
                CachedAlliances[alliance.Id] = alliance;
            }
        }
    }
}
