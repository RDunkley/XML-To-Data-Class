//********************************************************************************************************************************
// Filename:    IIntegralType.cs
// Owner:       Richard Dunkley
// Description: Interface supported by all integral type classes.
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
	/// <summary>
	///   Interface for integer types. Allows a generic display panel to be used with the integral type accessed through this interface.
	/// </summary>
	/// <typeparam name="T">Integral type (byte, sbyte, short, ushort, int, uint, long, ulong.</typeparam>
	public interface IIntegralType<T> where T : IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
	{
		#region Properties

		bool AllowHexType1Values { get; set; }

		bool AllowHexType2Values { get; set; }

		bool AllowBinaryValues { get; set; }

		bool AllowIntegerValues { get; set; }

		/// <summary>
		///   Gets or sets the minimum value the integral type can be.
		/// </summary>
		T MinimumValue { get; set; }

		/// <summary>
		///   Gets or sets the maximum value the integral type can be.
		/// </summary>
		T MaximumValue { get; set; }

		#endregion Properties

		#region Methods

		/// <summary>
		///   Attempts to parse the text to the integral type.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>True if the string was successfully parsed, false otherwise.</returns>
		bool TryParse(string value);

		/// <summary>
		///   Attempts to parse the <i>value</i> string to the integral type.
		/// </summary>
		/// <param name="value">String to be parsed.</param>
		/// <returns>True if the value was parsed successfully, false otherwise.</returns>
		/// <remarks>This method is used to parse values for min and max and does not allow hex, or binary string values.</remarks>
		bool TryParseForMinMax(string value);

		/// <summary>
		///   Parses the string to the integral value.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>The integral type parsed from the string.</returns>
		/// <exception cref="ArgumentNullException"><i>value</i> is a null reference.</exception>
		/// <exception cref="ArgumentException"><i>value</i> is an empty string.</exception>
		T Parse(string value);

		/// <summary>
		///   Parses the <i>value</i> string and returns the parsed value.
		/// </summary>
		/// <param name="value">String to be parsed.</param>
		/// <returns>Value parsed from the string.</returns>
		/// <remarks>This method is used to parse values for min and max and does not allow hex, or binary string values.</remarks>
		/// <exception cref="ArgumentNullException"><i>value</i> is a null reference.</exception>
		/// <exception cref="ArgumentException">The string provided was not valid.</exception>
		T ParseForMinMax(string value);

		#endregion Methods
	}
}
