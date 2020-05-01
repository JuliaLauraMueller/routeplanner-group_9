using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using RoutePlannerLib;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;
using System.Threading.Tasks;

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
				IEnumerable<string[]> linksAsStrings = reader.GetSplittedLines('\t');

				foreach (var l in linksAsStrings)
				{
					try
					{
						var city1 = cities[l[0]];
						var city2 = cities[l[1]];

						links.Add(new Link(city1, city2, city1.Location.Distance(city2.Location), TransportMode.Rail));
					}
					catch (KeyNotFoundException)
					{
						Console.WriteLine("aasdfasdf");
						//missing cities should be ignored
					}
				}
			}
			return Count - previousCount;
		}

        public City[] FindCities(TransportMode transportMode)
        {
			return links.Where(c => c.TransportMode == transportMode).SelectMany(cities => new[]
			{
				cities.FromCity,
				cities.ToCity
			})
                .Distinct()
                .ToArray();
		}

        public Task<List<Link>> FindShortestRouteBetweenAsync(string fromCity, string toCity, TransportMode mode, IProgress<string> reportProgress = null)
        {
			var resultAsTask = Task.Run(() => FindShortestRouteBetween(fromCity, toCity, mode, reportProgress));

			return resultAsTask;
		}

		public List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportMode mode, IProgress<string> reportProgress = null)
		{
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

            if(reportProgress != null)
				reportProgress.Report("Initialized vistited and pending, done");

			while (pending.Any())
			{
				var cur = pending.Last();
				pending.Remove(cur);

				if (!visited.ContainsKey(cur.VisitingCity))
				{
					visited[cur.VisitingCity] = cur;
					if (reportProgress != null)
						reportProgress.Report("Current item marked as visited item, done");

					foreach (var link in FindAllLinksForCity(cur.VisitingCity, mode))
						pending.Add(new DijkstraNode()
						{
							VisitingCity = (link.FromCity.Equals(cur.VisitingCity)) ? link.ToCity : link.FromCity,
							Distance = cur.Distance + link.Distance,
							PreviousCity = cur.VisitingCity
						});
					if (reportProgress != null)
						reportProgress.Report("New pending item was added, done");
				}
			}

			//check if any routes where found
			if (!visited.ContainsKey(cities[toCity]))
				return null;
			if (reportProgress != null)
				reportProgress.Report("Check if any routes where found, done");

			//list of cities that we passed along the way
			var citiesEnRoute = new List<City>();
			for (var c = cities[toCity]; c != null; c = visited[c].PreviousCity)
				citiesEnRoute.Add(c);
			citiesEnRoute.Reverse();


			//for each city en route, find the link (path) which will be passed along the way. Return all paths as Enumerable.
			IEnumerable<Link> paths = FindLinksToCitiesEnRoute(citiesEnRoute);
			if (reportProgress != null)
				reportProgress.Report("All paths are saved as IEnumerable, done");
			return paths.ToList();

		}

		public List<List<Link>> FindAllShortestRoutes()
		{
			List<List<Link>> allShortestRoutes = new List<List<Link>>();
			foreach (var link in links)
			{
				Console.WriteLine(link.TransportMode);
				// foreach (TransportMode.)
				allShortestRoutes.Add(FindShortestRouteBetween(link.FromCity.Name, link.ToCity.Name, TransportMode.Bus));
				allShortestRoutes.Add(FindShortestRouteBetween(link.FromCity.Name, link.ToCity.Name, TransportMode.Car));
				allShortestRoutes.Add(FindShortestRouteBetween(link.FromCity.Name, link.ToCity.Name, TransportMode.Flight));
				allShortestRoutes.Add(FindShortestRouteBetween(link.FromCity.Name, link.ToCity.Name, TransportMode.Rail));
				allShortestRoutes.Add(FindShortestRouteBetween(link.FromCity.Name, link.ToCity.Name, TransportMode.Ship));
				allShortestRoutes.Add(FindShortestRouteBetween(link.FromCity.Name, link.ToCity.Name, TransportMode.Tram));
			}
			return allShortestRoutes;
		}

		public List<List<Link>> FindAllShortestRoutesParallel()
		{
			List<List<Link>> allShortestRoutes = new List<List<Link>>();
			Parallel.ForEach(links, link =>
			{
				allShortestRoutes.Add(FindShortestRouteBetween(link.FromCity.Name, link.ToCity.Name, link.TransportMode));
			});
			return allShortestRoutes;
		}


		private IEnumerable<Link> FindLinksToCitiesEnRoute(List<City> citiesEnRoute)
        {
            var findList = new List<Link>();
			for (var i = 0; i < citiesEnRoute.Count - 1; i++)
			{
				for (var j = 0; j < links.Count ; j++)
				{
					if (links[j].FromCity.Equals(citiesEnRoute[i]) && links[j].ToCity.Equals(citiesEnRoute[i + 1]) 
						|| links[j].ToCity.Equals(citiesEnRoute[i]) && links[j].FromCity.Equals(citiesEnRoute[i + 1]))
					{
						findList.Add(links[j]);
					}
				}
			}
			return findList;
        }

		//Variante mit yield
		//
		//private IEnumerable<Link> FindLinksToCitiesEnRoute(List<City> citiesEnRoute)
		//{
		//	for (int i = 0; i < citiesEnRoute.Count - 1; i++)
		//	{
		//		foreach (var link in links)
		//		{
		//			if (link.FromCity.Equals(citiesEnRoute[i]) && link.ToCity.Equals(citiesEnRoute[i + 1])
		//				|| link.ToCity.Equals(citiesEnRoute[i]) && link.FromCity.Equals(citiesEnRoute[i + 1]))
		//			{
		//				yield return link;
		//			}
		//		}
		//	}
		//}

		private IEnumerable<Link> FindAllLinksForCity(City visitingCity, TransportMode mode)
		{
			for (var i = 0; i < links.Count; i++)
			{
				if ((links[i].FromCity.Equals(visitingCity) || links[i].ToCity.Equals(visitingCity))
                    && links[i].TransportMode.Equals(mode))

					yield return links[i];
			}
		}

		//Bei wie vielen Links treten die drei bevölkerungsreichsten Städte aller Cities in den Links als Start-Stadt auf?
		public int GetCountOfThreeBiggestCitiesInLinks()
		{
			return links.Where(l => cities.CityListEnumerator.OrderByDescending(c => c.Population).Take(3).Contains(l.FromCity)).Count();
		}

		public int GetCountOfThreeCitiesWithLongestNameInLinks()
		{
			return links.Where(l => cities.CityListEnumerator.OrderByDescending(c => c.Name.Length).Take(3).Contains(l.FromCity)
						|| cities.CityListEnumerator.OrderByDescending(c => c.Name.Length).Take(3).Contains(l.ToCity)).Count();
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
