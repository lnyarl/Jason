using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Jason
{
    public class JsonObject : JsonValue, IDictionary<string, JsonValue>
    {
        Dictionary<string, JsonValue> data = new Dictionary<string, JsonValue>();


        public void Add(string key, JsonValue value)
        {
            data.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return this.data.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return this.data.Keys; }
        }

        public bool Remove(string key)
        {
            return this.data.Remove(key);
        }

        public bool TryGetValue(string key, out JsonValue value)
        {
            return this.data.TryGetValue(key, out value);
        }

        public ICollection<JsonValue> Values
        {
            get { return this.data.Values; }
        }

        public JsonValue this[string key]
        {
            get
            {
                if (this.data.ContainsKey(key))
                {
                    return this.data[key];
                }
                else
                {
                    return new JsonToken(JsonToken.Token.STRING);
                }
            }
            set
            {
                if (this.data.ContainsKey(key))
                {
                    this.data.Add(key, value);
                }
                else
                {
                    this.data[key] = value;
                }
            }
        }

        public void Add(KeyValuePair<string, JsonValue> item)
        {
            this.data.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            this.data.Clear();
        }

        public bool Contains(KeyValuePair<string, JsonValue> item)
        {
            return this.data.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, JsonValue>[] array, int arrayIndex)
        {
        }

        public int Count
        {
            get { return this.data.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<string, JsonValue> item)
        {
            return this.data.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<string, JsonValue>> GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        public JsonToken.Token Type
        {
            get
            {
                return JsonToken.Token.OBJECT;
            }
        }

        public override string ToString()
        {
            List<string> result = new List<string>();
            foreach(KeyValuePair<string,JsonValue> v in this.data) {
                result.Add(v.Key + " : " + v.Value.ToString());
            }

            return "{ " + string.Join(",", result) + " }";
        }

        public string ToJSON()
        {
            List<string> result = new List<string>();
            foreach(KeyValuePair<string,JsonValue> v in this.data) {
                result.Add("\"" + v.Key + "\" : " + v.Value.ToString());
            }

            return "{ " + string.Join(",", result) + " }";
        }
    }
}
