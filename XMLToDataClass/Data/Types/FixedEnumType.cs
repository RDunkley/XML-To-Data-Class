//********************************************************************************************************************************
// Filename:    FixedEnumType.cs
// Owner:       Richard Dunkley
// Description: Abstract class which represents a C# built-in enumeration value in an XML file. Built-in is one that is a C#
//              built-in type or one that is referenced in an included assembly.
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
using CSCodeGen;
using System.Xml;

namespace XMLToDataClass.Data.Types
{
	/// <summary>
	///   Provides a generic class for using C# built-in enumerated types.
	/// </summary>
	/// <typeparam name="T">Fixed Enum type. Fixed meaning it has already been defined (C# built-in type or referenced library type).</typeparam>
	public abstract class FixedEnumType<T> : BaseType where T : struct, IConvertible
	{
		#region Properties

		/// <summary>
		///   Allows the values to represent the enumerated items instead of text.
		/// </summary>
		public bool AllowValues { get; set; }

		/// <summary>
		///   Specifies whether to ignore the case for the value names or not.
		/// </summary>
		public bool IgnoreCase { get; set; }

		/// <summary>
		///   Default value. Allows the values to represent the enumerated items instead of text.
		/// </summary>
		public static bool DefaultAllowValues { get; set; }

		/// <summary>
		///   Default value. Specifies whether to ignore the case for the value names or not.
		/// </summary>
		public static bool DefaultIgnoreCase { get; set; }

		#endregion Properties

		#region Methods

		/// <summary>
		///   Static constructor which assigns the default values.
		/// </summary>
		static FixedEnumType()
		{
			DefaultAllowValues = false;
			DefaultIgnoreCase = true;
		}

		/// <summary>
		///   Instantiates a new <see cref="FixedEnumType{T}"/> object.
		/// </summary>
		/// <param name="info"><see cref="DataInfo"/> object associated with this type.</param>
		/// <param name="possibleValues">Possible values the data type will have to parse. Can be empty.</param>
		/// <remarks>This is an abstract class, the inheritor must set the Type property of the base class.</remarks>
		/// <exception cref="ArgumentNullException"><paramref name="possibleValues"/> or <paramref name="info"/> is a null reference.</exception>
		/// <exception cref="ArgumentException">The type <i>T</i> specified is not a valid enumerated type.</exception>
		public FixedEnumType(DataInfo info, string[] possibleValues) : base(info, possibleValues)
		{
			if (!typeof(T).IsEnum)
				throw new ArgumentException("T must be an enumerated type");

			AllowValues = DefaultAllowValues;
			IgnoreCase = DefaultIgnoreCase;

			DisplayName = string.Format("{0} enumerated type", GetDataTypeString());
		}

		/// <summary>
		///   String of the C# representative data type.
		/// </summary>
		/// <returns>String containing the C# data type.</returns>
		public override string GetDataTypeString()
		{
			return typeof(T).Name;
		}

		/// <summary>
		///   Generates additional enumerations used by the import or export methods.
		/// </summary>
		/// <returns><see cref="EnumInfo"/> array representing additional fields needed by the import/export methods. Can be empty.</returns>
		public override EnumInfo[] GenerateAdditionalEnums()
		{
			return new EnumInfo[0];
		}

		/// <summary>
		///   Generates additional properties used by the import or export methods.
		/// </summary>
		/// <returns><see cref="PropertyInfo"/> array representing additional properties needed by the import/export methods. Can be empty.</returns>
		/// <remarks>These properties are typically used to persist state from import to export.</remarks>
		public override PropertyInfo[] GenerateAdditionalProperties()
		{
			List<PropertyInfo> propList = new List<PropertyInfo>();
			if (mInfo.IsOptional && mInfo.CanBeEmpty)
			{
				// Can't tell empty and null apart from null enum so store the info.
				propList.Add(new PropertyInfo
				(
					"public",
					"bool",
					string.Format("{0}NullIsEmpty", mInfo.PropertyName),
					string.Format("True if the null {0} value should be represented as an empty string in XML, false if it shouldn't be included.", mInfo.PropertyName)
				));
			}

			if(AllowValues)
			{
				// if values are allowed then store which should be used.
				propList.Add(new PropertyInfo
				(
					"public",
					"bool",
					string.Format("{0}StoreAsName", mInfo.PropertyName),
					string.Format("True if the enumerated value should be stored as a name, false if it should be stored as a value.")
				));
			}

			return propList.ToArray();
		}

