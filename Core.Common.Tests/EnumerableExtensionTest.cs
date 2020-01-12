using Core.Common.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Core.Common.Tests
{
    public class EnumerableExtensionTest
    {
        [Theory]
        [MemberData(nameof(CheckMinMaxData))]
        public void CheckMinMax(IEnumerable<int> values, (int min, int max)? minmaxExpected)
        {
            if (minmaxExpected.HasValue)
                Assert.Equal(minmaxExpected, values.MinMax());
            else
                Assert.Throws<InvalidOperationException>(() => values.MinMax());
        }

        public static IEnumerable<object[]> CheckMinMaxData()
        {
            yield return new object[] { new int[] { 1, 5, 3 }, (min: 1, max: 5) };
            yield return new object[] { Enumerable.Range(0, 10), (min: 0, max: 9) };
            yield return new object[] { new int[] { -1 }, (min: -1, max: -1) };
            yield return new object[] { Enumerable.Empty<int>(), null };
        }

        [Theory]
        [MemberData(nameof(CheckGroupByTwoData))]
        public void CheckGroupByTwo(IEnumerable<int> values, IEnumerable<(int a, int b)> expectedResult)
        {
            Assert.Equal(expectedResult, values.GroupByTwo((a, b) => (a, b)));
        }

        public static IEnumerable<object[]> CheckGroupByTwoData()
        {
            yield return new object[] { new int[] { 1, 5, 3, 9 }, new[] { (a: 1, b: 5), (a: 3, b: 9) } };
            yield return new object[] { new int[] { 1, 5, 3, 9, 5 }, new[] { (a: 1, b: 5), (a: 3, b: 9) } };
            yield return new object[] { new int[] { 1 }, Enumerable.Empty<(int a, int b)>() };
            yield return new object[] { new int[] { }, Enumerable.Empty<(int a, int b)>() };
        }

    }
}
