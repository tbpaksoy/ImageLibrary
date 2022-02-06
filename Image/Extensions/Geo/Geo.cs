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
    private Biome() { }
    public Biome(string name, Color color)
    {
        this.name = name;
        this.color = color;
    }
}
public class GeoMap : IExportable
{
    private Random random;
    public readonly Biome blank = new Biome("blank", Color.black);
    public string name { get; set; }
    public Biome[,] biomes;
    private int iterationsLeft;
    private bool renewIteration;
    private List<(int, int)> buffer = new List<(int, int)>();
    public void FeedBiome(Frequency<Biome>[] frequencies, int width, int height, int seed)
    {
        random = new Random(seed);
        foreach (Frequency<Biome> f in frequencies)
        {
            f.DeciseCount(random.Next());
        }
        biomes = new Biome[width, height];
        foreach (Frequency<Biome> f in frequencies)
        {
            int nullsLeft = HowManyNullsLeft();
            if (nullsLeft < f.min) break;
            else if (nullsLeft < f.decised) continue;
            int x = random.Next(0, biomes.GetLength(0)), y = random.Next(0, biomes.GetLength(1));
            while (biomes[x, y] != null)
            {
                x = random.Next(0, biomes.GetLength(0)); y = random.Next(0, biomes.GetLength(1));
            }
            Analyze.isTempStarted = false;
            Analyze.GetAdjenctIndexes<Biome>(f.item, x, y, biomes);
            FillNulls(f, x, y);
            renewIteration = true;
        }
        FillRest();
    }
    public int HowManyNullsLeft()
    {
        int result = 0;
        foreach (Biome b in biomes)
        {
            if (b == null) result++;
        }
        return result;
    }
    private void FillNulls(Frequency<Biome> biome, int x, int y)
    {
        if (renewIteration)
        {
            iterationsLeft = (int)MathF.Min(biomes.Length, biome.decised);
            renewIteration = false;
        }
        if (biomes[x, y] != null) return;
        else
        {
            biomes[x, y] = biome.item;
            FillBuffer(x, y);
        }
        for (int i = 0; i < iterationsLeft - 1; i++)
        {
            int selected = random.Next(0, buffer.Count);
            (int, int) indexes = buffer[selected];
            buffer.RemoveAt(selected);
            biomes[indexes.Item1, indexes.Item2] = biome.item;
            FillBuffer(indexes.Item1, indexes.Item2);
        }
    }
    private void FillBuffer(int x, int y)
    {
        if (x > 0 && y > 0 && x < biomes.GetLength(0) - 1 && y < biomes.GetLength(1) - 1)
        {
            if (biomes[x - 1, y] == null && !buffer.Contains((x - 1, y))) buffer.Add((x - 1, y));
            if (biomes[x + 1, y] == null && !buffer.Contains((x + 1, y))) buffer.Add((x + 1, y));
            if (biomes[x, y - 1] == null && !buffer.Contains((x, y - 1))) buffer.Add((x, y - 1));
            if (biomes[x, y + 1] == null && !buffer.Contains((x, y + 1))) buffer.Add((x, y + 1));
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
        return HowManyNullsLeft() == 0 && biomes != null;
    }
}