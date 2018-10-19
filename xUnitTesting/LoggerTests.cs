using Forebot2;
using System;
using System.IO;
using Xunit;

namespace xUnitTesting
{
    /// <summary>A class used to test the Text from the console output.</summary>
    class MockConsoleOutput : IDisposable
    {
        /// <summary>
        /// The mock console
        /// </summary>
        private StringWriter mockConsole;
        /// <summary>
        /// The original output of the console.
        /// </summary>
        private readonly TextWriter originalOutput;

        /// <summary>
        /// Construct a new mock console output, use <see cref="Dispose"/> to return to original output.
        /// </summary>
        public MockConsoleOutput()
        {
            mockConsole = new StringWriter();
            originalOutput = Console.Out;
            Console.SetOut(mockConsole);
        }

        /// <summary>
        /// Get the output of the console.
        /// </summary>
        /// <returns>A string representation of the console text.</returns>
        public string GetOuput()
        {
            return mockConsole.ToString();
        }

        /// <summary>
        /// Dispose of the mock console, and return it to the original output.
        /// </summary>
        public void Dispose()
        {
            Console.SetOut(originalOutput);
            mockConsole.Dispose();
        }
    }

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

        [Theory]
        [InlineData(LOG_SEVERITY.DEBUG)]
        [InlineData(LOG_SEVERITY.VERBOSE)]
        [InlineData(LOG_SEVERITY.INFO)]
        [InlineData(LOG_SEVERITY.WARNING)]
        [InlineData(LOG_SEVERITY.ERROR)]
        [InlineData(LOG_SEVERITY.CRITICAL)]
        public void LoggerFormattingTest(LOG_SEVERITY sev)
        {
            const string message = "This is a test message";
            const string source = "Testing";

            string expected;
            string actual;
            using (MockConsoleOutput mco = new MockConsoleOutput())
            {
                ApplicationLogger.Log(message, source, sev);
                expected = $"{DateTime.Now} [{sev,8}] {source}: {message}\r\n";//Note the '\r\n' is required as it writes a new line to the console.

                actual = mco.GetOuput();
                Assert.NotNull(actual);
                Assert.Equal(expected, actual);
            }

        }

    }
}
