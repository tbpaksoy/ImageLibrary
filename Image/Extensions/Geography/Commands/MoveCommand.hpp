#ifndef COMMAND
#define COMMAND
#include "Command.hpp"
#endif
#ifndef GEOMETRY
#define GEOMETRY
#include "Geometry.hpp"
#endif
namespace Tahsin
{
    class MoveCommand : public Command
    {
    private:
        Vector2D offset;
        Vector2D before;

    public:
        MoveCommand(GeographyObject *target, Vector2D offset);
        ~MoveCommand();
        void Execute() override;
        void Undo() override;
    };
}