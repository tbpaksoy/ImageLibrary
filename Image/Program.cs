using System;
using System.Collections.Generic;
using TahsinsLibrary.Analyze;
using TahsinsLibrary.Collections;
using TahsinsLibrary;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool.GetMinThreads(out int w, out int c);
            ThreadPool.SetMinThreads(w, c);
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
            const int width = 400, height = 300;
            Biome sea = new Biome("sea", TahsinsLibrary.Color.blue, true, false);
            Biome land = new Biome("land", TahsinsLibrary.Color.green, false, true);
            Biome snow = new Biome("snowy", TahsinsLibrary.Color.white, false, true);
            Biome savannah = new Biome("savannah", new Color(139, 134, 8), false, true);
            Biome tundra = new Biome("tundra", new Color(155, 151, 133), false, true);
            PoliticalMap map = new PoliticalMap();
            int size = width * height;
            Frequency<Biome> landF = new Frequency<Biome>(land, size / 25, size / 10);
            Frequency<Biome> snowyF = new Frequency<Biome>(snow, size / 25, size / 10);
            Frequency<Biome> savannahF = new Frequency<Biome>(savannah, size / 100, size / 40);
            Frequency<Biome> tundraF = new Frequency<Biome>(tundra, size / 30, size / 15);
            map.defaultBiome = sea;
            map.FeedBiome(new Frequency<Biome>[] { landF, landF, landF, landF, landF, snowyF, snowyF, savannahF, tundraF }, width, height, 5);
            Country country = new Country("Red", TahsinsLibrary.Color.red);
            Country chorum = new Country("çorum", Color.purple);
            Country burdur = new Country("brdr", Color.silver);
            int freeLand = map.howManyFreeLandLeft;
            Frequency<Country> redF = new Frequency<Country>(country, freeLand / 8 / 2, freeLand / 4 / 2);
            Frequency<Country> chorumF = new Frequency<Country>(chorum, freeLand / 6 / 2, freeLand / 4 / 2);
            Frequency<Country> brdrF = new Frequency<Country>(burdur, freeLand / 10, freeLand / 8);
            map.FeedCountries(redF, redF, chorumF, brdrF);
            map.PlaceCountries(10);
            map.Export("C:\\Users\\Tahsin\\Desktop\\Image", "burdur");
            /*
            foreach (string s in TahsinsLibrary.Image.BMP.CreateBMPHeader(50, 50))
            {
                Console.WriteLine(s);
            }
            Console.WriteLine(TahsinsLibrary.Image.BMP.CreateBMPHeader(50, 50).Length);
            */
        }
    }
}
