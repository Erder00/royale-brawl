namespace Supercell.Laser.Server.Handler
{
    using Supercell.Laser.Server.Database.Cache;
    using Supercell.Laser.Server.Networking.Session;

    internal static class ExitHandler
    {
        public static void Exit(object sender, ConsoleCancelEventArgs e)
        {
            Sessions.StartShutdown();

            Logger.Print("Shutting down...");
            AccountCache.SaveAll();
            AllianceCache.SaveAll();

            AccountCache.Started = false;
            AllianceCache.Started = false;

            Console.WriteLine("Server is now in maintenance mode, press any key to shutdown");
            Console.ReadLine();

            Environment.Exit(0);
        }

        public static void Init()
        {
            Console.CancelKeyPress += ExitHandler.Exit;
        }
    }
}
