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
            const int w = 400, h = 400;
            FreePolygon2D polygon = new();
            Random random = new Random();
            List<Vector2D> list = new();
            for (int i = 0; i < random.Next(5, 11); i++)
            {
                list.Add(new(random.Next(0, w), random.Next(0, h)));
            }
            polygon.vertices = list.ToArray();
            polygon.SolveComplexity();
            polygon.SortVertices();
            Vector2D[] points = polygon.GetPointsInside();

            Color[,] colorData = new Color[w, h];

            Color color = Color.GetColorFromLibrary("orange");

            foreach (Vector2D point in points)
            {
                colorData[(int)point.x, (int)point.y] = color;
            }
            Color negative = color.negative;
            foreach (Vector2D point in polygon.vertices)
            {
                colorData[(int)point.x, (int)point.y] = negative;
            }
            FileStream fs = new FileStream("test.bmp", FileMode.Create);
            fs.Write(Image.BMP.GetArray(colorData));
            fs.Flush();
            fs.Close();
        }
    }
}
