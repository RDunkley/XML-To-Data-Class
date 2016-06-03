using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XMLToDataClass.Data;
using CSCodeGen;

namespace XMLToDataClass.View
{
	public partial class ElementInfoPanel : UserControl
	{
		#region Properties

		public ElementInfo mInfo { get; private set; }

		#endregion Properties

		public ElementInfoPanel(ElementInfo info)
		{
			if (info == null)
				throw new ArgumentNullException("info");

			InitializeComponent();

			mInfo = info;
			classNameTextBox.Text = info.ClassName;
			textCheckBox.Checked = info.Text.Include;
			CDATACheckBox.Checked = info.CDATA.Include;

			foreach (AttributeInfo attrib in info.Attributes)
				attributeListBox.Items.Add(attrib.Info.Name);
			foreach (ElementInfo child in info.Children)
				elementListBox.Items.Add(child.Name);
		}

		private void classNameTextBox_TextChanged(object sender, EventArgs e)
		{
			if (!StringUtility.IsValidCSharpIdentifier(classNameTextBox.Text))
			{
				MessageBox.Show(string.Format("The class name specified {0} is not a valid C# class name identifier", classNameTextBox.Text), "Incorrect Identifier", MessageBoxButtons.OK, MessageBoxIcon.Error);
				classNameTextBox.Text = mInfo.ClassName;
				return;
			}
			mInfo.ClassName = classNameTextBox.Text;
		}

		private void CDATACheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mInfo.CDATA.Include = CDATACheckBox.Checked;
		}

		private void textCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mInfo.Text.Include = textCheckBox.Checked;
		}
	}
}
