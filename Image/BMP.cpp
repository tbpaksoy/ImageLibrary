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
int main()
{
    srand(time(NULL));
    vector<vector<Color>> cols;
    vector<Color> a{Color("FA45FD"), Color("FFAA55")};
    vector<Color> b{Color("AADDCC"), Color("FFDDA1")};
    cols.push_back(a);
    cols.push_back(b);
    cols = ScaleVector<Color>(cols, 100, 200);
    vector<string> data = GetBMPData(cols);
    ofstream fs("a.bmp", std::ofstream::binary);
    fs.write(ToByteArray(data), data.size());
    fs.close();
}