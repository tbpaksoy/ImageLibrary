#include <vector>
using namespace std;
namespace Tahsin
{

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
    };
    const Vector2D Vector2D::up = Vector2D(0, 1);
    const Vector2D Vector2D::down = Vector2D(0, -1);
    const Vector2D Vector2D::left = Vector2D(1, 0);
    const Vector2D Vector2D::right = Vector2D(-1, 0);
    class Shape2D
    {
    public:
        virtual float GetArea() = 0;
        virtual vector<Vector2D> GetPoints() = 0;
        virtual Vector2D GetCentroid() = 0;
    };
    class Line2D
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
            int dx = (int)(to.GetX() - from.GetX());
            int dy = (int)(to.GetY() - from.GetY());
            int d = 2 * dy - dx;
            int y = 0;
            vector<Vector2D> result;
            for (int x = (int)from.GetX(); x < (int)to.GetX(); x++)
            {
                result.push_back(Vector2D((int)x, (int)y));
                if (d > 0)
                {
                    y++;
                    d -= 2 * dx;
                }
                d += 2 * dy;
            }
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
    };

}
