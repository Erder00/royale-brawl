namespace Supercell.Laser.Logic.Avatar.Structures
{
    using Supercell.Laser.Titan.DataStream;

    public class PlayerDisplayData
    {
        public int ThumbnailId;
        public string Name;

        public PlayerDisplayData()
        {
            ;
        }

        public PlayerDisplayData(int thumbnail, string name)
        {
            ThumbnailId = thumbnail;
            Name = name;
        }

        public void Encode(ByteStream stream)
        {
            stream.WriteString(Name);
            stream.WriteVInt(100);
            stream.WriteVInt(ThumbnailId);
            stream.WriteVInt(43000000);
        }
    }
}
