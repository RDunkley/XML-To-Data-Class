//********************************************************************************************************************************
// Filename:    IntegralType.cs
// Owner:       Richard Dunkley
// Description: Class which represents an integral value in an XML file.
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
using System.Globalization;
using System.Text;

namespace XMLToDataClass.Data.Types
{
	/// <summary>
	///   Adds support for integral types (byte, sbyte, ushort, short, uint, int, ulong, and long).
	/// </summary>
	/// <typeparam name="T">Must be a byte, sbyte, ushort, short, uint, int, ulong, or long.</typeparam>
	public class IntegralType<T> : BaseType, IIntegralType<T> where T : IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
	{
		#region Properties

		public bool AllowHexType1Values { get; set; }

		public bool AllowHexType2Values { get; set; }

		public bool AllowBinaryValues { get; set; }

		public bool AllowIntegerValues { get; set; }

		public static bool DefaultAllowHexType1Values { get; set; }

		public static bool DefaultAllowHexType2Values { get; set; }

		public static bool DefaultAllowBinaryValues { get; set; }

		public static bool DefaultAllowIntegerValues { get; set; }

		/// <summary>
		///   Default maximum value of the integral.
		/// </summary>
		public static T DefaultMaximumValue { get; set; }

		/// <summary>
		///   Default minimum value of the integral.
		/// </summary>
		public static T DefaultMinimumValue { get; set; }

		/// <summary>
		///   Gets or sets the maximum value of the integral.
		/// </summary>
		public T MaximumValue { get; set; }

		/// <summary>
		///   Gets or sets the minimum value of the integral.
		/// </summary>
		public T MinimumValue { get; set; }

		#endregion Properties

		#region Methods

		/// <summary>
		///   Static constructor which assigns the default values.
		/// </summary>
		static IntegralType()
		{
			DefaultAllowHexType1Values = true;
			DefaultAllowHexType2Values = true;
			DefaultAllowBinaryValues = true;
			DefaultAllowIntegerValues = true;
			DefaultMinimumValue = GetMinValue();
			DefaultMaximumValue = GetMaxValue();
		}

		/// <summary>
		///   Instantiates a new <see cref="IntegralType{T}"/> object.
		/// </summary>
		/// <param name="info"><see cref="DataInfo"/> object associated with this type.</param>
		/// <param name="possibleValues">Possible values the data type will have to parse. Can be empty.</param>
		/// <param name="ignoreCase">True if the case of values should be ignored, false if they shouldn't.</param>
		/// <exception cref="ArgumentNullException"><i>possibleValues</i> or <i>info</i> is a null reference.</exception>
		/// <exception cref="ArgumentException">The type <i>T</i> specified is not a valid integral type.</exception>
		public IntegralType(DataInfo info, string[] possibleValues, bool ignoreCase) : base(info, possibleValues, ignoreCase)
		{
			AllowHexType1Values = DefaultAllowHexType1Values;
			AllowHexType2Values = DefaultAllowHexType2Values;
			AllowBinaryValues = DefaultAllowBinaryValues;
			AllowIntegerValues = DefaultAllowIntegerValues;
			MinimumValue = DefaultMinimumValue;
			MaximumValue = DefaultMaximumValue;

			switch (typeof(T).Name.ToLower())
			{
				case "byte":
					Type = DataType.Byte;
					break;
				case "sbyte":
					Type = DataType.SByte;
					break;
				case "uint16":
					Type = DataType.UShort;
					break;
				case "int16":
					Type = DataType.Short;
					break;
				case "uint32":
					Type = DataType.UInt;
					break;
				case "int32":
					Type = DataType.Int;
					break;
				case "uint64":
					Type = DataType.ULong;
					break;
				case "int64":
					Type = DataType.Long;
					break;
				default:
					throw new ArgumentException("The generic type must be created with the following value types as T: byte, sbyte, ushort, short, uint, int, ulong, and long.");
			}
			IsNullable = true;
			DataTypeString = Enum.GetName(typeof(DataType), Type).ToLower();
			string signedString = "signed";
			if (IsUnsigned())
				signedString = "unsigned";
			DisplayName = string.Format("{0}-bit {1} integer", GettMaxBitLength().ToString(), signedString);
			Usings.Add("System.Globalization");
		}

		/// <summary>
		///   Gets the maximum value supported by the type.
		/// </summary>
		/// <returns>Maximum supported value of the type.</returns>
		private static T GetMaxValue()
		{
			switch (typeof(T).Name.ToLower())
			{
				case "byte":
					return (T)(object)byte.MaxValue;
				case "sbyte":
					return (T)(object)sbyte.MaxValue;
				case "uint16":
					return (T)(object)ushort.MaxValue;
				case "int16":
					return (T)(object)short.MaxValue;
				case "uint32":
					return (T)(object)uint.MaxValue;
				case "int32":
					return (T)(object)int.MaxValue;
				case "uint64":
					return (T)(object)ulong.MaxValue;
				case "int64":
					return (T)(object)long.MaxValue;
				default:
					throw new NotImplementedException("The type of this class is not recognized as a supported type");
			}
		}

