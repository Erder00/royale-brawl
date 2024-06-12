namespace Supercell.Laser.Logic.Data
{
    public class NameColorData : LogicData
    {
        public NameColorData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public string ColorCode { get; set; }

        public string Gradient { get; set; }

        public int RequiredExpLevel { get; set; }

        public int RequiredTotalTrophies { get; set; }

        public int RequiredSeasonPoints { get; set; }

        public string RequiredHero { get; set; }

        public int SortOrder { get; set; }
    }
}
