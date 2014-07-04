/********************************************************************************************************************************
 * Copyright 2014 Richard Dunkley
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Globalization;

namespace XMLToDataClass.Data
{
	/// <summary>
	///   Provides various static methods for using <see cref="DataType"/>s.
	/// </summary>
	public static class DataTypeUtility
	{
		#region Methods

		/// <summary>
		///   Gets the string representation of the C# type that matches the specified <see cref="DataType"/>.
		/// </summary>
		/// <param name="type"><see cref="DataType"/> to get the type of.</param>
		/// <returns>String representation of the C# type.</returns>
		/// <exception cref="InvalidOperationException">The <i>type</i> was not recognized.</exception>
		public static string GetDataTypeString(DataType type)
		{
			switch (type)
			{
				case DataType.Boolean:
					return "bool";
				case DataType.DateTime:
					return "DateTime";
				case DataType.Double:
					return "double";
				case DataType.Float:
					return "float";
				case DataType.Int16:
					return "short";
				case DataType.Int32:
					return "int";
				case DataType.Int64:
					return "long";
				case DataType.Int8:
					return "sbyte";
				case DataType.String:
					return "string";
				case DataType.UInt16:
					return "ushort";
				case DataType.UInt32:
					return "uint";
				case DataType.UInt64:
					return "ulong";
				case DataType.UInt8:
					return "byte";
				default:
					throw new InvalidOperationException("An DataType was used that was not recognized.");
			}
		}

		/// <summary>
		///   Determines if the <see cref="DataType"/> represents a nullable type.
		/// </summary>
		/// <param name="type"><see cref="DataType"/> to determine if nullable.</param>
		/// <returns>True if <i>type</i> represents a nullable type, false otherwise.</returns>
		public static bool IsNullableType(DataType type)
		{
			switch (type)
			{
				case DataType.Boolean:
				case DataType.Double:
				case DataType.Float:
				case DataType.Int16:
				case DataType.Int32:
				case DataType.Int64:
				case DataType.Int8:
				case DataType.UInt16:
				case DataType.UInt32:
				case DataType.UInt64:
				case DataType.UInt8:
				case DataType.DateTime:
					return true;
			}
			return false;
		}

		/// <summary>
		///   Determines if the provided <see cref="DataType"/> represents an unsigned integer type.
		/// </summary>
		/// <param name="type"><see cref="DataType"/> to determine if unsigned integer.</param>
		/// <returns>True if it is an unsigned integer, false otherwise.</returns>
		public static bool IsUnsignedInteger(DataType type)
		{
			switch (type)
			{
				case DataType.UInt8:
				case DataType.UInt16:
				case DataType.UInt32:
				case DataType.UInt64:
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		///   Gets the attribute type.
		/// </summary>
		/// <param name="value">String to determine the attribute type.</param>
		/// <returns>The attribute <see cref="DataType"/>.</returns>
		public static DataType? GetAttributeType(string value)
		{
			if (value == null || value.Length == 0)
				return null;

			// Determine if it is a boolean value.
			if (string.Compare(value, "true", true) == 0)
				return DataType.Boolean;
			if (string.Compare(value, "false", true) == 0)
				return DataType.Boolean;

			// Determine if it is a date time.
			try
			{
				DateTime.Parse(value);
				return DataType.DateTime;
			}
			catch (FormatException)
			{
			}

			// Determine if it is an Int8.
			if (ParseInteger(value, DataType.Int8))
				return DataType.Int8;

			// Determine if it is an UInt8.
			if (ParseInteger(value, DataType.UInt8))
				return DataType.UInt8;

			// Determine if it is an Int16.
			if (ParseInteger(value, DataType.Int16))
				return DataType.Int16;

			// Determine if it is an UInt16.
			if (ParseInteger(value, DataType.UInt16))
				return DataType.UInt16;

			// Determine if it is an Int32.
			if (ParseInteger(value, DataType.Int32))
				return DataType.Int32;

			// Determine if it is an UInt32.
			if (ParseInteger(value, DataType.UInt32))
				return DataType.UInt32;

			// Determine if it is an Int64.
			if (ParseInteger(value, DataType.Int64))
				return DataType.Int64;

			// Determine if it is an UInt64.
			if (ParseInteger(value, DataType.UInt64))
				return DataType.UInt64;

			// Determine if it is a floating type.
			try
			{
				float.Parse(value);
				return DataType.Float;
			}
			catch (FormatException)
			{
			}
			catch (OverflowException)
			{
			}

			// Determine if it is a decimal.
			try
			{
				double.Parse(value);
				return DataType.Double;
			}
			catch (FormatException)
			{
			}
			catch (OverflowException)
			{
			}

			return DataType.String;
		}

		/// <summary>
		///   Attempts to try and parse an integer value.
		/// </summary>
		/// <param name="numberString">String to be parsed.</param>
		/// <param name="style"><see cref="NumberStyles"/> specifying the type of string.</param>
		/// <param name="intType"><see cref="DataType"/> of integer to be parsed.</param>
		/// <returns>True if the number can be parsed, false otherwise.</returns>
		private static bool TryParseInteger(string numberString, NumberStyles style, DataType intType)
		{
			switch (intType)
			{
				case DataType.UInt8:
					byte byteValue;
					return byte.TryParse(numberString, style, null, out byteValue);
				case DataType.Int8:
					sbyte sbyteValue;
					return sbyte.TryParse(numberString, style, null, out sbyteValue);
				case DataType.UInt16:
					ushort ushortValue;
					return ushort.TryParse(numberString, style, null, out ushortValue);
				case DataType.Int16:
					short shortValue;
					return short.TryParse(numberString, style, null, out shortValue);
				case DataType.UInt32:
					uint uintValue;
					return uint.TryParse(numberString, style, null, out uintValue);
				case DataType.Int32:
					int intValue;
					return int.TryParse(numberString, style, null, out intValue);
				case DataType.UInt64:
					ulong ulongValue;
					return ulong.TryParse(numberString, style, null, out ulongValue);
				case DataType.Int64:
					long longValue;
					return long.TryParse(numberString, style, null, out longValue);
				default:
					return false;
			}
		}

		/// <summary>
		///   Gets the maximum number of bits that can represent the integer type.
		/// </summary>
		/// <param name="intType"><see cref="DataType"/> representing the integer.</param>
		/// <returns>Maximum number of bits that can represent the integer, 0 if a non-integer <see cref="DataType"/> was provided.</returns>
		public static int GetIntegerMaxBitLength(DataType intType)
		{
			switch (intType)
			{
				case DataType.UInt8:
				case DataType.Int8:
					return 8;
				case DataType.UInt16:
				case DataType.Int16:
					return 16;
				case DataType.UInt32:
				case DataType.Int32:
					return 32;
				case DataType.UInt64:
				case DataType.Int64:
					return 64;
				default:
					return 0;
			}
		}

		/// <summary>
		///   Parses the <i>numberString</i> and returns the parsed integer value.
		/// </summary>
		/// <param name=\"value\">String containing a valid text representation of the integer.</param>
		/// <param name="intType"><see cref="DataType"/> used to determine which integer type should be parsed.</param>
		/// <returns>True if the integer could be parsed, false otherwise.</returns>
		/// <exception cref=\"InvalidDataException\">Unable to parse <i>value</i> into a valid number.</exception>
		/// <remarks>
		///   The following formats for integers are valid for unsigned types: 000000b (Binary), 0x00 (Hexadecimal), or 00h (Hexadecimal).  The following formats
		///   for integers are valid for signed types: 123 (Standard Integer Representation).
		/// </remarks>
		private static bool ParseInteger(string value, DataType intType)
		{
			// Check if the number is specified as a hexedecimal number.
			if (value.Length > 2 && value[0] == '0' && char.ToLower(value[1]) == 'x')
			{
				// If we are trying to parse a hexadecimal number then we assum it is an unsigned.
				if (!IsUnsignedInteger(intType))
					return false;

				return TryParseInteger(value.Substring(2), NumberStyles.AllowHexSpecifier, intType);
			}

			// Check if the number is a hexedecimal number.
			if (char.ToLower(value[value.Length - 1]) == 'h')
			{
				// If we are trying to parse a hexadecimal number then we assum it is an unsigned.
				if (!IsUnsignedInteger(intType))
					return false;

				return TryParseInteger(value.Substring(0, value.Length - 1), NumberStyles.AllowHexSpecifier, intType);
			}

			// Check if the number is a binary number.
			if (char.ToLower(value[value.Length - 1]) == 'b')
			{
				// If we are trying to parse a binary number then we assum it is an unsigned.
				if (!IsUnsignedInteger(intType))
					return false;

				int maxSize = GetIntegerMaxBitLength(intType);
				if (value.Length - 1 > maxSize)
					return false;

				string bitString = value.Substring(0, value.Length - 1);
				ulong bitValue = 0;
				for (int i = 0; i < bitString.Length; i++)
				{
					if (bitString[i] == '1')
					{
						bitValue *= 2;
						bitValue += 1;
					}
					else if (bitString[i] == '0')
					{
						bitValue *= 2;
					}
					else
					{
						return false;
					}
				}
				return true;
			}

			// Attempt to parse the number as just an integer.
			return TryParseInteger(value, NumberStyles.Integer, intType);
		}

		/// <summary>
		///   Determines the attribute type from the current type and a new string representation.
		/// </summary>
		/// <param name="value">String representation of the type.</param>
		/// <param name="type">Previously determined type.</param>
		/// <returns><see cref="DataType"/> object denoting the type of the attribute.</returns>
		public static DataType? DetermineAttributeType(string value, DataType? type)
		{
			// Get the new type based on the value.
			DataType? newType = GetAttributeType(value);

			if (newType.HasValue)
			{
				if (type.HasValue)
				{
					if ((int)type < (int)newType)
						return type;
					return newType;
				}
				else
				{
					return newType;
				}
			}
			else
			{
				if (type.HasValue)
					return type;
				else
					return null;
			}
		}

		#endregion Methods
	}
}
