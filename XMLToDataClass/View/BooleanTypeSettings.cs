//********************************************************************************************************************************
// Filename:    BooleanTypeSettings.cs
// Owner:       Richard Dunkley
// Description: Partial class containing the Boolean Type Settings GUI.
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
	public partial class BooleanTypeSettings : UserControl
	{
		private BooleanType mType;

		public event EventHandler SettingsChanged;

		public BooleanTypeSettings(BooleanType type)
		{
			if (type == null)
				throw new ArgumentNullException("type");
			mType = type;

			InitializeComponent();

			allowTrueFalseCheckBox.Checked = type.AllowTrueFalseStrings;
			allowZeroOneCheckBox.Checked = type.AllowZeroOneStrings;
			allowYesNoCheckBox.Checked = type.AllowYesNoStrings;
		}

		private void allowTrueFalseCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mType.AllowTrueFalseStrings = allowTrueFalseCheckBox.Checked;

			SettingsChanged?.Invoke(this, null);
		}

		private void allowZeroOneCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mType.AllowZeroOneStrings = allowZeroOneCheckBox.Checked;

			SettingsChanged?.Invoke(this, null);
		}

		private void allowYesNoCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mType.AllowYesNoStrings = allowYesNoCheckBox.Checked;

			SettingsChanged?.Invoke(this, null);
		}
	}
}
