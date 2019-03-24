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
using System;
using System.Collections.Generic;
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

		/// <summary>
		///   Gets or sets the name of the company.
		/// </summary>
		public string Company
		{
			get
			{
				return companyTextBox.Text;
			}
			set
			{
				companyTextBox.Text = value;
			}
		}

		/// <summary>
		///   Copyright template (as specified by CSCodeGen library).
		/// </summary>
		public string CopyrightTemplate
		{
			get
			{
				return copyrightTextBox.Text;
			}
			set
			{
				copyrightTextBox.Text = value;
			}
		}

		/// <summary>
		///   Gets or sets the name of the developer.
		/// </summary>
		public string Developer
		{
			get
			{
				return developerTextBox.Text;
			}
			set
			{
				developerTextBox.Text = value;
			}
		}

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

		/// <summary>
		///   File header template (as specified by CSCodeGen library).
		/// </summary>
		public string[] FileHeaderTemplate
		{
			get
			{
				return fileRichTextBox.Lines;
			}
			set
			{
				fileRichTextBox.Lines = value;
			}
		}

		/// <summary>
		///   Gets or sets the flower box character for the flower box documentation.
		/// </summary>
		public char FlowerBoxCharacter
		{
			get
			{
				return flowerTextBox.Text[0];
			}
			set
			{
				flowerTextBox.Text = value.ToString();
			}
		}

		/// <summary>
		///   Gets or sets whether to include the sub-header in the generated files.
		/// </summary>
		public bool IncludeSubHeader
		{
			get
			{
				return subHeaderCheckBox.Checked;
			}
			set
			{
				subHeaderCheckBox.Checked = value;
			}
		}

		/// <summary>
		///   Gets or sets the size of the indentations (in spaces).
		/// </summary>
		public int IndentSize
		{
			get
			{
				return int.Parse(indentTextBox.Text);
			}
			set
			{
				indentTextBox.Text = value.ToString();
			}
		}

		/// <summary>
		///   License template (as specified by CSCodeGen library).
		/// </summary>
		public string[] LicenseTemplate
		{
			get
			{
				return licenseRichTextBox.Lines;
			}
			set
			{
				licenseRichTextBox.Lines = value;
			}
		}

		/// <summary>
		///   Gets or sets the number of characters to allow on each line.
		/// </summary>
		public int NumberOfCharsPerLine
		{
			get
			{
				return int.Parse(numPerLineTextBox.Text);
			}
			set
			{
				numPerLineTextBox.Text = value.ToString();
			}
		}

		/// <summary>
		///   Gets or sets whether tabs should be used instead of spaces for indents.
		/// </summary>
		public bool UseTabs
		{
			get
			{
				return tabCheckBox.Checked;
			}
			set
			{
				tabCheckBox.Checked = value;
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
		}

		#endregion Methods

		private void exportButton_Click(object sender, System.EventArgs e)
		{
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

			List<SettingInfo> settingList = new List<SettingInfo>
			{
				new SettingInfo("Company", Company),
				new SettingInfo("Developer", Developer),
				new SettingInfo("FileExtensionAddition", FileExtensionAddition),
				Settings.GetSettingFromType<char>("FlowerBoxCharacter", FlowerBoxCharacter),
				Settings.GetSettingFromType<bool>("IncludeSubHeader", IncludeSubHeader),
				Settings.GetSettingFromType<int>("IndentSize", IndentSize),
				Settings.GetSettingFromType<int>("NumberOfCharsPerLine", NumberOfCharsPerLine),
				Settings.GetSettingFromType<bool>("UseTabs", UseTabs),
				new SettingInfo("CopyrightTemplate", CopyrightTemplate),
			};
			settingList.AddRange(Settings.GetSettingsFromArray<string>("FileHeaderTemplate", FileHeaderTemplate));
			settingList.AddRange(Settings.GetSettingsFromArray<string>("LicenseTemplate", LicenseTemplate));
			Settings settings = new Settings(DateTime.Now, Assembly.GetExecutingAssembly().GetName().Version, settingList.ToArray());

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

			SettingInfo value = file.Root.FindSetting("Company");
			if (value != null)
				Company = value.Value;
			value = file.Root.FindSetting("CopyrightTemplate");
			if (value != null)
				CopyrightTemplate = value.Value;
			value = file.Root.FindSetting("Developer");
			if (value != null)
				Developer = value.Value;
			value = file.Root.FindSetting("FileExtensionAddition");
			if (value != null)
				FileExtensionAddition = value.Value;
			if (file.Root.TryGetArrayFromSettings<string>("FileHeaderTemplate", out string[] values))
				FileHeaderTemplate = values;
			if (file.Root.TryGetTypeFromSetting<char>("FlowerBoxCharacter", out char charValue))
				FlowerBoxCharacter = charValue;
			if (file.Root.TryGetTypeFromSetting<bool>("IncludeSubHeader", out bool boolValue))
				IncludeSubHeader = boolValue;
			if (file.Root.TryGetTypeFromSetting<int>("IndentSize", out int intValue))
				IndentSize = intValue;
			if (file.Root.TryGetArrayFromSettings<string>("LicenseTemplate", out values))
				LicenseTemplate = values;
			if (file.Root.TryGetTypeFromSetting<int>("NumberOfCharsPerLine", out intValue))
				NumberOfCharsPerLine = intValue;
			if (file.Root.TryGetTypeFromSetting<bool>("UseTabs", out boolValue))
				UseTabs = boolValue;
		}
	}
}
