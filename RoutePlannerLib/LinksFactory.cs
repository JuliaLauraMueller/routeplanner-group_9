using RoutePlannerLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;


namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class LinksFactory
    {
        static public ILinks Create(Cities cities)
        {
            return Create(cities, AppDomain.CurrentDomain.GetType().FullName);
        }
        static public ILinks Create(Cities cities, string algorithmClassName)
        {
            //TODO: Create Links-class with the supplied name
            //NotSupportedException

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if(assembly.GetType(algorithmClassName) != null)
                {
                    dynamic links = assembly.CreateInstance(assembly.GetType(algorithmClassName).FullName);
                    return links;
                }
                    
            }
            throw new NotSupportedException("algorithmClassName " + algorithmClassName + " not found.");
        }
    }
}
