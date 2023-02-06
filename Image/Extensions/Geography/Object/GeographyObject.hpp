#ifndef STRING
#include <string>
#define STRING
#endif
#ifndef GEOMETRY
#include "Geometry.hpp"
#define GEOMETRY
#endif
namespace Tahsin
{
    class GeographyObject
    {
    protected:
        std::string name;

    public:
        Vector2D center;
        GeographyObject();
        ~GeographyObject();
    };
}