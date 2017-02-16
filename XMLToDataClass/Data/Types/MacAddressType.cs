//********************************************************************************************************************************
// Filename:    MacAddressType.cs
// Owner:       Richard Dunkley
// Description: Provides support for the C# PhysicalAddress type.
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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

namespace XMLToDataClass.Data.Types
{
	public class MacAddressType : BaseType
	{
		#region Properties

		public static bool DefaultAllowColonSeparator { get; set; }

		public static bool DefaultAllowDashSeparator { get; set; }

		public static bool DefaultAllowDotSeparator { get; set; }

		public bool AllowColonSeparator { get; set; }
		public bool AllowDashSeparator { get; set; }
		public bool AllowDotSeparator { get; set; }

		#endregion Properties

		#region Methods

		static MacAddressType()
		{
			DefaultAllowColonSeparator = true;
			DefaultAllowDashSeparator = true;
			DefaultAllowDotSeparator = true;
		}

		public MacAddressType(DataInfo info, string[] possibleValues) : base(info, possibleValues)
		{
			AllowColonSeparator = DefaultAllowColonSeparator;
			AllowDashSeparator = DefaultAllowDashSeparator;
			AllowDotSeparator = DefaultAllowDotSeparator;

			Type = DataType.MACAddress;
			IsNullable = false;
			DisplayName = "PhysicalAddress";

			Usings.Add("System.Net.NetworkInformation");
			Usings.Add("System.Text.RegularExpressions");
		}

		/// <summary>
		///   String of the C# representative data type.
		/// </summary>
		/// <returns>String containing the C# data type.</returns>
		public override string GetDataTypeString()
		{
			return "PhysicalAddress";
		}

		public override EnumInfo[] GenerateAdditionalEnums()
		{
			if (!AllowColonSeparator && !AllowDashSeparator && !AllowDotSeparator)
				return new EnumInfo[0];

			EnumInfo[] enums = new EnumInfo[1];
			enums[0] = new EnumInfo(
				"public",
				GetEnumTypeName(mInfo.PropertyName),
				"Enumerates the various MAC Address formats that can be parsed into a PhysicalAddress"
			);
			enums[0].Values.Add(new EnumValueInfo("NoSeparator", null, "Only the hexadecimal values are provided, no separators (Ex: MMMMMMSSSSSS)."));

			if (AllowColonSeparator)
				enums[0].Values.Add(new EnumValueInfo("Colon", null, "Each byte of the address is separated by a colon (':') (Ex: MM:MM:MM:SS:SS:SS)."));

			if (AllowDashSeparator)
				enums[0].Values.Add(new EnumValueInfo("Dash", null, "Each byte of the address is separated by a dash ('-') (Ex: MM-MM-MM-SS-SS-SS)."));

			if (AllowDotSeparator)
				enums[0].Values.Add(new EnumValueInfo("Period", null, "Triplets of the address are separated by periods ('.'). Address is composed of four triplets (Ex: MMM.MMM.SSS.SSS)."));

			return enums;
		}

		/// <summary>
		///   Gets the enumeration type's name from the property name.
		/// </summary>
		/// <param name="propertyName">Property name of the corresponding <see cref="DataInfo"/> object.</param>
		/// <returns>Enumeration type's name.</returns>
		private string GetEnumTypeName(string propertyName)
		{
			return string.Format("{0}MacFormatType", propertyName);
		}

		/// <summary>
		///   Gets the stored enumeration property's name.
		/// </summary>
		/// <param name="propertyName">Property name of the corresponding <see cref="DataInfo"/> object.</param>
		/// <returns>Stored enumeration property name.</returns>
		private string GetEnumPropertyName(string propertyName)
		{
			return string.Format("{0}ParsedFormat", propertyName);
		}

