namespace Supercell.Laser.Server.Logic.Game
{
    using Supercell.Laser.Logic.Battle;
    using Supercell.Laser.Logic.Battle.Structures;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Logic.Listener;
    using Supercell.Laser.Logic.Message.Battle;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Logic.Team;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Server.Networking;
    using Supercell.Laser.Server.Networking.Session;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Matchmaking
    {
        private static Dictionary<int, MatchmakingSlot> Slots;
        private static Thread UpdateThread;

        public static void Init()
        {
            Slots = new Dictionary<int, MatchmakingSlot>();

            foreach (EventData e in Events.GetEvents())
            {
                int mode = GameModeUtil.GetGameModeVariation(e.Location.GameMode);
                Slots.Add(e.Slot, new MatchmakingSlot(e, GamePlayUtil.GetPlayerCountWithGameModeVariation(mode))); // players count
            }

            UpdateThread = new Thread(Update);
            UpdateThread.Start();
        }

        private static void Update()
        {
            while (true)
            {
                foreach (MatchmakingSlot slot in Slots.Values)
                {
                    slot.Update();
                }
                Thread.Sleep(250);
            }
        }

        public static void RequestMatchmake(Connection connection, int slot, long team = -1)
        {
            if (!Slots.ContainsKey(slot)) return;

            connection.MatchmakeSlot = slot;
            MatchmakingEntry entry = new MatchmakingEntry(connection);
            entry.PlayerTeamId = team;
            connection.MatchmakingEntry = entry;
            Slots[slot].Add(entry);
        }

        public static void CancelMatchmake(Connection connection)
        {
            int slot = connection.MatchmakeSlot;
            if (Slots.ContainsKey(slot))
            {
                connection.MatchmakeSlot = -1;
                Slots[slot].Remove(connection.MatchmakingEntry);

                if (connection.MatchmakingEntry.PlayerTeamId > 0)
                {
                    TeamEntry team = Teams.Get(connection.MatchmakingEntry.PlayerTeamId);

                    foreach (TeamMember member in team.Members)
                    {
                        member.IsReady = false;
                        Session session = Sessions.GetSession(member.AccountId);
                        if (session != null)
                        {
                            session.Connection.MatchmakingEntry.PlayerTeamId = -1;
                            CancelMatchmake(session.Connection);
                            MatchMakingCancelledMessage cancelled = new MatchMakingCancelledMessage();
                            session.Connection.Send(cancelled);
                        }
                    }

                    team.TeamUpdated();
                }
            } 
        }
    }

    public class MatchmakingSlot
    {
        private Queue<MatchmakingEntry> RequestQueue;
        private Queue<MatchmakingEntry> RemoveQueue;

        private List<MatchmakingEntry> Queue;

        private int PlayersRequired;
        private EventData EventData;

        private int SecondsLeft;
        private int Turns;

        public const int SEARCH_TIMEOUT = 3;

        public MatchmakingSlot(EventData eventData, int playersRequired)
        {
            PlayersRequired = playersRequired;
            EventData = eventData;
            Queue = new List<MatchmakingEntry>();

            RequestQueue = new Queue<MatchmakingEntry>();
            RemoveQueue = new Queue<MatchmakingEntry>();

            SecondsLeft = SEARCH_TIMEOUT;
        }

