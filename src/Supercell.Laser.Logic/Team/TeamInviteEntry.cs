namespace Supercell.Laser.Logic.Team
{
    using Supercell.Laser.Titan.DataStream;

    public class TeamInviteEntry
    {
        public long InviterId { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public int Slot { get; set; }

        public void Encode(ByteStream stream)
        {
            stream.WriteLong(InviterId);
            stream.WriteLong(Id);

            stream.WriteString(Name);

            stream.WriteVInt(1);
            stream.WriteVInt(Slot);
        }
    }
}
