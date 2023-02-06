#ifndef COMMAND
#define COMMAND
#include "Command.hpp"
#endif
namespace Tahsin
{
    Command::Command(GeographyObject *target)
    {
        this->target = target;
    }
    Command::Command() {}
    Command::~Command() {}
}