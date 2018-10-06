using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Forebot2.Discord
{
    class Connection
    {
        /// <summary>The Client, AKA the bot user.</summary>
        private readonly DiscordSocketClient _client;

        /// <summary>Construct a new discord connection.</summary>        
        public Connection(DiscordSocketConfig config)
        {
            _client = new DiscordSocketClient(config);
        }



        /// <summary>Connect to the discord service.</summary>
        /// <param name="token">The token to use to connect.</param>        
        internal async Task ConnectAsync(string token)
        {
            _client.Log += DiscordLogger.Log;

            try
            {
                await _client.LoginAsync(TokenType.Bot, token);
            }
            catch (Exception ex)
            {
                ApplicationLogger.Log(
                    "Exception When Logging In: " + ex.Message,
                    "Discord.ConnectAsync",
                    LOG_SEVERITY.ERROR);
                return;
            }
            await _client.StartAsync();

            await Task.Delay(-1);
        }
    }
}
