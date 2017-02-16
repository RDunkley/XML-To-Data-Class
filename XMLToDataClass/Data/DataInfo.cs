//********************************************************************************************************************************
// Filename:    DataInfo.cs
// Owner:       Richard Dunkley
// Description: Class which represents the data contained in an XML node.
//********************************************************************************************************************************
// Copyright © Richard Dunkley 2016
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the
// License. You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0  Unless required by applicable
// law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and
// limitations under the License.
//********************************************************************************************************************************
using CSCodeGen;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using XMLToDataClass.Data.Types;

namespace XMLToDataClass.Data
{
	/// <summary>
	///   Contains all the information for <see cref="XmlNode"/> data.
	/// </summary>
	public class DataInfo
	{
		#region Fields

		private Dictionary<DataType, IDataType> mTypeLookup;

		#endregion Fields

		#region Properties

		/// <summary>
		///   Currently selected data type of the attribute.
		/// </summary>
		public DataType SelectedDataType { get; set; }

		/// <summary>
		///   Gets the currently selected data type object.
		/// </summary>
		public IDataType SelectedDataTypeObject
		{
			get
			{
				return mTypeLookup[SelectedDataType];
			}
		}

		/// <summary>
		///   True if the attribute can be an empty string, false otherwise.
		/// </summary>
		public bool CanBeEmpty { get; set; }

		/// <summary>
		///   True if the attribute is optional (some elements do not contain the attribute), false otherwise.
		/// </summary>
		public bool IsOptional { get; set; }

		/// <summary>
		///   Name of the attribute.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		///   Name of the attribute property.
		/// </summary>
		public string PropertyName { get; set; }

		#endregion Properties

		#region Methods

		/// <summary>
		///   Instantiates a new <see cref="AttributeInfo"/> object.
		/// </summary>
		/// <param name="name">Name of the attribute.</param>
		public DataInfo(string name, string[] possibleValues, bool optional, bool canBeEmpty)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (name.Length == 0)
				throw new ArgumentException("name is an empty string");

			Name = name;
			PropertyName = StringUtility.GetUpperCamelCase(name);
			CanBeEmpty = canBeEmpty;
			IsOptional = optional;

			// Create the possible data type objects.
			mTypeLookup = new Dictionary<DataType, IDataType>();
			mTypeLookup.Add(DataType.Boolean, new BooleanType(this, possibleValues));
			mTypeLookup.Add(DataType.Byte, new IntegralType<byte>(this, possibleValues));
			mTypeLookup.Add(DataType.Double, new FloatingType<double>(this, possibleValues));

			FloatingType<float> type = new FloatingType<Single>(this, possibleValues);
			mTypeLookup.Add(DataType.Float, new FloatingType<float>(this, possibleValues));

			mTypeLookup.Add(DataType.Int, new IntegralType<int>(this, possibleValues));
			mTypeLookup.Add(DataType.Long, new IntegralType<long>(this, possibleValues));
			mTypeLookup.Add(DataType.SByte, new IntegralType<sbyte>(this, possibleValues));
			mTypeLookup.Add(DataType.Short, new IntegralType<short>(this, possibleValues));
			mTypeLookup.Add(DataType.String, new StringType(this, possibleValues));
			mTypeLookup.Add(DataType.UInt, new IntegralType<uint>(this, possibleValues));
			mTypeLookup.Add(DataType.ULong, new IntegralType<ulong>(this, possibleValues));
			mTypeLookup.Add(DataType.UShort, new IntegralType<ushort>(this, possibleValues));
			mTypeLookup.Add(DataType.DateTime, new DateTimeType(this, possibleValues));
			mTypeLookup.Add(DataType.Enum, new EnumType(this, possibleValues));
			mTypeLookup.Add(DataType.SerialPortParity, new SerialPortParityEnumType(this, possibleValues));
			mTypeLookup.Add(DataType.SerialPortStopBits, new SerialPortStopBitsEnumType(this, possibleValues));
			mTypeLookup.Add(DataType.Version, new VersionType(this, possibleValues));
			mTypeLookup.Add(DataType.TimeSpan, new TimeSpanType(this, possibleValues));
			mTypeLookup.Add(DataType.MACAddress, new MacAddressType(this, possibleValues));
			mTypeLookup.Add(DataType.IPAddress, new IPAddressType(this, possibleValues));
			DetermineDefaultSelectedType();
		}

