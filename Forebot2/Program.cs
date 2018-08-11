using Forebot2.Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Forebot2
{
    /// <summary>The main program class for this application.</summary>
    class Program
    {
        const string ARG_FORMAT_EXAMPLE = "[key]=[value]";
        const string ARG_CONFIG_DIRECTORY_KEY = "cnfgDir";
        const string ARG_CREATE_FRESH_CONFIG = "freshCnfg";

        /// <summary>Refference to the Model Root of this application.</summary>
        private static readonly BotModel mdl = BotModel.Instance;


        /// <summary>The main starting point of any application.</summary>
        /// <remarks>If you had to read this to figure out what main is.....git gud scrub.</remarks>
        /// <param name="args">Arguments passed in to the application, from the command line, or shortcut.</param>
        static void Main(string[] args)
        {
            //Check if this applicaiton is already running...
            if (IsApplicationAlreadyRunning())
            {//...if it is, shut this instance down.
                Console.WriteLine("Only one instance of " + Process.GetCurrentProcess().ProcessName + " Can be run a a time.");
                Console.WriteLine("Shutting down this instance ... Goodbye!!");
                return; //returning in main exits the applicaiton.
            }

            //Initialising:
            ProcessArgs(args);
            Console.WriteLine();

            if (!ProcessConfigFile())//Will return false if a config line is invalid.
            {
                Console.WriteLine("WARNING - Configuration file is invalid, shutting down.");
                return;
            }


            Console.WriteLine("Hello World!");
            Console.WriteLine("Test - Value in model: " + mdl.sTestString);
            Console.ReadKey();

        }

        /// <summary>Check if this application is already running.</summary>
        /// <returns>TRUE if application is already running, FALSE otherwise</returns>
        private static bool IsApplicationAlreadyRunning()
        {
            string sProcessName = Process.GetCurrentProcess().ProcessName;
            if (Process.GetProcessesByName(sProcessName).Length > 1)
                return true;
            else
                return false;
        }

        /// <summary>Process the arguments passed in to the application.</summary>
        /// <param name="args">Arguments passed in to the application.</param>
        private static void ProcessArgs(string[] args)
        {
            Console.WriteLine("Processing Arguments.....");
            if (args == null)//if the array is null, throw an exception.
            {
                throw new ArgumentNullException("args", "Parameter contains null array.");
            }

            string[] sArgSplit;

            for (int i = 0; i < args.Length; i++)//Loop through each argument.
            {
                sArgSplit = args[i].Split('=');//Each arg should be in for form of [key]=[value], so split around the '='.

                if (sArgSplit.Length == 1)//if the length of the array is 1, then it isn't a key-value pair.
                {
                    Console.WriteLine("Argument [" + args[i] + "] Error: Not in the correct format, the format is:" + ARG_FORMAT_EXAMPLE);//So write to the console for debug reasons.
                    continue;//and continue with the next iteration of the for loop.
                }

                switch (sArgSplit[0])//Do a switch around the [key]
                {
                    case ARG_CONFIG_DIRECTORY_KEY:
                        Console.WriteLine("Setting Config directory to: " + sArgSplit[1]);
                        mdl.sConfigFileDirectory = sArgSplit[1];
                        break;

                    case ARG_CREATE_FRESH_CONFIG:
                        string createFresh = sArgSplit[1].ToUpper();

                        if (mdl.sTrueStrings.Contains(createFresh))
                        {
                            Console.WriteLine("Creating fresh Config file on startup");
                            mdl.bCreateFreshConfig = true;
                        }
                        else if (mdl.sFalseStrings.Contains(createFresh))
                        {
                            Console.WriteLine("Not creating fresh Config file on startup");
                            mdl.bCreateFreshConfig = false;
                        }
                        else
                        {
                            Console.WriteLine("Argument [" + args[i] + "] contains invalid [Value].");
                        }

                        break;


                    default:
                        Console.WriteLine("Invalid Argument: [" + args[i] + "]");
                        break;

                }

            }
            Console.WriteLine("Arguments Processing Compleate");
        }

        /// <summary>Process the config file. Can also Initialise a fresh file if <see cref="mdl.bCreateFreshConfig"/>.</summary>
        /// <returns>TRUE if a valid config file, FALSE otherwise.</returns>
        private static bool ProcessConfigFile()
        {
            Console.WriteLine("Prosessing Config File....");

            mdl.sConfigFilePath = Path.Combine(mdl.sConfigFileDirectory, mdl.sConfigFileName);// Combine the dirctroy and file name to a valid path.

            if (mdl.bCreateFreshConfig)//If instructed to create a fresh config file...
            {
                InitialiseFreshConfigfile();// initialise a fresh one.
            }

            string[] sLinesFromFile;

            try
            {
                sLinesFromFile = File.ReadAllLines(mdl.sConfigFilePath);
            }
            catch (Exception e)
            {
                Console.Write("Exception when reading configuration file: " + e.Message);
                return false;
            }

            for (int i = 0; i < sLinesFromFile.Length; i++)
            {
                Console.WriteLine("Processing line " + i + ": " + sLinesFromFile[i]);
                if (!ProcessConfigLine(sLinesFromFile[i])) //If the line isnt valid, return false.
                    return false;

            }

            //ToDo: read in the config file and store values in model.
            //If a config line is invalid return false.

            Console.WriteLine("Config Processing Compleate");
            return true;
        }

        /// <summary>Process a line from the config file.</summary>
        /// <param name="lineFromFile">A line from the file.</param>
        /// <returns>True if a valid line, false otherwise.</returns>
        private static bool ProcessConfigLine(string lineFromFile)
        {
            lineFromFile = lineFromFile.Trim(); //Remove the spaces from the start and end of the line.

            if (lineFromFile[0] == mdl.cConfigFileCommentChar) //Check if the line is a comment.
            {
                Console.WriteLine("Commented line found - ignoring...");
                return true; //Return true, as a commented line is a valid line.
            }

            return true;
        }

        /// <summary>Initialise a fresh config file at <see cref="sConfigFilePath"/>.</summary>        
        private static void InitialiseFreshConfigfile()
        {
            Console.WriteLine("Creating Fresh Config File at " + mdl.sConfigFilePath);
            if (!Directory.Exists(mdl.sConfigFileDirectory))// If the directory does not exist...
            {
                Console.WriteLine("Config Directory Path does not exist: [" + mdl.sConfigFileDirectory + "]");
                Console.WriteLine("Creating Directory... ");
                Directory.CreateDirectory(mdl.sConfigFileDirectory);// ...Create it
            }
            using (StreamWriter sw = File.CreateText(mdl.sConfigFilePath))// Create a fresh config file template (will also overwrite if one already exists)
            {
                sw.WriteLine(mdl.cConfigFileCommentChar + " Any lines that start with '" + mdl.cConfigFileCommentChar + "' will be ignored");
                sw.WriteLine(mdl.cConfigFileCommentChar + " Please do not change the values in front of the '='.");
                sw.WriteLine(mdl.cConfigFileCommentChar + " Bot Setup:");
                sw.WriteLine("BOT_TOKEN =           Token Here");
                sw.WriteLine("# Database Setup:");
                sw.WriteLine("DATABASE_ADDRESS =    localhost");
                sw.WriteLine("DATABASE_NAME =       Name Here");
                sw.WriteLine("DATABASE_USERNAME =   Username here");
                sw.WriteLine("DATABASE_PASSWORD =   Password here");
            }
            Console.WriteLine("Fresh Config file created");
        }
    }
}