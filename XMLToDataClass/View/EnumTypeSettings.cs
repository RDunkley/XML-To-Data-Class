using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XMLToDataClass.Data.Types;
using CSCodeGen;

namespace XMLToDataClass.View
{
	public partial class EnumTypeSettings : UserControl
	{
		private EnumType mType;

		public event EventHandler SettingsChanged;

		public EnumTypeSettings(EnumType type)
		{
			if (type == null)
				throw new ArgumentNullException("type");
			mType = type;

			InitializeComponent();

			foreach(string key in type.TypeLookup.Keys)
				mainDataGridView.Rows.Add(key, type.TypeLookup[key]);

			this.mainDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.mainDataGridView_CellValueChanged);
		}

		private void mainDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if(e.RowIndex == valueColumn.Index)
			{
				string key = (string)mainDataGridView.Rows[e.RowIndex].Cells[keyColumn.Index].Value;
				string newValue = (string)mainDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
				mType.TypeLookup[key] = newValue;
			}

			SettingsChanged?.Invoke(this, null);
		}

		private void addButton_Click(object sender, EventArgs e)
		{

			TextInputForm form = new TextInputForm();
			form.Text = "Add Enumerated Item";
			form.MainText = "Specify the XML string that will represent the new enumerated item";
			if (form.ShowDialog() != DialogResult.OK)
				return;

			string key = form.Value;
			if (key == null)
			{
				MessageBox.Show("The XML String specified is a null reference. The enumerated item will not be added.", "Error Adding Item", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (key.Length == 0)
			{
				MessageBox.Show("The XML String specified is an empty string. The enumerated item will not be added.", "Error Adding Item", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (mType.TypeLookup.ContainsKey(key))
			{
				MessageBox.Show("The XML String specified already exists as an enumerated item. A new enumerated item will not be added.", "Error Adding Item", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			string value = StringUtility.GetUpperCamelCase(key);
			mType.TypeLookup.Add(key, value);
			mainDataGridView.Rows.Add(key, value);

			SettingsChanged?.Invoke(this, null);
		}

		private void removeButton_Click(object sender, EventArgs e)
		{
			List<DataGridViewRow> rows = new List<DataGridViewRow>();
			foreach (DataGridViewRow row in mainDataGridView.SelectedRows)
				rows.Add(row);

			foreach (DataGridViewRow row in rows)
			{
				string key = (string)row.Cells[keyColumn.Index].Value;
				if (mType.TypeLookup.ContainsKey(key))
					mType.TypeLookup.Remove(key);

				mainDataGridView.Rows.Remove(row);
			}

			SettingsChanged?.Invoke(this, null);
		}
	}
}
