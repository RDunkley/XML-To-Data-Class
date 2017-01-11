//********************************************************************************************************************************
// Filename:    MainForm.cs
// Owner:       Richard Dunkley
// Description: Partial class of the main form of the application.
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
using System.IO;
using System.Text;
using System.Windows.Forms;
using XMLToDataClass.Data;
using XMLToDataClass.View;

namespace XMLToDataClass
{
	/// <summary>
	///   Main form of the application.
	/// </summary>
	public partial class MainForm : Form
	{
		#region Fields

		/// <summary>
		///   Form to load the XML file.
		/// </summary>
		private LoadForm mLoadForm = new LoadForm();

		/// <summary>
		///   Information loaded from the XML file.
		/// </summary>
		private XMLInfo mInfo;

		#endregion Fields

		#region Methods

		/// <summary>
		///   Instantiates a new MainForm object.
		/// </summary>
		public MainForm()
		{
			InitializeComponent();

			mLoadForm.FilePath = Properties.Settings.Default.XMLFileLocation;
			xmlFilePathLabel.Text = "";
			namespaceTextBox.Text = Properties.Settings.Default.Namespace;
			projectCheckBox.Checked = Properties.Settings.Default.Project;
			projectTextBox.Text = Properties.Settings.Default.ProjectName;
			solutionCheckBox.Checked = Properties.Settings.Default.Solution;
			solutionTextBox.Text = Properties.Settings.Default.SolutionName;

			if (Properties.Settings.Default.OutputFolder.Length != 0)
				codeTextBox.Text = Properties.Settings.Default.OutputFolder;
			else
				codeTextBox.Text = Environment.CurrentDirectory;

			mainSplitContainer.Panel2.AutoScroll = true;

			mainTreeView.AfterSelect += mainTreeView_AfterSelect;
			UpdateProjectSolutionInfo();
			UpdateTreeView();
		}

		/// <summary>
		///   Updates the Project/Solution information based on the current check boxes.
		/// </summary>
		private void UpdateProjectSolutionInfo()
		{
			projectTextBox.Enabled = projectCheckBox.Checked;
			solutionCheckBox.Enabled = projectCheckBox.Checked;
			solutionTextBox.Enabled = projectCheckBox.Checked & solutionCheckBox.Checked;
		}

		/// <summary>
		///   Updates the element tree.
		/// </summary>
		/// <param name="node"></param>
		private void UpdateTree(TreeNode node)
		{
			if (node.Tag is ElementInfo[] && mInfo.HierarchyMaintained)
			{
				// Elements are selected so load child elements.
				foreach (TreeNode child in node.Nodes)
				{
					if (child.Nodes.Count == 0)
						AddParentLevelTreeNode(child, child.Tag as ElementInfo);
				}
			}

			UpdateDetailView();
		}

		/// <summary>
		///   Handles the mainTreeView.AfterSelect event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e"><see cref="TreeViewEventArgs"/> containing the arguments for the event.</param>
		private void mainTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			UpdateTree(e.Node);
		}

		/// <summary>
		///   Handles the mainTreeView.AfterExpand event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e"><see cref="TreeViewEventArgs"/> containing the arguments for the event.</param>
		private void mainTreeView_AfterExpand(object sender, TreeViewEventArgs e)
		{
			UpdateTree(e.Node);
		}

		/// <summary>
		///   Handles the codeBrowseButton.Click event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e"><see cref="EventArgs"/> containing the arguments for the event.</param>
		private void codeBrowseButton_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			dialog.ShowNewFolderButton = true;
			dialog.Description = "Select the folder to save the data class files into";

			if (dialog.ShowDialog() != DialogResult.OK)
				return;

