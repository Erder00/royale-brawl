namespace Supercell.Laser.Logic.Data
{
    public class GlobalData : LogicData
    {
        public GlobalData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public int NumberValue { get; set; }

        public bool BooleanValue { get; set; }

        public string TextValue { get; set; }

        public string StringArray { get; set; }

        public int NumberArray { get; set; }
    }
}
