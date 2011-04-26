using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jason
{
    public class Json : JsonObject
    {
        private string json_string;
        private bool lazy_parsing;

        public Json(string json_string)
        {
            this.json_string = json_string;
            this.lazy_parsing = true;
            this.Init();
        }

        public Json(string json_string, bool lazy_parsing)
        {
            this.json_string = json_string;
            this.lazy_parsing = lazy_parsing;
            this.Init();
        }

        private void Init()
        {
            Parser parser = Parser.Instance;
            parser.Parse(json_string);
        }
    }
}
