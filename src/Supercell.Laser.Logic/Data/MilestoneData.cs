namespace Supercell.Laser.Logic.Data
{
    public class MilestoneData : LogicData
    {
        public MilestoneData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public int Type { get; set; }

        public int Index { get; set; }

        public int ProgressStart { get; set; }

        public int Progress { get; set; }

        public int League { get; set; }

        public int Tier { get; set; }

        public int Season { get; set; }

        public int SeasonEndRewardKeys { get; set; }

        public int PrimaryLvlUpRewardType { get; set; }

        public int PrimaryLvlUpRewardCount { get; set; }

        public int PrimaryLvlUpRewardExtraData { get; set; }

        public string PrimaryLvlUpRewardData { get; set; }

        public int SecondaryLvlUpRewardType { get; set; }

        public int SecondaryLvlUpRewardCount { get; set; }

        public int SecondaryLvlUpRewardExtraData { get; set; }

        public string SecondaryLvlUpRewardData { get; set; }
    }
}
