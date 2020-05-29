using RoutePlannerLib;
using System;
using System.Reflection;


namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class LinksFactory
    {
        static public ILinks Create(Cities cities, string algorithmClassName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            foreach (var assembly in assemblies)
            {
                var type = assembly.GetType(algorithmClassName); // type of assembly

                if (type != null)
                {
                    ConstructorInfo cstr = type.GetConstructor(new[] { typeof(Cities) });
                    object instance = cstr.Invoke(new object[] { cities });
                    return (ILinks)instance;
                }
            }
            throw new NotSupportedException("algorithmClassName " + algorithmClassName + " not found.");
        }
    }
}


