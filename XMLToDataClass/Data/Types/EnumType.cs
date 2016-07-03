//********************************************************************************************************************************
// Filename:    EnumType.cs
// Owner:       Richard Dunkley
// Description: Class which represents a enumerated value in an XML file.
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

namespace XMLToDataClass.Data.Types
{
	public class EnumType : BaseType
	{
		#region Properties

		public Dictionary<string, string> TypeLookup;

		#endregion Properties

		#region Methods

		static EnumType()
		{
		}

		/// <summary>
		///   Instantiates a new <see cref="EnumType"/> object.
		/// </summary>
		/// <param name="info"><see cref="DataInfo"/> object associated with this type.</param>
		/// <param name="possibleValues">Possible values the data type will have to parse. Can be empty.</param>
		/// <param name="ignoreCase">True if the case of values should be ignored, false if they shouldn't.</param>
		/// <exception cref="ArgumentNullException"><i>possibleValues</i> or <i>info</i> is a null reference.</exception>
		public EnumType(DataInfo info, string[] possibleValues, bool ignoreCase) : base(info, possibleValues, ignoreCase)
		{
			Type = DataType.Enum;
			IsNullable = true;
			DataTypeString = GetEnumTypeName(mInfo.PropertyName);
			DisplayName = "Custom Enumeration";

			List<string> valueList = new List<string>();
			foreach(string value in possibleValues)
			{
				string tempValue = value;
				if (mIgnoreCase)
					tempValue = value.ToLower();
				if (!valueList.Contains(tempValue))
					valueList.Add(tempValue);
			}

			List<string> nameList = new List<string>(valueList.Count);
			foreach(string value in valueList)
			{
				string name = StringUtility.GetUpperCamelCase(value);
				if (nameList.Contains(name))
					name = CreateAlternativeName(name, nameList);
				nameList.Add(name);
			}

			TypeLookup = new Dictionary<string, string>();
			for(int i = 0; i < valueList.Count; i++)
				TypeLookup.Add(valueList[i], nameList[i]);
		}

		private string CreateAlternativeName(string duplicateName, List<string> names)
		{
			int index = 1;
			string name = duplicateName;
			while(names.Contains(name))
			{
				name = string.Format("{0}{1}", duplicateName, index.ToString());
				index++;
			}
			return name;
		}

		/// <summary>
		///   Generates additional enumerations used by the import or export methods.
		/// </summary>
		/// <returns><see cref="EnumInfo"/> array represnenting additional fields needed by the import/export methods. Can be empty.</returns>
		/// <exception cref="InvalidOperationException">An attempt was made to generate a boolean parsing method, but none of the valid boolean string pairs were allowed. At least one boolean string pair (Ex: 'true/false', 'yes/no', or '0/1') must be allowed.</exception>
		public override EnumInfo[] GenerateAdditionalEnums()
		{
			if (TypeLookup.Count == 0)
				throw new InvalidOperationException("An attempt was made to generate enum code, but the number of enumerated items is zero.");
			ValidateLookup();

			EnumInfo enumInfo = new EnumInfo
			(
				"public",
				GetEnumTypeName(mInfo.PropertyName),
				string.Format("Enumerates the possible values of {0}", mInfo.PropertyName)
			);

			foreach(string value in TypeLookup.Keys)
			{
				enumInfo.Values.Add(new EnumValueInfo(TypeLookup[value], null, string.Format("Represents the '{0}' string.", value)));
			}

			return new EnumInfo[] { enumInfo };
		}

		private void ValidateLookup()
		{
			foreach(string key in TypeLookup.Keys)
			{
				string value = TypeLookup[key];

				if (value == null)
					throw new InvalidOperationException(string.Format("An attempt was made to generate code, but one of the enumerated item names belonging to '{0}' is a null reference ", key));

				if (value.Length == 0)
					throw new InvalidOperationException(string.Format("An attempt was made to generate code, but one of the enumerated item names belonging to '{0}' is an empty string.", key));

				if (!StringUtility.IsValidCSharpIdentifier(value))
					throw new InvalidOperationException(string.Format("An attempt was made to generate code, but one of the enumerated item names belonging to '{0}' is not a valid C# identifier.", key));
			}
		}

