//********************************************************************************************************************************
// Filename:    StringTypeSettings.cs
// Owner:       Richard Dunkley
// Description: Partial class containing the String Type Settings GUI.
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
using XMLToDataClass.Data.Types;

namespace XMLToDataClass.View
{
	public partial class StringTypeSettings : UserControl
	{
		public event EventHandler SettingsChanged;

		private StringType mType;

		public StringTypeSettings(StringType type)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			InitializeComponent();

			mType = type;
			minimumTextBox.Text = type.MinimumLength.ToString();
			maximumTextBox.Text = type.MaximumLength.ToString();
		}

		private void minimumTextBox_TextChanged(object sender, EventArgs e)
		{
			int value;
			if(!int.TryParse(minimumTextBox.Text, out value))
			{
				// Revert to previous value.
				minimumTextBox.Text = mType.MinimumLength.ToString();
				return;
			}
			if(value < 0)
			{
				// Revert to previous value.
				minimumTextBox.Text = mType.MinimumLength.ToString();
				return;
			}
			mType.MinimumLength = value;
			SettingsChanged?.Invoke(this, null);
		}

		private void maximumTextBox_TextChanged(object sender, EventArgs e)
		{
			int value;
			if (!int.TryParse(maximumTextBox.Text, out value))
			{
				// Revert to previous value.
				maximumTextBox.Text = mType.MaximumLength.ToString();
				return;
			}
			if (value < 0)
			{
				// Revert to previous value.
				maximumTextBox.Text = mType.MaximumLength.ToString();
				return;
			}
			mType.MaximumLength = value;
			SettingsChanged?.Invoke(this, null);
		}
	}
}
