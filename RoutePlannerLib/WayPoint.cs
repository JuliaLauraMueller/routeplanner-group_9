using System;
using System.Collections.Generic;
using System.Text;

namespace Fhnw.Ecnf.RoutPlanner.RoutePlannerLib
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
            double gradToRad = Math.PI / 180;
            int radius = 6371;

            double rad = radius * Math.Acos(Math.Sin(this.Latitude * gradToRad) * Math.Sin(target.Latitude * gradToRad)
                + Math.Cos(this.Latitude * gradToRad) * Math.Cos(target.Latitude * gradToRad)
                * Math.Cos(this.Longitude * gradToRad - target.Longitude * gradToRad));
            return rad;
        }
    }

}
