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

        public ListData() { }

        public ListData(char _c, double _prob)
        {
            c = _c;
            prob = _prob;
        }
    }

    static class Program
    {
        static void InsertSort(LinkedList<ListData> list, ListData data)
        {
            //if(list.Count == 0)
            //{
            //    list.AddFirst(data);
            //    return;
            //}
            
            for (var l = list.First; l != null; l = l.Next)
            {
                if (data.prob < l.Value.prob)
                {
                    list.AddBefore(l, data);
                    return;
                }
            }
            list.AddLast(data);
        }

        static void Main(string[] args)
        {
            StreamReader reader = new StreamReader("data.txt");

            string str = new string(reader.ReadToEnd().Where(a => a != '゛').ToArray());

            reader.Close();

            //出現確率の操作
            string manu = "まのちゃんかわい";

            foreach(var c in manu)
            {
                str += new string(c, 4000);
            }

            var dic = new SortedList<char, int>();

            foreach (char c in str)
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

            foreach (var d in dic)
            {
                var hoge = new ListData();
                hoge.prob = (double)d.Value / str.Length;
                hoge.c = d.Key;
                InsertSort(list, hoge);
            }

            double p = 0;
            foreach (var l in list)
            {
                if(manu.Contains(l.c))
                {
                    Console.Write("- ");
                }

                Console.WriteLine("{0} {1}", l.c, l.prob);
                p += l.prob;
            }

            Console.WriteLine(p);
            Console.Write(str.Length);
        }
    }
}
