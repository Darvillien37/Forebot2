using System;
using System.Collections.Generic;
using System.Text;

namespace Forebot2.Model
{
    /// <summary>Singleton root node of the runtime model/database used by the application. 
    /// This class cannot be inherited.</summary>
    public sealed class BotModel
    {
        /// <summary>Get the instance of the model.</summary>
        public static BotModel Instance { get; } = new BotModel();

      

        /// <summary>Get the data relating to the bot.</summary>
        public BotData Bot { get; } = new BotData();
        /// <summary>Get the data relating to the database, not the data IN the database.</summary>
        public DatabaseData Database { get; } = new DatabaseData();

        /// <summary>Private constructor, part of singleton design pattern.</summary>
        private BotModel()
        { }
        

        /// <summary>An array of strings representing acceptable yes/true strings.</summary>
        public readonly string[] sTrueStrings = { "Y", "YES", "T", "TRUE" };
        /// <summary>An array of strings representing acceptable no/false strings.</summary>
        public readonly string[] sFalseStrings = { "N", "NO", "F", "FALSE" };
        
    }

    

    /// <summary>Data relating to the bot itself.</summary>
    public class BotData
    {
        /// <summary>The token of the bot.</summary>
        private string _sToken = "N/A";
        /// <summary>A flag used so you can only set the token once.</summary>
        private bool _bCanChangeToken = true;
        /// <summary>GET the bot token, or set it (but you can only set it once, on initialisation).</summary>
        public string Token
        {
            get { return _sToken; }
            set
            {
                if (_bCanChangeToken) //If we are able to set the token, then do it once.
                {
                    _sToken = value; //Set the token.
                    _bCanChangeToken = false; //And set the flag to not be able to set it again.
                }
            }
        }

        /// <summary>The severity level of the discord bot.</summary>
        public LOG_SEVERITY Severity { get; set; } = LOG_SEVERITY.DEBUG;

        public BotData()
        { }
    }

    /// <summary>Data relating to the Database itself. Not the data in the database.</summary>
    public class DatabaseData
    {
        /// <summary>The Address of the Database.</summary>
        public string Address { get; set; } = "localhost";
        /// <summary>The Name of the database.</summary>
        public string Name { get; set; } = "NotSet";
        /// <summary>The Username to enter the database.</summary>
        public string Username { get; set; } = "NotSet";
        /// <summary>The Password to enter the database.</summary>
        public string Password { get; set; } = "NotSet";
    }
}
