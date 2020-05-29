using System;
using System.IO;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
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

    public class SimpleObjectReader : ISimpleObject
    {
        public string Message;
        public int Number;
        public double DecNumber;
        private TextReader Stream;

        public SimpleObjectReader()
        {
        }

        public SimpleObjectReader(TextReader sr)
        {
            Stream = sr;
        }

        public void Next(object obj = null)
        {
            throw new NotImplementedException();
        }
    }
}
