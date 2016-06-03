using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XMLToDataClass.Data;
using XMLToDataClass.Data.Types;
using System.Globalization;

namespace XMLToDataClass.View
{
	public partial class IntegralSettings<T> : UserControl where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
	{
		private IIntegralType<T> mType;

		public event EventHandler SettingsChanged;

		public IntegralSettings(IIntegralType<T> type)
		{
			if (typeof(T) != typeof(byte) && typeof(T) != typeof(sbyte) && typeof(T) != typeof(ushort) && typeof(T) != typeof(short) &&
				typeof(T) != typeof(uint) && typeof(T) != typeof(int) && typeof(T) != typeof(ulong) && typeof(T) != typeof(long))
			{
				throw new ArgumentException("The generic type must be created with the following value types as T: byte, sbyte, ushort, short, uint, int, ulong, and long.");
			}

			if (type == null)
				throw new ArgumentNullException("type");
			mType = type;

			InitializeComponent();

			minimumTextBox.Text = type.MinimumValue.ToString("N0", NumberFormatInfo.CurrentInfo);
			maximumTextBox.Text = type.MaximumValue.ToString("N0", NumberFormatInfo.CurrentInfo);
			hex1CheckBox.Checked = type.AllowHexType1Values;
			hex2CheckBox.Checked = type.AllowHexType2Values;
			binaryCheckBox.Checked = type.AllowBinaryValues;
			integerCheckBox.Checked = type.AllowIntegerValues;
		}

		private void minimumTextBox_TextChanged(object sender, EventArgs e)
		{
			if(!mType.TryParseForMinMax(minimumTextBox.Text))
			{
				// Revert to previous value.
				minimumTextBox.Text = mType.MinimumValue.ToString("N0", NumberFormatInfo.CurrentInfo);
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
				maximumTextBox.Text = mType.MaximumValue.ToString("N0", NumberFormatInfo.CurrentInfo);
				return;
			}
			else
			{
				// Parse value and assign to minimum value.
				mType.MaximumValue = mType.ParseForMinMax(maximumTextBox.Text);
			}

			SettingsChanged?.Invoke(this, null);
		}

		private void hex1CheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mType.AllowHexType1Values = hex1CheckBox.Checked;
			SettingsChanged?.Invoke(this, null);
		}

		private void hex2CheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mType.AllowHexType2Values = hex2CheckBox.Checked;
			SettingsChanged?.Invoke(this, null);
		}

		private void binaryCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mType.AllowBinaryValues = binaryCheckBox.Checked;
			SettingsChanged?.Invoke(this, null);
		}

		private void integerCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mType.AllowIntegerValues = integerCheckBox.Checked;
			SettingsChanged?.Invoke(this, null);
		}
	}
}
