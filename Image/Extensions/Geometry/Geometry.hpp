#include <vector>
#include <tuple>
#include <limits>
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
        float GetX();
        float GetY();
        Vector2D();
        Vector2D(float x, float y);
        static const Vector2D up;
        static const Vector2D down;
        static const Vector2D left;
        static const Vector2D right;
        Vector2D operator+(Vector2D v);
        Vector2D operator-(Vector2D v);
        Vector2D operator*(float value);
        Vector2D operator/(float value);
        static bool OnSegment(Vector2D p, Vector2D q, Vector2D r);
        static Orientation GetOrientation(Vector2D p, Vector2D q, Vector2D r);
        static bool Intersecting(Vector2D p1, Vector2D q1, Vector2D p2, Vector2D q2);
        int PointX();
        int PointY();
        static float Distance(Vector2D a, Vector2D b);
        float Distance(Vector2D v);
        static Vector2D Center(std::vector<Vector2D> resource);
        static Vector2D ToCenter(std::vector<Vector2D> resource);
        static Vector2D Interpolate(Vector2D a, Vector2D b, float t);
        static Vector2D QuadraticInterpolate(Vector2D a, Vector2D b, Vector2D c, float t);
    };
    const Vector2D Vector2D::up = Vector2D(0, 1);
    const Vector2D Vector2D::down = Vector2D(0, -1);
    const Vector2D Vector2D::left = Vector2D(1, 0);
    const Vector2D Vector2D::right = Vector2D(-1, 0);
    class IOffset
    {
    public:
        Vector2D offset;
        virtual std::vector<Vector2D> ApplyOffset() = 0;
        virtual std::vector<Vector2D> ApplyOffset(Vector2D offset) = 0;
        virtual std::vector<Vector2D> ApplyOffset(std::vector<Vector2D> points) = 0;
        static std::vector<Vector2D> ApplyOffset(std::vector<Vector2D> vertices, Vector2D offset);
    };
    class IRotate
    {
    public:
        float rotation;
        virtual std::vector<Vector2D> Rotate() = 0;
        virtual std::vector<Vector2D> Rotate(float rotation) = 0;
        virtual std::vector<Vector2D> Rotate(std::vector<Vector2D> vertices) = 0;
        static std::vector<Vector2D> Rotate(std::vector<Vector2D> vertices, float rotation);
    };
    class IScale
    {
    public:
        Vector2D scale = Vector2D(1.0, 1.0);
        virtual std::vector<Vector2D> ApplyScale() = 0;
        virtual std::vector<Vector2D> ApplyScale(Vector2D scale) = 0;
        virtual std::vector<Vector2D> ApplyScale(std::vector<Vector2D> points) = 0;
    };
    class ITransform : public IRotate, public IOffset, public IScale
    {
    public:
        virtual std::vector<Vector2D> GetPoints() = 0;
        virtual std::vector<Vector2D> ApplyTransform() = 0;
        virtual std::vector<Vector2D> GetTransformedPoints() = 0;
    };
    class Shape2D : public ITransform
    {
    public:
        virtual float GetArea() = 0;
        virtual std::vector<Vector2D> GetPoints() = 0;
        virtual Vector2D GetCentroid() = 0;
        virtual std::tuple<float, float, float, float> GetSegment() = 0;
        virtual bool IsPointInside(Vector2D point) = 0;
        virtual bool InSegment(Vector2D point);
    };
    class Line2D : public ITransform
    {
    public:
        Vector2D from, to;
        Line2D();
        Line2D(Vector2D from, Vector2D to);
        ~Line2D();
        std::vector<Vector2D> GetPoints();
        Vector2D GetCentroid();
        std::vector<Vector2D> ApplyOffset() override;
        std::vector<Vector2D> ApplyOffset(Vector2D offset) override;
        std::vector<Vector2D> ApplyOffset(std::vector<Vector2D> points) override;
        bool IntesectingWith(Line2D *line);
    };
    class Triangle : public Shape2D
    {
    public:
        Vector2D pointA;
        Vector2D pointB;
        Vector2D pointC;
        Triangle();
        Triangle(Vector2D pointA, Vector2D pointB, Vector2D pointC);
        ~Triangle();
        float GetArea() override;
        Vector2D GetCentroid() override;
        std::vector<Vector2D> GetPointsInside();
        bool IsPointInside(Vector2D point);
        std::vector<Vector2D> GetPoints() override;
        std::vector<Vector2D> ApplyTransform() override;
        std::vector<Vector2D> GetTransformedPoints() override;
        std::vector<Vector2D> Rotate() override;
        std::vector<Vector2D> Rotate(float rotation) override;
        std::vector<Vector2D> Rotate(std::vector<Vector2D> vertices) override {}
        std::vector<Vector2D> ApplyOffset() override;
        std::vector<Vector2D> ApplyOffset(Vector2D offset);
        std::vector<Vector2D> ApplyOffset(std::vector<Vector2D> offsets) override {}
        std::tuple<float, float, float, float> GetSegment() override;
        std::vector<Vector2D> ApplyScale();
        std::vector<Vector2D> ApplyScale(std::vector<Vector2D> vectors);
        std::vector<Vector2D> ApplyScale(Vector2D scale);
    };
    class FreePolygon2D : public Shape2D
    {
    public:
        std::vector<Vector2D> vertices;
        std::vector<Triangle *> Triangulate();
        float GetArea() override;
        bool IsPointInside(Vector2D point) override;
        std::vector<Vector2D> GetPoints() override;
        Vector2D GetCentroid() override;
        std::tuple<float, float, float, float> GetSegment() override;
        std::vector<Vector2D> ApplyOffset() override;
        std::vector<Vector2D> ApplyOffset(Vector2D offset) override;
        std::vector<Vector2D> ApplyOffset(std::vector<Vector2D> resource) override;
        std::vector<Vector2D> ApplyTransform() override;
        std::vector<Vector2D> GetTransformedPoints() override;
        std::vector<Vector2D> Rotate() override;
        std::vector<Vector2D> Rotate(float rotation) override;
        std::vector<Vector2D> Rotate(std::vector<Vector2D> vertices) override;
        std::vector<Vector2D> ApplyScale();
        std::vector<Vector2D> ApplyScale(std::vector<Vector2D> vectors);
        std::vector<Vector2D> ApplyScale(Vector2D scale);
    };
    template <typename T = Shape2D, size_t size = std::numeric_limits<int>::max()>
    class ShapeGroup2D
    {
    private:
        std::vector<T *> elements;

    public:
        void AddElement(T *t);
        
    };
}
