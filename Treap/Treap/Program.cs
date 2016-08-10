using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treap
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">Cheie</typeparam>
    public class Treap<TKey> : IEnumerable<TKey> where TKey : IComparable
    {
        #region private stuff
        private Node root;

        private class Node
        {
            private static Random seed = new Random(14);
            public TKey Key;
            public Node st, dr;
            public int priority;

            public Node(TKey key)
            {
                priority = seed.Next(1000);
                Key = key;
            }

            public override string ToString()
            {
                return $"(key:{Key}   pri:{priority})";
            }
        }

        private void Dfs(int depth, Node x)
        {
            if (x == null) return;
            Dfs(depth + 1, x.st);
            for (int i = 1; i <= depth; i++) Console.Write("    ");
            Console.WriteLine(x.ToString());
            Dfs(depth + 1, x.dr);
        }

        private Node Rot_left(Node x)
        {
            var aux = x.dr;
            x.dr = aux.st;
            aux.st = x;
            return aux;
        }

        private Node Rot_right(Node x)
        {
            var aux = x.st;
            x.st = aux.dr;
            aux.dr = x;
            return aux;
        }

        private Node Balance(Node x)
        {
            if (x.st != null && x.st.priority > x.priority) return Rot_right(x);
            if (x.dr != null && x.dr.priority > x.priority) return Rot_left(x);
            return x;
        }

        private Node Insert(Node root, Node node)
        {
            if (root == null) return node;
            if (node.Key.CompareTo(root.Key) < 0) root.st = Insert(root.st, node);
            else root.dr = Insert(root.dr, node);
            return Balance(root);
        }
        #endregion

        public void Add(TKey value)
        {
            root = Insert(root, new Node(value));
        }

        public void Print()
        {
            Dfs(0, root);
        }

        public IEnumerable<TKey> GetEnumerable()
        {
            var stack = new Stack<Node>();
            Node selected = root;
            while (true)
            {
                while (selected != null)
                {
                    stack.Push(selected);
                    selected = selected.st;
                }
                if(!stack.Any()) yield break;
                selected = stack.Pop();
                yield return selected.Key;
                selected = selected.dr;
            }
        }

        public IEnumerator<TKey> GetEnumerator()
        {
            return GetEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerable().GetEnumerator();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var r = new Random();
            var t = new Treap<int>();
            var q = new SortedDictionary<int,int>();

            for (var i = 1; i <= 10; i++)
            {
                t.Add(r.Next(100));
                t.Print();
                Console.WriteLine();
            }

            foreach (var x in t)
            {
                Console.WriteLine(x);
            }

            Console.ReadLine();
        }
    }
}
