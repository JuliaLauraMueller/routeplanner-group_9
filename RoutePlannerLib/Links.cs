using System.IO;
using System.Collections.Generic;
using RoutePlannerLib;
using Fhnw.Ecnf.RoutPlanner.RoutePlannerLib;
using System;
using System.Linq;

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

        //public List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportMode mode)
        //{
        //    RouteRequested?.Invoke(this, new RouteRequestEventArgs(cities[fromCity], cities[toCity], mode));
        //    return new List<Link>();
        //}

        public List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportMode mode)
		{
			//TODO: inform listeners
			RouteRequested?.Invoke(this, new RouteRequestEventArgs(cities[fromCity], cities[toCity], mode));

			//use dijkstra's algorithm to look for all single-source shortest paths
			var visited = new Dictionary<City, DijkstraNode>();
			var pending = new SortedSet<DijkstraNode>(new DijkstraNode[]
			{
				new DijkstraNode()
				{
					VisitingCity = cities[fromCity],
					Distance = 0
				}
			});

			while (pending.Any())
			{
				var cur = pending.Last();
				pending.Remove(cur);

				if (!visited.ContainsKey(cur.VisitingCity))
				{
					visited[cur.VisitingCity] = cur;

					foreach (var link in FindAllLinksForCity(cur.VisitingCity, mode))
						pending.Add(new DijkstraNode()
						{
							VisitingCity = (link.FromCity.Equals(cur.VisitingCity)) ? link.ToCity : link.FromCity,
							Distance = cur.Distance + link.Distance,
							PreviousCity = cur.VisitingCity
						});
				}
			}

			//did we find any route?
			if (!visited.ContainsKey(cities[toCity]))
				return null;

			//create a list of cities that we passed along the way
			var citiesEnRoute = new List<City>();
			for (var c = cities[toCity]; c != null; c = visited[c].PreviousCity)
				citiesEnRoute.Add(c);
			citiesEnRoute.Reverse();

			//for each city en route, find the link (path) which will be passed along the way. Return all paths as Enumerable.
			//IEnumerable<Link> paths = FindLinksToCitiesEnRoute(citiesEnRoute);
			//return paths.ToList();
			return null;
		}

		//private IEnumerable<Link> FindLinksToCitiesEnRoute(List<City> citiesEnRoute)
		//{
		//	var findList = new List<Link>();
		//	for (var i = 0; i < citiesEnRoute.Count; i++)
		//	{
		//		findList.Add(links[i].FromCity);
		//	}
		//}

		private IEnumerable<Link> FindAllLinksForCity(City visitingCity, TransportMode mode)
		{
			for (var i = 0; i < links.Count; i++)
			{
				if (links[i].FromCity.Equals(visitingCity) || links[i].ToCity.Equals(visitingCity) && links[i].TransportMode.Equals(mode))

					yield return links[i];
			}

		}

		public class DijkstraNode : IComparable<DijkstraNode>
		{
			public City VisitingCity;
			public double Distance;
			public City PreviousCity;

			public int CompareTo(DijkstraNode other)
			{
				return other.Distance.CompareTo(Distance);
			}
		}
	}
}
