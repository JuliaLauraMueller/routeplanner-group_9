using System;
using System.Collections.Generic;
using System.IO;

namespace RoutePlannerLib
{
    public class WayPoint 
    { 
        public string Name { get; set; } 
        public double Longitude { get; set; } 
        public double Latitude { get; set; } 
        public WayPoint(string name, double latitude, double longitude)
        { 
            Name = name; 
            Latitude = latitude; 
            Longitude = longitude; 
        }

        public override string ToString()
        {
            return $"Waypoint: {this.Name} {this.Latitude:F2} / {this.Longitude:F2}";
        }

        public double Distance(WayPoint target)
        {
            double grad;
            int r = 6371;
            grad = r * Math.Acos(Math.Sin(this.Latitude * (Math.PI / 180)) * Math.Sin(target.Latitude * (Math.PI / 180))
                + Math.Cos(this.Latitude * (Math.PI / 180)) * Math.Cos(target.Latitude * (Math.PI / 180))
                * Math.Cos(this.Longitude * (Math.PI / 180) - target.Longitude * (Math.PI / 180)));
            return grad;
        }
    }

    public class City
    {
        public string Name;
        public string Country;
        public double Population;
        public WayPoint Location;

        public City(string name, string country, double population, double latitude, double longitude)
        {
            Name = name;
            Country = country;
            Population = population;
            Location = new WayPoint(name, latitude, longitude);

        }
    }

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

        public int ReadCities(string filename){

            int counter = 0;

            using (var reader = new StreamReader(filename))
            {
                string cityProperty;
                string cityName;
                string countryName;
                double cityPopulation;
                double cityLatitude;
                double cityLongitute;

                for (int i = 0; i < 10; i++)
                {
                    cityProperty = reader.ReadLine();
                    cityName = cityProperty.Split("/t").ToString();
                    countryName = cityProperty.Split("/t").ToString();
                    // Convert funktioniert noch nicht richtig, Wieso?
                    cityPopulation = Convert.ToDouble(cityProperty.Split("/t").ToString());
                    cityLatitude = Convert.ToDouble(cityProperty.Split("/t").ToString());
                    cityLongitute = Convert.ToDouble(cityProperty.Split("/t").ToString());

                    cityList.Add(new City(cityName, countryName, cityPopulation, cityLatitude, cityLongitute));
                    Console.WriteLine(cityList);
                    
                    Console.WriteLine(reader.ReadLine());
                    counter++;
                }
            }
            return counter;
        }

        public int AddCity(City city)
        {
            cityList.Add(city);
            return cityList.Count;
        }
    }

}
