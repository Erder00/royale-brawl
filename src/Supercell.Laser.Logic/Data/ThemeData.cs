namespace Supercell.Laser.Logic.Data
{
    public class ThemeData : LogicData
    {
        public ThemeData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public string FileName { get; set; }

        public string ExportName { get; set; }

        public string ParticleFileName { get; set; }

        public string ParticleExportName { get; set; }

        public string ParticleStyle { get; set; }

        public int ParticleVariations { get; set; }

        public string ThemeMusic { get; set; }

        public bool UseInLevelSelection { get; set; }
    }
}
