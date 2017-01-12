//********************************************************************************************************************************
// Filename:    BooleanType.cs
// Owner:       Richard Dunkley
// Description: Class which represents a boolean value in an XML file.
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
	/// <summary>
	///   Adds support for the boolean data type.
	/// </summary>
	public class BooleanType : BaseType
	{
		#region Properties

		/// <summary>
		///   Default setting to allow 'True' and 'False' strings.
		/// </summary>
		public static bool DefaultAllowTrueFalseStrings { get; set; }

		/// <summary>
		///   Default setting to allow 'Yes' and 'No' strings.
		/// </summary>
		public static bool DefaultAllowYesNoStrings { get; set; }

		/// <summary>
		///   Default setting to allow '0' and '1' strings.
		/// </summary>
		public static bool DefaultAllowZeroOneStrings { get; set; }

		/// <summary>
		///   True to allow 'True' and 'False' strings, false to disallow.
		/// </summary>
		public bool AllowTrueFalseStrings { get; set; }

		/// <summary>
		///   True to allow 'Yes' and 'No' strings, false to disallow.
		/// </summary>
		public bool AllowYesNoStrings { get; set; }

		/// <summary>
		///   True to allow '0' and '1' strings, false to disallow.
		/// </summary>
		public bool AllowZeroOneStrings { get; set; }

		#endregion Properties

		#region Methods

		/// <summary>
		///   Static constructor which assigns the default values.
		/// </summary>
		static BooleanType()
		{
			DefaultAllowTrueFalseStrings = true;
			DefaultAllowYesNoStrings = true;
			DefaultAllowZeroOneStrings = true;
		}

		/// <summary>
		///   Instantiates a new <see cref="BooleanType"/> object.
		/// </summary>
		/// <param name="info"><see cref="DataInfo"/> object associated with this type.</param>
		/// <param name="possibleValues">Possible values the data type will have to parse. Can be empty.</param>
		/// <exception cref="ArgumentNullException"><paramref name="possibleValues"/> or <paramref name="info"/> is a null reference.</exception>
		public BooleanType(DataInfo info, string[] possibleValues) : base(info, possibleValues)
		{
			AllowTrueFalseStrings = DefaultAllowTrueFalseStrings;
			AllowYesNoStrings = DefaultAllowYesNoStrings;
			AllowZeroOneStrings = DefaultAllowZeroOneStrings;

			Type = DataType.Boolean;
			DisplayName = "Boolean";
		}

		/// <summary>
		///   String of the C# representive data type.
		/// </summary>
		/// <returns>String containing the C# data type.</returns>
		public override string GetDataTypeString()
		{
			return "bool";
		}

		/// <summary>
		///   Generates additional enumerations used by the import or export methods.
		/// </summary>
		/// <returns><see cref="EnumInfo"/> array represnenting additional fields needed by the import/export methods. Can be empty.</returns>
		/// <exception cref="InvalidOperationException">An attempt was made to generate a boolean parsing method, but none of the valid boolean string pairs were allowed. At least one boolean string pair (Ex: 'true/false', 'yes/no', or '0/1') must be allowed.</exception>
		public override EnumInfo[] GenerateAdditionalEnums()
		{
			if (!AllowTrueFalseStrings && !AllowZeroOneStrings && !AllowYesNoStrings)
				throw new InvalidOperationException("An attempt was made to generate a boolean parsing method, but none of the valid boolean string pairs were allowed. At least one boolean string pair (Ex: 'true/false', 'yes/no', or '0/1') must be allowed.");

			int count = 0;
			if (AllowTrueFalseStrings)
				count++;
			if (AllowYesNoStrings)
				count++;
			if (AllowZeroOneStrings)
				count++;

			if (count < 2)
				return new EnumInfo[0];

			EnumInfo enumInfo = new EnumInfo
			(
				"public",
				string.Format("{0}", GetEnumTypeName(mInfo.PropertyName)),
				string.Format("Represents the types the {0} can be parsed with", mInfo.PropertyName)
			);

			if (AllowTrueFalseStrings)
				enumInfo.Values.Add(new EnumValueInfo("TrueFalse", null, "Boolean represented as 'True' or 'False'."));
			if(AllowYesNoStrings)
				enumInfo.Values.Add(new EnumValueInfo("YesNo", null, "Boolean represented as 'Yes' or 'No'."));
			if(AllowZeroOneStrings)
				enumInfo.Values.Add(new EnumValueInfo("ZeroOne", null, "Boolean represented as '0' or '1'."));
			return new EnumInfo[] { enumInfo };
		}

		/// <summary>
		///   Generates additional properties used by the import or export methods.
		/// </summary>
		/// <returns><see cref="PropertyInfo"/> array representing additional properties needed by the import/export methods. Can be empty.</returns>
		/// <remarks>These properties are typically used to persist state from import to export.</remarks>
		/// <exception cref="InvalidOperationException">An attempt was made to generate a boolean parsing method, but none of the valid boolean string pairs were allowed. At least one boolean string pair (Ex: 'true/false', 'yes/no', or '0/1') must be allowed.</exception>
		public override PropertyInfo[] GenerateAdditionalProperties()
		{
			if (!AllowTrueFalseStrings && !AllowZeroOneStrings && !AllowYesNoStrings)
				throw new InvalidOperationException("An attempt was made to generate a boolean parsing method, but none of the valid boolean string pairs were allowed. At least one boolean string pair (Ex: 'true/false', 'yes/no', or '0/1') must be allowed.");

			List<PropertyInfo> propList = new List<PropertyInfo>();
			if(mInfo.IsOptional && mInfo.CanBeEmpty)
			{
				// Can't tell empty and null apart from null boolean so store the mInfo.
				propList.Add(new PropertyInfo
				(
					"public",
					"bool",
					string.Format("{0}NullIsEmpty", mInfo.PropertyName),
					string.Format("True if the null {0} boolean should be represented as an empty string in XML, false if it shouldn't be included.", mInfo.PropertyName)
				));
			}

			int count = 0;
			if (AllowTrueFalseStrings)
				count++;
			if (AllowYesNoStrings)
				count++;
			if (AllowZeroOneStrings)
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
					string.Format("Stores how the {0} boolean is converted to XML string", mInfo.PropertyName)
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
			if (!AllowTrueFalseStrings && !AllowZeroOneStrings && !AllowYesNoStrings)
				throw new InvalidOperationException("An attempt was made to generate a boolean parsing method, but none of the valid boolean string pairs were allowed. At least one boolean string pair (Ex: 'true/false', 'yes/no', or '0/1') must be allowed.");

			string enumPropertyName = GetEnumPropertyName(mInfo.PropertyName);
			string enumTypeName = GetEnumTypeName(mInfo.PropertyName);

			List<string> codeLines = new List<string>();

			string name;
			if(mInfo.IsOptional)
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
				if(mInfo.CanBeEmpty)
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
			if (AllowTrueFalseStrings)
				count++;
			if (AllowYesNoStrings)
				count++;
			if (AllowZeroOneStrings)
				count++;

			codeLines.Add(string.Empty);
			codeLines.Add(string.Format("if({0})", name));
			if (count == 1)
			{
				// Only one type is allowed so output in that type.
				if (AllowTrueFalseStrings)
					codeLines.Add("	return \"true\";");
				else if(AllowYesNoStrings)
					codeLines.Add("	return \"yes\";");
				else if(AllowZeroOneStrings)
					codeLines.Add("	return \"1\";");
			}
			else
			{
				codeLines.Add("{");
				int index = 0;
				StringBuilder temp = new StringBuilder();
				temp.Append(string.Format("	if({0} == {1}.", enumPropertyName, enumTypeName));
				if(AllowTrueFalseStrings)
				{
					temp.Append("TrueFalse)");
					codeLines.Add(temp.ToString());
					temp.Clear();
					codeLines.Add("		return \"true\";");
					index++;
					if (index + 1 == count)
						codeLines.Add("	else");
					else
						temp.Append(string.Format("	else if({0} == {1}.", enumPropertyName, enumTypeName));
				}
				if(AllowYesNoStrings)
				{
					if(index + 1 != count)
					{
						temp.Append("YesNo)");
						codeLines.Add(temp.ToString());
						temp.Clear();
					}
					codeLines.Add("		return \"yes\";");
					index++;
					if (index != count)
						codeLines.Add("	else");
				}
				if(AllowZeroOneStrings)
				{
					codeLines.Add("		return \"1\";");
				}
				codeLines.Add("}");
			}
			codeLines.Add("else");
			if (count == 1)
			{
				// Only one type is allowed so output in that type.
				if (AllowTrueFalseStrings)
					codeLines.Add("	return \"false\";");
				else if (AllowYesNoStrings)
					codeLines.Add("	return \"no\";");
				else if (AllowZeroOneStrings)
					codeLines.Add("	return \"0\";");
			}
			else
			{
				codeLines.Add("{");
				int index = 0;
				StringBuilder temp = new StringBuilder();
				temp.Append(string.Format("	if({0} == {1}.", enumPropertyName, enumTypeName));
				if (AllowTrueFalseStrings)
				{
					temp.Append("TrueFalse)");
					codeLines.Add(temp.ToString());
					temp.Clear();
					codeLines.Add("		return \"false\";");
					index++;
					if (index + 1 == count)
						codeLines.Add("	else");
					else
						temp.Append(string.Format("	else if({0} == {1}.", enumPropertyName, enumTypeName));
				}
				if (AllowYesNoStrings)
				{
					if (index + 1 != count)
					{
						temp.Append("YesNo)");
						codeLines.Add(temp.ToString());
						temp.Clear();
					}
					codeLines.Add("		return \"no\";");
					index++;
					if (index != count)
						codeLines.Add("	else");
				}
				if (AllowZeroOneStrings)
				{
					codeLines.Add("		return \"0\";");
				}
				codeLines.Add("}");
			}
			return codeLines.ToArray();
		}

		/// <summary>
		///   Generates the import method code for the boolean.
		/// </summary>
		/// <returns>String array representing the import method code.</returns>
		/// <exception cref="InvalidOperationException">An attempt was made to generate a boolean parsing method, but none of the valid boolean string pairs were allowed. At least one boolean string pair (Ex: 'true/false', 'yes/no', or '0/1') must be allowed.</exception>
		public override string[] GenerateImportMethodCode()
		{
			if (!AllowTrueFalseStrings && !AllowZeroOneStrings && !AllowYesNoStrings)
				throw new InvalidOperationException("An attempt was made to generate a boolean parsing method, but none of the valid boolean string pairs were allowed. At least one boolean string pair (Ex: 'true/false', 'yes/no', or '0/1') must be allowed.");

			List<string> codeLines = new List<string>();
			codeLines.Add("if (value == null)");
			if (mInfo.IsOptional)
			{
				codeLines.Add("{");
				codeLines.Add(string.Format("	{0} = null;", mInfo.PropertyName));
				if(mInfo.CanBeEmpty)
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
				if(mInfo.IsOptional)
					codeLines.Add(string.Format("	{0}NullIsEmpty = true;", mInfo.PropertyName));
				codeLines.Add("	return;");
				codeLines.Add("}");
			}
			else
			{
				codeLines.Add(string.Format("	throw new InvalidDataException(\"The string value for '{0}' is an empty string.\");", mInfo.Name));
			}

			int count = 0;
			if (AllowTrueFalseStrings)
				count++;
			if (AllowYesNoStrings)
				count++;
			if (AllowZeroOneStrings)
				count++;

			string enumPropertyName = GetEnumPropertyName(mInfo.PropertyName);
			string enumTypeName = GetEnumTypeName(mInfo.PropertyName);

			if (AllowTrueFalseStrings)
			{
				codeLines.Add("if (string.Compare(value, \"true\", true) == 0)");
				codeLines.Add("{");
				codeLines.Add(string.Format("	{0} = true;", mInfo.PropertyName));
				if(count > 1)
					codeLines.Add(string.Format("	{0} = {1}.TrueFalse;", enumPropertyName, enumTypeName));
				codeLines.Add("	return;");
				codeLines.Add("}");

				codeLines.Add("if (string.Compare(value, \"false\", true) == 0)");
				codeLines.Add("{");
				codeLines.Add(string.Format("	{0} = false;", mInfo.PropertyName));
				if (count > 1)
					codeLines.Add(string.Format("	{0} = {1}.TrueFalse;", enumPropertyName, enumTypeName));
				codeLines.Add("	return;");
				codeLines.Add("}");
			}

			if (AllowZeroOneStrings)
			{
				codeLines.Add("if (string.Compare(value, \"1\") == 0)");
				codeLines.Add("{");
				codeLines.Add(string.Format("	{0} = true;", mInfo.PropertyName));
				if (count > 1)
					codeLines.Add(string.Format("	{0} = {1}.ZeroOne;", enumPropertyName, enumTypeName));
				codeLines.Add("	return;");
				codeLines.Add("}");

				codeLines.Add("if (string.Compare(value, \"0\") == 0)");
				codeLines.Add("{");
				codeLines.Add(string.Format("	{0} = false;", mInfo.PropertyName));
				if (count > 1)
					codeLines.Add(string.Format("	{0} = {1}.ZeroOne;", enumPropertyName, enumTypeName));
				codeLines.Add("	return;");
				codeLines.Add("}");
			}

			if(AllowYesNoStrings)
			{
				codeLines.Add("if (string.Compare(value, \"yes\", true) == 0)");
				codeLines.Add("{");
				codeLines.Add(string.Format("	{0} = true;", mInfo.PropertyName));
				if (count > 1)
					codeLines.Add(string.Format("	{0} = {1}.YesNo;", enumPropertyName, enumTypeName));
				codeLines.Add("	return;");
				codeLines.Add("}");

				codeLines.Add("if (string.Compare(value, \"no\", true) == 0)");
				codeLines.Add("{");
				codeLines.Add(string.Format("	{0} = false;", mInfo.PropertyName));
				if (count > 1)
					codeLines.Add(string.Format("	{0} = {1}.YesNo;", enumPropertyName, enumTypeName));
				codeLines.Add("	return;");
				codeLines.Add("}");
			}
			codeLines.Add(string.Empty);
			codeLines.Add("throw new InvalidDataException(string.Format(\"The Boolean value specified ({0}) is not a valid boolean string representation (Ex: \\\"true\\\" or \\\"false\\\").\", value));");

			return codeLines.ToArray();
		}

		/// <summary>
		///   Gets the enumeration type's name from the property name.
		/// </summary>
		/// <param name="propertyName">Property name of the corresponding <see cref="DataInfo"/> object.</param>
		/// <returns>Enumeration type's name.</returns>
		private string GetEnumTypeName(string propertyName)
		{
			return string.Format("{0}BooleanType", propertyName);
		}

		/// <summary>
		///   Gets the stored enumeration property's name.
		/// </summary>
		/// <param name="propertyName">Property name of the corresponding <see cref="DataInfo"/> object.</param>
		/// <returns>Stored enumeration property name.</returns>
		private string GetEnumPropertyName(string propertyName)
		{
			return string.Format("{0}ParsedType", propertyName);
		}

		/// <summary>
		///   Saves the types configuration properties to XML child elements.
		/// </summary>
		/// <param name="doc"><see cref="XmlDocument"/> object representing the XML document to be written.</param>
		/// <param name="parent">Parent <see cref="XmlNode"/> to append the child settings to.</param>
		public override void Save(XmlDocument doc, XmlNode parent)
		{
			// Add AllowTrueFalseStrings setting.
			XmlElement element = doc.CreateElement("setting");
			XmlAttribute attrib = element.Attributes.Append(doc.CreateAttribute("name"));
			attrib.Value = "AllowTrueFalseStrings";
			attrib = element.Attributes.Append(doc.CreateAttribute("value"));
			attrib.Value = AllowTrueFalseStrings.ToString();
			parent.AppendChild(element);

			// Add AllowYesNoStrings setting.
			element = doc.CreateElement("setting");
			attrib = element.Attributes.Append(doc.CreateAttribute("name"));
			attrib.Value = "AllowYesNoStrings";
			attrib = element.Attributes.Append(doc.CreateAttribute("value"));
			attrib.Value = AllowYesNoStrings.ToString();
			parent.AppendChild(element);

			// Add AllowZeroOneStrings setting.
			element = doc.CreateElement("setting");
			attrib = element.Attributes.Append(doc.CreateAttribute("name"));
			attrib.Value = "AllowZeroOneStrings";
			attrib = element.Attributes.Append(doc.CreateAttribute("value"));
			attrib.Value = AllowZeroOneStrings.ToString();
			parent.AppendChild(element);
		}

		/// <summary>
		///   Loads the configuration properties from XML node.
		/// </summary>
		/// <param name="parent">Parent XML node containing the child settings elements.</param>
		public override void Load(XmlNode parent)
		{
			foreach(XmlNode node in parent.ChildNodes)
			{
				if(node.NodeType == XmlNodeType.Element && string.Compare(node.Name, "setting", true) == 0)
				{
					XmlAttribute nameAttrib = node.Attributes["name"];
					XmlAttribute valueAttrib = node.Attributes["value"];
					if(nameAttrib != null && valueAttrib != null)
					{
						// Set AllowTrueFalseStrings if found.
						if (nameAttrib.Value == "AllowTrueFalseStrings")
						{
							bool value;
							if (bool.TryParse(valueAttrib.Value, out value))
								AllowTrueFalseStrings = value;
						}

						// Set AllowYesNoStrings if found.
						if (nameAttrib.Value == "AllowYesNoStrings")
						{
							bool value;
							if (bool.TryParse(valueAttrib.Value, out value))
								AllowYesNoStrings = value;
						}

						// Set AllowZeroOneStrings if found.
						if (nameAttrib.Value == "AllowZeroOneStrings")
						{
							bool value;
							if (bool.TryParse(valueAttrib.Value, out value))
								AllowZeroOneStrings = value;
						}
					}
				}
			}
		}

		/// <summary>
		///   Attempts to parse a value to the boolean, using it's current settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		public override bool TryParse(string value)
		{
			return TryParse(value, AllowTrueFalseStrings, AllowZeroOneStrings, AllowYesNoStrings);
		}

		/// <summary>
		///   Attempts to parse a value to the boolean, using the specified settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <param name="allowTrueFalseStrings">True if 'true' or 'false' can be used as the strings, false if not.</param>
		/// <param name="allowZeroOneStrings">True if '1' or '0' can be used as the strings, false if not.</param>
		/// <param name="allowYesNoStrings">True if 'yes' or 'no' can be used as the strings, false if not.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		private static bool TryParse(string value, bool allowTrueFalseStrings, bool allowZeroOneStrings, bool allowYesNoStrings)
		{
			if (value == null)
				return false;
			if (value.Length == 0)
				return false;

			if (allowTrueFalseStrings)
			{
				if (string.Compare(value, "true", true) == 0)
					return true;
				if (string.Compare(value, "false", true) == 0)
					return true;
			}

			if (allowZeroOneStrings)
			{
				if (string.Compare(value, "0", true) == 0)
					return true;
				if (string.Compare(value, "1", true) == 0)
					return true;
			}

			if (allowYesNoStrings)
			{
				if (string.Compare(value, "yes", true) == 0)
					return true;
				if (string.Compare(value, "no", true) == 0)
					return true;
			}
			return false;
		}

		/// <summary>
		///   Attempts to parse a value to a boolean, using the default settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		public static bool TryParseWithDefaults(string value)
		{
			return TryParse(value, DefaultAllowTrueFalseStrings, DefaultAllowZeroOneStrings, DefaultAllowYesNoStrings);
		}

		#endregion Methods
	}
}
