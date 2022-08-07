#include <vector>
using namespace std;
namespace Tahsin
{

    struct Vector2D
    {
    private:
        float x, y;

    public:
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
    class Line
    {
    public:
        Vector2D from, to;
        Line()
        {
        }
        Line(Vector2D from, Vector2D to)
        {
            this->from = from;
            this->to = to;
        }
        ~Line() {}
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
        float GetArea() override
        {
            return abs(pointA.GetX() * pointB.GetY() + pointB.GetX() * pointC.GetY() + pointC.GetX() * pointA.GetY() - pointB.GetY() * pointB.GetX() - pointB.GetY() * pointC.GetX() - pointC.GetY() * pointA.GetX());
        }
        Vector2D GetCentroid() override
        {
            return (pointA + pointB + pointC) / 3;
        }
    };
}
