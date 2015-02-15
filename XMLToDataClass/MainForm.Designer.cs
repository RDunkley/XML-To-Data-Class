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
			this.processButton = new System.Windows.Forms.Button();
			this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
			this.elementGroupBox = new System.Windows.Forms.GroupBox();
			this.mainTreeView = new System.Windows.Forms.TreeView();
			this.label1 = new System.Windows.Forms.Label();
			this.xmlFilePathLabel = new System.Windows.Forms.Label();
			this.loadButton = new System.Windows.Forms.Button();
			this.namespaceTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.mainTableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
			this.mainSplitContainer.Panel1.SuspendLayout();
			this.mainSplitContainer.SuspendLayout();
			this.elementGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// codeTextBox
			// 
			this.codeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.codeTextBox.Location = new System.Drawing.Point(84, 32);
			this.codeTextBox.Name = "codeTextBox";
			this.codeTextBox.Size = new System.Drawing.Size(415, 20);
			this.codeTextBox.TabIndex = 2;
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
			this.label2.Size = new System.Drawing.Size(75, 29);
			this.label2.TabIndex = 5;
			this.label2.Text = "Output Folder:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// processButton
			// 
			this.processButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.processButton.Location = new System.Drawing.Point(3, 441);
			this.processButton.Name = "processButton";
			this.processButton.Size = new System.Drawing.Size(75, 23);
			this.processButton.TabIndex = 6;
			this.processButton.Text = "Process";
			this.processButton.UseVisualStyleBackColor = true;
			this.processButton.Click += new System.EventHandler(this.processButton_Click);
			// 
			// mainTableLayoutPanel
			// 
			this.mainTableLayoutPanel.ColumnCount = 3;
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
			this.mainTableLayoutPanel.Controls.Add(this.processButton, 0, 4);
			this.mainTableLayoutPanel.Controls.Add(this.mainSplitContainer, 0, 3);
			this.mainTableLayoutPanel.Controls.Add(this.label1, 0, 0);
			this.mainTableLayoutPanel.Controls.Add(this.xmlFilePathLabel, 1, 0);
			this.mainTableLayoutPanel.Controls.Add(this.loadButton, 2, 0);
			this.mainTableLayoutPanel.Controls.Add(this.codeBrowseButton, 2, 1);
			this.mainTableLayoutPanel.Controls.Add(this.codeTextBox, 1, 1);
			this.mainTableLayoutPanel.Controls.Add(this.namespaceTextBox, 1, 2);
			this.mainTableLayoutPanel.Controls.Add(this.label3, 0, 2);
			this.mainTableLayoutPanel.Controls.Add(this.label2, 0, 1);
			this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
			this.mainTableLayoutPanel.RowCount = 5;
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
			this.mainTableLayoutPanel.Size = new System.Drawing.Size(583, 467);
			this.mainTableLayoutPanel.TabIndex = 7;
			// 
			// mainSplitContainer
			// 
			this.mainTableLayoutPanel.SetColumnSpan(this.mainSplitContainer, 3);
			this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainSplitContainer.Location = new System.Drawing.Point(3, 90);
			this.mainSplitContainer.Name = "mainSplitContainer";
			// 
			// mainSplitContainer.Panel1
			// 
			this.mainSplitContainer.Panel1.Controls.Add(this.elementGroupBox);
			this.mainSplitContainer.Size = new System.Drawing.Size(577, 345);
			this.mainSplitContainer.SplitterDistance = 191;
			this.mainSplitContainer.TabIndex = 7;
			// 
			// elementGroupBox
			// 
			this.elementGroupBox.Controls.Add(this.mainTreeView);
			this.elementGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.elementGroupBox.Location = new System.Drawing.Point(0, 0);
			this.elementGroupBox.Name = "elementGroupBox";
			this.elementGroupBox.Size = new System.Drawing.Size(191, 345);
			this.elementGroupBox.TabIndex = 0;
			this.elementGroupBox.TabStop = false;
			this.elementGroupBox.Text = "Elements and Attributes";
			// 
			// mainTreeView
			// 
			this.mainTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTreeView.Location = new System.Drawing.Point(3, 16);
			this.mainTreeView.Name = "mainTreeView";
			this.mainTreeView.Size = new System.Drawing.Size(185, 326);
			this.mainTreeView.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(75, 29);
			this.label1.TabIndex = 10;
			this.label1.Text = "XML File:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// xmlFilePathLabel
			// 
			this.xmlFilePathLabel.AutoSize = true;
			this.xmlFilePathLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.xmlFilePathLabel.Location = new System.Drawing.Point(84, 0);
			this.xmlFilePathLabel.Name = "xmlFilePathLabel";
			this.xmlFilePathLabel.Size = new System.Drawing.Size(415, 29);
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
			this.namespaceTextBox.Location = new System.Drawing.Point(84, 61);
			this.namespaceTextBox.Name = "namespaceTextBox";
			this.namespaceTextBox.Size = new System.Drawing.Size(415, 20);
			this.namespaceTextBox.TabIndex = 9;
			this.namespaceTextBox.Text = "XMLToDataClass";
			this.namespaceTextBox.WordWrap = false;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.Location = new System.Drawing.Point(3, 58);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(75, 29);
			this.label3.TabIndex = 8;
			this.label3.Text = "Namespace:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(583, 467);
			this.Controls.Add(this.mainTableLayoutPanel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.Text = "XML to Data Class";
			this.mainTableLayoutPanel.ResumeLayout(false);
			this.mainTableLayoutPanel.PerformLayout();
			this.mainSplitContainer.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
			this.mainSplitContainer.ResumeLayout(false);
			this.elementGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.TextBox codeTextBox;
		private System.Windows.Forms.Button codeBrowseButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button processButton;
		private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
		private System.Windows.Forms.SplitContainer mainSplitContainer;
		private System.Windows.Forms.GroupBox elementGroupBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox namespaceTextBox;
		private System.Windows.Forms.TreeView mainTreeView;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label xmlFilePathLabel;
		private System.Windows.Forms.Button loadButton;
    }
}

