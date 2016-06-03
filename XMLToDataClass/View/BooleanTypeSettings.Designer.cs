namespace XMLToDataClass.View
{
	partial class BooleanTypeSettings
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
			this.allowTrueFalseCheckBox = new System.Windows.Forms.CheckBox();
			this.allowZeroOneCheckBox = new System.Windows.Forms.CheckBox();
			this.allowYesNoCheckBox = new System.Windows.Forms.CheckBox();
			this.mainTableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainTableLayoutPanel
			// 
			this.mainTableLayoutPanel.ColumnCount = 1;
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.mainTableLayoutPanel.Controls.Add(this.allowTrueFalseCheckBox, 0, 0);
			this.mainTableLayoutPanel.Controls.Add(this.allowZeroOneCheckBox, 0, 1);
			this.mainTableLayoutPanel.Controls.Add(this.allowYesNoCheckBox, 0, 2);
			this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
			this.mainTableLayoutPanel.RowCount = 3;
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.mainTableLayoutPanel.Size = new System.Drawing.Size(319, 160);
			this.mainTableLayoutPanel.TabIndex = 0;
			// 
			// allowTrueFalseCheckBox
			// 
			this.allowTrueFalseCheckBox.AutoSize = true;
			this.allowTrueFalseCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.allowTrueFalseCheckBox.Location = new System.Drawing.Point(3, 3);
			this.allowTrueFalseCheckBox.Name = "allowTrueFalseCheckBox";
			this.allowTrueFalseCheckBox.Size = new System.Drawing.Size(313, 47);
			this.allowTrueFalseCheckBox.TabIndex = 0;
			this.allowTrueFalseCheckBox.Text = "Allow \"True\" and \"False\" strings";
			this.allowTrueFalseCheckBox.UseVisualStyleBackColor = true;
			this.allowTrueFalseCheckBox.CheckedChanged += new System.EventHandler(this.allowTrueFalseCheckBox_CheckedChanged);
			// 
			// allowZeroOneCheckBox
			// 
			this.allowZeroOneCheckBox.AutoSize = true;
			this.allowZeroOneCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.allowZeroOneCheckBox.Location = new System.Drawing.Point(3, 56);
			this.allowZeroOneCheckBox.Name = "allowZeroOneCheckBox";
			this.allowZeroOneCheckBox.Size = new System.Drawing.Size(313, 47);
			this.allowZeroOneCheckBox.TabIndex = 1;
			this.allowZeroOneCheckBox.Text = "Allow \"0\" and \"1\" strings (0 - false, 1 - true)";
			this.allowZeroOneCheckBox.UseVisualStyleBackColor = true;
			this.allowZeroOneCheckBox.CheckedChanged += new System.EventHandler(this.allowZeroOneCheckBox_CheckedChanged);
			// 
			// allowYesNoCheckBox
			// 
			this.allowYesNoCheckBox.AutoSize = true;
			this.allowYesNoCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.allowYesNoCheckBox.Location = new System.Drawing.Point(3, 109);
			this.allowYesNoCheckBox.Name = "allowYesNoCheckBox";
			this.allowYesNoCheckBox.Size = new System.Drawing.Size(313, 48);
			this.allowYesNoCheckBox.TabIndex = 2;
			this.allowYesNoCheckBox.Text = "Allow \"Yes\" and \"No\" strings (no - false, yes - true)";
			this.allowYesNoCheckBox.UseVisualStyleBackColor = true;
			this.allowYesNoCheckBox.CheckedChanged += new System.EventHandler(this.allowYesNoCheckBox_CheckedChanged);
			// 
			// BooleanTypeSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.mainTableLayoutPanel);
			this.Name = "BooleanTypeSettings";
			this.Size = new System.Drawing.Size(319, 160);
			this.mainTableLayoutPanel.ResumeLayout(false);
			this.mainTableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
		private System.Windows.Forms.CheckBox allowTrueFalseCheckBox;
		private System.Windows.Forms.CheckBox allowZeroOneCheckBox;
		private System.Windows.Forms.CheckBox allowYesNoCheckBox;
	}
}
