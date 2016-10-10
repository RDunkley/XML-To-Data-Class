//********************************************************************************************************************************
// Filename:    BaseType.cs
// Owner:       Richard Dunkley
// Description: Contains the base information in every supported type. All types supported in an XML file are inherited from this
//              class.
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
using System.Xml;

namespace XMLToDataClass.Data.Types
{
	/// <summary>
	///   Base type for all data types the program can use to interpret data in the XML.
	/// </summary>
	public abstract class BaseType : IDataType
	{
		#region Fields

		/// <summary>
		///   Contains all the possible values for the data component in the currently loaded XML file.
		/// </summary>
		protected string[] mPossibleValues;

		/// <summary>
		///   True if the case of the values should be ignored, false otherwise. May not have any bearing on certain types.
		/// </summary>
		protected bool mIgnoreCase;

		/// <summary>
		///   <see cref="DataInfo"/> object assocaited with this type.
		/// </summary>
		protected DataInfo mInfo;

		#endregion Fields

		#region Properties

		/// <summary>
		///   Name of the data type which can be displayed to the user.
		/// </summary>
		public string DisplayName { get; protected set; }

		/// <summary>
		///   True if the data type is nullable, false otherwise.
		/// </summary>
		public bool IsNullable { get; protected set; }

		/// <summary>
		///   True if the data type supports 'Length' property, false otherwise.
		/// </summary>
		public bool IsArray { get; protected set; }

		/// <summary>
		///   <see cref="DataType"/> of this data type.
		/// </summary>
		public DataType Type { get; protected set;}

		/// <summary>
		///   List of the usings required by this type.
		/// </summary>
		public List<string> Usings { get; protected set; }

		#endregion Properties

		#region Methods

		/// <summary>
		///   Instantiates a new <see cref="BaseType"/> object given the representative possible values.
		/// </summary>
		/// <param name="info"><see cref="DataInfo"/> object associated with this type.</param>
		/// <param name="possibleValues">Possible values the data type will have to parse. Can be empty.</param>
		/// <param name="ignoreCase">True if the case of the values should be ignored, false otherwise. May not have any bearing on certain types.</param>
		/// <exception cref="ArgumentNullException"><paramref name="possibleValues"/> or <paramref name="info"/> is a null reference.</exception>
		public BaseType(DataInfo info, string[] possibleValues, bool ignoreCase)
		{
			if (info == null)
				throw new ArgumentNullException("info");
			if (possibleValues == null)
				throw new ArgumentNullException("possibleValues");

			mInfo = info;
			mPossibleValues = possibleValues;
			mIgnoreCase = ignoreCase;
			Usings = new List<string>();
			IsNullable = true;
			IsArray = false;
		}

		/// <summary>
		///   String of the C# representive data type.
		/// </summary>
		public abstract string GetDataTypeString();

		/// <summary>
		///   Abstract method to generate additional enumerations used by the import or export methods.
		/// </summary>
		/// <returns><see cref="EnumInfo"/> array represnenting additional fields needed by the import/export methods. Can be empty.</returns>
		/// <exception cref="InvalidOperationException">An attempt was made to generate the enumerations, but the current objects settings are not in a state in which this is possible.</exception>
		public abstract EnumInfo[] GenerateAdditionalEnums();

		/// <summary>
		///   Abstract method to generate additional properties used by the import or export methods.
		/// </summary>
		/// <returns><see cref="PropertyInfo"/> array representing additional properties needed by the import/export methods. Can be empty.</returns>
		/// <remarks>These properties are typically used to persist state from import to export.</remarks>
		/// <exception cref="InvalidOperationException">An attempt was made to generate the properties, but the current objects settings are not in a state in which this is possible.</exception>
		public abstract PropertyInfo[] GenerateAdditionalProperties();

		/// <summary>
		///   Abstract method to generate the export method code for the data type.
		/// </summary>
		/// <returns>String array representing the export method code.</returns>
		/// <exception cref="InvalidOperationException">An attempt was made to generate the code, but the current objects settings are not in a state in which this is possible.</exception>
		public abstract string[] GenerateExportMethodCode();

		/// <summary>
		///   Abstract method to generate the import method code for the data type.
		/// </summary>
		/// <returns>String array representing the import method code.</returns>
		/// <exception cref="InvalidOperationException">An attempt was made to generate the code, but the current objects settings are not in a state in which this is possible.</exception>
		public abstract string[] GenerateImportMethodCode();

		/// <summary>
		///   Returns a list of the possible values that are invalid based on the current data type settings.
		/// </summary>
		/// <returns>Array of invalid values. Can be empty if no invalid values are found.</returns>
		public string[] GetInvalidValues()
		{
			List<string> invalidValues = new List<string>();
			foreach (string value in mPossibleValues)
			{
				if (!TryParse(value))
				{
					if (!invalidValues.Contains(value))
						invalidValues.Add(value);
				}
			}

			return invalidValues.ToArray();
		}

		/// <summary>
		///   Determines whether the data type can parse all the possible values or not.
		/// </summary>
		/// <returns>True if it can't parse all the values, false otherwise.</returns>
		public bool HasInvalidValues()
		{
			foreach (string value in mPossibleValues)
			{
				if (!TryParse(value))
					return true;
			}
			return false;
		}

		/// <summary>
		///   Saves the types configuration properties to XML child elements.
		/// </summary>
		/// <param name="doc"><see cref="XmlDocument"/> object representing the XML document to be written.</param>
		/// <param name="parent">Parent <see cref="XmlNode"/> to append the child settings to.</param>
		public abstract void Save(XmlDocument doc, XmlNode parent);

		/// <summary>
		///   Loads the configuration properties from XML node.
		/// </summary>
		/// <param name="parent">Parent XML node containing the child settings elements.</param>
		public abstract void Load(XmlNode parent);

		/// <summary>
		///   Abstract method to try and parse a value to the data type, using it's current settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		public abstract bool TryParse(string value);

		#endregion Methods
	}
}
