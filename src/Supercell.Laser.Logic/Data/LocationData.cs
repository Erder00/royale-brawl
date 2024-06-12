namespace Supercell.Laser.Logic.Data
{
    public class LocationData : LogicData
    {
        public LocationData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public bool Disabled { get; set; }

        public string TID { get; set; }

        public string BgPrefix { get; set; }

        public string LocationTheme { get; set; }

        public string GroundSCW { get; set; }

        public string CampaignGroundSCW { get; set; }

        public string EnvironmentSCW { get; set; }

        public string IconSWF { get; set; }

        public string IconExportName { get; set; }

        public string GameMode { get; set; }

        public string AllowedMaps { get; set; }

        public string Music { get; set; }

        public string CommunityCredit { get; set; }
    }
}
