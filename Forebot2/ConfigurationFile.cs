using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Forebot2.Model;

namespace Forebot2
{
    public class ConfigurationFile
    {

        #region Key Constants for Key-Value pairs
        /// <summary>Key for the Bot Token.</summary>
        private const string KEY_BOT_TOKEN = "BOT_TOKEN";

        /// <summary>Key for the Database Address.</summary>
        private const string KEY_DATABASE_ADDRESS = "DATABASE_ADDRESS";
        /// <summary>Key for the Database Name.</summary>
        private const string KEY_DATABASE_NAME = "DATABASE_NAME";
        /// <summary>Key for the Database Username.</summary>
        private const string KEY_DATABASE_USERNAME = "DATABASE_USERNAME";
        /// <summary>Key for the Database Password.</summary>
        private const string KEY_DATABASE_PASSWORD = "DATABASE_PASSWORD";

        #endregion

        /// <summary>Character used to represent a commented line in the config file.</summary>
        private const char COMMENT_CHAR = '#';
        /// <summary>The default directory of the config file.</summary>
        private const string DEFAULT_DIRECTORY = @".\";

        /// <summary>GET the name of the config file for the bot.</summary>
        public static string Name { get; } = "Forebot.cfg";
        /// <summary>GET/SET the directory to find the config file.</summary>
        public static string Location { get; set; } = DEFAULT_DIRECTORY;
        /// <summary>GET the combination of the directory and name of the config file.</summary>
        public static string FullPath { get { return Path.Combine(Location, Name); } }

        /// <summary>Reference to the model.</summary>
        private static readonly BotModel mdl = BotModel.Instance;

        /// <summary>Process the config file. Can also Initialise a fresh file if <see cref="mdl.bCreateFreshConfig"/>.</summary>
        /// <returns>TRUE if a valid config file, FALSE otherwise.</returns>
        public static bool ProcessConfigFile()
        {
            Console.WriteLine("Starting: Processing Config File......");
            Console.WriteLine("\tLocation: " + FullPath);

            if (!File.Exists(FullPath))//If the config file does not exist...
            {
                Console.WriteLine("\tConfig file does not exist");
                InitialiseFreshConfigfile(); //Initialise a fresh one.
            }

            string[] sLinesFromFile;
            try
            {
                sLinesFromFile = File.ReadAllLines(FullPath);
            }
            catch (Exception e)
            {
                Console.Write("Exception when reading configuration file: " + e.Message);
                return false;
            }

            for (int i = 0; i < sLinesFromFile.Length; i++)
            {
                Console.WriteLine("\tProcessing line [" + i + "]: " + sLinesFromFile[i]);
                if (!ProcessLine(sLinesFromFile[i])) //If the line isn't valid, return false.
                {
                    Console.WriteLine("\tLine Invalid");
                    return false;
                }

            }

            Console.WriteLine("Completed: Processing Config File");
            return true;//if we have reached this point, then the config file is valid.
        }

        /// <summary>Process a line from the config file.</summary>
        /// <param name="lineFromFile">A line from the file.</param>
        /// <returns>True if a valid line, false otherwise.</returns>
        private static bool ProcessLine(string lineFromFile)
        {
            if (lineFromFile == null)
            {
                throw new ArgumentNullException("lineFromFile", "Should not be null");
            }

            lineFromFile = lineFromFile.Trim(); //Remove the spaces from the start and end of the line.

            if (lineFromFile.Length == 0)//if the line is blank, "".
            {
                Console.WriteLine("\t\tBlank line found - ignoring...");
                return true; //Return true, as a blank line is valid.
            }

            if (lineFromFile[0] == COMMENT_CHAR) //Check if the line is a comment.
            {
                Console.WriteLine("\t\tCommented line found - ignoring...");
                return true; //Return true, as a commented line is a valid line.
            }

            //A valid line is in the format of 'Key=Value' or 'key = value', or '   key  =    value    '.
            string[] splitLine = lineFromFile.Split('='); //Split the line around an '='.

            if (splitLine.Length != 2)
            {
                Console.WriteLine("\t\tSplit length not valid,required: 2,  actual: " + splitLine.Length);
                return false;//If the split length is not 2, then this line is invalid.
            }

            string key = splitLine[0];
            string value = splitLine[1];            

            switch (key)//Switch the KEY.
            {
                case KEY_BOT_TOKEN:
                    Console.WriteLine("\t\tSetting the Bot Token to: " + value);
                    mdl.Bot.Token = value;
                    Console.WriteLine("\t\tBot Token set to:" + mdl.Bot.Token);
                    break;

                case KEY_DATABASE_ADDRESS:
                    Console.WriteLine("\t\tSetting the Database Address to: " + value);
                    mdl.Database.Address = value;
                    Console.WriteLine("\t\tDatabase Address set to:" + mdl.Database.Address);
                    break;

                case KEY_DATABASE_NAME:
                    Console.WriteLine("\t\tSetting the Database Name to: " + value);
                    mdl.Database.Name = value;
                    Console.WriteLine("\t\tDatabase Name set to:" + mdl.Database.Name);
                    break;

                case KEY_DATABASE_USERNAME:
                    Console.WriteLine("\t\tSetting the Database Username to: " + value);
                    mdl.Database.Username = value;
                    Console.WriteLine("\t\tDatabase Username set to:" + mdl.Database.Username);
                    break;

                case KEY_DATABASE_PASSWORD:
                    Console.WriteLine("\t\tSetting the Database Password to: " + value);
                    mdl.Database.Password = value;
                    Console.WriteLine("\t\tDatabase Password set to:" + mdl.Database.Password);
                    break;


            }
            //If a config line is invalid return false.
            return true;
        }

        /// <summary>Initialise a fresh config file at <see cref="sConfigFilePath"/>.</summary>        
        private static void InitialiseFreshConfigfile()
        {
            Console.WriteLine("\tCreating Fresh Config File at " + FullPath);
            if (!Directory.Exists(Location))// If the directory does not exist...
            {
                Console.WriteLine("\tConfig Directory Path does not exist: [" + Location + "]");
                Console.WriteLine("\tCreating Directory... ");
                Directory.CreateDirectory(Location);// ...Create it
            }
            using (StreamWriter sw = File.CreateText(FullPath))// Create a fresh config file template (will also overwrite if one already exists)
            {
                sw.WriteLine(COMMENT_CHAR + " Any lines that start with '" + COMMENT_CHAR + "' will be ignored");
                sw.WriteLine(COMMENT_CHAR + " Please do not change the values in front of the '='.");
                sw.WriteLine("");
                sw.WriteLine(COMMENT_CHAR + " Bot Set-up:");
                sw.WriteLine(KEY_BOT_TOKEN + " =           Token Here");
                sw.WriteLine("");
                sw.WriteLine("# Database Set-up:");
                sw.WriteLine(KEY_DATABASE_ADDRESS + "=    localhost");
                sw.WriteLine(KEY_DATABASE_NAME + "=       Name Here");
                sw.WriteLine(KEY_DATABASE_USERNAME + "=   Username here");
                sw.WriteLine(KEY_DATABASE_PASSWORD + "=   Password here");
            }
            Console.WriteLine("\tFresh Config file created");
        }


    }
}
