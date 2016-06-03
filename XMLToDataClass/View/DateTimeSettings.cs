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
