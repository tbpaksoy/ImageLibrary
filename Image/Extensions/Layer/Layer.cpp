#include <vector>
using namespace std;
namespace Tahsin
{
    template <typename T>
    class Layer
    {
    protected:
        vector<vector<T>> data;
        vector<vector<int>> permission;

    public:
        vector<T> dataVarierty;
        Layer(int width, int height, int permissionCount)
        {
            for (int i = 0; i < height; i++)
            {
                vector<T> temp(width);
                data.push_back(temp);
            }
        }
        virtual void GenerateData() = 0;
        virtual vector<vector<T>> GetData() = 0;
    };
}