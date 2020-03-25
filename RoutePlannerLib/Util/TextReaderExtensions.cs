using System;
using System.Collections.Generic;
using System.IO;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
    public static class TextReaderExtensions
    {
        public static IEnumerable<string[]> GetSplittedLines(this TextReader reader, char splitter)
        {
            string propertyLine;

            while ((propertyLine = reader.ReadLine()) != null)
            {
                string[] Property = propertyLine.ToString().Split(splitter);
                yield return Property;

            }
        } 
        
    }
}
