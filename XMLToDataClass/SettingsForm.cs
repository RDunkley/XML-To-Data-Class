//********************************************************************************************************************************
// Filename:    SettingsForm.cs
// Owner:       Richard Dunkley
// Description: Partial class containing the application's settings Form.
//********************************************************************************************************************************
// Copyright © Richard Dunkley 2016
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the
// License. You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0  Unless required by applicable
// law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and
// limitations under the License.
//********************************************************************************************************************************
using CSCodeGen.Parse;
using CSCodeGenSettingsGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace XMLToDataClass
{
	/// <summary>
	///   Form to display the application settings.
	/// </summary>
	public partial class SettingsForm : Form
	{
		#region Properties

		public CSCodeGenSettingsPanel Panel { get; private set; }

		/// <summary>
		///   Extension to add to the filenames of the auto-generated files.
		/// </summary>
		public string FileExtensionAddition
		{
			get
			{
				return fileExtenstionTextBox.Text;
			}
			set
			{
				fileExtenstionTextBox.Text = value;
			}
		}

		#endregion Properties

		#region Methods

		/// <summary>
		///   Instantiates a new SettingsForm object.
		/// </summary>
		public SettingsForm()
		{
			InitializeComponent();

			Panel = new CSCodeGenSettingsPanel();
			mainTableLayoutPanel.Controls.Add(Panel, 1, 2);
			mainTableLayoutPanel.SetColumnSpan(Panel, 2);
			Panel.Dock = DockStyle.Fill;
		}

		#endregion Methods

		private void exportButton_Click(object sender, System.EventArgs e)
		{
			if (!ValidateSettings())
				return;

			SaveFileDialog dialog = new SaveFileDialog
			{
				CheckPathExists = true,
				OverwritePrompt = true,
				Title = "Specify the file and path to save the settings to",
				Filter = "Config files (*.x2dsettings)|*.x2dsettings|All files (*.*)|*.*",
				DefaultExt = "x2dsettings",
			};

			if (dialog.ShowDialog() != DialogResult.OK)
				return;

			Settings settings = new Settings(DateTime.Now, Assembly.GetExecutingAssembly().GetName().Version, ExportSettings());

			SettingsFile file = new SettingsFile(settings);
			try
			{
				file.ExportToXML(dialog.FileName);
			}
			catch(InvalidOperationException ex)
			{
				MessageBox.Show("Unable to save the settings information. " + ex.Message, "Error Saving Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public SettingInfo[] ExportSettings()
		{
			if (!ValidateSettings())
				return null;

			List<SettingInfo> setList = new List<SettingInfo>(Panel.ExportSettings());
			setList.AddRange(Settings.GetSettingInfos(typeof(SettingsForm), this));
			return setList.ToArray();
		}

		public void ImportSettings(Settings settings)
		{
			if (settings == null)
				throw new ArgumentNullException("settings");

			// Pull the settings for the CSCodeGen.
			Panel.ImportSettings(settings);

			// Pull the settings for this form.
			settings.SetProperties(typeof(SettingsForm), this);
		}

		private void importButton_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.CheckFileExists = true;
			dialog.CheckPathExists = true;
			dialog.Filter = "Setting files (*.x2dsettings)|*.x2dsettings|All files (*.*)|*.*";
			dialog.DefaultExt = "x2dsettings";
			dialog.Multiselect = false;
			dialog.Title = "Specify the file and path to load the settings from";

			if (dialog.ShowDialog() != DialogResult.OK)
				return;

			SettingsFile file;
			try
			{
				file = new SettingsFile(dialog.FileName);
			}
			catch(ArgumentException ex)
			{
				MessageBox.Show("Unable to load the settings information. " + ex.Message, "Error Loading Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			ImportSettings(file.Root);
		}

		public bool ValidateSettings()
		{
			if (!Panel.ValidateSettings())
				return false;

			if(!string.IsNullOrWhiteSpace(fileExtenstionTextBox.Text))
			{
				if(!fileExtenstionTextBox.Text.All(char.IsLetterOrDigit))
				{
					MessageBox.Show
					(
						string.Format
						(
							"The file extension specified ({0}) has one or more characters that are not alphanumeric.",
							fileExtenstionTextBox.Text
						),
						"Error With Input Values",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error
					);
					return false;
				}
			}

			return true;
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			if (!ValidateSettings())
				return;

			this.Close();
		}
	}
}
