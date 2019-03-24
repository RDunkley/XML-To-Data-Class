//********************************************************************************************************************************
// Filename:    Program.cs
// Owner:       Richard Dunkley
// Description: Static class used to startup the application (entry point to the application).
//********************************************************************************************************************************
// Copyright © Richard Dunkley 2016
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the
// License. You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0  Unless required by applicable
// law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and
// limitations under the License.
//********************************************************************************************************************************
using System;
using System.Windows.Forms;

namespace XMLToDataClass
{
	/// <summary>
	///   Entry point into the application.
	/// </summary>
	static class Program
	{
		public static CommandSettings Settings = new CommandSettings();

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			ConsoleArgs<CommandSettings>.PopulateSettings(Settings);
			Settings.ValidateSettings();

			GenController controller = new GenController(Settings);

			// Check if we should run entirely by command line.
			if(Settings.XMLFilePath != null && Settings.ConfigPath != null && !Settings.GUI)
			{
				controller.Process();
				Application.Exit();
				return;
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Application.Run(new MainForm(controller));
		}
	}
}
