namespace Supercell.Laser.Logic.Data
{
    public class AllianceBadgeData : LogicData
    {
        public AllianceBadgeData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public string IconSWF { get; set; }

        public string IconExportName { get; set; }

        public string Category { get; set; }
    }
}
