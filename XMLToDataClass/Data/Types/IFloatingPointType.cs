//********************************************************************************************************************************
// Filename:    IFloatingPointType.cs
// Owner:       Richard Dunkley
// Description: Interface supported by all floating point classes.
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

namespace XMLToDataClass.Data.Types
{
	public interface IFloatingPointType<T> where T : IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
	{
		#region Properties

		/// <summary>
		///   Gets or sets whether currency strings are allowed (Ex: $1,234.56).
		/// </summary>
		bool AllowCurrency { get; set; }

		/// <summary>
		///   Gets or sets whether parentheses are allowed for negative numbers (Ex: '(123)' is '-123').
		/// </summary>
		bool AllowParentheses { get; set; }

		/// <summary>
		///   Gets or sets whether exponents are allowed (Ex: '123E34').
		/// </summary>
		bool AllowExponent { get; set; }

		/// <summary>
		///   Gets or sets whether percents are allowed (Ex: '98.75%' is '0.9875').
		/// </summary>
		bool AllowPercent { get; set; }

		/// <summary>
		///   Gets or sets the maximum value the floating point type can be.
		/// </summary>
		T MaximumValue { get; set; }

		/// <summary>
		///   Gets or sets the minimum value the floating point type can be.
		/// </summary>
		T MinimumValue { get; set; }

		#endregion Properties

		#region Methods

		/// <summary>
		///   Attempts to parse the text to the floating point type.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>True if the string was successfully parsed, false otherwise.</returns>
		bool TryParse(string value);

		/// <summary>
		///   Attempts to parse the <i>value</i> string to the floating point type.
		/// </summary>
		/// <param name="value">String to be parsed.</param>
		/// <returns>True if the value was parsed successfully, false otherwise.</returns>
		/// <remarks>This method is used to parse values for min and max and does not allow currency, parentheses, or percent string values.</remarks>
		bool TryParseForMinMax(string value);

		/// <summary>
		///   Parses the string to the floating point value.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>The floating point type parsed from the string.</returns>
		/// <exception cref="ArgumentNullException"><i>value</i> is a null reference.</exception>
		/// <exception cref="ArgumentException"><i>value</i> is an empty string.</exception>
		T Parse(string value);

		/// <summary>
		///   Parses the <i>value</i> string and returns the parsed value.
		/// </summary>
		/// <param name="value">String to be parsed.</param>
		/// <returns>Value parsed from the string.</returns>
		/// <remarks>This method is used to parse values for min and max and does not allow currency, parentheses, or percent string values.</remarks>
		/// <exception cref="ArgumentNullException"><i>value</i> is a null reference.</exception>
		/// <exception cref="ArgumentException">The string provided was not valid.</exception>
		T ParseForMinMax(string value);

		#endregion Methods
	}
}
