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
	public partial class ElementInfoPanel : UserControl
	{
		#region Fields

		private XMLInfo mXMLInfo;

		#endregion Fields

		# region Properties

		public ElementInfo Info { get; private set; }

		#endregion Properties

		public ElementInfoPanel(ElementInfo info, XMLInfo xmlInfo)
		{
			if (info == null)
				throw new ArgumentNullException("info");
			if(info.HasText && !Enum.IsDefined(typeof(DataType),info.TextDataType))
				throw new ArgumentException("info contains a info.TextDataType that is not recognized");
			if (xmlInfo == null)
				throw new ArgumentNullException("xmlInfo");

			InitializeComponent();

			Info = info;
			mXMLInfo = xmlInfo;
			foreach (string data in Enum.GetNames(typeof(DataType)))
				dataTypeComboBox.Items.Add(data);
			dataTypeComboBox.SelectedItem = Enum.GetName(typeof(DataType), info.TextDataType);

			classNameTextBox.Text = info.ClassName;
			CDATACheckBox.Checked = info.HasCDATA;
			CDATAGroupBox.Enabled = info.HasCDATA;
			textCheckBox.Checked = info.HasText;
			textGroupBox.Enabled = info.HasText;
			CDATAOptionalCheckBox.Checked = info.CDATAIsOptional;
			textOptionalCheckBox.Checked = info.TextIsOptional;
		}

		private void classNameTextBox_TextChanged(object sender, EventArgs e)
		{
			if (!DataTypeUtility.IsIdentifierNameValid(classNameTextBox.Text))
			{
				MessageBox.Show(string.Format("The class name specified {0} is not a valid C# class name identifier", classNameTextBox.Text), "Incorrect Identifier", MessageBoxButtons.OK, MessageBoxIcon.Error);
				classNameTextBox.Text = Info.ClassName;
				return;
			}
			Info.ClassName = classNameTextBox.Text;
		}

		private void CDATACheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Info.HasCDATA = CDATACheckBox.Checked;
			CDATAGroupBox.Enabled = CDATACheckBox.Checked;
		}

		private void textCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Info.HasText = textCheckBox.Checked;
			textGroupBox.Enabled = textCheckBox.Checked;
		}

		private void CDATAOptionalCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Info.CDATAIsOptional = CDATAOptionalCheckBox.Checked;
		}

		private void textOptionalCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Info.TextIsOptional = textOptionalCheckBox.Checked;
		}

		private void dataTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(!Enum.IsDefined(typeof(DataType), (string)dataTypeComboBox.SelectedItem))
			{
				MessageBox.Show(string.Format("The data type specified {0} is not a valid C# data type", (string)dataTypeComboBox.SelectedItem), "Incorrect Data Type", MessageBoxButtons.OK, MessageBoxIcon.Error);
				dataTypeComboBox.SelectedItem = Enum.GetName(typeof(DataType), Info.TextDataType);
				return;
			}

			Info.TextDataType = (DataType)Enum.Parse(typeof(DataType), (string)dataTypeComboBox.SelectedItem);
		}
	}
}
