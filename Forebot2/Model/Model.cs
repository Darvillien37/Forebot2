﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Forebot2.Model
{
    /// <summary>Singleton base node of the runtime model/database used by the applicaion. 
    /// This class cannot be inherited.</summary>
    public sealed class BotModel
    {        
        /// <summary>Get the instance of the model.</summary>
        public static BotModel Instance { get; } = new BotModel();

        /// <summary>Private constructor, part of singleton design pattern.</summary>
        private BotModel()
        { }

        public string sConfigFileDirectory = @".\";
        public readonly string sConfigFileName = "Forebot.cfg";
        public string sConfigFilePath;
        public bool bCreateFreshConfig = false;

        public string sBotToken = "N/A";


        public readonly string[] sTrueStrings = { "Y", "YES", "T", "TRUE" };
        public readonly string[] sFalseStrings = { "N", "NO", "F", "FALSE" };

        public string sTestString = "Test String in the model";


    }
}
