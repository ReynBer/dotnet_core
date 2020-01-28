using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Core.Common.Tests
{
    public class ListComparerTest
    {
        [Theory]
        [InlineData(new[] { 1, 2 }, new[] { 1, 2 }, true)]
        [InlineData(new[] { 1, 2 }, new[] { 2, 1 }, false)]
        [InlineData(new[] { 1 }, new[] { 1, 2 }, false)]
        [InlineData(new[] { 1, 2 }, new[] { 1 }, false)]
        public void CheckListEqualityWithoutOrdering(IEnumerable<int> v1, IEnumerable<int> v2, bool expected)
            => Assert.Equal(expected, new ListComparer<int>((a, b) => a == b).Equals(v1, v2));

        [Theory]
        [InlineData(new[] { 1, 2 }, new[] { 1, 2 }, true)]
        [InlineData(new[] { 1, 2 }, new[] { 2, 1 }, true)]
        [InlineData(new[] { 1 }, new[] { 1, 2 }, false)]
        [InlineData(new[] { 1, 2 }, new[] { 1 }, false)]
        public void CheckListEqualityWithOrdering(IEnumerable<int> v1, IEnumerable<int> v2, bool expected)
            => Assert.Equal(new ListComparer<int>((a, b) => a == b).Equals(v1.OrderBy(v => v), v2.OrderBy(v => v)), expected);

    }
}
