using Core.Common.Extension;
using System.Collections.Generic;
using Xunit;

namespace Core.Common.Tests
{
    public class ComparableExtensionTest
    {
        [Theory]
        [InlineData(0, 0, 10, 0)]
        [InlineData(10, 0, 10, 10)]
        [InlineData(5, 0, 10, 5)]
        [InlineData(5, 10, 0, 5)]
        [InlineData(-1, 10, 0, 0)]
        [InlineData(15, 10, 0, 10)]
        [InlineData(-1, 0, 10, 0)]
        [InlineData(15, 0, 10, 10)]
        public void CheckClamp(int v, int limit1, int limit2, int expectedValue)
            => Assert.Equal(expectedValue, v.Clamp(limit1, limit2));
    }
}
