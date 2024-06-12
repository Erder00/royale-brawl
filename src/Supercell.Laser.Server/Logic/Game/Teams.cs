namespace Supercell.Laser.Server.Logic.Game
{
    using Supercell.Laser.Logic.Message.Team;
    using Supercell.Laser.Logic.Team;
    using Supercell.Laser.Server.Networking;
    using Supercell.Laser.Server.Networking.Session;

    public static class Teams
    {
        private static Dictionary<long, TeamEntry> Entries;
        private static long TeamIdCounter;

        public static void Init()
        {
            Entries = new Dictionary<long, TeamEntry>();
            TeamIdCounter = 0;
        }

        public static int Count
        {
            get
            {
                return Entries.Count;
            }
        }

        public static TeamEntry Create()
        {
            TeamEntry entry = new TeamEntry();
            entry.Id = ++TeamIdCounter;
            Entries.Add(entry.Id, entry);
            return entry;
        }

        public static void Remove(long id)
        {
            Entries.Remove(id);
        }

        public static void StartGame(TeamEntry team)
        {
            try
            {
                foreach (TeamMember member in team.Members)
                {
                    Connection connection = Sessions.GetSession(member.AccountId).Connection;
                    connection.Send(new TeamGameStartingMessage()
                    {
                        LocationId = team.LocationId
                    });
                    Matchmaking.RequestMatchmake(connection, team.EventSlot, team.Id);
                    member.IsReady = false;
                }
            } catch (Exception)
            {
                ;
            }
        }

        public static TeamEntry Get(long id)
        {
            if (Entries.ContainsKey(id))
            {
                return Entries[id];
            }
            return null;
        }
    }
}
