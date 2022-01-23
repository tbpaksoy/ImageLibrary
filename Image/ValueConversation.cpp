#include <iostream>
#include <string>
#include <array>
#include <sstream>
using namespace std;

int length = 8;
bool forceUpper = true;

string FormatString(string s)
{
    int decised = max((int)s.size(),length);
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
    return result;
}

int main()
{
    string value;
    cin >> value;
    cout << FormatString(value);
    cout << endl << toupper('a');
    return 0;
}