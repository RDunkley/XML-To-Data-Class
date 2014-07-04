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
			this.listView = new System.Windows.Forms.DataGridView();
			this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ClassName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.mainDataGridView = new System.Windows.Forms.DataGridView();
			this.attributeGroupBox = new System.Windows.Forms.GroupBox();
			this.CDATAGroupBox = new System.Windows.Forms.GroupBox();
			this.CDATACheckBox = new System.Windows.Forms.CheckBox();
			this.optionFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.textCheckBox = new System.Windows.Forms.CheckBox();
			this.CDATAOptionalCheckBox = new System.Windows.Forms.CheckBox();
			this.textGroupBox = new System.Windows.Forms.GroupBox();
			this.textFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.textOptionalCheckBox = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textDataComboBox = new System.Windows.Forms.ComboBox();
			this.elementGroupBox = new System.Windows.Forms.GroupBox();
			this.AttributeNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PropertyNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DataTypeColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.OptionalColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.emptyColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.mainTableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
			this.mainSplitContainer.Panel1.SuspendLayout();
			this.mainSplitContainer.Panel2.SuspendLayout();
			this.mainSplitContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.listView)).BeginInit();
			this.dataTableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mainDataGridView)).BeginInit();
			this.attributeGroupBox.SuspendLayout();
			this.CDATAGroupBox.SuspendLayout();
			this.optionFlowPanel.SuspendLayout();
			this.textGroupBox.SuspendLayout();
			this.textFlowLayoutPanel.SuspendLayout();
			this.elementGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// codeTextBox
			// 
			this.codeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.codeTextBox.Location = new System.Drawing.Point(84, 3);
			this.codeTextBox.Name = "codeTextBox";
			this.codeTextBox.Size = new System.Drawing.Size(415, 20);
			this.codeTextBox.TabIndex = 2;
			this.codeTextBox.Text = "D:\\Cloudstation\\VSProjects\\TotalEmu\\Example\\Example\\Data";
			// 
			// codeBrowseButton
			// 
			this.codeBrowseButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.codeBrowseButton.Location = new System.Drawing.Point(505, 3);
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
			this.label2.Location = new System.Drawing.Point(3, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(75, 29);
			this.label2.TabIndex = 5;
			this.label2.Text = "Code Folder:";
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
			this.mainTableLayoutPanel.Controls.Add(this.label2, 0, 0);
			this.mainTableLayoutPanel.Controls.Add(this.processButton, 0, 2);
			this.mainTableLayoutPanel.Controls.Add(this.codeBrowseButton, 2, 0);
			this.mainTableLayoutPanel.Controls.Add(this.codeTextBox, 1, 0);
			this.mainTableLayoutPanel.Controls.Add(this.mainSplitContainer, 0, 1);
			this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
			this.mainTableLayoutPanel.RowCount = 3;
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.mainTableLayoutPanel.Size = new System.Drawing.Size(583, 467);
			this.mainTableLayoutPanel.TabIndex = 7;
			// 
			// mainSplitContainer
			// 
			this.mainTableLayoutPanel.SetColumnSpan(this.mainSplitContainer, 3);
			this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainSplitContainer.Location = new System.Drawing.Point(3, 32);
			this.mainSplitContainer.Name = "mainSplitContainer";
			// 
			// mainSplitContainer.Panel1
			// 
			this.mainSplitContainer.Panel1.Controls.Add(this.elementGroupBox);
			// 
			// mainSplitContainer.Panel2
			// 
			this.mainSplitContainer.Panel2.Controls.Add(this.dataTableLayoutPanel);
			this.mainSplitContainer.Size = new System.Drawing.Size(577, 403);
			this.mainSplitContainer.SplitterDistance = 191;
			this.mainSplitContainer.TabIndex = 7;
			// 
			// listView
			// 
			this.listView.AllowUserToAddRows = false;
			this.listView.AllowUserToDeleteRows = false;
			this.listView.AllowUserToOrderColumns = true;
			this.listView.AllowUserToResizeRows = false;
			this.listView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.listView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.listView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameColumn,
            this.ClassName});
			this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView.Location = new System.Drawing.Point(3, 16);
			this.listView.MultiSelect = false;
			this.listView.Name = "listView";
			this.listView.RowHeadersVisible = false;
			this.listView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.listView.Size = new System.Drawing.Size(185, 384);
			this.listView.TabIndex = 2;
			this.listView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.listView_CellValueChanged);
			this.listView.SelectionChanged += new System.EventHandler(this.listView_SelectionChanged);
			// 
			// NameColumn
			// 
			this.NameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.NameColumn.HeaderText = "Name";
			this.NameColumn.Name = "NameColumn";
			this.NameColumn.ReadOnly = true;
			this.NameColumn.ToolTipText = "Name of the XML Element";
			this.NameColumn.Width = 60;
			// 
			// ClassName
			// 
			this.ClassName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.ClassName.HeaderText = "Data Class Name";
			this.ClassName.Name = "ClassName";
			this.ClassName.ToolTipText = "Name of the data Class that will be created.";
			// 
			// dataTableLayoutPanel
			// 
			this.dataTableLayoutPanel.ColumnCount = 1;
			this.dataTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dataTableLayoutPanel.Controls.Add(this.attributeGroupBox, 0, 1);
			this.dataTableLayoutPanel.Controls.Add(this.CDATAGroupBox, 0, 2);
			this.dataTableLayoutPanel.Controls.Add(this.optionFlowPanel, 0, 0);
			this.dataTableLayoutPanel.Controls.Add(this.textGroupBox, 0, 3);
			this.dataTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.dataTableLayoutPanel.Name = "dataTableLayoutPanel";
			this.dataTableLayoutPanel.RowCount = 4;
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.dataTableLayoutPanel.Size = new System.Drawing.Size(382, 403);
			this.dataTableLayoutPanel.TabIndex = 1;
			// 
			// mainDataGridView
			// 
			this.mainDataGridView.AllowUserToAddRows = false;
			this.mainDataGridView.AllowUserToDeleteRows = false;
			this.mainDataGridView.AllowUserToOrderColumns = true;
			this.mainDataGridView.AllowUserToResizeRows = false;
			this.mainDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.mainDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.mainDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AttributeNameColumn,
            this.PropertyNameColumn,
            this.DataTypeColumn,
            this.OptionalColumn,
            this.emptyColumn});
			this.mainDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
			this.mainDataGridView.Location = new System.Drawing.Point(3, 16);
			this.mainDataGridView.Name = "mainDataGridView";
			this.mainDataGridView.RowHeadersVisible = false;
			this.mainDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.mainDataGridView.Size = new System.Drawing.Size(370, 249);
			this.mainDataGridView.TabIndex = 0;
			// 
			// attributeGroupBox
			// 
			this.attributeGroupBox.Controls.Add(this.mainDataGridView);
			this.attributeGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.attributeGroupBox.Location = new System.Drawing.Point(3, 32);
			this.attributeGroupBox.Name = "attributeGroupBox";
			this.attributeGroupBox.Size = new System.Drawing.Size(376, 268);
			this.attributeGroupBox.TabIndex = 2;
			this.attributeGroupBox.TabStop = false;
			this.attributeGroupBox.Text = "Attributes";
			// 
			// CDATAGroupBox
			// 
			this.CDATAGroupBox.Controls.Add(this.CDATAOptionalCheckBox);
			this.CDATAGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CDATAGroupBox.Location = new System.Drawing.Point(3, 306);
			this.CDATAGroupBox.Name = "CDATAGroupBox";
			this.CDATAGroupBox.Size = new System.Drawing.Size(376, 44);
			this.CDATAGroupBox.TabIndex = 3;
			this.CDATAGroupBox.TabStop = false;
			this.CDATAGroupBox.Text = "CDATA";
			// 
			// CDATACheckBox
			// 
			this.CDATACheckBox.AutoSize = true;
			this.CDATACheckBox.Location = new System.Drawing.Point(3, 3);
			this.CDATACheckBox.Name = "CDATACheckBox";
			this.CDATACheckBox.Size = new System.Drawing.Size(110, 17);
			this.CDATACheckBox.TabIndex = 4;
			this.CDATACheckBox.Text = "Has CDATA Child";
			this.CDATACheckBox.UseVisualStyleBackColor = true;
			this.CDATACheckBox.CheckedChanged += new System.EventHandler(this.CDATACheckBox_CheckedChanged);
			// 
			// optionFlowPanel
			// 
			this.optionFlowPanel.Controls.Add(this.CDATACheckBox);
			this.optionFlowPanel.Controls.Add(this.textCheckBox);
			this.optionFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.optionFlowPanel.Location = new System.Drawing.Point(3, 3);
			this.optionFlowPanel.Name = "optionFlowPanel";
			this.optionFlowPanel.Size = new System.Drawing.Size(376, 23);
			this.optionFlowPanel.TabIndex = 4;
			// 
			// textCheckBox
			// 
			this.textCheckBox.AutoSize = true;
			this.textCheckBox.Location = new System.Drawing.Point(119, 3);
			this.textCheckBox.Name = "textCheckBox";
			this.textCheckBox.Size = new System.Drawing.Size(95, 17);
			this.textCheckBox.TabIndex = 5;
			this.textCheckBox.Text = "Has Text Child";
			this.textCheckBox.UseVisualStyleBackColor = true;
			this.textCheckBox.CheckedChanged += new System.EventHandler(this.textCheckBox_CheckedChanged);
			// 
			// CDATAOptionalCheckBox
			// 
			this.CDATAOptionalCheckBox.AutoSize = true;
			this.CDATAOptionalCheckBox.Location = new System.Drawing.Point(6, 19);
			this.CDATAOptionalCheckBox.Name = "CDATAOptionalCheckBox";
			this.CDATAOptionalCheckBox.Size = new System.Drawing.Size(76, 17);
			this.CDATAOptionalCheckBox.TabIndex = 0;
			this.CDATAOptionalCheckBox.Text = "Is Optional";
			this.CDATAOptionalCheckBox.UseVisualStyleBackColor = true;
			this.CDATAOptionalCheckBox.CheckedChanged += new System.EventHandler(this.CDATAOptionalCheckBox_CheckedChanged);
			// 
			// textGroupBox
			// 
			this.textGroupBox.Controls.Add(this.textFlowLayoutPanel);
			this.textGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textGroupBox.Location = new System.Drawing.Point(3, 356);
			this.textGroupBox.Name = "textGroupBox";
			this.textGroupBox.Size = new System.Drawing.Size(376, 44);
			this.textGroupBox.TabIndex = 5;
			this.textGroupBox.TabStop = false;
			this.textGroupBox.Text = "Text";
			// 
			// textFlowLayoutPanel
			// 
			this.textFlowLayoutPanel.Controls.Add(this.textOptionalCheckBox);
			this.textFlowLayoutPanel.Controls.Add(this.label1);
			this.textFlowLayoutPanel.Controls.Add(this.textDataComboBox);
			this.textFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textFlowLayoutPanel.Location = new System.Drawing.Point(3, 16);
			this.textFlowLayoutPanel.Name = "textFlowLayoutPanel";
			this.textFlowLayoutPanel.Size = new System.Drawing.Size(370, 25);
			this.textFlowLayoutPanel.TabIndex = 0;
			// 
			// textOptionalCheckBox
			// 
			this.textOptionalCheckBox.AutoSize = true;
			this.textOptionalCheckBox.Location = new System.Drawing.Point(3, 3);
			this.textOptionalCheckBox.Name = "textOptionalCheckBox";
			this.textOptionalCheckBox.Size = new System.Drawing.Size(76, 17);
			this.textOptionalCheckBox.TabIndex = 0;
			this.textOptionalCheckBox.Text = "Is Optional";
			this.textOptionalCheckBox.UseVisualStyleBackColor = true;
			this.textOptionalCheckBox.CheckedChanged += new System.EventHandler(this.textOptionalCheckBox_CheckedChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(85, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(60, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "Data Type:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDataComboBox
			// 
			this.textDataComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textDataComboBox.FormattingEnabled = true;
			this.textDataComboBox.Location = new System.Drawing.Point(151, 3);
			this.textDataComboBox.Name = "textDataComboBox";
			this.textDataComboBox.Size = new System.Drawing.Size(121, 21);
			this.textDataComboBox.TabIndex = 2;
			this.textDataComboBox.SelectedIndexChanged += new System.EventHandler(this.textDataComboBox_SelectedIndexChanged);
			// 
			// elementGroupBox
			// 
			this.elementGroupBox.Controls.Add(this.listView);
			this.elementGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.elementGroupBox.Location = new System.Drawing.Point(0, 0);
			this.elementGroupBox.Name = "elementGroupBox";
			this.elementGroupBox.Size = new System.Drawing.Size(191, 403);
			this.elementGroupBox.TabIndex = 0;
			this.elementGroupBox.TabStop = false;
			this.elementGroupBox.Text = "Unique XML Elements";
			// 
			// AttributeNameColumn
			// 
			this.AttributeNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.AttributeNameColumn.HeaderText = "Name";
			this.AttributeNameColumn.Name = "AttributeNameColumn";
			this.AttributeNameColumn.ReadOnly = true;
			this.AttributeNameColumn.ToolTipText = "Name of the XML Attribute or CDATA if it represents a child CDATA element, or Tex" +
    "t if it represents text inside the XML element.";
			// 
			// PropertyNameColumn
			// 
			this.PropertyNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.PropertyNameColumn.HeaderText = "Property Name";
			this.PropertyNameColumn.Name = "PropertyNameColumn";
			this.PropertyNameColumn.ToolTipText = "Name of the property that will be created to store the data.";
			// 
			// DataTypeColumn
			// 
			this.DataTypeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.DataTypeColumn.HeaderText = "Data Type";
			this.DataTypeColumn.Name = "DataTypeColumn";
			this.DataTypeColumn.ToolTipText = "Type of data that the attribute can be.";
			this.DataTypeColumn.Width = 63;
			// 
			// OptionalColumn
			// 
			this.OptionalColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.OptionalColumn.HeaderText = "Is Optional";
			this.OptionalColumn.Name = "OptionalColumn";
			this.OptionalColumn.ToolTipText = "Uncheck if the data component must be specified in each element";
			this.OptionalColumn.Width = 63;
			// 
			// emptyColumn
			// 
			this.emptyColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.emptyColumn.HeaderText = "Can Be Empty";
			this.emptyColumn.Name = "emptyColumn";
			this.emptyColumn.ToolTipText = "If the Data Type is a string then this specifies whether the string can be empty " +
    "or not";
			this.emptyColumn.Width = 72;
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
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.mainTableLayoutPanel.ResumeLayout(false);
			this.mainTableLayoutPanel.PerformLayout();
			this.mainSplitContainer.Panel1.ResumeLayout(false);
			this.mainSplitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
			this.mainSplitContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.listView)).EndInit();
			this.dataTableLayoutPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.mainDataGridView)).EndInit();
			this.attributeGroupBox.ResumeLayout(false);
			this.CDATAGroupBox.ResumeLayout(false);
			this.CDATAGroupBox.PerformLayout();
			this.optionFlowPanel.ResumeLayout(false);
			this.optionFlowPanel.PerformLayout();
			this.textGroupBox.ResumeLayout(false);
			this.textFlowLayoutPanel.ResumeLayout(false);
			this.textFlowLayoutPanel.PerformLayout();
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
		private System.Windows.Forms.TableLayoutPanel dataTableLayoutPanel;
		private System.Windows.Forms.DataGridView mainDataGridView;
		private System.Windows.Forms.DataGridView listView;
		private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn ClassName;
		private System.Windows.Forms.GroupBox attributeGroupBox;
		private System.Windows.Forms.GroupBox CDATAGroupBox;
		private System.Windows.Forms.FlowLayoutPanel optionFlowPanel;
		private System.Windows.Forms.CheckBox CDATACheckBox;
		private System.Windows.Forms.CheckBox textCheckBox;
		private System.Windows.Forms.CheckBox CDATAOptionalCheckBox;
		private System.Windows.Forms.GroupBox textGroupBox;
		private System.Windows.Forms.FlowLayoutPanel textFlowLayoutPanel;
		private System.Windows.Forms.CheckBox textOptionalCheckBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox textDataComboBox;
		private System.Windows.Forms.GroupBox elementGroupBox;
		private System.Windows.Forms.DataGridViewTextBoxColumn AttributeNameColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn PropertyNameColumn;
		private System.Windows.Forms.DataGridViewComboBoxColumn DataTypeColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn OptionalColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn emptyColumn;
    }
}

