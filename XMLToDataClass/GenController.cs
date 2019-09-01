//********************************************************************************************************************************
// Filename:    GenController.cs
// Owner:       Richard Dunkley
//********************************************************************************************************************************
// Copyright © Richard Dunkley 2019
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the
// License. You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0  Unless required by applicable
// law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and
// limitations under the License.
//********************************************************************************************************************************
using CSCodeGen.Parse.SettingsFile;
using System;
using System.IO;
using System.Security;
using System.Xml;
using XMLToDataClass.Data;

namespace XMLToDataClass
{
	public class GenController
	{
		/// <summary>
		///   Gets the information loaded from the XML file.
		/// </summary>
		public XMLInfo Info { get; private set; }

		public string XMLFilePath { get; private set; }

		public string OutputFolder { get; set; }

		public string NameSpace { get; set; }

		public bool GenSolution { get; set; }

		public bool GenProject { get; set; }

		public string ProjectName { get; set; }

		public string SolutionName { get; set; }

		public GenController(CommandSettings settings)
		{
			// Load settings from the stored configuration settings.
			XMLFilePath = Properties.Settings.Default.XMLFileLocation;
			NameSpace = Properties.Settings.Default.Namespace;
			GenProject = Properties.Settings.Default.Project;
			ProjectName = Properties.Settings.Default.ProjectName;
			GenSolution = Properties.Settings.Default.Solution;
			SolutionName = Properties.Settings.Default.SolutionName;
			if (Properties.Settings.Default.OutputFolder.Length != 0)
				OutputFolder = Properties.Settings.Default.OutputFolder;
			else
				OutputFolder = Environment.CurrentDirectory;
			if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.CSCodeGenSettings))
			{
				using (StringReader sr = new StringReader(Properties.Settings.Default.CSCodeGenSettings))
				{
					Settings sf = new Settings(sr);
					CSCodeGen.DefaultValues.ImportValues(sf);
				}
			}

			// Override with any settings from the command line.
			if (settings.XMLFilePath != null)
				Load(settings.XMLFilePath, settings.PreserveHierarchy);
			if (settings.ConfigPath != null)
			{
				LoadConfig(settings.ConfigPath);
			}
			if (settings.OutputFolder != null)
				OutputFolder = settings.OutputFolder;
			if(settings.SettingsFile != null)
				CSCodeGen.DefaultValues.ImportValues(settings.SettingsFile);
		}

		public void UnLoad()
		{
			if (Info != null)
				Info = null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="preserveHierarchy"></param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidDataException"></exception>
		public void Load(string filePath, bool preserveHierarchy)
		{
			if (filePath == null)
				throw new ArgumentNullException("filePath");

			try
			{
				filePath = Path.GetFullPath(filePath);
				Info = new XMLInfo(filePath, preserveHierarchy);
			}
			catch (Exception e)
			{
				if(e is SecurityException || e is NotSupportedException || e is PathTooLongException)
					throw new ArgumentException(string.Format("the file path specified was not valid: {0}", e.Message), e);
				throw;
			}

			XMLFilePath = filePath;
		}

		public void LoadConfig(string filePath)
		{
			if (filePath == null)
				throw new ArgumentNullException("filePath");

			Info.Load(filePath, out string outputFolder, out string nameSpace, out bool? genProject, out string projectName, out bool? genSolution, out string solutionName);

			if (!string.IsNullOrEmpty(outputFolder))
				OutputFolder = outputFolder;
			if (!string.IsNullOrEmpty(nameSpace))
				NameSpace = nameSpace;
			if (genProject.HasValue)
				GenProject = genProject.Value;
			if(!string.IsNullOrEmpty(projectName))
				ProjectName = projectName;
			if (genSolution.HasValue)
				GenSolution = genSolution.Value;
			if (!string.IsNullOrEmpty(solutionName))
				SolutionName = solutionName;
		}

		public void SaveConfig(string filePath)
		{
			Info.Save(filePath, OutputFolder, NameSpace, GenProject, ProjectName, GenSolution, SolutionName);
		}

		public void Validate()
		{
			if (string.IsNullOrWhiteSpace(OutputFolder))
				OutputFolder = Environment.CurrentDirectory;

			try
			{
				OutputFolder = Path.GetFullPath(OutputFolder);
			}
			catch (Exception ex)
			{
				if (ex is ArgumentException || ex is SecurityException || ex is NotSupportedException || ex is PathTooLongException)
					throw new InvalidOperationException("Unable to obtain the path to the code folder: " + ex.Message);
				throw;
			}

			if (string.IsNullOrEmpty(NameSpace))
				throw new InvalidOperationException("The Namespace cannot be empty");

			if (GenProject && string.IsNullOrWhiteSpace(ProjectName))
				throw new InvalidOperationException("If project creation is enabled, then the project name cannot be empty or whitespace.");
			if(GenSolution && string.IsNullOrWhiteSpace(SolutionName))
				throw new InvalidOperationException("If solution creation is enabled, then the solution name cannot be empty or whitespace.");
		}

		public void Process()
		{
			Validate();

			// Load the latest settings.
			if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.CSCodeGenSettings))
			{
				using (StringReader sr = new StringReader(Properties.Settings.Default.CSCodeGenSettings))
				{
					Settings sf = new Settings(sr);
					CSCodeGen.DefaultValues.ImportValues(sf);
				}
			}

			string projectName = null;
			if (GenProject)
				projectName = ProjectName;
			string solutionName = null;
			if (GenSolution)
				solutionName = SolutionName;

			Info.GenerateCode(OutputFolder, NameSpace, projectName, solutionName);
		}

		public void StoreSettings()
		{
			Properties.Settings.Default.XMLFileLocation = XMLFilePath;
			Properties.Settings.Default.Project = GenProject;
			Properties.Settings.Default.ProjectName = ProjectName;
			Properties.Settings.Default.Solution = GenSolution;
			Properties.Settings.Default.SolutionName = SolutionName;
			Properties.Settings.Default.Namespace = NameSpace;
			Properties.Settings.Default.OutputFolder = OutputFolder;
			Properties.Settings.Default.Save();
		}
	}
}
