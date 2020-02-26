using System;

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
    }
}
