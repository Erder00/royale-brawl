namespace Supercell.Laser.Logic.Data
{
    public class PinData : LogicData
    {
        public PinData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public int PinType { get; set; }

        public int Rarity { get; set; }

        public int Index { get; set; }

        public int Bonus { get; set; }

        public int CraftCost { get; set; }
    }
}
