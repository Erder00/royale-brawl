namespace Supercell.Laser.Server.Networking
{
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Battle;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Message;
    using Supercell.Laser.Server.Logic.Game;
    using Supercell.Laser.Server.Message;
    using System;
    using System.Net.Sockets;

    public class Connection
    {
        public Messaging Messaging { get; }
        public MessageManager MessageManager { get; }
        public byte[] ReadBuffer { get; }
        public Socket Socket { get; }

        public int Ping { get; private set; }

        public MemoryStream Memory { get; set; }
        public bool IsOpen;

        public int MatchmakeSlot;
        public MatchmakingEntry MatchmakingEntry;

        public long UdpSessionId;

        public ClientHome Home
        {
            get
            {
                if (MessageManager.HomeMode != null)
                {
                    return MessageManager.HomeMode.Home;
                }
                return null;
            }
        }

        public ClientAvatar Avatar
        {
            get
            {
                if (MessageManager.HomeMode != null)
                {
                    return MessageManager.HomeMode.Avatar;
                }
                return null;
            }
        }

        public Connection(Socket socket)
        {
            Socket = socket;
            ReadBuffer = new byte[1024];

            Memory = new MemoryStream();

            Messaging = new Messaging(this);
            MessageManager = new MessageManager(this);

            IsOpen = true;
            MatchmakeSlot = -1;

            UdpSessionId = -1;
        }

        public void PingUpdated(int value)
        {
            Ping = value;
        }

        public void Send(GameMessage message)
        {
            Messaging.Send(message);
        }

        public void Close()
        {
            try
            {
                IsOpen = false;
                Socket.Close();
            }
            catch (Exception) { }
        }

        public void Write(byte[] stream)
        {
            try
            {
                Socket.BeginSend(stream, 0, stream.Length, SocketFlags.None, new AsyncCallback(TCPGateway.OnSend), Socket);
            }
            catch (Exception) { }
        }
    }
}
