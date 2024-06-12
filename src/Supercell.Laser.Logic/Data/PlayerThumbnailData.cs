namespace Supercell.Laser.Logic.Data
{
    public class PlayerThumbnailData : LogicData
    {
        public PlayerThumbnailData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public int RequiredExpLevel { get; set; }

        public int RequiredTotalTrophies { get; set; }

        public int RequiredSeasonPoints { get; set; }

        public string RequiredHero { get; set; }

        public string IconSWF { get; set; }

        public string IconExportName { get; set; }

        public int SortOrder { get; set; }
    }
}
