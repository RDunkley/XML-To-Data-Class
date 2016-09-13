//********************************************************************************************************************************
// Filename:    TimeSpanSettings.cs
// Owner:       Richard Dunkley
// Description: Partial class containing the duration of time Type Settings GUI.
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
using System.Globalization;

namespace XMLToDataClass.View
{
	public partial class TimeSpanSettings : UserControl
	{
		private TimeSpanType mType;

		public event EventHandler SettingsChanged;

		public TimeSpanSettings(TimeSpanType type)
		{
			if (type == null)
				throw new ArgumentNullException("type");
			mType = type;

			InitializeComponent();

			CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
			foreach(CultureInfo info in cultures)
				cultureComboBox.Items.Add(info.Name);
			cultureComboBox.SelectedIndex = cultureComboBox.Items.IndexOf(mType.Culture.Name);
		}

		private void cultureComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			mType.Culture = CultureInfo.GetCultureInfo((string)cultureComboBox.Items[cultureComboBox.SelectedIndex]);

			SettingsChanged?.Invoke(this, null);
		}
	}
}
