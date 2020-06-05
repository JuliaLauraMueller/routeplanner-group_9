using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{

    public class SimpleObjectWriter 
    { 
        private TextWriter streamWriter;

        public SimpleObjectWriter(TextWriter stream)
        {
            streamWriter = stream;
        }

        public void Next(object obj)
        {
            if(obj != null)
            {
                streamWriter.WriteLine("Instance of " + obj.GetType().FullName);
                foreach (var i in obj.GetType().GetProperties().OrderBy(i => i.Name))
                {
                    if (i.PropertyType == typeof(string))
                        streamWriter.WriteLine(i.Name + "=\"" + i.GetValue(obj) + "\"");
                    else if(i.PropertyType == typeof(int))
                        streamWriter.WriteLine(i.Name + "=" + i.GetValue(obj).ToString());
                    else if (i.PropertyType == typeof(double))
                        streamWriter.WriteLine(i.Name + "="
                            + ((double)i.GetValue(obj)).ToString(CultureInfo.InvariantCulture));
                    else
                    {
                        streamWriter.WriteLine(i.Name + " is a nested object...");
                        Next(i.GetValue(obj));
                    }
                }
                streamWriter.WriteLine("End of instance");
            }
        }
    }
}
