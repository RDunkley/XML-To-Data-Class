//********************************************************************************************************************************
// Filename:    ElementInfoPanel.cs
// Owner:       Richard Dunkley
// Description: Partial class containing the element information GUI panel.
//********************************************************************************************************************************
// Copyright © Richard Dunkley 2016
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the
// License. You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0  Unless required by applicable
// law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and
// limitations under the License.
//********************************************************************************************************************************
using System;
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
