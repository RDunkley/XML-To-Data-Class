namespace XMLToDataClass
{
	partial class SettingsForm
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
			this.buttonFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.exportButton = new System.Windows.Forms.Button();
			this.importButton = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.fileExtenstionTextBox = new System.Windows.Forms.TextBox();
			this.mainTableLayoutPanel.SuspendLayout();
			this.buttonFlowLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainTableLayoutPanel
			// 
			this.mainTableLayoutPanel.ColumnCount = 4;
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.mainTableLayoutPanel.Controls.Add(this.buttonFlowLayoutPanel, 1, 3);
			this.mainTableLayoutPanel.Controls.Add(this.fileExtenstionTextBox, 2, 1);
			this.mainTableLayoutPanel.Controls.Add(this.label6, 1, 1);
			this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
			this.mainTableLayoutPanel.RowCount = 4;
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.Size = new System.Drawing.Size(426, 411);
			this.mainTableLayoutPanel.TabIndex = 0;
			// 
			// buttonFlowLayoutPanel
			// 
			this.mainTableLayoutPanel.SetColumnSpan(this.buttonFlowLayoutPanel, 2);
			this.buttonFlowLayoutPanel.Controls.Add(this.okButton);
			this.buttonFlowLayoutPanel.Controls.Add(this.cancelButton);
			this.buttonFlowLayoutPanel.Controls.Add(this.exportButton);
			this.buttonFlowLayoutPanel.Controls.Add(this.importButton);
			this.buttonFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.buttonFlowLayoutPanel.Location = new System.Drawing.Point(13, 378);
			this.buttonFlowLayoutPanel.Name = "buttonFlowLayoutPanel";
			this.buttonFlowLayoutPanel.Size = new System.Drawing.Size(400, 30);
			this.buttonFlowLayoutPanel.TabIndex = 18;
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(3, 3);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 13;
			this.okButton.Text = "Ok";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(84, 3);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 14;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// exportButton
			// 
			this.exportButton.Location = new System.Drawing.Point(165, 3);
			this.exportButton.Name = "exportButton";
			this.exportButton.Size = new System.Drawing.Size(75, 23);
			this.exportButton.TabIndex = 15;
			this.exportButton.Text = "Export";
			this.exportButton.UseVisualStyleBackColor = true;
			this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
			// 
			// importButton
			// 
			this.importButton.Location = new System.Drawing.Point(246, 3);
			this.importButton.Name = "importButton";
			this.importButton.Size = new System.Drawing.Size(75, 23);
			this.importButton.TabIndex = 16;
			this.importButton.Text = "Import";
			this.importButton.UseVisualStyleBackColor = true;
			this.importButton.Click += new System.EventHandler(this.importButton_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label6.Location = new System.Drawing.Point(13, 10);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(239, 26);
			this.label6.TabIndex = 17;
			this.label6.Text = "File Extension Addition:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// fileExtenstionTextBox
			// 
			this.fileExtenstionTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fileExtenstionTextBox.Location = new System.Drawing.Point(258, 13);
			this.fileExtenstionTextBox.Name = "fileExtenstionTextBox";
			this.fileExtenstionTextBox.Size = new System.Drawing.Size(155, 20);
			this.fileExtenstionTextBox.TabIndex = 16;
			// 
			// SettingsForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(426, 411);
			this.Controls.Add(this.mainTableLayoutPanel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SettingsForm";
			this.Text = "Code Generation Settings";
			this.mainTableLayoutPanel.ResumeLayout(false);
			this.mainTableLayoutPanel.PerformLayout();
			this.buttonFlowLayoutPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.FlowLayoutPanel buttonFlowLayoutPanel;
		private System.Windows.Forms.Button exportButton;
		private System.Windows.Forms.Button importButton;
		private System.Windows.Forms.TextBox fileExtenstionTextBox;
		private System.Windows.Forms.Label label6;
	}
}