/********************************************************************************************************************************
 * Copyright 2014 Richard Dunkley
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.IO;
using System.Windows.Forms;
using XMLToDataClass.Data;

namespace XMLToDataClass
{
	public partial class MainForm : Form
	{
		#region Fields

		private XMLInfo mInfo;

		#endregion Fields

		#region Methods

		public MainForm()
		{
			InitializeComponent();

			namespaceTextBox.Text = Properties.Settings.Default.Namespace;

			if (Properties.Settings.Default.OutputFolder.Length != 0)
				codeTextBox.Text = Properties.Settings.Default.OutputFolder;
			else
				codeTextBox.Text = Environment.CurrentDirectory;

			listView.Rows.Clear();
			mainDataGridView.Rows.Clear();
			foreach (int val in Enum.GetValues(typeof(DataType)))
			{
				string type = Enum.GetName(typeof(DataType), val);
				DataTypeColumn.Items.Add(type);
				textDataComboBox.Items.Add(type);
			}
			mainDataGridView.EditingControlShowing += mainDataGridView_EditingControlShowing;
		}

		void mainDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			ComboBox combo = e.Control as ComboBox;
			if(combo != null)
			{
				combo.SelectedIndexChanged -= new EventHandler(DataTypeComboBox_SelectedIndexChanged);
				combo.SelectedIndexChanged += new EventHandler(DataTypeComboBox_SelectedIndexChanged);
			}
		}

		private void DataTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox cb = (ComboBox)sender;
			foreach (DataGridViewRow row in mainDataGridView.SelectedRows)
			{
				AttributeInfo aInfo = row.Tag as AttributeInfo;
				aInfo.AttributeType = (DataType)Enum.Parse(typeof(DataType), cb.Text);
			}
		}

		private void codeBrowseButton_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			dialog.ShowNewFolderButton = true;
			dialog.Description = "Select the folder to save the data class files into";

			if (dialog.ShowDialog() != DialogResult.OK)
				return;

			codeTextBox.Text = dialog.SelectedPath;
		}

		private void processButton_Click(object sender, EventArgs e)
		{
			string codePath = codeTextBox.Text;
			string nameSpace = namespaceTextBox.Text;

			if(codePath.Length == 0)
			{
				MessageBox.Show("Path to the code folder cannot be empty", "Error Processing XML", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			try
			{
				codePath = Path.GetFullPath(codePath);

			}
			catch (Exception ex)
			{
				MessageBox.Show("Unable to obtain the path to the code folder: " + ex.Message, "Error Processing XML", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (nameSpace.Length == 0)
			{
				MessageBox.Show("The Namespace cannot be empty", "Error Processing XML", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			CodeGen.GenerateCodeClasses(mInfo, codePath, nameSpace);

			Properties.Settings.Default.Namespace = namespaceTextBox.Text;
			Properties.Settings.Default.OutputFolder = codeTextBox.Text;
			Properties.Settings.Default.Save();

			MessageBox.Show("Code files were generated successfully", "Processing Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void UpdateList()
		{
			listView.Rows.Clear();
			foreach (ElementInfo info in mInfo.AllElements)
			{
				int index = listView.Rows.Add(info.Name, info.ClassName);
				listView.Rows[index].Tag = info;
			}
			UpdateElementInfo();
		}

		private void UpdateElementInfo()
		{
			mainDataGridView.Rows.Clear();
			if(listView.SelectedRows.Count > 0 && listView.SelectedRows[0].Tag is ElementInfo)
			{
				ElementInfo info = listView.SelectedRows[0].Tag as ElementInfo;

				// Add the elements.
				foreach(AttributeInfo aInfo in info.Attributes)
				{
					int index = mainDataGridView.Rows.Add(aInfo.Name, aInfo.PropertyName, Enum.GetName(typeof(DataType), aInfo.AttributeType), aInfo.IsOptional, aInfo.CanBeEmpty);
					mainDataGridView.Rows[index].Tag = aInfo;
				}

				// Add CDATA if found.
				CDATACheckBox.Checked = info.HasCDATA;
				CDATAGroupBox.Enabled = info.HasCDATA;
				CDATAOptionalCheckBox.Checked = info.CDATAIsOptional;

				// Add Text if found.
				textCheckBox.Checked = info.HasText;
				textGroupBox.Enabled = info.HasText;
				textOptionalCheckBox.Checked = info.TextIsOptional;
				textDataComboBox.SelectedIndex = (int)info.TextDataType;
			}
		}

		#endregion Methods

		private void MainForm_Load(object sender, EventArgs e)
		{
			OpenFileDialog fileDialog = new OpenFileDialog();
			fileDialog.CheckFileExists = true;
			fileDialog.CheckPathExists = true;
			fileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
			fileDialog.DefaultExt = "xml";
			fileDialog.Multiselect = false;
			fileDialog.Title = "Select the XML file to generate the data classes from";

			if (fileDialog.ShowDialog() != DialogResult.OK)
				return;

			try
			{
				mInfo = CodeGen.ParseXML(fileDialog.FileName);
			}
			catch (InvalidDataException error)
			{
				MessageBox.Show(error.Message, "Error Parsing XML File", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.Exit();
			}

			UpdateList();
		}

		private void listView_SelectionChanged(object sender, EventArgs e)
		{
			UpdateElementInfo();
		}

		private void listView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex > 0 && e.RowIndex > 0 && listView.Rows[e.RowIndex].Tag is ElementInfo)
			{
				ElementInfo info = listView.Rows[e.RowIndex].Tag as ElementInfo;
				info.ChangeClassName((string)listView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
			}
		}

		private void CDATACheckBox_CheckedChanged(object sender, EventArgs e)
		{
			ElementInfo el = listView.SelectedRows[0].Tag as ElementInfo;
			el.HasCDATA = CDATACheckBox.Checked;
			CDATAGroupBox.Enabled = CDATACheckBox.Checked;
		}

		private void textCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			ElementInfo el = listView.SelectedRows[0].Tag as ElementInfo;
			el.HasText = textCheckBox.Checked;
			textGroupBox.Enabled = textCheckBox.Checked;
		}

		private void CDATAOptionalCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			ElementInfo el = listView.SelectedRows[0].Tag as ElementInfo;
			el.CDATAIsOptional = CDATAOptionalCheckBox.Checked;
		}

		private void textOptionalCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			ElementInfo el = listView.SelectedRows[0].Tag as ElementInfo;
			el.TextIsOptional = textOptionalCheckBox.Checked;
		}

		private void textDataComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			ElementInfo el = listView.SelectedRows[0].Tag as ElementInfo;
			el.TextDataType = (DataType)textDataComboBox.SelectedIndex;
		}
	}
}
