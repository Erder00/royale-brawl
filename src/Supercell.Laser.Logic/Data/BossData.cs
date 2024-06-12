namespace Supercell.Laser.Logic.Data
{
    public class BossData : LogicData
    {
        public BossData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public string TID { get; set; }

        public int PlayerCount { get; set; }

        public int RequiredCampaignProgressToUnlock { get; set; }

        public string Location { get; set; }

        public string AllowedHeroes { get; set; }

        public string Reward { get; set; }

        public int LevelGenerationSeed { get; set; }

        public string Map { get; set; }

        public string Boss { get; set; }

        public int BossLevel { get; set; }
    }
}
