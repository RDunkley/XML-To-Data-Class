namespace XMLToDataClass.View
{
	partial class StringTypeSettings
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
			this.minimumTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.maximumTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.mainTableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainTableLayoutPanel
			// 
			this.mainTableLayoutPanel.ColumnCount = 3;
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 166F));
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 59F));
			this.mainTableLayoutPanel.Controls.Add(this.minimumTextBox, 1, 0);
			this.mainTableLayoutPanel.Controls.Add(this.label1, 0, 0);
			this.mainTableLayoutPanel.Controls.Add(this.label2, 0, 1);
			this.mainTableLayoutPanel.Controls.Add(this.maximumTextBox, 1, 1);
			this.mainTableLayoutPanel.Controls.Add(this.label3, 2, 1);
			this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
			this.mainTableLayoutPanel.RowCount = 3;
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.Size = new System.Drawing.Size(290, 75);
			this.mainTableLayoutPanel.TabIndex = 0;
			// 
			// minimumTextBox
			// 
			this.minimumTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.minimumTextBox.Location = new System.Drawing.Point(169, 3);
			this.minimumTextBox.Name = "minimumTextBox";
			this.minimumTextBox.Size = new System.Drawing.Size(59, 20);
			this.minimumTextBox.TabIndex = 0;
			this.minimumTextBox.Text = "0";
			this.minimumTextBox.TextChanged += new System.EventHandler(this.minimumTextBox_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(160, 26);
			this.label1.TabIndex = 1;
			this.label1.Text = "Minimum Number of Characters:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Location = new System.Drawing.Point(3, 26);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(160, 26);
			this.label2.TabIndex = 2;
			this.label2.Text = "Maximum Number of Characters:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// maximumTextBox
			// 
			this.maximumTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.maximumTextBox.Location = new System.Drawing.Point(169, 29);
			this.maximumTextBox.Name = "maximumTextBox";
			this.maximumTextBox.Size = new System.Drawing.Size(59, 20);
			this.maximumTextBox.TabIndex = 3;
			this.maximumTextBox.Text = "0";
			this.maximumTextBox.TextChanged += new System.EventHandler(this.maximumTextBox_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.Location = new System.Drawing.Point(234, 26);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(53, 26);
			this.label3.TabIndex = 4;
			this.label3.Text = "0 - Infinite";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// StringTypeSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.mainTableLayoutPanel);
			this.MinimumSize = new System.Drawing.Size(290, 75);
			this.Name = "StringTypeSettings";
			this.Size = new System.Drawing.Size(290, 75);
			this.mainTableLayoutPanel.ResumeLayout(false);
			this.mainTableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
		private System.Windows.Forms.TextBox minimumTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox maximumTextBox;
		private System.Windows.Forms.Label label3;
	}
}
