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
using System.Text;
using System.Xml;

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
		/// <exception cref="ArgumentNullException"><paramref name="possibleValues"/> or <paramref name="info"/> is a null reference.</exception>
		public EnumType(DataInfo info, string[] possibleValues) : base(info, possibleValues)
		{
			Type = DataType.Enum;
			DisplayName = "Custom Enumeration";

			List<string> valueList = new List<string>();
			foreach(string value in possibleValues)
			{
				string tempValue = value;
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

		/// <summary>
		///   String of the C# representative data type.
		/// </summary>
		/// <returns>String containing the C# data type.</returns>
		public override string GetDataTypeString()
		{
			return GetEnumTypeName(mInfo.PropertyName);
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
		/// <returns><see cref="EnumInfo"/> array representing additional fields needed by the import/export methods. Can be empty.</returns>
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
				string.Format("Enumerates the possible values of {0}.", mInfo.PropertyName)
			);

			// Get all the enumerated type names and their associated strings.
			Dictionary<string, string[]> lookup = GetEnumNamesWithValues();
			foreach(string key in lookup.Keys)
				enumInfo.Values.Add(new EnumValueInfo(key, null, GetEnumDescription(lookup[key])));

			List<EnumInfo> enumList = new List<EnumInfo>();
			enumList.Add(enumInfo);

			foreach(string key in lookup.Keys)
			{
				if(lookup[key].Length > 1)
				{
					// This enumerated item has multiple strings that will parse to it so create an individual enum for it.
					enumInfo = new EnumInfo
					(
						"public",
						string.Format("{0}Items", key),
						string.Format("Enumerates the possible string values that can be parse to {0}.", key)
					);

					for (int i = 0; i < lookup[key].Length; i++)
						enumInfo.Values.Add(new EnumValueInfo(string.Format("Option{0}", i.ToString()), null, string.Format("Specified when the '{0}' string is found in the XML.", lookup[key][i])));
					enumList.Add(enumInfo);
				}
			}

			return enumList.ToArray();
		}

		/// <summary>
		///   Gets a description of the enumerated item.
		/// </summary>
		/// <param name="values">Various string values of the enumerated item.</param>
		/// <returns>Description.</returns>
		private string GetEnumDescription(string[] values)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("Represents the ");
			for (int i = 0; i < values.Length; i++)
			{
				sb.AppendFormat("'{0}'", values[i]);
				if (i != values.Length - 1)
					sb.AppendFormat(", ");
			}
			sb.Append(" string");
			if (values.Length > 1)
				sb.Append("s");
			sb.Append(".");
			return sb.ToString();
		}

		/// <summary>
		///   This method basically inverts the TypeLookup dictionary. Since multiple different keys in the dictionary can
		///   have the same value it recreates a lookup table based on the unique values and stores all the keys that
		///   reference that value.
		/// </summary>
		/// <returns>Lookup of the enumerated type names and their associated strings.</returns>
		private Dictionary<string,string[]> GetEnumNamesWithValues()
		{
			Dictionary<string, List<string>> lookup = new Dictionary<string, List<string>>();
			foreach(string value in TypeLookup.Keys)
			{
				if (!lookup.ContainsKey(TypeLookup[value]))
					lookup.Add(TypeLookup[value], new List<string>());
				lookup[TypeLookup[value]].Add(value);
			}

			Dictionary<string, string[]> returnLookup = new Dictionary<string, string[]>();
			foreach(string key in lookup.Keys)
				returnLookup.Add(key, lookup[key].ToArray());
			return returnLookup;
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

			Dictionary<string, string[]> lookup = GetEnumNamesWithValues();
			foreach(string key in lookup.Keys)
			{
				if(lookup[key].Length > 1)
				{
					propList.Add(new PropertyInfo
					(
						"public",
						string.Format("{0}Items", key),
						string.Format("{0}StringRepresentation", key),
						string.Format("Gets or sets the string representation of '{0}', when it's item is selected.", key)
					));
				}
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
			Dictionary<string, string[]> lookup = GetEnumNamesWithValues();
			codeLines.Add(string.Empty);
			codeLines.Add(string.Format("switch({0})", mInfo.PropertyName));
			codeLines.Add("{");
			foreach(string key in lookup.Keys)
			{
				codeLines.Add(string.Format("	case {0}.{1}:", enumTypeName, key));
				if (lookup[key].Length == 1)
				{
					codeLines.Add(string.Format("		return \"{0}\";", lookup[key][0]));
				}
				else
				{
					codeLines.Add(string.Format("		switch({0})", string.Format("{0}StringRepresentation", key)));
					codeLines.Add("		{");
					for (int i = 0; i < lookup[key].Length; i++)
					{
						codeLines.Add(string.Format("			case {0}Items.Option{1}:", key, i.ToString()));
						codeLines.Add(string.Format("				return \"{0}\";", lookup[key][i]));
					}
					codeLines.Add("			default:");
					codeLines.Add("				throw new NotImplementedException(\"The enumerated type was not recognized as a supported type.\");");
					codeLines.Add("		}");
				}
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

			string enumTypeName = GetEnumTypeName(mInfo.PropertyName);
			Dictionary<string, string[]> lookup = GetEnumNamesWithValues();
			foreach (string key in lookup.Keys)
			{
				for (int i = 0; i < lookup[key].Length; i++)
				{
					codeLines.Add(string.Format("if (string.Compare(value, \"{0}\", false) == 0)", lookup[key][i]));
					codeLines.Add("{");
					codeLines.Add(string.Format("	{0} = {1}.{2};", mInfo.PropertyName, enumTypeName, key));
					if (lookup[key].Length > 1)
						codeLines.Add(string.Format("	{0}StringRepresentation = {0}Items.Option{1};", key, i.ToString()));
					codeLines.Add("	return;");
					codeLines.Add("}");
				}
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
		///   Saves the types configuration properties to XML child elements.
		/// </summary>
		/// <param name="doc"><see cref="XmlDocument"/> object representing the XML document to be written.</param>
		/// <param name="parent">Parent <see cref="XmlNode"/> to append the child settings to.</param>
		public override void Save(XmlDocument doc, XmlNode parent)
		{
			// Add the DateTimeSelect option.
			foreach (string key in TypeLookup.Keys)
			{
				XmlElement element = doc.CreateElement("setting");
				XmlAttribute attrib = element.Attributes.Append(doc.CreateAttribute("name"));
				attrib.Value = key;
				attrib = element.Attributes.Append(doc.CreateAttribute("value"));
				attrib.Value = TypeLookup[key];
				parent.AppendChild(element);
			}
		}

		/// <summary>
		///   Loads the configuration properties from XML node.
		/// </summary>
		/// <param name="parent">Parent XML node containing the child settings elements.</param>
		public override void Load(XmlNode parent)
		{
			foreach (XmlNode node in parent.ChildNodes)
			{
				if (node.NodeType == XmlNodeType.Element && string.Compare(node.Name, "setting", true) == 0)
				{
					XmlAttribute nameAttrib = node.Attributes["name"];
					XmlAttribute valueAttrib = node.Attributes["value"];
					if (nameAttrib != null && valueAttrib != null)
					{
						// Set name is found.
						if (TypeLookup.ContainsKey(nameAttrib.Value))
							TypeLookup[nameAttrib.Value] = valueAttrib.Value;
						else
							TypeLookup.Add(nameAttrib.Value, valueAttrib.Value);
					}
				}
			}
		}

		/// <summary>
		///   Attempts to parse a value to the enumeration, using it's current settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		public override bool TryParse(string value)
		{
			return TryParse(value, TypeLookup);
		}

		/// <summary>
		///   Attempts to parse a value to the enumeration, using the specified settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <param name="typeLookup">Lookup table to determine if 'value' matches a enumerated item.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		private static bool TryParse(string value, Dictionary<string, string> typeLookup)
		{
			if (value == null)
				return false;
			if (value.Length == 0)
				return false;

			foreach(string key in typeLookup.Keys)
			{
				if (string.Compare(value, key, false) == 0)
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
