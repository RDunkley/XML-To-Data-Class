using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XMLToDataClass.View
{
	public partial class ViewValuesForm : Form
	{
		public ViewValuesForm(string title, string[] values)
		{
			InitializeComponent();

			List<string> uniqueValues = new List<string>();
			foreach (string value in values)
			{
				if (!uniqueValues.Contains(value))
				{
					uniqueValues.Add(value);
					mainListBox.Items.Add(value);
				}
			}
			this.Text = title;
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
