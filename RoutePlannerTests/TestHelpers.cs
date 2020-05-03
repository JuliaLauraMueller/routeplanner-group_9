using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    /// <summary>
    /// A test helper class which contains generic test methods which are used for concrete
    /// test during the labs.
    /// </summary>
    class TestHelpers
    {
        /// <summary>
        /// Checks if the methods of the passed type conform to the Microsoft coding convention
        /// that methods start with a capital letter.
        /// </summary>
        /// <param name="type"></param>
        public static bool CheckMethodNamesAreCapitalized(Type type)
        {
            var methodInfos = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (var methodInfo in methodInfos)
            {
                var name = methodInfo.Name.Replace("get_", string.Empty).Replace("set_", string.Empty).Replace("<", string.Empty);
                if (!char.IsUpper(name.First())){
                    return false;
                }
            }

            // all are capitilized
            return true;
        }

        public static bool CheckForMethodCallInMethod(string filename, string callingMethod, string calledMethod)
        {
            using (TextReader reader = new StreamReader(filename))
            {
                List<string> sourceCode = ReadFileContentAsEnumerable(reader).ToList<string>();
                //var query = from line in ReadFileContentAsEnumerable(reader); //skip header row

                return (sourceCode.Where(l => l.Contains(calledMethod)).Count() > 0);

            }
        }

        public static IEnumerable<string> ReadFileContentAsEnumerable(TextReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }

        /// <summary>
        /// Simple test if a method is a single liner making a LINQ call (opcode 40)
        /// </summary>
        /// <param name="methodInfo">the method to check</param>
        public static bool CheckForSingleLineLinqUsage(MethodInfo methodInfo)
        {
            // some more not very sophisticated tests to ensure LINQ has been used
            MethodBody mb = methodInfo.GetMethodBody();

            bool localVarCountLow = mb.LocalVariables.Count <=2;

            // the method should be smaller than 100 IL byte instructions
            bool iLCodeLinesLow = mb.GetILAsByteArray().Length < 100;

            bool linqCallIncluded = mb.GetILAsByteArray().ToList().Contains(40);

            return localVarCountLow && iLCodeLinesLow && linqCallIncluded;
        }

    }
}
