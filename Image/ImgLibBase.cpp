#include <iostream>
#include <string>
#include <array>
#include <sstream>
#include <vector>
#include <limits>
using namespace std;

namespace Tahsin
{
    const string accepted = "0123456789ABCDEFabcdef";
    int length = 8;
    bool forceUpper = true;
    float GoToValue(float a, float b)
    {
        if (b < a)
            return -abs(b - a);
        else
            return abs(a - b);
    }
    bool IsAcceptable(char c)
    {
        for (int i = 0; i < accepted.size(); i++)
        {
            if (c == accepted[i])
                return true;
        }
        return false;
    }
    bool IsAcceptable(string s)
    {
        for (int i = 0; i < s.size(); i++)
        {
            if (!IsAcceptable(s[i]))
            {
                return false;
            }
        }
        return true;
    }

    string FormatString(string s)
    {
        int decised = max((int)s.size(), length);
        stringstream ss;
        for (int i = 0; i < decised; i++)
        {
            if (i < s.size())
            {
                if (!isdigit(s[i]) && forceUpper)
                {
                    ss << (unsigned char)toupper(s[i]);
                }
                else
                {
                    ss << s[i];
                }
            }
            else
            {
                string temp = ss.str();
                ss.str("");
                ss << "0" << temp;
            }
        }
        return ss.str();
    }

