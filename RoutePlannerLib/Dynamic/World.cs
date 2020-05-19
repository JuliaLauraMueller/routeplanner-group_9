using System;
using System.Dynamic;
using RoutePlannerLib;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Dynamic
{
    public class World : DynamicObject
    {
        private Cities cities;

        public World(Cities cities)
        {
            this.cities = cities;

        }

        public dynamic CityName(City c)
        {
            if (cities.FindCity(c.Name))
            {
                return c;
            }
            else
                return $"The city {c} does not exist";
        }
    }
}
