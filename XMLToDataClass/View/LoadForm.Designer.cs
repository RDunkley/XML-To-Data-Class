namespace XMLToDataClass.View
{
	partial class LoadForm
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
			this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.filePathTextBox = new System.Windows.Forms.TextBox();
			this.browseButton = new System.Windows.Forms.Button();
			this.hierarchyCheckBox = new System.Windows.Forms.CheckBox();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.mainTableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainTableLayoutPanel
			// 
			this.mainTableLayoutPanel.ColumnCount = 4;
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.mainTableLayoutPanel.Controls.Add(this.label1, 0, 0);
			this.mainTableLayoutPanel.Controls.Add(this.filePathTextBox, 1, 0);
			this.mainTableLayoutPanel.Controls.Add(this.browseButton, 3, 0);
			this.mainTableLayoutPanel.Controls.Add(this.hierarchyCheckBox, 1, 1);
			this.mainTableLayoutPanel.Controls.Add(this.okButton, 1, 2);
			this.mainTableLayoutPanel.Controls.Add(this.cancelButton, 2, 2);
			this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
			this.mainTableLayoutPanel.RowCount = 3;
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.mainTableLayoutPanel.Size = new System.Drawing.Size(494, 87);
			this.mainTableLayoutPanel.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 29);
			this.label1.TabIndex = 0;
			this.label1.Text = "XML File Path:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// filePathTextBox
			// 
			this.mainTableLayoutPanel.SetColumnSpan(this.filePathTextBox, 2);
			this.filePathTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.filePathTextBox.Location = new System.Drawing.Point(85, 3);
			this.filePathTextBox.Name = "filePathTextBox";
			this.filePathTextBox.Size = new System.Drawing.Size(324, 20);
			this.filePathTextBox.TabIndex = 1;
			// 
			// browseButton
			// 
			this.browseButton.Location = new System.Drawing.Point(415, 3);
			this.browseButton.Name = "browseButton";
			this.browseButton.Size = new System.Drawing.Size(75, 23);
			this.browseButton.TabIndex = 2;
			this.browseButton.Text = "Browse";
			this.browseButton.UseVisualStyleBackColor = true;
			this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
			// 
			// hierarchyCheckBox
			// 
			this.hierarchyCheckBox.AutoSize = true;
			this.mainTableLayoutPanel.SetColumnSpan(this.hierarchyCheckBox, 2);
			this.hierarchyCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.hierarchyCheckBox.Location = new System.Drawing.Point(85, 32);
			this.hierarchyCheckBox.Name = "hierarchyCheckBox";
			this.hierarchyCheckBox.Size = new System.Drawing.Size(324, 17);
			this.hierarchyCheckBox.TabIndex = 3;
			this.hierarchyCheckBox.Text = "Preserve Hierarchy";
			this.hierarchyCheckBox.UseVisualStyleBackColor = true;
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(169, 61);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 6;
			this.okButton.Text = "Ok";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(250, 61);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 5;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// LoadForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(494, 87);
			this.Controls.Add(this.mainTableLayoutPanel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LoadForm";
			this.ShowIcon = false;
			this.Text = "Load XML File";
			this.mainTableLayoutPanel.ResumeLayout(false);
			this.mainTableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox filePathTextBox;
		private System.Windows.Forms.Button browseButton;
		private System.Windows.Forms.CheckBox hierarchyCheckBox;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
	}
}