namespace Supercell.Laser.Logic.Stream.Entry
{
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Titan.DataStream;

    public class AllianceStreamEntry
    {
        public long Id;
        public long AuthorId;
        public string AuthorName;
        public string Message;
        public AllianceRole AuthorRole;

        public DateTime SendTime;

        public long PlayerId;
        public string PlayerName;

        public int Type;
        public int Event;

        public AllianceStreamEntry()
        {
            SendTime = DateTime.UtcNow;
        }

        public void Encode(ByteStream stream)
        {
            stream.WriteVInt(Type);
            stream.WriteVLong(Id);
            stream.WriteVLong(AuthorId);
            stream.WriteString(AuthorName);
            stream.WriteVInt((int)AuthorRole);
            stream.WriteVInt((int)(DateTime.UtcNow - SendTime).TotalSeconds);
            stream.WriteVInt(0);

            if (Type == 4)
            {
                stream.WriteVInt(Event);
                stream.WriteVInt(1);
                stream.WriteVLong(PlayerId);
                stream.WriteString(PlayerName);
            }
            else
            {
                stream.WriteString(Message);
            }
        }
    }
}
