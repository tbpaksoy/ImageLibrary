#include <string>
#include <array>
#include <iostream>
using namespace std;
namespace Tahsin
{
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
    string *GroupAndReverse(string source,int length)
    {
        string *temp = GroupString(source,length);
        string *result = ReverseGroup(temp,source.size()/length);
        return result;
    }
    string UniteGroup(string source[])
    {
        string result = NULL;
        for(int i = 0; i < result.size(); i++)
        {
            result += source[i];
        }
        return result;
    }

}