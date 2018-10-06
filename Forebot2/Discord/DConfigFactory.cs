using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Forebot2.Discord.Entities;

namespace Forebot2.Discord
{
    class DConfigFactory
    {
        /// <summary>Get a new discord socket config, with no modifications.</summary>
        /// <returns>new socket config with no modifications.</returns>
        public static DiscordSocketConfig GetNew()
        {
            return new DiscordSocketConfig();
        }

        /// <summary>get the default config, with log level = verbose....</summary>
        /// <returns>Get the default socket config.</returns>
        public static DiscordSocketConfig Default()
        {
            return new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Verbose,
            };
        }
        
        /// <summary>Generate a custom config.</summary>
        /// <param name="logSeverity">The logging level of the connection, the higher the more verbosity, Max is <see cref="LogSeverity.Debug"/>.</param>
        /// <returns>A custom socket config.</returns>
        public static DiscordSocketConfig Generate(uint logSeverity)
        {
            uint MAX_SEVERITY = (uint)LogSeverity.Debug;
            if (logSeverity > MAX_SEVERITY)
            {
                throw new ArgumentOutOfRangeException("logSeverity", "too large, cannot be greater than " + MAX_SEVERITY);
            }
            LogSeverity ls = (LogSeverity)logSeverity;

            return new DiscordSocketConfig()
            {
                LogLevel = ls,
            };
        }

        /// <summary>Generate a custom config.</summary>
        /// <param name="logSeverity">The logging level of the connection.</param>
        /// <returns>A custom socket config.</returns>
        public static DiscordSocketConfig Generate(LOG_SEVERITY severity)
        {
            LogSeverity ls = Mappings.AppSevToDiscSev[severity];            
            return new DiscordSocketConfig()
            {
                LogLevel = ls,
            };
        }
    }
}
