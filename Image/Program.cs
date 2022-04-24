using System;
using System.Collections.Generic;
using TahsinsLibrary.Analyze;
using TahsinsLibrary.Collections;
using TahsinsLibrary;
using System.Diagnostics;
using TahsinsLibrary.Calculation;
using TahsinsLibrary.Array;
using TahsinsLibrary.String;
using System.IO;
namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            string current = Directory.GetCurrentDirectory();
            Color a = Color.random;
            Color b = Color.random;
            Color c = Color.random;
            List<(byte[], string)> datas = new List<(byte[], string)>();
            datas.Add((Image.BMP.GetMidColorData(new Color[] { a, b, c }, new Color[] { a, b, c }, 10, 10), "MidColor"));
            datas.Add((Image.BMP.GetTransitionData(new Color[] { a, b, c }, new Color[] { c, a, b }, 5), "Transition"));
            datas.Add((Image.BMP.GetVariantData(a, 10, 10), "Variants"));
            datas.Add((Image.BMP.GetPaletteData(new Color[,] { { a, b, c } }), "Palette"));
            foreach ((byte[], string) data in datas)
            {
                if (File.Exists(current + $"//{data.Item2}.bmp")) continue;
                FileStream fs = new FileStream(current + $"//{data.Item2}.bmp", FileMode.CreateNew);
                fs.Write(data.Item1);
                fs.Flush();
                fs.Close();
            }
        }
    }
}
