using Core.Common.Extension;
using System;
using Xunit;

namespace Core.Common.Tests
{
    public class StringExtensionTest
    {
        [Fact]
        public void CheckToJsonFormat()
        {
            const string json1 = "{\"a\":1}";
            var result = json1.ToJsonFormat();
            string nl = Environment.NewLine;
            const string tab = "  ";
            string expected = $"{nl}{tab}\"a\": 1{nl}";
            Assert.Equal("{" + expected + "}", result);

            string json2 = "{\"a\":1, \"b\":\"Toto\"}".Replace(" ", "");
            result = json2.ToJsonFormat();
            expected = $"{nl}{tab}\"a\": 1,{nl}{tab}\"b\": \"Toto\"{nl}";
            Assert.Equal("{" + expected + "}", result);
        }
    }
}
