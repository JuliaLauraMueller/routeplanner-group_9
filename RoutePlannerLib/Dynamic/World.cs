using System;
using System.Dynamic;
using System.Linq;
using RoutePlannerLib;
using Microsoft.CSharp;
using System.Collections.Generic;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Dynamic
{
    public class World : DynamicObject
    {
        private Cities cities;

        public World(Cities cities)
        {
            // if null passed just create an empty to implement graceful behaviour
            this.cities = cities ?? new Cities();
        }

        public override bool TryInvokeMember(            InvokeMemberBinder binder, object[] args,            out object result)
        {

            try
            {
                var city_new = cities[binder.Name]; // Senn's solution
                //var city = cities.FindCity(binder.Name);
                result = city_new;
            }  
            catch (KeyNotFoundException)
            {
                result = $"The city \"{binder.Name}\" does not exist!";
                //result = String.Format("The city \"{0}\" does not exist!", binder.Name); //\"{0}\"  \"Entenhausen\" 
            }
            return true;
        }
    }
}
