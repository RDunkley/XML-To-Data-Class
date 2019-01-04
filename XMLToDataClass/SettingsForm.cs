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
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using XMLToDataClass.Parse;

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
		public string[] CopyrightTemplate
		{
			get
			{
				return copyrightRichTextBox.Lines;
			}
			set
			{
				copyrightRichTextBox.Lines = value;
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
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.CheckPathExists = true;
			dialog.OverwritePrompt = true;
			dialog.Title = "Specify the file and path to save the settings to";
			dialog.Filter = "Config files (*.x2dsettings)|*.x2dsettings|All files (*.*)|*.*";
			dialog.DefaultExt = "x2dsettings";

			if (dialog.ShowDialog() != DialogResult.OK)
				return;

			List<SettingInfo> settingList = new List<SettingInfo>();
			settingList.Add(new SettingInfo("Company", Company));
			settingList.AddRange(GenerateSettingsFromStringArray("CopyrightTemplate", CopyrightTemplate));
			settingList.Add(new SettingInfo("Developer", Developer));
			settingList.Add(new SettingInfo("FileExtensionAddition", FileExtensionAddition));
			settingList.AddRange(GenerateSettingsFromStringArray("FileHeaderTemplate", FileHeaderTemplate));
			settingList.Add(new SettingInfo("FlowerBoxCharacter", FlowerBoxCharacter.ToString()));
			settingList.Add(new SettingInfo("IncludeSubHeader", IncludeSubHeader.ToString()));
			settingList.Add(new SettingInfo("IndentSize", IndentSize.ToString()));
			settingList.AddRange(GenerateSettingsFromStringArray("LicenseTemplate", LicenseTemplate));
			settingList.Add(new SettingInfo("NumberOfCharsPerLine", NumberOfCharsPerLine.ToString()));
			settingList.Add(new SettingInfo("UseTabs", UseTabs.ToString()));
			Settings settings = new Settings(DateTime.Now, Assembly.GetExecutingAssembly().GetName().Version, settingList.ToArray());


			SettingsFile file = new SettingsFile(settings, "UTF-8", "1.0");
			try
			{
				file.ExportToXML(dialog.FileName);
			}
			catch(InvalidOperationException ex)
			{
				MessageBox.Show("Unable to save the settings information. " + ex.Message, "Error Saving Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private SettingInfo[] GenerateSettingsFromStringArray(string name, string[] array)
		{
			List<SettingInfo> settingList = new List<SettingInfo>();
			settingList.Add(new SettingInfo(string.Format("{0}_size", name), array.Length.ToString()));
			for(int i = 0; i < array.Length; i++)
				settingList.Add(new SettingInfo(string.Format("{0}_{1}", name, i), array[i]));
			return settingList.ToArray();
		}

		private bool? GetBoolFromSetting(string name, SettingInfo[] settings)
		{
			SettingInfo setting = FindSetting(name, settings);
			if (setting == null)
				return null;
			bool result;
			if (!bool.TryParse(setting.Value, out result))
				return null;
			return result;
		}

		private int GetIntegerFromSetting(string name, SettingInfo[] settings)
		{
			SettingInfo setting = FindSetting(name, settings);
			if (setting == null)
				return -1;
			int result;
			if (!int.TryParse(setting.Value, out result))
				return -1;
			return result;
		}

		private string GetStringFromSetting(string name, SettingInfo[] settings)
		{
			SettingInfo setting = FindSetting(name, settings);
			if (setting == null)
				return null;
			return setting.Value;
		}

		private string[] GenerateStringArrayFromSettings(string name, SettingInfo[] settings)
		{
			int result = GetIntegerFromSetting(string.Format("{0}_size", name), settings);
			if (result == -1)
				return null;
			string[] array = new string[result];
			for(int i = 0; i < result; i++)
			{
				SettingInfo setting = FindSetting(string.Format("{0}_{1}", name, i), settings);
				if (setting == null)
					return null;
				array[i] = setting.Value;
			}
			return array;
		}

		private SettingInfo FindSetting(string name, SettingInfo[] settings)
		{
			foreach(SettingInfo setting in settings)
			{
				if (setting.Name == name)
					return setting;
			}
			return null;
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

			string input = GetStringFromSetting("Company", file.Root.SettingInfos);
			if (input != null)
				Company = input;
			string[] inputArray = GenerateStringArrayFromSettings("CopyrightTemplate", file.Root.SettingInfos);
			if (inputArray != null)
				CopyrightTemplate = inputArray;
			input = GetStringFromSetting("Developer", file.Root.SettingInfos);
			if (input != null)
				Developer = input;
			input = GetStringFromSetting("FileExtensionAddition", file.Root.SettingInfos);
			if (input != null)
				FileExtensionAddition = input;
			inputArray = GenerateStringArrayFromSettings("FileHeaderTemplate", file.Root.SettingInfos);
			if (inputArray != null)
				FileHeaderTemplate = inputArray;
			input = GetStringFromSetting("FlowerBoxCharacter", file.Root.SettingInfos);
			if (input != null)
				FlowerBoxCharacter = input[0];
			bool? boolValue = GetBoolFromSetting("IncludeSubHeader", file.Root.SettingInfos);
			if (boolValue.HasValue)
				IncludeSubHeader = boolValue.Value;
			int intValue = GetIntegerFromSetting("IndentSize", file.Root.SettingInfos);
			if (intValue != -1)
				IndentSize = intValue;
			inputArray = GenerateStringArrayFromSettings("LicenseTemplate", file.Root.SettingInfos);
			if (inputArray != null)
				LicenseTemplate = inputArray;
			intValue = GetIntegerFromSetting("NumberOfCharsPerLine", file.Root.SettingInfos);
			if (intValue != -1)
				NumberOfCharsPerLine = intValue;
			boolValue = GetBoolFromSetting("UseTabs", file.Root.SettingInfos);
			if (boolValue.HasValue)
				UseTabs = boolValue.Value;
		}
	}
}
