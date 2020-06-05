using System;
using System.IO;
using System.Linq;
using System.Globalization;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
    public class SimpleObjectReader
    {
        private TextReader streamReader;

        public SimpleObjectReader(TextReader sr)
        {
            if(sr == null)
            {
                throw new NullReferenceException("TextReader may not be null");
            }
            streamReader = sr;
        }

        public object Next()
        {
            var obj = CreateInstance(streamReader.ReadLine().Substring("Instance of ".Length));

            foreach(var i in obj.GetType().GetProperties().OrderBy(i2 => i2.Name)) // Properties von obj holen
            {
                if(i.PropertyType == typeof(string))
                {
                    var prop = streamReader.ReadLine().Substring(i.Name.Length + 2); 
                    i.SetValue(obj, prop.Substring(0, prop.Length - 1));
                }
                else if(i.PropertyType == typeof(int))
                {
                    i.SetValue(obj, int.Parse(streamReader.ReadLine().Substring(i.Name.Length + 1)));
                }
                else if(i.PropertyType == typeof(double))
                {
                    i.SetValue(obj, double.Parse(streamReader.ReadLine().Substring(i.Name.Length + 1), CultureInfo.InvariantCulture));
                }
                else
                {
                    streamReader.ReadLine();
                    i.SetValue(obj, Next());
                }
            }

            streamReader.ReadLine();
            return obj;
        }

        static private object CreateInstance(string _class)
        {
            // Durch alle Assyemlbies durch gehen von AppDomain.CurrentDomain.GetAssemblies()
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach(var assembly in assemblies)
            {
                var type = assembly.GetType(_class); // type of assembly
                if(type != null)
                {
                    var c = type.GetConstructor(new Type[] { });
                    if(c != null)
                    {
                        return c.Invoke(new object[] { });
                    }
                }
            }
            throw new NotSupportedException(_class);
        }
    }
}
