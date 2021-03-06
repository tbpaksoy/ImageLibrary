#include <string>
#include "ImgLibBase.cpp"
using namespace std;
namespace Tahsin
{
    class IExportable
    {
    public:
        wstring fileName;
        wstring directory;
        IExportable(wstring directory, wstring fileName);
        ~IExportable();
        void virtual Export();
        void virtual Export(wstring directory, wstring fileName);
    };

    IExportable::IExportable(wstring directory, wstring fileName)
    {
        this->directory = directory;
        this->fileName = fileName;
    }
    IExportable::~IExportable()
    {
    }
    void IExportable::Export(wstring directory, wstring fileName)
    {
    }
    void IExportable::Export()
    {
    }
    class IColorTurnable
    {
    protected:
        int size;
    public:
        IColorTurnable();
        virtual vector<vector<Color>> GetColorVector();
        int GetSize();
    };

    IColorTurnable::IColorTurnable()
    {
    }
    int IColorTurnable::GetSize()
    {
        return size;
    }
    vector<vector<Color>> IColorTurnable::GetColorVector()
    {
        
    }
    class IGetInfoString
    {
    public:
        IGetInfoString();
        virtual string GetInfo();
    };
    string IGetInfoString::GetInfo()
    {
        
    }
    IGetInfoString::IGetInfoString()
    {
    } 
}