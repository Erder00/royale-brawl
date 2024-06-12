namespace Supercell.Laser.Server.Networking
{
    using Supercell.Laser.Server.Networking.Session;

    public static class Connections
    {
        public static int Count => ActiveConnections.Count;

        private static List<Connection> ActiveConnections;
        private static Thread Thread;

        public static void Init()
        {
            ActiveConnections = new List<Connection>();
            Thread = new Thread(Update);
            Thread.Start();
        }

        private static void Update()
        {
            while (true)
            {
                foreach (Connection connection in ActiveConnections.ToArray())
                {
                    if (!connection.MessageManager.IsAlive())
                    {
                        if (connection.MessageManager.HomeMode != null)
                        {
                            Sessions.Remove(connection.Avatar.AccountId);
                        }
                        connection.Close();
                        ActiveConnections.Remove(connection);
                    }
                }
                Thread.Sleep(1000);
            }
        }

        public static void AddConnection(Connection connection)
        {
            ActiveConnections.Add(connection);
        }
    }
}
