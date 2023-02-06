#ifndef MOVECOMMAND
#define MOVECOMMAND
#include "MoveCommand.hpp"
#endif
#ifndef LAND
#define LAND
#include "Land.hpp"
#endif
namespace Tahsin
{
    MoveCommand::MoveCommand(GeographyObject *target, Vector2D offset)
    {
        this->target = target;
        this->offset = offset;
    }
    MoveCommand::~MoveCommand() {}
    void MoveCommand::Execute()
    {
        if (executed)
            return;
        before = target->center;
        executed = true;
        target->center = target->center + offset;
    }
    void MoveCommand::Undo()
    {
        if (!executed)
            return;
        executed = false;
        target->center = before;
    }
}