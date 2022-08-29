#include "ImgLibBase.cpp"
#include <vector>
#include <math.h>
#include <array>
#include "CustomMathF.cpp"
using namespace Tahsin;
using namespace std;
namespace Tahsin
{
    vector<vector<Color *>> CreateColorTransition(vector<Color *> from, vector<Color *> to, int step)
    {
        while (from.size() < max(from.size(), to.size()))
        {
            from.push_back(new Color(0, 0, 0, 255));
        }
        while (to.size() < max(from.size(), to.size()))
        {
            to.push_back(new Color(0, 0, 0, 255));
        }
        vector<vector<Color *>> result;
        for (int i = 0; i < step; i++)
        {
            vector<Color *> temp;
            for (int j = 0; j < from.size(); j++)
            {
                char r = GotoValue(from[i]->r, to[i]->r) * i / step;
                char g = GotoValue(from[i]->r, to[i]->r) * i / step;
                char b = GotoValue(from[i]->r, to[i]->r) * i / step;
                char a = GotoValue(from[i]->r, to[i]->r) * i / step;

                temp.push_back(new Color(r, g, b, a));
            }
            result.push_back(temp);
        }
        return result;
    }
}