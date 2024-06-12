namespace Supercell.Laser.Logic.Data
{
    public class RegionData : LogicData
    {
        public RegionData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public string TID { get; set; }

        public string DisplayName { get; set; }

        public bool IsCountry { get; set; }
    }
}
