#include <string>
#include "Color.cpp"
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
        virtual Color *TurnColor();
        int GetSize();
    };

    IColorTurnable::IColorTurnable()
    {
    }
    Color *IColorTurnable::TurnColor()
    {
    }
    int IColorTurnable::GetSize()
    {
        return size;
    }
}