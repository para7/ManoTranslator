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
        static Dictionary<char, string> decode;

        static void Main(string[] args)
        {
            //http://www.atmarkit.co.jp/ait/articles/1704/19/news021.html

            var reader = new XmlSerializer(typeof(Serial[]));

            Serial[] data;

            var Filepath = "output.xml";

            using (var streamReader = new StreamReader(Filepath, Encoding.UTF8))
            {
                data = (Serial[])reader.Deserialize(streamReader);
            }

            foreach(var x in data)
            {
                Console.WriteLine("{0} {1}", x.key, x.value);
            }
        }
    }
}
