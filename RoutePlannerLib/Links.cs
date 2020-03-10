using System.IO;
using System.Collections.Generic;
using RoutePlannerLib;
using System;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{

	///	<summary>
	///	Manages	links from a city to another city.
	///	</summary>
	public class Links
	{
		private List<Link> links = new List<Link>();
		private Cities cities;

		public delegate void RouteRequestHandler(object sender, RouteRequestEventArgs e);
		public event RouteRequestHandler RouteRequested;
		

		public int Count { get { return links.Count; } }

		///	<summary>
		///	Initializes	the	Links with	the	cities.
		///	</summary>
		///	<param name="cities"></param>
		public Links(Cities cities)
		{
			this.cities = cities;
		}

		///	<summary>
		///	Reads a	list of	links from the given file.
		///	Reads only links where the cities exist.
		///	</summary>
		///	<param name="filename">name	of links file</param>
		///	<returns>number	of read	route</returns>
		public int ReadLinks(string filename)
		{
			var previousCount = Count;
			using (var reader = new StreamReader(filename))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					var tokens = line.Split('\t');

					try
					{
						var city1 = cities[tokens[0]];
						var city2 = cities[tokens[1]];

						links.Add(new Link(city1, city2, city1.Location.Distance(city2.Location), TransportMode.Rail));
					}
					catch (KeyNotFoundException)
					{
						//missing cities should be ignored
					}
				}
			}

			return Count - previousCount;
		}

		public List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportMode mode)
		{
			RouteRequested?.Invoke(this, new RouteRequestEventArgs(cities[fromCity], cities[toCity], mode));
			return new List<Link>();
		}
	}

	public class RouteRequestEventArgs
	{
        public City FromCity { get; }
		public City ToCity { get; }
		public TransportMode Mode { get; }

		public RouteRequestEventArgs(City fromCity, City toCity, TransportMode mode)
		{
			this.ToCity = toCity;
			this.FromCity = fromCity;
			this.Mode = mode;
		}

		//public Link newLink { get; set; }
	}

	// Overwriting-Methode
	public class RouteRequestWatcher
	{

		private Dictionary<City, int> dict = new Dictionary<City, int>();
		public void LogRouteRequests(object source, RouteRequestEventArgs args)
		{
			if (dict.ContainsKey(args.ToCity))
			{
				int count = dict[args.ToCity];
				dict.Add(args.ToCity, count++);
			}
			else
			{
				dict.Add(args.ToCity, 1);
			}

			Console.WriteLine("Current Request State");
			Console.WriteLine("----------------------");
			foreach (var c in dict)
			{
				Console.WriteLine($"toCity: {c.Key.Name} has been requested {c.Value} times");
			}
		}

		public int GetCityRequests(City city)
		{
			if (dict.ContainsKey(city))
			{
				return dict[city];
			}
			return 0;
		}
	}
}
