namespace XMLToDataClass.View
{
	partial class TimeSpanSettings
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
			this.label2 = new System.Windows.Forms.Label();
			this.cultureComboBox = new System.Windows.Forms.ComboBox();
			this.mainTableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainTableLayoutPanel
			// 
			this.mainTableLayoutPanel.ColumnCount = 2;
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.Controls.Add(this.label2, 0, 0);
			this.mainTableLayoutPanel.Controls.Add(this.cultureComboBox, 1, 0);
			this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
			this.mainTableLayoutPanel.RowCount = 2;
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.mainTableLayoutPanel.Size = new System.Drawing.Size(169, 59);
			this.mainTableLayoutPanel.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Location = new System.Drawing.Point(3, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(43, 27);
			this.label2.TabIndex = 2;
			this.label2.Text = "Culture:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cultureComboBox
			// 
			this.cultureComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cultureComboBox.FormattingEnabled = true;
			this.cultureComboBox.Location = new System.Drawing.Point(52, 3);
			this.cultureComboBox.Name = "cultureComboBox";
			this.cultureComboBox.Size = new System.Drawing.Size(114, 21);
			this.cultureComboBox.TabIndex = 3;
			this.cultureComboBox.SelectedIndexChanged += new System.EventHandler(this.cultureComboBox_SelectedIndexChanged);
			// 
			// TimeSpanSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.mainTableLayoutPanel);
			this.MinimumSize = new System.Drawing.Size(169, 59);
			this.Name = "TimeSpanSettings";
			this.Size = new System.Drawing.Size(169, 59);
			this.mainTableLayoutPanel.ResumeLayout(false);
			this.mainTableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cultureComboBox;
	}
}
