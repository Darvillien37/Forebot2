using Forebot2.Model;
using System;
using System.IO;

namespace Forebot2
{
    public class ConfigurationFile
    {
        private const string LOGGER_SOURCE = "Config";

        #region Key Constants for Key-Value pairs
        /// <summary>Key for the Bot Token.</summary>
        private const string KEY_BOT_TOKEN = "BOT_TOKEN";
        /// <summary>Key for the initial log level of the bot.</summary>
        private const string KEY_LOG_LEVEL = "LOG_LEVEL";

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

            ApplicationLogger.Log("Starting: Processing Config File......", LOGGER_SOURCE, LOG_SEVERITY.INFO);
            ApplicationLogger.Log("Location: " + FullPath, LOGGER_SOURCE, LOG_SEVERITY.VERBOSE);

            if (!File.Exists(FullPath))//If the config file does not exist...
            {
                ApplicationLogger.Log("Config file does not exist", LOGGER_SOURCE, LOG_SEVERITY.WARNING);
                InitialiseFreshConfigfile(); //Initialise a fresh one.
                ApplicationLogger.Log("Please complete the Configuration file at: " + FullPath, LOGGER_SOURCE, LOG_SEVERITY.ERROR);
                return false;// return false because the config file has just been created and isn't valid in its initialised state.
            }

            string[] sLinesFromFile;
            try
            {
                sLinesFromFile = File.ReadAllLines(FullPath);
            }
            catch (Exception e)
            {
                ApplicationLogger.Log("Exception when reading configuration file: " + e.Message, LOGGER_SOURCE, LOG_SEVERITY.ERROR);                
                return false;
            }

            for (int i = 0; i < sLinesFromFile.Length; i++)
            {
                ApplicationLogger.Log("Processing line [" + i + "]: " + sLinesFromFile[i], LOGGER_SOURCE, LOG_SEVERITY.VERBOSE);                
                if (!ProcessLine(sLinesFromFile[i])) //If the line isn't valid, return false.
                {
                    ApplicationLogger.Log("Line Invalid: " + i, LOGGER_SOURCE, LOG_SEVERITY.WARNING);                    
                    return false;
                }

            }
            ApplicationLogger.Log("Completed: Processing Config File", LOGGER_SOURCE, LOG_SEVERITY.VERBOSE);            
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
                ApplicationLogger.Log("Blank line found - ignoring...", LOGGER_SOURCE, LOG_SEVERITY.VERBOSE);                
                return true; //Return true, as a blank line is valid.
            }

            if (lineFromFile[0] == COMMENT_CHAR) //Check if the line is a comment.
            {
                ApplicationLogger.Log("Commented line found - ignoring...", LOGGER_SOURCE, LOG_SEVERITY.VERBOSE);                
                return true; //Return true, as a commented line is a valid line.
            }

            //A valid line is in the format of 'Key=Value' or 'key = value', or '   key  =    value    '.
            string[] splitLine = lineFromFile.Split('='); //Split the line around an '='.

            if (splitLine.Length != 2)
            {
                ApplicationLogger.Log("Split length not valid, required: 2,  actual: " + splitLine.Length, LOGGER_SOURCE, LOG_SEVERITY.WARNING);
                return false;//If the split length is not 2, then this line is invalid.
            }

            string key = splitLine[0].Trim();
            string value = splitLine[1].Trim();

