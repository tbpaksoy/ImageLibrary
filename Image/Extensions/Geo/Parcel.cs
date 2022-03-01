using System.Collections.Generic;
using TahsinsLibrary.Analyze;
using System;
public abstract class Parcel<T>
{
    protected T[,] source;
    protected List<(int, int)> indexes = new List<(int, int)>();
    public bool isEmpty
    {
        get { return indexes.Count == 0; }
    }
    public (int, int)[] getIndexes
    {
        get { return indexes.ToArray(); }
    }
    public int size
    {
        get
        {
            return indexes.Count;
        }
    }
    public abstract void AddToParcel(List<(int, int)> indexes);
    public abstract void RemoveFromParcel(List<(int, int)> indexes);
    public abstract List<Parcel<T>> SplitParcel();
    public bool IsSubParcel(Parcel<T> toControl)
    {
        return Compare.IsSubList(indexes, toControl.indexes);
    }
    public bool IsSubParcel(List<(int, int)> indexes)
    {
        return Compare.IsSubList(this.indexes, indexes);
    }
    protected abstract void RegisterTable(ref bool[,] table);
    public (int, int) GetPointOn(int index)
    {
        return indexes[index];
    }
    public bool IsWhole()
    {
        Random random = new Random();
        (int, int) i = indexes[random.Next(0, indexes.Count)];
        if (indexes.Count == Analyze.CountAdjencts(indexes, i.Item1, i.Item2))
        {
            return true;
        }
        else return false;
    }
}
public class PhysicalParcel : Parcel<Biome>
{
    public PhysicalParcel() { }
    public PhysicalParcel(Biome[,] source, int x, int y)
    {
        this.source = source;
        indexes = Analyze.CheckAdjenctivty<Biome>(null, x, y, this.source);
    }
    public override void AddToParcel(List<(int, int)> indexes)
    {
        foreach ((int, int) i in indexes)
        {
            indexes.Add(i);
        }
    }
    public override void RemoveFromParcel(List<(int, int)> indexes)
    {
        foreach ((int, int) i in indexes)
        {
            indexes.Add(i);
        }
    }
    protected override void RegisterTable(ref bool[,] table)
    {
        foreach ((int, int) i in indexes)
        {
            table[i.Item1, i.Item2] = false;
        }
    }
    public override List<Parcel<Biome>> SplitParcel()
    {
        List<Parcel<Biome>> result = new List<Parcel<Biome>>();
        Random random = new Random();
        (int, int) i = indexes[random.Next(0, indexes.Count)];
        while (indexes.Count != Analyze.CountAdjencts(indexes, i.Item1, i.Item2))
        {
            List<(int, int)> temp = Analyze.GetSubAdjenctList(indexes, i.Item1, i.Item2);
            RemoveFromParcel(temp);
            PhysicalParcel parcel = new PhysicalParcel();
            parcel.indexes = temp;
            result.Add(parcel);
        }
        return result;
    }
}
public class PoliticalParcel : Parcel<Country>
{
    protected bool[,] avaibility;
    private PoliticalParcel() { }
    public PoliticalParcel(bool[,] avaibility)
    {
        this.avaibility = avaibility;
    }
    public PoliticalParcel(List<(int,int)> indexes)
    {
        this.indexes = indexes;
    }
    public PoliticalParcel(bool[,] avaibility, int x, int y)
    {
        this.avaibility = avaibility;
        indexes = Analyze.CheckAdjenctivty<bool>(true, x, y, this.avaibility);
    }
    public override void AddToParcel(List<(int, int)> indexes)
    {
        foreach ((int, int) i in indexes)
        {
            if (!indexes.Contains(i) && avaibility[i.Item1, i.Item2]) indexes.Add(i);
        }
    }
    public override void RemoveFromParcel(List<(int, int)> indexes)
    {
        foreach ((int, int) i in indexes)
        {
            if (indexes.Contains(i)) indexes.Add(i);
        }
    }
    protected override void RegisterTable(ref bool[,] table)
    {
        foreach ((int, int) i in indexes)
        {
            table[i.Item1, i.Item2] = false;
        }
    }
    public override List<Parcel<Country>> SplitParcel()
    {
        List<Parcel<Country>> result = new List<Parcel<Country>>();
        Random random = new Random();
        (int, int) i = indexes[random.Next(0, indexes.Count)];
        while (indexes.Count != Analyze.CountAdjencts(indexes, i.Item1, i.Item2))
        {
            List<(int, int)> temp = Analyze.GetSubAdjenctList(indexes, i.Item1, i.Item2);
            RemoveFromParcel(temp);
            PoliticalParcel parcel = new PoliticalParcel();
            parcel.indexes = temp;
            result.Add(parcel);
        }
        return result;
    }
}
