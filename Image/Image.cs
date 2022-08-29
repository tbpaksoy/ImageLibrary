using System;
using System.Collections.Generic;
using TahsinsLibrary.Calculation;
using TahsinsLibrary.String;
using System.IO;
using TahsinsLibrary.Analyze;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace TahsinsLibrary
{
    public interface IColorTurnable
    {
        public Color[,] TurnColor();
    }
    public interface IExportable
    {
        public string name { get; set; }
        public void Export(string path, string name);
        public bool IsReadyToExport();
    }
    public struct Color
    {
        [JsonInclude]
        public byte r, g, b, a;
        public static Color random
        {
            get
            {
                Random r = new Random();
                return new(r.Next() % 256, r.Next() % 256, r.Next() % 256);
            }
        }
        [JsonIgnore]
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
        [JsonIgnore]
        public Color negative
        {
            get
            {
                return new Color(255 - r, 255 - g, 255 - b, a);
            }
        }
        [JsonIgnore]
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
        public static Color GetMidColor(Color a, Color b)
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
            foreach (string file in Directory.EnumerateFiles(@$"{Directory.GetCurrentDirectory()}\Colors"))
            {
                if (file.EndsWith("json"))
                {
                    Dictionary<string, Color> dic = JsonSerializer.Deserialize<Dictionary<string, Color>>(File.OpenRead(file));
                    if (dic.ContainsKey(name)) return dic[name];
                }
            }
            return new Color();
        }
        public static void SaveColorToLibrary(string fileName, string colorName, Color color)
        {
            string s = @$"{Directory.GetCurrentDirectory()}\Colors\{fileName}.json";
            bool isNew = !File.Exists(s);
            if (isNew)
            {
                File.Create(s).Close();
                File.WriteAllText(s, "{\n}");
            }
            string content = File.ReadAllText(s);
            content = content.Substring(1, content.Length - 2);
            string newContent = "\u0022" + colorName + "\u0022" + ":" + JsonSerializer.Serialize(color) + "\n";
            File.WriteAllText(s, "{" + content + (string.IsNullOrWhiteSpace(content) && isNew ? null : ",") + "\n" + newContent + "}");
        }
        public static Color FromHex(string hex)
        {
            Color color = new Color();
            if (hex.Length == 8) color.hex = hex;
            return color;
        }
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
        public Color[,] albedo;
        public byte[,] metallic;
        public byte[,] smoothness;
        public Color[,] emission;
        public int paletteSize;
        public MaterialForUnity(string name, Color[,] albedo, byte[,] metallic, byte[,] smoothness, Color[,] emission, int paletteSize)
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
            byte[] data = Image.BMP.GetPaletteData(albedo, paletteSize, paletteSize);
            fs.Write(data);
            fs.Flush();
            fs.Close();
            fs = new FileStream($"{path}\\{name}Metallic.bmp", FileMode.Create);
            data = Image.BMP.GetPaletteData(Image.ToGreyScale(metallic), paletteSize, paletteSize);
            fs.Write(data);
            fs.Flush();
            fs.Close();
            fs = new FileStream($"{path}\\{name}Smoothness.bmp", FileMode.Create);
            data = Image.BMP.GetPaletteData(Image.ToGreyScale(smoothness), paletteSize, paletteSize);
            fs.Write(data);
            fs.Flush();
            fs.Close();
            fs = new FileStream($"{path}\\{name}Emission.bmp", FileMode.Create);
            data = Image.BMP.GetPaletteData(emission, paletteSize, paletteSize);
            fs.Write(data);
            fs.Flush();
            fs.Close();
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
        public Color[,] baseColor;
        public byte[,] metallic;
        public byte[,] specular;
        public byte[,] roughness;
        public Color[,] emission;
        public int paletteSize;
        public bool IsReadyToExport()
        {
            if (paletteSize < 1) return false;
            return true;
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
                byte[] data = Image.BMP.GetPaletteData(baseColor);
                fs.Write(data);
                fs.Flush();
                fs.Close();
            }
            if (metallic != null)
            {
                FileStream fs = new FileStream($"{path}\\{name}Metallic.bmp", FileMode.Create);
                byte[] data = Image.BMP.GetPaletteData(Image.ToGreyScale(metallic));
                fs.Write(data);
                fs.Flush();
                fs.Close();
            }
            if (roughness != null)
            {
                FileStream fs = new FileStream($"{path}\\{name}Roughness.bmp", FileMode.Create);
                byte[] data = Image.BMP.GetPaletteData(Image.ToGreyScale(roughness));
                fs.Write(data);
                fs.Flush();
                fs.Close();
            }
            if (emission != null)
            {
                FileStream fs = new FileStream($"{path}\\{name}Emission.bmp", FileMode.Create);
                byte[] data = Image.BMP.GetPaletteData(emission);
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
            #region byte[]
            public static byte[] GetTransitionData(Color[] a, Color[] b, int step = 5, int scaleX = 10, int scaleY = 10)
            {
                Color[,] colorData = GenerateColorTransition(a, b, step);
                List<byte> data = new();
                foreach (byte by in GetBMPHeader(colorData))
                {
                    data.Add(by);
                }
                foreach (byte by in GetColorMatrix(colorData))
                {
                    data.Add(by);
                }
                return data.ToArray();
            }
            public static byte[] GetMidColorData(Color[] a, Color[] b, int scaleX = 10, int scaleY = 10)
            {
                Color[,] colorData = ScaleColorArray(GenerateMidColorTable(a, b), scaleX, scaleY);
                List<byte> data = new();
                foreach (byte by in GetBMPHeader(colorData))
                {
                    data.Add(by);
                }
                foreach (byte by in GetColorMatrix(colorData))
                {
                    data.Add(by);
                }
                return data.ToArray();
            }
            public static byte[] GetVariantData(Color color, int width = 9, int height = 9, int scaleX = 10, int scaleY = 10)
            {
                Color[,] colorData = ScaleColorArray(GenerateColorVariants(color, width, height), scaleX, scaleY);
                List<byte> data = new();
                foreach (byte b in GetBMPHeader(colorData))
                {
                    data.Add(b);
                }
                foreach (byte b in GetColorMatrix(colorData))
                {
                    data.Add(b);
                }
                return data.ToArray();
            }
            public static byte[] GetPaletteData(Color[,] palette, int scaleX = 10, int scaleY = 10)
            {
                Color[,] colorData = ScaleColorArray(palette, scaleX, scaleY);
                List<byte> data = new();
                foreach (byte b in GetBMPHeader(colorData))
                {
                    data.Add(b);
                }
                foreach (byte b in GetColorMatrix(colorData))
                {
                    data.Add(b);
                }
                return data.ToArray();
            }
            public static byte[] GetArray(Color[,] colors)
            {
                List<byte> data = new();
                foreach (byte b in GetBMPHeader(colors))
                {
                    data.Add(b);
                }
                foreach (byte b in GetColorMatrix(colors))
                {
                    data.Add(b);
                }
                return data.ToArray();
            }
            public static byte[] GetColorMatrix(Color[,] resource)
            {
                List<byte> data = new List<byte>();
                int padding = resource.GetLength(0) * 3 % 4;
                for (int i = 0; i < resource.GetLength(0); i++)
                {
                    for (int j = 0; j < resource.GetLength(1); j++)
                    {
                        data.Add(resource[i, j].b);
                        data.Add(resource[i, j].g);
                        data.Add(resource[i, j].r);
                    }
                    for (int j = 0; j < padding; j++)
                    {
                        data.Add(0);
                    }
                }
                return data.ToArray();
            }
            public static byte[] GetBMPHeader(Color[,] resource)
            {
                List<byte> data = new List<byte>();
                #region BMP Header
                data.Add((byte)'B');
                data.Add((byte)'M');

                int paddingCount = resource.GetLength(0) * 3 % 4;

                int value = 54 + resource.GetLength(0) * resource.GetLength(1) * 3 + resource.GetLength(1) * paddingCount;
                string s = value.ToString("X8");
                byte[] temp = new byte[s.Length / 2];


                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] = Convert.ToByte(s.Substring(i * 2, 2), 16);
                }
                System.Array.Reverse(temp);
                foreach (byte b in temp)
                {
                    data.Add(b);
                }

                for (int i = 0; i < 4; i++) data.Add(0);

                data.Add(54);
                data.Add(0);
                data.Add(0);
                data.Add(0);
                #endregion
                #region DIB Header
                data.Add(40);
                data.Add(0);
                data.Add(0);
                data.Add(0);

                value = resource.GetLength(0);
                s = value.ToString("X8");
                temp = new byte[s.Length / 2];
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] = Convert.ToByte(s.Substring(i * 2, 2), 16);
                }
                System.Array.Reverse(temp);
                foreach (byte b in temp)
                {
                    data.Add(b);
                }

                value = resource.GetLength(1);
                s = value.ToString("X8");
                temp = new byte[s.Length / 2];
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] = Convert.ToByte(s.Substring(i * 2, 2), 16);
                }
                System.Array.Reverse(temp);
                foreach (byte b in temp)
                {
                    data.Add(b);
                }

                data.Add(1);
                data.Add(0);

                data.Add(24);
                data.Add(0);

                data.Add(0);
                data.Add(0);
                data.Add(0);
                data.Add(0);

                value = (resource.GetLength(0) + paddingCount) * 3 * resource.GetLength(1);
                s = value.ToString("X8");
                temp = new byte[s.Length / 2];
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] = Convert.ToByte(s.Substring(i * 2, 2), 16);
                }
                System.Array.Reverse(temp);
                foreach (byte b in temp)
                {
                    data.Add(b);
                }

                value = 2835;
                s = value.ToString("X8");
                temp = new byte[s.Length / 2];
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] = Convert.ToByte(s.Substring(i * 2, 2), 16);
                }
                System.Array.Reverse(temp);
                foreach (byte b in temp)
                {
                    data.Add(b);
                }

                value = 2835;
                s = value.ToString("X8");
                temp = new byte[s.Length / 2];
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] = Convert.ToByte(s.Substring(i * 2, 2), 16);
                }
                System.Array.Reverse(temp);
                foreach (byte b in temp)
                {
                    data.Add(b);
                }

                data.Add(0);
                data.Add(0);
                data.Add(0);
                data.Add(0);

                data.Add(0);
                data.Add(0);
                data.Add(0);
                data.Add(0);


                #endregion
                return data.ToArray();
            }
            #endregion
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
                        for (int j = i - 4; j < i; j++)
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
                        for (int j = i - 4; j < i; j++)
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
        public static Color[,] GenerateColorVariants(Color source, int width, int height)
        {
            Color[,] palette = new Color[width + 1, height + 1];
            for (int i = 0; i < width + 1; i++)
            {
                byte r = (byte)(source.r + CustomCalculation.GoToValue(source.r, new Color(0, 0, 0, 255).r) * i / width);
                byte g = (byte)(source.g + CustomCalculation.GoToValue(source.g, new Color(0, 0, 0, 255).g) * i / width);
                byte b = (byte)(source.b + CustomCalculation.GoToValue(source.b, new Color(0, 0, 0, 255).b) * i / width);
                byte a = (byte)(source.a + CustomCalculation.GoToValue(source.a, new Color(0, 0, 0, 255).a) * i / width);
                palette[i, 0] = new Color(r, g, b, a);
            }
            for (int i = 0; i < height + 1; i++)
            {
                byte r = (byte)(source.r + CustomCalculation.GoToValue(source.r, new Color(255, 255, 255, 255).r) * i / width);
                byte g = (byte)(source.g + CustomCalculation.GoToValue(source.g, new Color(255, 255, 255, 255).g) * i / width);
                byte b = (byte)(source.b + CustomCalculation.GoToValue(source.b, new Color(255, 255, 255, 255).b) * i / width);
                byte a = (byte)(source.a + CustomCalculation.GoToValue(source.a, new Color(255, 255, 255, 255).a) * i / width);
                palette[0, i] = new Color(r, g, b, a);
            }
            for (int i = 1; i < width + 1; i++)
            {
                for (int j = 1; j < height + 1; j++)
                {
                    palette[i, j] = palette[i, 0].GetMidColor(palette[0, j]);
                }
            }
            Color[,] temp = new Color[palette.GetLength(0), palette.GetLength(1)];
            for (int i = 0; i < temp.GetLength(0); i++)
            {
                for (int j = 0; j < temp.GetLength(1); j++)
                {
                    temp[i, j] = palette[i, j];
                }
            }
            return temp;
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
        public static Color[,] GenerateColorTransition(Color[] from, Color[] to, int step)
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
                Color[,] temp = new Color[palette.GetLength(0), palette.GetLength(1)];
                for (int i = 0; i < temp.GetLength(0); i++)
                {
                    for (int j = 0; j < temp.GetLength(1); j++)
                    {
                        temp[i, j] = palette[i, j];
                    }
                }
                return temp;
            }
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
        public static Color[,] ToGreyScale(byte[,] reources)
        {
            Color[,] colorData = new Color[reources.GetLength(0), reources.GetLength(1)];
            for (int i = 0; i < colorData.GetLength(0); i++)
            {
                for (int j = 0; j < colorData.GetLength(1); j++)
                {
                    colorData[i, j] = new Color(reources[i, j], reources[i, j], reources[i, j], (byte)255);
                }
            }
            return colorData;
        }
    }
}