    string GetHexValue(int value)
    {
        stringstream ss;
        ss << hex << value;
        string result = ss.str();
        while (result.size() < length)
        {
            result = "0" + result;
        }
        return result;
    }
    int GetDecimalValue(string s)
    {
        for (int i = 0; i < s.size(); i++)
        {
            if (!IsAcceptable(s[i]))
                throw invalid_argument("Unacceptable character.");
        }

        if (s.size() == 2 || s.size() == 1)
        {
            stringstream ss;
            ss << s;
            int result = 0;
            ss >> hex >> result;
            return result;
        }
        else
        {
            throw invalid_argument("Invalid string size:" + s.size());
        }
    }
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
    string *GroupString(string source, int length)
    {
        if (source.size() % length != 0)
        {
            return NULL;
        }
        else
        {
            string *result = new string[source.size() / length];
            for (int i = 0; i < source.size() / length; i++)
            {
                result[i] = source.substr(i * length, length);
            }
            return result;
        }
    }
    string *ReverseGroup(string source[])
    {
        string *result = new string[source->size()];
        for (int i = 0; i < source->size(); i++)
        {
            result[i] = source[source->size() - 1 - i];
        }
        return result;
    }
    string *ReverseGroup(string *source, int size)
    {
        string *result = new string[size];
        for (int i = 0; i < size; i++)
        {
            result[i] = source[size - 1 - i];
        }
        return result;
    }
    string *GroupAndReverse(string source, int length)
    {
        string *temp = GroupString(source, length);
        string *result = ReverseGroup(temp, source.size() / length);
        return result;
    }
    string UniteGroup(string source[])
    {
        string result = NULL;
        for (int i = 0; i < result.size(); i++)
        {
            result += source[i];
        }
        return result;
    }
    const char *ToByteArray(vector<string> source)
    {
        char *result = new char[source.size()];
        for (int i = 0; i < source.size(); i++)
        {
            result[i] = (char)GetDecimalValue(source[i]);
        }

        return result;
    }
    template <typename T>
    int GetTotalLength(vector<vector<T>> source)
    {
        int result = 0;
        if (source.size() == NULL)
        {
            for (vector<T> v : source)
            {
                if (v.size() != NULL)
                {
                    result += v.size();
                }
            }
        }
        return result;
    }
    template <typename T>
    bool IsHomogenious(vector<vector<T>> source)
    {
        if (source.size() != 0)
        {
            for (int i = 1; i < source.size(); i++)
            {
                if (source[i - 1].size() == 0 || source[i].size() == 0 || source[i - 1].size() != source[i].size())
                    return false;
            }
        }
        else
            return false;
        return true;
    }
    template <typename T>
    bool IsItQuadratic(vector<vector<T>> source)
    {
        return sqrtf(GetTotalLength(source)) - (int)sqrtf(GetTotalLength(source)) == 0;
    }
    template <typename T>
    int GetMeanOfLength(vector<vector<T>> source)
    {
        int result = 0;
        for (int i = 0; i < source.size(); i++)
        {
            result += source[i].size();
        }
        return result /= source.size();
    }
    template <typename T>
    vector<vector<T>> ScaleVector(vector<vector<T>> source, int x = 10, int y = 10)
    {
        vector<vector<T>> result;
        for (int i = 0; i < source.size() * y; i++)
        {
            vector<T> sub;
            for (int j = 0; j < source[0].size() * x; j++)
            {
                sub.push_back(source[i / y][j / x]);
            }
            result.push_back(sub);
        }
        return result;
    }
    vector<vector<Color>> CreateColorTransiton(vector<Color> a, vector<Color> b, int step = 5)
    {
        int decised = min(a.size(), b.size());
        vector<vector<Color>> result;
        for (int i = 0; i < decised; i++)
        {
            vector<Color> transition;
            Color from = a[i];
            Color to = b[i];
            for (int j = 0; j < step + 2; j++)
            {
                char r = from.r + (char)(GoToValue(from.r, to.r) * j / step);
                char g = from.g + (char)(GoToValue(from.g, to.g) * j / step);
                char b = from.b + (char)(GoToValue(from.b, to.b) * j / step);
                char a = from.a + (char)(GoToValue(from.a, to.a) * j / step);
                transition.push_back(Color(r, g, b, a));
            }
        }
        return result;
    }
    vector<vector<Color>> CreateColorVariants(Color color, int width = 5, int height = 5)
    {
        Color white = Color(255, 255, 255, 255);
        Color black = Color(255, 255, 255, 255);
        vector<Color> toWhite;
        vector<Color> toBlack;
        toWhite.push_back(color);
        toBlack.push_back(color);
        for (int i = 0; i < width; i++)
        {
            char r = (char)(color.r + GoToValue(color.r, black.r) * i / width);
            char g = (char)(color.g + GoToValue(color.g, black.g) * i / width);
            char b = (char)(color.b + GoToValue(color.b, black.b) * i / width);
            char a = (char)(color.a + GoToValue(color.a, black.a) * i / width);
            toBlack.push_back(Color(r, g, b, a));
        }
        for (int i = 0; i < height; i++)
        {
            char r = (char)(color.r + GoToValue(color.r, black.r) * i / width);
            char g = (char)(color.g + GoToValue(color.g, black.g) * i / width);
            char b = (char)(color.b + GoToValue(color.b, black.b) * i / width);
            char a = (char)(color.a + GoToValue(color.a, black.a) * i / width);
            toWhite.push_back(Color(r, g, b, a));
        }
        vector<vector<Color>> result;
        result.push_back(toBlack);
        for (int i = 0; i < height; i++)
        {
            vector<Color> temp;
            temp.push_back(toWhite[i]);
            for (int j = 0; j < width; j++)
            {
                temp.push_back(toWhite[i].GetMidColor(toBlack[j]));
            }
            result.push_back(temp);
        }
        return result;
    }
    vector<vector<Color>> CreateMidColorTable(vector<Color> a, vector<Color> b)
    {
        vector<vector<Color>> result;
        for (int i = 0; i < a.size(); i++)
        {
            vector<Color> temp;
            for (int j = 0; j < b.size(); j++)
            {
                if (i == 0)
                {
                    temp.push_back(a[i]);
                }
                else if (j == 0)
                {
                    temp.push_back(b[j]);
                }
                else
                {
                    temp.push_back(a[i].GetMidColor(b[j]));
                }
            }
            result.push_back(temp);
        }
        return result;
    }
    int Max(vector<int> resource)
    {
        int result = numeric_limits<int>::min();
        for (int i : resource)
        {
            if (i > result)
            {
                result = i;
            }
        }
        return result;
    }
    float Max(vector<float> resource)
    {
        float result = numeric_limits<float>::min();
        for (float f : resource)
        {
            if (f > result)
            {
                result = f;
            }
        }
        return result;
    }
    int Min(vector<int> resource)
    {
        int result = numeric_limits<int>::max();
        for (int i : resource)
        {
            if (i < result)
            {
                result = i;
            }
        }
        return result;
    }
    float Min(vector<float> resource)
    {
        float result = numeric_limits<float>::max();
        for (float f : resource)
        {
            if (f < result)
            {
                result = f;
            }
        }
        return result;
    }
}