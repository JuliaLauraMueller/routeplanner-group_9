using RoutePlannerLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{

	public class RouteRequestWatcher
	{

		private Dictionary<City, int> dict = new Dictionary<City, int>();
		public virtual DateTime GetCurrentDate { get { return DateTime.Now; } }
		private readonly List<Tuple<City, DateTime>> cityRequestsDate = new List<Tuple<City, DateTime>>();
		public void LogRouteRequests(object source, RouteRequestEventArgs args)
		{
			Tuple<City, DateTime> tuple = new Tuple<City, DateTime>(args.FromCity, GetCurrentDate);
			cityRequestsDate.Add(tuple);

			if (dict.ContainsKey(args.ToCity))
			{
				tuple = new Tuple<City, DateTime>(args.ToCity, GetCurrentDate);
				cityRequestsDate.Add(tuple);

				dict[args.ToCity]++;
			}
			else
			{
				dict.Add(args.ToCity, 1);
			}

			Console.WriteLine("Current Request State");
			Console.WriteLine("----------------------");
			foreach (var c in dict)
			{
				Console.WriteLine($"ToCity: {c.Key.Name} has been requested {c.Value} times");
			}
		}

		public int GetCityRequests(City city)
		{
			if (dict.ContainsKey(city))
			{
				Tuple<City, DateTime> tuple = new Tuple<City, DateTime>(city, GetCurrentDate);
				cityRequestsDate.Add(tuple);

				return dict[city];
			}
			return 0;
		}

		// Was waren die drei bevölkerungsreichsten Städte, die an einem bestimmten Tag abgefragt wurden?
		public IEnumerable<City> GetThreeBiggestCityOnDay(DateTime day)
		{
			return
				cityRequestsDate.Where(request => request.Item2.Date == day.Date)
					.Select(request => request.Item1)
					.Distinct()
					.OrderByDescending(c => c.Population)
					.Take(3);
		}

		public IEnumerable<City> GetThreeLongestCityNamesWithinPeriod(DateTime from, DateTime to)
		{
			return
				cityRequestsDate.Where(
						request => request.Item2.Date >= from.Date && request.Item2.Date <= to.Date)
					.Select(request => request.Item1)
					.Distinct()
					.OrderByDescending(c => c.Name.Length)
					.Take(3);
		}

		public IEnumerable<City> GetNotRequestedCities(Cities cities)
		{
			return cities.CityListEnumerator.Where(a => cityRequestsDate
						.Where(b => b.Item2 >= GetCurrentDate.AddDays(-14))
						.All(c => !c.Item1.Equals(a)));
		}
	}
}
