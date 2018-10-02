using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Forebot2.Discord
{
    class DiscordLogger
    {
        /// <summary>
        /// A dictionary to map the discord logging severity to the application logging severity
        /// </summary>
        private static readonly Dictionary<LogSeverity, LOG_SEVERITY> dicDiscSevToAppSev = new Dictionary<LogSeverity, LOG_SEVERITY>
        {
            { LogSeverity.Debug, LOG_SEVERITY.DEBUG },
            { LogSeverity.Verbose, LOG_SEVERITY.VERBOSE },
            { LogSeverity.Info, LOG_SEVERITY.INFO},
            { LogSeverity.Warning, LOG_SEVERITY.WARNING },
            { LogSeverity.Error, LOG_SEVERITY.ERROR },
            { LogSeverity.Critical, LOG_SEVERITY.CRITICAL },
        };

        /// <summary>Log a message from the discord connection.</summary>
        /// <param name="arg">A discord formatted Log Message.</param>
        /// <returns>The result of a Task.</returns>
        internal static Task Log(LogMessage arg)
        {
            string message = arg.Message;//Get the message.
            if (arg.Exception != null)//If there was an exception....
                message += " - With Exception: " + arg.Exception; //...then add the exception message to the whole message.
            try
            {
                ApplicationLogger.Log(message, "Discord." + arg.Source, dicDiscSevToAppSev[arg.Severity]);
            }
            catch (ArgumentNullException ANE)//from dicDiscSevToAppSev[arg.Severity]
            {
                ApplicationLogger.Log("Log Severity is null - Exception " + ANE.Message + "\tOriginal Message: " + message, "Discord.Log", LOG_SEVERITY.WARNING);
            }
            catch (KeyNotFoundException KNFE)//dicDiscSevToAppSev[arg.Severity]
            {
                ApplicationLogger.Log("Log Severity Not Mapped - Exception: " + KNFE.Message + "\tOriginal Message: " + message, "Discord.Log", LOG_SEVERITY.WARNING);
            }

            return Task.CompletedTask;//return a completed task.
        }
    }
}
