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
using System.Text;

namespace XMLToDataClass.Data
{
	/// <summary>
	///   Contains all the information for <see cref="XmlNode"/> attributes.
	/// </summary>
	public class AttributeInfo
	{
		#region Properties

		/// <summary>
		///   DataType of the attribute.
		/// </summary>
		public DataType AttributeType { get; set; }

		/// <summary>
		///   True if the attribute can be an empty string, false otherwise.
		/// </summary>
		public bool CanBeEmpty { get; set; }

		/// <summary>
		///   Contains an array of the enumerated type names for each of the values in the <see cref="EnumValueList"/>. Only valid if <see cref="AttributeType"/> is <see cref="DataType.Enum"/>.
		/// </summary>
		public string[] EnumNameList { get; set; }

		/// <summary>
		///   Contains the name of the enumerated type.  Only valid if <see cref="AttributeType"/> is <see cref="DataType.Enum"/>.
		/// </summary>
		public string EnumTypeName { get; set; }

		/// <summary>
		///   Contains an array of the strings that represent the enumerated types in the XML file. Only valid if <see cref="AttributeType"/> is <see cref="DataType.Enum"/>.
		/// </summary>
		public string[] EnumValueList { get; set; }

		/// <summary>
		///   True if the attribute is optional, false otherwise.
		/// </summary>
		public bool IsOptional { get; set; }

		/// <summary>
		///   Name of the attribute.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		///   Name of the attribute property.
		/// </summary>
		public string PropertyName { get; set; }

		/// <summary>
		///   Name of the property that defines whether the property is valid.
		/// </summary>
		public string PropertyValidName { get; private set; }

		#endregion Properties

		#region Methods

		/// <summary>
		///   Instantiates a new <see cref="AttributeInfo"/> object.
		/// </summary>
		/// <param name="name">Name of the attribute.</param>
		public AttributeInfo(string name)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (name.Length == 0)
				throw new ArgumentException("name is an empty string");

			Name = name;
			PropertyName = GeneratePropertyName(name);
			PropertyValidName = GetValidPropertyCapitalCase(PropertyName);
		}

		/// <summary>
		///   Changes the name of the property that will be generated to represent this attribute.
		/// </summary>
		/// <param name="newName">The new property name.</param>
		/// <exception cref="ArgumentNullException"><i>newName</i> is a null reference.</exception>
		/// <exception cref="ArgumentException"><i>newName</i> is an empty string.</exception>
		public void ChangePropertyName(string newName)
		{
			if (newName == null)
				throw new ArgumentNullException("newName");
			if (newName.Length == 0)
				throw new ArgumentException("newName is an empty string");

			PropertyName = newName;
			PropertyValidName = GetValidPropertyCapitalCase(PropertyName);
		}

		/// <summary>
		///   Generates the property name from the XML node name.
		/// </summary>
		/// <param name="name">Name of the XML node.</param>
		/// <returns>Generated property name.</returns>
		/// <exception cref="ArgumentNullException"><i>name</i> is a null reference.</exception>
		/// <exception cref="ArgumentException"><i>name</i> is an empty string.</exception>
		public static string GeneratePropertyName(string name)
		{
			if (name == null)
				throw new ArgumentNullException("name is a null reference");
			if (name.Length == 0)
				throw new ArgumentException("name is an empty string");

			StringBuilder builder = new StringBuilder();
			bool capNext = true;
			for (int i = 0; i < name.Length; i++)
			{
				if (name[i] == '_')
				{
					capNext = true;
				}
				else
				{
					if (capNext)
					{
						builder.Append(char.ToUpper(name[i]));
						capNext = false;
					}
					else
						builder.Append(name[i]);
				}
			}
			return builder.ToString();
		}

		/// <summary>
		///   Gets the valid property name in capital case of the associated attribute name.
		/// </summary>
		/// <param name="name">XML name of the attribute.</param>
		/// <returns>Valid property name in capital case.</returns>
		private static string GetValidPropertyCapitalCase(string name)
		{
			return string.Format("Is{0}Valid", name);
		}

		#endregion Methods
	}
}
