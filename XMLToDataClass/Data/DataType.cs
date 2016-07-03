//********************************************************************************************************************************
// Filename:    DataType.cs
// Owner:       Richard Dunkley
// Description: Enumeration containing the various data types that can be parsed from the XML nodes.
//********************************************************************************************************************************
// Copyright © Richard Dunkley 2016
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the
// License. You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0  Unless required by applicable
// law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and
// limitations under the License.
//********************************************************************************************************************************

namespace XMLToDataClass.Data
{
	/// <summary>
	///   Defines the type of attributes that are supported. Numbered from most restrictive to least restrictive.
	/// </summary>
	public enum DataType
	{
		/// <summary>
		///   Represents a boolean value type.
		/// </summary>
		Boolean = 3,

		/// <summary>
		///   Represents a 8-bit unsigned integer.
		/// </summary>
		Byte = 6,

		/// <summary>
		///   Represents a 64-bit unsigned integer.
		/// </summary>
		Int = 4,

		/// <summary>
		///   Represents a 64-bit integer.
		/// </summary>
		Long = 10,

		/// <summary>
		///   Represents a 8-bit integer.
		/// </summary>
		SByte = 5,

		/// <summary>
		///   Represents a 16-bit integer.
		/// </summary>
		Short = 7,

		/// <summary>
		///   Represents a string type.
		/// </summary>
		String = 12,

		/// <summary>
		///   Represents a 32-bit unsigned integer.
		/// </summary>
		UInt = 9,

		/// <summary>
		///   Represents a 64-bit unsigned integer.
		/// </summary>
		ULong = 11,

		/// <summary>
		///   Represents a 16-bit unsigned integer.
		/// </summary>
		UShort = 8,

		/// <summary>
		///   Represents a 32-bit floating point value.
		/// </summary>
		Float = 2,

		/// <summary>
		///   Represents a 64-bit floating point value.
		/// </summary>
		Double = 1,

		/// <summary>
		///   Represents a date time value.
		/// </summary>
		DateTime = 0,

		/// <summary>
		///  Represents an enumerated type.
		/// </summary>
		Enum = 13,
	}
}
