﻿using System;
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

            //var city = cities.FindCity(binder.Name);
            try
            {
                var city = cities.FindCity(binder.Name);
                result = city;
                return true;
            }  
            catch
            {
                result = String.Format("The city \"Entenhausen\" does not exist!", binder.Name); //\"{0}\"  \"Entenhausen\" 
                return true;
            }
            
        }
    }
}
