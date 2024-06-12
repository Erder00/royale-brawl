namespace Supercell.Laser.Logic.Listener
{
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Home;

    public interface LogicServerListener
    {
        public static LogicServerListener Instance;

        ClientAvatar GetAvatar(long id);
        LogicGameListener GetGameListener(long id);
        HomeMode GetHomeMode(long id);
        bool IsPlayerOnline(long id);
    }
}
