
namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
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
}
