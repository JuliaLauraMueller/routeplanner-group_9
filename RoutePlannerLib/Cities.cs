using Fhnw.Ecnf.RoutPlanner.RoutePlannerLib;
using Fhnw.Ecnf.RoutPlanner.RoutePlannerLib.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

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
                return city.Name.Equals(cityName, StringComparison.InvariantCultureIgnoreCase); // ToLower() vellech?
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
                //var foundCity = FindCity(ByName);

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
                //IEnumerable<string[]> citiesAsStrings = reader.GetSplittedLines("\t");
                string cityProperty;

                while ((cityProperty = reader.ReadLine()) != null)
                {
                    string[] cityPropertyArray = cityProperty.Split("\t");
                    cityList.Add(new City(cityPropertyArray[0].ToString(), cityPropertyArray[1].ToString(), int.Parse(cityPropertyArray[2]),
                        double.Parse(cityPropertyArray[3], CultureInfo.InvariantCulture), double.Parse(cityPropertyArray[4], CultureInfo.InvariantCulture)));

                    counter++;

                }
            }
            return counter;
        }


        //public int ReadCities(string filename)
        //{

        //    int counter = 0;

        //    using (var reader = new StreamReader(filename))
        //    {
        //        IEnumerable<string[]> citiesAsStrings = reader.GetSplittedLines('\t');

        //        foreach (var c in citiesAsStrings)
        //        {
        //            cityList.Add(new City(c[0].Trim(), c[1].Trim(),
        //                int.Parse(c[2]), double.Parse(c[3],
        //                CultureInfo.InvariantCulture), double.Parse(c[4],
        //                CultureInfo.InvariantCulture)));
        //            counter++;
        //        }
        //    }
        //    return counter;
        //}

        public IList<City> FindNeighbours(WayPoint location, double distance)
        {
           List<City> nearByCities = new List<City>();

            for (int i = 0; i < cityList.Count; i++)
            {
                if (location.Distance(cityList[i].Location) < distance)
                {
                    nearByCities.Add(cityList[i]);
                    
                }
            }
            return nearByCities;
        }

        public int AddCity(City city)
        {
            cityList.Add(city);
            return cityList.Count;
        }
    }

}
