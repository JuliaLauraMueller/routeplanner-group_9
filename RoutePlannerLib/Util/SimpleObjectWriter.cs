using System;
using System.IO;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
    [Serializable]
    public class SimpleObjectWriter
    {
        //public SimpleObjectWriter(TextWriter stream)
        //{
        //    this.Stream = stream;
        //}

        public TextWriter Stream;
        public string Message;
        public int Number;
        public double DecNumber;
    }
}
