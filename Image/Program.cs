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
            GeoMap map = new GeoMap();
            const int w = 100, h = 100;
            Frequency<Biome> b1 = new Frequency<Biome>(new Biome("Grass",Color.GetColorFromLibrary("green")),w*h/200, w*h/10);
            Frequency<Biome> b2 = new Frequency<Biome>(b1.item,w*h/100,w*h/50);
            map.defaultBiome = new Biome("Sea",Color.GetColorFromLibrary("blue"));
            map.FeedBiome(new Frequency<Biome>[]{b1,b1,b2},w,h,645);
            map.Export(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"deneme");
            Console.WriteLine("Done");
        }
    }
}
