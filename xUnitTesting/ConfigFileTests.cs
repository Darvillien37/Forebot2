using Forebot2;
using System;
using System.IO;
using Xunit;

namespace xUnitTesting
{
    public class ConfigFileTests
    {
        private const string TESTING_CONFIG_DIRECTORY = @".\ConfigFileTesting";

        private const char COMMENT_CHAR = '#';
        private const string KEY_BOT_TOKEN = "BOT_TOKEN";
        private const string KEY_LOG_LEVEL = "LOG_LEVEL";
        private const string KEY_DATABASE_ADDRESS = "DATABASE_ADDRESS";
        private const string KEY_DATABASE_NAME = "DATABASE_NAME";
        private const string KEY_DATABASE_USERNAME = "DATABASE_USERNAME";
        private const string KEY_DATABASE_PASSWORD = "DATABASE_PASSWORD";

        [Fact]
        public void PropertiesTests()
        {   
            //Save the original to set it back.
            string originalDirectory = ConfigurationFile.Location;
            //Set the expected values.
            string expectedDefaultDirectory = @".\";
            string expectedDefaultPath = Path.Combine(expectedDefaultDirectory, ConfigurationFile.Name);

            //---------------------- Test the unchanged values -----------------------------
            //Check that the config file name is correct:
            string actual = ConfigurationFile.Name;
            Assert.NotNull(actual);
            Assert.Equal(ConfigurationFile.Name, actual);

            //Check that the default directory is correct:
            actual = ConfigurationFile.Location;
            Assert.NotNull(actual);
            Assert.Equal(expectedDefaultDirectory, actual);

            //Check that the default path is correct:
            actual = ConfigurationFile.FullPath;
            Assert.NotNull(actual);
            Assert.Equal(expectedDefaultPath, actual);
            
            //---------------------- Change the properties and test them -----------------------------
            string expectedDirectory = @"F:\";
            string expectedPath = Path.Combine(expectedDirectory, ConfigurationFile.Name);

            //Check that changing the directory is correct:
            ConfigurationFile.Location = expectedDirectory;
            actual = ConfigurationFile.Location;
            Assert.NotNull(actual);
            Assert.Equal(expectedDirectory, actual);

            //Check that after changing the directory, the path is correct:
            actual = ConfigurationFile.FullPath;
            Assert.NotNull(actual);
            Assert.Equal(expectedPath, actual);

            //---------------------- Set the properties back to the original -----------------------------
            ConfigurationFile.Location = originalDirectory;
        }

        [Fact]
        public void FileNonExistantTest()
        {
            //Save the original location to set it back
            string originalDirectory = ConfigurationFile.Location;

            string TestingDirectory = TESTING_CONFIG_DIRECTORY + @"\FileNonExistantTest";
            ConfigurationFile.Location = TestingDirectory;

            //Confirm first that the file doesn't exist.
            Assert.False(File.Exists(ConfigurationFile.FullPath), "Failed: File Already exists.");

            //Confirm that the processing the file returns false, as it has only just been created, and it doesn't contain correct data.
            Assert.False(ConfigurationFile.ProcessConfigFile(), "Failed: Processing file passed, thus test failed.");

            //Confirm File has now been created.
            Assert.True(File.Exists(ConfigurationFile.FullPath), "Failed: The file was not created.");

            //Now delete the file as we don't want to use it anymore.
            File.Delete(ConfigurationFile.FullPath);

            //Confirm that it has been deleted.
            Assert.False(File.Exists(ConfigurationFile.FullPath), "Failed: The file was not deleted.");

            //Set back to the original location
            ConfigurationFile.Location = originalDirectory;

        }

        [Fact]
        public void ValidFileTest()
        {
            //Save the original to set it back.
            string originalDirectory = ConfigurationFile.Location;
            
            string testingDirectory = TESTING_CONFIG_DIRECTORY + @"\ValidFileTest";
            string path = Path.Combine(testingDirectory, ConfigurationFile.Name);

            Directory.CreateDirectory(testingDirectory);// Create the directory if it doesn't exist.
            ConfigurationFile.Location = testingDirectory;
            
            //write a valid file:
            using (StreamWriter sw = File.CreateText(path))//Create a custom invalid file
            {
                sw.WriteLine(COMMENT_CHAR + "A Commented Line");
                sw.WriteLine();//A blank line.
                sw.WriteLine("   ");//A line with spaces.                
                sw.WriteLine(KEY_BOT_TOKEN + " =           Token Here");
                sw.WriteLine(KEY_LOG_LEVEL + " =           " + LOG_SEVERITY.DEBUG.GetHashCode());
                sw.WriteLine(KEY_DATABASE_ADDRESS + "=    localhost");
                sw.WriteLine(KEY_DATABASE_NAME + "=       Name Here");
                sw.WriteLine(KEY_DATABASE_USERNAME + "=   Username here");
                sw.WriteLine(KEY_DATABASE_PASSWORD + "=   Password here");
            }

            Assert.True(ConfigurationFile.ProcessConfigFile(), "Failed: ValidFileTest, file not processed correctly");

            //Set back to the original location.
            ConfigurationFile.Location = originalDirectory;
        }

