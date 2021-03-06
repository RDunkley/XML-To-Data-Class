﻿//********************************************************************************************************************************
// Filename:    SerialPortStopBitsEnumType.cs
// Owner:       Richard Dunkley
// Description: Class which represents a C# SerialPort StopBits enumeration value in an XML file.
//********************************************************************************************************************************
// Copyright © Richard Dunkley 2016
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the
// License. You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0  Unless required by applicable
// law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and
// limitations under the License.
//********************************************************************************************************************************
using System.IO.Ports;

namespace XMLToDataClass.Data.Types
{
	/// <summary>
	///   Adds support for the serial port stop bits enumeration type.
	/// </summary>
	public class SerialPortStopBitsEnumType : FixedEnumType<StopBits>
	{
		/// <summary>
		///   Instantiates a new <see cref="SerialPortStopBitsEnumType"/> object.
		/// </summary>
		/// <param name="info"><see cref="DataInfo"/> object associated with this type.</param>
		/// <param name="possibleValues">Possible values the data type will have to parse. Can be empty.</param>
		/// <remarks>This is an abstract class, the inheritor must set the Type property of the base class.</remarks>
		/// <exception cref="ArgumentNullException"><paramref name="possibleValues"/> or <paramref name="info"/> is a null reference.</exception>
		public SerialPortStopBitsEnumType(DataInfo info, string[] possibleValues) : base(info, possibleValues)
		{
			Type = DataType.SerialPortStopBits;
			Usings.Add("System.IO.Ports");
		}
	}
}
