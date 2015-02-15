using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XMLToDataClass.Data;

namespace XMLToDataClass.View
{
	public partial class AttributeInfoPanel : UserControl
	{
		# region Properties

		public AttributeInfo Info { get; private set; }

		#endregion Properties

		public AttributeInfoPanel(AttributeInfo info)
		{
			InitializeComponent();

			Info = info;
			foreach (string data in Enum.GetNames(typeof(DataType)))
				dataTypeComboBox.Items.Add(data);
			dataTypeComboBox.SelectedItem = Enum.GetName(typeof(DataType), info.AttributeType);

			propertyNameTextBox.Text = Info.PropertyName;
			optionalCheckBox.Checked = Info.IsOptional;
			emptyCheckBox.Checked = Info.CanBeEmpty;

		}

		private void propertyNameTextBox_TextChanged(object sender, EventArgs e)
		{
			if (!DataTypeUtility.IsIdentifierNameValid(propertyNameTextBox.Text))
			{
				MessageBox.Show(string.Format("The property name specified {0} is not a valid C# property name identifier", propertyNameTextBox.Text), "Incorrect Identifier", MessageBoxButtons.OK, MessageBoxIcon.Error);
				propertyNameTextBox.Text = Info.PropertyName;
				return;
			}
			Info.PropertyName = propertyNameTextBox.Text;
		}

		private void dataTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!Enum.IsDefined(typeof(DataType), (string)dataTypeComboBox.SelectedItem))
			{
				MessageBox.Show(string.Format("The data type specified {0} is not a valid C# data type", (string)dataTypeComboBox.SelectedItem), "Incorrect Data Type", MessageBoxButtons.OK, MessageBoxIcon.Error);
				dataTypeComboBox.SelectedItem = Enum.GetName(typeof(DataType), Info.AttributeType);
				return;
			}
			Info.AttributeType = (DataType)Enum.Parse(typeof(DataType), (string)dataTypeComboBox.SelectedItem);
		}

		private void optionalCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Info.IsOptional = optionalCheckBox.Checked;
		}

		private void emptyCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Info.CanBeEmpty = emptyCheckBox.Checked;
		}
	}
}
