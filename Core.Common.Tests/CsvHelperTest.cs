using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Core.Common.Tests
{
    public class CsvHelperTest
    {
        [Theory]
        [MemberData(nameof(CheckReadFromData))]
        public void CheckReadFrom(string pathFile, IEnumerable<string[]> expectedResult)
        {
            Assert.Equal(expectedResult, CsvHelper.ReadFrom(pathFile, "|"));
        }


        public static IEnumerable<object[]> CheckReadFromData()
        { 
            yield return new object[] { Path.Combine("Assets", "Csv1.csv"), Csv1 };
        }

        [Theory]
        [MemberData(nameof(CheckCreateValuesData))]
        public void CheckCreateValues(string pathFile, IEnumerable<IDictionary<string, string>> expectedResult)
        {
            Assert.Equal(expectedResult, CsvHelper.ReadFrom(pathFile, "|").CreateValues());
        }

        public static IEnumerable<object[]> CheckCreateValuesData()
        {
            yield return new object[] { Path.Combine("Assets", "Csv1.csv"), Csv1Dico };
        }

        private static IEnumerable<string[]> Csv1
        {
            get
            {
                return new[]
                {
                    new string [] { "a", "b" , "c" }
                    , new string [] { "1", "2" , "3" }
                    , new string [] { "5", "4" , "1" }
                };
            }
        }
        private static IEnumerable<Dictionary<string, string>> Csv1Dico
        {
            get
            {
                return new[]
                {
                    new Dictionary<string, string>() { { "a", "1" }, { "b", "2" }, { "c", "3" } }
                    , new Dictionary<string, string>() { { "a", "5" }, { "b", "4" }, { "c", "1" } }
                };
            }
        }
    }
}
