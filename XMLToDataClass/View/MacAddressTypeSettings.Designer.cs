namespace XMLToDataClass.View
{
	partial class MacAddressTypeSettings
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
			this.allowColonCheckBox = new System.Windows.Forms.CheckBox();
			this.allowDashCheckBox = new System.Windows.Forms.CheckBox();
			this.allowPeriodCheckBox = new System.Windows.Forms.CheckBox();
			this.mainTableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainTableLayoutPanel
			// 
			this.mainTableLayoutPanel.ColumnCount = 1;
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.Controls.Add(this.allowColonCheckBox, 0, 0);
			this.mainTableLayoutPanel.Controls.Add(this.allowDashCheckBox, 0, 1);
			this.mainTableLayoutPanel.Controls.Add(this.allowPeriodCheckBox, 0, 2);
			this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
			this.mainTableLayoutPanel.RowCount = 4;
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.Size = new System.Drawing.Size(251, 95);
			this.mainTableLayoutPanel.TabIndex = 0;
			// 
			// allowColonCheckBox
			// 
			this.allowColonCheckBox.AutoSize = true;
			this.allowColonCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.allowColonCheckBox.Location = new System.Drawing.Point(3, 3);
			this.allowColonCheckBox.Name = "allowColonCheckBox";
			this.allowColonCheckBox.Size = new System.Drawing.Size(245, 17);
			this.allowColonCheckBox.TabIndex = 0;
			this.allowColonCheckBox.Text = "Allow Colon Format (Ex: 00:11:22:33:44:55)";
			this.allowColonCheckBox.UseVisualStyleBackColor = true;
			this.allowColonCheckBox.CheckedChanged += new System.EventHandler(this.allowColonCheckBox_CheckedChanged);
			// 
			// allowDashCheckBox
			// 
			this.allowDashCheckBox.AutoSize = true;
			this.allowDashCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.allowDashCheckBox.Location = new System.Drawing.Point(3, 26);
			this.allowDashCheckBox.Name = "allowDashCheckBox";
			this.allowDashCheckBox.Size = new System.Drawing.Size(245, 17);
			this.allowDashCheckBox.TabIndex = 1;
			this.allowDashCheckBox.Text = "Allow Dash Format (Ex: 00-11-22-33-44-55)";
			this.allowDashCheckBox.UseVisualStyleBackColor = true;
			this.allowDashCheckBox.CheckedChanged += new System.EventHandler(this.allowDashCheckBox_CheckedChanged);
			// 
			// allowPeriodCheckBox
			// 
			this.allowPeriodCheckBox.AutoSize = true;
			this.allowPeriodCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.allowPeriodCheckBox.Location = new System.Drawing.Point(3, 49);
			this.allowPeriodCheckBox.Name = "allowPeriodCheckBox";
			this.allowPeriodCheckBox.Size = new System.Drawing.Size(245, 17);
			this.allowPeriodCheckBox.TabIndex = 2;
			this.allowPeriodCheckBox.Text = "Allow Period Format (Ex: 001.122.334.455)";
			this.allowPeriodCheckBox.UseVisualStyleBackColor = true;
			this.allowPeriodCheckBox.CheckedChanged += new System.EventHandler(this.allowPeriodCheckBox_CheckedChanged);
			// 
			// MacAddressTypeSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.mainTableLayoutPanel);
			this.MinimumSize = new System.Drawing.Size(251, 95);
			this.Name = "MacAddressTypeSettings";
			this.Size = new System.Drawing.Size(251, 95);
			this.mainTableLayoutPanel.ResumeLayout(false);
			this.mainTableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
		private System.Windows.Forms.CheckBox allowColonCheckBox;
		private System.Windows.Forms.CheckBox allowDashCheckBox;
		private System.Windows.Forms.CheckBox allowPeriodCheckBox;
	}
}