			codeTextBox.Text = dialog.SelectedPath;
		}

		/// <summary>
		///   Handles the process.Click event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e"><see cref="EventArgs"/> containing the arguments for the event.</param>
		private void processButton_Click(object sender, EventArgs e)
		{
			generateButton.Enabled = false;

			CSCodeGen.DefaultValues.CompanyName = Properties.Settings.Default.CSCodeGenCompanyName;
			CSCodeGen.DefaultValues.Developer = Properties.Settings.Default.CSCodeGenDeveloper;
			CSCodeGen.DefaultValues.UseTabs = Properties.Settings.Default.CSCodeGenUseTabs;
			CSCodeGen.DefaultValues.IncludeSubHeader = Properties.Settings.Default.CSCodeGenIncludeSubHeader;
			CSCodeGen.DefaultValues.FlowerBoxCharacter = Properties.Settings.Default.CSCodeGenFlowerBoxCharacter;
			CSCodeGen.DefaultValues.NumCharactersPerLine = Properties.Settings.Default.CSCodeGenNumCharsPerLine;
			CSCodeGen.DefaultValues.TabSize = Properties.Settings.Default.CSCodeGenIndentSize;
			CSCodeGen.DefaultValues.FileInfoTemplate = ParseTemplate(Properties.Settings.Default.CSCodeGenFileHeaderTemplate);
			CSCodeGen.DefaultValues.CopyrightTemplate = MergeTemplate(ParseTemplate(Properties.Settings.Default.CSCodeGenCopyrightTemplate));
			CSCodeGen.DefaultValues.LicenseTemplate = ParseTemplate(Properties.Settings.Default.CSCodeGenLicenseTemplate);

			string codePath = codeTextBox.Text;
			string nameSpace = namespaceTextBox.Text;

			if(codePath.Length == 0)
			{
				MessageBox.Show("Path to the code folder cannot be empty", "Error Processing XML", MessageBoxButtons.OK, MessageBoxIcon.Error);
				generateButton.Enabled = true;
				return;
			}

			try
			{
				codePath = Path.GetFullPath(codePath);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Unable to obtain the path to the code folder: " + ex.Message, "Error Processing XML", MessageBoxButtons.OK, MessageBoxIcon.Error);
				generateButton.Enabled = true;
				return;
			}

			if(!Directory.Exists(codePath))
			{
				if (MessageBox.Show("The code output folder does not exist. Should it be created?", "Output Folder Does Not Exist", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
				{
					generateButton.Enabled = true;
					return;
				}

				try
				{
					Directory.CreateDirectory(codePath);
				}
				catch(Exception ex)
				{
					MessageBox.Show("Unable to create the path to the code folder: " + ex.Message, "Error Creating Output Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
					generateButton.Enabled = true;
					return;
				}
			}

			if (nameSpace.Length == 0)
			{
				MessageBox.Show("The Namespace cannot be empty", "Error Processing XML", MessageBoxButtons.OK, MessageBoxIcon.Error);
				generateButton.Enabled = true;
				return;
			}

			//CodeGen.GenerateCodeClasses(mInfo, codePath, nameSpace, projectCheckBox.Checked, projectTextBox.Text, solutionCheckBox.Checked, solutionTextBox.Text);

			string projectName = null;
			if (projectCheckBox.Checked)
				projectName = projectTextBox.Text;
			string solutionName = null;
			if (solutionCheckBox.Checked)
				solutionName = solutionTextBox.Text;
			mInfo.GenerateCode(codePath, nameSpace, projectName, solutionName);

			Properties.Settings.Default.Project = projectCheckBox.Checked;
			Properties.Settings.Default.ProjectName = projectTextBox.Text;
			Properties.Settings.Default.Solution = solutionCheckBox.Checked;
			Properties.Settings.Default.SolutionName = solutionTextBox.Text;
			Properties.Settings.Default.Namespace = namespaceTextBox.Text;
			Properties.Settings.Default.OutputFolder = codeTextBox.Text;
			Properties.Settings.Default.Save();

			MessageBox.Show("Code files were generated successfully", "Processing Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
			generateButton.Enabled = true;
		}

		private void saveConfigButton_Click(object sender, EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.CheckPathExists = true;
			dialog.OverwritePrompt = true;
			dialog.Title = "Specify the file and path to save the configuration data to";
			dialog.Filter = "Config files (*.x2dconf)|*.x2dconf|All files (*.*)|*.*";
			dialog.DefaultExt = "x2dconf";
			dialog.FileName = Properties.Settings.Default.ConfigFileLocation;

			if (dialog.ShowDialog() != DialogResult.OK)
				return;

			mInfo.Save(dialog.FileName);
			Properties.Settings.Default.ConfigFileLocation = dialog.FileName;
			Properties.Settings.Default.Save();
		}

		private void loadConfigButton_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.CheckFileExists = true;
			dialog.CheckPathExists = true;
			dialog.Filter = "Config files (*.x2dconf)|*.x2dconf|All files (*.*)|*.*";
			dialog.DefaultExt = "x2dconf";
			dialog.Multiselect = false;
			dialog.Title = "Specify the file and path to load the configuration data from";
			try
			{
				dialog.InitialDirectory = Path.GetDirectoryName(Properties.Settings.Default.ConfigFileLocation);
				dialog.FileName = Path.GetFileName(Properties.Settings.Default.ConfigFileLocation);
			}
			catch (Exception)
			{
				// Ignore exception and just use the default.
			}

			if (dialog.ShowDialog() != DialogResult.OK)
				return;

			mInfo.Load(dialog.FileName);
			Properties.Settings.Default.ConfigFileLocation = dialog.FileName;
			Properties.Settings.Default.Save();
			UpdateDetailView();
		}

		/// <summary>
		///   Updates access to the GUI (GUI is restricted until XML information is loaded).
		/// </summary>
		/// <param name="enable">True if it should be enabled, false otherwise.</param>
		private void UpdateGUIAccess(bool enable)
		{
			mainSplitContainer.Enabled = enable;
			mainClassLabel.Enabled = enable;
			mainClassTextBox.Enabled = enable;
			generateButton.Enabled = enable;
			saveConfigButton.Enabled = enable;
			loadConfigButton.Enabled = enable;
		}

		/// <summary>
		///   Updates the tree view.
		/// </summary>
		private void UpdateTreeView()
		{
			mainTreeView.Nodes.Clear();
			UpdateGUIAccess(false);

			if (mInfo != null)
			{
				ElementInfo[] roots;
				if (mInfo.HierarchyMaintained)
				{
					roots = new ElementInfo[1] { mInfo.RootNode };
				}
				else
				{
					roots = mInfo.GetAllNodes();
				}

				foreach (ElementInfo info in roots)
				{
					int index = mainTreeView.Nodes.Add(new TreeNode(info.Name));
					mainTreeView.Nodes[index].Tag = info;
					AddParentLevelTreeNode(mainTreeView.Nodes[index], info);
				}

				mainClassTextBox.Text = mInfo.MainClassName;

				UpdateDetailView();
				UpdateGUIAccess(true);
			}
		}

		/// <summary>
		///   Adds the parent level tree node.
		/// </summary>
		/// <param name="node">Node to be added.</param>
		/// <param name="info"><see cref="ElementInfo"/> object corresponding to the node.</param>
		private void AddParentLevelTreeNode(TreeNode node, ElementInfo info)
		{
			// Text
			int index = node.Nodes.Add(new TreeNode("Text"));
			node.Nodes[index].Tag = info.Text;

			// CDATA
			index = node.Nodes.Add(new TreeNode("CDATA"));
			node.Nodes[index].Tag = info.CDATA;

			// Attributes
			index = node.Nodes.Add(new TreeNode("Attributes"));
			node.Nodes[index].Tag = info.Attributes;
			foreach (AttributeInfo attrib in info.Attributes)
				AddLeafNode(node.Nodes[index], attrib);

			// Child Elements
			index = node.Nodes.Add(new TreeNode("Elements"));
			node.Nodes[index].Tag = info.Children;
			foreach (ElementInfo element in info.Children)
				AddLeafNode(node.Nodes[index], element);
		}

		/// <summary>
		///   Adds a leaf node.
		/// </summary>
		/// <param name="node">Node to be added.</param>
		/// <param name="attrib"><see cref="AttributeInfo"/> corresponding to the node.</param>
		private void AddLeafNode(TreeNode node, AttributeInfo attrib)
		{
			int index = node.Nodes.Add(new TreeNode(attrib.Info.Name));
			node.Nodes[index].Tag = attrib;
		}

		/// <summary>
		///   Adds a leaf node.
		/// </summary>
		/// <param name="node">Node to be added.</param>
		/// <param name="element"><see cref="ElementInfo"/> corresponding to the node.</param>
		private void AddLeafNode(TreeNode node, ElementInfo element)
		{
			int index = node.Nodes.Add(new TreeNode(element.Name));
			node.Nodes[index].Tag = element;
		}

		/// <summary>
		///   Updates the details view pane.
		/// </summary>
		private void UpdateDetailView()
		{
			if (mInfo != null)
				mainClassTextBox.Text = mInfo.MainClassName;

			mainSplitContainer.Panel2.Controls.Clear();
			if(mainTreeView.SelectedNode != null)
			{
				if(mainTreeView.SelectedNode.Tag is ElementInfo)
				{
					if (mInfo.HierarchyMaintained || mainTreeView.SelectedNode.Parent == null)
					{
						ElementInfoPanel panel = new ElementInfoPanel(mainTreeView.SelectedNode.Tag as ElementInfo);
						mainSplitContainer.Panel2.Controls.Add(panel);
						mainSplitContainer.Panel2.AutoScrollMinSize = panel.MinimumSize;
						panel.Dock = DockStyle.Fill;
					}
				}
				else if(mainTreeView.SelectedNode.Tag is AttributeInfo)
				{
					DataInfoPanel panel = new DataInfoPanel(((AttributeInfo)mainTreeView.SelectedNode.Tag).Info);
					mainSplitContainer.Panel2.Controls.Add(panel);
					mainSplitContainer.Panel2.AutoScrollMinSize = panel.MinimumSize;
					panel.Dock = DockStyle.Fill;
				}
				else if (mainTreeView.SelectedNode.Tag is CDataInfo)
				{
					CDataInfo info = mainTreeView.SelectedNode.Tag as CDataInfo;
					if (info.Include)
					{
						DataInfoPanel panel = new DataInfoPanel(info.Info);
						mainSplitContainer.Panel2.Controls.Add(panel);
						mainSplitContainer.Panel2.AutoScrollMinSize = panel.MinimumSize;
						panel.Dock = DockStyle.Fill;
						panel.Enabled = info.Include;
					}
				}
				else if (mainTreeView.SelectedNode.Tag is TextInfo)
				{
					TextInfo info = mainTreeView.SelectedNode.Tag as TextInfo;
					if (info.Include)
					{
						DataInfoPanel panel = new DataInfoPanel(info.Info);
						mainSplitContainer.Panel2.Controls.Add(panel);
						mainSplitContainer.Panel2.AutoScrollMinSize = panel.MinimumSize;
						panel.Dock = DockStyle.Fill;
						panel.Enabled = info.Include;
					}
				}
				else if(mainTreeView.SelectedNode.Tag is DataInfo)
				{
					DataInfoPanel panel = new DataInfoPanel(mainTreeView.SelectedNode.Tag as DataInfo);
					mainSplitContainer.Panel2.Controls.Add(panel);
					mainSplitContainer.Panel2.AutoScrollMinSize = panel.MinimumSize;
					panel.Dock = DockStyle.Fill;
				}
			}
		}

		/// <summary>
		///   Handles the loadButton.Click event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e"><see cref="EventArgs"/> containing the arguments for the event.</param>
		private void loadButton_Click(object sender, EventArgs e)
		{
			if (mLoadForm.ShowDialog() != DialogResult.OK)
				return;

			UpdateGUIAccess(false);
			xmlFilePathLabel.Text = "";

			try
			{
				mInfo = new XMLInfo(mLoadForm.FilePath, mLoadForm.PreserveHierarchy, mLoadForm.IgnoreCase);
			}
			catch (InvalidDataException error)
			{
				MessageBox.Show(error.Message, "Error Parsing XML File", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			catch (Exception error)
			{
				MessageBox.Show(error.Message, "Error Parsing XML File", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			xmlFilePathLabel.Text = mLoadForm.FilePath;
			Properties.Settings.Default.XMLFileLocation = mLoadForm.FilePath;
			Properties.Settings.Default.Save();
			UpdateTreeView();
		}

		/// <summary>
		///   Handles the projectCheckBox.CheckedChanged event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e"><see cref="EventArgs"/> containing the arguments for the event.</param>
		private void projectCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			UpdateProjectSolutionInfo();
		}

		/// <summary>
		///   Handles the solutionCheckBox.CheckedChanged event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e"><see cref="EventArgs"/> containing the arguments for the event.</param>
		private void solutionCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			UpdateProjectSolutionInfo();
		}

		/// <summary>
		///   Parses the template.
		/// </summary>
		/// <param name="template">Template string to be parsed.</param>
		/// <returns>Array of the parsed template lines.</returns>
		private string[] ParseTemplate(string template)
		{
			if (template == null)
				return new string[0];
			if (template.Length == 0)
				return new string[0];

			return template.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
		}

		/// <summary>
		///   Merges the template lines into one string.
		/// </summary>
		/// <param name="template">Lines of the template to be merged.</param>
		/// <returns>Merged template string.</returns>
		private string MergeTemplate(string[] template)
		{
			if (template == null)
				return string.Empty;
			if (template.Length == 0)
				return string.Empty;

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < template.Length; i++)
			{
				if (i == template.Length - 1)
					sb.Append(template[i]);
				else
					sb.AppendLine(template[i]);
			}
			return sb.ToString();
		}

		/// <summary>
		///   Handles the settingsButton.Click event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e"><see cref="EventArgs"/> containing the arguments for the event.</param>
		private void settingsButton_Click(object sender, EventArgs e)
		{
			SettingsForm form = new SettingsForm();
			form.Company = Properties.Settings.Default.CSCodeGenCompanyName;
			form.Developer = Properties.Settings.Default.CSCodeGenDeveloper;
			form.UseTabs = Properties.Settings.Default.CSCodeGenUseTabs;
			form.IncludeSubHeader = Properties.Settings.Default.CSCodeGenIncludeSubHeader;
			form.FlowerBoxCharacter = Properties.Settings.Default.CSCodeGenFlowerBoxCharacter;
			form.NumberOfCharsPerLine = Properties.Settings.Default.CSCodeGenNumCharsPerLine;
			form.IndentSize = Properties.Settings.Default.CSCodeGenIndentSize;
			form.FileHeaderTemplate = ParseTemplate(Properties.Settings.Default.CSCodeGenFileHeaderTemplate);
			form.CopyrightTemplate = ParseTemplate(Properties.Settings.Default.CSCodeGenCopyrightTemplate);
			form.LicenseTemplate = ParseTemplate(Properties.Settings.Default.CSCodeGenLicenseTemplate);

			if (form.ShowDialog() != DialogResult.OK)
				return;

			Properties.Settings.Default.CSCodeGenCompanyName = form.Company;
			Properties.Settings.Default.CSCodeGenDeveloper = form.Developer;
			Properties.Settings.Default.CSCodeGenUseTabs = form.UseTabs;
			Properties.Settings.Default.CSCodeGenIncludeSubHeader = form.IncludeSubHeader;
			Properties.Settings.Default.CSCodeGenFlowerBoxCharacter = form.FlowerBoxCharacter;
			Properties.Settings.Default.CSCodeGenNumCharsPerLine = form.NumberOfCharsPerLine;
			Properties.Settings.Default.CSCodeGenIndentSize = form.IndentSize;
			Properties.Settings.Default.CSCodeGenFileHeaderTemplate = MergeTemplate(form.FileHeaderTemplate);
			Properties.Settings.Default.CSCodeGenCopyrightTemplate = MergeTemplate(form.CopyrightTemplate);
			Properties.Settings.Default.CSCodeGenLicenseTemplate = MergeTemplate(form.LicenseTemplate);
			Properties.Settings.Default.Save();
		}

		#endregion Methods

		private void mainClassTextBox_TextChanged(object sender, EventArgs e)
		{
			if (mInfo != null)
				mInfo.MainClassName = mainClassTextBox.Text;
		}
	}
}
