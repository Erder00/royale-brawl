namespace Supercell.Laser.Server.Networking.Session
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Listener;

    public class Session
    {
        public HomeMode Home;
        public Connection Connection;
        public LogicGameListener GameListener => Home.GameListener;

        public Session(HomeMode home, Connection connection)
        {
            Home = home;
            Connection = connection;
        }
    }
}
