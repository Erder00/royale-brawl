namespace Supercell.Laser.Server.Logic
{
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Listener;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Networking.Session;

    public class ServerListener : LogicServerListener
    {
        public ClientAvatar GetAvatar(long id)
        {
            return Accounts.Load(id).Avatar;
        }

        public LogicGameListener GetGameListener(long id)
        {
            if (Sessions.IsSessionActive(id))
            {
                return Sessions.GetSession(id).GameListener;
            }
            return null;
        }

        public HomeMode GetHomeMode(long id)
        {
            if (Sessions.IsSessionActive(id))
            {
                return Sessions.GetSession(id).Home;
            }
            return null;
        }

        public bool IsPlayerOnline(long id)
        {
            return Sessions.IsSessionActive(id);
        }
    }
}
