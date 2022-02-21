#include <vector>
#include <math.h>
using namespace std;
namespace Tahsin
{
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
    }
    template <typename T>
    bool IsHomogenious(vector<vector<T>> source)
    {
        if (source.size() != NULL)
        {
            for (int i = 1; i < source.size(); i++)
            {
                if (source[i - 1].size() == NULL || source[i].size() == NULL || source[i - 1].size() != source[i].size())
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
        result /= source.size();
    }
}