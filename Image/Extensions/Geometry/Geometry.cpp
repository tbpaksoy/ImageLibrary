#include <vector>
#include <math.h>
#include <tuple>
#include <limits>
#include <algorithm>
#include <map>
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
        bool operator<(Vector2D v)
        {
            return GetMagnitude() < v.GetMagnitude();
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
        float GetMagnitude()
        {
            return sqrtf(x * x + y * y);
        }
        static tuple<float, float, float, float> GetSegment(vector<Vector2D> resource)
        {
            vector<float> x;
            vector<float> y;
            for (Vector2D v : resource)
            {
                x.push_back(v.x);
                y.push_back(v.y);
            }
            return make_tuple(
                *min_element(x.begin(), x.end()),
                *max_element(x.begin(), x.end()),
                *min_element(y.begin(), y.end()),
                *max_element(y.begin(), y.end()));
        }
        static tuple<float, float, float, float> GetSegment(vector<vector<Vector2D>> resource)
        {
            vector<float> x;
            vector<float> y;
            for (vector<Vector2D> v : resource)
            {
                for (Vector2D vv : v)
                {
                    x.push_back(vv.x);
                    y.push_back(vv.y);
                }
            }
            return make_tuple(
                *min_element(x.begin(), x.end()),
                *max_element(x.begin(), x.end()),
                *min_element(y.begin(), y.end()),
                *max_element(y.begin(), y.end()));
        }
        static tuple<float, float, float, float> GetSegment(vector<tuple<float, float, float, float>> resource)
        {
            vector<float> xMin, xMax, yMin, yMax;
            for (tuple<float, float, float, float> t : resource)
            {
                xMin.push_back(get<0>(t));
                xMax.push_back(get<1>(t));
                yMin.push_back(get<2>(t));
                xMax.push_back(get<3>(t));
            }
            return make_tuple(*min_element(xMin.begin(), xMin.end()), *max_element(xMax.begin(), xMax.end()),
                              *min_element(yMin.begin(), yMin.end()), *max_element(yMax.begin(), yMax.end()));
        }
        static Vector2D GetCenter(vector<Vector2D> resources)
        {
            Vector2D vector = Vector2D();
            for (Vector2D v : resources)
            {
                vector = v + vector;
            }
            return vector / resources.size();
        }
    };
    const Vector2D Vector2D::up = Vector2D(0, 1);
    const Vector2D Vector2D::down = Vector2D(0, -1);
    const Vector2D Vector2D::left = Vector2D(1, 0);
    const Vector2D Vector2D::right = Vector2D(-1, 0);
    class ITranslate
    {
    public:
        Vector2D offset;
        virtual vector<Vector2D> Translate() = 0;
        virtual vector<Vector2D> Translate(Vector2D offset) = 0;
        virtual vector<Vector2D> Translate(vector<Vector2D> points) = 0;
        static vector<Vector2D> Translate(vector<Vector2D> vertices, Vector2D offset)
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
        static vector<Vector2D> ApplyScale(vector<Vector2D> points, Vector2D scale)
        {
            vector<Vector2D> result;
            for (Vector2D v : points)
            {
                result.push_back(Vector2D(v.GetX() * scale.GetX(), v.GetY() * v.GetY()));
            }
            return result;
        }
    };
    class ITransform : public IRotate, public ITranslate
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
        virtual vector<Line2D *> ToLines()
        {
            vector<Line2D *> temp;
            vector<Vector2D> points = GetPoints();
            for (int i = 0; i < points.size() - 1; i++)
            {
                temp.push_back(new Line2D(points[i], points[i + 1]));
            }
            return temp;
        }
        virtual vector<Vector2D> GetOutLines()
        {
            vector<Vector2D> result;
            for (Line2D *line : ToLines())
            {
                for (Vector2D p : line->GetPoints())
                {
                    result.push_back(p);
                }
            }
            return result;
        }
        vector<Vector2D> Fit(int width, int height)
        {
            width--;
            height--;
            tuple<float, float, float, float> segment = Vector2D::GetSegment(GetPoints());
            Vector2D scale = Vector2D(width / (get<1>(segment) - get<0>(segment)), height / (get<3>(segment) - get<2>(segment)));
            vector<Vector2D> temp;
            for (Vector2D v : GetPoints())
            {
                temp.push_back(v);
            }
            Vector2D center = Vector2D::GetCenter(temp);
            for (int i = 0; i < temp.size(); i++)
            {
                temp[i] = temp[i] - center;
            }
            segment = Vector2D::GetSegment(temp);
            Vector2D offset = Vector2D(get<0>(segment), get<2>(segment));
            for (int i = 0; i < temp.size(); i++)
            {
                temp[i] = temp[i] - offset;
            }
            return temp;
        }
        vector<Vector2D> Fit(int width, int height, bool keepRatio)
        {
            if (keepRatio)
            {
                width--;
                height--;
                tuple<float, float, float, float> segment = Vector2D::GetSegment(GetPoints());
                float scale = min(width / (get<1>(segment) - get<0>(segment)), height / (get<3>(segment) - get<2>(segment)));
                vector<Vector2D> temp;
                for (Vector2D v : GetPoints())
                {
                    temp.push_back(v);
                }
                Vector2D center = Vector2D::GetCenter(temp);
                for (int i = 0; i < temp.size(); i++)
                {
                    temp[i] = temp[i] - center;
                }
                segment = Vector2D::GetSegment(temp);
                Vector2D offset = Vector2D(get<0>(segment), get<2>(segment));
                for (int i = 0; i < temp.size(); i++)
                {
                    temp[i] = temp[i] - offset;
                }
                return temp;
            }
            else
                return Fit(width, height);
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
        vector<Vector2D> Translate() override
        {
            vector<Vector2D> result;
            result.push_back(from + offset);
            result.push_back(to + offset);
            return result;
        }
        vector<Vector2D> Translate(Vector2D offset) override
        {
            vector<Vector2D> result;
            result.push_back(from + offset);
            result.push_back(to + offset);
            return result;
        }
        vector<Vector2D> Translate(vector<Vector2D> points) override
        {
            vector<Vector2D> result;
            for (Vector2D v : points)
            {
                result.push_back(v + offset);
            }
            return result;
        }
        vector<Vector2D> Rotate() override {}
        vector<Vector2D> Rotate(float rotation) {}
        vector<Vector2D> Rotate(vector<Vector2D> vertices) override {}
        vector<Vector2D> ApplyTransform() override {}
        vector<Vector2D> GetTransformedPoints() override {}
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
        vector<Vector2D> Translate() override
        {
            vector<Vector2D> result;
            result.push_back(pointA + offset);
            result.push_back(pointB + offset);
            result.push_back(pointC + offset);
            return result;
        }
        vector<Vector2D> Translate(Vector2D offset)
        {
            vector<Vector2D> result;
            result.push_back(pointA + offset);
            result.push_back(pointB + offset);
            result.push_back(pointC + offset);
            return result;
        }
        vector<Vector2D> Translate(vector<Vector2D> offsets) override {}
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
        vector<Vector2D> Translate() override
        {
            vector<Vector2D> result;
            for (Vector2D v : vertices)
            {
                result.push_back(v + offset);
            }
            return result;
        }
        vector<Vector2D> Translate(Vector2D offset) override
        {
            vector<Vector2D> result;
            for (Vector2D v : vertices)
            {
                result.push_back(v + offset);
            }
            return result;
        }
        vector<Vector2D> Translate(vector<Vector2D> resource) override
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
    class ShapeGroup
    {
    public:
        vector<Shape2D *> group;
        map<Shape2D *, Vector2D> origins;
        void Add(Shape2D *shape)
        {
            group.push_back(shape);
            origins.insert({shape, Vector2D::GetCenter(shape->GetPoints())});
        }
        vector<Vector2D> GetOutlines(Vector2D offset, Vector2D scale)
        {
            vector<Vector2D> temp;
            for (Shape2D *shape : group)
            {
                vector<Vector2D> buffer = shape->GetPoints();
                buffer = IScale::ApplyScale(buffer, scale);
                Vector2D calc = Vector2D(origins[shape].GetX() * scale.GetX(), origins[shape].GetY() * scale.GetY());
                buffer = ITranslate::Translate(buffer, calc + offset);
                vector<Line2D *> lines;
                for (int i = 0; i < buffer.size() - 1; i++)
                {
                    lines.push_back(new Line2D(buffer[i], buffer[i + 1]));
                }
                lines.push_back(new Line2D(buffer[0], buffer[buffer.size() - 1]));
                for (Line2D *line : lines)
                {
                    for (Vector2D v : line->GetPoints())
                    {
                        temp.push_back(v);
                    }
                }
                return temp;
            }
        }
        void RecalculateOrigins()
        {
            vector<Vector2D> allPoints;
            map<Shape2D *, Vector2D> localCenter;
            for (Shape2D *shape : group)
            {
                for (Vector2D point : shape->GetPoints())
                {
                    allPoints.push_back(point);
                }
                localCenter.insert(pair<Shape2D *, Vector2D>(shape, Vector2D::GetCenter(shape->GetPoints())));
            }
            Vector2D center = Vector2D::GetCenter(allPoints);
            vector<Shape2D *> keys;
            for (pair<Shape2D *, Vector2D> p : origins)
            {
                keys.push_back(p.first);
            }
            for (Shape2D *s : keys)
            {
                origins[s] = localCenter[s] - center;
            }
        }
        vector<tuple<float, float, float, float>> GetSegments()
        {
            vector<tuple<float, float, float, float>> segments;
            for (Shape2D *shape : group)
            {
                segments.push_back(shape->GetSegment());
            }
            return segments;
        }
        tuple<float, float, float, float> GetSegment()
        {
            return Vector2D::GetSegment(GetSegments());
        }
        vector<Vector2D> Fit(int width, int height)
        {
            width--;
            height--;
            vector<Vector2D> result;
            tuple<float, float, float, float> segment = GetSegment();
            Vector2D scale = Vector2D((float)width / abs(get<1>(segment) - get<0>(segment)), (float)height / abs(get<3>(segment) - get<2>(segment)));
            vector<Line2D *> lines;
            for (Shape2D *shape : group)
            {
                vector<Vector2D> points = shape->GetPoints();
                for (int i = 0; i < points.size(); i++)
                {
                    points[i] = Vector2D(points[i].GetX() * scale.GetX(), points[i].GetY() * scale.GetY());
                }
                for (int i = 0; i < points.size() - 1; i++)
                {
                    lines.push_back(new Line2D(points[i], points[i + 1]));
                    result.push_back(points[i]);
                }
                result.push_back(points[points.size() - 1]);
                lines.push_back(new Line2D(points[0], points[points.size() - 1]));
            }
            segment = Vector2D::GetSegment(result);
            scale = Vector2D((float)width / abs(get<1>(segment) - get<0>(segment)), (float)height / abs(get<3>(segment) - get<2>(segment)));
            for (int i = 0; i < lines.size(); i++)
            {
                lines[i]->from = Vector2D(scale.GetX() * lines[i]->from.GetX(), scale.GetY() * lines[i]->from.GetY());
                lines[i]->to = Vector2D(scale.GetX() * lines[i]->to.GetX(), scale.GetY() * lines[i]->to.GetY());
            }
            result.clear();
            for (Line2D *line : lines)
            {
                for (Vector2D v : line->GetPoints())
                {
                    result.push_back(v);
                }
            }
            segment = Vector2D::GetSegment(result);
            Vector2D offset = Vector2D(get<0>(segment), get<2>(segment));
            for (int i = 0; i < result.size(); i++)
            {   
                result[i] = result[i] - offset;
            }
            return result;
        }
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
