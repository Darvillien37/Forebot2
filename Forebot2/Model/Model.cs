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

        /// <summary>Get the data relating about the config file.</summary>
        public  ConfigFileData ConfigFile { get; } = new ConfigFileData();

        /// <summary>Get the data relating to the bot.</summary>
        public BotData Bot { get; } = new BotData();

        /// <summary>Private constructor, part of singleton design pattern.</summary>
        private BotModel()
        { }



        /// <summary>An array of strings representing acceptable yes/true strings.</summary>
        public readonly string[] sTrueStrings = { "Y", "YES", "T", "TRUE" };
        /// <summary>An array of strings representing acceptable no/false strings.</summary>
        public readonly string[] sFalseStrings = { "N", "NO", "F", "FALSE" };



    }

    /// <summary>Data relating to the config file, not necessarily the data IN the file, but about it.</summary>
    public class ConfigFileData
    {
        /// <summary>Character used to represent a commented line in the config file.</summary>
        public readonly char cCommentChar = '#';
        /// <summary>The name of the config file for the bot.</summary>
        public readonly string sName = "Forebot.cfg";

        /// <summary>The directory path to find the config file. 
        /// Defaulted to '.\' (where the application is running from).</summary>
        public string sDirectory = @".\";
        /// <summary>The combination of the directory and name of the config file.</summary>
        public string sPath = "N/A";
        /// <summary>Used to determine to create a fresh configuration file (in <see cref="sDirectory"/>) on application start.</summary>
        public bool bCreateFreshConfig = false; //Default to false.

        public ConfigFileData()
        {
        }
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

        public BotData()
        { }


    }
}