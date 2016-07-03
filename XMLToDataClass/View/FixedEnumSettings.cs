//********************************************************************************************************************************
// Filename:    FixedEnumSettings.cs
// Owner:       Richard Dunkley
// Description: Partial class containing the fixed (C# builting or library referenced) Enumeration Type Settings GUI.
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
	public partial class FixedEnumSettings<T> : UserControl where T : struct, IConvertible
	{
		FixedEnumType<T> mType;

		public event EventHandler SettingsChanged;

		public FixedEnumSettings(FixedEnumType<T> type)
		{
			if (type == null)
				throw new ArgumentException("type");
			mType = type;

			InitializeComponent();

			valuesCheckBox.Checked = type.AllowValues;
			ignoreCheckBox.Checked = type.IgnoreCase;

			foreach(Enum item in Enum.GetValues(typeof(T)))
			{
				mainDataGridView.Rows.Add(Enum.GetName(typeof(T), item), item.ToString("d"));
			}
		}

		private void valuesCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mType.AllowValues = valuesCheckBox.Checked;

			SettingsChanged?.Invoke(this, null);
		}

		private void ignoreCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mType.IgnoreCase = ignoreCheckBox.Checked;

			SettingsChanged?.Invoke(this, null);
		}
	}
}
