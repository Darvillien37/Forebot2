using Forebot2;
using System;
using Xunit;

namespace xUnitTesting
{
    public class LoggerTests
    {
        [Fact]
        public void BasicLoggerTest()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ApplicationLogger.Log(null, "BasicLoggerTest", LOG_SEVERITY.DEBUG));

            Assert.Throws<ArgumentNullException>(() =>
                ApplicationLogger.Log("A unit test method", null, LOG_SEVERITY.DEBUG));

            Assert.Throws<ArgumentNullException>(() =>
                ApplicationLogger.Log(null, null, LOG_SEVERITY.DEBUG));
        }
    }
}
