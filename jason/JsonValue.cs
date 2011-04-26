using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jason
{
    public interface JsonValue
    {
        JsonToken.Token Type{get;}
        JsonValue this[string index] { get; }
        string ToString();
        string ToJSON();
    }
}
