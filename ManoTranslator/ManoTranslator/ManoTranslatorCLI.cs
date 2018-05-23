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

    class Translator
    {
        static Dictionary<char, string> encode;
        static Dictionary<string, char> decode;

        public Translator()
        {
            //    //http://www.atmarkit.co.jp/ait/articles/1704/19/news021.html

            var reader = new XmlSerializer(typeof(Serial[]));

            Serial[] data;

            var Filepath = "manodictionary.xml";

            if (!System.IO.File.Exists(Filepath))
            {
                Console.WriteLine("manodictionary.xmlが見つかりません");
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
        }

        public string Encode(string str)
        {
            string ret = "";

            foreach (var c in str)
            {
                if (!encode.ContainsKey(c))
                {
                    ret += c;
                    continue;
                }

                ret += encode[c];
            }

            return ret;
        }

        public string Decode(string str)
        {
            string ret = "";

            const string howa = "ほわっ";
            const string mun = "むんっ";
            int seek = 0;

            string match = "";

            while (seek < str.Length)
            {
                var c = str[seek];

                if (c != howa.First() && c != mun.First())
                {
                    ret += match;
                    ret += c;
                    seek++;
                    match = "";
                    continue;
                }

                if (str.Length - seek < 3)
                {
                    ret += str.Substring(seek);
                    break;
                }

                var s = str.Substring(seek, 3);

                if (s != howa && s != mun)
                {
                    ret += match;
                    ret += c;
                    seek++;
                    match = "";
                    continue;
                }

                match += s;
                seek += 3;

                if (decode.ContainsKey(match))
                {
                    ret += decode[match];
                    match = "";
                    continue;
                }
            }
            ret += match;

            return ret;
        }

        static void OutputHelp()
        {
            Console.WriteLine("引数として与えられた文字列を「ほわっ」と「むんっ」に変換します。");

            Console.WriteLine("");
            Console.WriteLine("引数１つの操作");
            Console.WriteLine("");
            Console.WriteLine("args[0] : 日本語の文字列");
            Console.WriteLine("日本語を「ほわっ」と「むんっ」に変換します");
            Console.WriteLine("");
            Console.WriteLine("例：> ManoTranslator.exe まのちゃんかわいい");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("引数２つの操作");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("args[0] : -e | --encode");
            Console.WriteLine("args[1] : 日本語の文字列");
            Console.WriteLine("「ほわっ」と「むんっ」に変換します");
            Console.WriteLine("");
            Console.WriteLine("例：> ManoTranslator.exe -e まのちゃんかわいい");

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("args[0] : -d | --decode");
            Console.WriteLine("args[1] : 「ほわっ」と「むんっ」に変換された文字列");
            Console.WriteLine("日本語に戻します\n");
            Console.WriteLine("");
            Console.WriteLine("例：> ManoTranslator.exe -d ほわっほわっ...");
            Console.WriteLine("");
        }
    }
}
