using System;
using System.Diagnostics;
using System.Threading;

namespace Forebot2
{
    /// <summary>The main program class for this application.</summary>
    class Program
    {
        /// <summary>The main starting point of any application.</summary>
        /// <remarks>If you had to read this to figure out what main is.....git gud scrub.</remarks>
        /// <param name="args">Arguments passed in to the application, from the command line, or shortcut.</param>
        static void Main(string[] args)
        {
            //Check if this applicaiton is already running...
            if (IsApplicationAlreadyRunning())
            {//...if it is, shut it down.
                Console.WriteLine("Only one instance of " + Process.GetCurrentProcess().ProcessName + " Can be run a a time.");
                Console.WriteLine("Shutting down this instance ... Goodbye!!");
                return; //returning in main exits the applicaiton.
            }


            Console.WriteLine("Hello World!");
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
    }
}