        public void Update()
        {
            try
            {
                if (Sessions.Maintenance) return;

                foreach (MatchmakingEntry entry in Queue)
                {
                    if (!entry.Connection.IsOpen) Remove(entry);
                }

                while (RemoveQueue.Count > 0)
                {
                    Queue.Remove(RemoveQueue.Dequeue());
                }

                while (RequestQueue.Count > 0)
                {
                    MatchmakingEntry entry = RequestQueue.Dequeue();
                    MatchmakingEntry check = Queue.Find(x => x.Connection == entry.Connection);
                    if (check != null) continue;

                    Queue.Add(entry);
                }

                while (Queue.Count >= PlayersRequired)
                {
                    StartGame(Queue.Take(PlayersRequired).ToList());
                    Queue.RemoveRange(0, PlayersRequired);
                }

                if (Queue.Count > 0 && Queue.Count < PlayersRequired && SecondsLeft <= 0)
                {
                    SecondsLeft = SEARCH_TIMEOUT;
                    StartGame(Queue.Take(Queue.Count).ToList());
                    Queue.RemoveRange(0, Queue.Count);
                }

                if (Queue.Count > 0)
                {
                    Turns++;
                    if (Turns >= 4)
                    {
                        Turns = 0;
                        SecondsLeft--;
                    }
                }
                else
                {
                    Turns = 0;
                    SecondsLeft = SEARCH_TIMEOUT;
                }

                if (Queue.Count > 0)
                {
                    MatchMakingStatusMessage message = new MatchMakingStatusMessage();

                    message.Seconds = SecondsLeft;
                    message.Found = Queue.Count;
                    message.Max = PlayersRequired;
                    message.ShowTips = true;

                    foreach (MatchmakingEntry entry in Queue)
                    {
                        entry.Connection.Messaging.Send(message);
                    }
                }
            } catch (Exception e)
            {
                Console.WriteLine("GameMatchmakingManager exception!");
                Console.WriteLine($"{e.GetType().Name}, {e.Message}\n{e.StackTrace}");
            }
        }

        public void Remove(MatchmakingEntry entry)
        {
            RemoveQueue.Enqueue(entry);
        }

        public void Add(MatchmakingEntry entry)
        {
            RequestQueue.Enqueue(entry);
        }

        int[] botBrawlers = new int[] { 0, 1, 2, 3, 9, 18 }; // Allowed

