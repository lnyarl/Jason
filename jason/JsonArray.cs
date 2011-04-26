using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jason
{
    class JsonArray : IList<JsonValue>, JsonValue
    {
        List<JsonValue> data = new List<JsonValue>();


        public JsonToken.Token Type
        {
            get { return JsonToken.Token.ARRAY; }
        }
        public int IndexOf(JsonValue item)
        {
            return data.IndexOf(item);
        }

        public void Insert(int index, JsonValue item)
        {
            data.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            data.RemoveAt(index);
        }

        public JsonValue this[string index]
        {
            get
            {
                return this.data[int.Parse(index)];
            }
            set
            {
                this.data[int.Parse(index)] = value;
            }
        }

        public JsonValue this[int index]
        {
            get
            {
                return this.data[index];
            }
            set
            {
                this.data[index] = value;
            }
        }

        public void Add(JsonValue item)
        {
            this.data.Insert(0, item);
        }

        public void Clear()
        {
            this.data.Clear();
        }

        public bool Contains(JsonValue item)
        {
            return this.data.Contains(item);
        }

        public void CopyTo(JsonValue[] array, int arrayIndex)
        {
            this.data.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.data.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(JsonValue item)
        {
            return this.data.Remove(item);
        }

        public IEnumerator<JsonValue> GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        public override string ToString()
        {
            List<string> list = new List<string>();
            foreach (JsonValue v in this.data)
            {
                list.Add(v.ToString());
            }
            return "[" + string.Join(",", list) +"]";
        }

        public string ToJSON()
        {
            return ToString();
        }
    }
}