		/// <summary>
		///   Gets the minimum value supported by the type.
		/// </summary>
		/// <returns>Minimum supported value of the type.</returns>
		private static T GetMinValue()
		{
			switch (typeof(T).Name.ToLower())
			{
				case "byte":
					return (T)(object)byte.MinValue;
				case "sbyte":
					return (T)(object)sbyte.MinValue;
				case "uint16":
					return (T)(object)ushort.MinValue;
				case "int16":
					return (T)(object)short.MinValue;
				case "uint32":
					return (T)(object)uint.MinValue;
				case "int32":
					return (T)(object)int.MinValue;
				case "uint64":
					return (T)(object)ulong.MinValue;
				case "int64":
					return (T)(object)long.MinValue;
				default:
					throw new NotImplementedException("The type of this class is not recognized as a supported type");
			}
		}

		/// <summary>
		///   Gets the maximum length of the integral type in bits.
		/// </summary>
		/// <returns>Bit length of the underlying type (8, 16, 32, or 64).</returns>
		private static int GettMaxBitLength()
		{
			switch (typeof(T).Name.ToLower())
			{
				case "byte":
				case "sbyte":
					return 8;
				case "uint16":
				case "int16":
					return 16;
				case "uint32":
				case "int32":
					return 32;
				case "uint64":
				case "int64":
					return 64;
				default:
					throw new NotImplementedException("The type of this class is not recognized as a supported type");
			}
		}

		/// <summary>
		///   Determines if the integral type is unsigned or not.
		/// </summary>
		/// <returns>True if the integral type is unsigned, false otherwise.</returns>
		private bool IsUnsigned()
		{
			switch (typeof(T).Name.ToLower())
			{
				case "byte":
				case "uint16":
				case "uint32":
				case "uint64":
					return true;
				case "sbyte":
				case "int16":
				case "int32":
				case "int64":
					return false;
				default:
					throw new NotImplementedException("The type of this class is not recognized as a supported type");
			}
		}

		/// <summary>
		///   Generates additional enumerations used by the import or export methods.
		/// </summary>
		/// <returns><see cref="EnumInfo"/> array represnenting additional fields needed by the import/export methods. Can be empty.</returns>
		/// <exception cref="InvalidOperationException">An attempt was made to generate the enums, but either none of the parsing mechanisms are selected or the maximum allowed value is less than the minimum allowed.</exception>
		public override EnumInfo[] GenerateAdditionalEnums()
		{
			if (MaximumValue.CompareTo(MinimumValue) <= 0)
				throw new InvalidOperationException("An attempt was made to generate an integer parsing method, but the maximum value specified is less than or equal to the minimum value specified.");
			if(!AllowHexType1Values && !AllowHexType2Values && !AllowBinaryValues && !AllowIntegerValues)
				throw new InvalidOperationException("An attempt was made to generate a integral type parsing method, but none of the valid parsing methods were allowed. At least one parsing method (Ex: 'binary', 'hexadecimal', or 'integer') must be allowed.");

			int count = 0;
			if (AllowHexType1Values)
				count++;
			if (AllowHexType2Values)
				count++;
			if (AllowBinaryValues)
				count++;
			if (AllowIntegerValues)
				count++;

			if (count < 2)
				return new EnumInfo[0];

			EnumInfo enumInfo = new EnumInfo
			(
				"public",
				string.Format("{0}", GetEnumTypeName(mInfo.PropertyName)),
				string.Format("Represents the formats the {0} can be parsed from", mInfo.PropertyName)
			);

			if (AllowHexType1Values)
				enumInfo.Values.Add(new EnumValueInfo("HexType1", null, "Hexadecimal number ending with an 'h' (Ex: FFh)."));
			if (AllowHexType2Values)
				enumInfo.Values.Add(new EnumValueInfo("HexType2", null, "Hexadecimal number beginning with an '0x' (Ex: 0xFF)."));
			if (AllowBinaryValues)
				enumInfo.Values.Add(new EnumValueInfo("Binary", null, "Binary number ending with an 'b' (Ex: 0110b)."));
			if (AllowIntegerValues)
				enumInfo.Values.Add(new EnumValueInfo("Integer", null, "Integer number (Ex: 1,234, 1234 or -1234)."));
			return new EnumInfo[] { enumInfo };
		}

