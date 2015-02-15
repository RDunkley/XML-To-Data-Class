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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using XMLToDataClass.Data;
using XMLToDataClass.View;

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

			xmlFilePathLabel.Text = "";
			namespaceTextBox.Text = Properties.Settings.Default.Namespace;

			if (Properties.Settings.Default.OutputFolder.Length != 0)
				codeTextBox.Text = Properties.Settings.Default.OutputFolder;
			else
				codeTextBox.Text = Environment.CurrentDirectory;

			mainTreeView.AfterSelect += mainTreeView_AfterSelect;
			UpdateTreeView();
		}

		void mainTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			UpdateDetailView();
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

		private void UpdateGUIAccess(bool enable)
		{
			mainSplitContainer.Enabled = enable;
			processButton.Enabled = enable;
		}

		private void UpdateTreeView()
		{
			mainTreeView.Nodes.Clear();
			UpdateGUIAccess(false);

			if (mInfo != null)
			{
				foreach (ElementInfo info in mInfo.AllElements)
				{
					int index = mainTreeView.Nodes.Add(new TreeNode(info.Name));
					mainTreeView.Nodes[index].Tag = info;

					// Add the attributes.
					foreach (AttributeInfo aInfo in info.Attributes)
					{
						int aIndex = mainTreeView.Nodes[index].Nodes.Add(new TreeNode(aInfo.Name));
						mainTreeView.Nodes[index].Nodes[aIndex].Tag = aInfo;
					}
				}

				UpdateDetailView();
				UpdateGUIAccess(true);
			}
		}

		private void UpdateDetailView()
		{
			mainSplitContainer.Panel2.Controls.Clear();
			if(mainTreeView.SelectedNode != null)
			{
				if(mainTreeView.SelectedNode.Tag is ElementInfo)
				{
					ElementInfoPanel panel = new ElementInfoPanel(mainTreeView.SelectedNode.Tag as ElementInfo, mInfo);
					mainSplitContainer.Panel2.Controls.Add(panel);
					panel.Dock = DockStyle.Fill;
				}
				else if(mainTreeView.SelectedNode.Tag is AttributeInfo)
				{
					AttributeInfoPanel panel = new AttributeInfoPanel(mainTreeView.SelectedNode.Tag as AttributeInfo);
					mainSplitContainer.Panel2.Controls.Add(panel);
					panel.Dock = DockStyle.Fill;
				}
			}
		}

		#endregion Methods

		private void loadButton_Click(object sender, EventArgs e)
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

			UpdateGUIAccess(false);
			xmlFilePathLabel.Text = "";

			try
			{
				mInfo = CodeGen.ParseXML(fileDialog.FileName);
			}
			catch (InvalidDataException error)
			{
				MessageBox.Show(error.Message, "Error Parsing XML File", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.Exit();
			}
			catch (Exception error)
			{
				MessageBox.Show(error.Message, "Error Parsing XML File", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.Exit();
			}

			xmlFilePathLabel.Text = fileDialog.FileName;
			UpdateTreeView();
		}
	}
}
