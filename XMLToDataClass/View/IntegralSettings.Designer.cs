using System;

namespace XMLToDataClass.View
{
	partial class IntegralSettings<T> where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
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
			this.label2 = new System.Windows.Forms.Label();
			this.minimumTextBox = new System.Windows.Forms.TextBox();
			this.maximumTextBox = new System.Windows.Forms.TextBox();
			this.hex1CheckBox = new System.Windows.Forms.CheckBox();
			this.hex2CheckBox = new System.Windows.Forms.CheckBox();
			this.binaryCheckBox = new System.Windows.Forms.CheckBox();
			this.integerCheckBox = new System.Windows.Forms.CheckBox();
			this.mainTableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainTableLayoutPanel
			// 
			this.mainTableLayoutPanel.ColumnCount = 2;
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.Controls.Add(this.label1, 0, 0);
			this.mainTableLayoutPanel.Controls.Add(this.label2, 0, 1);
			this.mainTableLayoutPanel.Controls.Add(this.minimumTextBox, 1, 0);
			this.mainTableLayoutPanel.Controls.Add(this.maximumTextBox, 1, 1);
			this.mainTableLayoutPanel.Controls.Add(this.hex1CheckBox, 0, 2);
			this.mainTableLayoutPanel.Controls.Add(this.hex2CheckBox, 0, 3);
			this.mainTableLayoutPanel.Controls.Add(this.binaryCheckBox, 0, 4);
			this.mainTableLayoutPanel.Controls.Add(this.integerCheckBox, 0, 5);
			this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
			this.mainTableLayoutPanel.RowCount = 7;
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.Size = new System.Drawing.Size(273, 163);
			this.mainTableLayoutPanel.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(84, 26);
			this.label1.TabIndex = 0;
			this.label1.Text = "Minimum Value:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Location = new System.Drawing.Point(3, 26);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(84, 26);
			this.label2.TabIndex = 1;
			this.label2.Text = "Maximum Value:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// minimumTextBox
			// 
			this.minimumTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.minimumTextBox.Location = new System.Drawing.Point(93, 3);
			this.minimumTextBox.Name = "minimumTextBox";
			this.minimumTextBox.Size = new System.Drawing.Size(177, 20);
			this.minimumTextBox.TabIndex = 2;
			this.minimumTextBox.TextChanged += new System.EventHandler(this.minimumTextBox_TextChanged);
			// 
			// maximumTextBox
			// 
			this.maximumTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.maximumTextBox.Location = new System.Drawing.Point(93, 29);
			this.maximumTextBox.Name = "maximumTextBox";
			this.maximumTextBox.Size = new System.Drawing.Size(177, 20);
			this.maximumTextBox.TabIndex = 3;
			this.maximumTextBox.TextChanged += new System.EventHandler(this.maximumTextBox_TextChanged);
			// 
			// hex1CheckBox
			// 
			this.hex1CheckBox.AutoSize = true;
			this.mainTableLayoutPanel.SetColumnSpan(this.hex1CheckBox, 2);
			this.hex1CheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.hex1CheckBox.Location = new System.Drawing.Point(3, 55);
			this.hex1CheckBox.Name = "hex1CheckBox";
			this.hex1CheckBox.Size = new System.Drawing.Size(267, 17);
			this.hex1CheckBox.TabIndex = 4;
			this.hex1CheckBox.Text = "Allow Hexadecimal Values (Ex: FFh)";
			this.hex1CheckBox.UseVisualStyleBackColor = true;
			this.hex1CheckBox.CheckedChanged += new System.EventHandler(this.hex1CheckBox_CheckedChanged);
			// 
			// hex2CheckBox
			// 
			this.hex2CheckBox.AutoSize = true;
			this.mainTableLayoutPanel.SetColumnSpan(this.hex2CheckBox, 2);
			this.hex2CheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.hex2CheckBox.Location = new System.Drawing.Point(3, 78);
			this.hex2CheckBox.Name = "hex2CheckBox";
			this.hex2CheckBox.Size = new System.Drawing.Size(267, 17);
			this.hex2CheckBox.TabIndex = 5;
			this.hex2CheckBox.Text = "Allow Hexadecimal Values (Ex: 0xFF)";
			this.hex2CheckBox.UseVisualStyleBackColor = true;
			this.hex2CheckBox.CheckedChanged += new System.EventHandler(this.hex2CheckBox_CheckedChanged);
			// 
			// binaryCheckBox
			// 
			this.binaryCheckBox.AutoSize = true;
			this.mainTableLayoutPanel.SetColumnSpan(this.binaryCheckBox, 2);
			this.binaryCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.binaryCheckBox.Location = new System.Drawing.Point(3, 101);
			this.binaryCheckBox.Name = "binaryCheckBox";
			this.binaryCheckBox.Size = new System.Drawing.Size(267, 17);
			this.binaryCheckBox.TabIndex = 6;
			this.binaryCheckBox.Text = "Allow Binary Values (Ex: 1011b)";
			this.binaryCheckBox.UseVisualStyleBackColor = true;
			this.binaryCheckBox.CheckedChanged += new System.EventHandler(this.binaryCheckBox_CheckedChanged);
			// 
			// integerCheckBox
			// 
			this.integerCheckBox.AutoSize = true;
			this.mainTableLayoutPanel.SetColumnSpan(this.integerCheckBox, 2);
			this.integerCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.integerCheckBox.Location = new System.Drawing.Point(3, 124);
			this.integerCheckBox.Name = "integerCheckBox";
			this.integerCheckBox.Size = new System.Drawing.Size(267, 17);
			this.integerCheckBox.TabIndex = 7;
			this.integerCheckBox.Text = "Allow Integer Values (Ex: 1,234, 1234 or -1234)";
			this.integerCheckBox.UseVisualStyleBackColor = true;
			this.integerCheckBox.CheckedChanged += new System.EventHandler(this.integerCheckBox_CheckedChanged);
			// 
			// IntegralSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.mainTableLayoutPanel);
			this.MinimumSize = new System.Drawing.Size(273, 163);
			this.Name = "IntegralSettings";
			this.Size = new System.Drawing.Size(273, 163);
			this.mainTableLayoutPanel.ResumeLayout(false);
			this.mainTableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox minimumTextBox;
		private System.Windows.Forms.TextBox maximumTextBox;
		private System.Windows.Forms.CheckBox hex1CheckBox;
		private System.Windows.Forms.CheckBox hex2CheckBox;
		private System.Windows.Forms.CheckBox binaryCheckBox;
		private System.Windows.Forms.CheckBox integerCheckBox;
	}
}
