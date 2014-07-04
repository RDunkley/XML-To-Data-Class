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

namespace XMLToDataClass.Data
{
	/// <summary>
	///   Defines the type of attributes that are supported.
	/// </summary>
	public enum DataType
	{
		/// <summary>
		///   Represents a string type.
		/// </summary>
		String = 0,

		/// <summary>
		///   Represents a 64-bit integer.
		/// </summary>
		Int64 = 1,

		/// <summary>
		///   Represents a 64-bit unsigned integer.
		/// </summary>
		UInt64 = 2,

		/// <summary>
		///   Represents a 64-bit unsigned integer.
		/// </summary>
		Int32 = 3,

		/// <summary>
		///   Represents a 32-bit unsigned integer.
		/// </summary>
		UInt32 = 4,

		/// <summary>
		///   Represents a 16-bit integer.
		/// </summary>
		Int16 = 5,

		/// <summary>
		///   Represents a 16-bit unsigned integer.
		/// </summary>
		UInt16 = 6,

		/// <summary>
		///   Represents a 8-bit integer.
		/// </summary>
		Int8 = 7,

		/// <summary>
		///   Represents a 8-bit unsigned integer.
		/// </summary>
		UInt8 = 8,

		/// <summary>
		///   Represents a 32-bit floating point value.
		/// </summary>
		Float = 9,

		/// <summary>
		///   Represents a 64-bit floating point value.
		/// </summary>
		Double = 10,

		/// <summary>
		///   Represents a date time value.
		/// </summary>
		DateTime = 11,

		/// <summary>
		///   Represents a boolean value type.
		/// </summary>
		Boolean = 12,
	}
}
