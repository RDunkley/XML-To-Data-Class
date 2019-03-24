//********************************************************************************************************************************
// Filename:    CommandSettings.cs
// Owner:       Richard Dunkley
// Description: Class used to determine what the console arguments are. This leverages ConsoleArgs class to parse the settings.
//********************************************************************************************************************************
// Copyright © Richard Dunkley 2019
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the
// License. You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0  Unless required by applicable
// law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and
// limitations under the License.
//********************************************************************************************************************************
using System;
using System.IO;
using System.Security;

namespace XMLToDataClass
{
	[Usage("Usage: XMLToDataClass.exe [OPTIONS] [-e <settingsfile.x2dsettings>] [-n <namespace>] [-p <projectname>] [-s <solutionname>] [-c <configfile>] -i <xmlfilepath> -o <outputfolder>")]
	public class CommandSettings
	{
		[Argument('e', "Pulls in the application global settings from a previously saved settings file (x2dsettings).", Word ="setting")]
		public string SettingsFile { get; set; }

		[Argument('i', "XML File used to generate code from.", Word = "input")]
		public string XMLFilePath { get; set; }

		[Argument('o', "Path to the output folder to save the generated code in.", Word = "output")]
		public string OutputFolder { get; set; }

		[Argument('g', "Force running the GUI before generating the code. To run completely from command line the XML file must be specified (-i) otherwise the GUI is launched regardless of this flag.", Word = "gui")]
		public bool GUI { get; set; }

		[Argument('h', "Preserve Hierarchy. XML elements with the same name with different parents are considered different elements. Otherwise they are considered the same.", Word = "hierarchy")]
		public bool PreserveHierarchy { get; set; }

		[Argument('c', "Path to the configuration file that contains how the values in the XML should be formatted.", Word ="config")]
		public string ConfigPath { get; set; }

		public void ValidateSettings()
		{
			// Validate the settings file if provided.
			if(SettingsFile != null)
			{
				try
				{
					SettingsFile = Path.GetFullPath(SettingsFile);
				}
				catch(Exception e)
				{
					if(e is ArgumentException || e is SecurityException || e is NotSupportedException || e is PathTooLongException)
						throw new InvalidOperationException(string.Format("The -e option file path specified ({0}) was not a valid file path ({1}).", SettingsFile, e.Message), e);
					throw;
				}

				if(!File.Exists(SettingsFile))
					throw new InvalidOperationException(string.Format("The -e option file path specified ({0}) does not exist.", SettingsFile));
			}

			// Validate the XML file if provided.
			if (XMLFilePath != null)
			{
				try
				{
					XMLFilePath = Path.GetFullPath(XMLFilePath);
				}
				catch (Exception e)
				{
					if (e is ArgumentException || e is SecurityException || e is NotSupportedException || e is PathTooLongException)
						throw new InvalidOperationException(string.Format("The -i option file path specified ({0}) was not a valid file path ({1}).", XMLFilePath, e.Message), e);
					throw;
				}

				if (!File.Exists(XMLFilePath))
					throw new InvalidOperationException(string.Format("The -i option file path specified ({0}) does not exist.", XMLFilePath));
			}

			// Validate the output folder if provided.
			if (OutputFolder != null)
			{
				try
				{
					OutputFolder = Path.GetFullPath(OutputFolder);
				}
				catch (Exception e)
				{
					if (e is ArgumentException || e is SecurityException || e is NotSupportedException || e is PathTooLongException)
						throw new InvalidOperationException(string.Format("The -o option folder path specified ({0}) was not a valid folder path ({1}).", OutputFolder, e.Message), e);
					throw;
				}
			}

			// Validate the configuration file if provided.
			if (ConfigPath != null)
			{
				try
				{
					ConfigPath = Path.GetFullPath(ConfigPath);
				}
				catch (Exception e)
				{
					if (e is ArgumentException || e is SecurityException || e is NotSupportedException || e is PathTooLongException)
						throw new InvalidOperationException(string.Format("The -c option file path specified ({0}) was not a valid file path ({1}).", ConfigPath, e.Message), e);
					throw;
				}

				if (!File.Exists(ConfigPath))
					throw new InvalidOperationException(string.Format("The -c option file path specified ({0}) does not exist.", ConfigPath));
			}
		}
	}
}
