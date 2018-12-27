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
	}
}
