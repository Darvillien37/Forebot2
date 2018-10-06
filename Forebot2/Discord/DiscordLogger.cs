using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Forebot2.Discord.Entities;

namespace Forebot2.Discord
{
    class DiscordLogger
    {
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
                ApplicationLogger.Log(message, "{Discord}." + arg.Source, Mappings.DiscSevToAppSev[arg.Severity]);
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
