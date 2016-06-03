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
	public partial class BooleanTypeSettings : UserControl
	{
		private BooleanType mType;

		public event EventHandler SettingsChanged;

		public BooleanTypeSettings(BooleanType type)
		{
			if (type == null)
				throw new ArgumentNullException("type");
			mType = type;

			InitializeComponent();

			allowTrueFalseCheckBox.Checked = type.AllowTrueFalseStrings;
			allowZeroOneCheckBox.Checked = type.AllowZeroOneStrings;
			allowYesNoCheckBox.Checked = type.AllowYesNoStrings;
		}

		private void allowTrueFalseCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mType.AllowTrueFalseStrings = allowTrueFalseCheckBox.Checked;

			SettingsChanged?.Invoke(this, null);
		}

		private void allowZeroOneCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mType.AllowZeroOneStrings = allowZeroOneCheckBox.Checked;

			SettingsChanged?.Invoke(this, null);
		}

		private void allowYesNoCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			mType.AllowYesNoStrings = allowYesNoCheckBox.Checked;

			SettingsChanged?.Invoke(this, null);
		}
	}
}
