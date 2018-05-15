using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HuffmanCoding
{
    public class Node
    {
        public Node parent;
        public Node Left;
        public Node Right;

        public char c;
        public double prob;
    }

    public class ListData
    {
        public char c;
        public double prob;
    }

    static class Program
    {
        static void Main(string[] args)
        {
            StreamReader reader
                = new StreamReader(
       "data.txt");
            
            string str = new string(reader.ReadToEnd().Where(a => a != '゛').ToArray());

            reader.Close();

            var dic = new SortedList<char, int>();

            foreach(char c in str)
            {
                if (dic.ContainsKey(c))
                {
                    dic[c]++;
                }
                else
                {
                    dic.Add(c, 1);
                }
            }
            
            var list = new LinkedList<ListData>();

            foreach(var a in dic)
            {
                Console.WriteLine("{0} {1}",a.Key.ToString(), a.Value);
            }

            Console.WriteLine(dic.Count);
            Console.Write(str.Length);
        }
    }
}
