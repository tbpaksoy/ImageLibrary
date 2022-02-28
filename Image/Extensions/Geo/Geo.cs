using TahsinsLibrary;
using System;
using TahsinsLibrary.Collections;
using System.Collections.Generic;
using TahsinsLibrary.Analyze;
using System.IO;
using TahsinsLibrary.Calculation;
using System.Diagnostics;
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
        if (overPlacable) canPlaceCountry = false;
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
    protected Biome[,] biomes;
    private int iterationsLeft;
    private bool renewIteration;
    private List<(int, int)> buffer = new List<(int, int)>();
    private bool[,] emptiness;
    private Dictionary<Biome, int> seperator = new Dictionary<Biome, int>();
    protected bool[,] countryLands;
    protected Country[,] ownership;
    protected List<(int, int)> avaibleToPlaceCountry = new List<(int, int)>();
    public void FeedBiome(Frequency<Biome>[] frequencies, int width, int height, int seed)
    {
        Console.WriteLine("Feeding");
        random = new Random(seed);
        seperator.TryAdd(defaultBiome, 0);
        int seperatorIndex = 1;
        ownership = new Country[width, height];
        countryLands = new bool[width, height];
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
        Console.WriteLine("O" + output == null);
        biomes = output;
        for (int i = 0; i < biomes.GetLength(0); i++)
        {
            for (int j = 0; j < biomes.GetLength(1); j++)
            {
                countryLands[i, j] = biomes[i, j] == null ? false : biomes[i, j].canPlaceCountry;
                if (countryLands[i, j]) avaibleToPlaceCountry.Add((i, j));
            }
        }
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
        for (int i = 0; i < iterationsLeft - 1 && buffer.Count > 0; i++)
        {
            int selected = random.Next(0, buffer.Count);
            (int, int) indexes = buffer[selected];
            buffer.RemoveAt(selected);
            biomes[indexes.Item1, indexes.Item2] = biome.item;
            emptiness[indexes.Item1, indexes.Item2] = false;
            countryLands[indexes.Item1, indexes.Item2] = biome.item.canPlaceCountry;
            FillBuffer(indexes.Item1, indexes.Item2);
        }
    }
    private void FillAlt(Frequency<Biome> biome, int x, int y)
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
        int selected = random.Next(0, buffer.Count);
        (int, int) indexes = buffer[selected];
        Way way = (Way)random.Next(0, 4);
        int[] counts = new int[4];
        for (int i = 0; i < iterationsLeft - 1 && buffer.Count > 0; i++)
        {
            indexes = buffer[selected];
            buffer.RemoveAt(selected);
            biomes[indexes.Item1, indexes.Item2] = biome.item;
            emptiness[indexes.Item1, indexes.Item2] = false;
            countryLands[indexes.Item1, indexes.Item2] = biome.item.canPlaceCountry;
            FillBuffer(indexes.Item1, indexes.Item2);
            way = (Way)random.Next(0, 4);
            counts[(int)way]++;
            switch (way)
            {
                case Way.North:
                    selected = random.Next(0, buffer.Count);
                    while (buffer[selected].Item1 < indexes.Item1 || IsTrapped(buffer[selected].Item1, buffer[selected].Item2))
                    {
                        selected = random.Next(0, buffer.Count);
                    }
                    break;
                case Way.South:
                    selected = random.Next(0, buffer.Count);
                    while (buffer[selected].Item1 > indexes.Item1 || IsTrapped(buffer[selected].Item1, buffer[selected].Item2))
                    {
                        selected = random.Next(0, buffer.Count);
                    }
                    break;
                case Way.West:
                    selected = random.Next(0, buffer.Count);
                    while (buffer[selected].Item2 < indexes.Item2 || IsTrapped(buffer[selected].Item1, buffer[selected].Item2))
                    {
                        selected = random.Next(0, buffer.Count);
                    }
                    break;
                case Way.East:
                    selected = random.Next(0, buffer.Count);
                    while (buffer[selected].Item2 > indexes.Item2 || IsTrapped(buffer[selected].Item1, buffer[selected].Item2))
                    {
                        selected = random.Next(0, buffer.Count);
                    }
                    break;
            }
        }
    }
    private bool IsTrapped(int x, int y)
    {
        bool checkBorder = x == 0 || x == biomes.GetLength(0) - 1 || y == 0 || y == biomes.GetLength(1) - 1;
        if (checkBorder) return true;
        bool checkAvability = biomes[x + 1, y] == null || biomes[x + 1, y].overPlacable ||
                              biomes[x - 1, y] == null || biomes[x - 1, y].overPlacable ||
                              biomes[x, y - 1] == null || biomes[x, y - 1].overPlacable ||
                              biomes[x, y + 1] == null || biomes[x, y + 1].overPlacable;
        return checkBorder && checkAvability;
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
    private int HowManyOnWayInBuffer(int x, int y, Way way)
    {
        Dictionary<Way, int> counts = new Dictionary<Way, int>();
        counts.Add(Way.North, 0);
        counts.Add(Way.South, 0);
        counts.Add(Way.East, 0);
        counts.Add(Way.West, 0);

        foreach ((int, int) i in buffer)
        {
            if (i.Item1 < x)
            {
                counts[Way.North]++;
            }
            if (i.Item1 > x)
            {
                counts[Way.South]++;
            }
            if (i.Item1 < y)
            {
                counts[Way.East]++;
            }
            if (i.Item1 > y)
            {
                counts[Way.West]++;
            }
        }
        return counts[way];
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
    public virtual Color[] TurnColor()
    {
        Color[] colors = new Color[biomes.GetLength(0) * biomes.GetLength(1)];
        for (int i = 0; i < biomes.GetLength(0); i++)
        {
            for (int j = 0; j < biomes.GetLength(1); j++)
            {
                if (biomes[i, j] == null)
                {
                    colors[i * biomes.GetLength(1) + j] = blank.color;
                }
                else colors[i * biomes.GetLength(1) + j] = biomes[i, j].color;
            }
        }
        return colors;
    }
    public void Export(string path, string name)
    {
        if (IsReadyToExport())
        {
            FileStream fs = new FileStream(path + $"//{name}.bmp", FileMode.Create);
            Console.WriteLine("Writing");
            fs.Write((CustomCalculation.ToByteArray(TahsinsLibrary.Image.BMP.CreateDirectColor(TurnColor(), biomes.GetLength(1), biomes.GetLength(0)))));
            fs.Flush();
            fs.Close();
            Console.WriteLine("Exported");
        }
    }
    public virtual bool IsReadyToExport()
    {
        return biomes != null;
    }
    public int HowManyNullsLeft()
    {
        int count = 0;
        foreach (Biome b in biomes) if (b == null) count++;
        return count;
    }

}
public class PoliticalMap : GeoMap
{
    private List<(int, int)> buffer = new List<(int, int)>();
    private Random random;
    protected Frequency<Country>[] countries;
    private Dictionary<Country, int> seperator = new Dictionary<Country, int>();
    private List<(int, int)> centers = new List<(int, int)>();
    private List<PoliticalParcel> parcels = new List<PoliticalParcel>();
    public int howManyFreeLandLeft
    {
        get
        {
            int result = 0;
            foreach (bool b in countryLands)
            {
                if (b) result++;
            }
            return result;
        }
    }
    public int neededLands
    {
        get
        {
            if (countries == null) return 0;
            int result = 0;
            foreach (Frequency<Country> country in countries)
            {
                result += country.min;
            }
            return result;
        }
    }
    private void DesignateAreas()
    {
        Console.WriteLine("Designating");
        bool[,] table = new bool[biomes.GetLength(0), biomes.GetLength(1)];
        int passed = 0;
        for (int i = 0; i < table.GetLength(0); i++)
        {
            for (int j = 0; j < table.GetLength(1); j++)
            {
                if (!table[i, j])
                {
                    List<(int, int)> temp = Analyze.CheckAdjenctivty<bool>(countryLands[i, j], i, j, countryLands);
                    passed += temp.Count;
                    foreach ((int, int) x in temp)
                    {
                        table[x.Item1, x.Item2] = true;
                    }
                    if (countryLands[temp[0].Item1, temp[0].Item2])
                    {
                        PoliticalParcel parcel = new PoliticalParcel(temp);
                        parcels.Add(parcel);
                    }
                }
                else Console.WriteLine("S");
            }
        }
        Console.WriteLine("Designated");
    }
    public void PlaceCountries(int seed)
    {
        DesignateAreas();
        random = new Random(seed);
        Console.WriteLine("Placing countries");
        int minCountryArea = 0;
        int seperatorIndex = 0;
        foreach (Frequency<Country> f in countries)
        {
            f.DeciseCount(random.Next());
            minCountryArea += f.min;
            if (seperator.TryAdd(f.item, seperatorIndex)) seperatorIndex++;

        }
        if (neededLands > howManyFreeLandLeft)
        {
            Console.WriteLine("Stopped due the not enough lands.");
        }
        foreach (Frequency<Country> country in countries)
        {
            int x = 0, y = 0;
            PoliticalParcel parcel = parcels[random.Next(0, parcels.Count)];
            /*while (!countryLands[x, y] || Analyze.CheckAdjenctivty<bool>(true, x, y, countryLands).Count < country.min)
            {
                Console.Write("a");
                avaibleToPlaceCountry.RemoveAt(index);
                index = random.Next(0, avaibleToPlaceCountry.Count);
                x = avaibleToPlaceCountry[index].Item1; y = avaibleToPlaceCountry[index].Item2;
            }*/
            int attempts = 0;
            while (parcel.isEmpty)
            {
                attempts++;
                Console.WriteLine("Decising");
                if (parcels.Count == 0) break;
                if (parcel.isEmpty) parcels.Remove(parcel);
                parcel = parcels[random.Next(0, parcels.Count)];
            }
            Console.WriteLine("Decised");
            if (parcels.Count == 0) break;
            (int, int) index = parcel.getIndexes[random.Next(0, parcel.getIndexes.Length)];
            x = index.Item1; y = index.Item2;
            Console.WriteLine("Declaring");
            Stopwatch stopwatch = Stopwatch.StartNew();
            DeclareLandAsCountry(country, parcel);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }
        Console.WriteLine("All countries placed");
    }
    private void DeclareLandAsCountry(Frequency<Country> country, int x, int y)
    {
        centers.Add((x, y));
        List<(int, int)> avaible = Analyze.CheckAdjenctivty<bool>(true, x, y, countryLands);
        int decised = country.decised > avaible.Count ? Math.Max(avaible.Count, country.min) : country.decised;
        ownership[x, y] = country.item;
        List<(int, int)> buffer = new List<(int, int)>();

        if (x > 0 && countryLands[x - 1, y]) buffer.Add((x - 1, y));
        if (x < ownership.GetLength(0) - 1 && countryLands[x + 1, y]) buffer.Add((x + 1, y));
        if (y > 0 && countryLands[x, y - 1]) buffer.Add((x, y - 1));
        if (y < ownership.GetLength(1) - 1 && countryLands[x, y + 1]) buffer.Add((x, y + 1));
        countryLands[x, y] = false;
        for (int i = 1; i < decised; i++)
        {
            int index = random.Next(0, buffer.Count);
            x = buffer[index].Item1; y = buffer[index].Item2;
            if (x > 0 && countryLands[x - 1, y]) buffer.Add((x - 1, y));
            if (x < ownership.GetLength(0) - 1 && countryLands[x + 1, y]) buffer.Add((x + 1, y));
            if (y > 0 && countryLands[x, y - 1]) buffer.Add((x, y - 1));
            if (y < ownership.GetLength(1) - 1 && countryLands[x, y + 1]) buffer.Add((x, y + 1));
            ownership[x, y] = country.item;
            countryLands[x, y] = false;
            buffer.RemoveAt(index);
        }
    }
    private void DeclareLandAsCountry(Frequency<Country> country, PoliticalParcel parcel)
    {
        bool[,] table = new bool[biomes.GetLength(0), biomes.GetLength(1)];
        foreach ((int, int) i in parcel.getIndexes)
        {
            table[i.Item1, i.Item2] = true;
        }
        int index = random.Next(0, parcel.getIndexes.Length);
        (int, int) chosen = parcel.getIndexes[index];
        centers.Add(chosen);
        int decised = Math.Min(country.decised, parcel.getIndexes.Length);
        ownership[chosen.Item1, chosen.Item2] = country.item;
        List<(int, int)> buffer = new List<(int, int)>();
        List<(int, int)> temp = new List<(int, int)>();
        int x = chosen.Item1, y = chosen.Item2;
        countryLands[x, y] = false;
        table[x, y] = false;
        if (x > 0 && !table[x - 1, y]) buffer.Add((x - 1, y));
        if (x < ownership.GetLength(0) - 1 && !table[x + 1, y]) buffer.Add((x + 1, y));
        if (y > 0 && !table[x, y - 1]) buffer.Add((x, y - 1));
        if (y < ownership.GetLength(1) - 1 && !table[x, y + 1]) buffer.Add((x, y + 1));
        Console.WriteLine(buffer.Count);
        for (int i = 0, attempts = 0; i < decised; i++, attempts++)
        {
            if (attempts > decised) Console.Write("W!");
            index = random.Next(0, buffer.Count);
            chosen = buffer[index];
            x = chosen.Item1; y = chosen.Item2;
            if (!table[x, y] || !countryLands[x, y])
            {
                i--;
                continue;
            }
            if (x > 0 && table[x - 1, y]) buffer.Add((x - 1, y));
            if (x < ownership.GetLength(0) - 1 && table[x + 1, y]) buffer.Add((x + 1, y));
            if (y > 0 && table[x, y - 1]) buffer.Add((x, y - 1));
            if (y < ownership.GetLength(1) - 1 && table[x, y + 1]) buffer.Add((x, y + 1));
            ownership[chosen.Item1, chosen.Item2] = country.item;
            buffer.RemoveAt(index);
            countryLands[x, y] = false;
            table[x, y] = false;
            temp.Add(chosen);
        }
        Console.WriteLine();
        parcel.RemoveFromParcel(temp);
        foreach (PoliticalParcel newParcel in parcel.SplitParcel())
        {
            parcels.Add(newParcel);
        }
    }
    private void MedianSmooth(int width, int height)
    {
        Country[] window = new Country[width * height];
        Country[,] output = new Country[ownership.GetLength(0), ownership.GetLength(1)];
        int eX = (int)Math.Floor((float)width / 2f), eY = (int)Math.Floor((float)height / 2f);
        for (int x = eX; x < ownership.GetLength(0) - eX; x++)
        {
            for (int y = eY; y < ownership.GetLength(1) - eY; y++)
            {
                for (int a = 0, i = 0; a < width; a++)
                {
                    for (int b = 0; b < height; b++, i++)
                    {
                        window[i] = ownership[x + a - eX, y + b - eY];
                    }
                }
                for (int t = 0; t < window.Length; t++)
                {
                    for (int q = t + 1; q < window.Length; q++)
                    {
                        if (window[t] == null || window[q] == null)
                        {
                            Country temp = window[t];
                            window[t] = window[q];
                            window[q] = temp;
                        }
                        else
                        {
                            if (seperator[window[t]] < seperator[window[q]])
                            {
                                Country temp = window[t];
                                window[t] = window[q];
                                window[q] = temp;
                            }
                        }
                    }
                }
                output[x, y] = window[window.Length / 2];
            }
        }
        ownership = output;
    }
    public override Color[] TurnColor()
    {
        Color[] result = base.TurnColor();
        foreach ((int, int) i in centers)
        {
            List<(int, int)> borders = Analyze.EdgeDetect<Country>(ownership[i.Item1, i.Item2], i.Item1, i.Item2, ownership);
            foreach ((int, int) j in borders)
            {
                result[j.Item1 * ownership.GetLength(1) + j.Item2] = ownership[i.Item1, i.Item2].color;
            }
        }
        return result;
    }
    private int HowManyAreasLeftForCountries()
    {
        int count = 0;
        foreach (bool b in countryLands) if (b) count++;
        return count;
    }
    public override bool IsReadyToExport()
    {
        return base.IsReadyToExport();
    }
    public void FeedCountries(params Frequency<Country>[] countries)
    {
        this.countries = countries;
    }
    public void FeedCountries(Country[] countries, (int, int)[] frequencies)
    {
        if (countries.Length == frequencies.Length)
        {
            this.countries = new Frequency<Country>[countries.Length];
            for (int i = 0; i < countries.Length; i++)
            {
                this.countries[i] = new Frequency<Country>(countries[i], Math.Min(frequencies[i].Item1, frequencies[i].Item2), Math.Max(frequencies[i].Item1, frequencies[i].Item2));
            }
        }
        else Console.WriteLine("Can not feed due the arrays length not same.");
    }
}
public class Country
{
    public string name { get; private set; }
    public Color color { get; private set; }
    public Country(string name, Color color)
    {
        this.name = name;
        this.color = color;
    }
}