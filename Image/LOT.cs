using System;
using System.Collections.Generic;
using TahsinsLibrary.Array;
using System.Globalization;

namespace TahsinsLibrary
{
    public enum Way
    {
        North, South, West, East
    }
}
namespace TahsinsLibrary.String
{
    public static partial class CustomString
    {

        public static string Reverse(string resource)
        {
            if (resource != null)
            {
                char[] newChars = new char[resource.Length];
                for (int i = resource.Length - 1, j = 0; i >= 0; i--, j++)
                {
                    newChars[j] = resource[i];
                }
                return new string(newChars);
            }
            else return null;
        }
        public static string[] Group(string resource, int count)
        {
            if (resource == null) return null;
            string[] result = new string[(int)Math.Ceiling((float)(resource.Length / count))];
            int amount = (int)Math.Ceiling((float)(resource.Length / result.Length));
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = resource.Substring(i * amount, Math.Min(amount, resource.Length - i * amount));
            }
            return result;
        }
        public static string GenerateFromArray(string[] resource)
        {
            if (resource == null) return null;
            int length = 0;
            foreach (string s in resource)
            {
                if (s == null) return null;
                length += s.Length;
            }
            char[] temp = new char[length];
            for (int i = 0, k = 0; i < resource.Length; i++)
            {
                for (int j = 0; j < resource[i].Length; j++, k++)
                {
                    temp[k] = resource[i][j];
                }
            }
            return new string(temp);
        }
        public static string ReverseGroup(string resource, int count)
        {
            string[] temp = Group(resource, 2);
            temp = CustomArray.ReverseElements<string>(temp);
            return GenerateFromArray(temp);
        }
        public static string UniteAsOneString(string[] resource)
        {
            int length = 0;
            foreach (string s in resource)
            {
                length += s.Length;
            }
            char[] temp = new char[length];
            for (int i = 0, k = 0; i < resource.Length; i++)
            {
                for (int j = 0; j < resource[i].Length; j++, k++)
                {
                    temp[k] = resource[i][j];
                }
            }
            return new string(temp);
        }
    }
}
namespace TahsinsLibrary.Array
{
    public static partial class CustomArray
    {
        public static T[] ReverseElements<T>(T[] resource)
        {
            T[] result = new T[resource.Length];
            for (int i = 0, j = result.Length - 1; i < resource.Length; i++, j--)
            {
                result[i] = resource[j];
            }
            return result;
        }
        public static T[] SubArray<T>(T[] source, int index, int length)
        {
            T[] result = new T[length];
            for (int i = index, j = 0; i < index + length; i++, j++)
            {
                result[j] = source[i];
            }
            return result;
        }
    }
}
namespace TahsinsLibrary.Calculation
{
    public static class CustomCalculation
    {
        private static int _length = 8;
        public static int Length
        {
            set
            {
                if (value > 0) _length = value;
            }
        }
        public static string HexCodeFormat
        {
            get
            {
                return "X" + _length.ToString();
            }
        }
        public static string[] GetHex(ValueType[] resource)
        {
            string[] result = new string[resource.Length];
            for (int i = 0; i < resource.Length; i++)
            {
                result[i] = ((int)resource[i]).ToString(HexCodeFormat);
            }
            return result;
        }
        public static string[] GetHex(char[] resource)
        {
            string[] result = new string[resource.Length];
            for (int i = 0; i < resource.Length; i++)
            {
                result[i] = ((int)resource[i]).ToString(HexCodeFormat);
            }
            return result;
        }
        public static string[] GetHex(string resource)
        {
            string[] result = new string[resource.Length];
            for (int i = 0; i < resource.Length; i++)
            {
                result[i] = ((int)resource[i]).ToString(HexCodeFormat);
            }
            return result;
        }
        public static string[] GetHex(int[] resource)
        {
            string[] result = new string[resource.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = resource[i].ToString(HexCodeFormat);
            }
            return result;
        }
        public static string GetHex(char resource)
        {
            return ((int)resource).ToString(HexCodeFormat);
        }
        public static string GetHex(int resource)
        {
            return resource.ToString(HexCodeFormat);
        }
        public static string GetHex(ValueType resource)
        {
            return ((int)resource).ToString(HexCodeFormat);
        }
        public static string HexToBinary(string resource)
        {
            return Convert.ToString(Convert.ToInt32(resource, 16), 2);
        }
        public static byte[] ToByteArray(string[] resouce)
        {
            List<byte> temp = new List<byte>();
            foreach (string s in resouce)
            {
                for (int i = 0; i < s.Length / 2; i++)
                {
                    byte b = (byte.Parse(s.Substring(i * 2, 2), NumberStyles.HexNumber));
                    temp.Add(b);
                }
            }

            return temp.ToArray();
        }
        public static int DiffrenceAbs(int a, int b)
        {
            return Math.Abs(a - b);
        }
        public static int Diffrence(int a, int b)
        {
            if (a < b)
                return a - b;
            else
                return
                a + b;
        }
        public static float Diffrence(float a, float b)
        {
            if (a < b)
                return a - b;
            else
                return a + b;
        }
        public static float GoToValue(float from, float to)
        {
            if (from >= to) return -MathF.Abs(to - from);
            else return MathF.Abs(from - to);
        }
    }
}
namespace TahsinsLibrary.Analyze
{
    public static class Analyze
    {
        public static bool isTempStarted = false;
        private static List<(int, int)> temp = new List<(int, int)>();
        public static (List<string>, List<int>) AnalyzeByteList(List<byte> resource, bool writeToConsole = false, int writeBy = 0)
        {
            List<string> including = new List<string>();
            List<int> amount = new List<int>();

            including.Add(resource[0].ToString("X2"));
            amount.Add(1);

            for (int i = 1; i < resource.Count; i++)
            {
                if (resource[i - 1] == resource[i])
                {
                    amount[amount.Count - 1]++;
                }
                else
                {
                    including.Add(resource[i].ToString("X2"));
                    amount.Add(1);
                }
            }

            if (writeToConsole)
            {
                for (int i = writeBy; i < including.Count; i++)
                {
                    Console.WriteLine(including[i] + "--" + amount[i].ToString());
                }
            }
            return (including, amount);
        }
        public static (List<string>, List<int>) AnalyzeByteArray(byte[] resource, int analyzeBy = 0, bool writeToConsole = false, int analyzeTo = int.MaxValue)
        {
            List<string> including = new List<string>();
            List<int> amount = new List<int>();
            analyzeTo = Math.Min(resource.Length, analyzeTo);
            including.Add(resource[analyzeBy].ToString("X2"));
            amount.Add(1);

            for (int i = analyzeBy + 1; i < analyzeTo; i++)
            {
                if (resource[i - 1] == resource[i])
                {
                    amount[amount.Count - 1]++;
                }
                else
                {
                    including.Add(resource[i].ToString("X2"));
                    amount.Add(1);
                }
            }

            if (writeToConsole)
            {
                for (int i = 0; i < including.Count; i++)
                {
                    Console.WriteLine(including[i] + "--" + amount[i].ToString());
                }
            }
            return (including, amount);
        }

