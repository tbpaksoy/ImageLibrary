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
        void GetFromHex(string hex);
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

    vector<vector<Color>> CreateColorTable()
    {
        
    }
}