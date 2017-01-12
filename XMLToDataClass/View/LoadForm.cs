//********************************************************************************************************************************
// Filename:    LoadForm.cs
// Owner:       Richard Dunkley
// Description: Partial class for the Form used to load in the XML file.
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

namespace XMLToDataClass.View
{
	public partial class LoadForm : Form
	{
		public bool PreserveHierarchy
		{
			get
			{
				return hierarchyCheckBox.Checked;
			}
		}

		public string FilePath
		{
			get
			{
				return filePathTextBox.Text;
			}
			set
			{
				filePathTextBox.Text = value;
			}
		}

		public LoadForm()
		{
			InitializeComponent();

			hierarchyCheckBox.Checked = true;
		}

		private void browseButton_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.CheckFileExists = true;
			dialog.CheckPathExists = true;
			dialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
			dialog.Multiselect = false;
			dialog.Title = "Select XML file to Generate Code For";
			dialog.FileName = filePathTextBox.Text;

			if (dialog.ShowDialog() != DialogResult.OK)
				return;

			filePathTextBox.Text = dialog.FileName;
		}
	}
}
