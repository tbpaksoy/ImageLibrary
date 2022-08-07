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
        virtual Vector2D *GetOuterPoints()
        {
            Vector2D result[5];
            return result; 
        }
    };
}
