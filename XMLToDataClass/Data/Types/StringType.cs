//********************************************************************************************************************************
// Filename:    StringType.cs
// Owner:       Richard Dunkley
// Description: Class which represents a string value in an XML file.
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
using System.Xml;

namespace XMLToDataClass.Data.Types
{
	/// <summary>
	///   Adds support for the string data type.
	/// </summary>
	public class StringType : BaseType
	{
		#region Properties

		/// <summary>
		///   Default maximum length of the string.
		/// </summary>
		public static int DefaultMaximumLength { get; set; }

		/// <summary>
		///   Default minimum length of the string.
		/// </summary>
		public static int DefaultMinimumLength { get; set; }

		/// <summary>
		///   Maximum length of the string.
		/// </summary>
		public int MaximumLength { get; set; }

		/// <summary>
		///   Minimum length of the string.
		/// </summary>
		public int MinimumLength { get; set; }

		#endregion Properties

		#region Methods

		/// <summary>
		///   Static constructor which assigns the default values.
		/// </summary>
		static StringType()
		{
			DefaultMinimumLength = 0;
			DefaultMaximumLength = 0;
		}

		/// <summary>
		///   Instantiates a new <see cref="StringType"/> object.
		/// </summary>
		/// <param name="info"><see cref="DataInfo"/> object associated with this type.</param>
		/// <param name="possibleValues">Possible values the data type will have to parse. Can be empty.</param>
		/// <param name="ignoreCase">True if the case of values should be ignored, false if they shouldn't.</param>
		/// <exception cref="ArgumentNullException"><i>possibleValues</i> or <i>info</i> is a null reference.</exception>
		public StringType(DataInfo info, string[] possibleValues, bool ignoreCase) : base(info, possibleValues, ignoreCase)
		{
			MinimumLength = DefaultMinimumLength;
			MaximumLength = DefaultMaximumLength;

			Type = DataType.String;
			IsNullable = false;
			IsArray = true;
			DataTypeString = "string";
			DisplayName = "String";
		}

		/// <summary>
		///   Generates additional enumerations used by the import or export methods.
		/// </summary>
		/// <returns><see cref="EnumInfo"/> array represnenting additional fields needed by the import/export methods. Can be empty.</returns>
		/// <exception cref="InvalidOperationException">The minimum number of characters is set larger than the maximum number of characters.</exception>
		public override EnumInfo[] GenerateAdditionalEnums()
		{
			if (MaximumLength > 0 && MinimumLength >= MaximumLength)
				throw new InvalidOperationException("An attempt was made to generate a string attribute parsing method, but the minimum number of characters in the string is set to more than or equal to the maximum number of characters.");
			return new EnumInfo[0];
		}

		/// <summary>
		///   Generates additional properties used by the import or export methods.
		/// </summary>
		/// <returns><see cref="PropertyInfo"/> array representing additional properties needed by the import/export methods. Can be empty.</returns>
		/// <remarks>These properties are typically used to persist state from import to export.</remarks>
		/// <exception cref="InvalidOperationException">The minimum number of characters is set larger than the maximum number of characters.</exception>
		public override PropertyInfo[] GenerateAdditionalProperties()
		{
			if (MaximumLength > 0 && MinimumLength >= MaximumLength)
				throw new InvalidOperationException("An attempt was made to generate a string attribute parsing method, but the minimum number of characters in the string is set to more than or equal to the maximum number of characters.");
			return new PropertyInfo[0];
		}

		/// <summary>
		///   Generates the export method code for the string.
		/// </summary>
		/// <returns>String array representing the export method code.</returns>
		/// <exception cref="InvalidOperationException">The minimum number of characters is set larger than the maximum number of characters.</exception>
		public override string[] GenerateExportMethodCode()
		{
			if (MaximumLength > 0 && MinimumLength >= MaximumLength)
				throw new InvalidOperationException("An attempt was made to generate a string attribute parsing method, but the minimum number of characters in the string is set to more than or equal to the maximum number of characters.");

			List<string> codeLines = new List<string>();
			codeLines.Add(string.Format("return {0};", mInfo.PropertyName));
			return codeLines.ToArray();
		}