		/// <summary>
		///   Generates additional properties used by the import or export methods.
		/// </summary>
		/// <returns><see cref="PropertyInfo"/> array representing additional properties needed by the import/export methods. Can be empty.</returns>
		/// <remarks>These properties are typically used to persist state from import to export.</remarks>
		/// <exception cref="InvalidOperationException">An attempt was made to generate a boolean parsing method, but none of the valid boolean string pairs were allowed. At least one boolean string pair (Ex: 'true/false', 'yes/no', or '0/1') must be allowed.</exception>
		public override PropertyInfo[] GenerateAdditionalProperties()
		{
			List<PropertyInfo> propList = new List<PropertyInfo>();
			if (mInfo.IsOptional && mInfo.CanBeEmpty)
			{
				// Can't tell empty and null apart from null version so store the info.
				propList.Add(new PropertyInfo
				(
					"public",
					"bool",
					string.Format("{0}NullIsEmpty", mInfo.PropertyName),
					string.Format("True if the null {0} Version should be represented as an empty string in XML, false if it shouldn't be included.", mInfo.PropertyName)
				));
			}

			if (AllowColonSeparator || AllowDashSeparator || AllowDotSeparator)
			{
				propList.Add(new PropertyInfo
				(
					"public",
					GetEnumTypeName(mInfo.PropertyName),
					GetEnumPropertyName(mInfo.PropertyName),
					string.Format("Stores how the {0} MAC address is converted to an XML string", mInfo.PropertyName)
				));
			}
			return propList.ToArray();
		}

		/// <summary>
		///   Generates the export method code for the MAC address.
		/// </summary>
		/// <returns>String array representing the export method code.</returns>
		public override string[] GenerateExportMethodCode()
		{
			string enumPropertyName = GetEnumPropertyName(mInfo.PropertyName);
			string enumTypeName = GetEnumTypeName(mInfo.PropertyName);

			List<string> codeLines = new List<string>();

			if (mInfo.IsOptional)
			{
				codeLines.Add(string.Format("if({0} == null)", mInfo.PropertyName));
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
					codeLines.Add(string.Format("if({0} == null)", mInfo.PropertyName));
					codeLines.Add("	return string.Empty;");
				}
			}

			if (AllowColonSeparator || AllowDashSeparator || AllowDotSeparator)
			{
				codeLines.Add(string.Format("byte[] bytes = {0}.GetAddressBytes();", mInfo.PropertyName));
				codeLines.Add("StringBuilder sb = new StringBuilder();");
				codeLines.Add("for(int i = 0; i < bytes.Length; i++)");
				codeLines.Add("{");
				codeLines.Add("	string byteString = bytes[i].ToString(\"X2\");");
				codeLines.Add("	sb.Append(byteString[0]);");
				bool first = true;

				if (AllowColonSeparator)
				{
					codeLines.Add(string.Format("	if({0} == {1}.Colon)", enumPropertyName, enumTypeName));
					codeLines.Add("	{");
					codeLines.Add("		sb.Append(byteString[1]);");
					codeLines.Add("		if(i != bytes.Length - 1)");
					codeLines.Add("			sb.Append(\":\");");
					codeLines.Add("	}");
					first = false;
				}

				if (AllowDashSeparator)
				{
					if(first)
						codeLines.Add(string.Format("	if({0} == {1}.Dash)", enumPropertyName, enumTypeName));
					else
						codeLines.Add(string.Format("	else if({0} == {1}.Dash)", enumPropertyName, enumTypeName));
					codeLines.Add("	{");
					codeLines.Add("		sb.Append(byteString[1]);");
					codeLines.Add("		if(i != bytes.Length - 1)");
					codeLines.Add("			sb.Append(\"-\");");
					codeLines.Add("	}");
					first = false;
				}

				if (AllowDotSeparator)
				{
					if (first)
						codeLines.Add(string.Format("	if({0} == {1}.Period)", enumPropertyName, enumTypeName));
					else
						codeLines.Add(string.Format("	else if({0} == {1}.Period)", enumPropertyName, enumTypeName));
					codeLines.Add("	{");
					codeLines.Add("		if((i % 2) == 1)");
					codeLines.Add("			sb.Append(\".\");");
					codeLines.Add("		sb.Append(byteString[1]);");
					codeLines.Add("	}");
					first = false;
				}

				codeLines.Add("	else");
				codeLines.Add("	{");
				codeLines.Add("		sb.Append(byteString[1]);");
				codeLines.Add("	}");
				codeLines.Add("}");
				codeLines.Add("return sb.ToString();");
			}
			else
			{
				codeLines.Add(string.Format("return {0}.ToString();", mInfo.PropertyName));
			}
			return codeLines.ToArray();
		}

