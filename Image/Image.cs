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
        [JsonIgnore]
        public Color rMask => this with { g = b = this.r, a = 255 };
        [JsonIgnore]
        public Color gMask => this with { r = b = this.g, a = 255 };
        [JsonIgnore]
        public Color bMask => this with { r = g = this.b, a = 255 };
        [JsonIgnore]
        public Color aMask => this with { r = g = b = this.a, a = 255 };
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
            this.r = (byte)MathF.Max(MathF.Min(r, 255), 0);
            this.g = (byte)MathF.Max(MathF.Min(g, 255), 0);
            this.b = (byte)MathF.Max(MathF.Min(b, 255), 0);
            this.a = (byte)MathF.Max(MathF.Min(a, 255), 0);
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
        public static Color Interpolate(Color from, Color to, float t)
        {
            int r = (int)(from.r + (from.r - to.r) * t);
            int g = (int)(from.g + (from.g - to.g) * t);
            int b = (int)(from.b + (from.b - to.b) * t);
            int a = (int)(from.a + (from.a - to.a) * t);
            return new Color(r, g, b, a);
        }
        public static Color Mix(params Color[] colors)
        {
            int r, g, b, a;
            r = g = b = a = 0;
            foreach (Color color in colors)
            {
                r += color.r;
                g += color.g;
                b += color.b;
                a += color.a;
            }
            r /= colors.Length;
            g /= colors.Length;
            b /= colors.Length;
            a /= colors.Length;
            return new Color(r, g, b, a);
        }
        public static Color Mix(Color from, Color to, float value = 0.5f) => new Color(
            from.r + (int)((from.r - to.r) * value),
            from.g + (int)((from.g - to.g) * value),
            from.b + (int)((from.b - to.b) * value),
            from.a + (int)((from.a - to.a) * value));
        public static Color operator *(Color c, float v) => new Color((int)(c.r * v), (int)(c.g * v), (int)(c.b * v), c.a);
        public static Color operator +(Color c, int v) => new Color(c.r + v, c.g + v, c.b + v, c.a);
    }
    public struct CustomColorSpace
    {
        public Color from, to;
        public int rRange => to.r - from.r;
        public int gRange => to.g - from.g;
        public int bRange => to.b - from.b;
        public int aRange => to.a - from.a;
        public Color MoveTrough(float value = 0.5f) => new Color(
        (byte)(from.r + rRange * value),
        (byte)(from.g + gRange * value),
        (byte)(from.b + bRange * value),
        (byte)(from.a + aRange * value));
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
                int padding = resource.GetLength(0) * resource.GetLength(1) % 4;
                for (int i = 0; i < resource.GetLength(1); i++)
                {
                    for (int j = 0; j < resource.GetLength(0); j++)
                    {
                        data.Add(resource[j, i].b);
                        data.Add(resource[j, i].g);
                        data.Add(resource[j, i].r);
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
        public static Color[,] MirrorVertical(Color[,] source)
        {
            Color[,] result = new Color[source.GetLength(0), source.GetLength(1)];
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = source[i, result.GetLength(1) - j - 1];
                }
            }
            return result;
        }
        public static Color[,] Subversion(Color[,] source)
        {
            Color[,] result = new Color[source.GetLength(1), source.GetLength(0)];
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = source[j, i];
                }
            }
            return result;
        }
        public static Color[,] Contrast(Color[,] source, float value)
        {
            Color[,] result = source.Clone() as Color[,];
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] *= value;
                }
            }
            return result;
        }
        public static Color[,] Brightness(Color[,] source, int value)
        {
            Color[,] result = source.Clone() as Color[,];
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] += value;
                }
            }
            return result;
        }
        public static Color[,] Threshold(Color[,] source, Color threshold)
        {
            Color w = new Color(255, 255, 255, 255), b = new Color(0, 0, 0, 255);
            Color[,] result = new Color[source.GetLength(0), source.GetLength(1)];
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    Color temp = source[i, j];
                    result[i, j] = (temp.r >= threshold.r && temp.g >= threshold.g && temp.b >= threshold.b && temp.a >= threshold.a) ? w : b;
                }
            }
            return result;
        }
        public static Color[,] OvalStripes(int x, int y, float frequency = 1f)
        {
            Color[,] result = new Color[x, y];
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    int temp = (int)MathF.Abs((MathF.Sin(MathF.Sqrt(MathF.Pow(i - 0.5f, 2) + MathF.Pow(j - 0.5f, 2)))) * 255);
                    result[i, j] = new Color(temp, temp, temp, 255);
                }
            }
            return result;
        }
        public enum Format
        {
            BMP, PNG
        }
    }
    public static class Filter
    {
        public static Color[,] AntiAliase(Color[,] picture, float scale)
        {
            Color[,] result = new Color[picture.GetLength(0), picture.GetLength(1)];
            for (float i = 0.5f; i < result.GetLength(0) - 0.5f; i++)
            {
                for (float j = 0.5f; j < result.GetLength(1) - 0.5f; j++)
                {
                    int ri = (int)MathF.Round(i), rj = (int)MathF.Round(j);
                    float percent = (1f - MathF.Abs(i - ri)) * (1f - MathF.Abs(j - rj));
                    result[ri, rj] = picture[ri, rj] * percent;
                }
            }
            return result;
        }
    }
}
