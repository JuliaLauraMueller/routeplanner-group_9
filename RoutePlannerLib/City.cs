using System;
using System.Collections.Generic;
using System.Text;

namespace Fhnw.Ecnf.RoutPlanner.RoutePlannerLib
{
    public class City
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public int Population { get; set; }
        public WayPoint Location { get; set; }

        public City(string name, string country, int population, double latitude, double longitude)
        {
            Name = name;
            Country = country;
            Population = population;
            Location = new WayPoint(name, latitude, longitude);

        }

        public override bool Equals(object city)
        {
            if (city == null)
            {
                return false;
            }

            if (city is City c)
            {
                if (this.Name.ToLower().Equals(c.Name.ToLower()) && this.Country.ToLower().Equals(c.Country.ToLower()))
                {
                    return true;
                }
            }
            return false;
            
        }

        public override int GetHashCode()
        {
            Console.WriteLine(this.Name.GetHashCode() ^ this.Country.GetHashCode());
            return this.Name.GetHashCode() ^ this.Country.GetHashCode();
        }
    }
}
