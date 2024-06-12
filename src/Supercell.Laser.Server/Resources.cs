namespace Supercell.Laser.Server
{
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Listener;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Logic;
    using Supercell.Laser.Server.Logic.Game;
    using Supercell.Laser.Server.Message;
    using Supercell.Laser.Server.Networking;
    using Supercell.Laser.Server.Networking.Session;
    using Supercell.Laser.Server.Settings;

    internal static class Resources
    {
        /// <summary>
        /// Initializes the databases
        /// </summary>
        public static void InitDatabase()
        {
            Accounts.Init(Configuration.Instance.DatabaseUsername, Configuration.Instance.DatabasePassword);
            Alliances.Init(Configuration.Instance.DatabaseUsername, Configuration.Instance.DatabasePassword);
        }

        /// <summary>
        /// Initializes the logic part of server
        /// </summary>
        public static void InitLogic()
        {
            Fingerprint.Load();
            DataTables.Load();
            Events.Init();
            Sessions.Init();
            Leaderboards.Init();
            Battles.Init();
            Matchmaking.Init();
            Teams.Init();
        }

        /// <summary>
        /// Initializes the network part of server
        /// </summary>
        public static void InitNetwork()
        {
            Processor.Init();
            Connections.Init();
            LogicServerListener.Instance = new ServerListener();
            UDPGateway.Init("0.0.0.0", Configuration.Instance.UdpPort);
            TCPGateway.Init("0.0.0.0", 9339);
        }
    }
}
