using System;
using System.Dynamic;
using System.Linq;
using RoutePlannerLib;
using Microsoft.CSharp;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Dynamic
{
    public class World : DynamicObject
    {
        private Cities cities;

        public World(Cities cities)
        {
            this.cities = cities;
        }

        public override bool TryInvokeMember(            InvokeMemberBinder binder, object[] args,            out object result)
        {

            var city = cities.FindCity(binder.Name);

            if (city == null)
            {
                result = String.Format("The city \"{0}\" does not exist!", binder.Name);
                return false;
            }
            else
                result = city;
                
            return true;
        }
    }
}
