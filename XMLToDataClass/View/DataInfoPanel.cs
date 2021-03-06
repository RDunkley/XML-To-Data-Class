﻿//********************************************************************************************************************************
// Filename:    DataInfoPanel.cs
// Owner:       Richard Dunkley
// Description: Partial class containing the data information GUI panel.
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
using System.Collections.Generic;
using System.Windows.Forms;
using XMLToDataClass.Data;
using CSCodeGen;
using XMLToDataClass.Data.Types;
using System.IO.Ports;

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

			accessibilityComboBox.Items.AddRange(Enum.GetNames(typeof(DataInfo.Access)));

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
			summaryTextBox.Text = info.Summary;
			remarksTextBox.Text = info.Remarks;
			accessibilityComboBox.SelectedIndex = accessibilityComboBox.FindString(Enum.GetName(typeof(DataInfo.Access), info.Accessibility));
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
			if(propertyNameTextBox.Text == "Ordinal")
			{
				MessageBox.Show(string.Format("The property name cannot be set to 'Ordinal'. The generated code has this property reserved for the order of the XML elements."), "Incorrect Identifier", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				case DataType.SerialPortParity:
					control = new FixedEnumSettings<Parity>(Info.SelectedDataTypeObject as SerialPortParityEnumType);
					((FixedEnumSettings<Parity>)control).SettingsChanged += DataInfoPanel_SettingsChanged;
					break;
				case DataType.SerialPortStopBits:
					control = new FixedEnumSettings<StopBits>(Info.SelectedDataTypeObject as SerialPortStopBitsEnumType);
					((FixedEnumSettings<StopBits>)control).SettingsChanged += DataInfoPanel_SettingsChanged;
					break;
				case DataType.Version:
					control = new VersionTypeSettings(Info.SelectedDataTypeObject as VersionType);
					((VersionTypeSettings)control).SettingsChanged += DataInfoPanel_SettingsChanged;
					break;
				case DataType.TimeSpan:
					control = new TimeSpanSettings(Info.SelectedDataTypeObject as TimeSpanType);
					((TimeSpanSettings)control).SettingsChanged += DataInfoPanel_SettingsChanged;
					break;
				case DataType.MACAddress:
					control = new MacAddressTypeSettings(Info.SelectedDataTypeObject as MacAddressType);
					((MacAddressTypeSettings)control).SettingsChanged += DataInfoPanel_SettingsChanged;
					break;
				case DataType.IPAddress:
					control = new IPAddressTypeSettings(Info.SelectedDataTypeObject as IPAddressType);
					((IPAddressTypeSettings)control).SettingsChanged += DataInfoPanel_SettingsChanged;
					break;
				case DataType.GUID:
					control = new GUIDTypeSettings(Info.SelectedDataTypeObject as GUIDType);
					((GUIDTypeSettings)control).SettingsChanged += DataInfoPanel_SettingsChanged;
					break;
				default:
					throw new NotImplementedException("The data type specified is not recognized as a valid type");
			}
			typePanel.Controls.Add(control);
			typePanel.MinimumSize = control.MinimumSize;
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

		private void summaryTextBox_TextChanged(object sender, EventArgs e)
		{
			Info.Summary = summaryTextBox.Text;
		}

		private void remarksTextBox_TextChanged(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(remarksTextBox.Text))
				Info.Remarks = null;
			else
				Info.Remarks = remarksTextBox.Text;
		}

		private void accessibilityComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (accessibilityComboBox.SelectedIndex > -1)
				Info.Accessibility = (DataInfo.Access)Enum.Parse(typeof(DataInfo.Access), (string)accessibilityComboBox.Items[accessibilityComboBox.SelectedIndex]);
		}

		private void viewDataButton_Click(object sender, EventArgs e)
		{
			if (Info.PossibleValues == null || Info.PossibleValues.Length == 0)
			{
				MessageBox.Show("No Values to be displayed", "Error Displaying Values", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}

			ViewValuesForm form = new ViewValuesForm(Info.Name + " Values", Info.PossibleValues);
			form.ShowDialog();
			return;
		}
	}
}
