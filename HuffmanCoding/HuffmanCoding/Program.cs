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
        public string c;
        public double prob;

        public ListData() { }

        public ListData(string _c, double _prob)
        {
            c = _c;
            prob = _prob;
        }
    }

    static class Program
    {
        static void InsertSort(LinkedList<Node> list, Node node)
        {
            for (var l = list.First; l != null; l = l.Next)
            {
                if (node.data.prob < l.Value.data.prob)
                {
                    list.AddBefore(l, node);
                    return;
                }
            }
            list.AddLast(node);
        }


        static Stack<Node> sta;

        static Dictionary<char, string> manotree;

        static void dfs()
        {
            string c = sta.First().data.c;

            if (sta.Count > 1 && (c != "0" && c != "1"))
            {

                var hoge = new string(sta.Select(x => x.data.c.First()).ToArray());
            
                var value = new string(hoge.Substring(1, hoge.Length - 2).Reverse().ToArray()).Replace("0", "ほわっ").Replace("1", "むんっ");

                manotree.Add(c[0], value);
            }
            else
            {
                sta.Push(sta.First().Left);
                dfs();

                sta.Push(sta.First().Right);
                dfs();
            }

            sta.Pop();
        }

        static Node MakeTree(Node parent, Node left, Node right)
        {
            if (left.data.c.Length > 1)
            {
                left.data.c = "0";
            }

            if (right.data.c.Length > 1)
            {
                right.data.c = "1";
            }

            right.parent = parent;
            left.parent = parent;

            parent.Right = right;
            parent.Left = left;

            return parent;
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

            var list = new LinkedList<Node>();

            //確率順にソート
            foreach (var d in dic)
            {
                var hoge = new Node();
                hoge.data = new ListData();
                hoge.data.prob = (double)d.Value / str.Length;
                hoge.data.c = d.Key.ToString();
                InsertSort(list, hoge);
            }

            dic = null;

            //確率の表示
            double p = 0;
            foreach (var l in list)
            {
                if (manu.Contains(l.data.c))
                {
                    Console.Write("- ");
                }

                Console.WriteLine("{0} {1}", l.data.c, l.data.prob);
                p += l.data.prob;
            }

            Console.WriteLine("prob_sum: {0}", p);
            Console.WriteLine(str.Length);

            Node root = new Node();



            //ツリーの構築
            while (list.Count != 1)
            {
                var left = list.First();
                list.RemoveFirst();

                var right = list.First();
                list.RemoveFirst();

                var parent = new Node();
                parent.data = new ListData();
                parent.data.c = left.data.c + right.data.c;
                parent.data.prob = left.data.prob + right.data.prob;

                InsertSort(list, parent);

                root = MakeTree(parent, left, right);
            }

            //ツリーを潜っていく
            sta = new Stack<Node>();

            sta.Push(root);

            manotree = new Dictionary<char, string>();

            root.data.c = " ";

            dfs();

            foreach (var m in manotree)
            {
                Console.WriteLine("{0} {1}", m.Key, m.Value);
            }

        }

    }
}
