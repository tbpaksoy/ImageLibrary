#include "Extensions/Geometry/Geometry.cpp"
#include "ImgLibBase.cpp"
#include <vector>
#include <string>
namespace Tahsin
{
    class Culture
    {
    public:
        string name;
        Color color;
        Culture(string name, Color color)
        {
            this->color = color;
            this->name = name;
        }
        ~Culture() {}
    };
    class Biome
    {
    public:
        string name;
        Color color;
        bool canPlaceCountry;
        Biome(string name, Color color, bool canPlaceCountry)
        {
            this->name = name;
            this->color = color;
            this->canPlaceCountry = canPlaceCountry;
        }
    };
    class Settlement
    {
    public:
        string name;
        enum Status
        {
            Village,
            Town,
            City,
            Metropolis
        };
        Status status;
        Vector2D location;
        Settlement() {}
        Settlement(string name, Status status, Vector2D location)
        {
            this->name = name;
            this->status = status;
            this->location = location;
        }
        ~Settlement() {}
    };
    class PoliticalEntity
    {
    protected:
        Settlement *capital;

    public:
        string name;
        PoliticalEntity() {}
    };
    class Province : PoliticalEntity
    {
    public:
        State *owner;
        Province() {}
        Province(string name, Settlement *capital, vector<Settlement *> settlements)
        {
            this->name = name;
            if (!count(settlements.begin(), settlements.end(), capital))
            {
                settlements.push_back(capital);
            }
            this->capital = capital;
            this->settlements = settlements;
        }
        bool HasSettlement(Settlement *settlement)
        {
            for (Settlement *s : settlements)
            {
                if (s == settlement)
                    return true;
            }
            return false;
        }

    private:
        vector<Settlement *> settlements;
    };
    class State : PoliticalEntity
    {
    private:
        vector<Province *> provinces;

    public:
        State() {}
        bool HasSettlement(Settlement *settlement)
        {
            for (Province *p : provinces)
            {
                if (p->HasSettlement(settlement))
                    return true;
            }
            return false;
        }
        bool HasProvince(Province *province)
        {
            for (Province *p : provinces)
            {
                if (p == province)
                    return true;
            }
            return false;
        }
        
    };
}