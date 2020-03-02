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

        public override string ToString()
        {
            return $"Waypoint: {this.Name} {this.Latitude:F2} / {this.Longitude:F2}";
        }

        public double Distance(WayPoint target)
        {
            double grad;
            int r = 6371;
            grad = r * Math.Acos(Math.Sin(this.Latitude) * Math.Sin(target.Latitude)
                + Math.Cos(this.Latitude) * Math.Cos(target.Latitude) * Math.Cos(this.Longitude - target.Longitude));
            return grad * (Math.PI / 180);
        }
    }
}
