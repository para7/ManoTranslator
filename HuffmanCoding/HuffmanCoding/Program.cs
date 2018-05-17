using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HuffmanCoding
{
    //ツリーのノード
    public class Node
    {
        public Node parent;
        public Node Left;
        public Node Right;

        public ListData data;
    }

    //文字と確率のペア
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
        //文字通りインサートソートを行う
        static void InsertSort(LinkedList<Node> list, Node node)
        {
            for (var l = list.First; l != null; l = l.Next)
            {
                //リスト側の数値が大きくなった時点でその前に入れる
                if (node.data.prob < l.Value.data.prob)
                {
                    list.AddBefore(l, node);
                    return;
                }
            }
            //見つからなければ一番大きいので最後に入れる
            list.AddLast(node);
        }
        
        //符号生成でツリーを潜る時に履歴として使う
        static Stack<Node> sta;

        //符号化の辞書
        static Dictionary<char, string> manotree;
        //符号の重複をチェックする用
        static Dictionary<string, char> check;

        static void dfs()
        {
            string c = sta.First().data.c;

            //sta.Count > 1...根以外
            if (sta.Count > 1 && (c != "0" && c != "1"))
            {
                //履歴を文字列にする
                var hoge = new string(sta.Select(x => x.data.c.First()).ToArray());
            
                //Reverseは不要かもしれない
                var value = new string(hoge.Substring(1, hoge.Length - 2).Reverse().ToArray()).Replace("0", "ほわっ").Replace("1", "むんっ");

                //辞書に登録
                manotree.Add(c[0], value);
                check.Add(value, c[0]);
            }
            else
            {
                //左と右でそれぞれ潜る
                sta.Push(sta.First().Left);
                dfs();

                sta.Push(sta.First().Right);
                dfs();
            }

            sta.Pop();
        }

        //木を構築
        static Node MakeTree(Node parent, Node left, Node right)
        {
            //文字列は加算されて構成されているので、この条件式は終端ノード以外、を意味する
            if (left.data.c.Length > 1)
            {
                left.data.c = "0";
            }

            if (right.data.c.Length > 1)
            {
                right.data.c = "1";
            }

            //連結
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

            //カウント
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

                //親のノードを新規作成
                var parent = new Node();
                parent.data = new ListData();
                //親は文字列と確率を加算
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

            //ちゃんと出来てるか確認
            foreach (var m in manotree)
            {
                Console.WriteLine("{0} {1}", m.Key, m.Value);
            }

        }

    }
}
