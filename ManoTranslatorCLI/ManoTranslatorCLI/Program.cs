using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ManoTranslatorCLI
{
    [Serializable]
    public class Serial
    {
        public char key;
        public string value;

        public Serial() { }
        public Serial(char k, string v)
        {
            key = k;
            value = v;
        }
    }

    static class Program
    {
        static Dictionary<char, string> encode;
        static Dictionary<string, char> decode;

        static void Main(string[] args)
        {
            //http://www.atmarkit.co.jp/ait/articles/1704/19/news021.html

            var reader = new XmlSerializer(typeof(Serial[]));

            Serial[] data;

            var Filepath = "output.xml";

            if (!System.IO.File.Exists(Filepath))
            {
                Console.WriteLine("output.xmlが見つかりません");
                return;
            }

            using (var streamReader = new StreamReader(Filepath, Encoding.UTF8))
            {
                data = (Serial[])reader.Deserialize(streamReader);
            }

            encode = new Dictionary<char, string>();
            decode = new Dictionary<string, char>();
            foreach (var x in data)
            {
                encode.Add(x.key, x.value);
                decode.Add(x.value, x.key);
            }

            string print = "";

            switch (args.Length)
            {
                //エンコード
                case 1:
                    print = Encode(args[0]);
                    break;

                //オプションを判断
                case 2:
                    if(args[0] == "-d" || args[0] == "-decode")
                    {
                        print = Decode(args[1]);
                    }
                    else
                    {
                        //ヘルプを表示

                        return;
                    }
                    break;

                //ヘルプを表示
                default:
                    break;
            }

            Console.Write(print);

        }

        static string Encode(string str)
        {
            string ret = "";

            foreach(var c in str)
            {
                ret += encode[c];
            }

            return ret;
        }

        static string Decode(string str)
        {
            string ret = "";

            string match = "";
            while(str.Length > 0)
            {
                var s = str.Substring(0, 3);
                
                str = str.Substring(3);

                match += s;

                if(decode.ContainsKey(match))
                {
                    ret += decode[match];
                    match = "";
                    continue;
                }
            }

            return ret;
        }
    }
}
