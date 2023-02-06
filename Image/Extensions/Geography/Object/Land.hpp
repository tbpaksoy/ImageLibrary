#include "GeographyObject.hpp"
#ifndef GEOMETRY
#define GEOMETRY
#include "Geometry.hpp"
#endif

namespace Tahsin
{
    
    class Land : public GeographyObject
    {
    public:
        Land(Vector2D center);
        ~Land();
    };
}