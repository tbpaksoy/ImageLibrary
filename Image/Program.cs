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
class Program
{
    public static void Main(string[] args)
    {
        Biome land = new Biome("land",Color.green);
        Biome sea = new Biome("sea",Color.blue);
        GeoMap map = new GeoMap();
        Frequency<Biome> f1 = new Frequency<Biome>(land,59,60);
        Frequency<Biome> f2 = new Frequency<Biome>(sea,69,70);
        Frequency<Biome> f3 = new Frequency<Biome>(land,69,70);
        map.FeedBiome(new Frequency<Biome>[]{f1,f1,f2,f3,f2,f2},200,200,72);
        map.Export("C://Users//Tahsin//Desktop","tahsin");
    }
}

