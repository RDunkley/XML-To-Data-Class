//********************************************************************************************************************************
// Filename:    IDataType.cs
// Owner:       Richard Dunkley
// Description: Interface supported by all data types.
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
using System.Collections.Generic;
using System.Xml;

namespace XMLToDataClass.Data.Types
{
	public interface IDataType
	{
		#region Properties

		/// <summary>
		///   String of the C# representive data type.
		/// </summary>
		string DataTypeString { get; }

		/// <summary>
		///   Name of the data type which can be displayed to the user.
		/// </summary>
		string DisplayName { get; }

		/// <summary>
		///   True if the data type is nullable, false otherwise.
		/// </summary>
		bool IsNullable { get; }

		/// <summary>
		///   <see cref="DataType"/> of this data type.
		/// </summary>
		DataType Type { get; }

		/// <summary>
		///   List of the usings required by this type.
		/// </summary>
		List<string> Usings { get; }

		#endregion properties

		#region Methods

		/// <summary>
		///   Generates additional enumerations used by the import or export methods.
		/// </summary>
		/// <returns><see cref="EnumInfo"/> array represnenting additional fields needed by the import/export methods. Can be empty.</returns>
		/// <exception cref="InvalidOperationException">An attempt was made to generate the enumerations, but the current objects settings are not in a state in which this is possible.</exception>
		EnumInfo[] GenerateAdditionalEnums();

		/// <summary>
		///   Generates additional properties used by the import or export methods.
		/// </summary>
		/// <returns><see cref="PropertyInfo"/> array representing additional properties needed by the import/export methods. Can be empty.</returns>
		/// <remarks>These properties are typically used to persist state from import to export.</remarks>
		/// <exception cref="InvalidOperationException">An attempt was made to generate the properties, but the current objects settings are not in a state in which this is possible.</exception>
		PropertyInfo[] GenerateAdditionalProperties();

		/// <summary>
		///   Generates the export method code for the data type.
		/// </summary>
		/// <returns>String array representing the export method code.</returns>
		/// <exception cref="InvalidOperationException">An attempt was made to generate the code, but the current objects settings are not in a state in which this is possible.</exception>
		string[] GenerateExportMethodCode();

		/// <summary>
		///   Generates the import method code for the data type.
		/// </summary>
		/// <returns>String array representing the import method code.</returns>
		/// <exception cref="InvalidOperationException">An attempt was made to generate the code, but the current objects settings are not in a state in which this is possible.</exception>
		string[] GenerateImportMethodCode();

		/// <summary>
		///   Returns a list of the possible values that are invalid based on the current data type settings.
		/// </summary>
		/// <returns>Array of invalid values. Can be empty if no invalid values are found.</returns>
		string[] GetInvalidValues();

		/// <summary>
		///   Determines whether the data type can parse all the possible values or not.
		/// </summary>
		/// <returns>True if it can't parse all the values, false otherwise.</returns>
		bool HasInvalidValues();

		/// <summary>
		///   Attempts to parse a value to the data type, using it's current settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		bool TryParse(string value);

		/// <summary>
		///   Saves the types configuration properties to XML child elements.
		/// </summary>
		/// <param name="doc"><see cref="XmlDocument"/> object representing the XML document to be written.</param>
		/// <param name="parent">Parent <see cref="XmlNode"/> to append the child settings to.</param>
		void Save(XmlDocument doc, XmlNode parent);

		/// <summary>
		///   Loads the configuration properties from XML node.
		/// </summary>
		/// <param name="parent">Parent XML node containing the child settings elements.</param>
		void Load(XmlNode parent);

		#endregion Methods
	}
}
