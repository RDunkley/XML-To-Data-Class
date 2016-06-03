using System;

namespace XMLToDataClass.View
{
	partial class FloatingPointSettings<T> where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
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
			this.currencyCheckBox = new System.Windows.Forms.CheckBox();
			this.exponentCheckBox = new System.Windows.Forms.CheckBox();
			this.parenthesesCheckBox = new System.Windows.Forms.CheckBox();
			this.percentCheckBox = new System.Windows.Forms.CheckBox();
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
			this.mainTableLayoutPanel.Controls.Add(this.currencyCheckBox, 0, 2);
			this.mainTableLayoutPanel.Controls.Add(this.exponentCheckBox, 0, 3);
			this.mainTableLayoutPanel.Controls.Add(this.parenthesesCheckBox, 0, 4);
			this.mainTableLayoutPanel.Controls.Add(this.percentCheckBox, 0, 5);
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
			this.mainTableLayoutPanel.Size = new System.Drawing.Size(346, 165);
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
			this.minimumTextBox.Size = new System.Drawing.Size(250, 20);
			this.minimumTextBox.TabIndex = 2;
			this.minimumTextBox.TextChanged += new System.EventHandler(this.minimumTextBox_TextChanged);
			// 
			// maximumTextBox
			// 
			this.maximumTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.maximumTextBox.Location = new System.Drawing.Point(93, 29);
			this.maximumTextBox.Name = "maximumTextBox";
			this.maximumTextBox.Size = new System.Drawing.Size(250, 20);
			this.maximumTextBox.TabIndex = 3;
			this.maximumTextBox.TextChanged += new System.EventHandler(this.maximumTextBox_TextChanged);
			// 
			// currencyCheckBox
			// 
			this.currencyCheckBox.AutoSize = true;
			this.mainTableLayoutPanel.SetColumnSpan(this.currencyCheckBox, 2);
			this.currencyCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.currencyCheckBox.Location = new System.Drawing.Point(3, 55);
			this.currencyCheckBox.Name = "currencyCheckBox";
			this.currencyCheckBox.Size = new System.Drawing.Size(340, 17);
			this.currencyCheckBox.TabIndex = 4;
			this.currencyCheckBox.Text = "Allow Currency (Ex: \'$123.45\')";
			this.currencyCheckBox.UseVisualStyleBackColor = true;
			this.currencyCheckBox.CheckedChanged += new System.EventHandler(this.currencyCheckBox_CheckedChanged);
			// 
			// exponentCheckBox
			// 
			this.exponentCheckBox.AutoSize = true;
			this.mainTableLayoutPanel.SetColumnSpan(this.exponentCheckBox, 2);
			this.exponentCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.exponentCheckBox.Location = new System.Drawing.Point(3, 78);
			this.exponentCheckBox.Name = "exponentCheckBox";
			this.exponentCheckBox.Size = new System.Drawing.Size(340, 17);
			this.exponentCheckBox.TabIndex = 5;
			this.exponentCheckBox.Text = "Allow Exponents (Ex: \'123E45\', \'123E+45\', or \'123E-45\')";
			this.exponentCheckBox.UseVisualStyleBackColor = true;
			this.exponentCheckBox.CheckedChanged += new System.EventHandler(this.exponentCheckBox_CheckedChanged);
			// 
			// parenthesesCheckBox
			// 
			this.parenthesesCheckBox.AutoSize = true;
			this.mainTableLayoutPanel.SetColumnSpan(this.parenthesesCheckBox, 2);
			this.parenthesesCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.parenthesesCheckBox.Location = new System.Drawing.Point(3, 101);
			this.parenthesesCheckBox.Name = "parenthesesCheckBox";
			this.parenthesesCheckBox.Size = new System.Drawing.Size(340, 17);
			this.parenthesesCheckBox.TabIndex = 6;
			this.parenthesesCheckBox.Text = "Allow Parentheses to Represent Negative Numbers (Ex: \'(123)\' is \'-123\')";
			this.parenthesesCheckBox.UseVisualStyleBackColor = true;
			this.parenthesesCheckBox.CheckedChanged += new System.EventHandler(this.parenthesesCheckBox_CheckedChanged);
			// 
			// percentCheckBox
			// 
			this.percentCheckBox.AutoSize = true;
			this.mainTableLayoutPanel.SetColumnSpan(this.percentCheckBox, 2);
			this.percentCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.percentCheckBox.Location = new System.Drawing.Point(3, 124);
			this.percentCheckBox.Name = "percentCheckBox";
			this.percentCheckBox.Size = new System.Drawing.Size(340, 17);
			this.percentCheckBox.TabIndex = 7;
			this.percentCheckBox.Text = "Allow Percentages (Ex: \'97.3%\', \'100%\', or \'0.3%\')";
			this.percentCheckBox.UseVisualStyleBackColor = true;
			this.percentCheckBox.CheckedChanged += new System.EventHandler(this.percentCheckBox_CheckedChanged);
			// 
			// FloatingPointSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.mainTableLayoutPanel);
			this.Name = "FloatingPointSettings";
			this.Size = new System.Drawing.Size(346, 165);
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
		private System.Windows.Forms.CheckBox currencyCheckBox;
		private System.Windows.Forms.CheckBox exponentCheckBox;
		private System.Windows.Forms.CheckBox parenthesesCheckBox;
		private System.Windows.Forms.CheckBox percentCheckBox;
	}
}
