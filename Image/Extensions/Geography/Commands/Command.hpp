#ifndef GEOOBJ
#define GEOOBJ
#include "GeographyObject.hpp"
#endif
namespace Tahsin
{
    class Command
    {
    protected:
        GeographyObject *target;
        bool executed;
    public:
        Command(GeographyObject *target);
        Command();
        ~Command();
        virtual void Execute() = 0;
        virtual void Undo() = 0;
    };
}