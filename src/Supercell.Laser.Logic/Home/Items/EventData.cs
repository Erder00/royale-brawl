namespace Supercell.Laser.Logic.Home.Items
{
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Titan.DataStream;

    public class EventData
    {
        public int Slot;
        public int LocationId;
        public DateTime EndTime;
        public LocationData Location => DataTables.Get(DataType.Location).GetDataByGlobalId<LocationData>(LocationId);

        public void Encode(ByteStream encoder)
        {
            encoder.WriteVInt(0);
            encoder.WriteVInt(Slot);
            encoder.WriteVInt(0);
            encoder.WriteVInt((int)(EndTime - DateTime.Now).TotalSeconds);
            encoder.WriteVInt(10);

            ByteStreamHelper.WriteDataReference(encoder, Location);

            encoder.WriteVInt(2); // 0xacec7c
            encoder.WriteString(null); // 0xacecac
            encoder.WriteVInt(0); // 0xacecc0
            encoder.WriteVInt(0); // 0xacecd4
            encoder.WriteVInt(0); // 0xacece8

            encoder.WriteVInt(0); // 0xacecfc

            encoder.WriteVInt(0); // 0xacee58
            encoder.WriteVInt(0); // 0xacee6c
        }
    }
}
