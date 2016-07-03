//********************************************************************************************************************************
// Filename:    TextInputForm.cs
// Owner:       Richard Dunkley
// Description: Partial class containing the Text Input Form.
//********************************************************************************************************************************
// Copyright © Richard Dunkley 2016
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the
// License. You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0  Unless required by applicable
// law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and
// limitations under the License.
//********************************************************************************************************************************
using System.Windows.Forms;

namespace XMLToDataClass.View
{
	public partial class TextInputForm : Form
	{
		public string MainTitle
		{
			get
			{
				return this.Text;
			}
			set
			{
				this.Text = value;
			}
		}

		public string MainText
		{
			get
			{
				return mainLabel.Text;
			}
			set
			{
				mainLabel.Text = value;
			}
		}

		public string Value
		{
			get
			{
				return mainTextBox.Text;
			}
			set
			{
				mainTextBox.Text = value;
			}
		}

		public TextInputForm()
		{
			InitializeComponent();

			okButton.DialogResult = DialogResult.OK;
			cancelButton.DialogResult = DialogResult.Cancel;
		}
	}
}
