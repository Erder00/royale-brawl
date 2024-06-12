namespace Supercell.Laser.Logic.Data
{
    public class MessageData : LogicData
    {
        public MessageData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public string TID { get; set; }

        public string BubbleOverrideTID { get; set; }

        public bool Disabled { get; set; }

        public int MessageType { get; set; }

        public string FileName { get; set; }

        public string ExportName { get; set; }

        public int QuickEmojiType { get; set; }

        public int SortPriority { get; set; }

        public bool AgeGated { get; set; }
    }
}
