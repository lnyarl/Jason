using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Jason
{
    class JsonKeyValue :  JsonValue
    {
        public string Key
        {
            get;
            set;
        }

        public JsonValue Value
        {
            get;
            set;
        }

        public JsonValue this[string index]
        {
            get
            {
                return this.Value;
            }
        }

        public JsonToken.Token Type
        {
            get { return JsonToken.Token.KEY_VALUE_PAIR; }
        }

        public override string ToString()
        {
            return Key + " : " + Value;
        }

        public string ToJSON()
        {
            return ToString();
        }
    }
}
