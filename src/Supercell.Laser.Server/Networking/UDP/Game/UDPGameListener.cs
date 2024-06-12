namespace Supercell.Laser.Server.Networking.UDP.Game
{
    using Supercell.Laser.Logic.Listener;
    using Supercell.Laser.Logic.Message;

    public class UDPGameListener : LogicGameListener
    {
        private UDPSocket Socket;
        private Connection TCPConnection;

        public UDPGameListener(UDPSocket socket, Connection connection)
        {
            Socket = socket;
            TCPConnection = connection;
        }

        public override void SendMessage(GameMessage message)
        {
            Socket.SendMessage(message);
        }

        public override void SendTCPMessage(GameMessage message)
        {
            try
            {
                TCPConnection.Send(message);
            } catch (Exception ex) { }
        }
    }
}
