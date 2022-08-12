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
}