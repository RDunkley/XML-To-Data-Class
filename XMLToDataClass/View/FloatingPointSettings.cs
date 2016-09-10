//********************************************************************************************************************************
// Filename:    FloatingPointSettings.cs
// Owner:       Richard Dunkley
// Description: Partial class containing the Floating Point Type Settings GUI.
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
	public partial class FloatingPointSettings<T> : UserControl where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
	{
		private IFloatingPointType<T> mType;

		private bool mMinimumValueError = false;
		private bool mMaximumValueError = false;

		public event EventHandler SettingsChanged;

		public FloatingPointSettings(IFloatingPointType<T> type)
		{
			if (typeof(T) != typeof(float) && typeof(T) != typeof(double))
			{
				throw new ArgumentException("The generic type must be created with the following value types as T: float or double.");
			}

			if (type == null)
				throw new ArgumentNullException("type");
			mType = type;

			InitializeComponent();

			minimumTextBox.Text = type.MinimumValue.ToString();
			maximumTextBox.Text = type.MaximumValue.ToString();

			currencyCheckBox.Checked = type.AllowCurrency;
			exponentCheckBox.Checked = type.AllowExponent;
			parenthesesCheckBox.Checked = type.AllowParentheses;
			percentCheckBox.Checked = type.AllowPercent;

			UpdateErrorMessage();
		}

		private void minimumTextBox_TextChanged(object sender, EventArgs e)
		{
			if(!mType.TryParseForMinMax(minimumTextBox.Text))
			{
				// Show the error.
				mMinimumValueError = true;
				UpdateErrorMessage();
				return;
			}
			else
			{
				// Parse value and assign to minimum value.
				mType.MinimumValue = mType.ParseForMinMax(minimumTextBox.Text);
				mMinimumValueError = false;
			}

			UpdateErrorMessage();

			SettingsChanged?.Invoke(this, null);
		}

		private void UpdateErrorMessage()
		{
			if(mMinimumValueError)
			{
				if (mMaximumValueError)
					inputErrorLabel.Text = "Maximum and Minimum Values are not valid floating point values, they will be ignored until fixed";
				else
					inputErrorLabel.Text = "Minimum Value is not a valid floating point value, it will be ignored until fixed";
			}
			else
			{
				if (mMaximumValueError)
					inputErrorLabel.Text = "Maximum Value is not a valid floating point value, it will be ignored until fixed";
				else
					inputErrorLabel.Text = string.Empty;
			}
		}

		private void maximumTextBox_TextChanged(object sender, EventArgs e)
		{
			if (!mType.TryParseForMinMax(maximumTextBox.Text))
			{
				// Show the error.
				mMaximumValueError = true;
				UpdateErrorMessage();
				return;
			}
			else
			{
				// Parse value and assign to minimum value.
				mType.MaximumValue = mType.ParseForMinMax(maximumTextBox.Text);
				mMaximumValueError = false;
			}

			UpdateErrorMessage();

			SettingsChanged?.Invoke(this, null);
		}

		private void currencyCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mType.AllowCurrency = currencyCheckBox.Checked;
			SettingsChanged?.Invoke(this, null);
		}

		private void exponentCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mType.AllowExponent = exponentCheckBox.Checked;
			SettingsChanged?.Invoke(this, null);
		}

		private void parenthesesCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mType.AllowParentheses = parenthesesCheckBox.Checked;
			SettingsChanged?.Invoke(this, null);
		}

		private void percentCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mType.AllowPercent = percentCheckBox.Checked;
			SettingsChanged?.Invoke(this, null);
		}
	}
}
