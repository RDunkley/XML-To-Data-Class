//********************************************************************************************************************************
// Filename:    MacAddressTypeSettings.cs
// Owner:       Richard Dunkley
// Description: Partial class containing the MAC Address Type Settings GUI.
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
	public partial class MacAddressTypeSettings : UserControl
	{
		private MacAddressType mType;

		public event EventHandler SettingsChanged;

		public MacAddressTypeSettings(MacAddressType type)
		{
			if (type == null)
				throw new ArgumentNullException("type");
			mType = type;

			InitializeComponent();

			allowColonCheckBox.Checked = type.AllowColonSeparator;
			allowDashCheckBox.Checked = type.AllowDashSeparator;
			allowPeriodCheckBox.Checked = type.AllowDotSeparator;
		}

		private void allowColonCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mType.AllowColonSeparator = allowColonCheckBox.Checked;

			SettingsChanged?.Invoke(this, null);
		}

		private void allowDashCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mType.AllowDashSeparator = allowDashCheckBox.Checked;

			SettingsChanged?.Invoke(this, null);
		}

		private void allowPeriodCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mType.AllowDotSeparator = allowPeriodCheckBox.Checked;

			SettingsChanged?.Invoke(this, null);
		}
	}
}
