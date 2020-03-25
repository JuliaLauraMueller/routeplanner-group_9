using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace RoutePlannerLib
{
    public class Cities
    {
        private List<City> cityList = new List<City>();
        public int Count { get { return this.cityList.Count; } }

        public City this[int index] // indexer implementation
        {
            get {
                if (index > cityList.Count)
                {
                    throw new ArgumentOutOfRangeException("Index ist zu gross");
                }
                else if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("Index darf nicht negativ sein");
                }

                return this.cityList[index];
            }
            set {
                if (index >= cityList.Count)
                {
                    throw new ArgumentOutOfRangeException("Index ist zu gross");
                }
                else if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("Index darf nicht negativ sein");
                }

                cityList[index] = value;
            }
        }

        public Predicate<City> ByName(string cityName) {
            return delegate (City city)
            {
                return city.Name.Equals(cityName, StringComparison.InvariantCultureIgnoreCase);
            };
        }

        public City FindCity(Predicate<City> cityName)
        {
            return cityList.Find(cityName);
        }

        public City this[string cityName]
        {
            get
            {
                if (cityName == null)
                {
                    throw new ArgumentNullException();
                }

                var foundCity = this.cityList.Find(ByName(cityName));

                if (foundCity == null)
                {
                    throw new KeyNotFoundException("City not found!");
                }

                return foundCity;
            }
        }


        public int ReadCities(string filename)
        {
            int counter = 0;
            using (var reader = new StreamReader(filename))
            {
                IEnumerable<string[]> citiesAsStrings = reader.GetSplittedLines('\t');

                var list = citiesAsStrings.Select(city => new City(city[0].Trim(), city[1].Trim(),
                        int.Parse(city[2]), double.Parse(city[3],
                        CultureInfo.InvariantCulture), double.Parse(city[4],
                        CultureInfo.InvariantCulture))).ToArray();

                cityList.AddRange(list);
                counter = list.Count();
            }
           return counter;
        }

        public IEnumerable<City> FindNeighbours(WayPoint location, double distance)
        {
           return cityList.Where(i => location.Distance(i.Location) < distance);
        }

        public int AddCity(City city)
        {
            cityList.Add(city);
            return cityList.Count;
        }
    }

}
