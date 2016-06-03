namespace XMLToDataClass.View
{
	partial class DateTimeSettings
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
			this.label1 = new System.Windows.Forms.Label();
			this.mainComboBox = new System.Windows.Forms.ComboBox();
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
			this.mainTableLayoutPanel.Controls.Add(this.label1, 0, 1);
			this.mainTableLayoutPanel.Controls.Add(this.mainComboBox, 1, 1);
			this.mainTableLayoutPanel.Controls.Add(this.label2, 0, 0);
			this.mainTableLayoutPanel.Controls.Add(this.cultureComboBox, 1, 0);
			this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
			this.mainTableLayoutPanel.RowCount = 3;
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.Size = new System.Drawing.Size(303, 168);
			this.mainTableLayoutPanel.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 27);
			this.label1.TabIndex = 0;
			this.label1.Text = "Date/Time Type:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// mainComboBox
			// 
			this.mainComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainComboBox.FormattingEnabled = true;
			this.mainComboBox.Location = new System.Drawing.Point(97, 30);
			this.mainComboBox.Name = "mainComboBox";
			this.mainComboBox.Size = new System.Drawing.Size(203, 21);
			this.mainComboBox.TabIndex = 1;
			this.mainComboBox.SelectedIndexChanged += new System.EventHandler(this.mainComboBox_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Location = new System.Drawing.Point(3, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 27);
			this.label2.TabIndex = 2;
			this.label2.Text = "Culture:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cultureComboBox
			// 
			this.cultureComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cultureComboBox.FormattingEnabled = true;
			this.cultureComboBox.Location = new System.Drawing.Point(97, 3);
			this.cultureComboBox.Name = "cultureComboBox";
			this.cultureComboBox.Size = new System.Drawing.Size(203, 21);
			this.cultureComboBox.TabIndex = 3;
			this.cultureComboBox.SelectedIndexChanged += new System.EventHandler(this.cultureComboBox_SelectedIndexChanged);
			// 
			// DateTimeSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.mainTableLayoutPanel);
			this.Name = "DateTimeSettings";
			this.Size = new System.Drawing.Size(303, 168);
			this.mainTableLayoutPanel.ResumeLayout(false);
			this.mainTableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox mainComboBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cultureComboBox;
	}
}
