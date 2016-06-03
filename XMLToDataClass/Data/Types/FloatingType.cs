using CSCodeGen;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLToDataClass.Data.Types
{
	public class FloatingType<T> : BaseType, IFloatingPointType<T> where T : IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
	{
		#region Properties

		/// <summary>
		///   Gets or sets whether currency strings are allowed (Ex: $1,234.56).
		/// </summary>
		public bool AllowCurrency { get; set; }

		/// <summary>
		///   Gets or sets whether parentheses are allowed for negative numbers (Ex: '(123)' is '-123').
		/// </summary>
		public bool AllowParentheses { get; set; }

		/// <summary>
		///   Gets or sets whether exponents are allowed (Ex: '123E34').
		/// </summary>
		public bool AllowExponent { get; set; }

		/// <summary>
		///   Gets or sets whether percents are allowed (Ex: '98.75%' is '0.9875').
		/// </summary>
		public bool AllowPercent { get; set; }

		public static bool DefaultAllowCurrency { get; set; }
		public static bool DefaultAllowParentheses { get; set; }
		public static bool DefaultAllowExponent { get; set; }
		public static bool DefaultAllowPercent { get; set; }

		public static T DefaultMaximumValue { get; set; }
		public static T DefaultMinimumValue { get; set; }

		/// <summary>
		///   Gets or sets the maximum value the floating point type can be.
		/// </summary>
		public T MaximumValue { get; set; }

		/// <summary>
		///   Gets or sets the minimum value the floating point type can be.
		/// </summary>
		public T MinimumValue { get; set; }

		#endregion Properties

		#region Methods

		static FloatingType()
		{
			DefaultAllowCurrency = true;
			DefaultAllowParentheses = true;
			DefaultAllowExponent = true;
			DefaultAllowPercent = true;
			DefaultMaximumValue = GetMaxValue();
			DefaultMinimumValue = GetMinValue();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"><see cref="DataInfo"/> object associated with this type.</param>
		/// <param name="possibleValues">Possible values the data type will have to parse. Can be empty.</param>
		/// <param name="ignoreCase">True if the case of the values should be ignored, false otherwise. May not have any bearing on certain types.</param>
		public FloatingType(DataInfo info, string[] possibleValues, bool ignoreCase) : base(info, possibleValues, ignoreCase)
		{
			AllowCurrency = DefaultAllowCurrency;
			AllowParentheses = DefaultAllowParentheses;
			AllowExponent = DefaultAllowExponent;
			AllowPercent = DefaultAllowPercent;
			MinimumValue = DefaultMinimumValue;
			MaximumValue = DefaultMaximumValue;

			switch(typeof(T).Name.ToLower())
			{
				case "single":
					Type = DataType.Float;
					DisplayName = "Single Precision (32-bit) floating point number";
					break;
				case "double":
					Type = DataType.Double;
					DisplayName = "Double Precision (64-bit) floating point number";
					break;
				default:
					throw new ArgumentException("The generic type must be created with the following value types as T: float or double.");
			}
			IsNullable = true;
			DataTypeString = Enum.GetName(typeof(DataType), Type).ToLower();
		}

		/// <summary>
		///   Gets the maximum value supported by the type.
		/// </summary>
		/// <returns>Maximum supported value of the type.</returns>
		private static T GetMaxValue()
		{
			switch (typeof(T).Name.ToLower())
			{
				case "single":
					return (T)(object)float.MaxValue;
				case "double":
					return (T)(object)double.MaxValue;
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
				case "single":
					return (T)(object)float.MinValue;
				case "double":
					return (T)(object)double.MinValue;
				default:
					throw new NotImplementedException("The type of this class is not recognized as a supported type");
			}
		}

		public override EnumInfo[] GenerateAdditionalEnums()
		{
			if (MaximumValue.CompareTo(MinimumValue) <= 0)
				throw new InvalidOperationException("An attempt was made to generate a floating number parsing method, but the maximum value specified is less than or equal to the minimum value specified.");

			int count = 1;
			if (AllowCurrency)
				count++;
			if (AllowParentheses)
				count++;
			if (AllowExponent)
				count++;
			if (AllowPercent)
				count++;

			if (count < 2)
				return new EnumInfo[0];

			EnumInfo enumInfo = new EnumInfo
			(
				"public",
				string.Format("{0}", GetEnumTypeName(mInfo.PropertyName)),
				string.Format("Represents the formats the {0} can be parsed from", mInfo.PropertyName)
			);

			if (AllowCurrency)
				enumInfo.Values.Add(new EnumValueInfo("Currency", null, "Floating point number containing the currency symbol (Ex: $123.45)."));
			if (AllowParentheses)
				enumInfo.Values.Add(new EnumValueInfo("NegativeParentheses", null, "Floating point number containing wrapped in parentheses to represent a negative number (Ex: '(123)')."));
			if (AllowExponent)
				enumInfo.Values.Add(new EnumValueInfo("Exponent", null, "Floating point number containing an exponent (Ex: '123E45', '123E+45', or '123E-45')."));
			if(AllowPercent)
				enumInfo.Values.Add(new EnumValueInfo("Percent", null, "Floating point number representing a percent (Ex: '97.3%', '100%', or '0.3%')."));
			enumInfo.Values.Add(new EnumValueInfo("Decimal", null, "Decimal number (Ex: 1,234.5, 1234.567 or -1234.7)."));
			return new EnumInfo[] { enumInfo };
		}

		public override PropertyInfo[] GenerateAdditionalProperties()
		{
			if (MaximumValue.CompareTo(MinimumValue) <= 0)
				throw new InvalidOperationException("An attempt was made to generate a floating number parsing method, but the maximum value specified is less than or equal to the minimum value specified.");

			List<PropertyInfo> propList = new List<PropertyInfo>();
			if (mInfo.IsOptional && mInfo.CanBeEmpty)
			{
				// Can't tell empty and null apart from null float so store the info.
				propList.Add(new PropertyInfo
				(
					"public",
					"bool",
					string.Format("{0}NullIsEmpty", mInfo.PropertyName),
					string.Format("True if the null {0} boolean should be represented as an empty string in XML, false if it shouldn't be included.", mInfo.PropertyName)
				));
			}

			int count = 1;
			if (AllowCurrency)
				count++;
			if (AllowParentheses)
				count++;
			if (AllowExponent)
				count++;
			if (AllowPercent)
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
					string.Format("Determines what format the {0} floating type should be converted to in the XML string", mInfo.PropertyName)
				));
			}
			return propList.ToArray();
		}

		public override string[] GenerateExportMethodCode()
		{
			if (MaximumValue.CompareTo(MinimumValue) <= 0)
				throw new InvalidOperationException("An attempt was made to generate a floating number parsing method, but the maximum value specified is less than or equal to the minimum value specified.");

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

			int count = 1;
			if (AllowCurrency)
				count++;
			if (AllowExponent)
				count++;
			if (AllowParentheses)
				count++;
			if (AllowPercent)
				count++;

			codeLines.Add(string.Empty);
			if (count == 1)
			{
				// Only one type is allowed so output in that type.
				if (AllowCurrency)
					codeLines.Add(string.Format("return {0}.ToString(\"C\");", mInfo.PropertyName));
				else if (AllowExponent)
					codeLines.Add(string.Format("return {0}.ToString(\"E\");", mInfo.PropertyName));
				else if (AllowParentheses)
				{
					codeLines.Add(string.Format("if({0} < 0)", mInfo.PropertyName));
					codeLines.Add(string.Format("	return string.Format(\"({{0}})\", (Math.Abs({0})).ToString(\"N\"));", mInfo.PropertyName));
					codeLines.Add(string.Format("return {0}.ToString(\"N\");", mInfo.PropertyName));
				}
				else if(AllowPercent)
					codeLines.Add(string.Format("return {0}.ToString(\"P\");", mInfo.PropertyName));
				else
					codeLines.Add(string.Format("return {0}.ToString(\"N\");", mInfo.PropertyName));
			}
			else
			{
				int index = 0;
				StringBuilder temp = new StringBuilder();
				temp.Append(string.Format("if({0} == {1}.", enumPropertyName, enumTypeName));
				if (AllowCurrency)
				{
					temp.Append("Currency)");
					codeLines.Add(temp.ToString());
					temp.Clear();
					codeLines.Add(string.Format("	return {0}.ToString(\"C\");", mInfo.PropertyName));
					index++;
					if (index + 1 == count)
						codeLines.Add("	else");
					else
						temp.Append(string.Format("else if({0} == {1}.", enumPropertyName, enumTypeName));
				}
				if (AllowExponent)
				{
					if (index + 1 != count)
					{
						temp.Append("Exponent)");
						codeLines.Add(temp.ToString());
						temp.Clear();
					}
					codeLines.Add(string.Format("	return {0}.ToString(\"E\");", mInfo.PropertyName));
					index++;
					if (index + 1 == count)
						codeLines.Add("	else");
					else
						temp.Append(string.Format("else if({0} == {1}.", enumPropertyName, enumTypeName));
				}
				if (AllowParentheses)
				{
					if (index + 1 != count)
					{
						temp.Append("NegativeParentheses)");
						codeLines.Add(temp.ToString());
						temp.Clear();
					}
					codeLines.Add("{");
					codeLines.Add(string.Format("	if({0} < 0)", mInfo.PropertyName));
					codeLines.Add(string.Format("		return string.Format(\"({{0}})\", (Math.Abs({0})).ToString(\"N\"));", mInfo.PropertyName));
					codeLines.Add(string.Format("	return {0}.ToString(\"N\");", mInfo.PropertyName));
					codeLines.Add("}");
					index++;
					if (index + 1 == count)
						codeLines.Add("	else");
					else
						temp.Append(string.Format("else if({0} == {1}.", enumPropertyName, enumTypeName));
				}
				if (AllowPercent)
				{
					if (index + 1 != count)
					{
						temp.Append("Percent)");
						codeLines.Add(temp.ToString());
						temp.Clear();
					}
					codeLines.Add(string.Format("	return {0}.ToString(\"P\");", mInfo.PropertyName));
					index++;
					if (index + 1 == count)
						codeLines.Add("	else");
					else
						temp.Append(string.Format("else if({0} == {1}.", enumPropertyName, enumTypeName));
				}
				codeLines.Add(string.Format("	return {0}.ToString(\"N\");", mInfo.PropertyName));
			}
			return codeLines.ToArray();
		}

		public override string[] GenerateImportMethodCode()
		{
			if (MaximumValue.CompareTo(MinimumValue) <= 0)
				throw new InvalidOperationException("An attempt was made to generate a floating number parsing method, but the maximum value specified is less than or equal to the minimum value specified.");

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

			int count = 1;
			if (AllowCurrency)
				count++;
			if (AllowExponent)
				count++;
			if (AllowParentheses)
				count++;
			if (AllowPercent)
				count++;

			string enumPropertyName = GetEnumPropertyName(mInfo.PropertyName);
			string enumTypeName = GetEnumTypeName(mInfo.PropertyName);

			codeLines.Add(string.Format("{0} returnValue = 0;", DataTypeString));
			codeLines.Add("value = value.Trim();");
			codeLines.Add("try");
			codeLines.Add("{");
			bool first = true;
			if (AllowCurrency)
			{
				codeLines.Add(string.Empty);
				codeLines.Add("	if (value.Contains(NumberFormatInfo.CurrentInfo.CurrencySymbol))");
				codeLines.Add("	{");
				codeLines.Add("		// Number is represented as currency ($123.45).");
				codeLines.Add(string.Format("		returnValue = {0}.Parse(value, NumberStyles.Currency);", DataTypeString));
				if (count > 1)
					codeLines.Add(string.Format("		{0} = {1}.Currency;", enumPropertyName, enumTypeName));
				codeLines.Add("	}");
				first = false;
			}

			if (AllowPercent)
			{
				codeLines.Add(string.Empty);
				if (first)
					codeLines.Add("	if (value.Length > 1 && value[value.Length-1] == '%')");
				else
					codeLines.Add("	else if (value.Length > 1 && value[value.Length-1] == '%')");
				codeLines.Add("	{");
				codeLines.Add("		// Number is a percentage.");
				codeLines.Add(string.Format("		returnValue = {0}.Parse(value.Substring(0, value.Length-1), NumberStyles.Number) / 100;", DataTypeString));
				if (count > 1)
					codeLines.Add(string.Format("		{0} = {1}.Percent;", enumPropertyName, enumTypeName));
				codeLines.Add("	}");
			}

			if (AllowParentheses)
			{
				codeLines.Add(string.Empty);
				if (first)
					codeLines.Add("	if (value.Contains(\"(\") && value.Contains(\")\"))");
				else
					codeLines.Add("	else if (value.Contains(\"(\") && value.Contains(\")\"))");
				codeLines.Add("	{");
				codeLines.Add("		// Number is specified negative by parentheses.");
				codeLines.Add(string.Format("		returnValue = {0}.Parse(value, NumberStyles.Number|NumberStyles.AllowParentheses);", DataTypeString));
				if (count > 1)
					codeLines.Add(string.Format("		{0} = {1}.NegativeParentheses;", enumPropertyName, enumTypeName));
				codeLines.Add("	}");
				first = false;
			}

			if (AllowExponent)
			{
				codeLines.Add(string.Empty);
				if (first)
					codeLines.Add("	if (value.Contains(\"E\") || value.Contains(\"e\"))");
				else
					codeLines.Add("	else if (value.Contains(\"E\") || value.Contains(\"e\"))");
				codeLines.Add("	{");
				codeLines.Add("		// Number is an exponent.");
				codeLines.Add(string.Format("		returnValue = {0}.Parse(value, NumberStyles.Float);", DataTypeString));
				if (count > 1)
					codeLines.Add(string.Format("		{0} = {1}.Exponent;", enumPropertyName, enumTypeName));
				codeLines.Add("	}");
				first = false;
			}

			codeLines.Add(string.Empty);
			string space = string.Empty;
			if (!first)
			{
				codeLines.Add("	else");
				codeLines.Add("	{");
				space = "	";
			}
			codeLines.Add(string.Format("{0}	// Attempt to parse the number as just an decimal.", space));
			codeLines.Add(string.Format("{0}	returnValue = {1}.Parse(value, NumberStyles.Number);", space, DataTypeString));
			if (count > 1)
				codeLines.Add(string.Format("{0}	{1} = {2}.Decimal;", space, enumPropertyName, enumTypeName));
			if (!first)
				codeLines.Add("	}");

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

		/// <summary>
		///   Gets the enumeration type's name from the property name.
		/// </summary>
		/// <param name="propertyName">Property name of the corresponding <see cref="DataInfo"/> object.</param>
		/// <returns>Enumeration type's name.</returns>
		private string GetEnumTypeName(string propertyName)
		{
			return string.Format("{0}FloatFormat", propertyName);
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
			return TryParse(value, MinimumValue, MaximumValue, AllowCurrency, AllowExponent, AllowParentheses, AllowPercent);
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
				case "single":
					float floatValue;
					returnValue = float.TryParse(text, style, CultureInfo.CurrentCulture.NumberFormat, out floatValue);
					value = (T)(object)floatValue;
					break;
				case "double":
					double doubleValue;
					returnValue = double.TryParse(text, style, CultureInfo.CurrentCulture.NumberFormat, out doubleValue);
					value = (T)(object)doubleValue;
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
		private static bool TryParse(string value, T minValue, T maxValue, bool allowCurrency, bool allowExponent, bool allowParentheses, bool allowPercent)
		{
			if (value == null)
				return false;
			value = value.Trim();
			if (value.Length == 0)
				return false;

			T returnValue = DefaultMinimumValue;
			if (allowCurrency && value.Contains(NumberFormatInfo.CurrentInfo.CurrencySymbol))
			{
				// Number is represented as currency ($123.45).
				if (!TryParse(value, NumberStyles.Currency, out returnValue))
					return false;
			}
			else if (allowPercent && value.Length > 1 && char.ToLower(value[value.Length - 1]) == '%')
			{
				// Number is a percentage.
				if (!TryParse(value.Substring(0, value.Length - 1), NumberStyles.Number, out returnValue))
					return false;
			}
			else if (allowParentheses && value.Contains('(') && value.Contains(')'))
			{
				// Number is specified negative by parentheses.
				if (!TryParse(value, NumberStyles.Number|NumberStyles.AllowParentheses, out returnValue))
					return false;
			}
			else if (allowExponent && (value.Contains('E') || value.Contains('e')))
			{
				// Number is an exponent.
				if (!TryParse(value, NumberStyles.Float, out returnValue))
					return false;
			}
			else
			{
				// Attempt to parse the number as just an decimal.
				if (!TryParse(value, NumberStyles.Number, out returnValue))
					return false;
			}

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
		///   Attempts to parse the <i>value</i> string to the integral type.
		/// </summary>
		/// <param name="value">String to be parsed.</param>
		/// <returns>True if the value was parsed successfully, false otherwise.</returns>
		/// <remarks>This method is used to parse values for min and max and does not allow hex, or binary string values.</remarks>
		public bool TryParseForMinMax(string value)
		{
			return TryParse(value, GetMinValue(), GetMaxValue(), false, true, false, false);
		}

		/// <summary>
		///   Attempts to parse the <i>value</i> string to the integral type using the default settings.
		/// </summary>
		/// <param name="value">String to be parsed.</param>
		/// <param name="returnValue">Parsed value or zero if the parsing was unsuccessful.</param>
		/// <returns>True if the value was parsed successfully, false otherwise.</returns>
		public static bool TryParseWithDefaults(string value)
		{
			return TryParse(value, DefaultMinimumValue, DefaultMaximumValue, DefaultAllowCurrency, DefaultAllowExponent, DefaultAllowParentheses, DefaultAllowPercent);
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
			return Parse(value, MinimumValue, MaximumValue, AllowCurrency, AllowExponent, AllowParentheses, AllowPercent);
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
				case "single":
					return (T)(object)float.Parse(text, style);
				case "double":
					return (T)(object)double.Parse(text, style);
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
		private static T Parse(string value, T minValue, T maxValue, bool allowCurrency, bool allowExponent, bool allowParentheses, bool allowPercent)
		{
			if (value == null)
				throw new ArgumentNullException("value");
			if (value.Length == 0)
				throw new ArgumentException("value is an empty string");

			T returnValue = DefaultMinimumValue;
			try
			{
				if (allowCurrency && value.Contains(NumberFormatInfo.CurrentInfo.CurrencySymbol))
				{
					// Number is represented as currency ($123.45).
					returnValue = Parse(value, NumberStyles.Currency);
				}
				else if (allowPercent && value.Length > 1 && char.ToLower(value[value.Length - 1]) == '%')
				{
					// Number is a percentage.
					returnValue = Parse(value.Substring(0, value.Length - 1), NumberStyles.Number);
				}
				else if (allowParentheses && value.Contains('(') && value.Contains(')'))
				{
					// Number is specified negative by parentheses.
					returnValue = Parse(value, NumberStyles.Number | NumberStyles.AllowParentheses);
				}
				else if (allowExponent && (value.Contains('E') || value.Contains('e')))
				{
					// Number is an exponent.
					returnValue = Parse(value, NumberStyles.Float);
				}
				else
				{
					// Attempt to parse the number as just an decimal.
					returnValue = Parse(value, NumberStyles.Number);
				}
			}
			catch (FormatException e)
			{
				throw new ArgumentException(string.Format("The floating point value specified ({0}) is not in a valid format: {1}.", value, e.Message), e);
			}
			catch (OverflowException e)
			{
				throw new ArgumentException(string.Format("The floating point value specified ({0}) is larger or smaller than the integral type allows (Min: {1}, Max: {2}).", value, GetMinValue().ToString(), GetMaxValue().ToString()), e);
			}

			if (maxValue.CompareTo(GetMaxValue()) < 0)
			{
				// Verify that the value has not excedded the specified maximum size.
				if (returnValue.CompareTo(maxValue) > 0)
					throw new ArgumentException(string.Format("The floating point value specified ({0}) is larger than the maximum value allowed ({1}).", value, GetMaxValue().ToString()));
			}
			if (minValue.CompareTo(GetMinValue()) > 0)
			{
				// Verify that the value has not exceeded the specified minimum size.
				if (returnValue.CompareTo(minValue) < 0)
					throw new ArgumentException(string.Format("The floating point value specified ({0}) is smaller than the minimal value allowed ({1}).", value, GetMinValue().ToString()));
			}
			return returnValue;
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
			return Parse(value, GetMinValue(), GetMaxValue(), false, true, false, false);
		}

		/// <summary>
		///   Parses the <i>value</i> string using the default settings and returns the parsed value.
		/// </summary>
		/// <param name="value">String to be parsed.</param>
		/// <returns>Value parsed from the string.</returns>
		/// <exception cref="ArgumentException">The string provided was not valid.</exception>
		public static T ParseWithDefaults(string value)
		{
			return Parse(value, DefaultMinimumValue, DefaultMaximumValue, DefaultAllowCurrency, DefaultAllowExponent, DefaultAllowParentheses, DefaultAllowPercent);
		}

		#endregion Methods
	}
}
