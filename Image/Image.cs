using System;
using System.Collections.Generic;
using TahsinsLibrary.Calculation;
using TahsinsLibrary.String;
using System.IO;
using TahsinsLibrary.Analyze;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace TahsinsLibrary
{
    public interface IColorTurnable
    {
        public Color[] TurnColor();
    }
    public interface IExportable
    {
        public string name { get; set; }
        public void Export(string path, string name);
        public bool IsReadyToExport();
    }
    public struct Color
    {
        public byte r, g, b, a;
        public string hex
        {
            get
            {
                return r.ToString("X2") + g.ToString("X2") + b.ToString("X2") + a.ToString("X2");
            }
            set
            {
                if (value.Length == 8 && CustomString.IsHex(value))
                {
                    r = byte.Parse(value.Substring(0, 2), NumberStyles.HexNumber);
                    g = byte.Parse(value.Substring(2, 2), NumberStyles.HexNumber);
                    b = byte.Parse(value.Substring(4, 2), NumberStyles.HexNumber);
                    a = byte.Parse(value.Substring(6, 2), NumberStyles.HexNumber);
                }
            }
        }
        public Color negative
        {
            get
            {
                return new Color(255 - r, 255 - g, 255 - b, a);
            }
        }
        public Color greyScaleAvarage
        {
            get
            {
                int value = (r + g + b) / 3;
                return new Color(value, value, value, a);
            }
        }
        public Color(string hex)
        {
            r = g = b = a = 0;
            if (hex != null && CustomString.IsHex(hex))
            {
                switch (hex.Length)
                {
                    case 6:
                        r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                        g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                        b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                        a = 255;
                        break;
                    case 8:
                        r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                        g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                        b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                        a = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
                        break;
                }
            }
            else Console.WriteLine("Due the wrong format all values (r,g,b,a) set to 0.");
        }
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
        public Color(int r, int g, int b, int a)
        {
            this.r = (byte)r;
            this.g = (byte)g;
            this.b = (byte)b;
            this.a = (byte)a;
        }
        public Color(int r, int g, int b)
        {
            this.r = (byte)r;
            this.g = (byte)g;
            this.b = (byte)b;
            a = 255;
        }
        public override string ToString()
        {
            return r.ToString() + "," + g.ToString() + "," + b.ToString() + "," + a.ToString();
        }
        public string ToString(string format)
        {
            string result = this.ToString();
            switch (format.ToLower())
            {
                case "x":
                    result = r.ToString("X2") + "," + g.ToString("X2") + "," + b.ToString("X2") + "," + a.ToString("X2");
                    break;
                    //TODO
            }
            return result;
        }
        public static bool IsAllColorsSame(Color[] colors)
        {
            for (int i = 1; i < colors.Length; i++)
            {
                if (!colors[i - 1].Equals(colors[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public Color GetMidColor(Color color)
        {
            return new Color((r + color.r) / 2, (g + color.g) / 2, (b + color.b) / 2, (a + color.a) / 2);
        }
        public static Color GetMildColor(Color a, Color b)
        {
            return a.GetMidColor(b);
        }
        public void GetFromHex(string hex)
        {
            if (hex != null)
            {
                switch (hex.Length)
                {
                    case 2:
                        r = byte.Parse(hex, NumberStyles.HexNumber);
                        break;
                    case 4:
                        r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                        g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                        break;
                    case 6:
                        r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                        g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                        b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                        break;
                    case 8:
                        r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                        g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                        b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                        a = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
                        break;
                }
            }
        }
        public static Color GetColorFromLibrary(string name)
        {
            name = name.Trim().ToLower();
            foreach (string file in Directory.EnumerateFiles(@"C:\Users\Tahsin\Desktop\Image\Colors"))
            {
                if (file.EndsWith("json"))
                {
                    JObject jo = JObject.Parse(File.ReadAllText(file));
                    if (jo.ContainsKey(name))
                    {

                        return jo[name].ToObject<Color>();
                    }
                }
            }
            return new Color();
        }
        public static Color FromHex(string hex)
        {
            Color color = new Color();
            if (hex.Length == 8) color.hex = hex;
            return color;
        }
        public static readonly Color red = new Color(255, 0, 0);//X
        public static readonly Color green = new Color(0, 255, 0);//X
        public static readonly Color blue = new Color(0, 0, 255);//X
        public static readonly Color purple = new Color(128, 0, 128);//X
        public static readonly Color yellow = new Color(255, 255, 0);//X
        public static readonly Color lime = new Color(191, 255, 0);//x
        public static readonly Color pink = new Color(255, 192, 203);//X
        public static readonly Color indigo = new Color(75, 0, 130);//x
        public static readonly Color navy = new Color(0, 0, 128);//x
        public static readonly Color white = new Color(255, 255, 255);//X
        public static readonly Color black = new Color(0, 0, 0);//X
        public static readonly Color aliceBlue = new Color(240, 248, 255);//x
        public static readonly Color lavender = new Color(230, 230, 230);//x
        public static readonly Color lightBlue = new Color(173, 216, 230);//x
        public static readonly Color orange = new Color(255, 165, 0);//X
        public static readonly Color gold = new Color(255, 215, 0);//X
        public static readonly Color coral = new Color(255, 127, 80);//x
        public static readonly Color cyan = new Color(0, 255, 255);//x
        public static readonly Color silver = new Color(128, 128, 128);//X
        public static readonly Color paleTurquise = new Color(175, 238, 238);//x
        public static readonly Color turquise = new Color(64, 224, 208);//x
        public static readonly Color mediumTurquoise = new Color(72, 209, 204);//x
        public static readonly Color darkTurquoise = new Color(0, 206, 209);//x
        public static readonly Color grey = new Color(128, 128, 128);//X
        public static readonly Color ivory = new Color(255, 255, 240);//x
        public static readonly Color burgundy = new Color(128, 0, 32);//x
        public static readonly Color auburn = new Color(146, 39, 36);//x
        public static readonly Color kuCrimson = new Color(225, 8, 22);//x
        public static readonly Color wood = new Color(186, 140, 99);//x
        public static readonly Color bronze = new Color(205, 127, 50);//x
        public static readonly Color copper = new Color(184, 115, 51);//x
        public static readonly Color sand = new Color(194, 178, 128);//x
    }
    public struct ColorRange
    {
        public Color color
        {
            get; private set;
        }
        public float range;
        public ColorRange(Color color, float range)
        {
            this.color = color;
            this.range = range;
        }
    }
    public struct MaterialForUnity : IExportable
    {
        public string name { get; set; }
        public Color[] albedo;
        public byte[] metallic;
        public byte[] smoothness;
        public Color[] emission;
        public int paletteSize;
        public MaterialForUnity(string name, Color[] albedo, byte[] metallic, byte[] smoothness, Color[] emission, int paletteSize)
        {
            this.name = name;
            this.albedo = albedo;
            this.metallic = metallic;
            this.smoothness = smoothness;
            this.emission = emission;
            this.paletteSize = paletteSize;
        }
        public void Export(string path, string name)
        {
            if (!IsReadyToExport()) return;
            if (File.Exists($"{path}\\{name}Albedo.bmp") || File.Exists($"{path}\\{name}Metallic.bmp") || File.Exists($"{path}\\{name}Smoothness.bmp")
            || File.Exists($"{path}\\{name}Emission.bmp")
            ) return;
            int decised = (int)MathF.Ceiling(MathF.Sqrt(albedo.Length));
            FileStream fs = new FileStream($"{path}\\{name}Albedo.bmp", FileMode.Create);
            byte[] data = CustomCalculation.ToByteArray(Image.BMP.CreatePalette(albedo, decised, decised, paletteSize));
            fs.Write(data);
            fs.Flush();
            fs.Close();
            fs = new FileStream($"{path}\\{name}Metallic.bmp", FileMode.Create);
            data = CustomCalculation.ToByteArray(Image.BMP.CreatePalette(GetMetallic(), decised, decised, paletteSize));
            fs.Write(data);
            fs.Flush();
            fs.Close();
            fs = new FileStream($"{path}\\{name}Smoothness.bmp", FileMode.Create);
            data = CustomCalculation.ToByteArray(Image.BMP.CreatePalette(GetSmoothness(), decised, decised, paletteSize));
            fs.Write(data);
            fs.Flush();
            fs.Close();
            fs = new FileStream($"{path}\\{name}Emission.bmp", FileMode.Create);
            data = CustomCalculation.ToByteArray(Image.BMP.CreatePalette(emission, decised, decised, paletteSize));
            fs.Write(data);
            fs.Flush();
            fs.Close();
        }
        public Color[] GetMetallic()
        {
            Color[] result = new Color[metallic.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new Color(metallic[i], metallic[i], metallic[i]);
            }
            return result;
        }
        public Color[] GetSmoothness()
        {
            Color[] result = new Color[smoothness.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new Color(smoothness[i], smoothness[i], smoothness[i]);
            }
            return result;
        }
        public bool IsReadyToExport()
        {
            int[] lenghts = { albedo.Length, metallic.Length, smoothness.Length, emission.Length };
            for (int i = 1; i < lenghts.Length; i++)
            {
                if (lenghts[i] != lenghts[i - 1]) return false;
            }
            if (paletteSize < 1) return false;
            return true;
        }
    }
    public struct MaterialForBlender : IExportable
    {
        public string name { get; set; }
        public Color[] baseColor;
        public byte[] metallic;
        public byte[] specular;
        public byte[] roughness;
        public Color[] emission;
        public int paletteSize;
        public bool IsReadyToExport()
        {
            if (paletteSize < 1) return false;
            return true;
        }
        public Color[] GetMetallic()
        {
            Color[] result = new Color[metallic.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new Color(metallic[i], metallic[i], metallic[i]);
            }
            return result;
        }
        public Color[] GetRoughness()
        {
            Color[] result = new Color[roughness.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new Color(roughness[i], roughness[i], roughness[i]);
            }
            return result;
        }
        public void Export(string path, string name)
        {
            if (!IsReadyToExport()) return;
            if (File.Exists($"{path}\\{name}BaseColor.bmp") || File.Exists($"{path}\\{name}Metallic.bmp") || File.Exists($"{path}\\{name}Roughness.bmp")
            || File.Exists($"{path}\\{name}Emission.bmp")
            ) return;
            int decised = (int)MathF.Ceiling(MathF.Sqrt(Compare.Max(
            baseColor == null ? 0 : baseColor.Length,
            metallic == null ? 0 : metallic.Length,
            roughness == null ? 0 : roughness.Length,
            emission == null ? 0 : emission.Length)));
            if (baseColor != null)
            {
                FileStream fs = new FileStream($"{path}\\{name}BaseColor.bmp", FileMode.Create);
                byte[] data = CustomCalculation.ToByteArray(Image.BMP.CreatePalette(baseColor, decised, decised, paletteSize));
                fs.Write(data);
                fs.Flush();
                fs.Close();
            }
            if (metallic != null)
            {
                FileStream fs = new FileStream($"{path}\\{name}Metallic.bmp", FileMode.Create);
                byte[] data = CustomCalculation.ToByteArray(Image.BMP.CreatePalette(GetMetallic(), decised, decised, paletteSize));
                fs.Write(data);
                fs.Flush();
                fs.Close();
            }
            if (roughness != null)
            {
                FileStream fs = new FileStream($"{path}\\{name}Roughness.bmp", FileMode.Create);
                byte[] data = CustomCalculation.ToByteArray(Image.BMP.CreatePalette(GetRoughness(), decised, decised, paletteSize));
                fs.Write(data);
                fs.Flush();
                fs.Close();
            }
            if (emission != null)
            {
                FileStream fs = new FileStream($"{path}\\{name}Emission.bmp", FileMode.Create);
                byte[] data = CustomCalculation.ToByteArray(Image.BMP.CreatePalette(emission, decised, decised, paletteSize));
                fs.Write(data);
                fs.Flush();
                fs.Close();
            }
        }
    }

    public static class Image
    {

        public static class BMP
        {
            public static string[] GenerateColorMatrix(int width, Color[] resource)
        {
            List<string> temp = new List<string>();
            int height = resource.Length / width;
            int paddingCount = width * 3 % 4;
            CustomCalculation.Length = 2;
            for (int i = 0, q = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++, q++)
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
            public static string[] CreateData(int width, int height, Func<byte, Color> Formula)
            {
                List<string> data = new List<string>();

                string[] header = CreateBMPHeader(width, height);
                foreach (string s in header)
                {
                    data.Add(s);
                }
                string[] colors = GenerateColorMatrix(width, GenerateColorArray(width, height, Formula));
                foreach (string s in colors)
                {
                    data.Add(s);
                }

                return data.ToArray();
            }
            public static string[] CreatePalette(Color[] colors, int paletteWidth, int paletteHeight, int paletteSize)
            {
                List<string> data = new List<string>();
                string[] header = CreateBMPHeader(paletteWidth * paletteSize, paletteHeight * paletteSize);
                foreach (string s in header)
                {
                    data.Add(s);
                }
                string[] palette = GenerateColorMatrix(paletteWidth * paletteSize, GenerateColorPalette(colors, paletteWidth, paletteHeight, paletteSize));
                foreach (string s in palette)
                {
                    data.Add(s);
                }
                return data.ToArray();
            }
            public static string[] CreateBMPHeader(int width, int height)
            {
                List<string> data = new List<string>();
                CustomCalculation.Length = 2;

                data.Add(CustomCalculation.GetHex("B")[0] + CustomCalculation.GetHex("M")[0]);//ok

                CustomCalculation.Length = 8;
                int paddingCount = width * 3 % 4;
                string temp = CustomCalculation.GetHex(54 + width * height * 3 + height * paddingCount);
                data.Add(CustomString.ReverseGroup(temp, 2));
                //ok
                CustomCalculation.Length = 4;
                string[] tempArr = CustomCalculation.GetHex(new int[] { 0, 0 });
                foreach (string s in tempArr)
                {
                    data.Add(CustomString.ReverseGroup(s, 2));
                }
                //ok
                CustomCalculation.Length = 8;
                tempArr = CustomCalculation.GetHex(new int[] { 54, 40, width, height });
                foreach (string s in tempArr)
                {
                    data.Add(CustomString.ReverseGroup(s, 2));
                }

                CustomCalculation.Length = 4;
                tempArr = CustomCalculation.GetHex(new int[] { 1, 24 });
                foreach (string s in tempArr)
                {
                    data.Add(CustomString.ReverseGroup(s, 2));
                }

                CustomCalculation.Length = 8;
                tempArr = CustomCalculation.GetHex(new int[] { 0, 16, 2835, 2835, 0, 0 });
                foreach (string s in tempArr)
                {
                    data.Add(CustomString.ReverseGroup(s, 2));
                }

                return data.ToArray();
            }
            public static string[] CreateColorTransition(Color[] from, Color[] to, int colorSize, int step)
            {
                List<string> data = new List<string>();
                string[] header = CreateBMPHeader(from.Length * colorSize, (step + 2) * colorSize);
                foreach (string s in header)
                {
                    data.Add(s);
                }
                string[] palette = GenerateColorMatrix(from.Length * colorSize, GenerateColorTransition(from, to, step, colorSize));
                foreach (string s in palette)
                {
                    data.Add(s);
                }
                return data.ToArray();
            }
            public static string[] CreateColorVariants(Color resource, int width, int height, int colorSize)
            {
                List<string> data = new List<string>();
                string[] header = CreateBMPHeader((width + 1) * colorSize, (height + 1) * colorSize);
                foreach (string s in header)
                {
                    data.Add(s);
                }
                string[] palette = GenerateColorMatrix((width + 1) * colorSize, GenerateColorVariants(resource, width, height, colorSize));
                foreach (string s in palette)
                {
                    data.Add(s);
                }
                return data.ToArray();
            }
            public static string[] CreateMidColorTable(Color[] colors, int colorSize)
            {
                List<string> data = new List<string>();
                string[] header = CreateBMPHeader((colors.Length + 1) * colorSize, (colors.Length + 1) * colorSize);
                foreach (string s in header)
                {
                    data.Add(s);
                }
                string[] palette = GenerateColorMatrix((colors.Length + 1) * colorSize, GenerateMidColorTable(colors, colorSize));
                foreach (string s in palette)
                {
                    data.Add(s);
                }
                return data.ToArray();
            }
            public static string[] CreateDirectColor(Color[] colors, int width, int height)
            {
                if (colors.Length != width * height) return null;
                else
                {
                    List<string> data = new List<string>();
                    string[] header = CreateBMPHeader(width, height);
                    foreach (string s in header) data.Add(s);
                    string[] colorData = GenerateColorMatrix(width, colors);
                    foreach (string s in colorData) data.Add(s);
                    return data.ToArray();
                }
            }
            public static string[] CreateMidColorTable(Color[] a, Color[] b, int colorSize)
            {
                List<string> data = new List<string>();
                string[] header = CreateBMPHeader((a.Length + 1) * colorSize, (b.Length + 1) * colorSize);
                foreach (string s in header)
                {
                    data.Add(s);
                }
                string[] palette = GenerateColorMatrix((a.Length + 1) * colorSize, GenerateMidColorTable(a, b, colorSize));
                foreach (string s in palette)
                {
                    data.Add(s);
                }
                return data.ToArray();
            }
            public static string[] CreatePalette(Color[,] colors, int scaleX = 10, int scaleY = 10)
            {
                Color[] buffer = ToSingleDimension(ScaleColorArray(colors, scaleX, scaleY));
                List<string> data = new List<string>();
                string[] header = CreateBMPHeader(colors.GetLength(0) * scaleX, colors.GetLength(1) * scaleY);
                foreach (string s in header)
                {
                    data.Add(s);
                }
                string[] palette = GenerateColorMatrix(colors.GetLength(0) * scaleX, buffer);
                foreach (string s in palette)
                {
                    data.Add(s);
                }
                return data.ToArray();
            }
        }
        public static class PNG
        {
            public static List<string> AnalyzeIDATHex(string filePath)
            {
                if (filePath == null || !filePath.EndsWith(".png") || !File.Exists(filePath)) return null;
                List<string> idat = new List<string>();
                byte[] data = File.ReadAllBytes(filePath);
                for (int i = 4; i < data.Length - 4; i++)
                {
                    if (data[i] == (byte)'I' && data[i + 1] == (byte)'D' && data[i + 2] == (byte)'A' && data[i + 3] == (byte)'T')
                    {
                        string temp = null;
                        for(int j = i - 4; j < i; j++)
                        {
                            temp += data[j].ToString("X2");
                        }
                        int chunkLength = int.Parse(temp, NumberStyles.HexNumber);
                        Console.WriteLine(temp);
                        Console.WriteLine(chunkLength);
                        for (int j = i - 4; j < i + chunkLength + 4; j++)
                        {
                            idat.Add(data[j].ToString("X2"));
                        }
                        break;
                    }
                }
                return idat;
            }
            public static List<byte> AnalyzeIDATByte(string filePath)
            {
                if (filePath == null || !filePath.EndsWith(".png") || !File.Exists(filePath)) return null;
                List<byte> idat = new List<byte>();
                byte[] data = File.ReadAllBytes(filePath);
                for (int i = 4; i < data.Length - 4; i++)
                {
                    if (data[i] == (byte)'I' && data[i + 1] == (byte)'D' && data[i + 2] == (byte)'A' && data[i + 3] == (byte)'T')
                    {
                        string temp = null;
                        for(int j = i - 4; j < i; j++)
                        {
                            temp += data[j].ToString("X2");
                        }
                        int chunkLength = int.Parse(temp, NumberStyles.HexNumber);
                        Console.WriteLine(temp);
                        Console.WriteLine(chunkLength);
                        for (int j = i - 4; j < i + chunkLength + 4; j++)
                        {
                            idat.Add(data[j]);
                        }
                        break;
                    }
                }
                return idat;
            }
            public enum ColorType
            {
                Grayscale = 0, TrueColor = 2, Indexed = 3, GrayscaleAndAlpha = 4, TrueColorAndAlpha = 6
            }
            public static string[] CreatePNGHeader()
            {
                List<string> data = new List<string>();
                CustomCalculation.Length = 2;
                data.Add("89");
                foreach (char c in "PNG")
                {
                    data.Add(CustomCalculation.GetHex(c));
                }
                data.Add("0D");
                data.Add("0A");
                data.Add("1A");
                data.Add("0A");
                return data.ToArray();
            }
            public static string[] CreateIHDRChunk(int width, int height, int bitDepth, ColorType colorType = ColorType.TrueColorAndAlpha, bool interlaced = false)
            {
                if (!CheckColorFormat(bitDepth * GetColorChannels(colorType), colorType))
                {
                    throw new Exception("Color format not supported.");
                }
                List<string> data = new List<string>();
                CustomCalculation.Length = 8;
                foreach (string s in CustomString.Group(CustomCalculation.GetHex(13), 2))
                {
                    data.Add(s);
                }
                CustomCalculation.Length = 2;
                foreach (char c in "IHDR")
                {
                    data.Add(CustomCalculation.GetHex(c));
                }
                CustomCalculation.Length = 8;
                foreach (string s in CustomString.Group(CustomCalculation.GetHex(width), 2))
                {
                    data.Add(s);
                }
                foreach (string s in CustomString.Group(CustomCalculation.GetHex(height), 2))
                {
                    data.Add(s);
                }
                CustomCalculation.Length = 2;
                data.Add(CustomCalculation.GetHex(bitDepth));
                data.Add(CustomCalculation.GetHex(GetColorTypeIndex(colorType)));
                data.Add(CustomCalculation.GetHex(0));
                data.Add(CustomCalculation.GetHex(0));
                data.Add(CustomCalculation.GetHex(interlaced ? 1 : 0));
                data.Add("90");
                data.Add("77");
                data.Add("53");
                data.Add("DE");
                return data.ToArray();
            }
            public static int GetColorTypeIndex(ColorType colorType)
            {
                switch (colorType)
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
                switch (colorType)
                {
                    case ColorType.Grayscale: return 1;
                    case ColorType.GrayscaleAndAlpha: return 2;
                    case ColorType.Indexed: return 1;
                    case ColorType.TrueColor: return 3;
                    case ColorType.TrueColorAndAlpha: return 4;
                }
                return 0;
            }
            public static bool CheckColorFormat(int bitsPerChannel, ColorType colorType)
            {
                switch (colorType)
                {
                    case ColorType.Indexed:
                        if (bitsPerChannel == 1 || bitsPerChannel == 2 || bitsPerChannel == 4 || bitsPerChannel == 8)
                        {
                            return true;
                        }
                        break;
                    case ColorType.Grayscale:
                        if (bitsPerChannel == 1 || bitsPerChannel == 2 || bitsPerChannel == 4 || bitsPerChannel == 8 || bitsPerChannel == 16)
                        {
                            return true;
                        }
                        break;
                    case ColorType.GrayscaleAndAlpha:
                        if (bitsPerChannel == 32 || bitsPerChannel == 16)
                        {
                            return true;
                        }
                        break;
                    case ColorType.TrueColor:
                        if (bitsPerChannel == 24 || bitsPerChannel == 48)
                        {
                            return true;
                        }
                        break;
                    case ColorType.TrueColorAndAlpha:
                        if (bitsPerChannel == 32 || bitsPerChannel == 64)
                        {
                            return true;
                        }
                        break;
                }
                return false;
            }

        }
        public static Color[] GenerateColorArray(int width, int height, Func<byte, Color> Formula)
        {
            List<Color> colors = new List<Color>();
            for (int i = 0, k = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++, k++)
                {
                    colors.Add(Formula((byte)(Math.Min(k, 255))));
                }
            }

            return colors.ToArray();
        }
        public static Color[] ModifyColorArray(Color[] colors, int width, Func<byte, Color> Formula)
        {
            Color[] newColors = colors;
            for (int i = 0; i < newColors.Length / width; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    newColors[i + j] = Formula((byte)(i + j));
                }
            }
            return newColors;
        }
        public static Color[] ModifyColorArray(Color[] colors, int width, Func<byte, byte, Color> Formula)
        {
            Color[] newColors = colors;
            int height = newColors.Length / width;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    newColors[i * height + j] = Formula((byte)i, (byte)j);
                }
            }
            return newColors;
        }
        public static Color[] GenerateColorPalette(Color[] colors, int paletteWidth, int paletteHeight, int colorSize)
        {
            if (colors.Length > paletteWidth * paletteHeight)
            {
                throw new Exception("Palette size can not be smaller than total number of colors");
            }
            else
            {
                Color[,] palette = new Color[paletteWidth, paletteHeight];
                for (int i = 0, k = 0; i < paletteWidth; i++)
                {
                    for (int j = 0; j < paletteHeight; j++, k++)
                    {
                        if (k < colors.Length) palette[i, j] = colors[k];
                        else break;
                    }
                }
                Color[,] temp = new Color[paletteWidth * colorSize, paletteHeight * colorSize];
                for (int i = 0; i < temp.GetLength(0); i++)
                {
                    for (int j = 0; j < temp.GetLength(1); j++)
                    {
                        int a = i / colorSize, b = j / colorSize;
                        temp[i, j] = palette[a, b];
                    }
                }
                Color[] result = new Color[temp.GetLength(0) * temp.GetLength(1)];
                for (int i = 0, k = 0; i < temp.GetLength(0); i++)
                {
                    for (int j = 0; j < temp.GetLength(1); j++, k++)
                    {
                        result[j * paletteWidth * colorSize + i] = temp[i, j];
                    }
                }
                return result;
            }
        }
        public static Color[] GenerateColorTransition(Color[] from, Color[] to, int step, int colorSize)
        {
            if (from.Length != to.Length) throw new Exception("from and to's length have to be equal");
            else
            {
                step = Math.Max(step, 0);
                Color[,] palette = new Color[from.Length, step + 2];
                for (int i = 0; i < palette.GetLength(0); i++)
                {
                    for (int j = 1; j < palette.GetLength(1) - 1; j++)
                    {
                        byte r = (byte)(from[i].r + CustomCalculation.GoToValue(from[i].r, to[i].r) * j / step);
                        byte g = (byte)(from[i].g + CustomCalculation.GoToValue(from[i].g, to[i].g) * j / step);
                        byte b = (byte)(from[i].b + CustomCalculation.GoToValue(from[i].b, to[i].b) * j / step);
                        byte a = (byte)(from[i].a + CustomCalculation.GoToValue(from[i].a, to[i].a) * j / step);
                        palette[i, j] = new Color(r, g, b, a);
                    }
                    palette[i, step + 1] = to[i];
                    palette[i, 0] = from[i];
                }
                Color[,] temp = new Color[palette.GetLength(0) * colorSize, palette.GetLength(1) * colorSize];
                for (int i = 0; i < temp.GetLength(0); i++)
                {
                    for (int j = 0; j < temp.GetLength(1); j++)
                    {
                        temp[i, j] = palette[i / colorSize, j / colorSize];
                    }
                }
                Color[] result = new Color[temp.GetLength(0) * temp.GetLength(1)];
                for (int i = 0; i < temp.GetLength(0); i++)
                {
                    for (int j = 0; j < temp.GetLength(1); j++)
                    {
                        result[j * from.Length * colorSize + i] = temp[i, j];
                    }
                }
                return result;
            }
        }
        public static Color[] GenerateColorVariants(Color source, int width, int height, int colorSize)
        {
            Color[,] palette = new Color[width + 1, height + 1];
            for (int i = 0; i < width + 1; i++)
            {
                byte r = (byte)(source.r + CustomCalculation.GoToValue(source.r, Color.black.r) * i / width);
                byte g = (byte)(source.g + CustomCalculation.GoToValue(source.g, Color.black.g) * i / width);
                byte b = (byte)(source.b + CustomCalculation.GoToValue(source.b, Color.black.b) * i / width);
                byte a = (byte)(source.a + CustomCalculation.GoToValue(source.a, Color.black.a) * i / width);
                palette[i, 0] = new Color(r, g, b, a);
            }
            for (int i = 0; i < height + 1; i++)
            {
                byte r = (byte)(source.r + CustomCalculation.GoToValue(source.r, Color.white.r) * i / width);
                byte g = (byte)(source.g + CustomCalculation.GoToValue(source.g, Color.white.g) * i / width);
                byte b = (byte)(source.b + CustomCalculation.GoToValue(source.b, Color.white.b) * i / width);
                byte a = (byte)(source.a + CustomCalculation.GoToValue(source.a, Color.white.a) * i / width);
                palette[0, i] = new Color(r, g, b, a);
            }
            for (int i = 1; i < width + 1; i++)
            {
                for (int j = 1; j < height + 1; j++)
                {
                    palette[i, j] = palette[i, 0].GetMidColor(palette[0, j]);
                }
            }
            Color[,] temp = new Color[palette.GetLength(0) * colorSize, palette.GetLength(1) * colorSize];
            for (int i = 0; i < temp.GetLength(0); i++)
            {
                for (int j = 0; j < temp.GetLength(1); j++)
                {
                    temp[i, j] = palette[i / colorSize, j / colorSize];
                }
            }
            Color[] result = new Color[temp.GetLength(0) * temp.GetLength(1)];
            for (int i = 0; i < temp.GetLength(0); i++)
            {
                for (int j = 0; j < temp.GetLength(1); j++)
                {
                    result[j * (width + 1) * colorSize + i] = temp[i, j];
                }
            }
            return result;
        }
        public static Color[] GenerateMidColorTable(Color[] colors, int colorSize)
        {
            Color[,] palette = new Color[colors.Length + 1, colors.Length + 1];
            for (int i = 1; i < colors.Length + 1; i++)
            {
                palette[i, 0] = colors[i - 1];
                palette[0, i] = colors[i - 1];
            }
            for (int i = 1; i < palette.GetLength(0); i++)
            {
                for (int j = 1; j < palette.GetLength(1); j++)
                {
                    palette[i, j] = palette[i, 0].GetMidColor(palette[0, j]);
                }
            }
            Color[,] temp = new Color[palette.GetLength(0) * colorSize, palette.GetLength(1) * colorSize];
            for (int i = 0; i < temp.GetLength(0); i++)
            {
                for (int j = 0; j < temp.GetLength(1); j++)
                {
                    temp[i, j] = palette[i / colorSize, j / colorSize];
                }
            }
            Color[] result = new Color[temp.GetLength(0) * temp.GetLength(1)];
            for (int i = 0; i < temp.GetLength(0); i++)
            {
                for (int j = 0; j < temp.GetLength(1); j++)
                {
                    result[temp.GetLength(1) * j + i] = temp[i, j];
                }
            }
            return result;
        }
        public static Color[] GenerateMidColorTable(Color[] a, Color[] b, int colorSize)
        {
            Color[,] palette = new Color[a.Length + 1, b.Length + 1];
            for (int i = 1; i < a.Length + 1; i++)
            {
                palette[i, 0] = a[i - 1];
            }
            for (int i = 1; i < b.Length + 1; i++)
            {
                palette[0, i] = b[i - 1];
            }
            for (int i = 1; i < palette.GetLength(0); i++)
            {
                for (int j = 1; j < palette.GetLength(1); j++)
                {
                    palette[i, j] = palette[i, 0].GetMidColor(palette[0, j]);
                }
            }
            Color[,] temp = new Color[palette.GetLength(0) * colorSize, palette.GetLength(1) * colorSize];
            for (int i = 0; i < temp.GetLength(0); i++)
            {
                for (int j = 0; j < temp.GetLength(1); j++)
                {
                    temp[i, j] = palette[i / colorSize, j / colorSize];
                }
            }
            Color[] result = new Color[temp.GetLength(0) * temp.GetLength(1)];
            for (int i = 0; i < temp.GetLength(0); i++)
            {
                for (int j = 0; j < temp.GetLength(1); j++)
                {
                    result[temp.GetLength(0) * j + i] = temp[i, j];
                }
            }
            return result;
        }
        public static Color[,] GenerateMidColorTable(Color[] a, Color[] b)
        {
            Color[,] palette = new Color[a.Length + 1, b.Length + 1];
            for (int i = 1; i < a.Length + 1; i++)
            {
                palette[i, 0] = a[i - 1];
            }
            for (int i = 1; i < b.Length + 1; i++)
            {
                palette[0, i] = b[i - 1];
            }
            for (int i = 1; i < palette.GetLength(0); i++)
            {
                for (int j = 1; j < palette.GetLength(1); j++)
                {
                    palette[i, j] = palette[i, 0].GetMidColor(palette[0, j]);
                }
            }
            return palette;
        }
        public static Color[,] ScaleColorArray(Color[,] resource, int scaleX = 10, int scaleY = 10)
        {
            Color[,] result = new Color[resource.GetLength(0) * scaleX, resource.GetLength(1) * scaleY];
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = resource[i / scaleX, j / scaleY];
                }
            }
            return result;
        }
        public static Color[] ToSingleDimension(Color[,] resource)
        {
            Color[] result = new Color[resource.GetLength(0) * resource.GetLength(1)];
            for (int i = 0; i < resource.GetLength(0); i++)
            {
                for (int j = 0; j < resource.GetLength(1); j++)
                {
                    result[j * resource.GetLength(0) + i] = resource[i, j];
                }
            }
            return result;
        }
    }
}
