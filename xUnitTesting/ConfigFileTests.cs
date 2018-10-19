using Forebot2;
using System;
using System.IO;
using Xunit;

namespace xUnitTesting
{
    public class ConfigFileTests
    {

        [Fact]
        public void PropertiesTests()
        {
            string expectedFileName = "Forebot.cfg";
            string expectedDefaultDirectory = @".\";
            string expectedDefaultPath = Path.Combine(expectedDefaultDirectory, expectedFileName);

            //Check that the config file name is correct:
            string actual = ConfigurationFile.Name;
            Assert.NotNull(actual);
            Assert.Equal(expectedFileName, actual);          

            //Check that the default directory is correct:
            actual = ConfigurationFile.Location;
            Assert.NotNull(actual);
            Assert.Equal(expectedDefaultDirectory, actual);

            //Check that the default path is correct:
            actual = ConfigurationFile.FullPath;
            Assert.NotNull(actual);
            Assert.Equal(expectedDefaultPath, actual);


            string expectedDirectory = @"F:\";
            string expectedPath = Path.Combine(expectedDirectory, expectedFileName);

            //Check that changing the directory is correct:
            ConfigurationFile.Location = expectedDirectory;
            actual = ConfigurationFile.Location;
            Assert.NotNull(actual);
            Assert.Equal(expectedDirectory, actual);

            //Check that after changing the directory, the path is correct:
            actual = ConfigurationFile.FullPath;
            Assert.NotNull(actual);
            Assert.Equal(expectedPath, actual);
        }
    }
}
