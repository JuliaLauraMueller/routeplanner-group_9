using System;
using System.Collections.Generic;
using System.IO;

namespace Fhnw.Ecnf.RoutPlanner.RoutePlannerLib.Util
{
    public static class TextReaderExtensions
    {
        public static IEnumerable<string[]> GetSplittedLines(this TextReader reader, char splitter)
        {
            string[] Property = reader.ToString().Split(splitter);
            yield return Property;
        } 
        
    }
}
