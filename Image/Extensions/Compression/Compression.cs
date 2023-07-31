using System.Collections.Generic;
using System.Linq;
using System;
namespace TahsinsLibrary.Compression
{
    public class HuffmanNode<T>
    {
        public T data;
        public int count;
        public HuffmanNode<T> parent, left, right;
        public HuffmanNode(T data, int count)
        {
            this.data = data;
            this.count = count;
        }
        public HuffmanNode<T> UniteWith(HuffmanNode<T> node)
        {
            HuffmanNode<T> n = new HuffmanNode<T>(default, node.count + count);
            if (count > node.count)
            {
                n.left = this;
                n.right = node;
            }
            else
            {
                n.left = node;
                n.right = this;
            }
            node.parent = parent = n;
            return n;
        }
        public T GetData(params bool[] road)
        {
            HuffmanNode<T> active = this;
            foreach (bool b in road)
            {
                if (b && active.left != null)
                {
                    active = active.left;
                }
                else if (!b && active.right != null)
                {
                    active = active.right;
                }
                else break;
            }
            return active.data;
        }
        public T GetData(List<bool> road) => GetData(road.ToArray());
        public static void Traverse(HuffmanNode<T> node)
        {
            if (node == null) return;
            Console.WriteLine(node.data + node.count.ToString());
            Traverse(node.right);
            Traverse(node.left);
        }
        public Dictionary<T, bool[]> GetTable()
        {
            Dictionary<T, bool[]> result = new Dictionary<T, bool[]>();
            Stack<HuffmanNode<T>> temp = new Stack<HuffmanNode<T>>();
            HuffmanNode<T> active = this;
            List<bool> roadMap = new List<bool>();
            while (active != null || temp.Count > 0)
            {
                while (active != null)
                {
                    temp.Push(active);
                    active = active.left;
                    roadMap.Add(true);
                }
                active = temp.Pop();
                if (!EqualityComparer<T>.Default.Equals(active.data, (T)default))
                {
                    result.TryAdd(active.data, roadMap.ToArray()[1..^0]);
                }
                roadMap.RemoveAt(roadMap.Count - 1);
                active = active.right;
                roadMap.Add(false);
            }
            return result;
        }
    }
    public static class Compression
    {
        public static T[] Huffman<T>(ICollection<T> source)
        {
            Dictionary<T, int> data = new Dictionary<T, int>();
            foreach (T t in source)
            {
                if (data.ContainsKey(t)) data[t]++;
                else data.Add(t, 1);
            }
            data.OrderBy(key => key.Value);
            T[] result = data.Keys.ToArray();
            return result;
        }
        public static T[] Huffman<T>(ICollection<T> source, out Dictionary<T, bool[]> way)
        {
            Dictionary<T, int> data = new Dictionary<T, int>();
            foreach (T t in source)
            {
                if (data.ContainsKey(t)) data[t]++;
                else data.Add(t, 1);
            }
            data.OrderBy(key => key.Value);
            T[] result = data.Keys.ToArray();
            way = new Dictionary<T, bool[]>();
            return result;
        }
        public static (int, int, T)[] LZ77<T>(ICollection<T> source)
        {
            List<T> temp = new List<T>(source), match = new List<T>(), window = new List<T>();
            List<(int, int, T)> result = new List<(int, int, T)>();
            int d = 0;
            int l = 0;
            T t = default;
            while (temp.Count > 0)
            {
                if (window.Count > 0)
                {
                    l = match.Count;
                    t = match[0];
                }
                else
                {
                    d = 0;
                    l = 0;
                    t = temp[0];
                }
                result.Add((d, l, t));

            }
            return null;
        }
        public static T[] LongestCommonSubsequence<T>(ICollection<T> source)
        {
            int[,] temp = new int[source.Count + 1, source.Count + 1];
            T[] ts = new T[source.Count];
            source.CopyTo(ts, 0);
            for (int i = 1; i <= temp.GetLength(0); i++)
            {
                for (int j = 1; j <= temp.GetLength(1); j++)
                {
                    if (ts[i - 1].Equals(ts[j - 1])) temp[i, j] = temp[i - 1, j - 1] + 1;
                    else temp[i, j] = (int)MathF.Max(temp[i - 1, j], temp[i, j - 1]);
                }
            }
            List<T> result = new List<T>();
            int max = 0;
            for (int i = 1; i < temp.GetLength(0); i++)
            {
                for (int j = 1; j < temp.GetLength(1); j++)
                {
                    if (max < temp[i, j])
                    {
                        
                    }
                }
            }
            return result.ToArray();
        }
    }
}