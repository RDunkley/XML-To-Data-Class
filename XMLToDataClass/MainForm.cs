﻿//********************************************************************************************************************************
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
using CSCodeGen.Parse.SettingsFile;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
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

		private GenController mController;

		/// <summary>
		///   Form to load the XML file.
		/// </summary>
		private LoadForm mLoadForm = new LoadForm();

		#endregion Fields

		#region Methods

		/// <summary>
		///   Instantiates a new MainForm object.
		/// </summary>
		public MainForm(GenController controller)
		{
			mController = controller ?? throw new ArgumentNullException("controller");

			InitializeComponent();

			this.Text = string.Format("{0} Version: {1}", this.Text, Assembly.GetExecutingAssembly().GetName().Version.ToString());
			mainSplitContainer.Panel2.AutoScroll = true;

			UpdateGui();
			UpdateTrees();
		}

		private void UpdateGui()
		{
			// Load settings from the controller.
			if (mController.Info != null)
				xmlFilePathLabel.Text = mController.XMLFilePath;
			else
				xmlFilePathLabel.Text = string.Empty;

			namespaceTextBox.Text = mController.NameSpace;
			projectCheckBox.Checked = mController.GenProject;
			projectTextBox.Text = mController.ProjectName;
			solutionCheckBox.Checked = mController.GenSolution;
			solutionTextBox.Text = mController.SolutionName;
			codeTextBox.Text = mController.OutputFolder;
			UpdateProjectSolutionInfo();
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
			if (node.Tag is ElementInfo[] && mController.Info.HierarchyMaintained)
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
		private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			UpdateTree(e.Node);
		}

		/// <summary>
		///   Handles the mainTreeView.AfterExpand event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e"><see cref="TreeViewEventArgs"/> containing the arguments for the event.</param>
		private void TreeView_AfterExpand(object sender, TreeViewEventArgs e)
		{
			UpdateTree(e.Node);
		}

		/// <summary>
		///   Handles the codeBrowseButton.Click event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e"><see cref="EventArgs"/> containing the arguments for the event.</param>
		private void CodeBrowseButton_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog
			{
				ShowNewFolderButton = true,
				Description = "Select the folder to save the data class files into"
			};

			if (dialog.ShowDialog() != DialogResult.OK)
				return;

			codeTextBox.Text = dialog.SelectedPath;
			mController.OutputFolder = dialog.SelectedPath;
		}

		/// <summary>
		///   Handles the process.Click event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e"><see cref="EventArgs"/> containing the arguments for the event.</param>
		private void ProcessButton_Click(object sender, EventArgs e)
		{
			generateButton.Enabled = false;

			try
			{
				mController.Validate();
			}
			catch (InvalidOperationException ex)
			{
				MessageBox.Show(ex.Message, "Error Generating Code", MessageBoxButtons.OK, MessageBoxIcon.Error);
				generateButton.Enabled = true;
				return;
			}

			if (!Directory.Exists(mController.OutputFolder))
			{
				if (MessageBox.Show("The code output folder does not exist. Should it be created?", "Output Folder Does Not Exist", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
				{
					generateButton.Enabled = true;
					return;
				}

				try
				{
					Directory.CreateDirectory(mController.OutputFolder);
				}
				catch(Exception ex)
				{
					MessageBox.Show("Unable to create the path to the code folder: " + ex.Message, "Error Creating Output Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
					generateButton.Enabled = true;
					return;
				}
			}

			try
			{
				mController.Process();
			}
			catch(InvalidOperationException ex)
			{
				MessageBox.Show(ex.Message, "Error Generating Code", MessageBoxButtons.OK, MessageBoxIcon.Error);
				generateButton.Enabled = true;
				return;
			}

			// Store the settings.
			mController.StoreSettings();

			MessageBox.Show("Code files were generated successfully", "Processing Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
			generateButton.Enabled = true;
		}

		private void SaveConfigButton_Click(object sender, EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog
			{
				CheckPathExists = true,
				OverwritePrompt = true,
				Title = "Specify the file and path to save the configuration data to",
				Filter = "Config files (*.x2dconf)|*.x2dconf|All files (*.*)|*.*",
				DefaultExt = "x2dconf"
			};
			try
			{
				dialog.InitialDirectory = Path.GetDirectoryName(Properties.Settings.Default.ConfigFileLocation);
				dialog.FileName = Path.GetFileName(Properties.Settings.Default.ConfigFileLocation);
			}
			catch(Exception)
			{
				// Do nothing and just let it open the default location.
			}

			if (dialog.ShowDialog() != DialogResult.OK)
				return;

			mController.SaveConfig(dialog.FileName);
			Properties.Settings.Default.ConfigFileLocation = dialog.FileName;
			Properties.Settings.Default.Save();
		}

		private void LoadConfigButton_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog
			{
				CheckFileExists = true,
				CheckPathExists = true,
				Filter = "Config files (*.x2dconf)|*.x2dconf|All files (*.*)|*.*",
				DefaultExt = "x2dconf",
				Multiselect = false,
				Title = "Specify the file and path to load the configuration data from"
			};
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

			mController.LoadConfig(dialog.FileName);

			UpdateDetailView();
			Properties.Settings.Default.ConfigFileLocation = dialog.FileName;
			Properties.Settings.Default.Save();
		}

		/// <summary>
		///   Updates access to the GUI (GUI is restricted until XML information is loaded).
		/// </summary>
		/// <param name="enable">True if it should be enabled, false otherwise.</param>
		private void UpdateGUIAccess(bool enable)
		{
			mainSplitContainer.Enabled = enable;
			generateButton.Enabled = enable;
			saveConfigButton.Enabled = enable;
			loadConfigButton.Enabled = enable;
		}

		private void CleanTrees()
		{
			foreach (TabPage page in treeTabControl.TabPages)
			{
				if (page.Controls.Count > 0)
				{
					TreeView view = page.Controls[0] as TreeView;
					if (view != null)
					{
						view.AfterSelect -= TreeView_AfterSelect;
						view.AfterExpand -= TreeView_AfterExpand;
						view.Dispose();
					}
				}
				page.Dispose();
			}
			treeTabControl.TabPages.Clear();
		}

		private void UpdateTrees()
		{
			UpdateGUIAccess(false);
			CleanTrees();
			if (mController.Info == null)
				return;

			string[] tabNames = mController.Info.GetAllNamespaces();
			foreach(string name in tabNames)
			{
				TabPage page = new TabPage(name);
				TreeView view = new TreeView();
				view.AfterSelect += TreeView_AfterSelect;
				view.AfterExpand += TreeView_AfterExpand;
				page.Controls.Add(view);
				view.Dock = DockStyle.Fill;
				treeTabControl.TabPages.Add(page);
				UpdateTreeView(name, view);
			}

			UpdateDetailView();
			UpdateGUIAccess(true);
		}

		/// <summary>
		///   Updates the tree view.
		/// </summary>
		private void UpdateTreeView(string name, TreeView view)
		{
			view.Nodes.Clear();

			ElementInfo[] roots;
			if (mController.Info.HierarchyMaintained)
			{
				roots = new ElementInfo[1] { mController.Info.RootNode };
			}
			else
			{
				roots = mController.Info.GetAllNamespaceNodes(name);
			}

			foreach (ElementInfo info in roots)
			{
				int index = view.Nodes.Add(new TreeNode(info.Name));
				view.Nodes[index].Tag = info;
				AddParentLevelTreeNode(view.Nodes[index], info);
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
			int index = node.Nodes.Add(new TreeNode(element.FullName));
			node.Nodes[index].Tag = element;
		}

		/// <summary>
		///   Updates the details view pane.
		/// </summary>
		private void UpdateDetailView()
		{
			mainSplitContainer.Panel2.Controls.Clear();
			if (treeTabControl.SelectedTab == null)
				return;

			TreeView view = treeTabControl.SelectedTab.Controls[0] as TreeView;
			if (view == null)
				return;

			if (view.SelectedNode == null)
				return;
			
			if(view.SelectedNode.Tag is ElementInfo)
			{
				if (mController.Info.HierarchyMaintained || view.SelectedNode.Parent == null)
				{
					ElementInfoPanel panel = new ElementInfoPanel(view.SelectedNode.Tag as ElementInfo);
					mainSplitContainer.Panel2.Controls.Add(panel);
					mainSplitContainer.Panel2.AutoScrollMinSize = panel.MinimumSize;
					panel.AutoScroll = true;
					panel.Dock = DockStyle.Fill;
				}
			}
			else if(view.SelectedNode.Tag is AttributeInfo)
			{
				DataInfoPanel panel = new DataInfoPanel(((AttributeInfo)view.SelectedNode.Tag).Info);
				mainSplitContainer.Panel2.Controls.Add(panel);
				mainSplitContainer.Panel2.AutoScrollMinSize = panel.MinimumSize;
				panel.AutoScroll = true;
				panel.Dock = DockStyle.Fill;
			}
			else if (view.SelectedNode.Tag is CDataInfo)
			{
				CDataInfo info = view.SelectedNode.Tag as CDataInfo;
				if (info.Include)
				{
					DataInfoPanel panel = new DataInfoPanel(info.Info);
					mainSplitContainer.Panel2.Controls.Add(panel);
					mainSplitContainer.Panel2.AutoScrollMinSize = panel.MinimumSize;
					panel.AutoScroll = true;
					panel.Dock = DockStyle.Fill;
					panel.Enabled = info.Include;
				}
			}
			else if (view.SelectedNode.Tag is TextInfo)
			{
				TextInfo info = view.SelectedNode.Tag as TextInfo;
				if (info.Include)
				{
					DataInfoPanel panel = new DataInfoPanel(info.Info);
					mainSplitContainer.Panel2.Controls.Add(panel);
					mainSplitContainer.Panel2.AutoScrollMinSize = panel.MinimumSize;
					panel.AutoScroll = true;
					panel.Dock = DockStyle.Fill;
					panel.Enabled = info.Include;
				}
			}
			else if(view.SelectedNode.Tag is DataInfo)
			{
				DataInfoPanel panel = new DataInfoPanel(view.SelectedNode.Tag as DataInfo);
				mainSplitContainer.Panel2.Controls.Add(panel);
				mainSplitContainer.Panel2.AutoScrollMinSize = panel.MinimumSize;
				panel.AutoScroll = true;
				panel.Dock = DockStyle.Fill;
			}
		}

		/// <summary>
		///   Handles the loadButton.Click event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e"><see cref="EventArgs"/> containing the arguments for the event.</param>
		private void LoadButton_Click(object sender, EventArgs e)
		{
			mLoadForm.FilePath = mController.XMLFilePath;

			if (mLoadForm.ShowDialog() != DialogResult.OK)
				return;

			UpdateGUIAccess(false);
			xmlFilePathLabel.Text = string.Empty;
			mController.UnLoad();

			try
			{
				mController.Load(mLoadForm.FilePath, mLoadForm.PreserveHierarchy);
			}
			catch(ArgumentException ex)
			{
				MessageBox.Show(ex.Message, "Error Parsing XML File", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			xmlFilePathLabel.Text = mController.XMLFilePath;
			Properties.Settings.Default.XMLFileLocation = mLoadForm.FilePath;
			Properties.Settings.Default.Save();
			UpdateTrees();
		}

		/// <summary>
		///   Handles the projectCheckBox.CheckedChanged event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e"><see cref="EventArgs"/> containing the arguments for the event.</param>
		private void ProjectCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mController.GenProject = projectCheckBox.Checked;
			UpdateProjectSolutionInfo();
		}

		/// <summary>
		///   Handles the solutionCheckBox.CheckedChanged event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e"><see cref="EventArgs"/> containing the arguments for the event.</param>
		private void SolutionCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mController.GenSolution = solutionCheckBox.Checked;
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
		private void SettingsButton_Click(object sender, EventArgs e)
		{
			SettingsForm form = new SettingsForm();

			if(!string.IsNullOrWhiteSpace(Properties.Settings.Default.CSCodeGenSettings))
			{
				using (StringReader sr = new StringReader(Properties.Settings.Default.CSCodeGenSettings))
				{
					try
					{
						Settings sf = new Settings(sr);
						form.Panel.ImportSettings(sf);
					}
					catch(ArgumentException)
					{
						// Ignore error and skip the settings.
					}
				}
			}
			form.FileExtensionAddition = Properties.Settings.Default.FileExtensionAddition;

			if (form.ShowDialog() != DialogResult.OK)
				return;

			using (StringWriter sw = new StringWriter())
			{
				Settings sf = new Settings(DateTime.Now, new Version(1, 0), form.Panel.ExportSettings());
				sf.ExportToXML(sw);
				Properties.Settings.Default.CSCodeGenSettings = sw.ToString();
			}
			Properties.Settings.Default.FileExtensionAddition = form.FileExtensionAddition;
			Properties.Settings.Default.Save();
		}

		#endregion Methods

		private void CodeTextBox_TextChanged(object sender, EventArgs e)
		{
			mController.OutputFolder = codeTextBox.Text;
		}

		private void NamespaceTextBox_TextChanged(object sender, EventArgs e)
		{
			mController.NameSpace = namespaceTextBox.Text;
		}

		private void ProjectTextBox_TextChanged(object sender, EventArgs e)
		{
			mController.ProjectName = projectTextBox.Text;
		}

		private void SolutionTextBox_TextChanged(object sender, EventArgs e)
		{
			mController.SolutionName = solutionTextBox.Text;
		}

		private void treeTabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateDetailView();
		}
	}
}
