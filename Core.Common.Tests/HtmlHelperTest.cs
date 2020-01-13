using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Core.Common.Tests
{
    public class HtmlHelperTest
    {
        [Theory]
        [MemberData(nameof(CheckGetQueryStringData))]
        public void CheckGetQueryString(string url, IDictionary<string, string> expected)
        {
            var result = HtmlHelper.GetQueryString(url);
            Assert.Equal(result, expected);
        }

        public static IEnumerable<object[]> CheckGetQueryStringData()
        {
            yield return new object[] 
            { 
                "http://toto.com/specificities?a=16&b=5&page=0&page_size=200"
                , new Dictionary<string, string>() { { "a", "16" }, { "b", "5" }, { "page", "0" }, { "page_size", "200" }, } 
            };
        }
    }
}
