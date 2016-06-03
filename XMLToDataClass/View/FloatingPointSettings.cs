using System;
using System.Windows.Forms;
using XMLToDataClass.Data.Types;
using System.Globalization;

namespace XMLToDataClass.View
{
	public partial class FloatingPointSettings<T> : UserControl where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
	{
		private IFloatingPointType<T> mType;

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

			minimumTextBox.Text = type.MinimumValue.ToString("N", NumberFormatInfo.CurrentInfo);
			maximumTextBox.Text = type.MaximumValue.ToString("N", NumberFormatInfo.CurrentInfo);

			currencyCheckBox.Checked = type.AllowCurrency;
			exponentCheckBox.Checked = type.AllowExponent;
			parenthesesCheckBox.Checked = type.AllowParentheses;
			percentCheckBox.Checked = type.AllowPercent;
		}

		private void minimumTextBox_TextChanged(object sender, EventArgs e)
		{
			if(!mType.TryParseForMinMax(minimumTextBox.Text))
			{
				// Revert to previous value.
				minimumTextBox.Text = mType.MinimumValue.ToString("N", NumberFormatInfo.CurrentInfo);
				return;
			}
			else
			{
				// Parse value and assign to minimum value.
				mType.MinimumValue = mType.ParseForMinMax(minimumTextBox.Text);
			}

			SettingsChanged?.Invoke(this, null);
		}

		private void maximumTextBox_TextChanged(object sender, EventArgs e)
		{
			if (!mType.TryParseForMinMax(maximumTextBox.Text))
			{
				// Revert to previous value.
				maximumTextBox.Text = mType.MaximumValue.ToString("N", NumberFormatInfo.CurrentInfo);
				return;
			}
			else
			{
				// Parse value and assign to minimum value.
				mType.MaximumValue = mType.ParseForMinMax(maximumTextBox.Text);
			}

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
