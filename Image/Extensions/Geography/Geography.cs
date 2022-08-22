using TahsinsLibrary.Geometry;
using System.Collections.Generic;
namespace TahsinsLibrary.Geography
{
    public sealed class Culture
    {
        public string name;
        public Color color;
        public Culture(string name, Color color)
        {
            this.name = name;
            this.color = color;
        }
    }
    public sealed class Biome
    {
        public string name;
        public Color color;
        public bool canPlaceCountry;
        public Biome(string name, Color color, bool canPlaceCountry)
        {
            this.name = name;
            this.color = color;
            this.canPlaceCountry = canPlaceCountry;
        }

    }
    public sealed class Settlement
    {
        public string name;
        public enum Status
        {
            Village, Town, City, Metropolis
        }
        public Status status;
        public Vector2D location;
        public Settlement(string name, Status status, Vector2D location)
        {
            this.name = name;
            this.status = status;
            this.location = location;
        }
    }
    public abstract class PoliticalEntity
    {
        protected Settlement capital;
        public string name;
        public PoliticalEntity(string name ,Settlement capital)
        {
            this.name = name;
            this.capital = capital;
        }
    }
    public sealed class Province : PoliticalEntity
    {
        public State owner;
        private List<Settlement> settlements;

        public Province(string name, Settlement capital, List<Settlement> settlements) : base(name, capital)
        {
            this.settlements = settlements;
            this.name = name;
            if(!settlements.Contains(capital))
            {
                settlements.Add(capital);
            }
            this.capital = capital;
        }

        public void AddSettlement(Settlement settlement)
        {
            if(!settlements.Contains(settlement))
            {
                settlements.Add(settlement);
            }
        }
        public bool HasSettlement(Settlement settlement) => settlements.Contains(settlement);
    }
    public sealed class State : PoliticalEntity
    {
        private List<Province> provinces;

        public State(string name, Settlement capital, List<Province> provinces) : base(name, capital)
        {
            this.name = name;
            this.provinces = provinces;
            bool found = false;
            foreach(Province province in provinces)
            {
                if(province.HasSettlement(capital))
                {
                    found = true;
                    break;
                }
            }
            if(!found)
            {
                provinces.Add(new Province("Capital Proivnce",capital,new List<Settlement>{capital}));
            }
            this.capital = capital;
        }

        public void AddProvince(Province province)
        {
            if(!provinces.Contains(province))
            {
                provinces.Add(province);
            }
        }
        public Province RemoveProvince(Province province)
        {
            if(provinces.Contains(province))
            {
                provinces.Remove(province);
                return province;
            }
            else return null;
        }
        public bool HasProvince(Province province) => provinces.Contains(province);
        public bool HasSettlement(Settlement settlement)
        {
            foreach(Province province in provinces)
            {
                if(province.HasSettlement(settlement)) return true;
            }
            return false;
        }
    }
}