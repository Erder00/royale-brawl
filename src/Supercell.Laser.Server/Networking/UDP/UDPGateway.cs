namespace Supercell.Laser.Server.Networking
{
    using Supercell.Laser.Server.Networking.UDP.Game;
    using Supercell.Laser.Titan.DataStream;
    using System.Net;
    using System.Net.Sockets;

    public static class UDPGateway
    {
        private static Socket Socket;
        private static Thread Thread;

        private static long SessionCounter;
        private static Dictionary<long, UDPSocket> Sockets;

        private static ManualResetEvent ReceiveEvent;

        public static void Init(string host, int port)
        {
            Sockets = new Dictionary<long, UDPSocket>();

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Socket.Bind(new IPEndPoint(IPAddress.Parse(host), port));

            ReceiveEvent = new ManualResetEvent(false);

            Thread = new Thread(Update);
            Thread.Start();

            Logger.Print($"UDP Server started at {host}:{port}");
        }

        private static void ReceiveCallback(IAsyncResult result)
        {
            ReceiveEvent.Set();
            State state = (State)result.AsyncState;

            byte[] buffer = state.Buffer;
            EndPoint remoteEndPoint = state.RemoteEndPoint;
            int r = 0;
            try
            {
                r = Socket.EndReceiveFrom(result, ref remoteEndPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION: " + e.GetType().Name);
                Console.WriteLine(e.Message);
            }
            

            if (r <= 0) return;
            

            try
            {
                ByteStream stream = new ByteStream(buffer, r);
                long sessionId = stream.ReadLong();
                stream.ReadShort();

                if (!Sockets.ContainsKey(sessionId)) return;

                UDPSocket client = Sockets[sessionId];
                if (!client.IsConnected)
                {
                    client.SetEndPoint(remoteEndPoint);

                    if (client.Battle == null) return;

                    if (!client.IsSpectator)
                        client.Battle.GetPlayerBySessionId(client.SessionId).GameListener = new UDPGameListener(client, client.TCPConnection);
                }

                client.ProcessReceive(stream);
            }
            catch (Exception e) 
            {
                Logger.Error($"EXCEPTION: {e.GetType().Name} - {e.Message}\n{e.StackTrace}");
            }
        }

        private class State
        {
            public byte[] Buffer;
            public EndPoint RemoteEndPoint;
        }

        private static void Update()
        {
            while (true)
            {
                try
                {
                    ReceiveEvent.Reset();
                    EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

                    byte[] buffer = new byte[1500];

                    State state = new State()
                    {
                        RemoteEndPoint = remoteEndPoint,
                        Buffer = buffer
                    };

                    Socket.BeginReceiveFrom(buffer, 0, 1500, SocketFlags.None, ref remoteEndPoint, new AsyncCallback(ReceiveCallback), state);
                    ReceiveEvent.WaitOne();
                }
                catch (Exception) { }
            }
        }

        public static UDPSocket CreateSocket()
        {
            UDPSocket socket = new UDPSocket(++SessionCounter);
            Sockets.Add(socket.SessionId, socket);
            return socket;
        }

        public static void SendTo(byte[] data, int startIndex, int count, EndPoint remoteEndPoint)
        {
            try
            {
                if (remoteEndPoint == null) return;
                Socket.SendTo(data, startIndex, count, SocketFlags.None, remoteEndPoint);
            }
            catch (SocketException) { }
        }
    }
}