		/// <summary>
		///   Generates additional properties used by the import or export methods.
		/// </summary>
		/// <returns><see cref="PropertyInfo"/> array representing additional properties needed by the import/export methods. Can be empty.</returns>
		/// <remarks>These properties are typically used to persist state from import to export.</remarks>
		/// <exception cref="InvalidOperationException">An attempt was made to generate the properties, but either none of the parsing mechanisms are selected or the maximum allowed value is less than the minimum allowed.</exception>
		public override PropertyInfo[] GenerateAdditionalProperties()
		{
			if (MaximumValue.CompareTo(MinimumValue) <= 0)
				throw new InvalidOperationException("An attempt was made to generate an integer parsing method, but the maximum value specified is less than or equal to the minimum value specified.");
			if (!AllowHexType1Values && !AllowHexType2Values && !AllowBinaryValues && !AllowIntegerValues)
				throw new InvalidOperationException("An attempt was made to generate a integral type parsing method, but none of the valid parsing methods were allowed. At least one parsing method (Ex: 'binary', 'hexadecimal', or 'integer') must be allowed.");

			List<PropertyInfo> propList = new List<PropertyInfo>();
			if (mInfo.IsOptional && mInfo.CanBeEmpty)
			{
				// Can't tell empty and null apart from null integer so store the info.
				propList.Add(new PropertyInfo
				(
					"public",
					"bool",
					string.Format("{0}NullIsEmpty", mInfo.PropertyName),
					string.Format("True if the null {0} value should be represented as an empty string in XML, false if it shouldn't be included.", mInfo.PropertyName)
				));
			}

			int count = 0;
			if (AllowHexType1Values)
				count++;
			if (AllowHexType2Values)
				count++;
			if (AllowBinaryValues)
				count++;
			if (AllowIntegerValues)
				count++;

			if (count > 1)
			{
				string enumPropertyName = GetEnumPropertyName(mInfo.PropertyName);
				string enumTypeName = GetEnumTypeName(mInfo.PropertyName);

				propList.Add(new PropertyInfo
				(
					"public",
					string.Format("{0}", enumTypeName),
					string.Format("{0}", enumPropertyName),
					string.Format("Determines what format the {0} integral type should be converted to in the XML string", mInfo.PropertyName)
				));
			}
			return propList.ToArray();
		}

		private string GetToStringType()
		{
			switch (typeof(T).Name.ToLower())
			{
				case "sbyte":
				case "byte":
					return "byte";
				case "uint16":
				case "int16":
					return "short";
				case "uint32":
				case "int32":
					return "int";
				case "uint64":
				case "int64":
					return "long";
				default:
					throw new NotImplementedException("The type of this class is not recognized as a supported type");
			}
		}

		/// <summary>
		///   Generates the export method code for the enumerated type.
		/// </summary>
		/// <returns>String array representing the export method code.</returns>
		/// <exception cref="InvalidOperationException">An attempt was made to generate the method, but either none of the parsing mechanisms are selected or the maximum allowed value is less than the minimum allowed.</exception>
		public override string[] GenerateExportMethodCode()
		{
			if (MaximumValue.CompareTo(MinimumValue) <= 0)
				throw new InvalidOperationException("An attempt was made to generate an integer parsing method, but the maximum value specified is less than or equal to the minimum value specified.");
			if (!AllowHexType1Values && !AllowHexType2Values && !AllowBinaryValues && !AllowIntegerValues)
				throw new InvalidOperationException("An attempt was made to generate a integral type parsing method, but none of the valid parsing methods were allowed. At least one parsing method (Ex: 'binary', 'hexadecimal', or 'integer') must be allowed.");

			string enumPropertyName = GetEnumPropertyName(mInfo.PropertyName);
			string enumTypeName = GetEnumTypeName(mInfo.PropertyName);

			List<string> codeLines = new List<string>();

			string name;
			if (mInfo.IsOptional)
			{
				name = string.Format("{0}.Value", mInfo.PropertyName);
				codeLines.Add(string.Format("if(!{0}.HasValue)", mInfo.PropertyName));
				if (mInfo.CanBeEmpty)
				{
					codeLines.Add("{");
					codeLines.Add(string.Format("	if({0}NullIsEmpty)", mInfo.PropertyName));
					codeLines.Add("		return string.Empty;");
					codeLines.Add("	else");
					codeLines.Add("		return null;");
					codeLines.Add("}");
				}
				else
				{
					codeLines.Add("	return null;");
				}
			}
			else
			{
				if (mInfo.CanBeEmpty)
				{
					name = string.Format("{0}.Value", mInfo.PropertyName);
					codeLines.Add(string.Format("if(!{0}.HasValue)", mInfo.PropertyName));
					codeLines.Add("	return string.Empty;");
				}
				else
				{
					name = mInfo.PropertyName;
				}
			}

			int count = 0;
			if (AllowHexType1Values)
				count++;
			if (AllowHexType2Values)
				count++;
			if (AllowBinaryValues)
				count++;
			if (AllowIntegerValues)
				count++;

			codeLines.Add(string.Empty);
			if (count == 1)
			{
				// Only one type is allowed so output in that type.
				if (AllowHexType1Values)
					codeLines.Add(string.Format("return string.Format(\"{{0}}h\", {0}.ToString(\"X\"));", name));
				else if (AllowHexType2Values)
					codeLines.Add(string.Format("return string.Format(\"0x{{0}}\", {0}.ToString(\"X\"));", name));
				else if (AllowBinaryValues)
					codeLines.Add(string.Format("return string.Format(\"{{0}}b\", Convert.ToString(({1}){0}, 2));", name, GetToStringType()));
				else
					codeLines.Add(string.Format("return {0}.ToString();", name));
			}
			else
			{
				int index = 0;
				StringBuilder temp = new StringBuilder();
				temp.Append(string.Format("if({0} == {1}.", enumPropertyName, enumTypeName));
				if (AllowHexType1Values)
				{
					temp.Append("HexType1)");
					codeLines.Add(temp.ToString());
					temp.Clear();
					codeLines.Add(string.Format("	return string.Format(\"{{0}}h\", {0}.ToString(\"X\"));", name));
					index++;
					if (index + 1 == count)
						codeLines.Add("	else");
					else
						temp.Append(string.Format("else if({0} == {1}.", enumPropertyName, enumTypeName));
				}
				if (AllowHexType2Values)
				{
					if (index + 1 != count)
					{
						temp.Append("HexType2)");
						codeLines.Add(temp.ToString());
						temp.Clear();
					}
					codeLines.Add(string.Format("	return string.Format(\"0x{{0}}\", {0}.ToString(\"X\"));", name));
					index++;
					if (index + 1 == count)
						codeLines.Add("	else");
					else
						temp.Append(string.Format("else if({0} == {1}.", enumPropertyName, enumTypeName));
				}
				if (AllowBinaryValues)
				{
					if (index + 1 != count)
					{
						temp.Append("Binary)");
						codeLines.Add(temp.ToString());
						temp.Clear();
					}
					codeLines.Add(string.Format("	return string.Format(\"{{0}}b\", Convert.ToString(({1}){0}, 2));", name, GetToStringType()));
					index++;
					if (index != count)
						codeLines.Add("else");
				}
				if (AllowIntegerValues)
				{
					codeLines.Add(string.Format("	return {0}.ToString();", name));
				}
			}
			return codeLines.ToArray();
		}