        public static void GetAdjenctIndexesForClassType<T>(T item, int x, int y, T[,] source) where T : class
        {
            if (!isTempStarted)
            {
                temp.Clear();
                isTempStarted = true;
            }
            if (source[x, y] == item && !temp.Contains((x, y)))
            {
                temp.Add((x, y));
            }
            else return;
            if (x - 1 > -1) GetAdjenctIndexesForClassType<T>(item, x - 1, y, source);
            if (x + 1 < source.GetLength(0)) GetAdjenctIndexesForClassType<T>(item, x + 1, y, source);
            if (y - 1 > -1) GetAdjenctIndexesForClassType<T>(item, x, y - 1, source);
            if (y + 1 < source.GetLength(1)) GetAdjenctIndexesForClassType<T>(item, x, y + 1, source);
        }
        public static void GetAdjenctIndexesForStructType<T>(T item, int x, int y, T[,] source) where T : struct
        {
            if (!isTempStarted)
            {
                isTempStarted = true;
            }
            if (source[x, y].Equals(item) && !temp.Contains((x, y)))
            {
                temp.Add((x, y));
            }
            else return;
            if (x - 1 > -1) GetAdjenctIndexesForStructType<T>(item, x - 1, y, source);
            if (x + 1 < source.GetLength(0)) GetAdjenctIndexesForStructType<T>(item, x + 1, y, source);
            if (y - 1 > -1) GetAdjenctIndexesForStructType<T>(item, x, y - 1, source);
            if (y + 1 < source.GetLength(1)) GetAdjenctIndexesForStructType<T>(item, x, y + 1, source);
        }
        public static List<(int, int)> CheckAdjenctivty<T>(T item, int x, int y, T[,] source)
        {
            bool[,] isChecked = new bool[source.GetLength(0), source.GetLength(1)];
            List<(int, int)> list = new List<(int, int)>();
            List<(int, int)> borders = new List<(int, int)>();
            list.Add((x, y));
            if (x > 0 && item.Equals(source[x - 1, y])) borders.Add((x - 1, y));
            if (x < source.GetLength(1) - 1 && item.Equals(source[x + 1, y])) borders.Add((x + 1, y));
            if (y > 0 && item.Equals(source[x, y - 1])) borders.Add((x, y - 1));
            if (y < source.GetLength(1) - 1 && item.Equals(source[x, y + 1])) borders.Add((x, y + 1));
            while (borders.Count > 0)
            {
                for (int j = 0; j < borders.Count; j++)
                {
                    (int, int) i = borders[j];
                    borders.Remove(i);
                    isChecked[i.Item1, i.Item2] = true;
                    if (source[i.Item1, i.Item2].Equals(item))
                    {
                        list.Add(i);
                        if (i.Item1 > 0 && isChecked[i.Item1 - 1, i.Item2] == false) borders.Add((i.Item1 - 1, i.Item2));
                        if (i.Item1 < source.GetLength(0) - 1 && isChecked[i.Item1 + 1, i.Item2] == false) borders.Add((i.Item1 + 1, i.Item2));
                        if (i.Item2 > 0 && isChecked[i.Item1, i.Item2 - 1] == false) borders.Add((i.Item1, i.Item2 - 1));
                        if (i.Item2 < source.GetLength(1) - 1 && isChecked[i.Item1, i.Item2 + 1] == false) borders.Add((i.Item1, i.Item2 + 1));
                    }
                }
            }
            return list;
        }
        public static List<(int, int)> GetTemp()
        {
            return temp;
        }
        public static void ClearTemp()
        {
            isTempStarted = false;
            temp.Clear();
        }
        public static List<(int, int)> GetTempsCopy(bool clearTemp)
        {
            List<(int, int)> list = new List<(int, int)>();
            foreach ((int, int) i in temp) list.Add(i);
            if (clearTemp) temp.Clear();
            return list;
        }
    }
    public static class Compare
    {
        public static (bool[], int, int) CompareArrays(System.Array a, System.Array b, bool writeToConsole = false, int start = 0, int end = int.MaxValue)
        {
            bool[] comp = new bool[Math.Max(a.Length, b.Length)];
            int equal = 0;
            int notEqual = comp.Length;
            end = Math.Min(end, comp.Length);
            for (int i = 0; i < comp.Length; i++)
            {
                if (i < a.Length && i < b.Length && a.GetValue(i).Equals(b.GetValue(i)))
                {
                    equal++;
                    notEqual--;
                    comp[i] = true;
                }

            }

            if (writeToConsole)
            {
                for (int i = start; i < end; i++)
                {
                    Console.WriteLine(i.ToString("X2") + "->" + comp[i].ToString());
                }
                Console.WriteLine("+:" + equal.ToString() + ",-:" + notEqual.ToString());
            }

            return (comp, equal, notEqual);
        }
        public static int Max(params int[] numbers)
        {
            int result = int.MinValue;
            foreach (int i in numbers)
            {
                if (i > result) result = i;
            }
            return result;
        }
    }
}