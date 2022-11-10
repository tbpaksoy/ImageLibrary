#include <vector>
#include <math.h>
#include <tuple>
#include <limits>
#include <algorithm>
using namespace std;
namespace Tahsin
{
    const float pi = 22.0 / 7.0;
    struct Vector2D
    {
    private:
        float x, y;

    public:
        enum Orientation
        {
            CCW,
            CL,
            CW
        };
        float GetX()
        {
            return x;
        }
        float GetY()
        {
            return y;
        }
        Vector2D() {}
        Vector2D(float x, float y)
        {
            (*this).x = x;
            (*this).y = y;
        }
        static const Vector2D up;
        static const Vector2D down;
        static const Vector2D left;
        static const Vector2D right;
        Vector2D operator+(Vector2D v)
        {
            return Vector2D(x + v.x, y + v.y);
        }
        Vector2D operator-(Vector2D v)
        {
            return Vector2D(x - v.x, y - v.y);
        }
        Vector2D operator*(float value)
        {
            return Vector2D(x * value, y * value);
        }
        Vector2D operator/(float value)
        {
            return Vector2D(x / value, y / value);
        }
        static bool OnSegment(Vector2D p, Vector2D q, Vector2D r)
        {
            return q.GetX() <= max(p.GetX(), r.GetX()) && q.GetX() >= min(p.GetX(), r.GetX()) && q.GetY() <= max(p.GetY(), r.GetY()) && q.GetY() >= min(p.GetY(), r.GetY());
        }
        static Orientation GetOrientation(Vector2D p, Vector2D q, Vector2D r)
        {
            float value = (q.GetY() - p.GetY()) * (r.GetX() - q.GetY()) - (q.GetX() - p.GetX()) * (r.GetY() - q.GetY());
            return value == 0 ? CL : value > 0 ? CW
                                               : CCW;
        }
        static bool Intersecting(Vector2D p1, Vector2D q1, Vector2D p2, Vector2D q2)
        {
            Orientation o1 = GetOrientation(p1, q1, p2);
            Orientation o2 = GetOrientation(p1, q1, q2);
            Orientation o3 = GetOrientation(p2, q2, p1);
            Orientation o4 = GetOrientation(p2, q2, q1);
            if (o1 != o2 && o3 != o4)
                return true;
            if (o1 == CL && OnSegment(p1, p2, q1))
                return true;
            if (o2 == CL && OnSegment(p1, q2, q1))
                return true;
            if (o3 == CL && OnSegment(p2, p1, q2))
                return true;
            if (o4 == CL && OnSegment(p2, q1, q2))
                return true;
            return false;
        }
        int PointX()
        {
            return (int)x;
        }
        int PointY()
        {
            return (int)y;
        }
        static float Distance(Vector2D a, Vector2D b)
        {
            return sqrt(abs((a.x - b.x) * (a.y - b.y)));
        }
        float Distance(Vector2D v)
        {
            return sqrt(abs((x - v.x) * (y - v.y)));
        }
        static Vector2D Center(vector<Vector2D> resource)
        {
            Vector2D center = Vector2D();
            for (Vector2D v : resource)
            {
                center = center + v;
            }
            center = center / resource.size();
            return center;
        }
        static Vector2D ToCenter(vector<Vector2D> resource)
        {
            return Vector2D() - Center(resource);
        }
        static Vector2D Interpolate(Vector2D a, Vector2D b, float t)
        {
            t = abs(t);
            if (t > 1)
                t -= (int)t;
            return a + (b - a) * t;
        }
        static Vector2D QuadraticInterpolate(Vector2D a, Vector2D b, Vector2D c, float t)
        {
            return Interpolate(Interpolate(a, b, t), Interpolate(b, c, t), t);
        }
    };
    const Vector2D Vector2D::up = Vector2D(0, 1);
    const Vector2D Vector2D::down = Vector2D(0, -1);
    const Vector2D Vector2D::left = Vector2D(1, 0);
    const Vector2D Vector2D::right = Vector2D(-1, 0);
    class IOffset
    {
    public:
        Vector2D offset;
        virtual vector<Vector2D> ApplyOffset() = 0;
        virtual vector<Vector2D> ApplyOffset(Vector2D offset) = 0;
        virtual vector<Vector2D> ApplyOffset(vector<Vector2D> points) = 0;
        static vector<Vector2D> ApplyOffset(vector<Vector2D> vertices, Vector2D offset)
        {
            vector<Vector2D> result;
            for (int i = 0; i < vertices.size(); i++)
            {
                result.push_back(result[i] + offset);
            }
            return result;
        }
    };
    class IRotate
    {
    public:
        float rotation;
        virtual vector<Vector2D> Rotate() = 0;
        virtual vector<Vector2D> Rotate(float rotation) = 0;
        virtual vector<Vector2D> Rotate(vector<Vector2D> vertices) = 0;
        static vector<Vector2D> Rotate(vector<Vector2D> vertices, float rotation)
        {
            vector<Vector2D> result;
            for (Vector2D v : vertices)
            {
                Vector2D calculated = Vector2D(v.GetX() * cosf(rotation) - v.GetY() * sinf(rotation),
                                               v.GetX() * sinf(rotation) + v.GetY() * cosf(rotation));
            }
            return result;
        }
    };
    class IScale
    {
    public:
        Vector2D scale = Vector2D(1.0, 1.0);
        virtual vector<Vector2D> ApplyScale() = 0;
        virtual vector<Vector2D> ApplyScale(Vector2D scale) = 0;
        virtual vector<Vector2D> ApplyScale(vector<Vector2D> points) = 0;
    };
    class ITransform : public IRotate, public IOffset
    {
    public:
        virtual vector<Vector2D> GetPoints() = 0;
        virtual vector<Vector2D> ApplyTransform() = 0;
        virtual vector<Vector2D> GetTransformedPoints() = 0;
    };
    class Shape2D : public ITransform
    {
    public:
        virtual float GetArea() = 0;
        virtual vector<Vector2D> GetPoints() = 0;
        virtual Vector2D GetCentroid() = 0;
        virtual tuple<float, float, float, float> GetSegment() = 0;
        virtual bool IsPointInside(Vector2D point) = 0;
        virtual bool InSegment(Vector2D point)
        {
            tuple<float, float, float, float> segment = GetSegment();
            float x = point.GetX(), y = point.GetY();
            return !(x < get<0>(segment) || x > get<1>(segment) || y < get<2>(segment) || y > get<3>(segment));
        }
    };
    class Line2D : public ITransform
    {
    public:
        Vector2D from, to;
        Line2D()
        {
        }
        Line2D(Vector2D from, Vector2D to)
        {
            this->from = from;
            this->to = to;
        }
        ~Line2D() {}
        vector<Vector2D> GetPoints()
        {
            vector<Vector2D> result;
            int dx = (int)abs(to.GetX() - from.GetX());
            int sx = from.GetX() < to.GetY() ? 1 : -1;
            int dy = -(int)abs(to.GetY() - from.GetY());
            int sy = from.GetY() < to.GetY() ? 1 : -1;
            int error = dx + dy;
            int x = (int)from.GetX(), y = (int)from.GetY();
            while (true)
            {
                result.push_back(Vector2D(x, y));
                if (x == (int)to.GetX() && y == (int)to.GetY())
                    break;
                int e = 2 * error;
                if (e >= dy)
                {
                    if (x == (int)to.GetX())
                        break;
                    error += dy;
                    x += sx;
                }
                if (e <= dx)
                {
                    if (y == (int)to.GetY())
                        break;
                    error += dx;
                    y += sy;
                }
            }

            return result;
        }
        Vector2D GetCentroid()
        {
            return (from + to) / 2;
        }
        vector<Vector2D> ApplyOffset() override
        {
            vector<Vector2D> result;
            result.push_back(from + offset);
            result.push_back(to + offset);
            return result;
        }
        vector<Vector2D> ApplyOffset(Vector2D offset) override
        {
            vector<Vector2D> result;
            result.push_back(from + offset);
            result.push_back(to + offset);
            return result;
        }
        vector<Vector2D> ApplyOffset(vector<Vector2D> points) override
        {
            vector<Vector2D> result;
            for (Vector2D v : points)
            {
                result.push_back(v + offset);
            }
            return result;
        }
    };
    class Triangle : public Shape2D
    {
    public:
        Vector2D pointA;
        Vector2D pointB;
        Vector2D pointC;
        Triangle()
        {
        }
        Triangle(Vector2D pointA, Vector2D pointB, Vector2D pointC)
        {
            this->pointA = pointA;
            this->pointB = pointB;
            this->pointC = pointC;
        }
        ~Triangle() {}
        float GetArea() override
        {
            return abs(pointA.GetX() * pointB.GetY() + pointB.GetX() * pointC.GetY() + pointC.GetX() * pointA.GetY() - pointB.GetY() * pointB.GetX() - pointB.GetY() * pointC.GetX() - pointC.GetY() * pointA.GetX());
        }
        Vector2D GetCentroid() override
        {
            return (pointA + pointB + pointC) / 3;
        }
        vector<Vector2D> GetPointsInside()
        {
            vector<Vector2D> result;
            int yMax = (int)max(pointA.GetY(), max(pointB.GetY(), pointC.GetY()));
            int yMin = (int)min(pointA.GetY(), min(pointB.GetY(), pointC.GetY()));
            int xMax = (int)max(pointA.GetY(), max(pointB.GetY(), pointC.GetY()));
            int xMin = (int)min(pointA.GetY(), min(pointB.GetY(), pointC.GetY()));
            for (int i = xMin; i < xMax; i++)
            {
                for (int j = yMin; j < yMax; j++)
                {
                    Vector2D temp = Vector2D(i, j);
                    if (IsPointInside(temp))
                    {
                        result.push_back(temp);
                    }
                }
            }
            return result;
        }
        bool IsPointInside(Vector2D point)
        {
            if (!InSegment(point))
                return false;
            Vector2D a = pointA;
            Vector2D b = pointB;
            Vector2D c = pointC;
            Vector2D p = point;
            float w1 = a.GetX() * (c.GetY() - a.GetY()) + (p.GetY() - a.GetY()) * (c.GetX() - a.GetX()) - p.GetX() * (c.GetY() - a.GetY());
            w1 /= (b.GetY() - a.GetY()) * (c.GetX() - a.GetX()) - (b.GetX() - a.GetX()) * (c.GetY() - a.GetY());
            float w2 = p.GetY() - a.GetY() - w1 * (b.GetY() - a.GetY());
            w2 /= c.GetY() - a.GetY();
            return w1 >= 0 && w2 >= 0 && w1 + w2 <= 1;
        }
        vector<Vector2D> GetPoints() override
        {
            vector<Vector2D> result;
            result.push_back(pointA);
            result.push_back(pointB);
            result.push_back(pointC);
            return result;
        }
        vector<Vector2D> ApplyTransform() override
        {
        }
        vector<Vector2D> GetTransformedPoints() override
        {
        }
        vector<Vector2D> Rotate() override
        {
            vector<Vector2D> result;
            Vector2D a = pointA;
            Vector2D b = pointB;
            Vector2D c = pointC;
            result.push_back(Vector2D(a.GetX() * cosf(rotation) - a.GetY() * sinf(rotation), a.GetX() * sinf(rotation) + a.GetY() * cosf(rotation)));
            result.push_back(Vector2D(b.GetX() * cosf(rotation) - b.GetY() * sinf(rotation), b.GetX() * sinf(rotation) + b.GetY() * cosf(rotation)));
            result.push_back(Vector2D(c.GetX() * cosf(rotation) - c.GetY() * sinf(rotation), c.GetX() * sinf(rotation) + c.GetY() * cosf(rotation)));
            return result;
        }
        vector<Vector2D> Rotate(float rotation) override
        {
            vector<Vector2D> result;
            Vector2D a = pointA;
            Vector2D b = pointB;
            Vector2D c = pointC;
            result.push_back(Vector2D(a.GetX() * cosf(rotation) - a.GetY() * sinf(rotation), a.GetX() * sinf(rotation) + a.GetY() * cosf(rotation)));
            result.push_back(Vector2D(b.GetX() * cosf(rotation) - b.GetY() * sinf(rotation), b.GetX() * sinf(rotation) + b.GetY() * cosf(rotation)));
            result.push_back(Vector2D(c.GetX() * cosf(rotation) - c.GetY() * sinf(rotation), c.GetX() * sinf(rotation) + c.GetY() * cosf(rotation)));
            return result;
        }
        vector<Vector2D> Rotate(vector<Vector2D> vertices) override {}
        vector<Vector2D> ApplyOffset() override
        {
            vector<Vector2D> result;
            result.push_back(pointA + offset);
            result.push_back(pointB + offset);
            result.push_back(pointC + offset);
            return result;
        }
        vector<Vector2D> ApplyOffset(Vector2D offset)
        {
            vector<Vector2D> result;
            result.push_back(pointA + offset);
            result.push_back(pointB + offset);
            result.push_back(pointC + offset);
            return result;
        }
        vector<Vector2D> ApplyOffset(vector<Vector2D> offsets) override {}
        tuple<float, float, float, float> GetSegment() override
        {
            vector<float> x;
            x.push_back(pointA.GetX());
            x.push_back(pointB.GetX());
            x.push_back(pointC.GetX());
            vector<float> y;
            y.push_back(pointA.GetY());
            y.push_back(pointB.GetY());
            y.push_back(pointC.GetY());
            tuple<float, float, float, float> segment = make_tuple(*min_element(x.begin(), x.end()), *max_element(x.begin(), x.end()),
                                                                   *min_element(y.begin(), y.end()), *min_element(y.begin(), y.end()));
            return segment;
        }
    };
    class FreePolygon2D : public Shape2D
    {
    public:
        vector<Vector2D> vertices;
        vector<Triangle *> Triangulate()
        {
            vector<int> indices;
            vector<Triangle *> triangles;
            for (int i = 0; i < vertices.size(); i++)
            {
                indices.push_back(i);
            }
            while (indices.size() > 2)
            {
                for (int i = 0; i < indices.size() - 2; i++)
                {
                    Vector2D a = vertices[indices[i]];
                    Vector2D b = vertices[indices[i + 1]];
                    Vector2D c = vertices[indices[i + 2]];
                    Triangle *triangle = new Triangle();
                    bool test = false;
                    for (int j = 0; j < vertices.size(); j++)
                    {
                        if ((j < i || j > i + 2) && triangle->IsPointInside(vertices[j]))
                        {
                            test = true;
                            break;
                        }
                    }
                    if (!test)
                    {
                        triangles.push_back(triangle);
                        indices.erase(indices.begin() + i + 1);
                    }
                }
            }
            return triangles;
        }
        float GetArea() override
        {
            float area = 0.0;
            vector<Triangle *> triangles = Triangulate();
            for (Triangle *trianlge : triangles)
            {
                area += trianlge->GetArea();
            }
            return area;
        }
        bool IsPointInside(Vector2D point) override
        {
            if (!InSegment(point))
                return false;
            vector<Triangle *> triangles = Triangulate();
            for (Triangle *triangle : triangles)
            {
                if (triangle->IsPointInside(point))
                {
                    return true;
                }
            }
            return false;
        }
        vector<Vector2D> GetPoints() override
        {
            return vertices;
        }
        Vector2D GetCentroid() override
        {
            Vector2D sum = Vector2D();
            for (Vector2D v : vertices)
            {
                sum = sum + v;
            }
            return sum / (float)vertices.size();
        }
        tuple<float, float, float, float> GetSegment() override
        {
            vector<float> x;
            vector<float> y;
            for (Vector2D v : vertices)
            {
                x.push_back(v.GetX());
                y.push_back(v.GetY());
            }
            tuple<float, float, float, float> segment = make_tuple(*min_element(x.begin(), x.end()), *max_element(x.begin(), x.end()),
                                                                   *min_element(y.begin(), y.end()), *max_element(y.begin(), y.end()));
            return segment;
        }
        vector<Vector2D> ApplyOffset() override
        {
            vector<Vector2D> result;
            for (Vector2D v : vertices)
            {
                result.push_back(v + offset);
            }
            return result;
        }
        vector<Vector2D> ApplyOffset(Vector2D offset) override
        {
            vector<Vector2D> result;
            for (Vector2D v : vertices)
            {
                result.push_back(v + offset);
            }
            return result;
        }
        vector<Vector2D> ApplyOffset(vector<Vector2D> resource) override
        {
            vector<Vector2D> result;
            for (Vector2D v : resource)
            {
                result.push_back(v + offset);
            }
            return result;
        }
        vector<Vector2D> ApplyTransform() override {}
        vector<Vector2D> GetTransformedPoints() override {}
        vector<Vector2D> Rotate() override
        {
        }
        vector<Vector2D> Rotate(float rotation) override {}
        vector<Vector2D> Rotate(vector<Vector2D> vertices) override {}
    };
    FreePolygon2D *GetCircle(float radius = 1.0, int resolution = 16)
    {
        vector<Vector2D> vertices;
        for (int i = 0; i < resolution; i++)
        {
            float angle = 2 * pi / (float)resolution * (float)i;
            vertices.push_back(Vector2D(cosf(angle), sinf(angle)) * radius);
        }
        FreePolygon2D *circle = new FreePolygon2D();
        circle->vertices = vertices;
        return circle;
    }
}
