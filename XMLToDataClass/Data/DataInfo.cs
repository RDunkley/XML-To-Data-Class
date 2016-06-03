/********************************************************************************************************************************
 * Copyright 2014 Richard Dunkley
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
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
		public DataInfo(string name, string[] possibleValues, bool optional, bool canBeEmpty, bool ignoreCase)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (name.Length == 0)
				throw new ArgumentException("name is an empty string");

			Name = name;
			PropertyName = GeneratePropertyName(name);
			CanBeEmpty = canBeEmpty;
			IsOptional = optional;

			// Create the possible data type objects.
			mTypeLookup = new Dictionary<DataType, IDataType>();
			mTypeLookup.Add(DataType.Boolean, new BooleanType(this, possibleValues, ignoreCase));
			mTypeLookup.Add(DataType.Byte, new IntegralType<byte>(this, possibleValues, ignoreCase));
			mTypeLookup.Add(DataType.Double, new FloatingType<double>(this, possibleValues, ignoreCase));

			FloatingType<float> type = new FloatingType<Single>(this, possibleValues, ignoreCase);
			mTypeLookup.Add(DataType.Float, new FloatingType<float>(this, possibleValues, ignoreCase));

			mTypeLookup.Add(DataType.Int, new IntegralType<int>(this, possibleValues, ignoreCase));
			mTypeLookup.Add(DataType.Long, new IntegralType<long>(this, possibleValues, ignoreCase));
			mTypeLookup.Add(DataType.SByte, new IntegralType<sbyte>(this, possibleValues, ignoreCase));
			mTypeLookup.Add(DataType.Short, new IntegralType<short>(this, possibleValues, ignoreCase));
			mTypeLookup.Add(DataType.String, new StringType(this, possibleValues, ignoreCase));
			mTypeLookup.Add(DataType.UInt, new IntegralType<uint>(this, possibleValues, ignoreCase));
			mTypeLookup.Add(DataType.ULong, new IntegralType<ulong>(this, possibleValues, ignoreCase));
			mTypeLookup.Add(DataType.UShort, new IntegralType<ushort>(this, possibleValues, ignoreCase));
			mTypeLookup.Add(DataType.DateTime, new DateTimeType(this, possibleValues, ignoreCase));
			mTypeLookup.Add(DataType.Enum, new EnumType(this, possibleValues, ignoreCase));
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
		/// <returns>Data type string. May return nullable data type if the selected DataType object is nullable and this object is optional or can be empty.</returns>
		public string GetDataTypeString()
		{
			if (SelectedDataTypeObject.IsNullable && (IsOptional || CanBeEmpty))
				return string.Format("{0}?", SelectedDataTypeObject.DataTypeString);
			return SelectedDataTypeObject.DataTypeString;
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

		/// <summary>
		///   Generates the property name from the XML node name.
		/// </summary>
		/// <param name="name">Name of the XML node.</param>
		/// <returns>Generated property name.</returns>
		/// <exception cref="ArgumentNullException"><i>name</i> is a null reference.</exception>
		/// <exception cref="ArgumentException"><i>name</i> is an empty string.</exception>
		private string GeneratePropertyName(string name)
		{
			if (name == null)
				throw new ArgumentNullException("name is a null reference");
			if (name.Length == 0)
				throw new ArgumentException("name is an empty string");

			StringBuilder builder = new StringBuilder();
			bool capNext = true;
			for (int i = 0; i < name.Length; i++)
			{
				if (name[i] == '_')
				{
					capNext = true;
				}
				else
				{
					if (capNext)
					{
						builder.Append(char.ToUpper(name[i]));
						capNext = false;
					}
					else
						builder.Append(name[i]);
				}
			}
			return builder.ToString();
		}

		#endregion Methods
	}
}
