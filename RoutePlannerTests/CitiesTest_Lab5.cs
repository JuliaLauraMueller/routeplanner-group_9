using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Reflection.Emit;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Fhnw.Ecnf.RoutPlanner.RoutePlannerLib;
using RoutePlannerLib;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
  
    public partial class CitiesTest
    {
        [TestMethod]
        public void TestFindNeighborIsASingleLinqStatement()
        {
            Func<WayPoint, double, IEnumerable<City>> method = new Cities().FindNeighbours;
            MethodInfo methodInfo = method.Method;

            Assert.IsTrue(TestHelpers.CheckForSingleLineLinqUsage(methodInfo), "No single line LINQ call implemented");
        }

        [TestMethod]
        public void TestIfLinqAndLambdaIsUsedInReadCities()
        {
            Assert.IsTrue(TestHelpers.CheckForMethodCallInMethod("../../../../RoutePlannerLib/Cities.cs", "ReadCities", "Select"), "No LINQ used in ReadCities");
            Assert.IsTrue(TestHelpers.CheckForMethodCallInMethod("../../../../RoutePlannerLib/Cities.cs", "ReadCities", "=>"), "No Lambda used in ReadCities");
        }
    }


}