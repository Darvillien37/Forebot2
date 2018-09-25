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

     

        /// <summary>The main starting point of any application.</summary>
        /// <remarks>If you had to read this to figure out what main is.....git gud scrub.</remarks>
        /// <param name="args">Arguments passed in to the application, from the command line, or shortcut.</param>
        static void Main(string[] args)
        {
            PrintApplicationInfo();//Print the application info.

            //Check if this application is already running...
            if (IsApplicationAlreadyRunning())
            {//...if it is, shut this instance down.
                Console.WriteLine("Only one instance of " + Process.GetCurrentProcess().ProcessName + " Can be run a a time.");
                Console.WriteLine("Shutting down this instance ... Goodbye!!");
                return; //returning in main exits the application.
            }

            //Initialising:
            ProcessArgs(args);
            Console.WriteLine();

            if (!ConfigurationFile.ProcessConfigFile())//Will return false if a config line is invalid.
            {
                Console.WriteLine("WARNING - Configuration file is invalid, shutting down.");
                Console.ReadKey();
                return;
            }



            Console.WriteLine("Hello World!");

            Console.ReadKey();

        }

        /// <summary>Print the application name and version number.</summary>
        static private void PrintApplicationInfo()
        {
            AssemblyName aName = Assembly.GetExecutingAssembly().GetName();
            Console.WriteLine("Running: " + aName.Name + "  V" + aName.Version.ToString());
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
            Console.WriteLine("Starting: Processing Arguments.....");
            if (args == null)//if the array is null, throw an exception.
            {
                throw new ArgumentNullException("args", "Parameter contains null array.");
            }

            if (args.Length == 0)
            {
                Console.WriteLine("\tNo Arguments, setting to config path to '" + ConfigurationFile.FullPath + "'.");
                //file path is already defaulted to local directory.
            }
            else
            {
                string arg = args[0];
                Console.WriteLine("\tProcessing Argument 1: " + arg + ".");
                Console.WriteLine("\tSetting to config directory to '" + arg + "'.");
                ConfigurationFile.Location = arg;
                Console.WriteLine("\tConfig path set to '" + ConfigurationFile.FullPath + "'.");
            }
            Console.WriteLine("Complated: Arguments Processing");
        }

    }
}