		/// <summary>
		///   Generates the export method code for the enumerated type.
		/// </summary>
		/// <returns>String array representing the export method code.</returns>
		public override string[] GenerateExportMethodCode()
		{
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

			codeLines.Add(string.Empty);
			if (AllowValues)
			{
				codeLines.Add(string.Format("if({0}StoreAsName)", mInfo.PropertyName));
				codeLines.Add(string.Format("	return {0}.ToString();", name));
				codeLines.Add("else");
				codeLines.Add(string.Format("	return {0}.ToString(\"d\");", name));
			}
			else
			{
				codeLines.Add(string.Format("return {0}.ToString();", name));
			}
			return codeLines.ToArray();
		}

		/// <summary>
		///   Generates the import method code for the enumerated type.
		/// </summary>
		/// <returns>String array representing the import method code.</returns>
		public override string[] GenerateImportMethodCode()
		{
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

			string dataTypeString = GetDataTypeString();
			codeLines.Add(string.Empty);
			codeLines.Add(string.Format("{0} temp;", dataTypeString));
			codeLines.Add(string.Format("if(Enum.TryParse<{0}>(value, {1}, out temp))", dataTypeString, IgnoreCase.ToString().ToLower()));
			codeLines.Add("{");
			codeLines.Add(string.Format("	{0} = temp;", mInfo.PropertyName));
			codeLines.Add("	return;");
			codeLines.Add("}");

			if (AllowValues)
			{
				codeLines.Add(string.Empty);
				codeLines.Add(string.Format("if(Enum.IsDefined(typeof({0}), value))", dataTypeString));
				codeLines.Add("{");
				codeLines.Add(string.Format("	{0} = ({1})value;", mInfo.PropertyName, dataTypeString));
				codeLines.Add("	return;");
				codeLines.Add("}");
			}

			codeLines.Add(string.Empty);
			codeLines.Add(string.Format("throw new InvalidDataException(string.Format(\"The enumerated type value specified ({{0}}) is not a valid {0} string or value representation\", value));", dataTypeString));

			return codeLines.ToArray();
		}

		/// <summary>
		///   Saves the types configuration properties to XML child elements.
		/// </summary>
		/// <param name="doc"><see cref="XmlDocument"/> object representing the XML document to be written.</param>
		/// <param name="parent">Parent <see cref="XmlNode"/> to append the child settings to.</param>
		public override void Save(XmlDocument doc, XmlNode parent)
		{
			// Add AllowValues setting.
			XmlElement element = doc.CreateElement("setting");
			XmlAttribute attrib = element.Attributes.Append(doc.CreateAttribute("name"));
			attrib.Value = "AllowValues";
			attrib = element.Attributes.Append(doc.CreateAttribute("value"));
			attrib.Value = AllowValues.ToString();
			parent.AppendChild(element);

			// Add IgnoreCase setting.
			element = doc.CreateElement("setting");
			attrib = element.Attributes.Append(doc.CreateAttribute("name"));
			attrib.Value = "IgnoreCase";
			attrib = element.Attributes.Append(doc.CreateAttribute("value"));
			attrib.Value = IgnoreCase.ToString();
			parent.AppendChild(element);
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
						// Set AllowValues if found.
						if (nameAttrib.Value == "AllowValues")
						{
							bool value;
							if (bool.TryParse(valueAttrib.Value, out value))
								AllowValues = value;
						}

						// Set IgnoreCase if found.
						if (nameAttrib.Value == "IgnoreCase")
						{
							bool value;
							if (bool.TryParse(valueAttrib.Value, out value))
								IgnoreCase = value;
						}
					}
				}
			}
		}

		/// <summary>
		///   Attempts to parse a value to an enumerated type, using it's current settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		public override bool TryParse(string value)
		{
			return TryParse(value, AllowValues, IgnoreCase);
		}

		/// <summary>
		///   Attempts to parse a value to an enumerated type, using the specified settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <param name="allowValues">True if the values can be used as well as the strings.</param>
		/// <param name="ignoreCase">True if the case should be ignored for strings, false otherwise.</param>
		/// <returns>True if the value can be parsed, false otherwise.</returns>
		private static bool TryParse(string value, bool allowValues, bool ignoreCase)
		{
			if (value == null)
				return false;
			if (value.Length == 0)
				return false;

			T temp;
			if (Enum.TryParse<T>(value, ignoreCase, out temp))
				return true;

			if(allowValues)
			{
				if (Enum.IsDefined(typeof(T), value))
					return true;
			}
			return false;
		}

		/// <summary>
		///   Attempts to parse a value to an enumerated type, using the default settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		public static bool TryParseWithDefaults(string value)
		{
			return TryParse(value, DefaultAllowValues, DefaultIgnoreCase);
		}

		#endregion Methods
	}
}
