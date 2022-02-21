#include <array>
#include <vector>
#include <iostream>
#include "ValueConversation.cpp"
#include "ArrayOperations.cpp"

using namespace std;
const int BMPHeaderLength = 16;

string *CreateBitmapInfoHeader(int width, int height)
{
    cout << "0";
    vector<string> temp;
    Tahsin::length = 2;
    temp.push_back(Tahsin::GetHexValue('B'));
    temp.push_back(Tahsin::GetHexValue('M'));
    cout << "0";
    Tahsin::length = 8;
    int paddingCount = width * 3 % 4;
    string s = Tahsin::GetHexValue(54 + width * height * 3 + height * paddingCount);
    string *sa = Tahsin::GroupAndReverse(s, 2);
    for (int i = 0; i < s.size() / 2; i++)
    {
        temp.push_back(sa[i]);
    }
    Tahsin::length = 4;
    sa = new string[2]{Tahsin::GetHexValue(0), Tahsin::GetHexValue(0)};
    for (int i = 0; i < 2; i++)
    {
        string *reversed = Tahsin::GroupAndReverse(sa[i], 2);
        for (int j = 0; sa[i].size() / 2; i++)
        {
            temp.push_back(reversed[i]);
        }
    }
    cout << "0";
    Tahsin::length = 8;
    sa = new string[4]{Tahsin::GetHexValue(54), Tahsin::GetHexValue(40), Tahsin::GetHexValue(width), Tahsin::GetHexValue(height)};
    for (int i = 0; i < 4; i++)
    {
        string *reversed = Tahsin::GroupAndReverse(sa[i], 2);
        cout << "-" + sa[i].size() / 2;
        for (int j = 0; sa[i].size() / 2; i++)
        {
            cout << "asd";
            temp.push_back(reversed[i]);
        }
    }
    cout << "0";

    Tahsin::length = 4;
    sa = new string[2]{Tahsin::GetHexValue(1), Tahsin::GetHexValue(24)};
    for (int i = 0; i < 2; i++)
    {
        string *reversed = Tahsin::GroupAndReverse(sa[i], 2);
        for (int j = 0; sa[i].size() / 2; i++)
        {
            temp.push_back(reversed[i]);
        }
    }
    cout << "0";

    Tahsin::length = 8;
    sa = new string[6]{Tahsin::GetHexValue(0), Tahsin::GetHexValue(16), Tahsin::GetHexValue(2835), Tahsin::GetHexValue(2835), Tahsin::GetHexValue(0), Tahsin::GetHexValue(0)};
    for (int i = 0; i < 2; i++)
    {
        string *reversed = Tahsin::GroupAndReverse(sa[i], 2);
        for (int j = 0; sa[i].size() / 2; i++)
        {
            temp.push_back(reversed[i]);
        }
    }
    cout << "0";
    return temp.data();
}
int main()
{
    CreateBitmapInfoHeader(5, 5);
    return 0;
}
