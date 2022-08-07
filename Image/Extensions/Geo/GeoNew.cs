using TahsinsLibrary.Collections;
using TahsinsLibrary.Analyze;
using System;
using System.Collections.Generic;
namespace TahsinsLibrary.Geo
{
    public sealed class Biome
    {
        public Color color;
        public string name;
        public bool canPlaceCountry;
        public Biome(string name, Color color, bool canPlaceCountry = false)
        {
            this.color = color;
            this.name = name;
            this.canPlaceCountry = canPlaceCountry;
        }
    }
    public abstract class Parcel
    {
        private List<(int, int)> points = new List<(int, int)>();
        public int pointsCount => points.Count;
        public abstract void ScanPartly(World world, int x, int y);
        public abstract void ScanCompletely(World world);

    }
    public class World
    {
        public float[,] heights;
        public void GenerateWorld(){}
    }
}