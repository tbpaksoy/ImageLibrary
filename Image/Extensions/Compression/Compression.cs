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
        public HuffmanNode<T> Go(bool way)
        {
            if (way) return left;
            else return right;
        }
        public static void Traverse(HuffmanNode<T> node)
        {
            if (node == null) return;
            Console.WriteLine(node.data + node.count.ToString());
            Traverse(node.right);
            Traverse(node.left);
        }
    }
    public static class Compression
    {

        public static HuffmanNode<T> HuffmanAsNode<T>(T[] data)
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
                HuffmanNode<T> node0 = list[^1];
                HuffmanNode<T> node1 = list[^2];
                list.RemoveAt(list.Count - 1);
                list.RemoveAt(list.Count - 1);
                list.Add(node1.UniteWith(node0));
                list.Sort((i, j) => i.count.CompareTo(j.count));
                list.Reverse();
            }
            return list[0];
        }
        public static T[] HuffmanAsArray<T>(T[] data)
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
            Queue<HuffmanNode<T>> queue = new Queue<HuffmanNode<T>>();
            quantity.Reverse();
            foreach (KeyValuePair<T, int> i in quantity)
            {
                queue.Enqueue(new HuffmanNode<T>(i.Key, i.Value));
            }
            while (queue.Count > 1)
            {
                HuffmanNode<T> node = queue.Dequeue().UniteWith(queue.Dequeue());
                queue.Enqueue(node);
            }
            List<T> list = new List<T>();
            HuffmanNode<T> active = queue.Dequeue();
            Stack<HuffmanNode<T>> stack = new Stack<HuffmanNode<T>>();
            while (active != null || stack.Count > 0)
            {
                while (active != null)
                {
                    stack.Push(active);
                    active = active.left;
                }
                active = stack.Pop();
                list.Add(active.data);
                active = active.right;
            }
            return list.ToArray();
        }
    }
}