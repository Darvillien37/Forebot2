using System;
using System.Collections.Generic;
using System.Text;

namespace Forebot2
{
    /// <summary>Specifies the severity of a log message, at the application/program level.</summary>
    public enum LOG_SEVERITY
    {
        /// <summary>Any error which is forcing an application shutdown.</summary>
        CRITICAL = 0,
        /// <summary>An error which is fatal to an operation, but not the application.</summary>
        /// <remarks>EG. Can't open required file, missing data, incorrect connection strings, missing services, ect...</remarks>
        ERROR = 1,
        /// <summary>Anything which can cause Application oddities, but can automatically recover.</summary>
        /// <remarks>EG. Switching to backup server, retrying operation, missing secondary data, ect...</remarks>
        WARNING = 2,
        /// <summary>A generic information message.</summary>
        /// <remarks>EG. Start/stop, config assumptions, ect... Might not always be useful but usually the user wouldn't care.</remarks>
        INFO = 3,
        /// <summary>A message containing slightly more level of detail than <see cref="INFO"/>.</summary>
        VERBOSE = 4,
        /// <summary>Information that is diagnostically helpful when debugging.</summary>
        DEBUG = 5,
    }

    /// <summary>Represents a standard logging functions of the application,
    /// all implemented modules should use <see cref="Log(string, string, LOG_SEVERITY)"/> when wanting to Log issues to the console.</summary>
    public class ApplicationLogger
    {
        /// <summary>Log the Message to the logger.</summary>
        /// <param name="msg">The message contents.</param>
        /// <param name="source">The origin of the message.</param>
        /// <param name="severity">The severity of the message.<param>
        public static void Log(string msg, string source, LOG_SEVERITY severity)
        {
            var c = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;

            switch (severity)
            {
                case LOG_SEVERITY.CRITICAL:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case LOG_SEVERITY.ERROR:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;

                case LOG_SEVERITY.WARNING:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                default:
                case LOG_SEVERITY.INFO:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                case LOG_SEVERITY.VERBOSE:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;

                case LOG_SEVERITY.DEBUG:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;

            }

            string LogString = $"{DateTime.Now} [{severity,8}] {source}: {msg}";

            Console.WriteLine(LogString);
            Console.ForegroundColor = c;
        }

    }
}
