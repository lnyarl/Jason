using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jason
{
    public class JsonToken : JsonValue
    {
        public enum Token
        {
            OPEN_OBJECT,            // '{'
            CLOSE_OBJECT,           // '}'
            OPEN_ARRAY,             // '['
            CLOSE_ARRAY,            // ']'
            COMMA,                  // ','
            SEPRATION_KEY_VALUE,    // ':'
            STRING,                 // "..."
            NUMBER,                 // int, double ...
            BOOLEAN,                // true, false
            NULL,                   // null
            NONE,                   // none

            KEY_VALUE_PAIR,         // key : value
            OBJECT,
            ARRAY,
        };

        public Token token;
        public string data;

        public JsonToken(Token token, string data)
        {
            this.token = token;
            this.data = data;
        }

        public JsonToken(Token token)
        {
            this.token = token;
            this.data = "";
        }

        public override string ToString()
        {
            if (this.Type == Token.NULL)
                return "null";
            return data.ToString();
        }

        public string ToJSON()
        {
            if (token == Token.STRING)
            {
                return "\"" + data.ToString() + "\"";
            }
            else
            {
                return data.ToString();
            }
        }

        public JsonToken.Token Type
        {
            get { return this.token; }
        }

        public JsonValue this[string index]
        {
            get { return this; }
        }
    }
}
