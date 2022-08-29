using TahsinsLibrary.Layers;
using TahsinsLibrary.Analyze;
using System;
using System.Collections.Generic;
namespace TahsinsLibrary.Geography.Layers
{
    public class BiomeLayer : Layer<Biome>, IColorTurnable
    {
        public Biome defaultBiome;
        public Biome[] biomeVarierity;
        public int iterationCount, min, max;
        public BiomeLayer(int width, int height, int permissionCount) : base(width, height, permissionCount)
        {
            data = new Biome[width, height];
            permission = new int[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    permission[i, j] = permissionCount;
                }
            }
        }
        public override void GenerateData()
        {
            Random random = new Random();
            for (int i = 0; i < iterationCount; i++)
            {

            }
        }
        public override Biome[,] GetData()
        {
            throw new System.NotImplementedException();
        }

        public Color[,] TurnColor()
        {
            throw new System.NotImplementedException();
        }
    }
}