//********************************************************************************************************************************
// Filename:    VersionTypeSettings.cs
// Owner:       Richard Dunkley
// Description: Partial class containing the Version Type Settings GUI.
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
	public partial class VersionTypeSettings : UserControl
	{
		private VersionType mType;

		public event EventHandler SettingsChanged;

		public VersionTypeSettings(VersionType type)
		{
			if (type == null)
				throw new ArgumentNullException("type");
			mType = type;

			InitializeComponent();

			allowBuildCheckBox.Checked = type.AllowBuild;
			allowRevisionCheckBox.Checked = type.AllowRevision;
		}

		private void allowBuildCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mType.AllowBuild = allowBuildCheckBox.Checked;

			SettingsChanged?.Invoke(this, null);
		}

		private void allowRevisionCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mType.AllowRevision = allowRevisionCheckBox.Checked;

			SettingsChanged?.Invoke(this, null);
		}
	}
}
