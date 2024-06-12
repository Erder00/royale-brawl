namespace Supercell.Laser.Titan.Json
{
    using System.Text;

    public class LogicJSONObject : LogicJSONNode
    {
        private List<string> m_keys;
        private List<LogicJSONNode> m_items;

        public LogicJSONObject()
        {
            this.m_keys = new List<string>(20);
            this.m_items = new List<LogicJSONNode>(20);
        }

        public LogicJSONObject(int capacity)
        {
            this.m_keys = new List<string>(capacity);
            this.m_items = new List<LogicJSONNode>(capacity);
        }

        public void Destruct()
        {
            if (this.m_keys != null)
            {
                this.m_keys = null;
            }

            if (this.m_items != null)
            {
                this.m_items = null;
            }
        }

        public LogicJSONNode Get(string key)
        {
            int itemIndex = this.m_keys.IndexOf(key);

            if (itemIndex == -1)
            {
                return null;
            }

            return this.m_items[itemIndex];
        }

        public LogicJSONArray GetJSONArray(string key)
        {
            int itemIndex = this.m_keys.IndexOf(key);

            if (itemIndex == -1)
            {
                return null;
            }

            LogicJSONNode node = this.m_items[itemIndex];

            if (node.GetJSONNodeType() == LogicJSONNodeType.ARRAY)
            {
                return (LogicJSONArray) node;
            }

            //Debugger.Warning(string.Format("LogicJSONObject::getJSONArray type is {0}, key {1}", node.GetJSONNodeType(), key));

            return null;
        }

        public LogicJSONBoolean GetJSONBoolean(string key)
        {
            int itemIndex = this.m_keys.IndexOf(key);

            if (itemIndex == -1)
            {
                return null;
            }

            LogicJSONNode node = this.m_items[itemIndex];

            if (node.GetJSONNodeType() == LogicJSONNodeType.BOOLEAN)
            {
                return (LogicJSONBoolean) node;
            }

            //Debugger.Warning(string.Format("LogicJSONObject::getJSONBoolean type is {0}, key {1}", node.GetJSONNodeType(), key));

            return null;
        }

        public LogicJSONNumber GetJSONNumber(string key)
        {
            int itemIndex = this.m_keys.IndexOf(key);

            if (itemIndex == -1)
            {
                return null;
            }

            LogicJSONNode node = this.m_items[itemIndex];

            if (node.GetJSONNodeType() == LogicJSONNodeType.NUMBER)
            {
                return (LogicJSONNumber) node;
            }

            //Debugger.Warning(string.Format("LogicJSONObject::getJSONNumber type is {0}, key {1}", node.GetJSONNodeType(), key));

            return null;
        }

        public LogicJSONObject GetJSONObject(string key)
        {
            int itemIndex = this.m_keys.IndexOf(key);

            if (itemIndex == -1)
            {
                return null;
            }

            LogicJSONNode node = this.m_items[itemIndex];

            if (node.GetJSONNodeType() == LogicJSONNodeType.OBJECT)
            {
                return (LogicJSONObject) node;
            }

            //Debugger.Warning(string.Format("LogicJSONObject::getJSONObject type is {0}, key {1}", node.GetJSONNodeType(), key));

            return null;
        }

        public LogicJSONString GetJSONString(string key)
        {
            int itemIndex = this.m_keys.IndexOf(key);

            if (itemIndex == -1)
            {
                return null;
            }

            LogicJSONNode node = this.m_items[itemIndex];

            if (node.GetJSONNodeType() == LogicJSONNodeType.STRING)
            {
                return (LogicJSONString) node;
            }

            //Debugger.Warning(string.Format("LogicJSONObject::getJSONString type is {0}, key {1}", node.GetJSONNodeType(), key));

            return null;
        }

        public void Put(string key, LogicJSONNode item)
        {
            int keyIndex = this.m_keys.IndexOf(key);

            if (keyIndex != -1)
            {
                //Debugger.Error(string.Format("LogicJSONObject::put already contains key {0}", key));
            }
            else
            {
                int itemIndex = this.m_items.IndexOf(item);

                if (itemIndex != -1)
                {
                    //Debugger.Error(string.Format("LogicJSONObject::put already contains the given JSONNode pointer. Key {0}", key));
                }
                else
                {
                    this.m_items.Add(item);
                    this.m_keys.Add(key);
                }
            }
        }

        public void Remove(string key)
        {
            int keyIndex = this.m_keys.IndexOf(key);

            if (keyIndex != -1)
            {
                this.m_keys.RemoveAt(keyIndex);
                this.m_items.RemoveAt(keyIndex);
            }
        }

        public int GetObjectCount()
        {
            return this.m_items.Count;
        }

        public override LogicJSONNodeType GetJSONNodeType()
        {
            return LogicJSONNodeType.OBJECT;
        }

        public override void WriteToString(StringBuilder builder)
        {
            builder.Append('{');

            for (int i = 0; i < this.m_items.Count; i++)
            {
                if (i > 0)
                {
                    builder.Append(',');
                }

                LogicJSONParser.WriteString(this.m_keys[i], builder);
                builder.Append(':');
                this.m_items[i].WriteToString(builder);
            }

            builder.Append('}');
        }
    }
}