using RoutePlannerLib;
using System;
using System.Collections.Generic;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
	public enum TransportMode
	{
		Ship,
		Rail,
		Flight,
		Car,
		Bus,
		Tram
	};
	
	/// <summary>
	/// Represents a link between two cities with its distance
	/// </summary>
	public class Link : IComparable
	{
        public City FromCity { get; set; }
        public City ToCity { get; set; }
        public double Distance { get; set; }
        public TransportMode TransportMode { get; set; } = TransportMode.Car;
		
		public Link(City _fromCity, City _toCity, double _distance)
		{
			FromCity = _fromCity;
			ToCity = _toCity;
			Distance = _distance;
		}

		public Link(City _fromCity, City _toCity, double _distance, TransportMode _transportMode) : this(_fromCity, _toCity, _distance)
		{
			TransportMode = _transportMode;
		}
        
        /// <summary>
        /// Uses distance as default comparison criteria 
        /// </summary>
        public int CompareTo(object o)
		{
			// Wieso die Schreibweise (Link)o ?
			return Distance.CompareTo(((Link)o).Distance);
		}
		
		/// <summary>
		/// checks if both cities of the link are included in the passed city list
		/// </summary>
		/// <param name="cities">list of city objects</param>
		/// <returns>true if both link-cities are in the list</returns>
		internal bool IsIncludedIn(List<City> cities)
		{
			var foundFrom = false;
			var foundTo = false;
			foreach (var c in cities)
			{
				if (!foundFrom && c.Name == FromCity.Name)
					foundFrom = true;
				
				if (!foundTo && c.Name == ToCity.Name)
					foundTo = true;
			}
			
			return foundTo && foundFrom;
		}
	}
}
