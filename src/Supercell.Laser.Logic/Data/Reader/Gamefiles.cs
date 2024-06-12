using System.Linq;
using Supercell.Laser.Logic.Data.Helper;

namespace Supercell.Laser.Logic.Data.Reader
{
    public class Gamefiles : IDisposable
    {
        private readonly Dictionary<int, DataTable> _dataTables = new Dictionary<int, DataTable>();

        public Gamefiles()
        {
            if (DataTables.Gamefiles.Count <= 0) return;

            for (var i = 0; i < DataTables.Gamefiles.Count; i++)
                _dataTables.Add((int)DataTables.Gamefiles.ElementAt(i).Key, new DataTable());
        }

        public void Dispose()
        {
            _dataTables.Clear();
        }

        public DataTable Get(DataType index)
        {
            return _dataTables[(int) index];
        }

        public DataTable Get(int index)
        {
            return _dataTables[index];
        }

        public bool ContainsTable(int t)
        {
            return _dataTables.ContainsKey(t);
        }

        public void Initialize(Table table, DataType index)
        {
            _dataTables[(int)index] = new DataTable(table, index);
        }
    }
}