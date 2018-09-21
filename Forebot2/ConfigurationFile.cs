﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Forebot2.Model;

namespace Forebot2
{
    public class ConfigurationFile
    {
        /// <summary>Character used to represent a commented line in the config file.</summary>
        private const char COMMENT_CHAR = '#';
        /// <summary>The default directory of the config file.</summary>
        private const string DEFAULT_DIRECTORY = @".\";

        /// <summary>GET the name of the config file for the bot.</summary>
        public static string Name { get; } = "Forebot.cfg";
        /// <summary>GET/SET the directory to find the config file.</summary>
        public static string Location { get; set; } = DEFAULT_DIRECTORY;
        /// <summary>GET the combination of the directory and name of the config file.</summary>
        public static string FullPath { get { return Path.Combine(Location, Name); } }

        /// <summary>Refference to the model.</summary>
        private static readonly BotModel mdl = BotModel.Instance;

        /// <summary>Process the config file. Can also Initialise a fresh file if <see cref="mdl.bCreateFreshConfig"/>.</summary>
        /// <returns>TRUE if a valid config file, FALSE otherwise.</returns>
        public static bool ProcessConfigFile()
        {
            Console.WriteLine("Starting: Processing Config File......");
            Console.WriteLine("\tLocation: " + FullPath);

            if (!File.Exists(FullPath))//If the config file does not exist...
            {
                Console.WriteLine("\tConfig file does not exist");
                InitialiseFreshConfigfile(); //Initialise a fresh one.
            }

            string[] sLinesFromFile;
            try
            {
                sLinesFromFile = File.ReadAllLines(FullPath);
            }
            catch (Exception e)
            {
                Console.Write("Exception when reading configuration file: " + e.Message);
                return false;
            }

            for (int i = 0; i < sLinesFromFile.Length; i++)
            {
                Console.WriteLine("\tProcessing line [" + i + "]: " + sLinesFromFile[i]);
                if (!ProcessLine(sLinesFromFile[i])) //If the line isn't valid, return false.
                {
                    Console.WriteLine("\tLine Invalid");
                    return false;
                }

            }

            Console.WriteLine("Complated: Processing Config File");
            return true;//if we have reached this point, then the config file is valid.
        }

        /// <summary>Process a line from the config file.</summary>
        /// <param name="lineFromFile">A line from the file.</param>
        /// <returns>True if a valid line, false otherwise.</returns>
        private static bool ProcessLine(string lineFromFile)
        {
            if (lineFromFile == null)
            {
                throw new ArgumentNullException("lineFromFile", "Should not be null");
            }

            lineFromFile = lineFromFile.Trim(); //Remove the spaces from the start and end of the line.

            if (lineFromFile.Length == 0)//if the line is blank, "".
            {
                Console.WriteLine("\t\tBlank line found - ignoring...");
                return true; //Return true, as a blank line is valid.
            }

            if (lineFromFile[0] == COMMENT_CHAR) //Check if the line is a comment.
            {
                Console.WriteLine("\t\tCommented line found - ignoring...");
                return true; //Return true, as a commented line is a valid line.
            }

            //A valid line is in the format of 'Key=Value' or 'key = value', or '   key  =    value    '.
            string[] splitLine = lineFromFile.Split('='); //Split the line around an '='.

            if (splitLine.Length != 2)
            {
                Console.WriteLine("\t\tSplit length not valid,required: 2,  actual: " + splitLine.Length);
                return false;//If the split length is not 2, then this line is invalid.
            }

            switch (lineFromFile)
            {
                case "test":
                    break;
            }

            //ToDo: read in the config file and store values in model.
            //If a config line is invalid return false.
            return true;
        }

        /// <summary>Initialise a fresh config file at <see cref="sConfigFilePath"/>.</summary>        
        private static void InitialiseFreshConfigfile()
        {
            Console.WriteLine("\tCreating Fresh Config File at " + FullPath);
            if (!Directory.Exists(Location))// If the directory does not exist...
            {
                Console.WriteLine("\tConfig Directory Path does not exist: [" + Location + "]");
                Console.WriteLine("\tCreating Directory... ");
                Directory.CreateDirectory(Location);// ...Create it
            }
            using (StreamWriter sw = File.CreateText(FullPath))// Create a fresh config file template (will also overwrite if one already exists)
            {
                sw.WriteLine(COMMENT_CHAR + " Any lines that start with '" + COMMENT_CHAR + "' will be ignored");
                sw.WriteLine(COMMENT_CHAR + " Please do not change the values in front of the '='.");
                sw.WriteLine("");
                sw.WriteLine(COMMENT_CHAR + " Bot Set-up:");
                sw.WriteLine("BOT_TOKEN =           Token Here");
                sw.WriteLine("");
                sw.WriteLine("# Database Set-up:");
                sw.WriteLine("DATABASE_ADDRESS =    localhost");
                sw.WriteLine("DATABASE_NAME =       Name Here");
                sw.WriteLine("DATABASE_USERNAME =   Username here");
                sw.WriteLine("DATABASE_PASSWORD =   Password here");
            }
            Console.WriteLine("\tFresh Config file created");
        }
    }
}
