using System;
using System.Collections.Generic;
using TahsinsLibrary.Calculation;
using TahsinsLibrary.String;
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
        public Color(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            a = 255;
        }
        public override string ToString()
        {
            return r.ToString()+","+g.ToString()+","+b.ToString()+","+a.ToString();
        }
        public string ToString(string format)
        {
            string result = this.ToString();
            switch(format.ToLower())
            {
                case "x":
                    result = r.ToString("X2")+","+g.ToString("X2")+","+b.ToString("X2")+","+a.ToString("X2");
                break;
                //TODO
            }
            return result;
        }
        public static bool IsAllColorsSame(Color[] colors)
        {
            for (int i = 1; i < colors.Length; i++)
            {
                if(!colors[i-1].Equals(colors[i]))
                {
                    return false;
                }
            }
            return true;
        }
        public static readonly Color red = new Color(255,0,0);
        public static readonly Color green = new Color (0,255,0);
        public static readonly Color blue = new Color(0,0,255);
        public static readonly Color purple = new Color(128,0,128);
        public static readonly Color yellow = new Color(255,255,0);
        public static readonly Color lime = new Color(191,255,0);
        public static readonly Color pink = new Color(255,192,203);
        public static readonly Color indigo = new Color(75,0,130);
        public static readonly Color navy = new Color(0,128,0);
        public static readonly Color white = new Color(255,255,255);
        public static readonly Color black = new Color(0,0,0);
        public static readonly Color aliceBlue = new Color(240,248,255);
        public static readonly Color lavender = new Color(230,230,230);
        public static readonly Color lightBlue = new Color(173,216,230);
        public static readonly Color orange = new Color(255,165,0);
        public static readonly Color gold = new Color(255,215,0);
        public static readonly Color coral = new Color(255,127,80);
        public static readonly Color cyan = new Color(0,255,255);
        public static readonly Color silver = new Color(128,128,128);
    }
    public struct ColorRange
    {
        public Color color
        {
            get;private set;
        }
        public float range;
        public ColorRange(Color color,float range)
        {
            this.color = color;
            this.range = range;
        }
    }
    public static class Image
    {

    public static class BMP
    {
        public static string[] Test()
        {
            string[] header = CreateBMPHeader();
            string[] colors = GenerateColorMatrix();
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
        public static string[] CreatePalette(Color[] colors, int paletteWidth, int paletteHeight, int paletteSize)
        {
            List<string> data = new List<string>();
            string[] header = CreateBMPHeader(paletteWidth*paletteSize, paletteHeight*paletteSize);
            foreach(string s in header)
            {
                data.Add(s);
            }
            string[] palette = GenerateColorMatrix(paletteWidth*paletteSize,GenerateColorPalette(colors,paletteWidth,paletteHeight,paletteSize));
            foreach(string s in palette)
            {
                data.Add(s);
            }
            return data.ToArray();
        }
        public static string[] CreateBMPHeader()
        {
            List<string> data = new List<string>();
            CustomCalculation.Length = 2;

            data.Add(CustomCalculation.GetHex("B")[0]+CustomCalculation.GetHex("M")[0]);//ok
            

            CustomCalculation.Length = 4;
            string temp = CustomCalculation.GetHex(7654);
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
        public static string[] CreateBMPHeader(int width, int height)
        {
                List<string> data = new List<string>();
            CustomCalculation.Length = 2;

            data.Add(CustomCalculation.GetHex("B")[0]+CustomCalculation.GetHex("M")[0]);//ok

            CustomCalculation.Length = 8;
            int paddingCount = width * 3 % 4;
            string temp = CustomCalculation.GetHex(54 + width * height * 3 + height * paddingCount);
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
        public static string[] CreateColorTransition(Color[] from, Color[] to, int colorSize, int step)
        {
            List<string> data = new List<string>();
            string[] header = CreateBMPHeader(from.Length*colorSize,(step+2)*colorSize);
            foreach(string s in header)
            {
                data.Add(s);
            }
            string[] palette = GenerateColorMatrix(from.Length*colorSize,GenerateColorTransition(from,to,step,colorSize));
            foreach(string s in palette)
            {
                data.Add(s);
            }
            return data.ToArray();
        }
    }
    public static class PNG
    {
        public enum ColorType
        {
            Grayscale=0,TrueColor=2,Indexed=3,GrayscaleAndAlpha=4,TrueColorAndAlpha=6
        }
        public static string[] CreatePNGHeader()
        {
            List<string> data = new List<string>();
            CustomCalculation.Length = 2;
            data.Add("89");
            foreach(char c in "PNG")
            {
               data.Add(CustomCalculation.GetHex(c)); 
            }
            data.Add("0D");
            data.Add("0A");
            data.Add("1A");
            data.Add("0A");
            return data.ToArray();
        }
        public static string[] CreateIHDRChunk(int width, int height, int bitDepth,ColorType colorType)
        {
            if(!CheckColorFormat(bitDepth*GetColorChannels(colorType), colorType))
            {
                throw new Exception("Color format not supported.");
            }
            List<string> data = new List<string>();
            CustomCalculation.Length = 8;
            foreach(string s in CustomString.Group(CustomCalculation.GetHex(13),2))
            {
                data.Add(s);
            }
            CustomCalculation.Length = 2;
            foreach(char c in "IHDR")
            {
                data.Add(CustomCalculation.GetHex(c));
            }
            CustomCalculation.Length = 8;
            foreach(string s in CustomString.Group(CustomCalculation.GetHex(width),2))
            {
                data.Add(s);
            }
            foreach(string s in CustomString.Group(CustomCalculation.GetHex(height),2))
            {
                data.Add(s);
            }
            CustomCalculation.Length = 2;
            data.Add(CustomCalculation.GetHex(bitDepth));
            data.Add(CustomCalculation.GetHex(GetColorTypeIndex(colorType)));
            data.Add(CustomCalculation.GetHex(0));
            data.Add(CustomCalculation.GetHex(0));
            data.Add(CustomCalculation.GetHex(0));
            return data.ToArray();
        }
        public static int GetColorTypeIndex(ColorType colorType)
        {
            switch(colorType)
            {
                case ColorType.Grayscale: return 0;
                case ColorType.TrueColor: return 2;
                case ColorType.Indexed: return 3;
                case ColorType.GrayscaleAndAlpha: return 4;
                case ColorType.TrueColorAndAlpha: return 6;
            }
            throw new Exception();
        }
        public static int GetColorChannels(ColorType colorType)
        {
            switch(colorType)
            {
                case ColorType.Grayscale:return 1;
                case ColorType.GrayscaleAndAlpha:return 2;
                case ColorType.Indexed:return 1;
                case ColorType.TrueColor:return 3;
                case ColorType.TrueColorAndAlpha:return 4;
            }
            return 0;
        }
        public static bool CheckColorFormat(int bitsPerChannel, ColorType colorType)
        {
            switch(colorType)
            {
                case ColorType.Indexed:
                if(bitsPerChannel == 1 || bitsPerChannel == 2 || bitsPerChannel == 4 || bitsPerChannel == 8)
                {
                    return true;
                }
                break;
                case ColorType.Grayscale:
                if(bitsPerChannel == 1 || bitsPerChannel == 2 || bitsPerChannel == 4 || bitsPerChannel == 8 || bitsPerChannel == 16)
                {
                    return true;
                }
                break;
                case ColorType.GrayscaleAndAlpha:
                if(bitsPerChannel == 32 || bitsPerChannel == 16)
                {
                    return true;
                }
                break;
                case ColorType.TrueColor:
                if(bitsPerChannel == 24 || bitsPerChannel == 48)
                {
                    return true;
                }
                break;
                case ColorType.TrueColorAndAlpha:
                if(bitsPerChannel == 32 || bitsPerChannel == 64)
                {
                    return true;
                }
                break;
            }
            return false;
        }
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
        
        public static string[] GenerateColorMatrix(int width,Color[] resource)
        {
            List<string> temp = new List<string>();
            int height = resource.Length / width;
            int paddingCount = width*3%4;
            CustomCalculation.Length = 2;
            for (int i = 0,q=0; i < height; i++)
            {
                for (int j = 0; j < width; j++,q++)
                {
                    int index = q;
                    temp.Add(CustomCalculation.GetHex(resource[index].b));
                    //Console.Write(" "+temp[temp.Count-1]);
                    temp.Add(CustomCalculation.GetHex(resource[index].g));
                    //Console.Write(temp[temp.Count-1]);
                    temp.Add(CustomCalculation.GetHex(resource[index].r));
                    //Console.Write(temp[temp.Count-1]+" ");
                }
                for (int k = 0; k < paddingCount; k++)
                {
                    temp.Add(CustomCalculation.GetHex(0));
                    //Console.Write(temp[temp.Count-1]);
                }
                //Console.WriteLine();
            }
            //Console.WriteLine(temp.Count);

            return temp.ToArray();
        }
      
    
        public static Color[] GenerateColorArray(int width, int height)
        {
            Color[] colors = new Color[width * height];
            return colors;
        }
        public static Color[] GenerateColorArray(int width, int height,Func<byte,Color> Formula)
        {
            List<Color>colors = new List<Color>();
            for (int i = 0,k=0; i < height; i++)
            {
                for (int j = 0; j < width; j++,k++)
                {
                    colors.Add(Formula((byte)(Math.Min(k,255))));
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
        public static Color[] GenerateColorPalette(Color[] colors, int paletteWidth,int paletteHeight,int colorSize)
        {
            if(colors.Length > paletteWidth * paletteHeight)
            {
                throw new Exception("Palette size can not be smaller than total number of colors");
            }
            else
            {
                Color[,] palette = new Color[paletteWidth,paletteHeight];
                for (int i = 0,k=0; i < paletteWidth; i++)
                {
                    for (int j = 0; j < paletteHeight; j++,k++)
                    {
                        if(k < colors.Length) palette[i,j] = colors[k];
                        else break;
                    }
                }
                Color[,] temp = new Color[paletteWidth*colorSize,paletteHeight*colorSize];
                for (int i = 0; i < temp.GetLength(0); i++)
                {
                    for (int j = 0; j < temp.GetLength(1); j++)
                    {
                        int a = i / colorSize , b = j / colorSize;
                        temp[i,j] = palette[a,b];
                    }
                }
                Color[] result = new Color[temp.GetLength(0)*temp.GetLength(1)]; 
                for (int i = 0,k=0; i < temp.GetLength(0); i++)
                {
                    for (int j = 0; j < temp.GetLength(1); j++,k++)
                    {
                        result[j*paletteWidth*colorSize+i] = temp[i,j];
                    }
                }
                return result;
            }
        }
        public static Color[] GenerateColorTransition(Color[] from, Color[] to, int step,int colorSize)
        {
            if(from.Length != to.Length) throw new Exception("from and to's length have to be equal");
            else
            {
                step = Math.Max(step,0);
                Color[,] palette = new Color[from.Length,step+2];
                for (int i = 0; i < palette.GetLength(0); i++)
                {
                    for (int j = 1; j < palette.GetLength(1)-1; j++)
                    {
                        byte r = (byte)(from[i].r+CustomCalculation.GoToValue(from[i].r,to[i].r)*(j)/(step));
                        byte g = (byte)(from[i].g+CustomCalculation.GoToValue(from[i].g,to[i].g)*(j)/(step));
                        byte b = (byte)(from[i].b+CustomCalculation.GoToValue(from[i].b,to[i].b)*(j)/(step));
                        byte a = (byte)(from[i].a+CustomCalculation.GoToValue(from[i].a,to[i].a)*(j)/(step));
                        palette[i,j] = new Color(r,g,b,a);
                    }
                    palette[i,step+1] = to[i];
                    palette[i,0] = from[i];
                }
                Color[,] temp = new Color[palette.GetLength(0)*colorSize,palette.GetLength(1)*colorSize];
                for (int i = 0; i < temp.GetLength(0); i++)
                {
                    for (int j = 0; j < temp.GetLength(1); j++)
                    {
                        temp[i,j] = palette[i/colorSize,j/colorSize];
                    }
                }
                Color[] result = new Color[temp.GetLength(0)*temp.GetLength(1)];
                for (int i = 0; i < temp.GetLength(0); i++)
                {
                    for (int j = 0; j < temp.GetLength(1); j++)
                    {
                        result[j*from.Length*colorSize+i] = temp[i,j];
                    }
                }
                return result;
            }
        }
    }
}
