using Supercell.Laser.Logic.Data.Reader;

namespace Supercell.Laser.Logic.Data.Helper
{
    public class DataTable
    {
        public List<LogicData> Datas;
        public DataType Index;

        public DataTable()
        {
            Datas = new List<LogicData>();
        }

        public DataTable(Table table, DataType index)
        {
            Index = index;
            Datas = new List<LogicData>();

            for (var i = 0; i < table.GetRowCount(); i += 2)
            {
                var row = table.GetRowAt(i);
                var data = DataTables.Create(Index, row, this);
                Datas.Add(data);
            }
        }

        public int Count
        {
            get
            {
                return Datas?.Count ?? 0;
            }
        }

        public List<LogicData> GetDatas()
        {
            return Datas;
        }

        public LogicData GetDataWithId(int id)
        {
            return Datas[GlobalId.GetInstanceId(id)];
        }

        public T GetDataWithId<T>(int id) where T : LogicData
        {
            return Datas[GlobalId.GetInstanceId(id)] as T;
        }

        public T GetData<T>(string name) where T : LogicData
        {
            return Datas.Find(data => data.GetName() == name) as T;
        }

        public T GetData<T>(int id) where T : LogicData
        {
            if (Datas.Count < id) return null;

            return Datas[id] as T;
        }

        public T GetDataByGlobalId<T>(int id) where T : LogicData
        {
            id = GlobalId.GetInstanceId(id);
            if (Datas.Count < id) return null;

            return Datas[id] as T;
        }

        public int GetInstanceId(string name)
        {
            return Datas.Find(data => data.GetName() == name).GetInstanceId();
        }

        public int GetIndex()
        {
            return (int) Index;
        }
    }
}