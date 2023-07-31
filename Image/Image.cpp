#include "Image.hpp"
#include <sstream>
#include <limits>
#include <math.h>
#include <string>
#include <fstream>
#include <algorithm>
#include <iostream>
namespace Tahsin
{
    std::string Color::GetHex()
    {
        std::string result;
        std::stringstream ss;
        ss << std::hex << r;
        result += ss.str();
        ss.str(nullptr);
        ss << std::hex << g;
        result += ss.str();
        ss.str(nullptr);
        ss << std::hex << b;
        result += ss.str();
        ss.str(nullptr);
        ss << std::hex << a;
        result += ss.str();
        return result;
    }
    Color Color::GetNegative()
    {
        char max = std::numeric_limits<char>::max();
        return Color(max - r, max - g, max - b);
    }
    Color Color::GetGreyScale()
    {
        float value = (r + g + b) / 3;
        return Color(value, value, value);
    }
    Color Color::RedMask()
    {
        return Color(r, r, r);
    }
    Color Color::GreenMask()
    {
        return Color(g, g, g);
    }
    Color Color::BlueMask()
    {
        return Color(b, b, b);
    }
    Color Color::AlphaMask()
    {
        return Color(a, a, a);
    }
    Color::Color() {}
    Color::Color(unsigned char r, unsigned char g, unsigned char b)
    {
        this->r = r;
        this->g = g;
        this->b = b;
        this->a = 255;
    }
    Color::Color(unsigned char r, unsigned char g, unsigned char b, unsigned char a)
    {
        this->r = r;
        this->g = g;
        this->b = b;
        this->a = a;
    }
    Color::Color(std::string hexValue) {}
    Color Color::GetMidColor(Color color)
    {
        return Color((r + color.r) / 2, (g + color.g) / 2, (b + color.b) / 2, (a + color.a) / 2);
    }
    Color Color::operator*(float value)
    {
        unsigned char max = std::numeric_limits<unsigned char>::max();
        unsigned char r = (unsigned char)std::min((float)max, (float)(r * value));
        unsigned char g = (unsigned char)std::min((float)max, (float)(g * value));
        unsigned char b = (unsigned char)std::min((float)max, (float)(b * value));
        unsigned char a = (unsigned char)std::min((float)max, (float)(a * value));
        return Color(r, g, b, a);
    }
    Color Color::operator+(char value)
    {
        unsigned char max = std::numeric_limits<unsigned char>::max();
        unsigned char r = (unsigned char)std::min((float)max, (float)(r + value));
        unsigned char g = (unsigned char)std::min((float)max, (float)(g + value));
        unsigned char b = (unsigned char)std::min((float)max, (float)(b + value));
        unsigned char a = (unsigned char)std::min((float)max, (float)(a + value));
        return Color(r, g, b, a);
    }
    std::vector<std::vector<Color>> GenerateColorVariants(Color source, int width, int height)
    {
        Color white = Color(255, 255, 255, 255);
        Color black = Color(0, 0, 0, 255);
        std::vector<Color> toWhite;
        std::vector<Color> toBlack;
        toWhite.push_back(source);
        toBlack.push_back(source);
        for (int i = 0; i < width; i++)
        {
            unsigned char r = (unsigned char)(source.r - (source.r - black.r) * (float)i / width);
            unsigned char g = (unsigned char)(source.g - (source.g - black.g) * (float)i / width);
            unsigned char b = (unsigned char)(source.b - (source.b - black.b) * (float)i / width);
            unsigned char a = (unsigned char)(source.a - (source.a - black.a) * (float)i / width);
            toBlack.push_back(Color(r, g, b, a));
        }
        for (int i = 0; i < height; i++)
        {
            unsigned char r = (unsigned char)(source.r - (source.r - white.r) * (float)i / width);
            unsigned char g = (unsigned char)(source.g - (source.g - white.g) * (float)i / width);
            unsigned char b = (unsigned char)(source.b - (source.b - white.b) * (float)i / width);
            unsigned char a = (unsigned char)(source.a - (source.a - white.a) * (float)i / width);
            toWhite.push_back(Color(r, g, b, a));
        }
        std::vector<std::vector<Color>> result;
        result.push_back(toBlack);
        for (int i = 1; i < height; i++)
        {
            std::vector<Color> temp;
            temp.push_back(toWhite[i]);
            for (int j = 0; j < width; j++)
            {
                temp.push_back(toWhite[i].GetMidColor(toBlack[j]));
            }
            result.push_back(temp);
        }
        return result;
    }
    std::vector<std::vector<Color>> GenerateMidColorTable(std::vector<Color> a, std::vector<Color> b)
    {
        std::vector<std::vector<Color>> result;
        for (int i = 0; i < a.size(); i++)
        {
            std::vector<Color> temp;
            temp.push_back(a[i]);
            for (int j = 0; j < b.size(); j++)
            {
                temp.push_back(a[i].GetMidColor(b[j]));
            }
            result.push_back(temp);
        }
        result.push_back(b);
        return result;
    }
    std::vector<std::vector<Color>> ScaleColorArray(std::vector<std::vector<Color>> resource, int scaleX, int scaleY)
    {
        std::vector<std::vector<Color>> result;
        result.resize(resource.size() * scaleX, std::vector<Color>(resource[0].size() * scaleY));
        for (size_t i = 0; i < result.size(); i++)
        {
            for (size_t j = 0; j < result[i].size(); j++)
            {
                result[i][j] = resource[i / scaleX][j / scaleY];
            }
        }
        return result;
    }
    std::vector<std::vector<Color>> GenerateColorTransition(std::vector<Color> from, std::vector<Color> to, int step)
    {

        std::vector<std::vector<Color>> palette;
        palette.resize(from.size(), std::vector<Color>(step + 2));
        for (int i = 0; i < palette.size(); i++)
        {
            for (int j = 1; j < step + 1; j++)
            {
                unsigned char r = (unsigned char)(from[i].r + (to[i].r - from[i].r) * (float)j / step);
                unsigned char g = (unsigned char)(from[i].g + (to[i].g - from[i].g) * (float)j / step);
                unsigned char b = (unsigned char)(from[i].b + (to[i].b - from[i].b) * (float)j / step);
                unsigned char a = (unsigned char)(from[i].a + (to[i].a - from[i].a) * (float)j / step);
                palette[i][j] = Color(r, g, b, a);
            }
            palette[i][step + 1] = to[i];
            palette[i][0] = from[i];
        }
        return palette;
    }
    std::vector<std::vector<Color>> MirrorVertical(std::vector<std::vector<Color>> source)
    {
        std::vector<std::vector<Color>> result;
        result.resize(source.size(), std::vector<Color>(source[0].size()));
        for (size_t i = 0; i < result.size(); i++)
        {
            for (size_t j = 0; j < result[i].size(); j++)
            {
                result[i][j] = source[i][result[i].size() - j - 1];
            }
        }
        return result;
    }
    std::vector<std::vector<Color>> Subversion(std::vector<std::vector<Color>> source)
    {
        std::vector<std::vector<Color>> result;
        result.resize(source[0].size(), std::vector<Color>(source.size()));
        for (size_t i = 0; i < result.size(); i++)
        {
            for (size_t j = 0; j < result[0].size(); j++)
            {
                result[i][j] = source[j][i];
            }
        }
        return result;
    }
    std::vector<std::vector<Color>> Contrast(std::vector<std::vector<Color>> source, float value)
    {
        std::vector<std::vector<Color>> result;
        result.resize(source.size(), std::vector<Color>(source[0].size()));
        for (size_t i = 0; i < result.size(); i++)
        {
            for (size_t j = 0; j < result[0].size(); j++)
            {
                result[i][j] = source[i][j] * value;
            }
        }
        return result;
    }
    void ApplyContrast(std::vector<std::vector<Color>> &source, float value)
    {
        for (size_t i = 0; i < source.size(); i++)
        {
            for (size_t j = 0; j < source[0].size(); j++)
            {
                source[i][j] = source[i][j] * value;
            }
        }
    }
    std::vector<std::vector<Color>> Brightness(std::vector<std::vector<Color>> source, float value)
    {
        std::vector<std::vector<Color>> result;
        result.resize(source.size(), std::vector<Color>(source[0].size()));
        for (size_t i = 0; i < result.size(); i++)
        {
            for (size_t j = 0; j < result[0].size(); j++)
            {
                result[i][j] = source[i][j] + value;
            }
        }
        return result;
    }
    void ApplyBrightness(std::vector<std::vector<Color>> &source, int value)
    {
        for (size_t i = 0; i < source.size(); i++)
        {
            for (size_t j = 0; j < source[0].size(); j++)
            {
                source[i][j] = source[i][j] + value;
            }
        }
    }
    std::vector<char> GetBMPHeader(std::vector<std::vector<Color>> source)
    {
        std::vector<char> data;
        std::stringstream ss;
        data.push_back('B');
        data.push_back('M');
        int paddingCount = source[0].size() * 3 % 4;
        int value = 54 + source[0].size() * source.size() * 3 + source.size() * paddingCount;
        ss << std::hex << value;
        std::string s = ss.str();
        ss.str("");
        while (s.size() < 8)
        {
            s = "0" + s;
        }
        std::vector<char> temp(s.length() / 2);
        for (int i = 0; i < temp.size(); i++)
        {
            temp[i] = stoi(s.substr(i * 2, 2), nullptr, 16);
        }
        reverse(temp.begin(), temp.end());
        for (char c : temp)
        {
            data.push_back(c);
        }

        for (int i = 0; i < 4; i++)
            data.push_back(0);

        data.push_back(54);
        data.push_back(0);
        data.push_back(0);
        data.push_back(0);
        data.push_back(40);
        data.push_back(0);
        data.push_back(0);
        data.push_back(0);

        value = source[0].size();
        ss << value;
        s.clear();
        s = ss.str();
        ss.str("");
        while (s.size() < 8)
        {
            s = "0" + s;
        }
        temp.clear();
        temp.resize(s.length() / 2);
        for (int i = 0; i < temp.size(); i++)
        {
            temp[i] = stoi(s.substr(i * 2, 2), nullptr, 16);
        }
        reverse(temp.begin(), temp.end());
        for (char c : temp)
        {
            data.push_back(c);
        }

        temp.clear();
        value = source.size();
        ss << std::hex << value;
        s.clear();
        s = ss.str();
        ss.str("");
        while (s.size() < 8)
        {
            s = "0" + s;
        }

        temp.resize(s.length() / 2);
        for (int i = 0; i < temp.size(); i++)
        {
            temp[i] = stoi(s.substr(i * 2, 2), nullptr, 16);
        }
        reverse(temp.begin(), temp.end());
        for (char c : temp)
        {
            data.push_back(c);
        }
        data.push_back(1);
        data.push_back(0);

        data.push_back(24);
        data.push_back(0);

        data.push_back(0);
        data.push_back(0);
        data.push_back(0);
        data.push_back(0);

        value = (source[0].size() + paddingCount) * 3 * source.size();
        ss << std::hex << value;
        s = ss.str();
        ss.str("");
        while (s.size() < 8)
        {
            s = "0" + s;
        }

        temp.clear();
        temp.resize(s.length() / 2);
        for (int i = 0; i < temp.size(); i++)
        {
            temp[i] = stoi(s.substr(i * 2, 2), nullptr, 16);
        }
        reverse(temp.begin(), temp.end());
        for (char c : temp)
        {
            data.push_back(c);
        }
        value = 2835;
        ss << std::hex << value;
        s = ss.str();
        while (s.size() < 8)
        {
            s = "0" + s;
        }
        temp.clear();
        temp.resize(s.length() / 2);
        for (int i = 0; i < temp.size(); i++)
        {
            temp[i] = stoi(s.substr(i * 2, 2), nullptr, 16);
        }
        reverse(temp.begin(), temp.end());
        for (char c : temp)
        {
            data.push_back(c);
        }
        for (char c : temp)
        {
            data.push_back(c);
        }

        data.push_back(0);
        data.push_back(0);
        data.push_back(0);
        data.push_back(0);
        data.push_back(0);
        data.push_back(0);
        data.push_back(0);
        data.push_back(0);

        return data;
    }
    std::vector<char> ToBMPColorCode(std::vector<std::vector<Color>> source)
    {
        std::vector<char> data;
        int padding = source.size() % 4;
        for (std::vector<Color> v : source)
        {
            for (Color c : v)
            {
                data.push_back(c.b);
                data.push_back(c.g);
                data.push_back(c.r);
            }
            for (int i = 0; i < padding; i++)
            {
                data.push_back(0);
            }
        }

        return data;
    }
    std::vector<char> ToBMP(std::vector<std::vector<Color>> source)
    {
        std::vector<char> data;
        for (char c : GetBMPHeader(source))
            data.push_back(c);
        for (char c : ToBMPColorCode(source))
            data.push_back(c);
        return data;
    }
}