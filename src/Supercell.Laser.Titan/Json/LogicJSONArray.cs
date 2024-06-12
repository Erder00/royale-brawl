namespace Supercell.Laser.Titan.Json
{
    using System.Text;

    public class LogicJSONArray : LogicJSONNode
    {
        private readonly List<LogicJSONNode> m_items;

        public LogicJSONArray()
        {
            this.m_items = new List<LogicJSONNode>(20);
        }

        public LogicJSONArray(int capacity)
        {
            this.m_items = new List<LogicJSONNode>(capacity);
        }

        public LogicJSONNode Get(int idx)
        {
            return this.m_items[idx];
        }

        public void Add(LogicJSONNode item)
        {
            this.m_items.Add(item);
        }

        public LogicJSONArray GetJSONArray(int index)
        {
            LogicJSONNode node = this.m_items[index];

            if (node.GetJSONNodeType() != LogicJSONNodeType.ARRAY)
            {
                return null;
            }

            return (LogicJSONArray) node;
        }

        public LogicJSONBoolean GetJSONBoolean(int index)
        {
            LogicJSONNode node = this.m_items[index];

            if (node.GetJSONNodeType() != LogicJSONNodeType.BOOLEAN)
            {
                return null;
            }

            return (LogicJSONBoolean) node;
        }

        public LogicJSONNumber GetJSONNumber(int index)
        {
            LogicJSONNode node = this.m_items[index];

            if (node.GetJSONNodeType() != LogicJSONNodeType.NUMBER)
            {
                return null;
            }

            return (LogicJSONNumber) node;
        }

        public LogicJSONObject GetJSONObject(int index)
        {
            LogicJSONNode node = this.m_items[index];

            if (node.GetJSONNodeType() != LogicJSONNodeType.OBJECT)
            {
                return null;
            }

            return (LogicJSONObject) node;
        }

        public LogicJSONString GetJSONString(int index)
        {
            LogicJSONNode node = this.m_items[index];

            if (node.GetJSONNodeType() != LogicJSONNodeType.STRING)
            {
                return null;
            }

            return (LogicJSONString) node;
        }

        public int Size()
        {
            return this.m_items.Count;
        }

        public override LogicJSONNodeType GetJSONNodeType()
        {
            return LogicJSONNodeType.ARRAY;
        }

        public override void WriteToString(StringBuilder builder)
        {
            builder.Append('[');

            for (int i = 0; i < this.m_items.Count; i++)
            {
                if (i > 0)
                {
                    builder.Append(',');
                }

                this.m_items[i].WriteToString(builder);
            }

            builder.Append(']');
        }
    }
}