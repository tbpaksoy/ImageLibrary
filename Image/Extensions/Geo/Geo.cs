using TahsinsLibrary;
using System;
using TahsinsLibrary.Collections;
using System.Collections.Generic;
using TahsinsLibrary.Analyze;
using System.IO;
using TahsinsLibrary.Calculation;
public class Biome
{
    public string name { get; private set; }
    public Color color { get; private set; }
    public bool overPlacable = false;
    public bool canPlaceCountry;
    private Biome() { }
    public Biome(string name, Color color)
    {
        this.name = name;
        this.color = color;
    }
    public Biome(string name, Color color, bool overPlacable, bool canPlaceCountry)
    {
        this.name = name;
        this.color = color;
        this.overPlacable = overPlacable;
        if(overPlacable) canPlaceCountry = false;
        else this.canPlaceCountry = canPlaceCountry;
    }
    public static bool IsOverWriteable(Biome biome)
    {
        return biome == null || biome.overPlacable;
    }
}
public class GeoMap : IExportable, IColorTurnable
{
    private Random random;
    public readonly Biome blank = new Biome("blank", Color.black);
    public Biome defaultBiome;
    public string name { get; set; }
    public Biome[,] biomes;
    private int iterationsLeft;
    private bool renewIteration;
    private List<(int, int)> buffer = new List<(int, int)>();
    private bool[,] emptiness;
    private Dictionary<Biome, int> seperator = new Dictionary<Biome, int>();
    
