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
            double gradToRad = Math.PI / 180;
            int radius = 6371;

            double rad = radius * Math.Acos(Math.Sin(this.Latitude * gradToRad) * Math.Sin(target.Latitude * gradToRad)
                + Math.Cos(this.Latitude * gradToRad) * Math.Cos(target.Latitude * gradToRad)
                * Math.Cos(this.Longitude * gradToRad - target.Longitude * gradToRad));
            return rad;
        }
    }

    public class City
    {
        public string Name;
        public string Country;
        public int Population;
        public WayPoint Location;

        public City(string name, string country, int population, double latitude, double longitude)
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

                while ((cityProperty = reader.ReadLine()) != null)
                {
                    string[] cityPropertyArray = cityProperty.Split("\t");
                    cityList.Add(new City(cityPropertyArray[0].ToString(), cityPropertyArray[1].ToString(), int.Parse(cityPropertyArray[2]),
                        double.Parse(cityPropertyArray[3]), double.Parse(cityPropertyArray[4])));

                    counter++;
                    
                }
                // Console.WriteLine("Liste Stadtname: " + cityList[0].Name);
                // Console.WriteLine("Liste Stadtname: " + cityList[1].Name);
            }

            return counter;
        }

        

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