		/// <summary>
		///   Generates the import method code for the enumerated type.
		/// </summary>
		/// <returns>String array representing the import method code.</returns>
		/// <exception cref="InvalidOperationException">An attempt was made to generate the method, but either none of the parsing mechanisms are selected or the maximum allowed value is less than the minimum allowed.</exception>
		public override string[] GenerateImportMethodCode()
		{
			if (MaximumValue.CompareTo(MinimumValue) <= 0)
				throw new InvalidOperationException("An attempt was made to generate an integer parsing method, but the maximum value specified is less than or equal to the minimum value specified.");
			if (!AllowHexType1Values && !AllowHexType2Values && !AllowBinaryValues && !AllowIntegerValues)
				throw new InvalidOperationException("An attempt was made to generate a integral type parsing method, but none of the valid parsing methods were allowed. At least one parsing method (Ex: 'binary', 'hexadecimal', or 'integer') must be allowed.");

			List<string> codeLines = new List<string>();
			codeLines.Add("if (value == null)");
			if (mInfo.IsOptional)
			{
				codeLines.Add("{");
				codeLines.Add(string.Format("	{0} = null;", mInfo.PropertyName));
				if (mInfo.CanBeEmpty)
					codeLines.Add(string.Format("	{0}NullIsEmpty = false;", mInfo.PropertyName));
				codeLines.Add("	return;");
				codeLines.Add("}");
			}
			else
			{
				codeLines.Add(string.Format("	throw new InvalidDataException(\"The string value for '{0}' is a null reference.\");", mInfo.Name));
			}

			codeLines.Add("if(value.Length == 0)");
			if (mInfo.CanBeEmpty)
			{
				codeLines.Add("{");
				codeLines.Add(string.Format("	{0} = null;", mInfo.PropertyName));
				if (mInfo.IsOptional)
					codeLines.Add(string.Format("	{0}NullIsEmpty = true;", mInfo.PropertyName));
				codeLines.Add("	return;");
				codeLines.Add("}");
			}
			else
			{
				codeLines.Add(string.Format("	throw new InvalidDataException(\"The string value for '{0}' is an empty string.\");", mInfo.Name));
			}

			int count = 0;
			if (AllowHexType1Values)
				count++;
			if (AllowHexType2Values)
				count++;
			if (AllowBinaryValues)
				count++;
			if (AllowIntegerValues)
				count++;

			string enumPropertyName = GetEnumPropertyName(mInfo.PropertyName);
			string enumTypeName = GetEnumTypeName(mInfo.PropertyName);

			codeLines.Add(string.Format("{0} returnValue = 0;", DataTypeString));
			codeLines.Add("bool parsed = false;");
			codeLines.Add("try");
			codeLines.Add("{");
			bool first = true;
			if (AllowHexType1Values)
			{
				codeLines.Add(string.Empty);
				codeLines.Add("	if (value.Length > 1 && char.ToLower(value[value.Length - 1]) == 'h')");
				codeLines.Add("	{");
				codeLines.Add("		// Number is a hexedecimal type 1 number (FFh).");
				codeLines.Add(string.Format("		returnValue = {0}.Parse(value.Substring(0, value.Length - 1), NumberStyles.AllowHexSpecifier);", DataTypeString));
				if(count > 1)
					codeLines.Add(string.Format("		{0} = {1}.HexType1;", enumPropertyName, enumTypeName));
				codeLines.Add("		parsed = true;");
				codeLines.Add("	}");
				first = false;
			}

			if (AllowHexType2Values)
			{
				codeLines.Add(string.Empty);
				if(first)
					codeLines.Add("	if (value.Length > 2 && value[0] == '0' && char.ToLower(value[1]) == 'x')");
				else
					codeLines.Add("	else if (value.Length > 2 && value[0] == '0' && char.ToLower(value[1]) == 'x')");
				codeLines.Add("	{");
				codeLines.Add("		// Number is specified as a hexedecimal type 2 number (0xFF).");
				codeLines.Add(string.Format("		returnValue = {0}.Parse(value.Substring(2), NumberStyles.AllowHexSpecifier);", DataTypeString));
				if (count > 1)
					codeLines.Add(string.Format("		{0} = {1}.HexType2;", enumPropertyName, enumTypeName));
				codeLines.Add("		parsed = true;");
				codeLines.Add("	}");
				first = false;
			}

			if(AllowBinaryValues)
			{
				codeLines.Add(string.Empty);
				if(first)
					codeLines.Add("	if (value.Length > 1 && char.ToLower(value[value.Length - 1]) == 'b')");
				else
					codeLines.Add("	else if (value.Length > 1 && char.ToLower(value[value.Length - 1]) == 'b')");
				codeLines.Add("	{");
				codeLines.Add("		// Number is a binary number.");
				codeLines.Add(string.Format("		returnValue = Convert.To{0}(value.Substring(0, value.Length - 1), 2);", GetConvertMethodName()));
				if (count > 1)
					codeLines.Add(string.Format("		{0} = {1}.Binary;", enumPropertyName, enumTypeName));
				codeLines.Add("		parsed = true;");
				codeLines.Add("	}");
			}

			if(AllowIntegerValues)
			{
				codeLines.Add(string.Empty);
				string space = string.Empty;
				if (!first)
				{
					codeLines.Add("	else");
					codeLines.Add("	{");
					space = "	";
				}
				codeLines.Add(string.Format("{0}	// Attempt to parse the number as just an integer.", space));
				codeLines.Add(string.Format("{0}	returnValue = {1}.Parse(value, NumberStyles.Integer | NumberStyles.AllowThousands);", space, DataTypeString));
				if (count > 1)
					codeLines.Add(string.Format("{0}	{1} = {2}.Integer;", space, enumPropertyName, enumTypeName));
				codeLines.Add(string.Format("{0}	parsed = true;", space));
				if (!first)
					codeLines.Add("	}");
			}
			codeLines.Add("}");
			codeLines.Add("catch (FormatException e)");
			codeLines.Add("{");
			codeLines.Add(string.Format("	throw new InvalidDataException(string.Format(\"The {0} value specified ({{0}}) is not in a valid {0} string format: {{1}}.\", value, e.Message), e);", DataTypeString));
			codeLines.Add("}");
			codeLines.Add("catch (OverflowException e)");
			codeLines.Add("{");
			codeLines.Add(string.Format("	throw new InvalidDataException(string.Format(\"The {0} value specified ({{0}}) was larger or smaller than a {0} value (Min: {{1}}, Max: {{2}}).\", value, {0}.MinValue.ToString(), {0}.MaxValue.ToString()), e);", DataTypeString));
			codeLines.Add("}");
			codeLines.Add(string.Empty);
			codeLines.Add("if(!parsed)");
			codeLines.Add(string.Format("	throw new InvalidDataException(string.Format(\"The {0} value specified ({{0}}) is not in a valid {0} string format.\", value));", DataTypeString));
			codeLines.Add(string.Empty);
			if (MaximumValue.CompareTo(GetMaxValue()) < 0)
			{
				codeLines.Add(string.Format("// Verify that the {0} value has not excedded the maximum size.", DataTypeString));
				codeLines.Add(string.Format("if(returnValue > {0})", MaximumValue.ToString()));
				codeLines.Add(string.Format("	throw new InvalidDataException(string.Format(\"The {0} value specified ({{0}}) was larger than the maximum value allowed for this type ({1}).\", value));", DataTypeString, MaximumValue.ToString()));
			}
			if (MinimumValue.CompareTo(GetMinValue()) > 0)
			{
				codeLines.Add(string.Format("// Verify that the {0} value is not lower than the minimum size.", DataTypeString));
				codeLines.Add(string.Format("if(returnValue < {0})", MinimumValue.ToString()));
				codeLines.Add(string.Format("	throw new InvalidDataException(string.Format(\"The {0} value specified ({{0}}) was less than the minimum value allowed for this type ({1}).\", value));", DataTypeString, MinimumValue.ToString()));
			}
			codeLines.Add(string.Format("{0} = returnValue;", mInfo.PropertyName));
			return codeLines.ToArray();
		}