		private void DetermineDefaultSelectedType()
		{
			int i = 0;
			while(Enum.IsDefined(typeof(DataType), i))
			{
				DataType type = (DataType)i;
				if(!mTypeLookup[type].HasInvalidValues())
				{
					SelectedDataType = type;
					return;
				}
				i++;
			}

			// If all fail (shouldn't, but) default to string.
			SelectedDataType = DataType.String;
		}

		/// <summary>
		///   Returns the data type string of the currently selected DataType object.
		/// </summary>
		/// <returns>Data type string. May return null-able data type if the selected DataType object is null-able and this object is optional or can be empty.</returns>
		public string GetDataTypeString()
		{
			if (SelectedDataTypeObject.IsNullable && (IsOptional || CanBeEmpty))
				return string.Format("{0}?", SelectedDataTypeObject.GetDataTypeString());
			return SelectedDataTypeObject.GetDataTypeString();
		}

		/// <summary>
		///   Returns all the custom usings for the selected data type.
		/// </summary>
		/// <returns></returns>
		public string[] GetSelectedUsings()
		{
			return SelectedDataTypeObject.Usings.ToArray();
		}

		/// <summary>
		///   Gets all the <see cref="DataType"/>s that are supported by all the possible values.
		/// </summary>
		/// <returns>Array of supported <see cref="DataType"/> objects.</returns>
		public DataType[] GetSupportedTypes()
		{
			List<DataType> typeList = new List<DataType>();
			foreach (DataType type in mTypeLookup.Keys)
			{
				if(!mTypeLookup[type].HasInvalidValues())
				{
					// No invalid values so the type must be able to support all the possible values.
					typeList.Add(type);
				}
			}
			return typeList.ToArray();
		}

		/// <summary>
		///   Gets all the <see cref="DataType"/>s that are possible regardless of whether they support all the possible values.
		/// </summary>
		/// <returns>Array of all possible <see cref="DataType"/> objects.</returns>
		public DataType[] GetAllDataTypes()
		{
			List<DataType> typeList = new List<DataType>(mTypeLookup.Count);
			typeList.AddRange(mTypeLookup.Keys);
			return typeList.ToArray();
		}

		public XmlElement Save(XmlDocument doc, XmlNode parent)
		{
			XmlElement element = doc.CreateElement(Name);
			XmlAttribute attrib = element.Attributes.Append(doc.CreateAttribute("SelectedDataType"));
			attrib.Value = Enum.GetName(typeof(DataType), SelectedDataType);

			attrib = element.Attributes.Append(doc.CreateAttribute("CanBeEmpty"));
			attrib.Value = CanBeEmpty.ToString();

			attrib = element.Attributes.Append(doc.CreateAttribute("IsOptional"));
			attrib.Value = IsOptional.ToString();

			attrib = element.Attributes.Append(doc.CreateAttribute("PropertyName"));
			attrib.Value = PropertyName;

			foreach(DataType key in mTypeLookup.Keys)
			{
				// Save each child data type as a separate child element.
				XmlElement child = doc.CreateElement(Enum.GetName(typeof(DataType), key));
				mTypeLookup[key].Save(doc, child);
				element.AppendChild(child);
			}

			parent.AppendChild(element);
			return element;
		}

		public XmlElement Load(XmlNode parent)
		{
			foreach(XmlNode node in parent.ChildNodes)
			{
				if(node.NodeType == XmlNodeType.Element && string.Compare(node.Name, Name, false) == 0)
				{
					XmlAttribute attrib = node.Attributes["SelectedDataType"];
					if(attrib != null)
					{
						DataType dataTypeValue;
						if (Enum.TryParse<DataType>(attrib.Value, out dataTypeValue))
							SelectedDataType = dataTypeValue;
					}

					attrib = node.Attributes["CanBeEmpty"];
					if(attrib != null)
					{
						bool boolValue;
						if (bool.TryParse(attrib.Value, out boolValue))
							CanBeEmpty = boolValue;
					}

					attrib = node.Attributes["IsOptional"];
					if (attrib != null)
					{
						bool boolValue;
						if (bool.TryParse(attrib.Value, out boolValue))
							IsOptional = boolValue;
					}

					attrib = node.Attributes["PropertyName"];
					if(attrib != null)
					{
						PropertyName = attrib.Value;
					}

					// Parse the data type settings.
					foreach(XmlNode child in node.ChildNodes)
					{
						if(child.NodeType == XmlNodeType.Element)
						{
							DataType nodeType;
							if(Enum.TryParse<DataType>(child.Name, out nodeType))
							{
								// Found a data type element so load it.
								mTypeLookup[nodeType].Load(child);
							}
						}
					}

					return node as XmlElement;
				}
			}

			return null;
		}

		#endregion Methods
	}
}
