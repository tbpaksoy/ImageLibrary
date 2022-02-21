#include <iostream>
#include <string>
#include <array>
#include <sstream>
using namespace std;

namespace Tahsin
{
    const string accepted = "0123456789ABCDEFabcdef";
    int length = 8;
    bool forceUpper = true;

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
}