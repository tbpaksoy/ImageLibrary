using System;
using System.Collections.Generic;
using TahsinsLibrary.Array;
using System.Globalization;
namespace TahsinsLibrary.String
{
    public static partial class CustomString
    {
        public static string Reverse(string resource)
        {
            if(resource!=null)
            {
                char[] newChars = new char[resource.Length];
                for(int i =  resource.Length - 1,  j = 0; i >= 0; i--,j++)
                {
                    newChars[j] = resource[i]; 
                }
                return new string(newChars);
            }
            else return null;
        }
        public static string[] Group(string resource, int count)
        {
            if(resource == null) return null;
            string[] result = new string[(int)Math.Ceiling((float)(resource.Length/count))];
            int amount = (int)Math.Ceiling((float)(resource.Length/result.Length));
            for(int i = 0; i < result.Length; i++)
            {
                result[i] = resource.Substring(i*amount,Math.Min(amount,resource.Length-i*amount));
            }
            return result;
        }
        public static string GenerateFromArray(string[] resource)
        {
            if(resource == null) return null;
            int length = 0;
            foreach(string s in resource)
            {
                if(s == null) return null;
                length += s.Length;
            }
            char[] temp = new char[length];
            for (int i = 0,k=0; i < resource.Length; i++)
            {
                for (int j = 0; j < resource[i].Length; j++,k++)
                {
                    temp[k] = resource[i][j];
                }
            }
            return new string(temp);
        }
        public static string ReverseGroup(string resource, int count)
        {
            string [] temp = Group(resource,2);
            temp =  CustomArray.ReverseElements<string>(temp);
            return GenerateFromArray(temp);
        }
        public static string UniteAsOneString(string[] resource)
        {
            int length = 0;
            foreach(string s in resource)
            {
                length += s.Length;
            }
            char[] temp = new char[length];
            for (int i = 0,k=0; i < resource.Length; i++)
            {
                for (int j = 0; j < resource[i].Length; j++,k++)
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
            for (int i = 0 ,j = result.Length - 1; i < resource.Length; i++,j--)
            {
                result[i] = resource[j];
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
                if(value > 0) _length = value;
            }
        }
        public static string HexCodeFormat
        {
            get
            {
                return "X"+_length.ToString();
            }
        } 
        public static string[] GetHex(ValueType[] resource)
        {
            string[] result  = new string[resource.Length];
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
                result[i]=((int)resource[i]).ToString(HexCodeFormat);
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
            return Convert.ToString(Convert.ToInt32(resource,16),2);
        }
        public static byte[] ToByteArray(string[] resouce)
        {
            /*for (int i = 16; i < resouce.Length; i++)
            {
                if(!(resouce[i]=="FF" || resouce[i]=="00" || resouce[i] == "000000"))
                {
                    Console.WriteLine(resouce[i]);
                }
            }*/
            List<byte>temp = new List<byte>();
            foreach(string s in resouce)
            {
                for (int i = 0; i < s.Length/2; i++)
                {
                    byte b =(byte.Parse(s.Substring(i*2,2),NumberStyles.HexNumber));
                    temp.Add(b);
                }
            }
            /*for (int i = 16; i < temp.Count; i++)
            {
                if(!(temp[i]==0 || temp[i]==255)) Console.WriteLine(temp[i]);
            }*/

            return temp.ToArray();
        }
    }
}