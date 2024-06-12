namespace Supercell.Laser.Logic.Data
{
    public class SkinRarityData : LogicData
    {
        public SkinRarityData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public int Price { get; set; }

        public int Rarity { get; set; }
    }
}
