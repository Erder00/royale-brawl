namespace Supercell.Laser.Server.Networking
{
    using Supercell.Laser.Server.Networking.Session;
    using System.Net;
    using System.Net.Sockets;

    public static class TCPGateway
    {
        private static List<Connection> ActiveConnections;

        private static Socket Socket;
        private static Thread Thread;

        private static ManualResetEvent AcceptEvent;

        public static void Init(string host, int port)
        {
            ActiveConnections = new List<Connection>();

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.Bind(new IPEndPoint(IPAddress.Parse(host), port));
            Socket.Listen(100);

            AcceptEvent = new ManualResetEvent(false);

            Thread = new Thread(TCPGateway.Update);
            Thread.Start();

            Logger.Print($"TCP Server started at {host}:{port}");
        }

        private static void Update()
        {
            while (true)
            {
                AcceptEvent.Reset();
                Socket.BeginAccept(new AsyncCallback(OnAccept), null);
                AcceptEvent.WaitOne();
            }
        }

        private static void OnAccept(IAsyncResult ar)
        {
            try
            {
                Socket client = Socket.EndAccept(ar);
                Connection connection = new Connection(client);
                ActiveConnections.Add(connection);
                Logger.Print("New connection!");
                Connections.AddConnection(connection);
                client.BeginReceive(connection.ReadBuffer, 0, 1024, SocketFlags.None, new AsyncCallback(OnReceive), connection);
            } 
            catch (Exception)
            {
                ;
            }

            AcceptEvent.Set();
        }

        private static void OnReceive(IAsyncResult ar)
        {
            Connection connection = (Connection)ar.AsyncState;
            if (connection == null) return;

            try
            {
                int r = connection.Socket.EndReceive(ar);
                if (r <= 0)
                {
                    Logger.Print("client disconnected.");
                    ActiveConnections.Remove(connection);
                    if (connection.MessageManager.HomeMode != null)
                    {
                        Sessions.Remove(connection.Avatar.AccountId);
                    }
                    connection.Close();
                    return;
                }

                connection.Memory.Write(connection.ReadBuffer, 0, r);
                if (connection.Messaging.OnReceive() != 0)
                {
                    ActiveConnections.Remove(connection);
                    if (connection.MessageManager.HomeMode != null)
                    {
                        Sessions.Remove(connection.Avatar.AccountId);
                    }
                    connection.Close();
                    Logger.Print("client disconnected.");
                    return;
                }
                connection.Socket.BeginReceive(connection.ReadBuffer, 0, 1024, SocketFlags.None, new AsyncCallback(OnReceive), connection);
            }
            catch (SocketException)
            {
                ActiveConnections.Remove(connection);
                if (connection.MessageManager.HomeMode != null)
                {
                    Sessions.Remove(connection.Avatar.AccountId);
                }
                connection.Close();
                Logger.Print("client disconnected.");
            }
            catch (Exception exception)
            {
                connection.Close();
                Logger.Print("Unhandled exception: " + exception + ", trace: " + exception.StackTrace);
            }
        }

        public static void OnSend(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                socket.EndSend(ar);
            }
            catch (Exception)
            {
                ;
            }
        }
    }
}