		private string GetConvertMethodName()
		{
			switch (typeof(T).Name.ToLower())
			{
				case "byte":
					return "Byte";
				case "sbyte":
					return "SByte";
				case "uint16":
					return "UInt16";
				case "int16":
					return "Int16";
				case "uint32":
					return "UInt32";
				case "int32":
					return "Int32";
				case "uint64":
					return "UInt64";
				case "int64":
					return "Int64";
				default:
					throw new ArgumentException("The generic type must be created with the following value types as T: byte, sbyte, ushort, short, uint, int, ulong, and long.");
			}
		}

		/// <summary>
		///   Gets the enumeration type's name from the property name.
		/// </summary>
		/// <param name="propertyName">Property name of the corresponding <see cref="DataInfo"/> object.</param>
		/// <returns>Enumeration type's name.</returns>
		private string GetEnumTypeName(string propertyName)
		{
			return string.Format("{0}IntegerFormat", propertyName);
		}

		/// <summary>
		///   Gets the stored enumeration property's name.
		/// </summary>
		/// <param name="propertyName">Property name of the corresponding <see cref="DataInfo"/> object.</param>
		/// <returns>Stored enumeration property name.</returns>
		private string GetEnumPropertyName(string propertyName)
		{
			return string.Format("{0}Format", propertyName);
		}

