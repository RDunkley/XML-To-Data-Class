//********************************************************************************************************************************
// Filename:    DateTimeType.cs
// Owner:       Richard Dunkley
// Description: Class which represents a date/time value in an XML file.
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
using System.Xml;

namespace XMLToDataClass.Data.Types
{
	public class DateTimeType : BaseType
	{
		#region Enumerations

		public enum DateTimeOption
		{
			DateTime,
			Date,
			Time,
			YearMonth,
			MonthDay,
		}

		#endregion Enumerations

		#region Properties

		/// <summary>
		///   Default setting to allow 'True' and 'False' strings.
		/// </summary>
		public static DateTimeOption DefaultDateTimeSelect { get; set; }

		public static CultureInfo DefaultCulture { get; set; }

		/// <summary>
		///   True to allow 'True' and 'False' strings, false to disallow.
		/// </summary>
		public DateTimeOption DateTimeSelect { get; set; }

		public CultureInfo Culture { get; set; }

		#endregion Properties

		#region Methods

		static DateTimeType()
		{
			DefaultDateTimeSelect = DateTimeOption.DateTime;
			DefaultCulture = CultureInfo.CurrentCulture;
		}

		/// <summary>
		///   Instantiates a new <see cref="DateTimeType"/> object.
		/// </summary>
		/// <param name="info"><see cref="DataInfo"/> object associated with this type.</param>
		/// <param name="possibleValues">Possible values the data type will have to parse. Can be empty.</param>
		/// <exception cref="ArgumentNullException"><paramref name="possibleValues"/> or <paramref name="info"/> is a null reference.</exception>
		public DateTimeType(DataInfo info, string[] possibleValues) : base(info, possibleValues)
		{
			DateTimeSelect = DefaultDateTimeSelect;
			Culture = DefaultCulture;

			Type = DataType.DateTime;
			DisplayName = "Date/Time";
		}

