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
namespace XMLToDataClass
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.codeTextBox = new System.Windows.Forms.TextBox();
			this.codeBrowseButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.generateButton = new System.Windows.Forms.Button();
			this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
			this.elementGroupBox = new System.Windows.Forms.GroupBox();
			this.mainTreeView = new System.Windows.Forms.TreeView();
			this.label1 = new System.Windows.Forms.Label();
			this.xmlFilePathLabel = new System.Windows.Forms.Label();
			this.loadButton = new System.Windows.Forms.Button();
			this.namespaceTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.projectCheckBox = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.projectTextBox = new System.Windows.Forms.TextBox();
			this.solutionCheckBox = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.solutionTextBox = new System.Windows.Forms.TextBox();
			this.settingsButton = new System.Windows.Forms.Button();
			this.buttonFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.saveConfigButton = new System.Windows.Forms.Button();
			this.loadConfigButton = new System.Windows.Forms.Button();
			this.mainTableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
			this.mainSplitContainer.Panel1.SuspendLayout();
			this.mainSplitContainer.SuspendLayout();
			this.elementGroupBox.SuspendLayout();
			this.buttonFlowLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// codeTextBox
			// 
			this.codeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.codeTextBox.Location = new System.Drawing.Point(88, 32);
			this.codeTextBox.Name = "codeTextBox";
			this.codeTextBox.Size = new System.Drawing.Size(411, 20);
			this.codeTextBox.TabIndex = 2;
			this.codeTextBox.TextChanged += new System.EventHandler(this.codeTextBox_TextChanged);
			// 
			// codeBrowseButton
			// 
			this.codeBrowseButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.codeBrowseButton.Location = new System.Drawing.Point(505, 32);
			this.codeBrowseButton.Name = "codeBrowseButton";
			this.codeBrowseButton.Size = new System.Drawing.Size(75, 23);
			this.codeBrowseButton.TabIndex = 3;
			this.codeBrowseButton.Text = "Browse";
			this.codeBrowseButton.UseVisualStyleBackColor = true;
			this.codeBrowseButton.Click += new System.EventHandler(this.codeBrowseButton_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Location = new System.Drawing.Point(3, 29);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(79, 29);
			this.label2.TabIndex = 5;
			this.label2.Text = "Output Folder:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// generateButton
			// 
			this.generateButton.AutoSize = true;
			this.generateButton.Location = new System.Drawing.Point(3, 3);
			this.generateButton.Name = "generateButton";
			this.generateButton.Size = new System.Drawing.Size(89, 23);
			this.generateButton.TabIndex = 6;
			this.generateButton.Text = "Generate Code";
			this.generateButton.UseVisualStyleBackColor = true;
			this.generateButton.Click += new System.EventHandler(this.processButton_Click);
			// 
			// mainTableLayoutPanel
			// 
			this.mainTableLayoutPanel.ColumnCount = 3;
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.mainTableLayoutPanel.Controls.Add(this.mainSplitContainer, 0, 7);
			this.mainTableLayoutPanel.Controls.Add(this.label1, 0, 0);
			this.mainTableLayoutPanel.Controls.Add(this.xmlFilePathLabel, 1, 0);
			this.mainTableLayoutPanel.Controls.Add(this.loadButton, 2, 0);
			this.mainTableLayoutPanel.Controls.Add(this.codeBrowseButton, 2, 1);
			this.mainTableLayoutPanel.Controls.Add(this.codeTextBox, 1, 1);
			this.mainTableLayoutPanel.Controls.Add(this.namespaceTextBox, 1, 2);
			this.mainTableLayoutPanel.Controls.Add(this.label3, 0, 2);
			this.mainTableLayoutPanel.Controls.Add(this.label2, 0, 1);
			this.mainTableLayoutPanel.Controls.Add(this.projectCheckBox, 1, 3);
			this.mainTableLayoutPanel.Controls.Add(this.label4, 0, 4);
			this.mainTableLayoutPanel.Controls.Add(this.projectTextBox, 1, 4);
			this.mainTableLayoutPanel.Controls.Add(this.solutionCheckBox, 1, 5);
			this.mainTableLayoutPanel.Controls.Add(this.label5, 0, 6);
			this.mainTableLayoutPanel.Controls.Add(this.solutionTextBox, 1, 6);
			this.mainTableLayoutPanel.Controls.Add(this.settingsButton, 2, 8);
			this.mainTableLayoutPanel.Controls.Add(this.buttonFlowLayoutPanel, 0, 8);
			this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
			this.mainTableLayoutPanel.RowCount = 9;
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.mainTableLayoutPanel.Size = new System.Drawing.Size(583, 467);
			this.mainTableLayoutPanel.TabIndex = 7;
			// 
			// mainSplitContainer
			// 
			this.mainTableLayoutPanel.SetColumnSpan(this.mainSplitContainer, 3);
			this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainSplitContainer.Location = new System.Drawing.Point(3, 185);
			this.mainSplitContainer.Name = "mainSplitContainer";
			// 
			// mainSplitContainer.Panel1
			// 
			this.mainSplitContainer.Panel1.Controls.Add(this.elementGroupBox);
			// 
			// mainSplitContainer.Panel2
			// 
			this.mainSplitContainer.Panel2.AutoScroll = true;
			this.mainSplitContainer.Size = new System.Drawing.Size(577, 244);
			this.mainSplitContainer.SplitterDistance = 191;
			this.mainSplitContainer.TabIndex = 7;
			// 
			// elementGroupBox
			// 
			this.elementGroupBox.Controls.Add(this.mainTreeView);
			this.elementGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.elementGroupBox.Location = new System.Drawing.Point(0, 0);
			this.elementGroupBox.Name = "elementGroupBox";
			this.elementGroupBox.Size = new System.Drawing.Size(191, 244);
			this.elementGroupBox.TabIndex = 0;
			this.elementGroupBox.TabStop = false;
			this.elementGroupBox.Text = "Elements";
			// 
			// mainTreeView
			// 
			this.mainTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTreeView.Location = new System.Drawing.Point(3, 16);
			this.mainTreeView.Name = "mainTreeView";
			this.mainTreeView.Size = new System.Drawing.Size(185, 225);
			this.mainTreeView.TabIndex = 0;
			this.mainTreeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.mainTreeView_AfterExpand);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(79, 29);
			this.label1.TabIndex = 10;
			this.label1.Text = "XML File:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// xmlFilePathLabel
			// 
			this.xmlFilePathLabel.AutoSize = true;
			this.xmlFilePathLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.xmlFilePathLabel.Location = new System.Drawing.Point(88, 0);
			this.xmlFilePathLabel.Name = "xmlFilePathLabel";
			this.xmlFilePathLabel.Size = new System.Drawing.Size(411, 29);
			this.xmlFilePathLabel.TabIndex = 11;
			this.xmlFilePathLabel.Text = "label4";
			this.xmlFilePathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// loadButton
			// 
			this.loadButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.loadButton.Location = new System.Drawing.Point(505, 3);
			this.loadButton.Name = "loadButton";
			this.loadButton.Size = new System.Drawing.Size(75, 23);
			this.loadButton.TabIndex = 12;
			this.loadButton.Text = "Load";
			this.loadButton.UseVisualStyleBackColor = true;
			this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
			// 
			// namespaceTextBox
			// 
			this.namespaceTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.namespaceTextBox.Location = new System.Drawing.Point(88, 61);
			this.namespaceTextBox.Name = "namespaceTextBox";
			this.namespaceTextBox.Size = new System.Drawing.Size(411, 20);
			this.namespaceTextBox.TabIndex = 9;
			this.namespaceTextBox.Text = "XMLToDataClass";
			this.namespaceTextBox.WordWrap = false;
			this.namespaceTextBox.TextChanged += new System.EventHandler(this.namespaceTextBox_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.Location = new System.Drawing.Point(3, 58);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(79, 26);
			this.label3.TabIndex = 8;
			this.label3.Text = "Namespace:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// projectCheckBox
			// 
			this.projectCheckBox.AutoSize = true;
			this.projectCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.projectCheckBox.Location = new System.Drawing.Point(88, 87);
			this.projectCheckBox.Name = "projectCheckBox";
			this.projectCheckBox.Size = new System.Drawing.Size(411, 17);
			this.projectCheckBox.TabIndex = 14;
			this.projectCheckBox.Text = "Generate Project File with Code";
			this.projectCheckBox.UseVisualStyleBackColor = true;
			this.projectCheckBox.CheckedChanged += new System.EventHandler(this.projectCheckBox_CheckedChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label4.Location = new System.Drawing.Point(3, 107);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(79, 26);
			this.label4.TabIndex = 15;
			this.label4.Text = "Project Name:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// projectTextBox
			// 
			this.projectTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.projectTextBox.Location = new System.Drawing.Point(88, 110);
			this.projectTextBox.Name = "projectTextBox";
			this.projectTextBox.Size = new System.Drawing.Size(411, 20);
			this.projectTextBox.TabIndex = 16;
			this.projectTextBox.TextChanged += new System.EventHandler(this.projectTextBox_TextChanged);
			// 
			// solutionCheckBox
			// 
			this.solutionCheckBox.AutoSize = true;
			this.solutionCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.solutionCheckBox.Location = new System.Drawing.Point(88, 136);
			this.solutionCheckBox.Name = "solutionCheckBox";
			this.solutionCheckBox.Size = new System.Drawing.Size(411, 17);
			this.solutionCheckBox.TabIndex = 17;
			this.solutionCheckBox.Text = "Generate Solution for Project";
			this.solutionCheckBox.UseVisualStyleBackColor = true;
			this.solutionCheckBox.CheckedChanged += new System.EventHandler(this.solutionCheckBox_CheckedChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label5.Location = new System.Drawing.Point(3, 156);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(79, 26);
			this.label5.TabIndex = 18;
			this.label5.Text = "Solution Name:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// solutionTextBox
			// 
			this.solutionTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.solutionTextBox.Location = new System.Drawing.Point(88, 159);
			this.solutionTextBox.Name = "solutionTextBox";
			this.solutionTextBox.Size = new System.Drawing.Size(411, 20);
			this.solutionTextBox.TabIndex = 19;
			this.solutionTextBox.TextChanged += new System.EventHandler(this.solutionTextBox_TextChanged);
			// 
			// settingsButton
			// 
			this.settingsButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.settingsButton.Location = new System.Drawing.Point(505, 435);
			this.settingsButton.Name = "settingsButton";
			this.settingsButton.Size = new System.Drawing.Size(75, 29);
			this.settingsButton.TabIndex = 20;
			this.settingsButton.Text = "Settings";
			this.settingsButton.UseVisualStyleBackColor = true;
			this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
			// 
			// buttonFlowLayoutPanel
			// 
			this.mainTableLayoutPanel.SetColumnSpan(this.buttonFlowLayoutPanel, 2);
			this.buttonFlowLayoutPanel.Controls.Add(this.generateButton);
			this.buttonFlowLayoutPanel.Controls.Add(this.saveConfigButton);
			this.buttonFlowLayoutPanel.Controls.Add(this.loadConfigButton);
			this.buttonFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.buttonFlowLayoutPanel.Location = new System.Drawing.Point(3, 435);
			this.buttonFlowLayoutPanel.Name = "buttonFlowLayoutPanel";
			this.buttonFlowLayoutPanel.Size = new System.Drawing.Size(496, 29);
			this.buttonFlowLayoutPanel.TabIndex = 21;
			// 
			// saveConfigButton
			// 
			this.saveConfigButton.AutoSize = true;
			this.saveConfigButton.Location = new System.Drawing.Point(98, 3);
			this.saveConfigButton.Name = "saveConfigButton";
			this.saveConfigButton.Size = new System.Drawing.Size(75, 23);
			this.saveConfigButton.TabIndex = 7;
			this.saveConfigButton.Text = "Save";
			this.saveConfigButton.UseVisualStyleBackColor = true;
			this.saveConfigButton.Click += new System.EventHandler(this.saveConfigButton_Click);
			// 
			// loadConfigButton
			// 
			this.loadConfigButton.Location = new System.Drawing.Point(179, 3);
			this.loadConfigButton.Name = "loadConfigButton";
			this.loadConfigButton.Size = new System.Drawing.Size(75, 23);
			this.loadConfigButton.TabIndex = 8;
			this.loadConfigButton.Text = "Load";
			this.loadConfigButton.UseVisualStyleBackColor = true;
			this.loadConfigButton.Click += new System.EventHandler(this.loadConfigButton_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(583, 467);
			this.Controls.Add(this.mainTableLayoutPanel);
			this.Name = "MainForm";
			this.Text = "XML to Data Class";
			this.mainTableLayoutPanel.ResumeLayout(false);
			this.mainTableLayoutPanel.PerformLayout();
			this.mainSplitContainer.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
			this.mainSplitContainer.ResumeLayout(false);
			this.elementGroupBox.ResumeLayout(false);
			this.buttonFlowLayoutPanel.ResumeLayout(false);
			this.buttonFlowLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.TextBox codeTextBox;
		private System.Windows.Forms.Button codeBrowseButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button generateButton;
		private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
		private System.Windows.Forms.SplitContainer mainSplitContainer;
		private System.Windows.Forms.GroupBox elementGroupBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox namespaceTextBox;
		private System.Windows.Forms.TreeView mainTreeView;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label xmlFilePathLabel;
		private System.Windows.Forms.Button loadButton;
		private System.Windows.Forms.CheckBox projectCheckBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox projectTextBox;
		private System.Windows.Forms.CheckBox solutionCheckBox;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox solutionTextBox;
		private System.Windows.Forms.Button settingsButton;
		private System.Windows.Forms.FlowLayoutPanel buttonFlowLayoutPanel;
		private System.Windows.Forms.Button saveConfigButton;
		private System.Windows.Forms.Button loadConfigButton;
	}
}

