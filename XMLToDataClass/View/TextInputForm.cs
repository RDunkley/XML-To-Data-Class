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
