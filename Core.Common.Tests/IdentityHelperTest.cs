using Xunit;

namespace Core.Common.Tests
{
    public class IdentityHelperTest
    {
        [Theory]
        [InlineData(1)]
        public void CheckIdentityInt(int value)
        {
            Assert.Equal(value, IdentityHelper.Get<int>()(value));
            Assert.Equal(value, IdentityHelper.GetExp<int>().Compile()(value));
        }

        [Theory]
        [InlineData("1")]
        public void CheckIdentityString(string value)
        {
            Assert.Equal(value, IdentityHelper.Get<string>()(value));
            Assert.Equal(value, IdentityHelper.GetExp<string>().Compile()(value));
        }
    }
}
