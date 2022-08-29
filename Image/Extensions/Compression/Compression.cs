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
        public static HuffmanNode<T> Huffman<T>(T[] data)
        {
            Dictionary<T, int> dataQuantity = new Dictionary<T, int>();
            foreach (T t in data)
            {
                if (!dataQuantity.TryAdd(t, 1))
                {
                    dataQuantity[t]++;
                }
            }
            List<KeyValuePair<T, int>> quantity = new List<KeyValuePair<T, int>>();
            foreach (KeyValuePair<T, int> pair in dataQuantity.OrderBy(key => key.Value))
            {
                quantity.Add(pair);
            }
            List<HuffmanNode<T>> list = new List<HuffmanNode<T>>();
            foreach (KeyValuePair<T, int> i in quantity)
            {
                list.Add(new HuffmanNode<T>(i.Key, i.Value));
            }
            while (list.Count > 1)
            {
                HuffmanNode<T> node0 = list[^0];
                HuffmanNode<T> node1 = list[^1];
                list.RemoveAt(list.Count - 1);
                list.RemoveAt(list.Count - 1);
                list.Add(node1.UniteWith(node0));
                list.Sort((i, j) => i.count.CompareTo(j.count));
                list.Reverse();
            }
            return list[0];
        }
        public static HuffmanNode<T> Huffman<T>(List<T> data) => Huffman<T>(data.ToArray());
        public static string[] LZ77(List<char> list)
        {
            List<string> result = new List<string>();
            while (list.Count > 0)
            {

            }
            return result.ToArray();
        }
    }
}