#include <math.h>
using namespace std;
namespace Tahsin
{
    float GotoValue(float from, float to)
    {
        if (from >= to)
        {
            -abs(to - from);
        }
        else
            return abs(from - to);
    }
}