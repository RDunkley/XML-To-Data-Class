namespace XMLToDataClass.View
{
	partial class DataInfoPanel
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
			this.propertyNameTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.dataTypeComboBox = new System.Windows.Forms.ComboBox();
			this.emptyCheckBox = new System.Windows.Forms.CheckBox();
			this.optionalCheckBox = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.nameLabel = new System.Windows.Forms.Label();
			this.allTypesCheckBox = new System.Windows.Forms.CheckBox();
			this.typePanel = new System.Windows.Forms.Panel();
			this.errorLabel = new System.Windows.Forms.Label();
			this.dataTableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// dataTableLayoutPanel
			// 
			this.dataTableLayoutPanel.ColumnCount = 3;
			this.dataTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 86F));
			this.dataTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 106F));
			this.dataTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dataTableLayoutPanel.Controls.Add(this.propertyNameTextBox, 1, 1);
			this.dataTableLayoutPanel.Controls.Add(this.label2, 0, 1);
			this.dataTableLayoutPanel.Controls.Add(this.label3, 0, 3);
			this.dataTableLayoutPanel.Controls.Add(this.dataTypeComboBox, 1, 3);
			this.dataTableLayoutPanel.Controls.Add(this.emptyCheckBox, 1, 5);
			this.dataTableLayoutPanel.Controls.Add(this.optionalCheckBox, 1, 4);
			this.dataTableLayoutPanel.Controls.Add(this.label1, 0, 0);
			this.dataTableLayoutPanel.Controls.Add(this.nameLabel, 1, 0);
			this.dataTableLayoutPanel.Controls.Add(this.allTypesCheckBox, 1, 2);
			this.dataTableLayoutPanel.Controls.Add(this.typePanel, 0, 6);
			this.dataTableLayoutPanel.Controls.Add(this.errorLabel, 2, 3);
			this.dataTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.dataTableLayoutPanel.Name = "dataTableLayoutPanel";
			this.dataTableLayoutPanel.RowCount = 7;
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
			this.dataTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dataTableLayoutPanel.Size = new System.Drawing.Size(382, 374);
			this.dataTableLayoutPanel.TabIndex = 3;
			// 
			// propertyNameTextBox
			// 
			this.dataTableLayoutPanel.SetColumnSpan(this.propertyNameTextBox, 2);
			this.propertyNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyNameTextBox.Location = new System.Drawing.Point(89, 23);
			this.propertyNameTextBox.Name = "propertyNameTextBox";
			this.propertyNameTextBox.Size = new System.Drawing.Size(290, 20);
			this.propertyNameTextBox.TabIndex = 1;
			this.propertyNameTextBox.TextChanged += new System.EventHandler(this.propertyNameTextBox_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Location = new System.Drawing.Point(3, 20);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 26);
			this.label2.TabIndex = 0;
			this.label2.Text = "Property Name:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.Location = new System.Drawing.Point(3, 69);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 27);
			this.label3.TabIndex = 6;
			this.label3.Text = "Data Type:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// dataTypeComboBox
			// 
			this.dataTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.dataTypeComboBox.FormattingEnabled = true;
			this.dataTypeComboBox.Location = new System.Drawing.Point(89, 72);
			this.dataTypeComboBox.Name = "dataTypeComboBox";
			this.dataTypeComboBox.Size = new System.Drawing.Size(100, 21);
			this.dataTypeComboBox.TabIndex = 7;
			this.dataTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.dataTypeComboBox_SelectedIndexChanged);
			// 
			// emptyCheckBox
			// 
			this.emptyCheckBox.AutoSize = true;
			this.emptyCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.emptyCheckBox.Location = new System.Drawing.Point(89, 128);
			this.emptyCheckBox.Name = "emptyCheckBox";
			this.emptyCheckBox.Size = new System.Drawing.Size(100, 23);
			this.emptyCheckBox.TabIndex = 9;
			this.emptyCheckBox.Text = "Can Be Empty";
			this.emptyCheckBox.UseVisualStyleBackColor = true;
			this.emptyCheckBox.CheckedChanged += new System.EventHandler(this.emptyCheckBox_CheckedChanged);
			// 
			// optionalCheckBox
			// 
			this.optionalCheckBox.AutoSize = true;
			this.optionalCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.optionalCheckBox.Location = new System.Drawing.Point(89, 99);
			this.optionalCheckBox.Name = "optionalCheckBox";
			this.optionalCheckBox.Size = new System.Drawing.Size(100, 23);
			this.optionalCheckBox.TabIndex = 8;
			this.optionalCheckBox.Text = "Optional";
			this.optionalCheckBox.UseVisualStyleBackColor = true;
			this.optionalCheckBox.CheckedChanged += new System.EventHandler(this.optionalCheckBox_CheckedChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 20);
			this.label1.TabIndex = 10;
			this.label1.Text = "XML Name:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.dataTableLayoutPanel.SetColumnSpan(this.nameLabel, 2);
			this.nameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nameLabel.Location = new System.Drawing.Point(89, 0);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(290, 20);
			this.nameLabel.TabIndex = 11;
			this.nameLabel.Text = "Name";
			this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// allTypesCheckBox
			// 
			this.allTypesCheckBox.AutoSize = true;
			this.allTypesCheckBox.Location = new System.Drawing.Point(89, 49);
			this.allTypesCheckBox.Name = "allTypesCheckBox";
			this.allTypesCheckBox.Size = new System.Drawing.Size(99, 17);
			this.allTypesCheckBox.TabIndex = 12;
			this.allTypesCheckBox.Text = "Show All Types";
			this.allTypesCheckBox.UseVisualStyleBackColor = true;
			this.allTypesCheckBox.CheckedChanged += new System.EventHandler(this.allTypesCheckBox_CheckedChanged);
			// 
			// typePanel
			// 
			this.dataTableLayoutPanel.SetColumnSpan(this.typePanel, 3);
			this.typePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.typePanel.Location = new System.Drawing.Point(3, 157);
			this.typePanel.Name = "typePanel";
			this.typePanel.Size = new System.Drawing.Size(376, 214);
			this.typePanel.TabIndex = 13;
			// 
			// errorLabel
			// 
			this.errorLabel.AutoSize = true;
			this.errorLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.errorLabel.ForeColor = System.Drawing.Color.Red;
			this.errorLabel.Location = new System.Drawing.Point(195, 69);
			this.errorLabel.Name = "errorLabel";
			this.errorLabel.Size = new System.Drawing.Size(184, 27);
			this.errorLabel.TabIndex = 14;
			this.errorLabel.Text = "Error";
			this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// DataInfoPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dataTableLayoutPanel);
			this.Name = "DataInfoPanel";
			this.Size = new System.Drawing.Size(382, 374);
			this.dataTableLayoutPanel.ResumeLayout(false);
			this.dataTableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel dataTableLayoutPanel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox propertyNameTextBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox dataTypeComboBox;
		private System.Windows.Forms.CheckBox optionalCheckBox;
		private System.Windows.Forms.CheckBox emptyCheckBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.CheckBox allTypesCheckBox;
		private System.Windows.Forms.Panel typePanel;
		private System.Windows.Forms.Label errorLabel;
	}
}
