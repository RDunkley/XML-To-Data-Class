using CSCodeGen;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		/// 
		/// </summary>
		/// <param name="info"><see cref="DataInfo"/> object associated with this type.</param>
		/// <param name="possibleValues">Possible values the data type will have to parse. Can be empty.</param>
		/// <param name="ignoreCase">True if the case of the values should be ignored, false otherwise. May not have any bearing on certain types.</param>
		/// <exception cref="ArgumentNullException"><i>possibleValues</i> or <i>info</i> is a null reference.</exception>
		public DateTimeType(DataInfo info, string[] possibleValues, bool ignoreCase) : base(info, possibleValues, ignoreCase)
		{
			DateTimeSelect = DefaultDateTimeSelect;
			Culture = DefaultCulture;

			Type = DataType.DateTime;
			IsNullable = true;
			DataTypeString = "DateTime";
			DisplayName = "Date/Time";
		}

		/// <summary>
		///   Generates additional enumerations used by the import or export methods.
		/// </summary>
		/// <returns><see cref="EnumInfo"/> array represnenting additional fields needed by the import/export methods. Can be empty.</returns>
		/// <exception cref="InvalidOperationException">An attempt was made to generate a boolean parsing method, but none of the valid boolean string pairs were allowed. At least one boolean string pair (Ex: 'true/false', 'yes/no', or '0/1') must be allowed.</exception>
		public override EnumInfo[] GenerateAdditionalEnums()
		{
			return new EnumInfo[0];
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
		///   Attempts to parse a value to the boolean, using it's current settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		public override bool TryParse(string value)
		{
			return TryParse(value, DateTimeSelect, Culture);
		}

		/// <summary>
		///   Attempts to parse a value to the boolean, using the specified settings.
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
		///   Attempts to parse a value to a boolean, using the default settings.
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