		/// <summary>
		///   Attempts to parse a value to the integral type, using it's current settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		public override bool TryParse(string value)
		{
			return TryParse(value, MinimumValue, MaximumValue, AllowHexType1Values, AllowHexType2Values, AllowBinaryValues, AllowIntegerValues);
		}

		/// <summary>
		///   Attempts to parse the text to the integral value based on the specified <see cref="NumberStyles"/>.
		/// </summary>
		/// <param name="text">Text to be parsed.</param>
		/// <param name="style"><see cref="NumberStyles"/> of the string.</param>
		/// <param name="value">Integral type parsed from the text.</param>
		/// <returns>True if the parsing was successful, false otherwise.</returns>
		private static bool TryParse(string text, NumberStyles style, out T value)
		{
			bool returnValue = false;
			switch (typeof(T).Name.ToLower())
			{
				case "byte":
					byte byteValue;
					returnValue = byte.TryParse(text, style, CultureInfo.CurrentCulture.NumberFormat, out byteValue);
					value = (T)(object)byteValue;
					break;
				case "sbyte":
					sbyte sbyteValue;
					returnValue = sbyte.TryParse(text, style, CultureInfo.CurrentCulture.NumberFormat, out sbyteValue);
					value = (T)(object)sbyteValue;
					break;
				case "uint16":
					ushort ushortValue;
					returnValue = ushort.TryParse(text, style, CultureInfo.CurrentCulture.NumberFormat, out ushortValue);
					value = (T)(object)ushortValue;
					break;
				case "int16":
					short shortValue;
					returnValue = short.TryParse(text, style, CultureInfo.CurrentCulture.NumberFormat, out shortValue);
					value = (T)(object)shortValue;
					break;
				case "uint32":
					uint uintValue;
					returnValue = uint.TryParse(text, style, CultureInfo.CurrentCulture.NumberFormat, out uintValue);
					value = (T)(object)uintValue;
					break;
				case "int32":
					int intValue;
					returnValue = int.TryParse(text, style, CultureInfo.CurrentCulture.NumberFormat, out intValue);
					value = (T)(object)intValue;
					break;
				case "uint64":
					ulong ulongValue;
					returnValue = ulong.TryParse(text, style, CultureInfo.CurrentCulture.NumberFormat, out ulongValue);
					value = (T)(object)ulongValue;
					break;
				case "int64":
					long longValue;
					returnValue = long.TryParse(text, style, CultureInfo.CurrentCulture.NumberFormat, out longValue);
					value = (T)(object)longValue;
					break;
				default:
					throw new NotImplementedException("The type of this class is not recognized as a supported type");
			}
			return returnValue;
		}

		/// <summary>
		///   Attempts to parse the <i>value</i> string to the integral type.
		/// </summary>
		/// <param name="value">String to be parsed.</param>
		/// <param name="maxValue">Maximum value the parsed integral can have.</param>
		/// <param name="minValue">Minimum value the parsed integral can have.</param>
		/// <param name="returnValue">Parsed value or zero if the parsing was unsuccessful.</param>
		/// <returns>True if the value was parsed successfully, false otherwise.</returns>
		private static bool TryParse(string value, T minValue, T maxValue, bool allowHexType1, bool allowHexType2, bool allowBinary, bool allowInteger)
		{
			if (value == null)
				return false;
			if (value.Length == 0)
				return false;

			T returnValue = DefaultMinimumValue;
			bool parsed = false;
			if (allowHexType2 && value.Length > 2 && value[0] == '0' && char.ToLower(value[1]) == 'x')
			{
				// Number is specified as a hexedecimal number.
				if (!TryParse(value.Substring(2), NumberStyles.AllowHexSpecifier, out returnValue))
					return false;
				parsed = true;
			}
			else if (allowHexType1 && value.Length > 1 && char.ToLower(value[value.Length - 1]) == 'h')
			{
				// Number is specified as a hexedecimal number.
				if (!TryParse(value.Substring(0, value.Length - 1), NumberStyles.AllowHexSpecifier, out returnValue))
					return false;
				parsed = true;
			}
			else if (allowBinary && char.ToLower(value[value.Length - 1]) == 'b')
			{
				// Number is specified as a binary number.
				if (!TryParseBitString(value.Substring(0, value.Length - 1), out returnValue))
					return false;
				parsed = true;
			}
			else if(allowInteger)
			{
				// Attempt to parse the number as just an integer.");
				if (!TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, out returnValue))
					return false;
				parsed = true;
			}

			if (!parsed)
				return false;

			if (maxValue.CompareTo(GetMaxValue()) < 0)
			{
				// Verify that the value has not excedded the specified maximum size.
				if (returnValue.CompareTo(maxValue) > 0)
					return false;
			}
			if (minValue.CompareTo(GetMinValue()) > 0)
			{
				// Verify that the value has not exceeded the specified minimum size.
				if (returnValue.CompareTo(minValue) < 0)
					return false;
			}
			return true;
		}