        public class InvalidKeyValueTests
        {
            const string TESTING_DIRECTORY = TESTING_CONFIG_DIRECTORY + @"\InvalidKeyValueTest";


            [Fact]
            public void InvalidLogLevel()
            {
                //Save the original to set it back.
                string originalDirectory = ConfigurationFile.Location;

                string path = Path.Combine(TESTING_DIRECTORY, ConfigurationFile.Name);
                Directory.CreateDirectory(TESTING_DIRECTORY);// Create the directory if it doesn't exist.
                ConfigurationFile.Location = TESTING_DIRECTORY;

                //Test with more than 1 equals.
                using (StreamWriter sw = File.CreateText(path))//Create a custom invalid file
                {
                    sw.WriteLine("KEY_LOG_LEVEL = -1");
                }
                Assert.True(File.Exists(path), "Failed: File not created");
                Assert.False(ConfigurationFile.ProcessConfigFile(), "Failed: KEY_LOG_LEVEL = -1.");


                //Test with more than 1 equals.
                using (StreamWriter sw = File.CreateText(path))//Create a custom invalid file
                {
                    sw.WriteLine("KEY_LOG_LEVEL = 10");
                }
                Assert.True(File.Exists(path), "Failed: File not created");
                Assert.False(ConfigurationFile.ProcessConfigFile(), "Failed: KEY_LOG_LEVEL = -1.");

                //Set back to the original location
                 ConfigurationFile.Location = originalDirectory;

            }

            //ToDo: more invalid key value pairs


        }

        public class InvalidLinesTest
        {
            const string TESTING_DIRECTORY = TESTING_CONFIG_DIRECTORY + @"\InvalidLinesTest";

            [Fact]
            public void SplitLinesTest()
            {
                //Save the original to set it back.
                string originalDirectory = ConfigurationFile.Location;

                string path = Path.Combine(TESTING_DIRECTORY, ConfigurationFile.Name);
                Directory.CreateDirectory(TESTING_DIRECTORY);// Create the directory if it doesn't exist.

                //Test with more than 1 equals.
                using (StreamWriter sw = File.CreateText(path))//Create a custom invalid file
                {
                    sw.WriteLine("Line=With=TwoEquals");
                }
                Assert.True(File.Exists(path), "Failed: File not created");
                ConfigurationFile.Location = TESTING_DIRECTORY;
                Assert.False(ConfigurationFile.ProcessConfigFile(), "Failed: Line=With=TwoEquals.");

                //Test with no Equals:
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("LineWithNoEquals");
                }
                Assert.True(File.Exists(path), "Failed: File not created");
                ConfigurationFile.Location = TESTING_DIRECTORY;
                Assert.False(ConfigurationFile.ProcessConfigFile(), "Failed: LineWithNoEquals.");

                //Set back to the original location
                ConfigurationFile.Location = originalDirectory;

            }

            [Fact]
            public void UnknownKeyTest()
            {
                //Save the original to set it back.
                string originalDirectory = ConfigurationFile.Location;

                string path = Path.Combine(TESTING_DIRECTORY, ConfigurationFile.Name);
                Directory.CreateDirectory(TESTING_DIRECTORY);// Create the directory if it doesn't exist.

                //Test with more than 1 equals.
                using (StreamWriter sw = File.CreateText(path))//Create a custom invalid file
                {
                    sw.WriteLine("InvalidKey=aValue");
                }
                Assert.True(File.Exists(path), "Failed: File not created");
                ConfigurationFile.Location = TESTING_DIRECTORY;
                Assert.False(ConfigurationFile.ProcessConfigFile(), "Failed: Line=With=TwoEquals.");


                //Set back to the original location
                ConfigurationFile.Location = originalDirectory;
            }

        }


    }
}
