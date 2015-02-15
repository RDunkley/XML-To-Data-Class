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
			this.CDATAGroupBox = new System.Windows.Forms.GroupBox();
			this.CDATAOptionalCheckBox = new System.Windows.Forms.CheckBox();
			this.optionFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.CDATACheckBox = new System.Windows.Forms.CheckBox();
			this.textCheckBox = new System.Windows.Forms.CheckBox();
			this.textGroupBox = new System.Windows.Forms.GroupBox();
			this.textFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.textOptionalCheckBox = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.dataTypeComboBox = new System.Windows.Forms.ComboBox();
			this.nameTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.classNameTextBox = new System.Windows.Forms.TextBox();
			this.dataTableLayoutPanel.SuspendLayout();
			this.CDATAGroupBox.SuspendLayout();
			this.optionFlowPanel.SuspendLayout();
			this.textGroupBox.SuspendLayout();
			this.textFlowLayoutPanel.SuspendLayout();
			this.nameTableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// dataTableLayoutPanel
			// 
			this.dataTableLayoutPanel.ColumnCount = 1;
			this.dataTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dataTableLayoutPanel.Controls.Add(this.CDATAGroupBox, 0, 2);
			this.dataTableLayoutPanel.Controls.Add(this.optionFlowPanel, 0, 1);
			this.dataTableLayoutPanel.Controls.Add(this.textGroupBox, 0, 3);
			this.dataTableLayoutPanel.Controls.Add(this.nameTableLayoutPanel, 0, 0);
			this.dataTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.dataTableLayoutPanel.Name = "dataTableLayoutPanel";
			this.dataTableLayoutPanel.RowCount = 5;
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dataTableLayoutPanel.Size = new System.Drawing.Size(382, 374);
			this.dataTableLayoutPanel.TabIndex = 2;
			// 
			// CDATAGroupBox
			// 
			this.CDATAGroupBox.Controls.Add(this.CDATAOptionalCheckBox);
			this.CDATAGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CDATAGroupBox.Location = new System.Drawing.Point(3, 67);
			this.CDATAGroupBox.Name = "CDATAGroupBox";
			this.CDATAGroupBox.Size = new System.Drawing.Size(376, 44);
			this.CDATAGroupBox.TabIndex = 3;
			this.CDATAGroupBox.TabStop = false;
			this.CDATAGroupBox.Text = "CDATA";
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
			// optionFlowPanel
			// 
			this.optionFlowPanel.Controls.Add(this.CDATACheckBox);
			this.optionFlowPanel.Controls.Add(this.textCheckBox);
			this.optionFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.optionFlowPanel.Location = new System.Drawing.Point(3, 38);
			this.optionFlowPanel.Name = "optionFlowPanel";
			this.optionFlowPanel.Size = new System.Drawing.Size(376, 23);
			this.optionFlowPanel.TabIndex = 4;
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
			// textGroupBox
			// 
			this.textGroupBox.Controls.Add(this.textFlowLayoutPanel);
			this.textGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textGroupBox.Location = new System.Drawing.Point(3, 117);
			this.textGroupBox.Name = "textGroupBox";
			this.textGroupBox.Size = new System.Drawing.Size(376, 47);
			this.textGroupBox.TabIndex = 5;
			this.textGroupBox.TabStop = false;
			this.textGroupBox.Text = "Text";
			// 
			// textFlowLayoutPanel
			// 
			this.textFlowLayoutPanel.Controls.Add(this.textOptionalCheckBox);
			this.textFlowLayoutPanel.Controls.Add(this.label1);
			this.textFlowLayoutPanel.Controls.Add(this.dataTypeComboBox);
			this.textFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textFlowLayoutPanel.Location = new System.Drawing.Point(3, 16);
			this.textFlowLayoutPanel.Name = "textFlowLayoutPanel";
			this.textFlowLayoutPanel.Size = new System.Drawing.Size(370, 28);
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
			// dataTypeComboBox
			// 
			this.dataTypeComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.dataTypeComboBox.FormattingEnabled = true;
			this.dataTypeComboBox.Location = new System.Drawing.Point(151, 3);
			this.dataTypeComboBox.Name = "dataTypeComboBox";
			this.dataTypeComboBox.Size = new System.Drawing.Size(121, 21);
			this.dataTypeComboBox.TabIndex = 2;
			this.dataTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.dataTypeComboBox_SelectedIndexChanged);
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
			// ElementInfoPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dataTableLayoutPanel);
			this.Name = "ElementInfoPanel";
			this.Size = new System.Drawing.Size(382, 374);
			this.dataTableLayoutPanel.ResumeLayout(false);
			this.CDATAGroupBox.ResumeLayout(false);
			this.CDATAGroupBox.PerformLayout();
			this.optionFlowPanel.ResumeLayout(false);
			this.optionFlowPanel.PerformLayout();
			this.textGroupBox.ResumeLayout(false);
			this.textFlowLayoutPanel.ResumeLayout(false);
			this.textFlowLayoutPanel.PerformLayout();
			this.nameTableLayoutPanel.ResumeLayout(false);
			this.nameTableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel dataTableLayoutPanel;
		private System.Windows.Forms.GroupBox CDATAGroupBox;
		private System.Windows.Forms.CheckBox CDATAOptionalCheckBox;
		private System.Windows.Forms.FlowLayoutPanel optionFlowPanel;
		private System.Windows.Forms.CheckBox CDATACheckBox;
		private System.Windows.Forms.CheckBox textCheckBox;
		private System.Windows.Forms.GroupBox textGroupBox;
		private System.Windows.Forms.FlowLayoutPanel textFlowLayoutPanel;
		private System.Windows.Forms.CheckBox textOptionalCheckBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox dataTypeComboBox;
		private System.Windows.Forms.TableLayoutPanel nameTableLayoutPanel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox classNameTextBox;
	}
}