		/// <summary>
		///   Attempts to parse the bit string.
		/// </summary>
		/// <param name="bitString">String of '1's and '0's making up the bitstream.</param>
		/// <param name="value">Parsed integral type or zero if method returns false.</param>
		/// <returns>True if the binary string was successfully parsed, false otherwise.</returns>
		private static bool TryParseBitString(string bitString, out T value)
		{
			value = GetMinValue();

			// Check for an invalid size.
			if (bitString.Length - 1 > GettMaxBitLength())
				return false;

			// Check for invalid characters.
			for (int i = 0; i < bitString.Length; i++)
			{
				if (bitString[i] != '0' && bitString[i] != '1')
					return false;
			}
			try
			{
				value = ParseBitString(bitString);
				return true;
			}
			catch(ArgumentException)
			{
				return false;
			}
		}

		/// <summary>
		///   Attempts to parse the <i>value</i> string to the integral type.
		/// </summary>
		/// <param name="value">String to be parsed.</param>
		/// <returns>True if the value was parsed successfully, false otherwise.</returns>
		/// <remarks>This method is used to parse values for min and max and does not allow hex, or binary string values.</remarks>
		public bool TryParseForMinMax(string value)
		{
			return TryParse(value, GetMinValue(), GetMaxValue(), false, false, false, true);
		}

		/// <summary>
		///   Attempts to parse the <i>value</i> string to the integral type using the default settings.
		/// </summary>
		/// <param name="value">String to be parsed.</param>
		/// <param name="returnValue">Parsed value or zero if the parsing was unsuccessful.</param>
		/// <returns>True if the value was parsed successfully, false otherwise.</returns>
		public static bool TryParseWithDefaults(string value)
		{
			return TryParse(value, DefaultMinimumValue, DefaultMaximumValue, DefaultAllowHexType1Values, DefaultAllowHexType2Values, DefaultAllowBinaryValues, DefaultAllowIntegerValues);
		}

		/// <summary>
		///   Parses the <i>value</i> string and returns the parsed value.
		/// </summary>
		/// <param name="value">String to be parsed.</param>
		/// <returns>Value parsed from the string.</returns>
		/// <exception cref="ArgumentNullException"><i>value</i> is a null reference.</exception>
		/// <exception cref="ArgumentException">The string provided was not valid.</exception>
		public T Parse(string value)
		{
			return Parse(value, MinimumValue, MaximumValue, AllowHexType1Values, AllowHexType2Values, AllowBinaryValues, AllowIntegerValues);
		}

		/// <summary>
		///   Parses the <i>text</i> string and returns the parsed value.
		/// </summary>
		/// <param name="text">Text to be parsed</param>
		/// <param name="style"><see cref="NumberStyles"/> representing the style of the text to be parsed.</param>
		/// <returns>Value parsed from the string.</returns>
		/// <exception cref="FormatException"><i>text</i> is not of the correct format.</exception>
		/// <exception cref="OverflowException"><i>text</i> represents a number less than the minimum value for that type or a number greater than the maximum value for that type.</exception>
		private static T Parse(string text, NumberStyles style)
		{
			switch (typeof(T).Name.ToLower())
			{
				case "byte":
					return (T)(object)byte.Parse(text, style);
				case "sbyte":
					return (T)(object)sbyte.Parse(text, style);
				case "uint16":
					return (T)(object)ushort.Parse(text, style);
				case "int16":
					return (T)(object)short.Parse(text, style);
				case "uint32":
					return (T)(object)uint.Parse(text, style);
				case "int32":
					return (T)(object)int.Parse(text, style);
				case "uint64":
					return (T)(object)ulong.Parse(text, style);
				case "int64":
					return (T)(object)long.Parse(text, style);
				default:
					throw new NotImplementedException("The type of this class is not recognized as a supported type");
			}
		}

