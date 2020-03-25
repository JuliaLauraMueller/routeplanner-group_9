using System;
using System.Collections.Generic;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{

	public class RouteRequestWatcher
	{

		private Dictionary<City, int> dict = new Dictionary<City, int>();
		public void LogRouteRequests(object source, RouteRequestEventArgs args)
		{
			if (dict.ContainsKey(args.ToCity))
			{
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
				return dict[city];
			}
			return 0;
		}
	}
}
