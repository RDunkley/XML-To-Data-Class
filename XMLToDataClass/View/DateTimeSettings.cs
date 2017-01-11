//********************************************************************************************************************************
// Filename:    DateTimeSettings.cs
// Owner:       Richard Dunkley
// Description: Partial class containing the Date/Time Type Settings GUI.
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
	public partial class DateTimeSettings : UserControl
	{
		private DateTimeType mType;

		public event EventHandler SettingsChanged;

		public DateTimeSettings(DateTimeType type)
		{
			if (type == null)
				throw new ArgumentNullException("type");
			mType = type;

			InitializeComponent();

			CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
			foreach(CultureInfo info in cultures)
				cultureComboBox.Items.Add(info.Name);
			cultureComboBox.SelectedIndex = cultureComboBox.Items.IndexOf(mType.Culture.Name);

			mainComboBox.Items.AddRange(Enum.GetNames(typeof(DateTimeType.DateTimeOption)));
			mainComboBox.SelectedIndex = mainComboBox.Items.IndexOf(Enum.GetName(typeof(DateTimeType.DateTimeOption), mType.DateTimeSelect));
		}

		private void mainComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			mType.DateTimeSelect = (DateTimeType.DateTimeOption)Enum.Parse(typeof(DateTimeType.DateTimeOption), (string)mainComboBox.Items[mainComboBox.SelectedIndex]);

			SettingsChanged?.Invoke(this, null);
		}

		private void cultureComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			mType.Culture = CultureInfo.GetCultureInfo((string)cultureComboBox.Items[cultureComboBox.SelectedIndex]);

			SettingsChanged?.Invoke(this, null);
		}
	}
}
