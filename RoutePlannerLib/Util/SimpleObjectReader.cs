using System;
using System.IO;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
    [Serializable]
    public class SimpleObjectReader : Attribute
    {
        private StringReader sr;

        public SimpleObjectReader()
        {
        }

        public SimpleObjectReader(StringReader sr)
        {
            this.sr = sr;
        }
    }
}
