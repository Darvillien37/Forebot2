using Forebot2.Discord;
using Forebot2.Model;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

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
        static async Task Main(string[] args)
        {
            PrintApplicationInfo();//Print the application info to the log

            //Check if this application is already running...
            if (ProcessRunningCheck())
            {//...if it is, shut this instance down.
                ApplicationLogger.Log("Only one instance of " + ProcessName + " Can be run a a time.",
                "ProcessRunningCheck",
                LOG_SEVERITY.ERROR);
                ApplicationLogger.Log("Shutting down this instance ... Goodbye!!",
                    "ProcessRunningCheck",
                    LOG_SEVERITY.CRITICAL);
                Console.ReadKey();
                return; //returning in main exits the application.
            }

            //Initialising:
            //Process the args passed into the program
            ProcessArgs(args);
            //Now process the config file
            if (!ConfigurationFile.ProcessConfigFile())//Will return false if a config line is invalid.
            {
                ApplicationLogger.Log("Configuration file is invalid, Shutting down", "MAIN", LOG_SEVERITY.WARNING);
                Console.ReadKey();
                return;
            }

            Connection DiscordConnection = new Connection(DConfigFactory.Generate(mdl.Bot.Severity));
            await DiscordConnection.ConnectAsync(mdl.Bot.Token);

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
        private static bool ProcessRunningCheck()
        {
            string sProcessName = Process.GetCurrentProcess().ProcessName; //Get the current process name

            //Get a list of all processes running on this machine, that contain this processes name.
            //If the list is bigger than 1, the 1 being this current process, then the application is already running.
            if (Process.GetProcessesByName(sProcessName).Length > 1)
                return true;
            else
                return false;
        }

        /// <summary>Process the arguments passed in to the application.</summary>
        /// <param name="args">Arguments passed in to the application.</param>
        private static void ProcessArgs(string[] args)
        {
            const string LOG_SOURCE = "Args";
            ApplicationLogger.Log("Starting: Processing Arguments And Config.....", LOG_SOURCE, LOG_SEVERITY.INFO);

            if (args == null)//if the array is null, set it as an empty array.
                args = new string[] { };

            //If there are no args, then set the default config directory.
            if (args.Length == 0)
                ApplicationLogger.Log("No Arguments, config file path remains: '" + ConfigurationFile.FullPath + "'.", LOG_SOURCE, LOG_SEVERITY.WARNING);
            //file path is already defaulted to local directory.            
            else
            {
                string arg = args[0];//The first arg should be the path to the config file               
                ApplicationLogger.Log("Setting to config directory to '" + arg + "'.", LOG_SOURCE, LOG_SEVERITY.VERBOSE);
                ConfigurationFile.Location = arg;
                ApplicationLogger.Log("Config path set to '" + ConfigurationFile.FullPath + "'.", LOG_SOURCE, LOG_SEVERITY.VERBOSE);
            }

            ApplicationLogger.Log("Completed: Arguments And Config Processing", LOG_SOURCE, LOG_SEVERITY.INFO);
        }

    }
}
