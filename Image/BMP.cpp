#include <array>
#include <vector>
#include <iostream>
#include <stdio.h>
#include "ImgLibBase.cpp"
#include <filesystem>
#include <fstream>

using namespace std;
using namespace Tahsin;
const int BMPHeaderLength = 16;
vector<string> CreateBitmapInfoHeader(int width, int height)
{
    vector<string> temp;
    length = 2;
    temp.push_back(GetHexValue('B'));
    temp.push_back(GetHexValue('M'));
    length = 8;
    int paddingCount = width * 3 % 4;
    cout << endl
         << paddingCount << endl;
    string s = GetHexValue(54 + width * height * 3 + height * paddingCount);
    string *sa = GroupAndReverse(s, 2);
    for (int i = 0; i < s.size() / 2; i++)
    {
        temp.push_back(sa[i]);
    }
    length = 4;
    sa = new string[2]{GetHexValue(0), GetHexValue(0)};
    for (int i = 0; i < 2; i++)
    {
        string *reversed = GroupAndReverse(sa[i], 2);
        for (int j = 0; j < sa[i].size() / 2; j++)
        {
            temp.push_back(reversed[j]);
        }
    }
    length = 8;
    sa = new string[4]{GetHexValue(54), GetHexValue(40), GetHexValue(width), GetHexValue(height)};
    for (int i = 0; i < 4; i++)
    {
        string *reversed = GroupAndReverse(sa[i], 2);
        for (int j = 0; j < sa[i].size() / 2; j++)
        {
            temp.push_back(reversed[j]);
        }
    }
    length = 4;
    sa = new string[2]{GetHexValue(1), GetHexValue(24)};
    for (int i = 0; i < 2; i++)
    {
        string *reversed = GroupAndReverse(sa[i], 2);
        for (int j = 0; j < sa[i].size() / 2; j++)
        {
            temp.push_back(reversed[j]);
        }
    }
    length = 8;
    sa = new string[6]{GetHexValue(0), GetHexValue(16), GetHexValue(2835), GetHexValue(2835), GetHexValue(0), GetHexValue(0)};
    for (int i = 0; i < 6; i++)
    {
        string *reversed = GroupAndReverse(sa[i], 2);
        for (int j = 0; j < sa[i].size() / 2; j++)
        {
            temp.push_back(reversed[j]);
        }
    }
    return temp;
}
vector<string> GenerateColorData(vector<vector<Color>> resource)
{
    length = 2;
    vector<string> data;
    for (vector<Color> v : resource)
    {
        for (Color c : v)
        {
            data.push_back(GetHexValue((unsigned char)c.b));
            data.push_back(GetHexValue((unsigned char)c.g));
            data.push_back(GetHexValue((unsigned char)c.r));
        }
        while (data.size() % 4 != 0)
        {
            data.push_back(GetHexValue((unsigned char)0));
        }
    }
    return data;
}
vector<string> GetBMPData(vector<vector<Color>> colors)
{
    vector<string> data;
    for (string s : CreateBitmapInfoHeader(colors[0].size(), colors.size()))
    {
        data.push_back(s);
    }
    for (string s : GenerateColorData(colors))
    {
        data.push_back(s);
    }
    return data;
}
vector<char> GetBMPHeader(vector<vector<Color>> resource)
{
    vector<char> data;
    if (Tahsin::IsHomogenious<Color>(resource))
    {
        stringstream ss;
        data.push_back('B');
        data.push_back('M');
        int paddingCount = resource[0].size() * 3 % 4;

        int value = 54 + resource[0].size() * resource.size() * 3 + resource.size() * paddingCount;
        ss << hex << value;
        string s = ss.str();
        ss.str("");
        while (s.size() < 8)
        {
            s = "0" + s;
        }
        vector<char> temp(s.length() / 2);
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

        value = resource[0].size();
        ss << value;
        s.clear();
        s = ss.str();
        ss.str("");
        cout << "q" << resource[0].size() << endl;
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
        value = resource.size();
        ss << hex << value;
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

        value = (resource[0].size() + paddingCount) * 3 * resource.size();
        ss << hex << value;
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
        ss << hex << value;
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
    }

    return data;
}
vector<char> GetColorData(vector<vector<Color>> resource)
{
    vector<char> data;

    if (IsHomogenious(resource))
    {
        for (vector<Color> v : resource)
        {
            for (Color c : v)
            {
                data.push_back(c.b);
                data.push_back(c.g);
                data.push_back(c.r);
            }
            while (v.size() % 4 != 0)
            {
                data.push_back(0);
            }
        }
    }

    return data;
}
int main()
{
    vector<Color> temp(4);
    vector<vector<Color>> colors;
    colors.push_back(temp);
    colors.push_back(temp);
    colors.push_back(temp);
    colors.push_back(temp);

    for (char c : GetBMPHeader(colors))
    {
        cout << (unsigned int)c << ",";
    }
    cout << "end";
}