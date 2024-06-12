namespace Supercell.Laser.Logic.Data
{
    public class MapData : LogicData
    {
        public MapData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string CodeName { get; set; }

        public string Group { get; set; }

        public string Data { get; set; }

        public bool ConstructFromBlocks { get; set; }
    }
}