		/// <summary>
		///   Generates additional properties used by the import or export methods.
		/// </summary>
		/// <returns><see cref="PropertyInfo"/> array representing additional properties needed by the import/export methods. Can be empty.</returns>
		/// <remarks>These properties are typically used to persist state from import to export.</remarks>
		/// <exception cref="InvalidOperationException">An attempt was made to generate a boolean parsing method, but none of the valid boolean string pairs were allowed. At least one boolean string pair (Ex: 'true/false', 'yes/no', or '0/1') must be allowed.</exception>
		public override PropertyInfo[] GenerateAdditionalProperties()
		{
			ValidateLookup();

			List<PropertyInfo> propList = new List<PropertyInfo>();
			if (mInfo.IsOptional && mInfo.CanBeEmpty)
			{
				// Can't tell empty and null apart from null enum so store the info.
				propList.Add(new PropertyInfo
				(
					"public",
					"bool",
					string.Format("{0}NullIsEmpty", mInfo.PropertyName),
					string.Format("True if the null {0} enumeration should be represented as an empty string in XML, false if it shouldn't be included.", mInfo.PropertyName)
				));
			}
			return propList.ToArray();
		}

		/// <summary>
		///   Generates the export method code for the boolean.
		/// </summary>
		/// <returns>String array representing the export method code.</returns>
		/// <exception cref="InvalidOperationException">An attempt was made to generate a boolean parsing method, but none of the valid boolean string pairs were allowed. At least one boolean string pair (Ex: 'true/false', 'yes/no', or '0/1') must be allowed.</exception>
		public override string[] GenerateExportMethodCode()
		{
			if (TypeLookup.Count == 0)
				throw new InvalidOperationException("An attempt was made to generate code for an enumerated type, but the number of enumerated items is zero.");
			ValidateLookup();

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

			string enumTypeName = GetEnumTypeName(mInfo.PropertyName);
			codeLines.Add(string.Empty);
			codeLines.Add(string.Format("switch({0})", mInfo.PropertyName));
			codeLines.Add("{");
			foreach(string value in TypeLookup.Keys)
			{
				codeLines.Add(string.Format("	case {0}.{1}:", enumTypeName, TypeLookup[value]));
				codeLines.Add(string.Format("		return \"{0}\";", value));
			}
			codeLines.Add("	default:");
			codeLines.Add("		throw new NotImplementedException(\"The enumerated type was not recognized as a supported type.\");");
			codeLines.Add("}");
			return codeLines.ToArray();
		}

		/// <summary>
		///   Generates the import method code for the boolean.
		/// </summary>
		/// <returns>String array representing the import method code.</returns>
		/// <exception cref="InvalidOperationException">An attempt was made to generate a boolean parsing method, but none of the valid boolean string pairs were allowed. At least one boolean string pair (Ex: 'true/false', 'yes/no', or '0/1') must be allowed.</exception>
		public override string[] GenerateImportMethodCode()
		{
			if (TypeLookup.Count == 0)
				throw new InvalidOperationException("An attempt was made to generate code for an enumerated type, but the number of enumerated items is zero.");
			ValidateLookup();

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

			string boolString = "true";
			if (!mIgnoreCase)
				boolString = "false";

			string enumTypeName = GetEnumTypeName(mInfo.PropertyName);
			foreach (string key in TypeLookup.Keys)
			{
				codeLines.Add(string.Format("if (string.Compare(value, \"{0}\", {1}) == 0)", key, boolString));
				codeLines.Add("{");
				codeLines.Add(string.Format("	{0} = {1}.{2};", mInfo.PropertyName, enumTypeName, TypeLookup[key]));
				codeLines.Add("	return;");
				codeLines.Add("}");
			}
			codeLines.Add(string.Format("throw new InvalidDataException(string.Format(\"The enum value specified ({{0}}) is not a recognized enumerated type for {0}.\", value));", mInfo.Name));
			return codeLines.ToArray();
		}

		/// <summary>
		///   Gets the enumeration type's name from the property name.
		/// </summary>
		/// <param name="propertyName">Property name of the corresponding <see cref="DataInfo"/> object.</param>
		/// <returns>Enumeration type's name.</returns>
		private string GetEnumTypeName(string propertyName)
		{
			return string.Format("{0}Enum", propertyName);
		}

		/// <summary>
		///   Attempts to parse a value to the enumeration, using it's current settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		public override bool TryParse(string value)
		{
			return TryParse(value, TypeLookup, mIgnoreCase);
		}

		/// <summary>
		///   Attempts to parse a value to the enumeration, using the specified settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <param name="typeLookup">Lookup table to determine if 'value' matches a enumerated item.</param>
		/// <param name="ignoreCase">True if the case should be ignored, false otherwise.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		private static bool TryParse(string value, Dictionary<string, string> typeLookup, bool ignoreCase)
		{
			if (value == null)
				return false;
			if (value.Length == 0)
				return false;

			foreach(string key in typeLookup.Keys)
			{
				if (string.Compare(value, key, ignoreCase) == 0)
					return true;
			}
			return false;
		}

		/// <summary>
		///   Attempts to parse a value to a enumeration, using the default settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		public static bool TryParseWithDefaults(string value)
		{
			// No default settings for this type so it always returns false.
			return false;
		}

		#endregion Methods
	}
}
