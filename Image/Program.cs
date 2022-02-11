using System;
using System.Globalization;
using TahsinsLibrary.String;
using TahsinsLibrary.Calculation;
using System.Collections.Generic;
using System.IO;
using TahsinsLibrary;
using TahsinsLibrary.Analyze;
using System.Linq;
using TahsinsLibrary.Array;
using TahsinsLibrary.Collections;
using System.Diagnostics;
class Program
{
    public static void Main(string[] args)
    {

        /*
        Stopwatch stopwatch = Stopwatch.StartNew();
        Random random = new Random(28634);
        bool[,] bools = new bool[50, 50];
        for (int i = 0; i < 50; i++)
        {
            for (int j = 0; j < 50; j++)
            {
                bools[i, j] = random.Next() % 2 == 0;
            }
        }
        int x = random.Next(0, 50), y = random.Next(0, 50);
        List<(int, int)> asd = Analyze.CheckAdjenctivty<bool>(bools[x, y], x, y, bools);
        for (int i = 0; i < 50; i++)
        {
            for (int j = 0; j < 50; j++)
            {
                if (asd.Contains((i, j)))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write(bools[i, j].ToString()[0] + " ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;

                }
                else Console.Write(bools[i, j].ToString()[0] + " ");

            }
            Console.WriteLine();
        }
        stopwatch.Stop();
        Console.WriteLine(stopwatch.ElapsedMilliseconds);
        */

        Stopwatch stopwatch = Stopwatch.StartNew();
        Random random = new Random(9);
        Biome grass = new Biome("grass", Color.green);
        Biome sea = new Biome("sea", Color.blue);
        sea.overPlacable = true;
        GeoMap map = new GeoMap();
        Frequency<Biome> grassF = new Frequency<Biome>(grass, 10000, 400000);
        Frequency<Biome> sandF = new Frequency<Biome>(new Biome("sand", Color.sand), 25000, 100000);
        map.defaultBiome = sea;
        for (int i = 0; i < 1; i++)
        {
            List<Frequency<Biome>> frequencies = new List<Frequency<Biome>>();
            int iterations = random.Next(5, 31);
            for (int j = 0; j < iterations; j++)
            {
                if (random.Next() % 2 == 0) frequencies.Add(grassF);
                else frequencies.Add(sandF);
            }
            map.FeedBiome(frequencies.ToArray(), 2000, 2000, 3);
            map.Export("C://Users//Tahsin//Desktop//CGM", 1060.ToString());
        }
        stopwatch.Stop();
        Console.WriteLine(stopwatch.ElapsedMilliseconds);
    }

}


