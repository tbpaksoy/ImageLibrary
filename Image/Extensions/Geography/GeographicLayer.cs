using TahsinsLibrary.Layers;
using TahsinsLibrary.Analyze;
using TahsinsLibrary.Geometry;
using System;
using System.Collections.Generic;
namespace TahsinsLibrary.Geography.Layers
{
    public class TopographicLayer : Layer<float>, IColorTurnable
    {
        private Random random;
        public float minHeight, maxHeight;
        public int iterationCount;
        public int minQuality, maxQuality, smoothQuality;
        public bool smooth;
        private float minAssigned = int.MaxValue, maxAssigned = int.MinValue;
        public TopographicLayer(int width, int height, int permissionCount, int seed) : base(width, height, permissionCount)
        {
            random = new Random(seed);
            data = new float[width, height];
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
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    data[i, j] = minHeight;
                }
            }
            minAssigned = minHeight;
            for (int i = 0; i < iterationCount; i++)
            {
                List<Vector2D> points = new List<Vector2D>();
                int quality = random.Next(minQuality, maxQuality);
                for (int j = 0; j < quality; j++)
                {
                    points.Add(new Vector2D(random.Next(0, data.GetLength(0) - 1) + random.NextSingle(), random.Next(0, data.GetLength(1) - 1) + random.NextSingle()));
                }
                FreePolygon2D polygon = new FreePolygon2D() { vertices = points.ToArray() };
                float value = random.Next((int)minHeight, (int)maxHeight - 1) + random.NextSingle();
                Console.WriteLine(value);
                minAssigned = MathF.Min(minAssigned, value);
                maxAssigned = MathF.Max(maxAssigned, value);
                if (smooth) polygon = polygon.GetSmoothVersion(smoothQuality);
                foreach (Vector2D point in polygon.GetPointsInside())
                {
                    if (permission[point.pointX, point.pointY] > 0)
                    {
                        data[point.pointX, point.pointY] = value;
                        permission[point.pointX, point.pointY]--;
                    }
                }
            }
            Console.WriteLine(maxAssigned);
            Console.WriteLine(minAssigned);
        }

        public override float[,] GetData() => data;

        public Color[,] TurnColor()
        {
            Color deepest = new Color(0, 0, 0, 255);
            Color highest = new Color(255, 255, 255, 255);
            Color[,] result = new Color[data.GetLength(0), data.GetLength(1)];
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    float temp = data[i, j] / (maxAssigned - minAssigned);
                    byte r = (byte)(highest.r * temp);
                    byte g = (byte)(highest.g * temp);
                    byte b = (byte)(highest.b * temp);
                    result[i, j] = new Color(r, g, b);
                }
            }
            return result;
        }
    }
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