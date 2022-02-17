using System;
using System.Collections.Generic;
using TahsinsLibrary.Analyze;
using TahsinsLibrary.Collections;
namespace Image
{
    class Program
    {
        static void Main(string[] args)
        {
            /*const int uv = 25;
            bool[,] bools = new bool[uv, uv];
            Random random = new Random();

            for (int i = 0; i < uv; i++)
            {
                for (int j = 0; j < uv; j++)
                {
                    bools[i, j] = random.Next() % 2 == 0;
                }
            }
            int x = random.Next(0, uv), y = random.Next(0, uv);
            List<(int, int)> ix = Analyze.EdgeDetect<bool>(bools[x, y], x, y, bools);
            List<(int, int)> jy = Analyze.CheckAdjenctivty<bool>(bools[x, y], x, y, bools);
            for (int i = 0; i < uv; i++)
            {
                for (int j = 0; j < uv; j++)
                {
                    if (ix.Contains((i, j)) && jy.Contains((i, j)))
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Write(bools[i, j].ToString()[0] + " ");
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else if (jy.Contains((i, j)) && !ix.Contains((i, j)))
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write(bools[i, j].ToString()[0] + " ");
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else Console.Write(bools[i, j].ToString()[0] + " ");
                }
                Console.WriteLine();
            }*/

            const int w = 500, h = 250;
            Biome sea = new Biome("sea", TahsinsLibrary.Color.blue, true, false);
            Biome land = new Biome("land", TahsinsLibrary.Color.green, false, true);
            Biome snow = new Biome("snowy", TahsinsLibrary.Color.white, false, true);
            PoliticalMap map = new PoliticalMap();
            int size = w * h;
            Frequency<Biome> landF = new Frequency<Biome>(land, size / 25, size / 10);
            Frequency<Biome> snowyF = new Frequency<Biome>(snow, size / 25, size / 10);
            map.defaultBiome = sea;
            map.FeedBiome(new Frequency<Biome>[] { landF, landF, landF, landF, landF, snowyF, snowyF }, w, h, 5);
            Country country = new Country("Red", TahsinsLibrary.Color.red);
            Frequency<Country> redF = new Frequency<Country>(country, size / 15, size / 10, 5);
            map.countries = new Frequency<Country>[] { redF, redF, redF };
            map.PlaceCountries(10);
            map.Export("C:\\Users\\Tahsin\\Desktop\\Image", "PM0");

        }
    }
}
