#include "Geometry.hpp"
#include <math.h>
#include <algorithm>
namespace Tahsin
{
    float Vector2D::GetX()
    {
        return x;
    }
    float Vector2D::GetY()
    {
        return y;
    }
    Vector2D::Vector2D()
    {
    }
    Vector2D::Vector2D(float x, float y)
    {
        (*this).x = x;
        (*this).y = y;
    }
    Vector2D Vector2D::operator+(Vector2D v)
    {
        return Vector2D(x + v.x, y + v.y);
    }
    Vector2D Vector2D::operator-(Vector2D v)
    {
        return Vector2D(x - v.x, y - v.y);
    }
    Vector2D Vector2D::operator*(float value)
    {
        return Vector2D(x * value, y * value);
    }
    Vector2D Vector2D::operator/(float value)
    {
        return Vector2D(x / value, y / value);
    }
    bool Vector2D::OnSegment(Vector2D p, Vector2D q, Vector2D r)
    {
        return q.x < std::max(p.x, r.x) && q.x > std::min(p.x, r.x) && q.y < std::max(p.y, r.y) && q.y > std::min(p.y, r.y);
    }
    Vector2D::Orientation Vector2D::GetOrientation(Vector2D p, Vector2D q, Vector2D r)
    {
        float value = (q.x - p.y) * (r.x - q.y) - (q.x - p.x) * (r.y - q.y);
        return value == 0 ? CL : value > 0 ? CW
                                           : CCW;
    }
    bool Vector2D::Intersecting(Vector2D p1, Vector2D q1, Vector2D p2, Vector2D q2)
    {
        float de = (p1.GetX() - q1.GetX()) * (p2.GetY() - q2.GetY()) - (p1.GetY() - q1.GetY()) * (p2.GetX() - q2.GetX());
        if (de == 0)
            return false;
        float x = (((p1.GetX() * q1.GetY() - p1.GetY() * q1.GetX())) * (p2.GetX() - q2.GetX()) - (p1.GetX() - q1.GetX()) * (p2.GetX() * q2.GetX() - p2.GetY() * q2.GetX())) / de;
        float y = (((p1.GetX() * q1.GetY() - p1.GetY() * q1.GetX())) * (p2.GetY() - q2.GetY()) - (p1.GetY() - q1.GetY()) * (p2.GetX() * q2.GetX() - p2.GetY() * q2.GetX())) / de;
        Vector2D temp = Vector2D(x, y);
        float a = p1.Distance(temp) + temp.Distance(q1);
        float b = p1.Distance(q1);
        float c = p2.Distance(temp) + temp.Distance(q2);
        float d = p2.Distance(q2);
        a = (int)(a * 1000);
        b = (int)(b * 1000);
        c = (int)(c * 1000);
        d = (int)(d * 1000);
        return a == b && c == d;
    }
    int Vector2D::PointX()
    {
        return (int)x;
    }
    int Vector2D::PointY()
    {
        return (int)y;
    }
    float Vector2D::Distance(Vector2D a, Vector2D b)
    {
        return sqrt(abs((a.x - b.x) * (a.y - b.y)));
    }
    float Vector2D::Distance(Vector2D v)
    {
        return sqrt(abs((x - v.x) * (y - v.y)));
    }
    Vector2D Vector2D::Center(std::vector<Vector2D> resource)
    {
        Vector2D center = Vector2D();
        for (Vector2D v : resource)
        {
            center = center + v;
        }
        center = center / resource.size();
        return center;
    }
    Vector2D Vector2D::ToCenter(std::vector<Vector2D> resource)
    {
        return Vector2D() - Center(resource);
    }
    Vector2D Vector2D::Interpolate(Vector2D a, Vector2D b, float t)
    {
        t = abs(t);
        if (t > 1)
            t -= (int)t;
        return a + (b - a) * t;
    }
    Vector2D Vector2D::QuadraticInterpolate(Vector2D a, Vector2D b, Vector2D c, float t)
    {
        return Interpolate(Interpolate(a, b, t), Interpolate(b, c, t), t);
    }
    bool Shape2D::InSegment(Vector2D point)
    {
        std::tuple<float, float, float, float> segment = GetSegment();
        float x = point.GetX(), y = point.GetY();
        return !(x < std::get<0>(segment) || x > std::get<1>(segment) || y < std::get<2>(segment) || y > std::get<3>(segment));
    }
    Line2D::Line2D() {}
    Line2D::Line2D(Vector2D from, Vector2D to)
    {
        this->from = from;
        this->to = to;
    }
    std::vector<Vector2D> Line2D::GetPoints()
    {
        std::vector<Vector2D> result;
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
    Vector2D Line2D::GetCentroid()
    {
        return (from + to) / 2;
    }
    std::vector<Vector2D> Line2D::ApplyOffset()
    {
        std::vector<Vector2D> result;
        result.push_back(from + offset);
        result.push_back(to + offset);
        return result;
    }
    std::vector<Vector2D> Line2D::ApplyOffset(Vector2D offset)
    {
        std::vector<Vector2D> result;
        result.push_back(from + offset);
        result.push_back(to + offset);
        return result;
    }
    std::vector<Vector2D> Line2D::ApplyOffset(std::vector<Vector2D> points)
    {
        std::vector<Vector2D> result;
        for (Vector2D v : points)
        {
            result.push_back(v + offset);
        }
        return result;
    }
    bool Line2D::IntesectingWith(Line2D *line)
    {
        return Vector2D::Intersecting(from, to, line->from, line->to);
    }
    Triangle::Triangle() {}
    Triangle::Triangle(Vector2D pointA, Vector2D pointB, Vector2D pointC)
    {
        this->pointA = pointA;
        this->pointB = pointB;
        this->pointC = pointC;
    }
    Triangle::~Triangle() {}
    float Triangle::GetArea()
    {
        return abs(pointA.GetX() * pointB.GetY() + pointB.GetX() * pointC.GetY() + pointC.GetX() * pointA.GetY() - pointB.GetY() * pointB.GetX() - pointB.GetY() * pointC.GetX() - pointC.GetY() * pointA.GetX());
    }
    Vector2D Triangle::GetCentroid()
    {
        return (pointA + pointB + pointC) / 3.0;
    }
    std::vector<Vector2D> Triangle::GetPointsInside()
    {
        std::vector<Vector2D> result;
        int yMax = (int)std::max(pointA.GetY(), std::max(pointB.GetY(), pointC.GetY()));
        int yMin = (int)std::min(pointA.GetY(), std::min(pointB.GetY(), pointC.GetY()));
        int xMax = (int)std::max(pointA.GetY(), std::max(pointB.GetY(), pointC.GetY()));
        int xMin = (int)std::min(pointA.GetY(), std::min(pointB.GetY(), pointC.GetY()));
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
    bool Triangle::IsPointInside(Vector2D point)
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
    std::vector<Vector2D> Triangle::GetPoints()
    {
        std::vector<Vector2D> result;
        result.push_back(pointA);
        result.push_back(pointB);
        result.push_back(pointC);
        return result;
    }
    std::vector<Vector2D> Triangle::ApplyTransform()
    {
        std::vector<Vector2D> tranformed = ApplyScale(ApplyOffset(Rotate()));
        pointA = tranformed[0];
        pointB = tranformed[1];
        pointC = tranformed[2];
        return tranformed;
    }
    std::vector<Vector2D> Triangle::GetTransformedPoints()
    {
        return ApplyScale(ApplyOffset(Rotate()));
    }
    std::vector<Vector2D> Triangle::Rotate()
    {
        std::vector<Vector2D> result;
        Vector2D a = pointA;
        Vector2D b = pointB;
        Vector2D c = pointC;
        result.push_back(Vector2D(a.GetX() * cosf(rotation) - a.GetY() * sinf(rotation), a.GetX() * sinf(rotation) + a.GetY() * cosf(rotation)));
        result.push_back(Vector2D(b.GetX() * cosf(rotation) - b.GetY() * sinf(rotation), b.GetX() * sinf(rotation) + b.GetY() * cosf(rotation)));
        result.push_back(Vector2D(c.GetX() * cosf(rotation) - c.GetY() * sinf(rotation), c.GetX() * sinf(rotation) + c.GetY() * cosf(rotation)));
        return result;
    }
    std::vector<Vector2D> Triangle::Rotate(float rotation)
    {
        std::vector<Vector2D> result;
        Vector2D a = pointA;
        Vector2D b = pointB;
        Vector2D c = pointC;
        result.push_back(Vector2D(a.GetX() * cosf(rotation) - a.GetY() * sinf(rotation), a.GetX() * sinf(rotation) + a.GetY() * cosf(rotation)));
        result.push_back(Vector2D(b.GetX() * cosf(rotation) - b.GetY() * sinf(rotation), b.GetX() * sinf(rotation) + b.GetY() * cosf(rotation)));
        result.push_back(Vector2D(c.GetX() * cosf(rotation) - c.GetY() * sinf(rotation), c.GetX() * sinf(rotation) + c.GetY() * cosf(rotation)));
        return result;
    }
    std::vector<Vector2D> Triangle::ApplyOffset()
    {
        std::vector<Vector2D> result;
        result.push_back(pointA + offset);
        result.push_back(pointB + offset);
        result.push_back(pointC + offset);
        return result;
    }
    std::vector<Vector2D> Triangle::ApplyOffset(Vector2D offset)
    {
        std::vector<Vector2D> result;
        result.push_back(pointA + offset);
        result.push_back(pointB + offset);
        result.push_back(pointC + offset);
        return result;
    }
    std::tuple<float, float, float, float> Triangle::GetSegment()
    {
        std::vector<float> x;
        x.push_back(pointA.GetX());
        x.push_back(pointB.GetX());
        x.push_back(pointC.GetX());
        std::vector<float> y;
        y.push_back(pointA.GetY());
        y.push_back(pointB.GetY());
        y.push_back(pointC.GetY());
        std::tuple<float, float, float, float> segment = std::make_tuple(*min_element(x.begin(), x.end()), *max_element(x.begin(), x.end()),
                                                                         *min_element(y.begin(), y.end()), *min_element(y.begin(), y.end()));
        return segment;
    }
    std::vector<Vector2D> Triangle::ApplyScale()
    {
        Vector2D center = (pointA + pointB + pointC) / 3;
        std::vector<Vector2D> result;
        result.push_back(Vector2D((pointA.GetX() - center.GetX()) * scale.GetX(), (pointA.GetY() - center.GetY()) * scale.GetY()) + center);
        result.push_back(Vector2D((pointB.GetX() - center.GetX()) * scale.GetX(), (pointB.GetY() - center.GetY()) * scale.GetY()) + center);
        result.push_back(Vector2D((pointC.GetX() - center.GetX()) * scale.GetX(), (pointC.GetY() - center.GetY()) * scale.GetY()) + center);
        return result;
    }
    std::vector<Vector2D> Triangle::ApplyScale(std::vector<Vector2D> vectors)
    {
        Vector2D center = Vector2D::Center(vectors);
        std::vector<Vector2D> result;
        for (Vector2D v : vectors)
        {
            result.push_back(Vector2D((v.GetX() - center.GetX()) * scale.GetX(), (v.GetY() - center.GetY()) * scale.GetY()) + center);
        }
        return result;
    }
    std::vector<Vector2D> Triangle::ApplyScale(Vector2D scale)
    {
        Vector2D center = (pointA + pointB + pointC) / 3;
        std::vector<Vector2D> result;
        result.push_back(Vector2D((pointA.GetX() - center.GetX()) * scale.GetX(), (pointA.GetY() - center.GetY()) * scale.GetY()) + center);
        result.push_back(Vector2D((pointB.GetX() - center.GetX()) * scale.GetX(), (pointB.GetY() - center.GetY()) * scale.GetY()) + center);
        result.push_back(Vector2D((pointC.GetX() - center.GetX()) * scale.GetX(), (pointC.GetY() - center.GetY()) * scale.GetY()) + center);
        return result;
    }
    std::vector<Triangle *> FreePolygon2D::Triangulate()
    {
        std::vector<int> indices;
        std::vector<Triangle *> triangles;
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
    float FreePolygon2D::GetArea()
    {

        float area = 0.0;
        std::vector<Triangle *> triangles = Triangulate();
        for (Triangle *trianlge : triangles)
        {
            area += trianlge->GetArea();
        }
        return area;
    }
    bool FreePolygon2D::IsPointInside(Vector2D point)
    {
        if (!InSegment(point))
            return false;
        std::vector<Triangle *> triangles = Triangulate();
        for (Triangle *triangle : triangles)
        {
            if (triangle->IsPointInside(point))
            {
                return true;
            }
        }
        return false;
    }
    std::vector<Vector2D> FreePolygon2D::GetPoints()
    {
        return vertices;
    }
    Vector2D FreePolygon2D::GetCentroid()
    {
        return Vector2D::Center(vertices);
    }
    std::tuple<float, float, float, float> FreePolygon2D::GetSegment()
    {
        std::vector<float> x;
        std::vector<float> y;
        for (Vector2D v : vertices)
        {
            x.push_back(v.GetX());
            y.push_back(v.GetY());
        }
        std::tuple<float, float, float, float> segment = std::make_tuple(*min_element(x.begin(), x.end()), *max_element(x.begin(), x.end()),
                                                                         *min_element(y.begin(), y.end()), *max_element(y.begin(), y.end()));
        return segment;
    }
    std::vector<Vector2D> FreePolygon2D::ApplyOffset()
    {
        std::vector<Vector2D> result;
        for (Vector2D v : vertices)
        {
            result.push_back(v + offset);
        }
        return result;
    }
    std::vector<Vector2D> FreePolygon2D::ApplyOffset(Vector2D offset)
    {
        std::vector<Vector2D> result;
        for (Vector2D v : vertices)
        {
            result.push_back(v + offset);
        }
        return result;
    }
    std::vector<Vector2D> FreePolygon2D::ApplyOffset(std::vector<Vector2D> resource)
    {
        std::vector<Vector2D> result;
        for (Vector2D v : resource)
        {
            result.push_back(v + offset);
        }
        return result;
    }
    std::vector<Vector2D> FreePolygon2D::ApplyTransform()
    {
        vertices = ApplyScale(ApplyOffset(Rotate()));
        scale = Vector2D(1, 1);
        offset = Vector2D();
        rotation = 0;
        return vertices;
    }
    std::vector<Vector2D> FreePolygon2D::GetTransformedPoints()
    {
        return ApplyScale(ApplyOffset(Rotate()));
    }
    std::vector<Vector2D> FreePolygon2D::Rotate()
    {
        std::vector<Vector2D> result;
        for (Vector2D v : vertices)
        {
            float x = v.GetX() * cos(rotation) - v.GetY() * sin(rotation);
            float y = v.GetX() * sin(rotation) + v.GetY() * cos(rotation);
            result.push_back(Vector2D(x, y));
        }
        return result;
    }
    std::vector<Vector2D> FreePolygon2D::Rotate(float rotation)
    {
        std::vector<Vector2D> result;
        for (Vector2D v : vertices)
        {
            float x = v.GetX() * cos(rotation) - v.GetY() * sin(rotation);
            float y = v.GetX() * sin(rotation) + v.GetY() * cos(rotation);
            result.push_back(Vector2D(x, y));
        }
        return result;
    }
    std::vector<Vector2D> FreePolygon2D::Rotate(std::vector<Vector2D> resource)
    {
        std::vector<Vector2D> result;
        for (Vector2D v : resource)
        {
            float x = v.GetX() * cos(rotation) - v.GetY() * sin(rotation);
            float y = v.GetX() * sin(rotation) + v.GetY() * cos(rotation);
            result.push_back(Vector2D(x, y));
        }
        return result;
    }
    std::vector<Vector2D> FreePolygon2D::ApplyScale()
    {
        std::vector<Vector2D> result;
        Vector2D center = GetCentroid();
        for (Vector2D v : vertices)
        {
            float x = (v.GetX() - center.GetX()) * scale.GetX() + center.GetX();
            float y = (v.GetY() - center.GetY()) * scale.GetY() + center.GetY();
            result.push_back(Vector2D(x, y));
        }
        return result;
    }
    std::vector<Vector2D> FreePolygon2D::ApplyScale(std::vector<Vector2D> vectors)
    {
        std::vector<Vector2D> result;
        Vector2D center = GetCentroid();
        for (Vector2D v : vectors)
        {
            float x = (v.GetX() - center.GetX()) * scale.GetX() + center.GetX();
            float y = (v.GetY() - center.GetY()) * scale.GetY() + center.GetY();
            result.push_back(Vector2D(x, y));
        }
        return result;
    }
    std::vector<Vector2D> FreePolygon2D::ApplyScale(Vector2D scale)
    {
        std::vector<Vector2D> result;
        Vector2D center = GetCentroid();
        for (Vector2D v : vertices)
        {
            float x = (v.GetX() - center.GetX()) * scale.GetX() + center.GetX();
            float y = (v.GetY() - center.GetY()) * scale.GetY() + center.GetY();
            result.push_back(Vector2D(x, y));
        }
        return result;
    }
}