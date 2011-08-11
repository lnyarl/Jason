using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jason;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
namespace Jason.test
{
    class Program
    {
        static void Main(string[] args)
        {
            //testNumber();
            testJson1();
            testJson2();
            testJson3();
            testStringError();
            //testString();
        }

        private static void testStringError()
        {
            string json = File.ReadAllText("./test-stringerror.json");
            Parser adf = new Parser();
            try
            {
                JsonValue r = adf.Parse(json);
            }
            catch (JsonException e)
            {
                Debug.Assert(e.ErrorCode == "E101");
            }
        }

        private static void testJson3()
        {
            string json = File.ReadAllText("./test3.json");
            Parser adf = new Parser();
            JsonValue r = adf.Parse(json);

            Debug.Assert(r["menu"]["items"]["0"]["id"].ToString() == "Open");
            Debug.Assert(r["menu"]["items"]["6"].ToString() == "null");
            Debug.Assert(r["menu"]["items"]["8"]["id"].ToString() == "Pause");
            Debug.Assert(r["menu"]["items"]["11"]["label"].ToString() == "Find...");
        }

        private static void testJson2()
        {
            string json = File.ReadAllText("./test2.json");
            Parser adf = new Parser();
            JsonValue r = adf.Parse(json);

            Debug.Assert(r["web-app"]["servlet"]["0"]["servlet-name"].ToString() == "cofaxCDS");
            Debug.Assert(r["web-app"]["servlet"]["0"]["init-param"]["useJSP"].ToString() == "false");
            Debug.Assert(r["web-app"]["servlet"]["2"]["servlet-class"].ToString() == "org.cofax.cds.AdminServlet");
            Debug.Assert(r["web-app"]["servlet-mapping"]["cofaxTools"].ToString() == "/tools/*");
        }

        private static void testJson1()
        {
            string json = File.ReadAllText("./test.json");
            Parser adf = new Parser();
            JsonValue r = adf.Parse(json);

            Debug.Assert(r["widget"]["tq"]["0"].ToString() == "a");
        }

        private static void testString()
        {
            //Json json = new Json("{\"sdf\" : \"dddd\" ");
            //Regex a = new Regex("-?0|([1-9][0-9]*)(.[0-9]+(e|E(+|-)?[0-9]+)?)?");
            string a = "\"jdbc:microsoft:sqlserver://LOCALHOST:1433;DatabaseName=goon\"";

            Parser adf = new Parser();
            JsonToken b = adf.GetString(a);
        }

        private static void testNumber()
        {
            //Json json = new Json("{\"sdf\" : \"dddd\" ");
            //Regex a = new Regex("-?0|([1-9][0-9]*)(.[0-9]+(e|E(+|-)?[0-9]+)?)?");
            Regex a = new Regex("-?(0|([1-9][0-9]*))(\\.[0-9]+)?((e|E)(\\+|-)?[0-9]+)?");

            Match b = a.Match("234", 0);
            Console.WriteLine(b.Value);

            b = a.Match("-0", 0);
            Console.WriteLine(b.Value);

            b = a.Match("0", 0);
            Console.WriteLine(b.Value);

            b = a.Match("-323", 0);
            Console.WriteLine(b.Value);

            b = a.Match("2", 0);
            Console.WriteLine(b.Value);

            b = a.Match("2.2", 0);
            Console.WriteLine(b.Value);

            b = a.Match("0.2", 0);
            Console.WriteLine(b.Value);

            b = a.Match("234e+34", 0);
            Console.WriteLine(b.Value);

            b = a.Match("234e34", 0);
            Console.WriteLine(b.Value);

            b = a.Match("234e-34", 0);
            Console.WriteLine(b.Value);

            b = a.Match("234.034E34", 0);
            Console.WriteLine(b.Value);
        }
    }
}
