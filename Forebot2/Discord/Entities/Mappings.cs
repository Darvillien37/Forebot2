using Discord;
using System.Collections.Generic;

namespace Forebot2.Discord.Entities
{
    internal class Mappings
    {
        /// <summary>A dictionary to map the discord logging severity to the application logging severity</summary>
        internal static readonly Dictionary<LogSeverity, LOG_SEVERITY> DiscSevToAppSev = new Dictionary<LogSeverity, LOG_SEVERITY>
        {
            { LogSeverity.Debug, LOG_SEVERITY.DEBUG },
            { LogSeverity.Verbose, LOG_SEVERITY.VERBOSE },
            { LogSeverity.Info, LOG_SEVERITY.INFO},
            { LogSeverity.Warning, LOG_SEVERITY.WARNING },
            { LogSeverity.Error, LOG_SEVERITY.ERROR },
            { LogSeverity.Critical, LOG_SEVERITY.CRITICAL },
        };

        /// <summary>A dictionary to map the discord logging severity to the application logging severity</summary>
        internal static readonly Dictionary<LOG_SEVERITY, LogSeverity> AppSevToDiscSev = new Dictionary<LOG_SEVERITY, LogSeverity>
        {
            { LOG_SEVERITY.DEBUG, LogSeverity.Debug },
            { LOG_SEVERITY.VERBOSE, LogSeverity.Verbose },
            { LOG_SEVERITY.INFO, LogSeverity.Info },
            { LOG_SEVERITY.WARNING,LogSeverity.Warning },
            { LOG_SEVERITY.ERROR, LogSeverity.Error },
            { LOG_SEVERITY.CRITICAL, LogSeverity.Critical },
        };

    }
}
