using System;
using System.Collections.Generic;
using TahsinsLibrary.Analyze;
using System.Linq;
namespace TahsinsLibrary.Geometry
{
    public interface ITranslate
    {
        public Vector2D offset { get; set; }
        public Vector2D[] Translate();
        public Vector2D[] Translate(Vector2D offset);
        public Vector2D[] Translate(Vector2D[] points);
        public static Vector2D[] Translate(Vector2D[] vertices, Vector2D offset)
        {
            Vector2D[] result = new Vector2D[vertices.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = vertices[i] + offset;
            }
            return result;
        }
    }
    public interface IRotate
    {
        public float rotation { get; set; }
        public Vector2D[] Rotate();
        public Vector2D[] Rotate(float rotation);
        public Vector2D[] Rotate(Vector2D[] vertices);
        public static Vector2D[] Rotate(Vector2D[] vertices, float rotation)
        {
            Vector2D[] result = new Vector2D[vertices.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new Vector2D(vertices[i].x * MathF.Cos(rotation) - vertices[i].y * MathF.Sin(rotation), vertices[i].x * MathF.Sin(rotation) + vertices[i].y * MathF.Cos(rotation));
            }
            return result;
        }
    }
    public interface IScale
    {
        Vector2D scale { get; set; }
        public Vector2D[] Scale();
        public Vector2D[] Scale(Vector2D scale);
        public Vector2D[] Scale(Vector2D[] points);
        public static Vector2D[] Scale(Vector2D[] vertices, Vector2D scale)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector2D(vertices[i].x * scale.x, vertices[i].y * scale.y);
            }
            Vector2D center = Vector2D.Center(vertices);
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] -= center;
            }
            return vertices;
        }
    }
    public interface ITransform : IRotate, ITranslate, IScale
    {
        public Vector2D[] points { get; }
        public Vector2D[] ApplyTransform();
        public Vector2D[] GetTransformedPoints();
    }
    public struct Vector2D
    {
        public enum Orientation { CounterClockWise, CoLinear, ClockWise }
        public static readonly Vector2D up = new Vector2D(0f, 1f);
        public static readonly Vector2D down = new Vector2D(0f, -1f);
        public static readonly Vector2D left = new Vector2D(-1f, 0f);
        public static readonly Vector2D right = new Vector2D(1f, 0f);
        public float magnitude => MathF.Sqrt(x * x + y * y);
        public Vector2D normal => this / MathF.Max(x, y);
        public static Vector2D GetRandom(int minX, int maxX, int minY, int maxY)
        {
            Random random = new Random();
            return new Vector2D(random.Next(minX, maxX) + random.NextSingle(), random.Next(minY, maxY) + random.NextSingle());
        }
        public float x
        {
            get; private set;
        }
        public float y
        {
            get; private set;
        }
        public int pointX => (int)x;
        public int pointY => (int)y;
        public Vector2D(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public static Vector2D operator +(Vector2D vector, float value)
        {
            Vector2D result = new Vector2D(vector.x + value, vector.y + value);
            return result;
        }
        public static Vector2D operator -(Vector2D vector, float value)
        {
            Vector2D result = new Vector2D(vector.x - value, vector.y - value);
            return result;
        }
        public static Vector2D operator *(Vector2D vector, float value)
        {
            Vector2D result = new Vector2D(vector.x * value, vector.y * value);
            return result;
        }
        public static Vector2D operator /(Vector2D vector, float value)
        {
            Vector2D result = new Vector2D(vector.x / value, vector.y / value);
            return result;
        }
        public static Vector2D operator +(Vector2D a, Vector2D b)
        {
            Vector2D result = new Vector2D(a.x + b.x, a.y + b.y);
            return result;
        }
        public static Vector2D operator -(Vector2D a, Vector2D b)
        {
            Vector2D result = new Vector2D(a.x - b.x, a.y - b.y);
            return result;
        }
        public override string ToString()
        {
            return $"Vector2D ({x},{y})";
        }
        public static bool OnSegment(Vector2D p, Vector2D q, Vector2D r) => q.x <= MathF.Max(p.x, r.x) && q.x >= MathF.Min(p.x, r.x) && q.y <= MathF.Max(p.y, r.y) && q.y >= MathF.Min(p.y, r.y);
        public static Orientation GetOrientation(Vector2D p, Vector2D q, Vector2D r)
        {
            float value = (q.y - p.y) * (r.x - q.x) - (q.x - p.x) * (r.y - q.y);
            return value == 0 ? Orientation.CoLinear : value > 0 ? Orientation.ClockWise : Orientation.CounterClockWise;
        }
        public static bool Intersecting(Vector2D p1, Vector2D q1, Vector2D p2, Vector2D q2)
        {
            Orientation o1 = GetOrientation(p1, q1, p2);
            Orientation o2 = GetOrientation(p1, q1, q2);
            Orientation o3 = GetOrientation(p2, q2, p1);
            Orientation o4 = GetOrientation(p2, q2, q1);
            if (o1 != o2 && o3 != o4) return true;
            if (o1 == Orientation.CoLinear && OnSegment(p1, p2, q1)) return true;
            if (o2 == Orientation.CoLinear && OnSegment(p1, q2, q1)) return true;
            if (o3 == Orientation.CoLinear && OnSegment(p2, p1, q2)) return true;
            if (o4 == Orientation.CoLinear && OnSegment(p2, q1, q2)) return true;
            return false;
        }
        public static bool Intersecting(Vector2D p1, Vector2D q1, Vector2D p2, Vector2D q2, out Vector2D intersectionPoint)
        {
            intersectionPoint = new Vector2D(float.NaN, float.NaN);
            bool intersecting = Intersecting(p1, q1, p2, q2);
            if (intersecting) intersectionPoint = GetIntersectionPoint(p1, q1, p2, q2);
            return intersecting;
        }
        public static Vector2D GetIntersectionPoint(Vector2D p1, Vector2D q1, Vector2D p2, Vector2D q2)
        {
            float a1 = q1.y - q1.x;
            float b1 = q1.x - q1.x;
            float c1 = a1 * q1.x + b1 * q1.y;

            float a2 = q2.y - p2.y;
            float b2 = p2.x - q2.x;
            float c2 = a2 * p2.x + b2 * p2.y;

            float detereminant = a1 * b2 - a2 * b1;

            return detereminant == 0 ? new(float.NaN, float.NaN) : new Vector2D(b2 * c1 - b1 * c2, a1 * c2 - a2 * c1) / detereminant;
        }
        public float Distance(Vector2D point) => MathF.Sqrt(MathF.Abs((x - point.x) * (y - point.y)));
        public static Vector2D Center(params Vector2D[] vectors)
        {
            Vector2D center = new Vector2D();
            foreach (Vector2D v in vectors)
            {
                center += v;
            }
            center /= vectors.Length;
            return center;
        }
        public static (float, float, float, float) GetSegment(params Vector2D[] resources)
        {
            float[] x = new float[resources.Length];
            float[] y = new float[resources.Length];
            for (int i = 0; i < resources.Length; i++)
            {
                x[i] = resources[i].x;
                y[i] = resources[i].y;
            }
            return (x.Min(), x.Max(), y.Min(), y.Max());
        }
        public static (float, float, float, float) GetSegment(params Vector2D[][] resources)
        {
            float xMin = float.MaxValue, xMax = float.MinValue, yMin = float.MaxValue, yMax = float.MinValue;
            foreach (Vector2D[] vectors in resources)
            {
                float[] x = new float[vectors.Length], y = new float[vectors.Length];
                for (int i = 0; i < vectors.Length; i++)
                {
                    x[i] = vectors[i].x;
                    y[i] = vectors[i].y;
                }
                (float, float) xn = (x.Min(), x.Max());
                (float, float) yn = (y.Min(), y.Max());
                xMin = xn.Item1 < xMin ? xn.Item1 : xMin;
                xMax = xn.Item2 > xMax ? xn.Item2 : xMax;
                yMin = yn.Item1 < yMin ? yn.Item1 : yMin;
                yMax = yn.Item2 > yMax ? yn.Item2 : yMax;
            }
            return (xMin, xMax, yMin, yMax);
        }
        public static (float, float, float, float) GetSegment(params (float, float, float, float)[] segments)
        {
            float xMin, xMax, yMin, yMax;
            xMin = float.MaxValue;
            yMin = float.MaxValue;
            xMax = float.MinValue;
            yMax = float.MinValue;
            foreach ((float, float, float, float) segment in segments)
            {
                if (xMin > segment.Item1) xMin = segment.Item1;
                if (xMax < segment.Item2) xMax = segment.Item2;
                if (yMin > segment.Item3) yMin = segment.Item3;
                if (yMax < segment.Item4) yMax = segment.Item4;
            }
            return (xMin, xMax, yMin, yMax);
        }
        public static Vector2D ToCenter(int width, int height, params Vector2D[] resources)
        {
            (float, float, float, float) segment = GetSegment(resources);
            Vector2D centerOfSegment = new Vector2D(segment.Item2 - segment.Item1, segment.Item4 - segment.Item3) / 2f;
            Vector2D center = new Vector2D(width, height) / 2f;
            return centerOfSegment - center;
        }
        public bool AxisBiggerThan(float number)
        {
            return x > number || y > number;
        }
        public static Vector2D Interpolate(Vector2D a, Vector2D b, float t)
        {
            t = MathF.Abs(t);
            if (t > 1f) t %= 1f;
            return a + (b - a) * t;
        }
        public static Vector2D QuadraticInterpolate(Vector2D a, Vector2D b, Vector2D c, float t) => Interpolate(Interpolate(a, b, t), Interpolate(b, c, t), t);
        public static float GetAngle(Vector2D a, Vector2D b) => (float)Math.Atan2(b.y - a.y, b.x - a.x);

    }
    public abstract class Shape2D : ITransform
    {
        public abstract float area { get; }
        public abstract Vector2D[] points { get; }
        public abstract Vector2D centroid { get; }
        public Vector2D offset { get; set; } = new Vector2D();
        public float rotation { get; set; } = 0f;
        public Vector2D scale { get; set; } = new Vector2D(1f, 1f);

        public Vector2D[] Translate()
        {
            Vector2D[] result = new Vector2D[points.Length];
            for (int i = 0; i < result.Length; i++) result[i] = points[i] + offset;
            return result;
        }

        public Vector2D[] Translate(Vector2D offset)
        {
            Vector2D[] result = new Vector2D[points.Length];
            for (int i = 0; i < result.Length; i++) result[i] = points[i] + offset;
            return result;
        }

        public Vector2D[] Translate(Vector2D[] points) => ITranslate.Translate(points, offset);

        public Vector2D[] ApplyTransform()
        {
            Vector2D[] result = Scale();
            result = Rotate(result);
            result = Translate(result);
            return result;
        }

        public Vector2D[] GetTransformedPoints() => Scale(Rotate(Translate()));

        public Vector2D[] Rotate()
        {
            Vector2D[] result = new Vector2D[points.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new Vector2D(points[i].x * MathF.Cos(rotation) - points[i].y * MathF.Sin(rotation), points[i].x * MathF.Sin(rotation) + points[i].y * MathF.Cos(rotation));
            }
            return result;
        }

        public Vector2D[] Rotate(float rotation)
        {
            Vector2D[] result = new Vector2D[points.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new Vector2D(points[i].x * MathF.Cos(rotation) - points[i].y * MathF.Sin(rotation), points[i].x * MathF.Sin(rotation) + points[i].y * MathF.Cos(rotation));
            }
            return result;
        }

        public Vector2D[] Rotate(Vector2D[] vertices) => IRotate.Rotate(vertices, rotation);
        public abstract Line2D[] ToLines();
        public virtual Line2D[] ToTransformedLines()
        {
            List<Line2D> lines = new List<Line2D>();
            Vector2D[] transformed = ApplyTransform();
            for (int i = 0; i < transformed.Length - 1; i++)
            {
                lines.Add(new Line2D() { from = transformed[i], to = transformed[i + 1] });
            }
            lines.Add(new Line2D() { from = transformed[0], to = transformed[^1] });
            return lines.ToArray();
        }
        public virtual Vector2D[] GetOutlines()
        {
            List<Vector2D> temp = new List<Vector2D>();
            foreach (Line2D line in ToLines())
            {
                foreach (Vector2D v in line.GetPoints())
                {
                    temp.Add(v);
                }
            }
            return temp.ToArray();
        }
        public virtual Vector2D[] GetTransformedOutlines()
        {
            List<Vector2D> temp = new List<Vector2D>();
            foreach (Line2D line in ToTransformedLines())
            {
                foreach (Vector2D v in line.GetPoints())
                {
                    temp.Add(v);
                }
            }
            return temp.ToArray();
        }
        public abstract bool InSegment(Vector2D point);
        public abstract (float, float, float, float) GetSegment();
        public abstract bool IsPointInside(Vector2D point);
        public abstract void Normalize();

        public Vector2D[] Scale()
        {
            Vector2D[] result = new Vector2D[points.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new Vector2D(points[i].x * scale.x, points[i].y * scale.y);
            }
            Vector2D center = Vector2D.Center(result);
            for (int i = 0; i < result.Length; i++)
            {
                result[i] -= center;
            }
            return result;
        }

        public Vector2D[] Scale(Vector2D scale)
        {
            Vector2D[] result = new Vector2D[points.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new Vector2D(points[i].x * scale.x, points[i].y * scale.y);
            }
            Vector2D center = Vector2D.Center(result);
            for (int i = 0; i < result.Length; i++)
            {
                //result[i] -= center;
            }
            return result;
        }

        public Vector2D[] Scale(Vector2D[] points)
        {
            Vector2D[] result = new Vector2D[points.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new Vector2D(points[i].x * scale.x, points[i].y * scale.y);
            }
            Vector2D center = Vector2D.Center(result);
            for (int i = 0; i < result.Length; i++)
            {
                result[i] -= center;
            }
            return result;
        }
        public Vector2D[] Fit(int width, int height)
        {
            width -= 1;
            height -= 1;
            (float, float, float, float) segment = Vector2D.GetSegment(points);
            Vector2D scale = new Vector2D(width / MathF.Abs(segment.Item2 - segment.Item1), height / MathF.Abs(segment.Item4 - segment.Item3));
            List<Vector2D> temp = new List<Vector2D>();
            foreach (Vector2D v in points)
            {
                temp.Add(new Vector2D(v.x * scale.x, v.y * scale.y));
            }
            Vector2D center = Vector2D.Center(temp.ToArray());
            for (int i = 0; i < temp.Count; i++)
            {
                temp[i] -= center;
            }
            segment = Vector2D.GetSegment(temp.ToArray());
            Vector2D offset = new Vector2D(segment.Item1, segment.Item3);
            for (int i = 0; i < temp.Count; i++)
            {
                temp[i] -= offset;
            }
            return temp.ToArray();
        }
        public Vector2D[] Fit(int width, int height, bool keepRatio = false)
        {
            if (keepRatio)
            {
                width -= 1;
                height -= 1;
                (float, float, float, float) segment = Vector2D.GetSegment(points);
                float scale = MathF.Min(width / MathF.Abs(segment.Item2 - segment.Item1), height / MathF.Abs(segment.Item4 - segment.Item3));
                List<Vector2D> temp = new List<Vector2D>();
                foreach (Vector2D v in points)
                {
                    temp.Add(new Vector2D(v.x * scale, v.y * scale));
                }
                Vector2D center = Vector2D.Center(temp.ToArray());
                for (int i = 0; i < temp.Count; i++)
                {
                    temp[i] -= center;
                }
                segment = Vector2D.GetSegment(temp.ToArray());
                Vector2D offset = new Vector2D(segment.Item1, segment.Item3);
                for (int i = 0; i < temp.Count; i++)
                {
                    temp[i] -= offset;
                }
                return temp.ToArray();
            }
            else return Fit(width, height);
        }
        public Vector2D[] Smooth(int quality)
        {
            List<Vector2D> temp = new List<Vector2D>();
            Vector2D[] points = this.points.Clone() as Vector2D[];
            for (int i = 1; i < quality + 1; i++)
            {
                temp.Add(Vector2D.QuadraticInterpolate((points[^1] + points[0]) / 2f, points[0], (points[0] + points[1]) / 2f, 1f / quality * i));
            }
            for (int i = 1; i < points.Length - 1; i++)
            {
                for (int j = 1; j < quality + 1; j++)
                {
                    temp.Add(Vector2D.QuadraticInterpolate((points[i - 1] + points[i]) / 2f, points[i], (points[i] + points[i + 1]) / 2f, 1f / quality * j));
                }
            }
            for (int i = 1; i < quality + 1; i++)
            {
                temp.Add(Vector2D.QuadraticInterpolate((points[^2] + points[^1]) / 2f, points[^1], (points[^1] + points[0]) / 2f, 1f / quality * i));
            }
            return temp.ToArray();
        }
        public abstract FreePolygon2D GetSmoothVersion(int quality);
    }
    public sealed class Triangle : Shape2D
    {
        public Triangle(Vector2D pointA, Vector2D pointB, Vector2D pointC)
        {
            this.pointA = pointA;
            this.pointB = pointB;
            this.pointC = pointC;
        }
        public Vector2D pointA { get; private set; }
        public Vector2D pointB { get; private set; }
        public Vector2D pointC { get; private set; }
        public override float area => MathF.Abs(pointA.x * pointB.y + pointB.x * pointC.y + pointC.x * pointA.y - pointA.y * pointB.x - pointB.y * pointC.x - pointC.y * pointA.x) / 2f;
        public override Vector2D[] points => new Vector2D[] { pointA, pointB, pointC };

        public override Vector2D centroid => (pointA + pointB + pointC) / 3;

        public Vector2D[] GetPointsInside()
        {
            List<Vector2D> result = new List<Vector2D>();
            int yMax = (int)(MathF.Max(pointA.y, MathF.Max(pointB.y, pointC.y)));
            int yMin = (int)(MathF.Min(pointA.y, MathF.Min(pointB.y, pointC.y)));
            int xMax = (int)(MathF.Max(pointA.x, MathF.Max(pointB.x, pointC.x)));
            int xMin = (int)(MathF.Min(pointA.x, MathF.Min(pointB.x, pointC.x)));
            for (int i = xMin; i < xMax; i++)
            {
                for (int j = yMin; j < yMax; j++)
                {

                    Vector2D temp = new Vector2D(i, j);
                    if (IsPointInside(temp))
                    {
                        result.Add(temp);
                    }
                }
            }
            return result.ToArray();
        }
        //xMin,xMax,yMin,yMax
        public override (float, float, float, float) GetSegment()
        {

            float xMin = Compare.Min(new float[] { pointA.x, pointB.x, pointC.x });
            float xMax = Compare.Max(new float[] { pointA.x, pointB.x, pointC.x });
            float yMin = Compare.Min(new float[] { pointA.y, pointB.y, pointC.y });
            float yMax = Compare.Max(new float[] { pointA.y, pointB.y, pointC.y });
            return (xMin, xMax, yMin, yMax);
        }

        public override FreePolygon2D GetSmoothVersion(int quality) => new FreePolygon2D() { vertices = Smooth(quality) };

        public override bool InSegment(Vector2D point)
        {
            (float, float, float, float) segment = GetSegment();
            float x = point.x, y = point.y;
            return !(x < segment.Item1 || x > segment.Item2 || y < segment.Item3 || y > segment.Item4);
        }

        public override bool IsPointInside(Vector2D point)
        {
            if (!InSegment(point)) return false;
            Vector2D a = pointA;
            Vector2D b = pointB;
            Vector2D c = pointC;
            Vector2D p = point;
            float w1 = a.x * (c.y - a.y) + (p.y - a.y) * (c.x - a.x) - p.x * (c.y - a.y);
            w1 /= (b.y - a.y) * (c.x - a.x) - (b.x - a.x) * (c.y - a.y);
            float w2 = p.y - a.y - w1 * (b.y - a.y);
            w2 /= c.y - a.y;
            return w1 >= 0 && w2 >= 0 && w1 + w2 <= 1;
        }

        public override void Normalize()
        {
            Vector2D center = centroid;
            pointA -= centroid;
            pointB -= centroid;
            pointC -= centroid;
        }

        public override Line2D[] ToLines()
        {
            return new Line2D[]
            {
                new(){from = pointA, to = pointB},
                new(){from = pointB, to = pointC},
                new(){from = pointC, to = pointA}
            };
        }

    }
    public class FreePolygon2D : Shape2D
    {
        public Vector2D[] vertices;
        public override float area
        {
            get
            {
                float result = 0;
                foreach (Triangle triangle in TriangulatePolygon()) result += triangle.area;
                return result;
            }
        }
        public override Vector2D[] points => vertices;

        public override Vector2D centroid
        {
            get
            {
                Vector2D center = new Vector2D();
                foreach (Vector2D vector in vertices)
                {
                    center += vector;
                }
                center /= vertices.Length;
                return center;
            }
        }
        private bool[] CalculateConcavity()
        {
            bool[] result = new bool[vertices.Length];
            for (int i = 0; i < result.Length; i++)
            {
                Vector2D a = vertices[i - 1 < 0 ? vertices.Length - 1 : i - 1];
                Vector2D p = vertices[i];
                Vector2D b = vertices[(i + 1) % vertices.Length];
                result[i] = ((p.x - a.x) * (b.y - a.y) - (p.y - a.y) * (b.x - a.x)) > 1f;
            }
            return result;
        }
        public Triangle[] TriangulatePolygon()
        {
            Console.WriteLine("Triangulation Process Began");
            List<int> indexList = new List<int>();
            List<Triangle> triangles = new List<Triangle>();
            for (int i = 0; i < vertices.Length; i++)
            {
                indexList.Add(i);
            }
            while (indexList.Count > 2)
            {
                for (int i = 0; i < indexList.Count - 2; i++)
                {
                    Vector2D a = vertices[indexList[i]];
                    Vector2D b = vertices[indexList[i + 1]];
                    Vector2D c = vertices[indexList[i + 2]];
                    Triangle triangle = new Triangle(a, b, c);
                    bool test = false;
                    for (int j = 0; j < indexList.Count; j++)
                    {
                        if ((j < i || j > i + 2) && triangle.IsPointInside(vertices[j]))
                        {
                            test = true;
                            break;
                        }
                    }
                    if (!test)
                    {
                        triangles.Add(triangle);
                        indexList.RemoveAt(i + 1);
                    }
                }
            }
            Console.WriteLine("Triangulation Process Ended");
            return triangles.ToArray();
        }
        public void OrderVertices()
        {
            Vector2D center = Vector2D.Center(vertices);
            vertices = vertices.OrderBy(v => Vector2D.GetAngle(center, v)).ToArray();
        }
        public Vector2D[] GetPointsInside()
        {
            List<Vector2D> result = new List<Vector2D>();

            Triangle[] triangles = TriangulatePolygon();
            foreach (Triangle triangle in triangles)
            {
                Vector2D[] points = triangle.GetPointsInside();
                foreach (Vector2D point in points)
                {
                    result.Add(point);
                }
            }

            return result.ToArray();
        }
        public void SolveComplexity()
        {
            Console.WriteLine("Solving Process Began");
            List<Vector2D> temp = new List<Vector2D>();
            foreach (Vector2D vector in vertices)
            {
                temp.Add(vector);
            }
            for (int i = 0; i < vertices.Length - 1; i++)
            {
                for (int j = i + 2; j < vertices.Length - 1; j++)
                {
                    if (Vector2D.Intersecting(vertices[i], vertices[i + 1], vertices[j], vertices[j + 1], out Vector2D point))
                    {
                        temp.Add(point);
                    }
                }
            }
            vertices = temp.ToArray();
            Console.WriteLine("Solving Process Ended");
        }
        public override bool IsPointInside(Vector2D point)
        {
            if (!InSegment(point)) return false;
            foreach (Triangle triangle in TriangulatePolygon())
            {
                if (triangle.IsPointInside(point)) return true;
            }
            return false;
        }

        public override Line2D[] ToLines()
        {
            Line2D[] lines = new Line2D[vertices.Length];
            lines[0] = new() { from = vertices[0], to = vertices[1] };
            for (int i = 1; i < vertices.Length - 1; i++)
            {
                lines[i] = new() { from = vertices[i], to = vertices[i + 1] };
            }
            lines[lines.Length - 1] = new() { from = vertices[vertices.Length - 1], to = vertices[0] };
            return lines;
        }

        public override bool InSegment(Vector2D point)
        {
            (float, float, float, float) segment = GetSegment();
            float x = point.x, y = point.y;
            return !(x < segment.Item1 || x > segment.Item2 || y < segment.Item3 || y > segment.Item4);
        }

        public override (float, float, float, float) GetSegment()
        {
            List<float> temp = new List<float>();
            foreach (Vector2D vector in points)
            {
                temp.Add(vector.x);
            }
            float xMin = Compare.Min(temp.ToArray());
            float xMax = Compare.Max(temp.ToArray());
            temp.Clear();
            foreach (Vector2D vector in points)
            {
                temp.Add(vector.y);
            }
            float yMin = Compare.Min(temp.ToArray());
            float yMax = Compare.Max(temp.ToArray());
            return (xMin, xMax, yMin, yMax);
        }

        public override void Normalize()
        {
            Vector2D vector = centroid;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] -= vector;
            }
        }

        public override FreePolygon2D GetSmoothVersion(int quality) => new FreePolygon2D() { vertices = Smooth(quality) };
    }
    public sealed class Line2D : ITransform
    {
        public Vector2D from;
        public Vector2D to;
        public Vector2D centroid => (from + to) / 2f;

        public float rotation { get; set; }
        public Vector2D offset { get; set; }

        public Vector2D[] points => new Vector2D[] { from, to };

        public Vector2D scale { get; set; }

        public Vector2D[] Translate()
        {
            return new Vector2D[] { from + offset, to + offset };
        }

        public Vector2D[] Translate(Vector2D offset)
        {
            return new Vector2D[] { from + offset, to + offset };
        }

        public Vector2D[] Translate(Vector2D[] points) => ITranslate.Translate(points, offset);

        public Vector2D[] ApplyTransform() => Scale(Rotate(Translate()));

        public Vector2D[] GetPoints()
        {
            List<Vector2D> result = new();
            int dx = (int)MathF.Abs(to.x - from.x);
            int sx = from.x < to.x ? 1 : -1;
            int dy = -(int)MathF.Abs(to.y - from.y);
            int sy = from.y < to.y ? 1 : -1;
            int error = dx + dy;
            int x = (int)from.x, y = (int)from.y;
            while (true)
            {
                result.Add(new Vector2D(x, y));
                if (x == (int)to.x && y == (int)to.y) break;
                int e = 2 * error;
                if (e >= dy)
                {
                    if (x == (int)to.x) break;
                    error += dy;
                    x += sx;
                }
                if (e <= dx)
                {
                    if (y == (int)to.y) break;
                    error += dx;
                    y += sy;
                }
            }
            return result.ToArray();
        }
        public Vector2D[] GetTransformedPoints()
        {
            Vector2D[] points = ApplyTransform();
            int dx = (int)(points[1].x - points[0].x);
            int dy = (int)(points[1].y - points[0].y);
            int d = 2 * dy - dx;
            int y = (int)to.y;
            List<Vector2D> result = new List<Vector2D>();

            for (int x = (int)points[0].x; x < (int)points[1].x; x++)
            {
                result.Add(new(x, y));
                if (d > 0)
                {
                    y++;
                    d -= 2 * dx;
                }
                d += 2 * dy;
            }

            return result.ToArray();
        }
        public Vector2D[] Rotate()
        {
            return new Vector2D[]
            {
                new(from.x * MathF.Cos(rotation) - from.y * MathF.Sin(rotation), from.x * MathF.Sin(rotation) + from.y * MathF.Cos(rotation)),
                new(to.x * MathF.Cos(rotation) - to.y * MathF.Sin(rotation), to.x * MathF.Sin(rotation) + to.y * MathF.Cos(rotation))
            };
        }
        public Vector2D[] Rotate(float rotation)
        {
            return new Vector2D[]
            {
                new(from.x * MathF.Cos(rotation) - from.y * MathF.Sin(rotation),from.x * MathF.Sin(rotation) + from.y * MathF.Cos(rotation)),
                new(to.x * MathF.Cos(rotation) - to.y * MathF.Sin(rotation),to.x * MathF.Sin(rotation) + to.y * MathF.Cos(rotation))
            };
        }
        public Vector2D[] Rotate(Vector2D[] vertices) => IRotate.Rotate(vertices, rotation);
        public Vector2D[] Scale()
        {
            Vector2D[] result = new Vector2D[points.Length];
            for (int i = 0; i < result.Length; i++)
            {
                Vector2D temp = points[i];
                result[i] = new Vector2D(temp.x * scale.x, temp.y * scale.y);
            }
            Vector2D center = Vector2D.Center(result);
            for (int i = 0; i < result.Length; i++)
            {
                result[i] -= center;
            }
            return result;
        }

        public Vector2D[] Scale(Vector2D scale)
        {
            Vector2D[] result = new Vector2D[points.Length];
            for (int i = 0; i < result.Length; i++)
            {
                Vector2D temp = points[i];
                result[i] = new Vector2D(temp.x * scale.x, temp.y * scale.y);
            }
            Vector2D center = Vector2D.Center(result);
            for (int i = 0; i < result.Length; i++)
            {
                //result[i] -= center;
            }
            return result;
        }

        public Vector2D[] Scale(Vector2D[] points)
        {
            Vector2D[] result = new Vector2D[points.Length];
            for (int i = 0; i < result.Length; i++)
            {
                Vector2D temp = points[i];
                result[i] = new Vector2D(temp.x * scale.x, temp.y * scale.y);
            }
            Vector2D center = Vector2D.Center(result);
            for (int i = 0; i < result.Length; i++)
            {
                //result[i] -= center;
            }
            return result;
        }
    }
    public sealed class ShapeGroup
    {
        public List<Shape2D> group = new List<Shape2D>();
        public Dictionary<Shape2D, Vector2D> origins = new Dictionary<Shape2D, Vector2D>();
        public void Add(Shape2D shape)
        {
            group.Add(shape);
            origins.Add(shape, Vector2D.Center(shape.points));
        }
        public Vector2D[] GetOutlines(Vector2D offset, Vector2D scale)
        {
            List<Vector2D> temp = new List<Vector2D>();
            foreach (Shape2D shape in group)
            {
                Vector2D[] buffer = shape.points.Clone() as Vector2D[];
                buffer = IScale.Scale(buffer, scale);
                Vector2D calc = new Vector2D(origins[shape].x * scale.x, origins[shape].y * scale.y);
                buffer = ITranslate.Translate(buffer, calc + offset);
                Line2D[] lines = new Line2D[buffer.Length];
                for (int i = 0; i < buffer.Length - 1; i++)
                {
                    lines[i] = new Line2D() { from = buffer[i], to = buffer[i + 1] };
                }
                lines[^1] = new Line2D() { from = buffer[0], to = buffer[^1] };
                foreach (Line2D line in lines)
                {
                    foreach (Vector2D v in line.GetPoints())
                    {
                        temp.Add(v);
                    }
                }
            }
            return temp.ToArray();
        }
        public void RecalculateOrigins()
        {
            List<Vector2D> allPoints = new List<Vector2D>();
            Dictionary<Shape2D, Vector2D> localCenter = new Dictionary<Shape2D, Vector2D>();
            foreach (Shape2D shape in group)
            {
                foreach (Vector2D v in shape.points)
                {
                    allPoints.Add(v);
                }
                localCenter.Add(shape, Vector2D.Center(shape.points));
            }
            Vector2D center = Vector2D.Center(allPoints.ToArray());
            foreach (Shape2D key in origins.Keys.ToArray())
            {
                origins[key] = localCenter[key] - center;
            }
        }
        public Vector2D[] Fit(int width, int height)
        {
            width--;
            height--;
            List<Vector2D> result = new List<Vector2D>();
            (float, float, float, float) segment = GetSegment();
            Vector2D scale = new Vector2D((float)width / MathF.Abs(segment.Item2 - segment.Item1), (float)height / MathF.Abs(segment.Item4 - segment.Item3));
            List<Line2D> lines = new List<Line2D>();
            foreach (Shape2D shape in group)
            {
                Vector2D[] points = shape.points.Clone() as Vector2D[];
                for (int i = 0; i < points.Length; i++)
                {
                    float x = points[i].x, y = points[i].y;
                    points[i] = new Vector2D(x * scale.x, y * scale.y);
                }
                for (int i = 0; i < points.Length - 1; i++)
                {
                    lines.Add(new Line2D() { from = points[i], to = points[i + 1] });
                    result.Add(points[i]);
                }
                result.Add(points[^1]);
                lines.Add(new Line2D() { from = points[0], to = points[^1] });
            }
            segment = Vector2D.GetSegment(result.ToArray());
            scale = new Vector2D((float)width / MathF.Abs(segment.Item2 - segment.Item1), (float)height / MathF.Abs(segment.Item4 - segment.Item3));
            for (int i = 0; i < lines.Count; i++)
            {
                lines[i].from = new Vector2D(scale.x * lines[i].from.x, scale.y * lines[i].from.y);
                lines[i].to = new Vector2D(scale.x * lines[i].to.x, scale.y * lines[i].to.y);
            }
            result.Clear();
            foreach (Line2D line in lines)
            {
                foreach (Vector2D v in line.GetPoints())
                {
                    result.Add(v);
                }
            }
            segment = Vector2D.GetSegment(result.ToArray());
            Vector2D offset = new Vector2D(segment.Item1, segment.Item3);
            for (int i = 0; i < result.Count; i++)
            {
                result[i] -= offset;
            }
            return result.ToArray();
        }
        public Vector2D[] Fit(int width, int height, bool keepRatio)
        {
            if (keepRatio)
            {
                width--;
                height--;
                List<Vector2D> result = new List<Vector2D>();
                (float, float, float, float) segment = GetSegment();
                float scale = MathF.Min(width / (segment.Item2 - segment.Item1), height / (segment.Item4 - segment.Item3));
                List<Line2D> lines = new List<Line2D>();
                foreach (Shape2D shape in group)
                {
                    Vector2D[] points = shape.points.Clone() as Vector2D[];
                    for (int i = 0; i < points.Length; i++)
                    {
                        float x = points[i].x, y = points[i].y;
                        points[i] = new Vector2D(x * scale, y * scale);
                    }
                    for (int i = 0; i < points.Length - 1; i++)
                    {
                        lines.Add(new Line2D() { from = points[i], to = points[i + 1] });
                        result.Add(points[i]);
                    }
                    result.Add(points[^1]);
                    lines.Add(new Line2D() { from = points[0], to = points[^1] });
                }
                segment = Vector2D.GetSegment(result.ToArray());
                scale = MathF.Min(width / (segment.Item2 - segment.Item1), height / (segment.Item4 - segment.Item3));
                for (int i = 0; i < lines.Count; i++)
                {
                    lines[i].from = new Vector2D(scale * lines[i].from.x, scale * lines[i].from.y);
                    lines[i].to = new Vector2D(scale * lines[i].to.x, scale * lines[i].to.y);
                }
                result.Clear();
                foreach (Line2D line in lines)
                {
                    foreach (Vector2D v in line.GetPoints())
                    {
                        result.Add(v);
                    }
                }
                segment = Vector2D.GetSegment(result.ToArray());
                Vector2D offset = new Vector2D(segment.Item1, segment.Item3);
                for (int i = 0; i < result.Count; i++)
                {
                    result[i] -= offset;
                }
                return result.ToArray();
            }
            else return Fit(width, height);
        }
        public Vector2D[] GetPoints(Vector2D offset, Vector2D scale)
        {
            List<Vector2D> temp = new List<Vector2D>();
            foreach (Shape2D shape in group)
            {
                Vector2D[] buffer = shape.points.Clone() as Vector2D[];
                buffer = IScale.Scale(buffer, scale);
                Vector2D calc = new Vector2D(origins[shape].x * scale.x, origins[shape].y * scale.y);
                buffer = ITranslate.Translate(buffer, calc + offset);
                foreach (Vector2D v in buffer)
                {
                    temp.Add(v);
                }
            }
            return temp.ToArray();
        }
        public (float, float, float, float)[] GetSegments()
        {
            List<(float, float, float, float)> segments = new List<(float, float, float, float)>();
            foreach (Shape2D shape in group)
            {
                segments.Add(shape.GetSegment());
            }
            return segments.ToArray();
        }
        public (float, float, float, float) GetSegment() => Vector2D.GetSegment(GetSegments());
    }
    public static class GeometryUtility
    {
        public static void JustCut(ref FreePolygon2D polygon, Shape2D shape, bool useTranform = false)
        {
            List<Vector2D> intersectinPoints = new List<Vector2D>();
            Vector2D[] polygonlPoints = useTranform ? polygon.GetTransformedPoints() : polygon.points;
            Vector2D[] shapePoints = useTranform ? shape.GetTransformedPoints() : polygon.points;
            List<Vector2D> appliedPoints = new List<Vector2D>();
            foreach (Vector2D point in polygonlPoints)
            {
                if (!shape.IsPointInside(point)) appliedPoints.Add(point);
            }
            foreach (Vector2D point in shapePoints)
            {
                if (polygon.IsPointInside(point)) appliedPoints.Add(point);
            }

        }
        public static FreePolygon2D CutAndSnatch(ref FreePolygon2D polygon, Shape2D shape, bool useTranform = false)
        {
            throw new Exception();
        }
        public static FreePolygon2D CutSnatchAndUniteWithShape(ref FreePolygon2D polygon, Shape2D shape, bool useTranform = false)
        {
            throw new Exception();
        }
        public static void Add(ref FreePolygon2D polygon, Shape2D shape, bool useTranform = false)
        {

        }
        public static FreePolygon2D GetCircle(float radius = 1f, int resolution = 16)
        {
            resolution = (int)MathF.Max(3, resolution);
            Vector2D[] points = new Vector2D[resolution];
            for (int i = 0; i < resolution; i++)
            {
                float angle = 2f * MathF.PI / (float)resolution * (float)(i);
                points[i] = new Vector2D(MathF.Cos(angle), MathF.Sin(angle))
                  * radius;
            }
            return new FreePolygon2D() { vertices = points };
        }
        public static void Intersect(ref FreePolygon2D polygon, Shape2D shape, bool useTranform = false) { }
        public static Line2D[] BuildLinesFrom(Vector2D[] points)
        {
            Line2D[] lines = new Line2D[points.Length];
            for (int i = 0; i < lines.Length - 1; i++)
            {
                lines[i] = new Line2D() { from = points[i], to = points[i + 1] };
            }
            lines[^1] = new Line2D() { from = points[0], to = points[^1] };
            return lines;
        }
    }
}