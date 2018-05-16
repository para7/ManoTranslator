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

        public ListData data;
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

            foreach (var c in manu)
            {
                str += new string(c, 1200);
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

            //確率順にソート
            foreach (var d in dic)
            {
                var hoge = new ListData();
                hoge.prob = (double)d.Value / str.Length;
                hoge.c = d.Key;
                InsertSort(list, hoge);
            }

            dic = null;

            /*
            double p = 0;
            foreach (var l in list)
            {
                if (manu.Contains(l.c))
                {
                    Console.Write("- ");
                }

                Console.WriteLine("{0} {1}", l.c, l.prob);
                p += l.prob;
            }

            Console.WriteLine(p);
            Console.Write(str.Length);
            */

            Node root = new Node();

            //ツリーの構築
            while (list.Count != 1)
            {
                var left = list.First();
                list.RemoveFirst();

                var right = list.First();
                list.RemoveFirst();

                var parent = new ListData('n', left.prob + right.prob);

                InsertSort(list, parent);

                root = MakeTree(parent, left, right);
            }

            //ツリーを潜っていく
            var sta = new Stack<Node>();


        }

        static Node MakeTree(ListData parent, ListData left, ListData right)
        {
            var p = new Node();
            p.data = parent;

            var l = new Node();
            l.data = left;
            if(l.data.c == 'n')
            {
                l.data.c = '0';
            }
            
            var r = new Node();
            r.data = right;
            if (r.data.c == 'n')
            {
                r.data.c = '1';
            }
            
            r.parent = l.parent = p;
            p.Right = r;
            p.Left = l;
        
            return p;
        }
    }
}
