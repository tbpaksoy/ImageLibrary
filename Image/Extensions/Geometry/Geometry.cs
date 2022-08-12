using System;
using System.Collections.Generic;
namespace TahsinsLibrary.Geometry
{
    public interface IOffset
    {
        public Vector2D offset { get; set; }
        public Vector2D[] ApplyOffset();
        public Vector2D[] ApplyOffset(Vector2D offset);
        public Vector2D[] ApplyOffset(Vector2D[] points);
        public static Vector2D[] ApplyOffset(Vector2D[] vertices, Vector2D offset)
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
    public interface ITransform : IRotate, IOffset
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
        public float x
        {
            get; private set;
        }
        public float y
        {
            get; private set;
        }
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
        public float Distance(Vector2D point) => MathF.Sqrt(MathF.Abs(x - point.x * y - point.y));

    }
    public abstract class Shape2D : ITransform
    {
        public abstract float area { get; }
        public abstract Vector2D[] points { get; }
        public abstract Vector2D centroid { get; }
        public Vector2D offset { get; set; }
        public float rotation { get; set; }

        public Vector2D[] ApplyOffset()
        {
            Vector2D[] result = new Vector2D[points.Length];
            for (int i = 0; i < result.Length; i++) result[i] = points[i] + offset;
            return result;
        }

        public Vector2D[] ApplyOffset(Vector2D offset)
        {
            Vector2D[] result = new Vector2D[points.Length];
            for (int i = 0; i < result.Length; i++) result[i] = points[i] + offset;
            return result;
        }

        public Vector2D[] ApplyOffset(Vector2D[] points) => IOffset.ApplyOffset(points, offset);

        public Vector2D[] ApplyTransform() => ApplyOffset(Rotate());

        public Vector2D[] GetTransformedPoints() => ApplyOffset(Rotate());

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
        public bool IsPointInside(Vector2D point)
        {

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
        public void SortVertices()
        {
            Console.WriteLine("Sorting Process Began");
            Vector2D center = centroid;
            Vector2D[] vertices = new Vector2D[this.vertices.Length];
            (float, int)[] temp = new (float, int)[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2D v = this.vertices[i] - center;
                temp[i] = (MathF.Atan2(v.y, v.x), i);
            }
            Random random = new Random();
            for (int i = temp.Length - 1; i > 0; i--)
            {
                int index1 = random.Next(0, temp.Length), index2 = random.Next(0, temp.Length);
                (float, int) tmp = temp[index1];
                temp[index1] = temp[index2];
                temp[index2] = tmp;
            }
            for (int i = 0; i < temp.Length; i++)
            {
                for (int j = i + 1; j < temp.Length; j++)
                {
                    if (temp[i].Item1 < temp[j].Item1)
                    {
                        (float, int) tmp = temp[i];
                        temp[i] = temp[j];
                        temp[j] = tmp;
                    }
                }
            }
            for (int i = 0; i < temp.Length; i++)
            {
                vertices[i] = this.vertices[temp[i].Item2];
            }
            this.vertices = vertices;
            Console.WriteLine("Sortinge Process Ended");
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
                        Console.WriteLine("X");
                    }
                }
            }
            vertices = temp.ToArray();
            Console.WriteLine("Solving Process Ended");
        }
        public bool IsPointInside(Vector2D point)
        {
            foreach (Triangle triangle in TriangulatePolygon())
            {
                if (triangle.IsPointInside(point)) return true;
            }
            return false;
        }
    }
    public sealed class Line2D : ITransform
    {
        public Vector2D from;
        public Vector2D to;
        public Vector2D centroid => (from + to) / 2f;

        public float rotation { get; set; }
        public Vector2D offset { get; set; }

        public Vector2D[] points => new Vector2D[] { from, to };

        public Vector2D[] ApplyOffset()
        {
            return new Vector2D[] { from + offset, to + offset };
        }

        public Vector2D[] ApplyOffset(Vector2D offset)
        {
            return new Vector2D[] { from + offset, to + offset };
        }

        public Vector2D[] ApplyOffset(Vector2D[] points) => IOffset.ApplyOffset(points, offset);

        public Vector2D[] ApplyTransform() => ApplyOffset(Rotate());

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

    }
}