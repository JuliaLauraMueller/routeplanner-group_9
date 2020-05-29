using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{

    public class SimpleObjectWriter : ISimpleObject
    {
        /*
Sie sollen nun die eingelesenen Städte mit einem eigenen Serializer/Deserializer persistieren.
Schreiben Sie einen generischen Serializer, der in der Lage ist beliebige Klassen und deren
öffentlichen Properties vom Typ string, double und int mittels Reflection zu serialisieren und
deserialisieren. Das Format der Serialisierung finden Sie im Anhang.

Legen Sie dazu die neuen Klassen SimpleObjectReader und SimpleObjectWriter im
Namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util an. Das Interface der
Klassen können Sie aus den Unittests ableiten.

Hinweise:

- Sie sollen die Next-Methoden rekursiv implementieren, damit «nested Objects» korrekt
  behandelt werden.
- Verwenden Sie Assembly.GetType(string) damit auch Typen gefunden werden, welche
  internal markiert sind.

*/
        public string Message;
        public int Number;
        public double DecNumber;
        private TextWriter Stream;

        public SimpleObjectWriter(TextWriter stream)
        {
            Stream = stream;         
        }
        long fakultaet_rek(long n)
        { 
        if(n==1){
            return 1;
        }
        else{ // Rekursionsfall
        long ergebnis = n * fakultaet_rek(n - 1); return ergebnis;
        }
        }

public void Next(object c1)
        {
            if(c1 != null)
            {
                Type myType = c1.GetType();
                var obj = c1;
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                var newStream = Stream;
                var i = 0;
                Type type = assemblies[0].GetType(myType.FullName); ;
                while (i < assemblies.Length && type != null)
                {
                    type = assemblies[i].GetType(myType.FullName); // type of assembly
                }
                Stream.WriteLine($"Instance of {{type}}\r\n");
                

                IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

                foreach (PropertyInfo prop in props)
                {
                    object propValue = prop.GetValue(c1, null);
                    //if (propValue.GetType())
                    //{

                        newStream.WriteLine("Location is a nested object...\r\n");
                        Next(propValue);
                    //}
                    //else
                    //{
                        newStream.WriteLine(prop.Name+"="+propValue+"\r\n");
                    //}
                    // Do something with propValue
                }
            }
        }
    }
}
