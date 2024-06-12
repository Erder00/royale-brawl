using System;
using Newtonsoft.Json;
using Supercell.Laser.Logic.Data.Reader;

namespace Supercell.Laser.Logic.Data.Helper
{
    public class LogicData
    {
        private int _dataType;
        private int _id;
        protected DataTable DataTable;
        protected Row Row;

        public LogicData(Row row, DataTable dataTable)
        {
            Row = row;
            DataTable = dataTable;
        }

        public void LoadData(LogicData data, Type type, Row row, int dataType = -1)
        {
            _dataType = (int)DataTables.Types[this.GetType()];
            _id = GlobalId.CreateGlobalId(_dataType, DataTable.Count);
            Row = row;
            Row.LoadData(data);
        }

        public int GetDataType()
        {
            return _dataType;
        }

        public int GetGlobalId()
        {
            return _id;
        }

        public int GetInstanceId()
        {
            return GlobalId.GetInstanceId(_id);
        }

        public int GetClassId()
        {
            return GlobalId.GetClassId(_id);
        }

        public string GetName()
        {
            return Row.GetName();
        }
    }
}