            switch (key)//Switch the KEY.
            {
                case KEY_BOT_TOKEN:
                    ApplicationLogger.Log("Setting Bot Token: " + value, LOGGER_SOURCE, LOG_SEVERITY.VERBOSE);
                    mdl.Bot.Token = value;
                    ApplicationLogger.Log("Bot Token set to:" + mdl.Bot.Token, LOGGER_SOURCE, LOG_SEVERITY.VERBOSE);
                    break;

                case KEY_LOG_LEVEL:
                    LOG_SEVERITY ll = LOG_SEVERITY.DEBUG;
                    try
                    {
                        ll = (LOG_SEVERITY)int.Parse(value);
                    }
                    catch (Exception ex)
                    {
                        ApplicationLogger.Log("Value not in correct format: " + ex.Message, LOGGER_SOURCE, LOG_SEVERITY.WARNING);
                        return false;
                    }


                    ApplicationLogger.Log("Setting Bot Log Level: " + ll, LOGGER_SOURCE, LOG_SEVERITY.VERBOSE);
                    mdl.Bot.Severity = ll;
                    ApplicationLogger.Log("Bot Log Level set to:" + mdl.Bot.Severity, LOGGER_SOURCE, LOG_SEVERITY.VERBOSE);
                    break;

                case KEY_DATABASE_ADDRESS:
                    ApplicationLogger.Log("Setting Database Address: " + value, LOGGER_SOURCE, LOG_SEVERITY.VERBOSE);
                    mdl.Database.Address = value;
                    ApplicationLogger.Log("Database Address set to:" + mdl.Database.Address, LOGGER_SOURCE, LOG_SEVERITY.VERBOSE);
                    break;

                case KEY_DATABASE_NAME:
                    ApplicationLogger.Log("Setting Database Name: " + value, LOGGER_SOURCE, LOG_SEVERITY.VERBOSE);
                    mdl.Database.Name = value;
                    ApplicationLogger.Log("Database Name set to:" + mdl.Database.Name, LOGGER_SOURCE, LOG_SEVERITY.VERBOSE);
                    break;

                case KEY_DATABASE_USERNAME:
                    ApplicationLogger.Log("Setting Database Username: " + value, LOGGER_SOURCE, LOG_SEVERITY.VERBOSE);
                    mdl.Database.Username = value;
                    ApplicationLogger.Log("Database Username set to:" + mdl.Database.Username, LOGGER_SOURCE, LOG_SEVERITY.VERBOSE);
                    break;

                case KEY_DATABASE_PASSWORD:
                    ApplicationLogger.Log("Setting Database Password: " + value, LOGGER_SOURCE, LOG_SEVERITY.VERBOSE);
                    mdl.Database.Password = value;
                    ApplicationLogger.Log("Database Password set to:" + mdl.Database.Password, LOGGER_SOURCE, LOG_SEVERITY.VERBOSE);
                    break;

                default:
                    ApplicationLogger.Log("Unknown key:" + key, LOGGER_SOURCE, LOG_SEVERITY.WARNING);
                    return false;
            }
            //If a config line is invalid return false.
            return true;
        }

        /// <summary>Initialise a fresh config file at <see cref="sConfigFilePath"/>.</summary>        
        private static void InitialiseFreshConfigfile()
        {
            ApplicationLogger.Log("Creating Fresh Config File at " + FullPath, LOGGER_SOURCE, LOG_SEVERITY.WARNING);            
            if (!Directory.Exists(Location))// If the directory does not exist...
            {
                ApplicationLogger.Log("tConfig Directory Path does not exist: [" + Location + "] - Creating Directory...", LOGGER_SOURCE, LOG_SEVERITY.WARNING);
                Directory.CreateDirectory(Location);// ...Create it
            }
            using (StreamWriter sw = File.CreateText(FullPath))// Create a fresh config file template (will also overwrite if one already exists)
            {
                sw.WriteLine(COMMENT_CHAR + " Any lines that start with '" + COMMENT_CHAR + "' will be ignored");
                sw.WriteLine(COMMENT_CHAR + " Please do not change the values in front of the '='.");
                sw.WriteLine(COMMENT_CHAR + "Any log level k-v are represented as an integer: " +
                    LOG_SEVERITY.DEBUG + " = " + LOG_SEVERITY.DEBUG.GetHashCode() + "  -> " +
                    LOG_SEVERITY.VERBOSE + " = " + LOG_SEVERITY.VERBOSE.GetHashCode() + "  -> " +
                    LOG_SEVERITY.INFO + " = " + LOG_SEVERITY.INFO.GetHashCode() + "  -> " +
                    LOG_SEVERITY.WARNING + " = " + LOG_SEVERITY.WARNING.GetHashCode() + "  -> " +
                    LOG_SEVERITY.ERROR + " = " + LOG_SEVERITY.ERROR.GetHashCode() + "  -> " +
                    LOG_SEVERITY.CRITICAL + " = " + LOG_SEVERITY.CRITICAL.GetHashCode() + ".");
                sw.WriteLine("");
                sw.WriteLine(COMMENT_CHAR + " Bot Set-up:");
                sw.WriteLine(KEY_BOT_TOKEN + " =           Token Here");
                sw.WriteLine(KEY_LOG_LEVEL + " =           " + LOG_SEVERITY.DEBUG.GetHashCode());                
                sw.WriteLine("");
                sw.WriteLine("# Database Set-up:");
                sw.WriteLine(KEY_DATABASE_ADDRESS + "=    localhost");
                sw.WriteLine(KEY_DATABASE_NAME + "=       Name Here");
                sw.WriteLine(KEY_DATABASE_USERNAME + "=   Username here");
                sw.WriteLine(KEY_DATABASE_PASSWORD + "=   Password here");
            }
            ApplicationLogger.Log("Fresh Config file created", LOGGER_SOURCE, LOG_SEVERITY.INFO);            
        }


    }
}
