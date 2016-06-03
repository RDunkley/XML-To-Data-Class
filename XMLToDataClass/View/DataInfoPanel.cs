using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XMLToDataClass.Data;
using CSCodeGen;
using XMLToDataClass.Data.Types;

namespace XMLToDataClass.View
{
	public partial class DataInfoPanel : UserControl
	{
		# region Properties

		public DataInfo Info { get; private set; }

		#endregion Properties

		public DataInfoPanel(DataInfo info)
		{
			if (info == null)
				throw new ArgumentNullException("info");

			InitializeComponent();

			Info = info;
			List<DataType> supportedTypes = new List<DataType>();
			supportedTypes.AddRange(info.GetSupportedTypes());
			if (supportedTypes.Contains(info.SelectedDataType))
				allTypesCheckBox.Checked = false;
			else
				allTypesCheckBox.Checked = true;

			UpdateComboBox();

			nameLabel.Text = Info.Name;
			propertyNameTextBox.Text = Info.PropertyName;
			optionalCheckBox.Checked = Info.IsOptional;
			emptyCheckBox.Checked = Info.CanBeEmpty;
			errorLabel.Text = string.Empty;
			UpdateError();
		}

		private void UpdateComboBox()
		{
			DataType[] types;
			if (allTypesCheckBox.Checked)
				types = Info.GetAllDataTypes();
			else
				types = Info.GetSupportedTypes();

			dataTypeComboBox.Items.Clear();
			foreach (DataType type in types)
				dataTypeComboBox.Items.Add(Enum.GetName(typeof(DataType), type));
			dataTypeComboBox.SelectedItem = Enum.GetName(typeof(DataType), Info.SelectedDataType);
		}

		private void propertyNameTextBox_TextChanged(object sender, EventArgs e)
		{
			if (!StringUtility.IsValidCSharpIdentifier(propertyNameTextBox.Text))
			{
				MessageBox.Show(string.Format("The property name specified {0} is not a valid C# property name identifier", propertyNameTextBox.Text), "Incorrect Identifier", MessageBoxButtons.OK, MessageBoxIcon.Error);
				propertyNameTextBox.Text = Info.PropertyName;
				return;
			}
			Info.PropertyName = propertyNameTextBox.Text;
		}

		private void dataTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!Enum.IsDefined(typeof(DataType), (string)dataTypeComboBox.SelectedItem))
			{
				MessageBox.Show(string.Format("The data type specified {0} is not a valid C# data type", (string)dataTypeComboBox.SelectedItem), "Incorrect Data Type", MessageBoxButtons.OK, MessageBoxIcon.Error);
				dataTypeComboBox.SelectedItem = Enum.GetName(typeof(DataType), Info.SelectedDataType);
				return;
			}
			Info.SelectedDataType = (DataType)Enum.Parse(typeof(DataType), (string)dataTypeComboBox.SelectedItem);

			typePanel.Controls.Clear();
			UserControl control;
			switch(Info.SelectedDataType)
			{
				case DataType.Boolean:
					control = new BooleanTypeSettings(Info.SelectedDataTypeObject as BooleanType);
					((BooleanTypeSettings)control).SettingsChanged += DataInfoPanel_SettingsChanged;
					break;
				case DataType.Byte:
					control = new IntegralSettings<byte>(Info.SelectedDataTypeObject as IntegralType<byte>);
					((IntegralSettings<byte>)control).SettingsChanged += DataInfoPanel_SettingsChanged;
					break;
				case DataType.Double:
					control = new FloatingPointSettings<double>(Info.SelectedDataTypeObject as FloatingType<double>);
					((FloatingPointSettings<double>)control).SettingsChanged += DataInfoPanel_SettingsChanged;
					break;
				case DataType.Float:
					control = new FloatingPointSettings<float>(Info.SelectedDataTypeObject as FloatingType<float>);
					((FloatingPointSettings<float>)control).SettingsChanged += DataInfoPanel_SettingsChanged;
					break;
				case DataType.Int:
					control = new IntegralSettings<int>(Info.SelectedDataTypeObject as IntegralType<int>);
					((IntegralSettings<int>)control).SettingsChanged += DataInfoPanel_SettingsChanged;
					break;
				case DataType.Long:
					control = new IntegralSettings<long>(Info.SelectedDataTypeObject as IntegralType<long>);
					((IntegralSettings<long>)control).SettingsChanged += DataInfoPanel_SettingsChanged;
					break;
				case DataType.SByte:
					control = new IntegralSettings<sbyte>(Info.SelectedDataTypeObject as IntegralType<sbyte>);
					((IntegralSettings<sbyte>)control).SettingsChanged += DataInfoPanel_SettingsChanged;
					break;
				case DataType.Short:
					control = new IntegralSettings<short>(Info.SelectedDataTypeObject as IntegralType<short>);
					((IntegralSettings<short>)control).SettingsChanged += DataInfoPanel_SettingsChanged;
					break;
				case DataType.String:
					control = new StringTypeSettings(Info.SelectedDataTypeObject as StringType);
					((StringTypeSettings)control).SettingsChanged += DataInfoPanel_SettingsChanged;
					break;
				case DataType.UInt:
					control = new IntegralSettings<uint>(Info.SelectedDataTypeObject as IntegralType<uint>);
					((IntegralSettings<uint>)control).SettingsChanged += DataInfoPanel_SettingsChanged;
					break;
				case DataType.ULong:
					control = new IntegralSettings<ulong>(Info.SelectedDataTypeObject as IntegralType<ulong>);
					((IntegralSettings<ulong>)control).SettingsChanged += DataInfoPanel_SettingsChanged;
					break;
				case DataType.UShort:
					control = new IntegralSettings<ushort>(Info.SelectedDataTypeObject as IntegralType<ushort>);
					((IntegralSettings<ushort>)control).SettingsChanged += DataInfoPanel_SettingsChanged;
					break;
				case DataType.DateTime:
					control = new DateTimeSettings(Info.SelectedDataTypeObject as DateTimeType);
					((DateTimeSettings)control).SettingsChanged += DataInfoPanel_SettingsChanged;
					break;
				case DataType.Enum:
					control = new EnumTypeSettings(Info.SelectedDataTypeObject as EnumType);
					((EnumTypeSettings)control).SettingsChanged += DataInfoPanel_SettingsChanged;
					break;
				default:
					throw new NotImplementedException("The data type specified is not recognized as a valid type");
			}
			typePanel.Controls.Add(control);
			control.Dock = DockStyle.Fill;

			UpdateError();
		}

		private void UpdateError()
		{
			if (Info.SelectedDataTypeObject.HasInvalidValues())
				errorLabel.Text = "*Errors will occur parsing current XML";
			else
				errorLabel.Text = string.Empty;
		}

		private void DataInfoPanel_SettingsChanged(object sender, EventArgs e)
		{
			UpdateError();
		}

		private void optionalCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Info.IsOptional = optionalCheckBox.Checked;
		}

		private void emptyCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Info.CanBeEmpty = emptyCheckBox.Checked;
		}

		private void allTypesCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			UpdateComboBox();
		}
	}
}
