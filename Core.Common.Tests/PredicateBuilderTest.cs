using System.Linq;
using Xunit;

namespace Core.Common.Tests
{
    public class PredicateBuilderTest
    {
        [Fact]
        public void CheckInAndOr()
        {
            var values = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.AsQueryable();
            var exp = PredicateBuilder<int>.In(v => v, new int[] { 1, 3, 2, 6 }).And(v => v % 2 == 0).Or(v => v == 1);
            var expectedResult = new[] { 1, 2, 6 };
            var result = values.Where(exp).ToArray();
            Assert.Equal(expectedResult, result);
            exp = PredicateBuilder<int>.In(new int[] { 1, 3, 2, 6 }).And(v => v % 2 == 0).Or(v => v == 1);
            result = values.Where(exp).ToArray();
            Assert.Equal(expectedResult, result);
            var values2 = new[] { (v: true, r: 1), (v: false, r: 2), (v: true, r: 3) }.AsQueryable();
            var exp2 = PredicateBuilder
                .From(values2)
                .And(v => v.r == 1);
            var expectedResult2 = new[] { (v: true, r: 1) };
            var result2 = values2.Where(exp2).ToArray();
            Assert.Equal(expectedResult2, result2);
            var values3 = new[] { new { v = true, r = 1 }, new { v = false, r = 2 }, new { v = true, r = 3 } }.AsQueryable();
            var exp3 = PredicateBuilder
                .From(values3)
                .And(v => v.r == 1)
                .And(v => v.v)
                .And(v => true.Equals(true));
            var expectedResult3 = new[] { new { v = true, r = 1 } };
            var result3 = values3.Where(exp3).ToArray();
            Assert.Equal(expectedResult3, result3);
        }

        [Fact]
        public void CheckEqual()
        {
            var values = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.AsQueryable();
            var exp = PredicateBuilder<int>.Equal(v => v, 5);
            var expectedResult = new[] { 5 };
            var result = values.Where(exp).ToArray();
            Assert.Equal(expectedResult, result);
            exp = PredicateBuilder<int>.Equal(5);
            result = values.Where(exp).ToArray();
            Assert.Equal(expectedResult, result);
        }
    }
}
