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
using TahsinsLibrary.Geometry;
namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new();
            const int w = 500, h = 500;
            Color[,] colorData = new Color[w, h];
            Line2D line = new();
            line.from = new(123, 123);
            line.to = new(100, 312);
            Color color = Color.random;
            foreach (Vector2D v in line.GetPoints())
            {
                Console.WriteLine(v);
                colorData[(int)v.x, (int)v.y] = color;
            }
            byte[] data = Image.BMP.GetArray(colorData);
            FileStream fs = new("test.bmp", FileMode.Create);
            fs.Write(data);
            fs.Flush();
            fs.Close();
        }
    }
}
