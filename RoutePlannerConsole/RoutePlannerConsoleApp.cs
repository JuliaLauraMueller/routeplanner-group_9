using System;
using RoutePlannerLib;

namespace RoutePlannerConsole
{
    class RoutePlannerConsoleApp
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Route Planner (Version {0})", typeof(RoutePlannerConsoleApp).Assembly.GetName().Version);

            // WayPoints
            var wayPoint = new WayPoint("Windisch", 47.479319847061966, 8.212966918945312);
            var wayPoint2 = new WayPoint(null, 47.479319847061966, 8.212966918945312);
            var bern = new WayPoint("Bern", 46.947998, 7.448148);
            var tripolis = new WayPoint("Tripolis", 32.876175, 13.187507);
            // Console.WriteLine($"{wayPoint.Name}: {wayPoint.Latitude}/{wayPoint.Longitude}");

            // Cities
            var cities = new Cities();
            var city1 = new City("Bern", "Schweiz", 12345, 46.947998, 7.448148);

            Console.WriteLine(wayPoint.ToString());
            Console.WriteLine(wayPoint2.ToString());
            Console.WriteLine("------------------------------------------");
            Console.WriteLine($"Der Abstand zwischen {bern.Name} und {tripolis.Name} ist {tripolis.Distance(bern):F2} km");
            Console.WriteLine("Anzahl der Cities in der Liste: " + cities.ReadCities("./citiesTestDataLab2.txt"));
            Console.WriteLine("Neue City hinzugefügt, Anzahl: " + cities.AddCity(city1));
            Console.WriteLine("Neue City hinzugefügt, Anzahl: " + cities.AddCity(city1));

            Console.WriteLine("Nachbarstädte von Bern: " + cities.FindNeighbours(city1.Location, 9526.247927408867).Count);
        }
    }
}
