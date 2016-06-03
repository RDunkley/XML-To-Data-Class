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

		private LoadForm mLoadForm = new LoadForm();

		private XMLInfo mInfo;

		#endregion Fields

		#region Methods

		public MainForm()
		{
			InitializeComponent();

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


			mainTreeView.AfterSelect += mainTreeView_AfterSelect;
			UpdateProjectSolutionInfo();
			UpdateTreeView();
		}

		private void UpdateProjectSolutionInfo()
		{
			projectTextBox.Enabled = projectCheckBox.Checked;
			solutionCheckBox.Enabled = projectCheckBox.Checked;
			solutionTextBox.Enabled = projectCheckBox.Checked & solutionCheckBox.Checked;
		}

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

		void mainTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			UpdateTree(e.Node);
		}

		private void mainTreeView_AfterExpand(object sender, TreeViewEventArgs e)
		{
			UpdateTree(e.Node);
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
				ElementInfo[] roots;
				if (mInfo.HierarchyMaintained)
				{
					roots = new ElementInfo[1] { mInfo.GetRootNode() };
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

				UpdateDetailView();
				UpdateGUIAccess(true);
			}
		}

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

		private void AddLeafNode(TreeNode node, AttributeInfo attrib)
		{
			int index = node.Nodes.Add(new TreeNode(attrib.Info.Name));
			node.Nodes[index].Tag = attrib;
		}

		private void AddLeafNode(TreeNode node, ElementInfo element)
		{
			int index = node.Nodes.Add(new TreeNode(element.Name));
			node.Nodes[index].Tag = element;
		}

		private void UpdateDetailView()
		{
			mainSplitContainer.Panel2.Controls.Clear();
			if(mainTreeView.SelectedNode != null)
			{
				if(mainTreeView.SelectedNode.Tag is ElementInfo)
				{
					if (mInfo.HierarchyMaintained || mainTreeView.SelectedNode.Parent == null)
					{
						ElementInfoPanel panel = new ElementInfoPanel(mainTreeView.SelectedNode.Tag as ElementInfo);
						mainSplitContainer.Panel2.Controls.Add(panel);
						panel.Dock = DockStyle.Fill;
					}
				}
				else if(mainTreeView.SelectedNode.Tag is AttributeInfo)
				{
					DataInfoPanel panel = new DataInfoPanel(((AttributeInfo)mainTreeView.SelectedNode.Tag).Info);
					mainSplitContainer.Panel2.Controls.Add(panel);
					panel.Dock = DockStyle.Fill;
				}
				else if (mainTreeView.SelectedNode.Tag is CDataInfo)
				{
					CDataInfo info = mainTreeView.SelectedNode.Tag as CDataInfo;
					DataInfoPanel panel = new DataInfoPanel(info.Info);
					mainSplitContainer.Panel2.Controls.Add(panel);
					panel.Dock = DockStyle.Fill;
					panel.Enabled = info.Include;
				}
				else if (mainTreeView.SelectedNode.Tag is TextInfo)
				{
					TextInfo info = mainTreeView.SelectedNode.Tag as TextInfo;
					DataInfoPanel panel = new DataInfoPanel(info.Info);
					mainSplitContainer.Panel2.Controls.Add(panel);
					panel.Dock = DockStyle.Fill;
					panel.Enabled = info.Include;
				}
				else if(mainTreeView.SelectedNode.Tag is DataInfo)
				{
					DataInfoPanel panel = new DataInfoPanel(mainTreeView.SelectedNode.Tag as DataInfo);
					mainSplitContainer.Panel2.Controls.Add(panel);
					panel.Dock = DockStyle.Fill;
				}
			}
		}

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
				Application.Exit();
			}
			catch (Exception error)
			{
				MessageBox.Show(error.Message, "Error Parsing XML File", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.Exit();
			}

			xmlFilePathLabel.Text = mLoadForm.FilePath;
			UpdateTreeView();
		}

		#endregion Methods

		private void projectCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			UpdateProjectSolutionInfo();
		}

		private void solutionCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			UpdateProjectSolutionInfo();
		}
	}
}