		/// <summary>
		///   Generates the import method code for the MAC address.
		/// </summary>
		/// <returns>String array representing the import method code.</returns>
		public override string[] GenerateImportMethodCode()
		{
			List<string> codeLines = new List<string>();
			codeLines.Add("if(value == null)");
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

			string enumPropertyName = GetEnumPropertyName(mInfo.PropertyName);
			string enumTypeName = GetEnumTypeName(mInfo.PropertyName);

			codeLines.Add("value = value.ToUpper();");
			if (AllowColonSeparator)
			{
				codeLines.Add("if(Regex.IsMatch(value, @\"^([0-9A-F]{2}[:]){5}([0-9A-F]{2})$\"))");
				codeLines.Add("{");
				codeLines.Add("	// IP address is formatted with colons (Ex: 00:00:00:00:00:00).");
				codeLines.Add(string.Format("	{0} = {1}.Colon;", enumPropertyName, enumTypeName));
				codeLines.Add("	value = value.Replace(':', '-');");
				codeLines.Add(string.Format("	{0} = PhysicalAddress.Parse(value);", mInfo.PropertyName));
				codeLines.Add("	return;");
				codeLines.Add("}");
			}

			if (AllowDashSeparator)
			{
				codeLines.Add("if(Regex.IsMatch(value, @\"^([0-9A-F]{2}[-]){5}([0-9A-F]{2})$\"))");
				codeLines.Add("{");
				codeLines.Add("	// IP address is formatted with dashes (Ex: 00-00-00-00-00-00).");
				codeLines.Add(string.Format("	{0} = {1}.Dash;", enumPropertyName, enumTypeName));
				codeLines.Add(string.Format("	{0} = PhysicalAddress.Parse(value);", mInfo.PropertyName));
				codeLines.Add("	return;");
				codeLines.Add("}");
			}

			if (AllowDotSeparator)
			{
				codeLines.Add("if(Regex.IsMatch(value, @\"^([0-9A-F]{3}[.]){3}([0-9A-F]{3})$\"))");
				codeLines.Add("{");
				codeLines.Add("	// IP address is formatted with dashes (Ex: 000.000.000.000).");
				codeLines.Add(string.Format("	{0} = {1}.Period;", enumPropertyName, enumTypeName));
				codeLines.Add("	value = value.Remove(11, 1);");
				codeLines.Add("	value = value.Remove(7, 1);");
				codeLines.Add("	value = value.Remove(3, 1);");
				codeLines.Add(string.Format("	{0} = PhysicalAddress.Parse(value);", mInfo.PropertyName));
				codeLines.Add("	return;");
				codeLines.Add("}");
			}

			codeLines.Add("// IP address is formatted with only hexadecimal bytes (Ex: 000000000000).");
			codeLines.Add("if(!Regex.IsMatch(value, @\"^([0-9A-F]{12})$\"))");
			codeLines.Add(string.Format("	throw new InvalidDataException(string.Format(\"The MAC Address value ({{0}}) for '{0}' does not contain a valid representation of a MAC address\", value));", mInfo.Name));
			if(AllowColonSeparator || AllowDashSeparator || AllowDotSeparator)
				codeLines.Add(string.Format("	{0} = {1}.NoSeparator;", enumPropertyName, enumTypeName));
			codeLines.Add(string.Format("{0} = PhysicalAddress.Parse(value);", mInfo.PropertyName));
			return codeLines.ToArray();
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
						// Set AllowColonSeparator if found.
						if (nameAttrib.Value == "AllowColonSeparator")
						{
							bool value;
							if (bool.TryParse(valueAttrib.Value, out value))
								AllowColonSeparator = value;
						}

						// Set AllowDashSeparator if found.
						if (nameAttrib.Value == "AllowDashSeparator")
						{
							bool value;
							if (bool.TryParse(valueAttrib.Value, out value))
								AllowDashSeparator = value;
						}

						// Set AllowDotSeparator if found.
						if (nameAttrib.Value == "AllowDotSeparator")
						{
							bool value;
							if (bool.TryParse(valueAttrib.Value, out value))
								AllowDotSeparator = value;
						}
					}
				}
			}
		}

		/// <summary>
		///   Saves the types configuration properties to XML child elements.
		/// </summary>
		/// <param name="doc"><see cref="XmlDocument"/> object representing the XML document to be written.</param>
		/// <param name="parent">Parent <see cref="XmlNode"/> to append the child settings to.</param>
		public override void Save(XmlDocument doc, XmlNode parent)
		{
			// Add AllowColonSeparator setting.
			XmlElement element = doc.CreateElement("setting");
			XmlAttribute attrib = element.Attributes.Append(doc.CreateAttribute("name"));
			attrib.Value = "AllowColonSeparator";
			attrib = element.Attributes.Append(doc.CreateAttribute("value"));
			attrib.Value = AllowColonSeparator.ToString();
			parent.AppendChild(element);

			// Add AllowDashSeparator setting.
			element = doc.CreateElement("setting");
			attrib = element.Attributes.Append(doc.CreateAttribute("name"));
			attrib.Value = "AllowDashSeparator";
			attrib = element.Attributes.Append(doc.CreateAttribute("value"));
			attrib.Value = AllowDashSeparator.ToString();
			parent.AppendChild(element);

			// Add AllowDotSeparator setting.
			element = doc.CreateElement("setting");
			attrib = element.Attributes.Append(doc.CreateAttribute("name"));
			attrib.Value = "AllowDotSeparator";
			attrib = element.Attributes.Append(doc.CreateAttribute("value"));
			attrib.Value = AllowDotSeparator.ToString();
			parent.AppendChild(element);
		}

		public override bool TryParse(string value)
		{
			return TryParse(value, AllowColonSeparator, AllowDashSeparator, AllowDotSeparator);
		}

		/// <summary>
		///   Attempts to parse a value to a PhysicalAddress, using the specified settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <param name="allowColonSeparator">True if the colon separator means of MAC formatting is allowed, false if not.</param>
		/// <param name="allowDashSeparator">True if the dash separator means of MAC formatting is allowed, false if not.</param>
		/// <param name="allowDotSeparator">True if the dot separator means of MAC formatting is allowed, false if not.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		private static bool TryParse(string value, bool allowColonSeparator, bool allowDashSeparator, bool allowDotSeparator)
		{
			if (value == null)
				return false;
			if (value.Length == 0)
				return false;

			value = value.ToUpper();
			if (allowColonSeparator)
			{
				if (Regex.IsMatch(value, @"^([0-9A-F]{2}[:]){5}([0-9A-F]{2})$"))
					return true;
			}

			if (allowDashSeparator)
			{
				if (Regex.IsMatch(value, @"^([0-9A-F]{2}[-]){5}([0-9A-F]{2})$"))
					return true;
			}

			if (allowDotSeparator)
			{
				if (Regex.IsMatch(value, @"^([0-9A-F]{3}[.]){3}([0-9A-F]{3})$"))
					return true;
			}

			// IP address is formatted with only hexadecimal bytes (Ex: 000000000000).
			if (Regex.IsMatch(value, @"^([0-9A-F]{12})$"))
				return true;
			return false;
		}

		/// <summary>
		///   Attempts to parse a value to a PhysicalAddress, using the default settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		public static bool TryParseWithDefaults(string value)
		{
			return TryParse(value, DefaultAllowColonSeparator, DefaultAllowDashSeparator, DefaultAllowDotSeparator);
		}

		#endregion Methods
	}
}
