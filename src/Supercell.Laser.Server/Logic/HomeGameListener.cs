namespace Supercell.Laser.Server.Logic
{
    using Supercell.Laser.Logic.Listener;
    using Supercell.Laser.Server.Networking;
    using Supercell.Laser.Logic.Message;

    public class HomeGameListener : LogicGameListener
    {
        private Connection Connection;

        public HomeGameListener(Connection connection)
        {
            Connection = connection;
        }

        public override void SendMessage(GameMessage message)
        {
            Connection.Send(message);
        }

        public override void SendTCPMessage(GameMessage message)
        {
            Connection.Send(message);
        }
    }
}
