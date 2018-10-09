using Forebot2;
using Xunit;

namespace xUnitTesting
{
    public class UtilityTests
    {
        [Fact]
        public void MyFirstTest()
        {
            const int expected = 5;

            int actual = Utilities.MyUtilityTest(expected);

            Assert.Equal(expected, actual);
        }
    }
}
