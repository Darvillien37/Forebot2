using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Forebot2.Model;

namespace Forebot2.Discord
{
    class Connection
    {       
        private DiscordSocketClient _client;
        private readonly DiscordSocketConfig _socketConfig;
        private readonly BotModel _mdl = BotModel.Instance;

 
        /// <summary>Construct a new discord connection.</summary>        
        public Connection()
        {
            _socketConfig = new DiscordSocketConfig()
            {
                LogLevel = (LogSeverity)_mdl.Bot.Severity,
            };

            _client = new DiscordSocketClient(_socketConfig);
            _client.Log += DiscordLogger.Log;
        }

      
       

        internal async Task ConnectAsync()
        {
            _client = new DiscordSocketClient();
        }


    }
}
