using TahsinsLibrary.Collections;
using TahsinsLibrary.Analyze;
using System;
using System.Collections.Generic;
namespace TahsinsLibrary.Geo
{
    public sealed class Biome
    {
        public string name
        {
            get; private set;
        }
        public Color color
        {
            get; private set;
        }
        public bool overPlacable
        {
            get; private set;
        }
        public bool canPlaceCountry
        {
            get; private set;
        }
        private Biome() { }
        public Biome(string name, Color color, bool overPlacable, bool canPlaceCountry)
        {
            this.name = name;
            this.color = color;
            this.overPlacable = overPlacable;
            this.canPlaceCountry = canPlaceCountry;
        }
        public static bool CanPlaceover(Biome biome)
        {
            return biome == null || biome.overPlacable;
        }
    }
    public class GeoMap : IExportable, IColorTurnable
    {
        private Random random;
        public readonly Biome blank = new Biome("blank", new Color(), true, false);
        public Biome defaultBiome;
        internal Biome[,] biomes;
        public int width => biomes.GetLength(0);
        public int height => biomes.GetLength(1);

        public string name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<Parcel> parcels = new List<Parcel>();
        private bool Fill(Frequency<Biome> biome, Parcel parcel, bool[,] checkTable)
        {
            if (!IsReadyToFill(biome, parcel, checkTable)) return false;

            return true;
        }
        private bool Fill(Frequency<Biome> biome, int x, int y, bool[,] checkTable)
        {
            int decised = (int)MathF.Min(MathF.Min(biome.min, biomes.Length), biome.decised);
            if (!IsReadyToFill(biome, x, y, checkTable)) return false;
            List<(int, int)> buffer = new List<(int, int)>();
            buffer.Add((x, y));
            buffer.Add((x, y));
            if (x > 0 && y > 0 && x < biomes.GetLength(0) - 1 && y < biomes.GetLength(1) - 1)
            {
                if ((biomes[x - 1, y] == null || biomes[x - 1, y].overPlacable) && !buffer.Contains((x - 1, y))) buffer.Add((x - 1, y));
                if ((biomes[x + 1, y] == null || biomes[x + 1, y].overPlacable) && !buffer.Contains((x + 1, y))) buffer.Add((x + 1, y));
                if ((biomes[x, y - 1] == null || biomes[x, y - 1].overPlacable) && !buffer.Contains((x, y - 1))) buffer.Add((x, y - 1));
                if ((biomes[x, y + 1] == null || biomes[x, y + 1].overPlacable) && !buffer.Contains((x, y + 1))) buffer.Add((x, y + 1));
            }
            for (int i = 0; i < decised; i++)
            {
                int selected = random.Next(0, buffer.Count);
                (int, int) indexes = buffer[selected];
                x = indexes.Item1; y = indexes.Item2;
                buffer.RemoveAt(selected);
                biomes[x, y] = biome.item;
                checkTable[x, y] = biome.item.overPlacable;
                if (x > 0 && y > 0 && x < biomes.GetLength(0) - 1 && y < biomes.GetLength(1) - 1)
                {
                    if ((biomes[x - 1, y] == null || biomes[x - 1, y].overPlacable) && !buffer.Contains((x - 1, y))) buffer.Add((x - 1, y));
                    if ((biomes[x + 1, y] == null || biomes[x + 1, y].overPlacable) && !buffer.Contains((x + 1, y))) buffer.Add((x + 1, y));
                    if ((biomes[x, y - 1] == null || biomes[x, y - 1].overPlacable) && !buffer.Contains((x, y - 1))) buffer.Add((x, y - 1));
                    if ((biomes[x, y + 1] == null || biomes[x, y + 1].overPlacable) && !buffer.Contains((x, y + 1))) buffer.Add((x, y + 1));
                }
            }
            return true;
        }
        private bool IsReadyToFill(Frequency<Biome> biome, Parcel parcel, bool[,] checkTable)
        {
            return parcel.points >= MathF.Min(biome.decised, biome.min);
        }
        private bool IsReadyToFill(Frequency<Biome> biome, int x, int y, bool[,] checkTable)
        {
            bool posC = x > 0 && y > 0 && x < biomes.GetLength(0) && y < biomes.GetLength(1);
            bool suiC = Biome.CanPlaceover(biomes[x, y]) && Analyze.AnalyzeF.CheckAdjenctivty<bool>(true, x, y, checkTable).Count >= MathF.Min(biome.decised, biome.min);
            return posC && suiC;
        }
        private void FirstScan(bool[,] checkTable, int x = 0, int y = 0)
        {
            parcels.Add(new Parcel(this, checkTable, x, y));
        }
        private void Scan(bool[,] checkTable)
        {
            foreach (Parcel parcel in parcels)
            {
                while (!parcel.IsWhole(checkTable))
                {
                    parcels.Add(parcel.Split(checkTable));
                }
            }
        }

        public void Export(string path, string name)
        {
            throw new NotImplementedException();
        }

        public bool IsReadyToExport()
        {
            throw new NotImplementedException();
        }

        public Color[] TurnColor()
        {
            throw new NotImplementedException();
        }
    }
    public sealed class Parcel
    {
        private Random generator;
        public (int, int) randomPoint
        {
            get
            {
                int selected = generator.Next(0, indexes.Count);
                (int, int) result = indexes[selected];
                indexes.RemoveAt(selected);
                return result;
            }
        }
        public int points => indexes.Count;
        private GeoMap source;
        private List<(int, int)> indexes = new List<(int, int)>();
        private List<(int, int)> edges = new List<(int, int)>();
        private Parcel()
        {
            generator = new Random();
        }
        public Parcel(GeoMap source)
        {
            this.source = source;
            generator = new Random();
        }
        public Parcel(GeoMap source, bool[,] checkTable, int x = 0, int y = 0)
        {
            this.source = source;
            Scan(x, y, checkTable);
            generator = new Random();
        }
        public void Scan(int x, int y, bool[,] checkTable)
        {
            if (x > -1 && y > -1 && x < source.width && y < source.height)
            {
                foreach ((int, int) i in Analyze.AnalyzeF.CheckAdjenctivty<bool>(true, x, y, checkTable))
                {
                    indexes.Add(i);
                }
            }
        }
        public Parcel Split(bool[,] checkTable)
        {
            Random random = new Random();
            Parcel parcel = new Parcel();
            parcel.source = source;
            List<(int, int)> temp = Analyze.AnalyzeF.CheckAdjenctivty<bool>(true, random.Next() % source.width, random.Next() % source.height, checkTable);
            foreach ((int, int) i in temp)
            {
                temp.Remove(i);
            }
            parcel.indexes = temp;
            return parcel;
        }
        public bool IsWhole(bool[,] checkTable)
        {
            Random random = new Random();
            return indexes.Count == AnalyzeF.CheckAdjenctivty<bool>(true, random.Next() % source.width, random.Next() % source.height, checkTable).Count;
        }
        public List<(int, int)> GetPoints(bool[,] checkTable, int amount)
        {
            throw new Exception();
        }
    }
}