        public void StartGame(List<MatchmakingEntry> entries)
        {
            // The battle inits here
            // TODO: Merge it to BattleManager or smth..

            BattleMode battle = new BattleMode(EventData.LocationId);
            battle.Id = Battles.Add(battle);

            Random rand = new Random();

            List<MatchmakingEntry> sortedEntries = new List<MatchmakingEntry>();
            Dictionary<long, List<MatchmakingEntry>> teamEntries = new Dictionary<long, List<MatchmakingEntry>>();

            if (GameModeUtil.HasTwoTeams(battle.GetGameModeVariation()))
            {
                MatchmakingEntry[] entriesWithTeam = entries.FindAll(entry => entry.PlayerTeamId > 0).ToArray();
                for (int i = 0; i < entriesWithTeam.Length; i++)
                {
                    MatchmakingEntry entry = entriesWithTeam[i];
                    long teamId = entry.PlayerTeamId;
                    if (!teamEntries.ContainsKey(teamId)) teamEntries.Add(teamId, new List<MatchmakingEntry>());
                    teamEntries[teamId].Add(entry);
                    entries.Remove(entry);
                }

                long[] ids = teamEntries.Keys.ToArray();
                for (int i = 0; i < ids.Length; i++)
                {
                    List<MatchmakingEntry> teamedEntries = teamEntries[ids[i]];
                    for (int j = 0; j < teamedEntries.Count; j++)
                    {
                        sortedEntries.Add(teamedEntries[j]);
                        teamedEntries[j].PrefferedTeam = i;
                    }
                }

                for (int i = 0; i < entries.Count; i++)
                {
                    sortedEntries.Add(entries[i]);
                }

                for (int i = 0; i < sortedEntries.Count; i++)
                {
                    UDPSocket socket = UDPGateway.CreateSocket();
                    socket.TCPConnection = sortedEntries[i].Connection;
                    socket.Battle = battle;
                    sortedEntries[i].Connection.UdpSessionId = socket.SessionId;

                    int teamIndex = i % 2;
                    if (sortedEntries[i].PrefferedTeam != -1)
                    {
                        teamIndex = sortedEntries[i].PrefferedTeam;
                    }

                    if (battle.GetTeamPlayersCount(teamIndex) >= 3)
                    {
                        if (teamIndex == 1) teamIndex = 0;
                        else teamIndex = 1;
                    }
                    BattlePlayer player = BattlePlayer.Create(sortedEntries[i].Connection.Home, sortedEntries[i].Connection.Avatar, i, teamIndex);
                    player.TeamId = sortedEntries[i].PlayerTeamId;
                    sortedEntries[i].Player = player;
                    battle.AddPlayer(player, sortedEntries[i].Connection.UdpSessionId);
                }

                List<int> team1BotsCharacters = new List<int>();
                List<int> team2BotsCharacters = new List<int>();

                for (int i = sortedEntries.Count; i < battle.GetPlayersCountWithGameModeVariation(); i++)
                {
                    bool isBotValid = false;

                    int botCharacter = -1;
                    while (!isBotValid)
                    {
                        botCharacter = 16000000 + botBrawlers[rand.Next(0, botBrawlers.Length)];
                        if (i % 2 == 0)
                        {
                            isBotValid = !team1BotsCharacters.Contains(botCharacter);
                        }
                        else
                        {
                            isBotValid = !team2BotsCharacters.Contains(botCharacter);
                        }
                        if (!isBotValid) isBotValid = !GameModeUtil.HasTwoTeams(battle.GetGameModeVariation());
                    }

                    int teamIndex = i % 2;
                    if (battle.GetTeamPlayersCount(teamIndex) >= 3)
                    {
                        if (teamIndex == 1) teamIndex = 0;
                        else teamIndex = 1;
                    }

                    if (teamIndex == 0)
                    {
                        team1BotsCharacters.Add(botCharacter);
                    }
                    else
                    {
                        team2BotsCharacters.Add(botCharacter);
                    }
                    CharacterData data = DataTables.Get(16).GetDataByGlobalId<CharacterData>(botCharacter);
                    BattlePlayer bot = BattlePlayer.CreateBotInfo(data.ItemName.ToUpper(), i, teamIndex, botCharacter);
                    battle.AddPlayer(bot, -1);
                }
            }
            else
            {
                sortedEntries = entries;
                for (int i = 0; i < entries.Count; i++)
                {
                    UDPSocket socket = UDPGateway.CreateSocket();
                    socket.TCPConnection = sortedEntries[i].Connection;
                    socket.Battle = battle;
                    sortedEntries[i].Connection.UdpSessionId = socket.SessionId;

                    int teamIndex = i;
                    BattlePlayer player = BattlePlayer.Create(sortedEntries[i].Connection.Home, sortedEntries[i].Connection.Avatar, i, teamIndex);
                    player.TeamId = sortedEntries[i].PlayerTeamId;
                    sortedEntries[i].Player = player;
                    battle.AddPlayer(player, sortedEntries[i].Connection.UdpSessionId);
                }

                for (int i = entries.Count; i < battle.GetPlayersCountWithGameModeVariation(); i++)
                {
                    int botCharacter = 16000000 + botBrawlers[rand.Next(0, botBrawlers.Length)];

                    int teamIndex = i;

                    CharacterData data = DataTables.Get(16).GetDataByGlobalId<CharacterData>(botCharacter);
                    BattlePlayer bot = BattlePlayer.CreateBotInfo(data.ItemName.ToUpper(), i, teamIndex, botCharacter);
                    battle.AddPlayer(bot, -1);
                }
            }

            battle.AddGameObjects();

            for (int i = 0; i < sortedEntries.Count; i++)
            {
                StartLoadingMessage startLoading = new StartLoadingMessage();
                startLoading.LocationId = battle.Location.GetGlobalId();
                startLoading.TeamIndex = sortedEntries[i].Player.TeamIndex;
                startLoading.OwnIndex = sortedEntries[i].Player.PlayerIndex;
                startLoading.GameMode = battle.GetGameModeVariation() == 6 ? 6 : 1;

                sortedEntries[i].Connection.Avatar.UdpSessionId = sortedEntries[i].Connection.UdpSessionId;
                startLoading.Players.AddRange(battle.GetPlayers());
                sortedEntries[i].Connection.Send(startLoading);
            }

            battle.Start();
        }
    }

    public class MatchmakingEntry
    {
        public readonly Connection Connection;
        public BattlePlayer Player;
        public long PlayerTeamId;

        public int PrefferedTeam;

        public MatchmakingEntry(Connection connection)
        {
            PrefferedTeam = -1;
            this.Connection = connection;
        }
    }
}
