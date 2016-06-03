namespace XMLToDataClass.View
{
	partial class EnumTypeSettings
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
			this.mainDataGridView = new System.Windows.Forms.DataGridView();
			this.keyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.addButton = new System.Windows.Forms.Button();
			this.removeButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.mainDataGridView)).BeginInit();
			this.mainTableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainDataGridView
			// 
			this.mainDataGridView.AllowUserToAddRows = false;
			this.mainDataGridView.AllowUserToDeleteRows = false;
			this.mainDataGridView.AllowUserToOrderColumns = true;
			this.mainDataGridView.AllowUserToResizeRows = false;
			this.mainDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.mainDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.keyColumn,
            this.valueColumn});
			this.mainTableLayoutPanel.SetColumnSpan(this.mainDataGridView, 2);
			this.mainDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainDataGridView.Location = new System.Drawing.Point(3, 3);
			this.mainDataGridView.Name = "mainDataGridView";
			this.mainDataGridView.RowHeadersVisible = false;
			this.mainDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.mainDataGridView.ShowCellErrors = false;
			this.mainDataGridView.ShowEditingIcon = false;
			this.mainDataGridView.ShowRowErrors = false;
			this.mainDataGridView.Size = new System.Drawing.Size(318, 164);
			this.mainDataGridView.TabIndex = 0;
			// 
			// keyColumn
			// 
			this.keyColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.keyColumn.HeaderText = "XML String";
			this.keyColumn.MinimumWidth = 20;
			this.keyColumn.Name = "keyColumn";
			this.keyColumn.ReadOnly = true;
			this.keyColumn.ToolTipText = "The value string found in the XMl file.";
			this.keyColumn.Width = 84;
			// 
			// valueColumn
			// 
			this.valueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.valueColumn.HeaderText = "EnumTypeName";
			this.valueColumn.Name = "valueColumn";
			this.valueColumn.ToolTipText = "The name of the enumeration item.";
			// 
			// mainTableLayoutPanel
			// 
			this.mainTableLayoutPanel.ColumnCount = 2;
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.mainTableLayoutPanel.Controls.Add(this.mainDataGridView, 0, 0);
			this.mainTableLayoutPanel.Controls.Add(this.addButton, 0, 1);
			this.mainTableLayoutPanel.Controls.Add(this.removeButton, 1, 1);
			this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
			this.mainTableLayoutPanel.RowCount = 2;
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.mainTableLayoutPanel.Size = new System.Drawing.Size(324, 199);
			this.mainTableLayoutPanel.TabIndex = 1;
			// 
			// addButton
			// 
			this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.addButton.Location = new System.Drawing.Point(84, 173);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(75, 23);
			this.addButton.TabIndex = 1;
			this.addButton.Text = "Add";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// removeButton
			// 
			this.removeButton.Location = new System.Drawing.Point(165, 173);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(75, 23);
			this.removeButton.TabIndex = 2;
			this.removeButton.Text = "Remove";
			this.removeButton.UseVisualStyleBackColor = true;
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// EnumTypeSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.mainTableLayoutPanel);
			this.Name = "EnumTypeSettings";
			this.Size = new System.Drawing.Size(324, 199);
			((System.ComponentModel.ISupportInitialize)(this.mainDataGridView)).EndInit();
			this.mainTableLayoutPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView mainDataGridView;
		private System.Windows.Forms.DataGridViewTextBoxColumn keyColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn valueColumn;
		private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button removeButton;
	}
}