		/// <summary>
		///   Generates the import method code for the string.
		/// </summary>
		/// <returns>String array representing the import method code.</returns>
		/// <exception cref="InvalidOperationException">The minimum number of characters is set larger than the maximum number of characters.</exception>
		public override string[] GenerateImportMethodCode()
		{
			if(MaximumLength > 0 && MinimumLength >= MaximumLength)
				throw new InvalidOperationException("An attempt was made to generate a string attribute parsing method, but the minimum number of characters in the string is set to more than or equal to the maximum number of characters.");

			List<string> codeLines = new List<string>();
			codeLines.Add("if (value == null)");
			if (mInfo.IsOptional)
			{
				codeLines.Add("{");
				codeLines.Add(string.Format("	{0} = null;", mInfo.PropertyName));
				codeLines.Add("	return;");
				codeLines.Add("}");
			}
			else
			{
				codeLines.Add(string.Format("	throw new InvalidDataException(\"The string value for '{0}' is a null reference.\");", mInfo.Name));
			}

			if(!mInfo.CanBeEmpty)
			{
				codeLines.Add("if (value.Length == 0)");
				codeLines.Add(string.Format("	throw new InvalidDataException(\"The string value for '{0}' is an empty string.\");", mInfo.Name));
			}

			if (MinimumLength > 0)
			{
				codeLines.Add(string.Format("if (value.Length < {0})", MinimumLength.ToString()));
				codeLines.Add(string.Format("	throw new InvalidDataException(string.Format(\"The '{0}' attribute provided ({{0}}) does not meet the minimum length requirement ({1}).\", value));", mInfo.Name, MinimumLength.ToString()));
			}
			if(MaximumLength > 0)
			{
				codeLines.Add(string.Format("if (value.Length > {0})", MaximumLength.ToString()));
				codeLines.Add(string.Format("	throw new InvalidDataException(string.Format(\"The '{0}' attribute provided ({{0}}) exceeds the maximum length requirement ({1}).\", value));", mInfo.Name, MaximumLength.ToString()));
			}

			codeLines.Add(string.Format("{0} = value;", mInfo.PropertyName));
			return codeLines.ToArray();
		}

		/// <summary>
		///   Attempts to parse a value to the string, using it's current settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		public override bool TryParse(string value)
		{
			return TryParse(value, MinimumLength, MaximumLength);
		}

		/// <summary>
		///   Attempts to parse a value to the string, using the specified settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <param name="minimumLength">Minimum size of the string.</param>
		/// <param name="maximumLength">Maximum size of the string.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		private static bool TryParse(string value, int minimumLength, int maximumLength)
		{
			if (value == null)
				return false;
			if (value.Length == 0)
				return false;

			if (minimumLength > 0)
			{
				if (value.Length < minimumLength)
					return false;
			}
			if (maximumLength > 0)
			{
				if (value.Length > maximumLength)
					return false;
			}
			return true;
		}

		/// <summary>
		///   Attempts to parse a value to the string, using the default settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		public static bool TryParseWithDefaults(string value)
		{
			return TryParse(value, DefaultMinimumLength, DefaultMaximumLength);
		}

		/// <summary>
		///   Saves the types configuration properties to XML child elements.
		/// </summary>
		/// <param name="doc"><see cref="XmlDocument"/> object representing the XML document to be written.</param>
		/// <param name="parent">Parent <see cref="XmlNode"/> to append the child settings to.</param>
		public override void Save(XmlDocument doc, XmlNode parent)
		{
			// Add MinimumLength setting.
			XmlElement element = doc.CreateElement("setting");
			XmlAttribute attrib = element.Attributes.Append(doc.CreateAttribute("name"));
			attrib.Value = "MinimumLength";
			attrib = element.Attributes.Append(doc.CreateAttribute("value"));
			attrib.Value = MinimumLength.ToString();
			parent.AppendChild(element);

			// Add MaximumLength setting.
			element = doc.CreateElement("setting");
			attrib = element.Attributes.Append(doc.CreateAttribute("name"));
			attrib.Value = "MaximumLength";
			attrib = element.Attributes.Append(doc.CreateAttribute("value"));
			attrib.Value = MaximumLength.ToString();
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
						// Set MinimumLength if found.
						if (nameAttrib.Value == "MinimumLength")
						{
							int value;
							if (int.TryParse(valueAttrib.Value, out value))
								MinimumLength = value;
						}

						// Set MaximumLength if found.
						if (nameAttrib.Value == "MaximumLength")
						{
							int value;
							if (int.TryParse(valueAttrib.Value, out value))
								MaximumLength = value;
						}
					}
				}
			}
		}

		#endregion Methods
	}
}
