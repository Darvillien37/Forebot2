using Forebot2.Model;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Forebot2
{
    /// <summary>The main program class for this application.</summary>
    class Program
    {
        /// <summary>Reference to the Model Root of this application.</summary>
        private static readonly BotModel mdl = BotModel.Instance;        

        /// <summary>Get the Build Version of the program.</summary>
        public static string BuildVersion { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }
        /// <summary>Get the process name of this application.</summary>
        public static string ProcessName { get { return Process.GetCurrentProcess().ProcessName; } }


        /// <summary>The main starting point of any application.</summary>
        /// <remarks>If you had to read this to figure out what main is.....git gud scrub.</remarks>
        /// <param name="args">Arguments passed in to the application, from the command line, or shortcut.</param>
        static void Main(string[] args)
        {             
            PrintApplicationInfo();//Print the application info.

            //Check if this application is already running...
            if (IsApplicationAlreadyRunning())
            {//...if it is, shut this instance down.
                ApplicationLogger.Log(
                    "Only one instance of " + ProcessName + " Can be run a a time.",
                    "MAIN",
                    LOG_SEVERITY.ERROR
                    );
                ApplicationLogger.Log(
                    "Shutting down this instance ... Goodbye!!",
                    "MAIN",
                    LOG_SEVERITY.CRITICAL
                    );
                return; //returning in main exits the application.
            }

            //Initialising:
            ProcessArgs(args);

            if (!ConfigurationFile.ProcessConfigFile())//Will return false if a config line is invalid.
            {
                ApplicationLogger.Log("Configuration file is invalid, shutting down", "MAIN", LOG_SEVERITY.CRITICAL);
                return;
            }



            ApplicationLogger.Log("Test", "Testing", LOG_SEVERITY.DEBUG);
            ApplicationLogger.Log("Test", "Testing", LOG_SEVERITY.VERBOSE);
            ApplicationLogger.Log("Test", "Testing", LOG_SEVERITY.INFO);
            ApplicationLogger.Log("Test", "Testing", LOG_SEVERITY.WARNING);
            ApplicationLogger.Log("Test", "Testing", LOG_SEVERITY.ERROR);
            ApplicationLogger.Log("Test", "Testing", LOG_SEVERITY.CRITICAL);

            Console.ReadKey();

        }

        /// <summary>Print the application name and version number.</summary>
        static private void PrintApplicationInfo()
        {
            AssemblyName aName = Assembly.GetExecutingAssembly().GetName();
            ApplicationLogger.Log(
                "Running: " + aName.Name + "  V" + aName.Version.ToString(),
                "Initialisation",
                LOG_SEVERITY.INFO);
        }

        /// <summary>Check if this application is already running.</summary>
        /// <returns>TRUE if application is already running, FALSE otherwise</returns>
        private static bool IsApplicationAlreadyRunning()
        {
            string sProcessName = Process.GetCurrentProcess().ProcessName; //Get the current process name
            if (Process.GetProcessesByName(sProcessName).Length > 1) //Get a list of all processes running on this machine, that contain this processes name.
                return true;//If the list is bigger than 1, the 1 being this current process, then the application is already running.
            else
                return false;//Otherwise it isn't.
        }

        /// <summary>Process the arguments passed in to the application.</summary>
        /// <param name="args">Arguments passed in to the application.</param>
        private static void ProcessArgs(string[] args)
        {
            ApplicationLogger.Log("Starting: Processing Arguments.....", "Args", LOG_SEVERITY.INFO);
            if (args == null)//if the array is null, throw an exception.
            {
                throw new ArgumentNullException("args", "Parameter contains null array.");
            }

            if (args.Length == 0)
            {
                ApplicationLogger.Log("No Arguments, setting to config path to '" + ConfigurationFile.FullPath + "'.", "Args", LOG_SEVERITY.WARNING);
                //file path is already defaulted to local directory.
            }
            else
            {
                string arg = args[0];
                ApplicationLogger.Log("Processing Argument 1: " + arg + ".", "Args", LOG_SEVERITY.VERBOSE);
                ApplicationLogger.Log("Setting to config directory to '" + arg + "'.", "Args", LOG_SEVERITY.VERBOSE);
                ConfigurationFile.Location = arg;
                ApplicationLogger.Log("Config path set to '" + ConfigurationFile.FullPath + "'.", "Args", LOG_SEVERITY.VERBOSE);
            }
            ApplicationLogger.Log("Completed: Arguments Processing", "Args", LOG_SEVERITY.INFO);
        }

    }
}
