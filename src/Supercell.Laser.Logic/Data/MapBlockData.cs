namespace Supercell.Laser.Logic.Data
{
    public class MapBlockData : LogicData
    {
        public MapBlockData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string CodeName { get; set; }

        public string Group { get; set; }

        public string Data { get; set; }
    }
}
