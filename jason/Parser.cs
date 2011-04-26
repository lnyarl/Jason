using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Jason
{
    public class Parser
    {
        public static Parser instance = new Parser();
        public static Parser Instance { 
            get {
                return instance;
            } 
        }
        public Stack<JsonValue> stack = new Stack<JsonValue>();

        public JsonValue Parse(string json_string)
        {
            IEnumerable<JsonToken> enumer = this.getToken(json_string);
            JsonToken current = null;
            foreach (JsonToken lookahead in enumer)
            {
                if (current == null)
                {
                    current = lookahead;
                    continue;
                }
                ShiftReduce(current, lookahead);


                current = lookahead;
            }
            ShiftReduce(current, null);
            return stack.Pop();
        }

        bool Check()
        {
            return true;
        }

        void ShiftReduce(JsonToken current, JsonToken lookahead)
        {
            switch (current.token)
            {
                case JsonToken.Token.OPEN_OBJECT:
                    stack.Push(current);
                    break;
                case JsonToken.Token.OPEN_ARRAY:
                    stack.Push(current);
                    break;
                case JsonToken.Token.CLOSE_OBJECT:
                    JsonValue t = stack.Pop();
                    JsonObject jsonobject = new JsonObject();
                    while(t.Type != JsonToken.Token.OPEN_OBJECT){
                        JsonKeyValue tmp = t as JsonKeyValue;
                        jsonobject.Add(tmp.Key, tmp.Value);
                        t = stack.Pop();
                    }
                    if (lookahead != null &&
                        (lookahead.token == JsonToken.Token.COMMA ||
                        lookahead.token == JsonToken.Token.CLOSE_OBJECT))
                    {
                        makeKeyValue(jsonobject, lookahead);
                    }
                    else
                    {
                        stack.Push(jsonobject);
                    }
                    break;
                case JsonToken.Token.CLOSE_ARRAY:
                    JsonValue u = stack.Pop();
                    JsonArray jsonarray = new JsonArray();
                    while(u.Type != JsonToken.Token.OPEN_ARRAY){
                        jsonarray.Add(u);
                        u = stack.Pop();
                    }
                    jsonarray.Reverse();
                    if (lookahead != null &&
                        (lookahead.token == JsonToken.Token.COMMA ||
                        lookahead.token == JsonToken.Token.CLOSE_OBJECT))
                    {
                        makeKeyValue(jsonarray, lookahead);
                    }
                    else
                    {
                        stack.Push(jsonarray);
                    }
                    break;
                case JsonToken.Token.NULL:
                case JsonToken.Token.BOOLEAN:
                case JsonToken.Token.NUMBER:
                    if (lookahead != null &&
                        (lookahead.token == JsonToken.Token.COMMA ||
                        lookahead.token == JsonToken.Token.CLOSE_OBJECT))
                    {
                        makeKeyValue(current, lookahead);
                    }
                    else
                    {
                        stack.Push(current);
                    }
                    break;
                case JsonToken.Token.STRING:

                    current.data = current.data.Substring(1, current.data.Length - 2);
                    if (lookahead != null &&
                        (lookahead.token == JsonToken.Token.COMMA ||
                        lookahead.token == JsonToken.Token.CLOSE_OBJECT))
                    {
                        makeKeyValue(current, lookahead);
                    }
                    else
                    {
                        stack.Push(current);
                    }

                    break;
                case JsonToken.Token.SEPRATION_KEY_VALUE:
                    stack.Push(current);
                    break;
            }
        }

        private void makeKeyValue(JsonValue current, JsonToken lookahead)
        {
            JsonValue sep = (JsonValue)stack.Pop();
            //이전걸 꺼내서 봤는데 헐 key/value pair의 구분자네? 
            if (sep.Type == JsonToken.Token.SEPRATION_KEY_VALUE)
            {
                JsonKeyValue kv = new JsonKeyValue();
                kv.Key = ((JsonToken)stack.Pop()).data;
                kv.Value = current;
                stack.Push(kv);
            }
            else
            {
                stack.Push(sep);
                stack.Push(current);
            }
        }

        #region Tokenizer
        int line = 1;
        int index = 0;
        public IEnumerable<JsonToken> getToken(string json_string)
        {
            for (index = 0; index < json_string.Length; index++)
            {
                char current = json_string[index];
                switch (current)
                {
                    case ' ':
                    case '\t':
                        break;
                    case '\r':
                        break;
                    case '\n':
                        line++;
                        break;
                    case '{':
                        yield return new JsonToken(JsonToken.Token.OPEN_OBJECT);
                        break;
                    case '}':
                        yield return new JsonToken(JsonToken.Token.CLOSE_OBJECT);
                        break;
                    case '[':
                        yield return new JsonToken(JsonToken.Token.OPEN_ARRAY);
                        break;
                    case ']':
                        yield return new JsonToken(JsonToken.Token.CLOSE_ARRAY);
                        break;
                    case ',':
                        yield return new JsonToken(JsonToken.Token.COMMA);
                        break;
                    case ':':
                        yield return new JsonToken(JsonToken.Token.SEPRATION_KEY_VALUE);
                        break;
                    case '"':
                        yield return GetString(json_string);
                        break;
                    case 't':
                        if (json_string.Substring(index, 4).ToUpper() == "TRUE")
                        {
                            index += 3;
                            yield return new JsonToken(JsonToken.Token.BOOLEAN, "true");
                        }
                        else
                        {
                            yield return new JsonToken(JsonToken.Token.NONE);
                        }
                        break;
                    case 'f':
                        if (json_string.Substring(index, 5).ToUpper() == "FALSE")
                        {
                            index += 4;
                            yield return new JsonToken(JsonToken.Token.BOOLEAN, "false");
                        }
                        else
                        {
                            yield return new JsonToken(JsonToken.Token.NONE);
                        }
                        break;
                    case 'n':
                        if (json_string.Substring(index, 4).ToUpper() == "NULL")
                        {
                            index += 3;
                            yield return new JsonToken(JsonToken.Token.NULL);
                        }
                        else
                        {
                            yield return new JsonToken(JsonToken.Token.NONE);
                        }
                        break;
                    default:
                        //number
                        JsonToken number = GetNumber(json_string);
                        if (number == null)
                        {
                            yield return new JsonToken(JsonToken.Token.NONE, "unexpected character(" + line + ") : " + current);
                        }
                        else
                        {
                            yield return number;
                        }
                        break;
                }
            }
            yield break;
        }

        private JsonToken GetNumber(string json_string)
        {
            Regex ex = new Regex("-?(0|([1-9][0-9]*))(\\.[0-9]+)?((e|E)(\\+|-)?[0-9]+)?");

            Match b = ex.Match(json_string, index);
            index += b.Value.Length-1;
            return new JsonToken(JsonToken.Token.NUMBER, b.Value);
        }

        string hexa = "0123456789ABEDEF";
        public JsonToken GetString(string json_string)

        {
            int start = index;
            int length = 1;
            bool end = false;
            index++;
            for (; index < json_string.Length && !end; )
            {
                switch (json_string[index])
                {
                    case '\n':
                        index++;
                        return new JsonToken(JsonToken.Token.NONE, "string can not contains '\n' character (" + line + ")");
                    case '\\':
                        length++;
                        index++;
                        if ("\"\\/bfnrtu".Contains(json_string[index]))
                        {
                            length++;
                            index++;
                            if (json_string[index-1] == 'u')
                            {
                                string hexa_ = json_string.Substring(index + 1, 4);
                                foreach (char c in hexa_) {
                                    if (!this.hexa.Contains(c))
                                        return new JsonToken(JsonToken.Token.NONE, "this is not hexadecimal digit(" + line + ") : " + hexa_);
                                    index++;
                                    length++;
                                }
                            }
                        }
                        break; 
                    case '"':
                        length++;
                        end = true;
                        break;
                    default:
                        length++;
                        index++;
                        break;
                }
            }
            return new JsonToken(JsonToken.Token.STRING, json_string.Substring(start, length));
        }
        #endregion
    }
}