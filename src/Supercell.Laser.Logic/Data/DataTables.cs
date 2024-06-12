global using Supercell.Laser.Logic.Data.Helper;
global using Supercell.Laser.Logic.Data.Reader;

namespace Supercell.Laser.Logic.Data
{
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Data.Reader;
    using Supercell.Laser.Titan.Debug;
    
    public partial class DataTables
    {
        public static readonly Dictionary<DataType, string> Gamefiles = new Dictionary<DataType, string>();
        private static Gamefiles Tables;

        private static void AddFilesToLoad()
        {
            Gamefiles.Add(DataType.Projectile, "Assets/csv_logic/projectiles.csv");
            Gamefiles.Add(DataType.AllianceBadge, "Assets/csv_logic/alliance_badges.csv");
            Gamefiles.Add(DataType.Location, "Assets/csv_logic/locations.csv");
            Gamefiles.Add(DataType.Character, "Assets/csv_logic/characters.csv");
            Gamefiles.Add(DataType.AreaEffect, "Assets/csv_logic/area_effects.csv");
            Gamefiles.Add(DataType.Item, "Assets/csv_logic/items.csv");
            Gamefiles.Add(DataType.Skill, "Assets/csv_logic/skills.csv");
            Gamefiles.Add(DataType.Card, "Assets/csv_logic/cards.csv");
            Gamefiles.Add(DataType.Tile, "Assets/csv_logic/tiles.csv");
            Gamefiles.Add(DataType.PlayerThumbnail, "Assets/csv_logic/player_thumbnails.csv");
            Gamefiles.Add(DataType.Skin, "Assets/csv_logic/skins.csv");
            Gamefiles.Add(DataType.Milestone, "Assets/csv_logic/milestones.csv");
            Gamefiles.Add(DataType.SkinConf, "Assets/csv_logic/skin_confs.csv");
            Gamefiles.Add(DataType.Emote, "Assets/csv_logic/emotes.csv");
        }

        public static void Load()
        {
            DataTables.AddFilesToLoad();

            Tables = new Gamefiles();

            foreach (var file in Gamefiles)
                Tables.Initialize(new Table(file.Value), file.Key);

            Debugger.Print($"{Gamefiles.Count} Data Tables initialized!");
        }

        public static bool TableExists(int t)
        {
            return Tables.ContainsTable(t);
        }

        public static DataTable Get(DataType classId)
        {
            return Tables.Get(classId);
        }

        public static DataTable Get(int classId)
        {
            return Tables.Get(classId);
        }
    }
}