//********************************************************************************************************************************
// Filename:    TimeSpanType.cs
// Owner:       Richard Dunkley
// Description: Class which represents a time span value in an XML file.
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
using System.Xml;
using CSCodeGen;
using System.Globalization;

namespace XMLToDataClass.Data.Types
{
	public class TimeSpanType : BaseType
	{
		#region Properties

		public static CultureInfo DefaultCulture { get; set; }

		public CultureInfo Culture { get; set; }

		#endregion Properties

		#region Methods

		static TimeSpanType()
		{
			DefaultCulture = CultureInfo.CurrentCulture;
		}

		/// <summary>
		///   Instantiates a new <see cref="TimeSpanType"/> object.
		/// </summary>
		/// <param name="info"><see cref="DataInfo"/> object associated with this type.</param>
		/// <param name="possibleValues">Possible values the data type will have to parse. Can be empty.</param>
		/// <exception cref="ArgumentNullException"><paramref name="possibleValues"/> or <paramref name="info"/> is a null reference.</exception>
		public TimeSpanType(DataInfo info, string[] possibleValues) : base(info, possibleValues)
		{
			Culture = DefaultCulture;

			Type = DataType.TimeSpan;
			DisplayName = "Duration of Time";
			Usings.Add("System.Globalization");
		}

		/// <summary>
		///   String of the C# representive data type.
		/// </summary>
		/// <returns>String containing the C# data type.</returns>
		public override string GetDataTypeString()
		{
			return "TimeSpan";
		}

		/// <summary>
		///   Generates additional enumerations used by the import or export methods.
		/// </summary>
		/// <returns><see cref="EnumInfo"/> array represnenting additional fields needed by the import/export methods. Can be empty.</returns>
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
				// Can't tell empty and null apart from null date-time so store the info.
				propList.Add(new PropertyInfo
				(
					"public",
					"bool",
					string.Format("{0}NullIsEmpty", mInfo.PropertyName),
					string.Format("True if the null {0} TimeSpan should be represented as an empty string in XML, false if it shouldn't be included.", mInfo.PropertyName)
				));
			}

			return propList.ToArray();
		}

		/// <summary>
		///   Generates the export method code for the boolean.
		/// </summary>
		/// <returns>String array representing the export method code.</returns>
		/// <exception cref="InvalidOperationException">An attempt was made to generate a parsing method, but the <see cref="Culture"/> specified is null.</exception>
		public override string[] GenerateExportMethodCode()
		{
			if (Culture == null)
				throw new InvalidOperationException("Culture is a null reference");

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
			codeLines.Add(string.Format("CultureInfo culture = new CultureInfo(\"{0}\");", Culture.Name));
			codeLines.Add(string.Format("return {0}.ToString(\"c\", culture);", name));
			return codeLines.ToArray();
		}

		/// <summary>
		///   Generates the import method code for the type.
		/// </summary>
		/// <returns>String array representing the import method code.</returns>
		/// <exception cref="InvalidOperationException">An attempt was made to generate a parsing method, but the <see cref="Culture"/> specified is null.</exception>
		public override string[] GenerateImportMethodCode()
		{
			if (Culture == null)
				throw new InvalidOperationException("Culture is a null reference");

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

			codeLines.Add(string.Format("CultureInfo info = CultureInfo.GetCultureInfo(\"{0}\");", Culture.Name));
			codeLines.Add("TimeSpan returnValue;");
			codeLines.Add("if(TimeSpan.TryParse(value, info, out returnValue))");
			codeLines.Add("{");
			codeLines.Add(string.Format("	{0} = returnValue;", mInfo.PropertyName));
			codeLines.Add("	return;");
			codeLines.Add("}");
			codeLines.Add(string.Empty);
			codeLines.Add("throw new InvalidDataException(string.Format(\"The time duration value specified ({0}) is not a valid TimeSpan standard string representation\", value));");
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
						// Set Culture if found.
						if (nameAttrib.Value == "Culture")
						{
							try
							{
								Culture = CultureInfo.GetCultureInfo(valueAttrib.Value);
							}
							catch (Exception)
							{ }
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
			// Add the Culture option.
			XmlElement element = doc.CreateElement("setting");
			XmlAttribute attrib = element.Attributes.Append(doc.CreateAttribute("name"));
			attrib.Value = "Culture";
			attrib = element.Attributes.Append(doc.CreateAttribute("value"));
			attrib.Value = Culture.ToString();
			parent.AppendChild(element);
		}

		/// <summary>
		///   Attempts to parse a value to the time span, using it's current settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		public override bool TryParse(string value)
		{
			return TryParse(value, Culture);
		}

		/// <summary>
		///   Attempts to parse a value to the time span, using the specified settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <param name="culture"><see cref="CultureInfo"/> object to determine the culture to use.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		private static bool TryParse(string value, CultureInfo culture)
		{
			if (value == null)
				return false;
			if (value.Length == 0)
				return false;

			TimeSpan parsedValue;
			return TimeSpan.TryParse(value, culture, out parsedValue);
		}

		/// <summary>
		///   Attempts to parse a value to a time span, using the default settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		public static bool TryParseWithDefaults(string value)
		{
			return TryParse(value, DefaultCulture);
		}

		#endregion Methods
	}
}