		/// <summary>
		///   Parses the <i>value</i> string and returns the parsed value.
		/// </summary>
		/// <param name="value">String to be parsed.</param>
		/// <param name="maxValue">Maximum value the parsed integral can have.</param>
		/// <param name="minValue">Minimum value the parsed integral can have.</param>
		/// <returns>Value parsed from the string.</returns>
		/// <exception cref="ArgumentNullException"><i>value</i> is a null reference.</exception>
		/// <exception cref="ArgumentException">The string provided was not valid.</exception>
		private static T Parse(string value, T minValue, T maxValue, bool allowHexType1, bool allowHexType2, bool allowBinary, bool allowInteger)
		{
			if (value == null)
				throw new ArgumentNullException("value");
			if (value.Length == 0)
				throw new ArgumentException("value is an empty string");

			T returnValue = DefaultMinimumValue;
			bool parsed = false;
			try
			{
				if (allowHexType2 && value.Length > 2 && value[0] == '0' && char.ToLower(value[1]) == 'x')
				{
					// Number is specified as a hexedecimal number.
					returnValue = Parse(value.Substring(2), NumberStyles.AllowHexSpecifier);
					parsed = true;
				}
				else if (allowHexType1 && value.Length > 1 && char.ToLower(value[value.Length - 1]) == 'h')
				{
					// Number is specified as a hexedecimal number.
					returnValue = Parse(value.Substring(0, value.Length - 1), NumberStyles.AllowHexSpecifier);
					parsed = true;
				}
				else if (allowBinary && char.ToLower(value[value.Length - 1]) == 'b')
				{
					// Number is specified as a binary number.
					returnValue = ParseBitString(value.Substring(0, value.Length - 1));
					parsed = true;
				}
				else if(allowInteger)
				{
					// Attempt to parse the number as just an integer.
					returnValue = Parse(value, NumberStyles.Integer | NumberStyles.AllowThousands);
					parsed = true;
				}
			}
			catch (FormatException e)
			{
				throw new ArgumentException(string.Format("The integral value specified ({0}) is not in a valid format: {1}.", value, e.Message), e);
			}
			catch (OverflowException e)
			{
				throw new ArgumentException(string.Format("The integral value specified ({0}) is larger or smaller than the integral type allows (Min: {1}, Max: {2}).", value, GetMinValue().ToString(), GetMaxValue().ToString()), e);
			}

			if(!parsed)
				throw new ArgumentException(string.Format("The integral value specified ({0}) is not in a valid format.", value));

			if (maxValue.CompareTo(GetMaxValue()) < 0)
			{
				// Verify that the value has not excedded the specified maximum size.
				if (returnValue.CompareTo(maxValue) > 0)
					throw new ArgumentException(string.Format("The integral value specified ({0}) is larger than the maximum value allowed ({1}).", value, GetMaxValue().ToString()));
			}
			if (minValue.CompareTo(GetMinValue()) > 0)
			{
				// Verify that the value has not exceeded the specified minimum size.
				if (returnValue.CompareTo(minValue) < 0)
					throw new ArgumentException(string.Format("The integral value specified ({0}) is smaller than the minimal value allowed ({1}).", value, GetMinValue().ToString()));
			}
			return returnValue;
		}

		/// <summary>
		///   Parses the bit string to the integral type.
		/// </summary>
		/// <param name="bitString">String of '1's and '0's making up the bitstream.</param>
		/// <returns>Parsed integral type.</returns>
		/// <exception cref="ArgumentException"><i>bitString</i> length is longer than the number of bits allowed in the integral type or one of the characters is not a '1' or '0'.</exception>
		private static T ParseBitString(string bitString)
		{
			string typeName = typeof(T).Name.ToLower();

			// Check for an invalid size.
			int maxBitSize = GettMaxBitLength();
			if (bitString.Length - 1 > maxBitSize)
				throw new ArgumentException(string.Format("The value specified ({0}) was determined to be a binary type but had more bits ({1}) than can be contained in the {2} type ({3}).", bitString, bitString.Length, typeName, maxBitSize));

			// Check for invalid characters.
			for (int i = 0; i < bitString.Length; i++)
			{
				if (bitString[i] != '0' && bitString[i] != '1')
					throw new ArgumentException(string.Format("The value specified ({0}) was determined to be a binary type but had a character ({1}) at index {2} than is not a '1' or '0'.", bitString, bitString[i], i));
			}

			switch (typeName)
			{
				case "byte":
					return (T)(object)Convert.ToByte(bitString, 2);
				case "sbyte":
					return (T)(object)Convert.ToSByte(bitString, 2);
				case "uint16":
					return (T)(object)Convert.ToUInt16(bitString, 2);
				case "int16":
					return (T)(object)Convert.ToInt16(bitString, 2);
				case "uint32":
					return (T)(object)Convert.ToUInt32(bitString, 2);
				case "int32":
					return (T)(object)Convert.ToInt32(bitString, 2);
				case "uint64":
					return (T)(object)Convert.ToUInt64(bitString, 2);
				case "int64":
					return (T)(object)Convert.ToInt64(bitString, 2);
				default:
					throw new NotImplementedException("The type of this class is not recognized as a supported type");
			}
		}

		/// <summary>
		///   Parses the <i>value</i> string and returns the parsed value.
		/// </summary>
		/// <param name="value">String to be parsed.</param>
		/// <returns>Value parsed from the string.</returns>
		/// <remarks>This method is used to parse values for min and max and does not allow hex, or binary string values.</remarks>
		/// <exception cref="ArgumentNullException"><i>value</i> is a null reference.</exception>
		/// <exception cref="ArgumentException">The string provided was not valid.</exception>
		public T ParseForMinMax(string value)
		{
			return Parse(value, GetMinValue(), GetMaxValue(), false, false, false, true);
		}

		/// <summary>
		///   Parses the <i>value</i> string using the default settings and returns the parsed value.
		/// </summary>
		/// <param name="value">String to be parsed.</param>
		/// <returns>Value parsed from the string.</returns>
		/// <exception cref="ArgumentException">The string provided was not valid.</exception>
		public static T ParseWithDefaults(string value)
		{
			return Parse(value, DefaultMinimumValue, DefaultMaximumValue, DefaultAllowHexType1Values, DefaultAllowHexType2Values, DefaultAllowBinaryValues, DefaultAllowIntegerValues);
		}

		#endregion Methods
	}
}