		/// <summary>
		///   String of the C# representive data type.
		/// </summary>
		/// <returns>String containing the C# data type.</returns>
		public override string GetDataTypeString()
		{
			return "DateTime";
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
					string.Format("True if the null {0} DateTime should be represented as an empty string in XML, false if it shouldn't be included.", mInfo.PropertyName)
				));
			}

			string formatPropertyName = GetFormatPropertyName(mInfo.PropertyName);

			propList.Add(new PropertyInfo
			(
				"public",
				"string",
				formatPropertyName,
				string.Format("Stores how the {0} date/time is converted to XML string", mInfo.PropertyName)
			));
			return propList.ToArray();
		}

		/// <summary>
		///   Generates the export method code for the boolean.
		/// </summary>
		/// <returns>String array representing the export method code.</returns>
		/// <exception cref="InvalidOperationException">An attempt was made to generate a boolean parsing method, but none of the valid boolean string pairs were allowed. At least one boolean string pair (Ex: 'true/false', 'yes/no', or '0/1') must be allowed.</exception>
		public override string[] GenerateExportMethodCode()
		{
			string formatPropertyName = GetFormatPropertyName(mInfo.PropertyName);
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
			codeLines.Add(string.Format("return {0}.ToString({1}, culture);", name, formatPropertyName));
			return codeLines.ToArray();
		}

		/// <summary>
		///   Generates the import method code for the boolean.
		/// </summary>
		/// <returns>String array representing the import method code.</returns>
		/// <exception cref="InvalidOperationException">An attempt was made to generate a boolean parsing method, but none of the valid boolean string pairs were allowed. At least one boolean string pair (Ex: 'true/false', 'yes/no', or '0/1') must be allowed.</exception>
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

			string formatPropertyName = GetFormatPropertyName(mInfo.PropertyName);
			codeLines.Add(string.Format("CultureInfo info = CultureInfo.GetCultureInfo(\"{0}\");", Culture.Name));
			codeLines.Add("DateTimeStyles styles = DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.RoundtripKind;");
			codeLines.Add("DateTime returnValue;");
			List<string> options = new List<string>();
			if(DateTimeSelect == DateTimeOption.Date || DateTimeSelect == DateTimeOption.DateTime)
			{
				options.Add("d");
				options.Add("D");
			}
			if(DateTimeSelect == DateTimeOption.Time || DateTimeSelect == DateTimeOption.DateTime)
			{
				options.Add("t");
				options.Add("T");
			}
			if (DateTimeSelect == DateTimeOption.MonthDay || DateTimeSelect == DateTimeOption.DateTime)
				options.Add("m");
			if (DateTimeSelect == DateTimeOption.YearMonth || DateTimeSelect == DateTimeOption.DateTime)
				options.Add("y");
			if (DateTimeSelect == DateTimeOption.DateTime)
			{
				options.Add("f");
				options.Add("F");
				options.Add("g");
				options.Add("G");
				options.Add("o");
				options.Add("R");
				options.Add("s");
				options.Add("u");
				options.Add("U");
			}

			foreach(string option in options)
			{
				codeLines.Add(string.Empty);
				codeLines.Add(string.Format("if(DateTime.TryParseExact(value, \"{0}\", info, styles, out returnValue))", option));
				codeLines.Add("{");
				codeLines.Add(string.Format("	{0} = returnValue;", mInfo.PropertyName));
				codeLines.Add(string.Format("	{0} = \"{1}\";", formatPropertyName, option));
				codeLines.Add("	return;");
				codeLines.Add("}");
			}

			codeLines.Add(string.Empty);
			codeLines.Add("throw new InvalidDataException(string.Format(\"The Date/Time value specified ({0}) is not a valid DateTime standard string representation\", value));");
			return codeLines.ToArray();
		}

		/// <summary>
		///   Gets the stored format property's name.
		/// </summary>
		/// <param name="propertyName">Property name of the corresponding <see cref="DataInfo"/> object.</param>
		/// <returns>Stored enumeration property name.</returns>
		private string GetFormatPropertyName(string propertyName)
		{
			return string.Format("{0}ParsedFormat", propertyName);
		}

		/// <summary>
		///   Saves the types configuration properties to XML child elements.
		/// </summary>
		/// <param name="doc"><see cref="XmlDocument"/> object representing the XML document to be written.</param>
		/// <param name="parent">Parent <see cref="XmlNode"/> to append the child settings to.</param>
		public override void Save(XmlDocument doc, XmlNode parent)
		{
			// Add the DateTimeSelect option.
			XmlElement element = doc.CreateElement("setting");
			XmlAttribute attrib = element.Attributes.Append(doc.CreateAttribute("name"));
			attrib.Value = "DateTimeSelect";
			attrib = element.Attributes.Append(doc.CreateAttribute("value"));
			attrib.Value = Enum.GetName(typeof(DateTimeOption), DateTimeSelect);
			parent.AppendChild(element);

			// Add the Culture option.
			element = doc.CreateElement("setting");
			attrib = element.Attributes.Append(doc.CreateAttribute("name"));
			attrib.Value = "Culture";
			attrib = element.Attributes.Append(doc.CreateAttribute("value"));
			attrib.Value = Culture.ToString();
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
						// Set DateTimeSelect if found.
						if (nameAttrib.Value == "DateTimeSelect")
						{
							DateTimeOption value;
							if (Enum.TryParse<DateTimeOption>(valueAttrib.Value, out value))
								DateTimeSelect = value;
						}

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
		///   Attempts to parse a value to the date/time, using it's current settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		public override bool TryParse(string value)
		{
			return TryParse(value, DateTimeSelect, Culture);
		}

		/// <summary>
		///   Attempts to parse a value to the date/time, using the specified settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <param name="option"></param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		private static bool TryParse(string value, DateTimeOption option, CultureInfo culture)
		{
			if (value == null)
				return false;
			if (value.Length == 0)
				return false;

			List<string> options = new List<string>();
			if (option == DateTimeOption.Date || option == DateTimeOption.DateTime)
			{
				options.Add("d");
				options.Add("D");
			}
			if (option == DateTimeOption.Time || option == DateTimeOption.DateTime)
			{
				options.Add("t");
				options.Add("T");
			}
			if (option == DateTimeOption.MonthDay || option == DateTimeOption.DateTime)
				options.Add("m");
			if (option == DateTimeOption.YearMonth || option == DateTimeOption.DateTime)
				options.Add("Y");
			if (option == DateTimeOption.DateTime)
			{
				options.Add("f");
				options.Add("F");
				options.Add("g");
				options.Add("G");
				options.Add("o");
				options.Add("R");
				options.Add("s");
				options.Add("u");
				options.Add("U");
			}

			DateTimeStyles styles = DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.RoundtripKind;
			DateTime returnValue;
			foreach (string item in options)
			{
				if (DateTime.TryParseExact(value, item, culture, styles, out returnValue))
					return true;
			}
			return false;
		}

		/// <summary>
		///   Attempts to parse a value to a date/time, using the default settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		public static bool TryParseWithDefaults(string value)
		{
			return TryParse(value, DefaultDateTimeSelect, DefaultCulture);
		}

		#endregion Methods
	}
}
