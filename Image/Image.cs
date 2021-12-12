using System;
using System.Collections.Generic;
using TahsinsLibrary.Calculation;
using TahsinsLibrary.String;
using System.Linq;
namespace TahsinsLibrary
{
public struct Color
{
    public byte r,g,b,a;
    public Color(byte r, byte g, byte b, byte a)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }
    public override string ToString()
    {
        return r.ToString()+","+g.ToString()+","+b.ToString()+","+a.ToString();
    }
}
    public static class Image
    {
        public static string[] Test()
        {
            string[] header = BMP.CreateBMPHeader();
            string[] colors = BMP.GenerateColorMatrix();
            List<string> temp = new List<string>();
            foreach(string s in header)
            {
                temp.Add(CustomCalculation.HexToBinary(s));
            }
            foreach(string s in colors)
            {
                temp.Add(CustomCalculation.HexToBinary(s));
            }
            return temp.ToArray();
        }
    public static class BMP
    {
        public static string[] CreateBMPHeader()
        {
            List<string> data = new List<string>();
            CustomCalculation.Length = 2;

            data.Add(CustomCalculation.GetHex("B")[0]+CustomCalculation.GetHex("M")[0]);//ok
            

            CustomCalculation.Length = 8;
            string temp = CustomCalculation.GetHex(70);
            data.Add(CustomString.ReverseGroup(temp,2));
            //ok
            CustomCalculation.Length = 4;
            string[] tempArr = CustomCalculation.GetHex(new int[]{0,0});
            foreach(string s in tempArr)
            {
                data.Add(CustomString.ReverseGroup(s,2));
            }
            //ok
            CustomCalculation.Length = 8;
            tempArr = CustomCalculation.GetHex(new int[]{54,40,2,2});
                        foreach(string s in tempArr)
            {
                data.Add(CustomString.ReverseGroup(s,2));
            }

            CustomCalculation.Length = 4;
            tempArr = CustomCalculation.GetHex(new int[]{1,24});
                        foreach(string s in tempArr)
            {
                data.Add(CustomString.ReverseGroup(s,2));
            }

            CustomCalculation.Length = 8;
            tempArr = CustomCalculation.GetHex(new int[]{0,16,2835,2835,0,0});
            foreach(string s in tempArr)
            {
                data.Add(CustomString.ReverseGroup(s,2));
            }
            return data.ToArray();
        }
        public static string[] GenerateColorMatrix()
        {
            List<string> data = new List<string>();
            data.Add("0000FF");
            data.Add("FFFFFF");
            data.Add("0000");
            data.Add("FF0000");
            data.Add("00FF00");
            data.Add("0000");
            return data.ToArray();
        }
        public static string[] CreateBMPHeader(int width, int height)
        {
                List<string> data = new List<string>();
            CustomCalculation.Length = 2;

            data.Add(CustomCalculation.GetHex("B")[0]+CustomCalculation.GetHex("M")[0]);//ok
            

            CustomCalculation.Length = 8;
            string temp = CustomCalculation.GetHex(54 + width * height * 3 + height * 3);
            data.Add(CustomString.ReverseGroup(temp,2));
            //ok
            CustomCalculation.Length = 4;
            string[] tempArr = CustomCalculation.GetHex(new int[]{0,0});
            foreach(string s in tempArr)
            {
                data.Add(CustomString.ReverseGroup(s,2));
            }
            //ok
            CustomCalculation.Length = 8;
            tempArr = CustomCalculation.GetHex(new int[]{54,40,width,height});
                        foreach(string s in tempArr)
            {
                data.Add(CustomString.ReverseGroup(s,2));
            }

            CustomCalculation.Length = 4;
            tempArr = CustomCalculation.GetHex(new int[]{1,24});
                        foreach(string s in tempArr)
            {
                data.Add(CustomString.ReverseGroup(s,2));
            }

            CustomCalculation.Length = 8;
            tempArr = CustomCalculation.GetHex(new int[]{0,16,2835,2835,0,0});
            foreach(string s in tempArr)
            {
                data.Add(CustomString.ReverseGroup(s,2));
            }
            return data.ToArray();
        }
        public static string[] GenerateColorMatrix(int width,Color[] resource)
        {
            List<string> temp = new List<string>();
            for (int i = 0,j=0; i < resource.Length; i++,j++)
            {
                if(j>0 && j%width == 0)
                {
                CustomCalculation.Length=6;
                temp.Add(CustomCalculation.GetHex(0));
                j=0;
                
                }
                CustomCalculation.Length = 2;
                temp.Add(CustomCalculation.GetHex(resource[i].b));
                temp.Add(CustomCalculation.GetHex(resource[i].g));
                temp.Add(CustomCalculation.GetHex(resource[i].r));
            }
            CustomCalculation.Length=6;
            temp.Add(CustomCalculation.GetHex(0));
            return temp.ToArray();
        }
        public static string[] CreateData(int width,int height,Func<byte,Color> Formula)
        {
            List<string> data = new List<string>();

            string[]header = CreateBMPHeader(width,height);
            foreach(string s in header)
            {
                data.Add(s);
            }
            string[] colors = GenerateColorMatrix(width,GenerateColorArray(width, height, Formula));
            foreach(string s in colors)
            {
                data.Add(s);
            }

            return data.ToArray();
        }
    }
    
        public static (Color[],int) GenerateColorArray(int width, int height)
        {
            Color[] colors = new Color[width * height];
            return (colors, width);
        }
        public static Color[] GenerateColorArray(int width, int height,Func<byte,Color> Formula)
        {
            List<Color>colors = new List<Color>();
            for (int i = 0,k=0; i < height; i++)
            {
                for (int j = 0; j < width; j++,k++)
                {
                    colors.Add(Formula((byte)(k%256)));
                }
            }

            return colors.ToArray();
        }
        public static Color[] ModifyColorArray(Color[] colors,int width,Func<byte, Color> Formula)
        {
            Color[] newColors =  colors;
            for(int i = 0; i < newColors.Length/width;i++)
            {
                for(int j = 0; j < width; j++)
                {
                    newColors[i + j] = Formula((byte)(i + j));
                }
            }
            return newColors;
        }
        public static Color[] ModifyColorArray(Color[] colors,int width,Func<byte,byte, Color> Formula)
        {
            Color[] newColors =  colors;
            int height = newColors.Length/width;
            for(int i = 0; i < height;i++)
            {
                for(int j = 0; j < width; j++)
                {
                    newColors[i*height + j] = Formula((byte)i,(byte)j);
                }
            }
            return newColors;
        }
    }
}
