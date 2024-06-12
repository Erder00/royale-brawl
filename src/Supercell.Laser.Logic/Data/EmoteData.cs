namespace Supercell.Laser.Logic.Data
{
    public class EmoteData : LogicData
    {
        public EmoteData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public bool Disabled { get; set; }

        public string FileName { get; set; }

        public string ExportName { get; set; }

        public string Character { get; set; }

        public string Skin { get; set; }

        public int Type { get; set; }

        public int Rarity { get; set; }

        public bool LockedForChronos { get; set; }

        public int BundleCode { get; set; }

        public bool IsDefaultBattleEmote { get; set; }
    }
}
