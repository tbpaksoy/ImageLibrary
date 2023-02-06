using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;
using System.Collections;
namespace TahsinsLibrary.String
{
    public static partial class CustomString
    {
        public static bool IsHex(string s)
        {
            foreach (char c in s)
            {
                if (!s.Contains(c)) return false;
            }
            return true;
        }
    }
}
namespace TahsinsLibrary.Calculation
{
    public static class CustomCalculation
    {
        public static float GoToValue(float from, float to)
        {
            if (from >= to) return -MathF.Abs(to - from);
            else return MathF.Abs(from - to);
        }
    }
}
namespace TahsinsLibrary.Analyze
{

}
public static class Compare
{
    public static float Max(params float[] numbers)
    {
        int result = int.MinValue;
        foreach (int i in numbers)
        {
            if (i > result) result = i;
        }
        return result;
    }
    public static float Min(params float[] numbers)
    {
        int result = int.MaxValue;
        foreach (int i in numbers)
        {
            if (i < result) result = i;
        }
        return result;
    }
}
