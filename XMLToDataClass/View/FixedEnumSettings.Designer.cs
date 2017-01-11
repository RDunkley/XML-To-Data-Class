using System;

namespace XMLToDataClass.View
{
	partial class FixedEnumSettings<T> where T : struct, IConvertible
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.valuesCheckBox = new System.Windows.Forms.CheckBox();
			this.ignoreCheckBox = new System.Windows.Forms.CheckBox();
			this.mainDataGridView = new System.Windows.Forms.DataGridView();
			this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.mainTableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mainDataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// mainTableLayoutPanel
			// 
			this.mainTableLayoutPanel.ColumnCount = 1;
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.mainTableLayoutPanel.Controls.Add(this.valuesCheckBox, 0, 0);
			this.mainTableLayoutPanel.Controls.Add(this.ignoreCheckBox, 0, 1);
			this.mainTableLayoutPanel.Controls.Add(this.mainDataGridView, 0, 2);
			this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
			this.mainTableLayoutPanel.RowCount = 3;
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.mainTableLayoutPanel.Size = new System.Drawing.Size(224, 164);
			this.mainTableLayoutPanel.TabIndex = 0;
			// 
			// valuesCheckBox
			// 
			this.valuesCheckBox.AutoSize = true;
			this.valuesCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.valuesCheckBox.Location = new System.Drawing.Point(3, 3);
			this.valuesCheckBox.Name = "valuesCheckBox";
			this.valuesCheckBox.Size = new System.Drawing.Size(218, 17);
			this.valuesCheckBox.TabIndex = 0;
			this.valuesCheckBox.Text = "Allow values as well as names";
			this.valuesCheckBox.UseVisualStyleBackColor = true;
			this.valuesCheckBox.CheckedChanged += new System.EventHandler(this.valuesCheckBox_CheckedChanged);
			// 
			// ignoreCheckBox
			// 
			this.ignoreCheckBox.AutoSize = true;
			this.ignoreCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ignoreCheckBox.Location = new System.Drawing.Point(3, 26);
			this.ignoreCheckBox.Name = "ignoreCheckBox";
			this.ignoreCheckBox.Size = new System.Drawing.Size(218, 17);
			this.ignoreCheckBox.TabIndex = 1;
			this.ignoreCheckBox.Text = "Ignore Case for Names";
			this.ignoreCheckBox.UseVisualStyleBackColor = true;
			this.ignoreCheckBox.CheckedChanged += new System.EventHandler(this.ignoreCheckBox_CheckedChanged);
			// 
			// mainDataGridView
			// 
			this.mainDataGridView.AllowUserToAddRows = false;
			this.mainDataGridView.AllowUserToDeleteRows = false;
			this.mainDataGridView.AllowUserToResizeRows = false;
			this.mainDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.mainDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameColumn,
            this.valueColumn});
			this.mainDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.mainDataGridView.Location = new System.Drawing.Point(3, 49);
			this.mainDataGridView.MultiSelect = false;
			this.mainDataGridView.Name = "mainDataGridView";
			this.mainDataGridView.ReadOnly = true;
			this.mainDataGridView.RowHeadersVisible = false;
			this.mainDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.mainDataGridView.ShowCellErrors = false;
			this.mainDataGridView.ShowEditingIcon = false;
			this.mainDataGridView.ShowRowErrors = false;
			this.mainDataGridView.Size = new System.Drawing.Size(218, 112);
			this.mainDataGridView.TabIndex = 2;
			// 
			// nameColumn
			// 
			this.nameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.nameColumn.HeaderText = "Name";
			this.nameColumn.Name = "nameColumn";
			this.nameColumn.ReadOnly = true;
			this.nameColumn.ToolTipText = "Name of the enumerated item";
			this.nameColumn.Width = 60;
			// 
			// valueColumn
			// 
			this.valueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.valueColumn.HeaderText = "Value";
			this.valueColumn.Name = "valueColumn";
			this.valueColumn.ReadOnly = true;
			this.valueColumn.ToolTipText = "Value of the enumerated item.";
			// 
			// FixedEnumSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.mainTableLayoutPanel);
			this.MinimumSize = new System.Drawing.Size(224, 164);
			this.Name = "FixedEnumSettings";
			this.Size = new System.Drawing.Size(224, 164);
			this.mainTableLayoutPanel.ResumeLayout(false);
			this.mainTableLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.mainDataGridView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
		private System.Windows.Forms.CheckBox valuesCheckBox;
		private System.Windows.Forms.CheckBox ignoreCheckBox;
		private System.Windows.Forms.DataGridView mainDataGridView;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn valueColumn;
	}
}