    public void FeedBiome(Frequency<Biome>[] frequencies, int width, int height, int seed)
    {
        Console.WriteLine("Feeding");
        random = new Random(seed);
        seperator.TryAdd(defaultBiome, 0);
        int seperatorIndex = 1;
        foreach (Frequency<Biome> f in frequencies)
        {
            f.DeciseCount(random.Next());
            if (!seperator.ContainsKey(f.item))
            {
                seperator.Add(f.item, seperatorIndex);
                seperatorIndex++;
            }
        }
        biomes = new Biome[width, height];
        emptiness = new bool[width, height];
        for (int i = 0; i < biomes.GetLength(0); i++)
        {
            for (int j = 0; j < biomes.GetLength(1); j++)
            {
                biomes[i, j] = defaultBiome;
                if (defaultBiome.overPlacable)
                {
                    emptiness[i, j] = true;
                }
            }
        }
        foreach (Frequency<Biome> f in frequencies)
        {
            int nullsLeft = HowManyWriteablesLeft();
            if (nullsLeft < f.min) break;
            else if (nullsLeft < f.decised) continue;
            int x = random.Next(0, biomes.GetLength(0)), y = random.Next(0, biomes.GetLength(1));
            while (!(biomes[x, y] != null || !biomes[x, y].overPlacable))
            {
                x = random.Next(0, biomes.GetLength(0)); y = random.Next(0, biomes.GetLength(1));
            }
            Fill(f, x, y);
            buffer.Clear();
            renewIteration = true;
        }
        Console.WriteLine("Smoothing");
        MedianSmooth(5, 5);
    }
    private void Smooth(int threshold)
    {
        bool[,] isChecked = new bool[biomes.GetLength(0), biomes.GetLength(1)];
        List<(int, int)> list = Analyze.CheckAdjenctivty<bool>(true, 0, 0, emptiness);
        foreach ((int, int) i in list) isChecked[i.Item1, i.Item2] = true;
        for (int i = 0; i < biomes.GetLength(0); i++)
        {
            for (int j = 0; j < biomes.GetLength(1); j++)
            {
                if (!emptiness[i, j] && !isChecked[i, j])
                {
                    list = Analyze.CheckAdjenctivty<bool>(true, i, j, emptiness);
                    if (list.Count < threshold)
                    {
                        foreach ((int, int) ii in list)
                        {
                            isChecked[ii.Item1, ii.Item2] = true;
                        }
                        foreach ((int, int) index in list)
                        {
                            emptiness[index.Item1, index.Item2] = false;
                            if (biomes[index.Item1, index.Item2].overPlacable)
                            {
                                if (index.Item1 > 0)
                                {
                                    biomes[index.Item1, index.Item2] = biomes[index.Item1 - 1, index.Item2];
                                }
                                else if (index.Item1 == 0)
                                {
                                    biomes[index.Item1, index.Item2] = biomes[index.Item1 + 1, index.Item2];
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    private void MedianSmooth(int width, int height)
    {
        Biome[] window = new Biome[width * height];
        Biome[,] output = new Biome[biomes.GetLength(0), biomes.GetLength(1)];
        int eX = (int)Math.Floor((float)width / 2f), eY = (int)Math.Floor((float)height / 2f);
        for (int x = eX; x < biomes.GetLength(0) - eX; x++)
        {
            for (int y = eY; y < biomes.GetLength(1) - eY; y++)
            {
                for (int a = 0, i = 0; a < width; a++)
                {
                    for (int b = 0; b < height; b++, i++)
                    {
                        window[i] = biomes[x + a - eX, y + b - eY];
                    }
                }
                for (int t = 0; t < window.Length; t++)
                {
                    for (int q = t + 1; q < window.Length; q++)
                    {
                        if (seperator[window[t]] < seperator[window[q]])
                        {
                            Biome temp = window[t];
                            window[t] = window[q];
                            window[q] = temp;
                        }
                    }
                }
                output[x, y] = window[window.Length / 2];
            }
        }
        biomes = output;
    }
    public int HowManyWriteablesLeft()
    {
        int result = 0;
        foreach (Biome b in biomes)
        {
            if (b == null || b.overPlacable) result++;
        }
        return result;
    }
    private bool IsOverWriteable(Biome itself, params Biome[] biomes)
    {
        bool temp = Biome.IsOverWriteable(biomes[0]);
        for (int i = 1; i < biomes.Length; i++) temp &= Biome.IsOverWriteable(biomes[i]);
        return temp && Biome.IsOverWriteable(itself);
    }
    public void WriteEmptiness()
    {
        for (int i = 0; i < emptiness.GetLength(0); i++)
        {
            for (int j = 0; j < emptiness.GetLength(1); j++)
            {
                Console.Write(emptiness[i, j].ToString()[0] + " ");
            }
            Console.WriteLine();
        }
    }
    private void EraseOnlyPoints(Way way)
    {
        for (int i = 1; i < biomes.GetLength(0) - 1; i++)
        {
            for (int j = 1; j < biomes.GetLength(1) - 1; j++)
            {
                if (IsOverWriteable(biomes[i, j], biomes[i - 1, j], biomes[i + 1, j], biomes[i, j - 1], biomes[i, j + 1]))
                {
                    switch (way)
                    {
                        case Way.North:
                            biomes[i, j] = biomes[i, j + 1];
                            break;
                        case Way.South:
                            biomes[i, j] = biomes[i, j - 1];
                            break;
                        case Way.East:
                            biomes[i, j] = biomes[i + 1, j];
                            break;
                        case Way.West:
                            biomes[i, j] = biomes[i - 1, j];
                            break;
                    }
                }
            }
        }
    }
    private void EraseOnlyPoints(Way way, int threshold)
    {
        if (threshold > 4 || threshold < 1) return;
        int notNull = 0;
        for (int i = 1; i < biomes.GetLength(0) - 1; i++)
        {
            for (int j = 1; j < biomes.GetLength(1) - 1; j++)
            {
                if (biomes[i, j] == null || biomes[i, j].overPlacable)
                {
                    if (biomes[i + 1, j] != null) notNull++;
                    if (biomes[i - 1, j] != null) notNull++;
                    if (biomes[i, j + 1] != null) notNull++;
                    if (biomes[i, j - 1] != null) notNull++;
                    if (notNull >= threshold)
                    {
                        switch (way)
                        {
                            case Way.North:
                                biomes[i, j] = biomes[i, j + 1];
                                break;
                            case Way.South:
                                biomes[i, j] = biomes[i, j - 1];
                                break;
                            case Way.East:
                                biomes[i, j] = biomes[i + 1, j];
                                break;
                            case Way.West:
                                biomes[i, j] = biomes[i - 1, j];
                                break;
                        }
                    }
                }
            }
        }
    }
    private void Fill(Frequency<Biome> biome, int x, int y)
    {
        if (renewIteration)
        {
            iterationsLeft = (int)MathF.Min(biomes.Length, biome.decised);
            renewIteration = false;
        }
        if (biomes[x, y] == null || biomes[x, y].overPlacable)
        {
            biomes[x, y] = biome.item;
            FillBuffer(x, y);
            emptiness[x, y] = false;
        }
        else
        {
            return;
        }
        for (int i = 0; i < iterationsLeft - 1 && buffer.Count > 0 - 1; i++)
        {
            int selected = random.Next(0, buffer.Count);
            (int, int) indexes = buffer[selected];
            buffer.RemoveAt(selected);
            biomes[indexes.Item1, indexes.Item2] = biome.item;
            emptiness[indexes.Item1, indexes.Item2] = false;
            FillBuffer(indexes.Item1, indexes.Item2);

        }
    }
    private void FillBuffer(int x, int y)
    {
        if (x > 0 && y > 0 && x < biomes.GetLength(0) - 1 && y < biomes.GetLength(1) - 1)
        {
            if ((biomes[x - 1, y] == null || biomes[x - 1, y].overPlacable) && !buffer.Contains((x - 1, y))) buffer.Add((x - 1, y));
            if ((biomes[x + 1, y] == null || biomes[x + 1, y].overPlacable) && !buffer.Contains((x + 1, y))) buffer.Add((x + 1, y));
            if ((biomes[x, y - 1] == null || biomes[x, y - 1].overPlacable) && !buffer.Contains((x, y - 1))) buffer.Add((x, y - 1));
            if ((biomes[x, y + 1] == null || biomes[x, y + 1].overPlacable) && !buffer.Contains((x, y + 1))) buffer.Add((x, y + 1));
        }

    }
    private void FillRest()
    {
        Biome last = null;
        foreach (Biome b in biomes)
        {
            if (b != null)
            {
                last = b;
                break;
            }
        }
        for (int i = 0; i < biomes.GetLength(0); i++)
        {
            for (int j = 0; j < biomes.GetLength(1); j++)
            {
                if (biomes[i, j] == null) biomes[i, j] = blank;
                else last = biomes[i, j];
            }
        }
    }
    public Color[] TurnColor()
    {
        Color[] colors = new Color[biomes.GetLength(0) * biomes.GetLength(1)];
        for (int i = 0; i < biomes.GetLength(0); i++)
        {
            for (int j = 0; j < biomes.GetLength(1); j++)
            {
                if (biomes[i, j] == null)
                {
                    colors[i * biomes.GetLength(0) + j] = blank.color;
                    continue;
                }
                colors[i * biomes.GetLength(0) + j] = biomes[i, j].color;
            }
        }
        return colors;
    }
    public void Export(string path, string name)
    {
        if (IsReadyToExport())
        {
            FileStream fs = new FileStream(path + $"//{name}.bmp", FileMode.Create);
            fs.Write((CustomCalculation.ToByteArray(Image.BMP.CreateDirectColor(TurnColor(), biomes.GetLength(0), biomes.GetLength(1)))));
            fs.Flush();
            fs.Close();
            Console.WriteLine("Exported");
        }
    }
    public bool IsReadyToExport()
    {
        return biomes != null;
    }
    
}
public class PoliticalMap{}