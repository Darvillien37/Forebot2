using Forebot2;
using System;
using System.IO;
using Xunit;

namespace xUnitTesting
{
    public class ConfigFileTests
    {

        private const string TESTING_CONFIG_DIRECTORY = @".\ConfigFileTesting";

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

        [Fact]
        public void FileNonExistantTest()
        {
            string TestingDirectory = TESTING_CONFIG_DIRECTORY + @"\FileNonExistantTest";
            ConfigurationFile.Location = TestingDirectory;

            //Confirm first that the file doesn't exist.
            Assert.False(File.Exists(ConfigurationFile.FullPath));

            //Confirm that the processing the file returns false, as it has only just been created, and it doesn't contain correct data.
            Assert.False(ConfigurationFile.ProcessConfigFile());

            //Confirm File has now been created.
            Assert.True(File.Exists(ConfigurationFile.FullPath));

            //Now delete the file as we don't want to use it anymore.
            File.Delete(ConfigurationFile.FullPath);

            //Confirm that it has been deleted.
            Assert.False(File.Exists(ConfigurationFile.FullPath));

        }

        [Fact]
        public void ValidFileTest()
        {
            string TestingDirectory = TESTING_CONFIG_DIRECTORY + @"\ValidFileTest";
            ConfigurationFile.Location = TestingDirectory;
            //Confirm that an already valid file exists for this test:            
            Assert.True(File.Exists(ConfigurationFile.FullPath));

            //Confirm that the file has been processed correctly:
            Assert.True(ConfigurationFile.ProcessConfigFile(), "Processing was not correct");
        }
    }
}
