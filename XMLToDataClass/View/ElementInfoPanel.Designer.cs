namespace XMLToDataClass.View
{
	partial class ElementInfoPanel
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
			this.dataTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.classNameTextBox = new System.Windows.Forms.TextBox();
			this.textCheckBox = new System.Windows.Forms.CheckBox();
			this.CDATACheckBox = new System.Windows.Forms.CheckBox();
			this.attributeGroupBox = new System.Windows.Forms.GroupBox();
			this.attributeListBox = new System.Windows.Forms.ListBox();
			this.elementsGroupBox = new System.Windows.Forms.GroupBox();
			this.elementListBox = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.classDescriptionTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.classRemarksTextBox = new System.Windows.Forms.TextBox();
			this.dataTableLayoutPanel.SuspendLayout();
			this.attributeGroupBox.SuspendLayout();
			this.elementsGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// dataTableLayoutPanel
			// 
			this.dataTableLayoutPanel.ColumnCount = 2;
			this.dataTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
			this.dataTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dataTableLayoutPanel.Controls.Add(this.label1, 0, 1);
			this.dataTableLayoutPanel.Controls.Add(this.classNameTextBox, 1, 0);
			this.dataTableLayoutPanel.Controls.Add(this.label2, 0, 0);
			this.dataTableLayoutPanel.Controls.Add(this.textCheckBox, 0, 4);
			this.dataTableLayoutPanel.Controls.Add(this.CDATACheckBox, 0, 5);
			this.dataTableLayoutPanel.Controls.Add(this.attributeGroupBox, 0, 6);
			this.dataTableLayoutPanel.Controls.Add(this.elementsGroupBox, 0, 7);
			this.dataTableLayoutPanel.Controls.Add(this.classDescriptionTextBox, 1, 1);
			this.dataTableLayoutPanel.Controls.Add(this.label3, 0, 2);
			this.dataTableLayoutPanel.Controls.Add(this.classRemarksTextBox, 1, 2);
			this.dataTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.dataTableLayoutPanel.Name = "dataTableLayoutPanel";
			this.dataTableLayoutPanel.RowCount = 8;
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.dataTableLayoutPanel.Size = new System.Drawing.Size(294, 250);
			this.dataTableLayoutPanel.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Location = new System.Drawing.Point(3, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(69, 26);
			this.label2.TabIndex = 0;
			this.label2.Text = "Class Name:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// classNameTextBox
			// 
			this.classNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.classNameTextBox.Location = new System.Drawing.Point(78, 3);
			this.classNameTextBox.Name = "classNameTextBox";
			this.classNameTextBox.Size = new System.Drawing.Size(213, 20);
			this.classNameTextBox.TabIndex = 1;
			this.classNameTextBox.TextChanged += new System.EventHandler(this.classNameTextBox_TextChanged);
			// 
			// textCheckBox
			// 
			this.textCheckBox.AutoSize = true;
			this.dataTableLayoutPanel.SetColumnSpan(this.textCheckBox, 2);
			this.textCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textCheckBox.Location = new System.Drawing.Point(3, 81);
			this.textCheckBox.Name = "textCheckBox";
			this.textCheckBox.Size = new System.Drawing.Size(288, 17);
			this.textCheckBox.TabIndex = 5;
			this.textCheckBox.Text = "Has Text Child Node";
			this.textCheckBox.UseVisualStyleBackColor = true;
			this.textCheckBox.CheckedChanged += new System.EventHandler(this.textCheckBox_CheckedChanged);
			// 
			// CDATACheckBox
			// 
			this.CDATACheckBox.AutoSize = true;
			this.dataTableLayoutPanel.SetColumnSpan(this.CDATACheckBox, 2);
			this.CDATACheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CDATACheckBox.Location = new System.Drawing.Point(3, 104);
			this.CDATACheckBox.Name = "CDATACheckBox";
			this.CDATACheckBox.Size = new System.Drawing.Size(288, 17);
			this.CDATACheckBox.TabIndex = 4;
			this.CDATACheckBox.Text = "Has CDATA Child";
			this.CDATACheckBox.UseVisualStyleBackColor = true;
			this.CDATACheckBox.CheckedChanged += new System.EventHandler(this.CDATACheckBox_CheckedChanged);
			// 
			// attributeGroupBox
			// 
			this.dataTableLayoutPanel.SetColumnSpan(this.attributeGroupBox, 2);
			this.attributeGroupBox.Controls.Add(this.attributeListBox);
			this.attributeGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.attributeGroupBox.Location = new System.Drawing.Point(3, 127);
			this.attributeGroupBox.Name = "attributeGroupBox";
			this.attributeGroupBox.Size = new System.Drawing.Size(288, 57);
			this.attributeGroupBox.TabIndex = 7;
			this.attributeGroupBox.TabStop = false;
			this.attributeGroupBox.Text = "Attributes";
			// 
			// attributeListBox
			// 
			this.attributeListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.attributeListBox.FormattingEnabled = true;
			this.attributeListBox.Location = new System.Drawing.Point(3, 16);
			this.attributeListBox.Name = "attributeListBox";
			this.attributeListBox.Size = new System.Drawing.Size(282, 38);
			this.attributeListBox.TabIndex = 0;
			// 
			// elementsGroupBox
			// 
			this.dataTableLayoutPanel.SetColumnSpan(this.elementsGroupBox, 2);
			this.elementsGroupBox.Controls.Add(this.elementListBox);
			this.elementsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.elementsGroupBox.Location = new System.Drawing.Point(3, 190);
			this.elementsGroupBox.Name = "elementsGroupBox";
			this.elementsGroupBox.Size = new System.Drawing.Size(288, 57);
			this.elementsGroupBox.TabIndex = 8;
			this.elementsGroupBox.TabStop = false;
			this.elementsGroupBox.Text = "Child Elements:";
			// 
			// elementListBox
			// 
			this.elementListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.elementListBox.FormattingEnabled = true;
			this.elementListBox.Location = new System.Drawing.Point(3, 16);
			this.elementListBox.Name = "elementListBox";
			this.elementListBox.Size = new System.Drawing.Size(282, 38);
			this.elementListBox.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(69, 26);
			this.label1.TabIndex = 2;
			this.label1.Text = "Summary:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// classDescriptionTextBox
			// 
			this.classDescriptionTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.classDescriptionTextBox.Location = new System.Drawing.Point(78, 29);
			this.classDescriptionTextBox.Name = "classDescriptionTextBox";
			this.classDescriptionTextBox.Size = new System.Drawing.Size(213, 20);
			this.classDescriptionTextBox.TabIndex = 9;
			this.classDescriptionTextBox.WordWrap = false;
			this.classDescriptionTextBox.TextChanged += new System.EventHandler(this.classDescriptionTextBox_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.Location = new System.Drawing.Point(3, 52);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(69, 26);
			this.label3.TabIndex = 10;
			this.label3.Text = "Remarks:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// classRemarksTextBox
			// 
			this.classRemarksTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.classRemarksTextBox.Location = new System.Drawing.Point(78, 55);
			this.classRemarksTextBox.Name = "classRemarksTextBox";
			this.classRemarksTextBox.Size = new System.Drawing.Size(213, 20);
			this.classRemarksTextBox.TabIndex = 11;
			this.classRemarksTextBox.TextChanged += new System.EventHandler(this.classRemarksTextBox_TextChanged);
			// 
			// ElementInfoPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dataTableLayoutPanel);
			this.MinimumSize = new System.Drawing.Size(294, 190);
			this.Name = "ElementInfoPanel";
			this.Size = new System.Drawing.Size(294, 250);
			this.dataTableLayoutPanel.ResumeLayout(false);
			this.dataTableLayoutPanel.PerformLayout();
			this.attributeGroupBox.ResumeLayout(false);
			this.elementsGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel dataTableLayoutPanel;
		private System.Windows.Forms.CheckBox CDATACheckBox;
		private System.Windows.Forms.CheckBox textCheckBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox classNameTextBox;
		private System.Windows.Forms.GroupBox attributeGroupBox;
		private System.Windows.Forms.ListBox attributeListBox;
		private System.Windows.Forms.GroupBox elementsGroupBox;
		private System.Windows.Forms.ListBox elementListBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox classDescriptionTextBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox classRemarksTextBox;
	}
}
