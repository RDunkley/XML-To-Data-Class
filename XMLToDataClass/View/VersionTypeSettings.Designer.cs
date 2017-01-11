namespace XMLToDataClass.View
{
	partial class VersionTypeSettings
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
			this.allowBuildCheckBox = new System.Windows.Forms.CheckBox();
			this.allowRevisionCheckBox = new System.Windows.Forms.CheckBox();
			this.mainTableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainTableLayoutPanel
			// 
			this.mainTableLayoutPanel.ColumnCount = 1;
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.Controls.Add(this.allowBuildCheckBox, 0, 0);
			this.mainTableLayoutPanel.Controls.Add(this.allowRevisionCheckBox, 0, 1);
			this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
			this.mainTableLayoutPanel.RowCount = 3;
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.Size = new System.Drawing.Size(133, 78);
			this.mainTableLayoutPanel.TabIndex = 0;
			// 
			// allowBuildCheckBox
			// 
			this.allowBuildCheckBox.AutoSize = true;
			this.allowBuildCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.allowBuildCheckBox.Location = new System.Drawing.Point(3, 3);
			this.allowBuildCheckBox.Name = "allowBuildCheckBox";
			this.allowBuildCheckBox.Size = new System.Drawing.Size(127, 17);
			this.allowBuildCheckBox.TabIndex = 0;
			this.allowBuildCheckBox.Text = "Allow Build";
			this.allowBuildCheckBox.UseVisualStyleBackColor = true;
			this.allowBuildCheckBox.CheckedChanged += new System.EventHandler(this.allowBuildCheckBox_CheckedChanged);
			// 
			// allowRevisionCheckBox
			// 
			this.allowRevisionCheckBox.AutoSize = true;
			this.allowRevisionCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.allowRevisionCheckBox.Location = new System.Drawing.Point(3, 26);
			this.allowRevisionCheckBox.Name = "allowRevisionCheckBox";
			this.allowRevisionCheckBox.Size = new System.Drawing.Size(127, 17);
			this.allowRevisionCheckBox.TabIndex = 1;
			this.allowRevisionCheckBox.Text = "Allow Revision";
			this.allowRevisionCheckBox.UseVisualStyleBackColor = true;
			this.allowRevisionCheckBox.CheckedChanged += new System.EventHandler(this.allowRevisionCheckBox_CheckedChanged);
			// 
			// VersionTypeSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.mainTableLayoutPanel);
			this.MinimumSize = new System.Drawing.Size(133, 78);
			this.Name = "VersionTypeSettings";
			this.Size = new System.Drawing.Size(133, 78);
			this.mainTableLayoutPanel.ResumeLayout(false);
			this.mainTableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
		private System.Windows.Forms.CheckBox allowBuildCheckBox;
		private System.Windows.Forms.CheckBox allowRevisionCheckBox;
	}
}
