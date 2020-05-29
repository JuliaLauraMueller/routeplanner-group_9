using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using System;
using System.IO;
using System.Reflection;
using RoutePlannerLib;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [TestClass]
    public class LinksFactoryTest
    {
        private const string CitiesTestFile = "citiesTestDataLab4.txt";

        private const string LinksTestFile = "linksTestDataLab4.txt";


        [TestMethod]
        public void TestLoadDynamicValid()
        {
            var cities = new Cities();
            
            // now test for correct dynamic creation of valid routed class passed as string
            var links = LinksFactory.Create(cities, "Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Links");
            Assert.IsInstanceOfType(links, typeof(ILinks));
        }

        [TestMethod]
        public void TestLoadDynamicInvalid()
        {
            var cities = new Cities();
            // pass a name of a class that does not exist
            try
            {
                ILinks links = LinksFactory.Create(cities, "Class.Does.Not.Exist");
                Assert.Fail("Should throw a NotSupportedException");
            }
            catch (NotSupportedException)
            {
            }

            // pass a name of a class that exists, but does not implement the interface
            try
            {
                ILinks links = LinksFactory.Create(cities, "Cities");
                Assert.Fail("Should throw a NotSupportedException");
            }
            catch (NotSupportedException)
            {
            }
        }


        [TestMethod]
        public void TestLoadAndRunDynamicSecondImpl()
        {
            var cities = new Cities();
            cities.ReadCities(CitiesTestFile);

            ILinks links = LinksFactory.Create(cities,
                "Fhnw.Ecnf.RoutePlanner.RoutePlannerTest.TestLinksValidConstructor");
            Assert.AreEqual(42, links.ReadLinks("nonsense"));

            ILinks links2 = LinksFactory.Create(cities, "Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Links");
            Assert.AreNotEqual(links.GetType(), links2.GetType());

            try
            {
                LinksFactory.Create(cities,
                    "Fhnw.Ecnf.RoutePlanner.RoutePlannerTest.Lab5Test.TestLinksInvalidConstructor");
                Assert.Fail("Should throw a NotSupportedException, because it doesn't have the right constructor");
            }
            catch (NotSupportedException)
            {
            }

            try
            {
                LinksFactory.Create(cities, "Fhnw.Ecnf.RoutePlanner.RoutePlannerTest.Lab10Test.TestLinksNoInterface");
                Assert.Fail("Should throw a NotSupportedException, because ILinks is not implemented");
            }
            catch (NotSupportedException)
            {
            }
        }

        [TestMethod]
        public void TestLoadAndRunDynamicClassicImpl()
        {
            var cities = new Cities();
            cities.ReadCities(CitiesTestFile);

            ILinks links = LinksFactory.Create(cities, "Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Links");

            links.ReadLinks(LinksTestFile);

            Assert.AreEqual(28, cities.Count);

            // test available cities
            List<Link> resultLinks = links.FindShortestRouteBetween("Zürich", "Basel", TransportMode.Rail);

            var expectedLinks = new List<Link>();
            expectedLinks.Add(new Link(new City("Zürich", "Switzerland", 7000, 1, 2),
                new City("Aarau", "Switzerland", 7000, 1, 2), 0));
            expectedLinks.Add(new Link(new City("Aarau", "Switzerland", 7000, 1, 2),
                new City("Liestal", "Switzerland", 7000, 1, 2), 0));
            expectedLinks.Add(new Link(new City("Liestal", "Switzerland", 7000, 1, 2),
                new City("Basel", "Switzerland", 7000, 1, 2), 0));

            Assert.IsNotNull(resultLinks);
            Assert.AreEqual(expectedLinks.Count, resultLinks.Count);

            for (int i = 0; i < resultLinks.Count; i++)
                Assert.IsTrue(
                    (expectedLinks[i].FromCity.Name == resultLinks[i].FromCity.Name &&
                     expectedLinks[i].ToCity.Name == resultLinks[i].ToCity.Name) ||
                    (expectedLinks[i].FromCity.Name == resultLinks[i].ToCity.Name &&
                     expectedLinks[i].ToCity.Name == resultLinks[i].FromCity.Name));

            try
            {
                resultLinks = links.FindShortestRouteBetween("doesNotExist", "either", TransportMode.Rail);
                Assert.Fail("Should throw a KeyNotFoundException");
            }
            catch (KeyNotFoundException)
            {
            }
        }
    }

    class TestLinksInvalidConstructor : ILinks
    {
        public List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportMode mode)
        {
            return null;
        }

        public List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportMode mode, IProgress<string> reportProgress = null)
        {
            throw new NotImplementedException();
        }

        public int ReadLinks(string filename)
        {
            return 42;
        }
    }

    public class TestLinksValidConstructor : ILinks
    {
        public TestLinksValidConstructor(Cities c)
        {
        }

        public List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportMode mode)
        {
            return null;
        }

        public List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportMode mode, IProgress<string> reportProgress = null)
        {
            throw new NotImplementedException();
        }

        public int ReadLinks(string filename)
        {
            return 42;
        }
    }

    class TestLinksNoInterface
    {
        public TestLinksNoInterface(Cities c)
        {
        }

        public List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportMode mode)
        {
            return null;
        }

        public int ReadLinks(string filename)
        {
            return 42;
        }
    }	
}