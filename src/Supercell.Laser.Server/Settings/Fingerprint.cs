namespace Supercell.Laser.Server.Settings
{
    using Supercell.Laser.Titan.Json;

    public static class Fingerprint
    {
        public static string Sha { get; private set; }
        public static string Version { get; private set; }

        public static void Load()
        {
            string json = File.ReadAllText("Assets/fingerprint.json");

            LogicJSONObject jsonObject = LogicJSONParser.ParseObject(json);
            Sha = jsonObject.GetJSONString("sha").GetStringValue();
            Version = jsonObject.GetJSONString("version").GetStringValue();

            Logger.Print($"Loaded fingerprint.json v{Version}");
        }
    }
}
