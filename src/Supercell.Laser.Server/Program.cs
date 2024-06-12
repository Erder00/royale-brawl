namespace Supercell.Laser.Server
{
    using Supercell.Laser.Server.Handler;
    using Supercell.Laser.Server.Settings;
    using System.Drawing;

    static class Program
    {
        public const string SERVER_VERSION = "1.2";
        public const string BUILD_TYPE = "Beta";

        private static void Main(string[] args)
        {
            Console.Title = "RoyaleBrawl - #1 Open Source Brawl Stars Server Emulator";
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);

            Colorful.Console.WriteWithGradient(
                @"
    ____                    __     ____                      __
   / __ \____  __  ______ _/ /__  / __ )_________ __      __/ /
  / /_/ / __ \/ / / / __ `/ / _ \/ __  / ___/ __ `/ | /| / / / 
 / _, _/ /_/ / /_/ / /_/ / /  __/ /_/ / /  / /_/ /| |/ |/ / /  
/_/ |_|\____/\__, /\__,_/_/\___/_____/_/   \__,_/ |__/|__/_/   
            /____/                                             " + "\n\n\n", Color.Fuchsia, Color.Cyan, 8);

            Logger.Print("RoyaleBrawl Server Emulator is now starting...");

            Logger.Init();
            Configuration.Instance = Configuration.LoadFromFile("config.json");

            Resources.InitDatabase();
            Resources.InitLogic();
            Resources.InitNetwork();

            Logger.Print("Server started! Let's play Brawl Stars!");

            ExitHandler.Init();
            CmdHandler.Start();
        }
    }
}