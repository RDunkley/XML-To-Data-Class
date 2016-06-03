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
			this.nameTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.classNameTextBox = new System.Windows.Forms.TextBox();
			this.textCheckBox = new System.Windows.Forms.CheckBox();
			this.CDATACheckBox = new System.Windows.Forms.CheckBox();
			this.attributeGroupBox = new System.Windows.Forms.GroupBox();
			this.attributeListBox = new System.Windows.Forms.ListBox();
			this.elementsGroupBox = new System.Windows.Forms.GroupBox();
			this.elementListBox = new System.Windows.Forms.ListBox();
			this.dataTableLayoutPanel.SuspendLayout();
			this.nameTableLayoutPanel.SuspendLayout();
			this.attributeGroupBox.SuspendLayout();
			this.elementsGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// dataTableLayoutPanel
			// 
			this.dataTableLayoutPanel.ColumnCount = 1;
			this.dataTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dataTableLayoutPanel.Controls.Add(this.nameTableLayoutPanel, 0, 0);
			this.dataTableLayoutPanel.Controls.Add(this.textCheckBox, 0, 1);
			this.dataTableLayoutPanel.Controls.Add(this.CDATACheckBox, 0, 2);
			this.dataTableLayoutPanel.Controls.Add(this.attributeGroupBox, 0, 3);
			this.dataTableLayoutPanel.Controls.Add(this.elementsGroupBox, 0, 4);
			this.dataTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.dataTableLayoutPanel.Name = "dataTableLayoutPanel";
			this.dataTableLayoutPanel.RowCount = 5;
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.dataTableLayoutPanel.Size = new System.Drawing.Size(382, 374);
			this.dataTableLayoutPanel.TabIndex = 2;
			// 
			// nameTableLayoutPanel
			// 
			this.nameTableLayoutPanel.ColumnCount = 2;
			this.nameTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72F));
			this.nameTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.nameTableLayoutPanel.Controls.Add(this.label2, 0, 0);
			this.nameTableLayoutPanel.Controls.Add(this.classNameTextBox, 1, 0);
			this.nameTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nameTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
			this.nameTableLayoutPanel.Name = "nameTableLayoutPanel";
			this.nameTableLayoutPanel.RowCount = 1;
			this.nameTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.nameTableLayoutPanel.Size = new System.Drawing.Size(376, 29);
			this.nameTableLayoutPanel.TabIndex = 6;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Location = new System.Drawing.Point(3, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(66, 29);
			this.label2.TabIndex = 0;
			this.label2.Text = "Class Name:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// classNameTextBox
			// 
			this.classNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.classNameTextBox.Location = new System.Drawing.Point(75, 3);
			this.classNameTextBox.Name = "classNameTextBox";
			this.classNameTextBox.Size = new System.Drawing.Size(298, 20);
			this.classNameTextBox.TabIndex = 1;
			this.classNameTextBox.TextChanged += new System.EventHandler(this.classNameTextBox_TextChanged);
			// 
			// textCheckBox
			// 
			this.textCheckBox.AutoSize = true;
			this.textCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textCheckBox.Location = new System.Drawing.Point(3, 38);
			this.textCheckBox.Name = "textCheckBox";
			this.textCheckBox.Size = new System.Drawing.Size(376, 17);
			this.textCheckBox.TabIndex = 5;
			this.textCheckBox.Text = "Has Text Child Node";
			this.textCheckBox.UseVisualStyleBackColor = true;
			this.textCheckBox.CheckedChanged += new System.EventHandler(this.textCheckBox_CheckedChanged);
			// 
			// CDATACheckBox
			// 
			this.CDATACheckBox.AutoSize = true;
			this.CDATACheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CDATACheckBox.Location = new System.Drawing.Point(3, 61);
			this.CDATACheckBox.Name = "CDATACheckBox";
			this.CDATACheckBox.Size = new System.Drawing.Size(376, 17);
			this.CDATACheckBox.TabIndex = 4;
			this.CDATACheckBox.Text = "Has CDATA Child";
			this.CDATACheckBox.UseVisualStyleBackColor = true;
			this.CDATACheckBox.CheckedChanged += new System.EventHandler(this.CDATACheckBox_CheckedChanged);
			// 
			// attributeGroupBox
			// 
			this.attributeGroupBox.Controls.Add(this.attributeListBox);
			this.attributeGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.attributeGroupBox.Location = new System.Drawing.Point(3, 84);
			this.attributeGroupBox.Name = "attributeGroupBox";
			this.attributeGroupBox.Size = new System.Drawing.Size(376, 140);
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
			this.attributeListBox.Size = new System.Drawing.Size(370, 121);
			this.attributeListBox.TabIndex = 0;
			// 
			// elementsGroupBox
			// 
			this.elementsGroupBox.Controls.Add(this.elementListBox);
			this.elementsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.elementsGroupBox.Location = new System.Drawing.Point(3, 230);
			this.elementsGroupBox.Name = "elementsGroupBox";
			this.elementsGroupBox.Size = new System.Drawing.Size(376, 141);
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
			this.elementListBox.Size = new System.Drawing.Size(370, 122);
			this.elementListBox.TabIndex = 0;
			// 
			// ElementInfoPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dataTableLayoutPanel);
			this.Name = "ElementInfoPanel";
			this.Size = new System.Drawing.Size(382, 374);
			this.dataTableLayoutPanel.ResumeLayout(false);
			this.dataTableLayoutPanel.PerformLayout();
			this.nameTableLayoutPanel.ResumeLayout(false);
			this.nameTableLayoutPanel.PerformLayout();
			this.attributeGroupBox.ResumeLayout(false);
			this.elementsGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel dataTableLayoutPanel;
		private System.Windows.Forms.CheckBox CDATACheckBox;
		private System.Windows.Forms.CheckBox textCheckBox;
		private System.Windows.Forms.TableLayoutPanel nameTableLayoutPanel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox classNameTextBox;
		private System.Windows.Forms.GroupBox attributeGroupBox;
		private System.Windows.Forms.ListBox attributeListBox;
		private System.Windows.Forms.GroupBox elementsGroupBox;
		private System.Windows.Forms.ListBox elementListBox;
	}
}
