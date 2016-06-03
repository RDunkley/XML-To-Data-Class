using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XMLToDataClass.Data.Types;

namespace XMLToDataClass.View
{
	public partial class StringTypeSettings : UserControl
	{
		public event EventHandler SettingsChanged;

		private StringType mType;

		public StringTypeSettings(StringType type)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			InitializeComponent();

			mType = type;
			minimumTextBox.Text = type.MinimumLength.ToString();
			maximumTextBox.Text = type.MaximumLength.ToString();
		}

		private void minimumTextBox_TextChanged(object sender, EventArgs e)
		{
			int value;
			if(!int.TryParse(minimumTextBox.Text, out value))
			{
				// Revert to previous value.
				minimumTextBox.Text = mType.MinimumLength.ToString();
				return;
			}
			if(value < 0)
			{
				// Revert to previous value.
				minimumTextBox.Text = mType.MinimumLength.ToString();
				return;
			}
			mType.MinimumLength = value;
			SettingsChanged?.Invoke(this, null);
		}

		private void maximumTextBox_TextChanged(object sender, EventArgs e)
		{
			int value;
			if (!int.TryParse(maximumTextBox.Text, out value))
			{
				// Revert to previous value.
				maximumTextBox.Text = mType.MaximumLength.ToString();
				return;
			}
			if (value < 0)
			{
				// Revert to previous value.
				maximumTextBox.Text = mType.MaximumLength.ToString();
				return;
			}
			mType.MaximumLength = value;
			SettingsChanged?.Invoke(this, null);
		}
	}
}
