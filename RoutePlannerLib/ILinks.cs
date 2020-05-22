﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public interface ILinks
    {
        /// <summary>
        /// Reads a list of links from the given file.
        /// Reads only links between existing cities.
        /// </summary>
        /// <param name="filename">name of input file</param>
        /// <returns>number of links read</returns>
        int ReadLinks(string filename);
        /// <summary>
        /// Determines the shortest path between the given cities.
        /// </summary>
        /// <param name="fromCity">name of start city</param>
        /// <param name="toCity">name of destination city</param>
        /// <param name="mode">transportation mode</param>
        /// <returns></returns>
        List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportMode mode, IProgress<string> reportProgress = null);
    }
}
