#include <string>
#include "ValueConversation.cpp"
#include <vector>
using namespace std;
namespace Tahsin
{
    struct Color
    {
    public:
        char r, g, b, a;
        Color();
        Color(char r, char g, char b, char a);
        Color(string hex);
        void GetFromHex(string hex);
        Color GetMidColor(Color color);
        Color GetNegative();
    };
    Color::Color()
    {
        r = g = b = a = 0;
    }
    Color::Color(char r, char g, char b, char a)
    {
        this->r = r;
        this->b = b;
        this->g = g;
        this->a = a;
    }
    Color::Color(string hex)
    {
        r = g = b = a = 0;
        if (IsAcceptable(hex))
        {
            switch (hex.size())
            {
            case 2:
                r = GetDecimalValue(hex);
                break;
            case 4:
                r = GetDecimalValue(hex.substr(0, 2));
                g = GetDecimalValue(hex.substr(2, 2));
                break;
            case 6:
                r = GetDecimalValue(hex.substr(0, 2));
                g = GetDecimalValue(hex.substr(2, 2));
                b = GetDecimalValue(hex.substr(4, 2));
                break;
            case 8:
                r = GetDecimalValue(hex.substr(0, 2));
                g = GetDecimalValue(hex.substr(2, 2));
                b = GetDecimalValue(hex.substr(4, 2));
                a = GetDecimalValue(hex.substr(6, 8));
                break;
            }
        }
    }
    void Color::GetFromHex(string hex)
    {
        if (IsAcceptable(hex))
        {
            switch (hex.size())
            {
            case 2:
                r = GetDecimalValue(hex);
                break;
            case 4:
                r = GetDecimalValue(hex.substr(0, 2));
                g = GetDecimalValue(hex.substr(2, 2));
                break;
            case 6:
                r = GetDecimalValue(hex.substr(0, 2));
                g = GetDecimalValue(hex.substr(2, 2));
                b = GetDecimalValue(hex.substr(4, 2));
                break;
            case 8:
                r = GetDecimalValue(hex.substr(0, 2));
                g = GetDecimalValue(hex.substr(2, 2));
                b = GetDecimalValue(hex.substr(4, 2));
                a = GetDecimalValue(hex.substr(6, 8));
                break;
            }
        }
    }
    Color Color::GetMidColor(Color color)
    {
        char r = (this->r + color.r) / 2, g = (this->g + color.g) / 2, b = (this->b + color.b) / 2, a = (this->a + color.a) / 2;
        return Color(r, g, b, a);
    }
    Color Color::GetNegative()
    {
        return Color(255 - r, 255 - g, 255 - b, 255 - a);
    }
    vector<vector<Color>> CreateColorTable()
    {
    }
}