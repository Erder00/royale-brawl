namespace Supercell.Laser.Logic.Helper
{
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Titan.DataStream;

    public static class ByteStreamHelper
    {
        public static void WriteDataReference(ChecksumEncoder stream, LogicData data)
        {
            if (data == null)
            {
                stream.WriteVInt(0);
                return;
            }

            stream.WriteVInt(data.GetClassId());
            stream.WriteVInt(data.GetInstanceId());
        }

        public static void WriteDataReference(ChecksumEncoder stream, int globalId)
        {
            if (globalId <= 0)
            {
                stream.WriteVInt(0);
                return;
            }

            stream.WriteVInt(GlobalId.GetClassId(globalId));
            stream.WriteVInt(GlobalId.GetInstanceId(globalId));
        }

        public static void WriteDataReference(BitStream bitStream, int globalId)
        {
            bitStream.WritePositiveInt(GlobalId.GetClassId(globalId), 5); // 0x244f08
            bitStream.WritePositiveInt(GlobalId.GetInstanceId(globalId), 8); // 0x244f1c
        }

        public static int ReadDataReference(ByteStream stream)
        {
            int classId = stream.ReadVInt();
            if (classId <= 0) return 0;
            int instanceId = stream.ReadVInt();

            return GlobalId.CreateGlobalId(classId, instanceId);
        }

        public static void WriteIntList(ChecksumEncoder stream, IEnumerable<int> list)
        {
            int[] array = list.ToArray();

            stream.WriteVInt(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                stream.WriteVInt(array[i]);
            }
        }
    }
}
