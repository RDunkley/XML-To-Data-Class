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
	public partial class LoadForm : Form
	{
		public bool PreserveHierarchy
		{
			get
			{
				return hierarchyCheckBox.Checked;
			}
		}
		public bool IgnoreCase
		{
			get
			{
				return caseCheckBox.Checked;
			}
		}

		public string FilePath
		{
			get
			{
				return filePathTextBox.Text;
			}
		}

		public LoadForm()
		{
			InitializeComponent();

			hierarchyCheckBox.Checked = true;
			caseCheckBox.Checked = true;
		}

		private void browseButton_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.CheckFileExists = true;
			dialog.CheckPathExists = true;
			dialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
			dialog.Multiselect = false;
			dialog.Title = "Select XML file to Generate Code For";

			if (dialog.ShowDialog() != DialogResult.OK)
				return;

			filePathTextBox.Text = dialog.FileName;
		}
	}
}
