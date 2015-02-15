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
using System.Collections.Generic;
using System.Text;

namespace XMLToDataClass.Data
{
	/// <summary>
	///   Contains information about all the XML nodes in an XML file that contain the same name.
	/// </summary>
	public class ElementInfo
	{
		#region Properties

		/// <summary>
		///   <see cref="AttributeInfo"/> objects containing information about all the possible attributes of the XML nodes.
		/// </summary>
		public AttributeInfo[] Attributes { get; private set; }

		/// <summary>
		///   False if all the XML nodes contain a child CDATA node, true if some do not.
		/// </summary>
		/// 
		/// <remarks>Only valid if <see cref="ElementInfo.HasCDATA"/> is true.</remarks>
		public bool CDATAIsOptional { get; set; }

		/// <summary>
		///   Gets the property name of the children array associated with this XML node.
		/// </summary>
		public string ChildArrayNameProperty { get; private set; }

		/// <summary>
		///   Gets the variable name of the children array associated with this XML node.
		/// </summary>
		public string ChildArrayNameVariable { get; private set; }

		/// <summary>
		///   Child <see cref="ElementInfo"/> objects containing information about all the possible child XML nodes contained within these nodes.
		/// </summary>
		public ElementInfo[] Children { get; set; }

		/// <summary>
		///   Gets the data class name associated with the XML node.
		/// </summary>
		public string ClassName { get; set; }

		/// <summary>
		///   True if at least one XML node has a child CDATA node, false otherwise.
		/// </summary>
		public bool HasCDATA { get; set; }

		/// <summary>
		///   True if at least one XML node has a child Text node, false otherwise.
		/// </summary>
		public bool HasText { get; set; }

		/// <summary>
		///   Gets the XML node name.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		///   <see cref="DataType"/> that represents the information in the child Text nodes.
		/// </summary>
		public DataType TextDataType { get; set; }

		/// <summary>
		///   Contains an array of the strings that represent the text values found in the XML file.
		/// </summary>
		public string[] TextValueList { get; set; }

		/// <summary>
		///   False if all the XML nodes contain a child Text node, true if some do not.
		/// </summary>
		/// 
		/// <remarks>Only valid if <see cref="ElementInfo.HasText"/> is true.</remarks>
		public bool TextIsOptional { get; set; }

		#endregion Properties

		#region Methods

		/// <summary>
		///   Instantiates a new <see cref="ElementInfo"/> object with the specified values.
		/// </summary>
		/// <param name="name">Name of the XML nodes.</param>
		/// <param name="attributes"><see cref="AttributeInfo"/> objects associated with the XML nodes.</param>
		/// <param name="children">Child <see cref="ElementInfo"/> objects.</param>
		/// <exception cref="ArgumentNullException"><i>name</i>, <i>attributes</i>, or <i>children</i> is a null reference.</exception>
		/// <exception cref="ArgumentException"><i>name</i> is an empty string or is entirely made up of whitespace.</exception>
		public ElementInfo(string name, AttributeInfo[] attributes, ElementInfo[] children)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (name.Length == 0)
				throw new ArgumentException("name is an empty string");
			name = name.Trim();
			if (name.Length == 0)
				throw new ArgumentException("name is made up entirely of whitespace.");

			if (attributes == null)
				throw new ArgumentNullException("attributes");
			if (children == null)
				throw new ArgumentNullException("children");

			Name = name;
			ClassName = GenerateClassName(name);
			ChildArrayNameVariable = GenerateChildArrayNameVariable(ClassName);
			ChildArrayNameProperty = GenerateChildArrayNameProperty(ClassName);

			// Verify that none of the attribute names match this name.
			for (int i = 0; i < attributes.Length; i++)
			{
				// If the attribute does match this then change the name.
				if (attributes[i].PropertyName == ClassName)
					attributes[i].ChangePropertyName(string.Format("{0}Attribute", attributes[i].PropertyName));
			}

			// Verify that no two attribute names have the same name.
			List<string> names = new List<string>(attributes.Length);
			for (int i = 0; i < attributes.Length; i++)
			{
				string propertyName = attributes[i].PropertyName;
				int index = 1;
				while(names.Contains(propertyName))
				{
					propertyName = string.Format("{0}{1}", propertyName, index);
				}

				if (propertyName != attributes[i].PropertyName)
					attributes[i].ChangePropertyName(propertyName);
				names.Add(propertyName);
			}

			Attributes = attributes;
			Children = children;
			HasCDATA = false;
			HasText = false;
		}

		/// <summary>
		///   Instantiates a new <see cref="ElementInfo"/> object containing a CDATA child XML node.
		/// </summary>
		/// <param name="name">Name of the XML nodes.</param>
		/// <param name="attributes"><see cref="AttributeInfo"/> objects associated with the XML nodes.</param>
		/// <param name="children">Child <see cref="ElementInfo"/> objects.</param>
		/// <param name="optionalCDATA">True if the CDATA child XML node is optional, false otherwise.</param>
		/// <exception cref="ArgumentNullException"><i>name</i>, <i>attributes</i>, or <i>children</i> is a null reference.</exception>
		/// <exception cref="ArgumentException"><i>name</i> is an empty string or is entirely made up of whitespace.</exception>
		public ElementInfo(string name, AttributeInfo[] attributes, ElementInfo[] children, bool optionalCDATA)
			: this(name, attributes, children)
		{
			HasCDATA = true;
			CDATAIsOptional = optionalCDATA;

			// Verify that none of the attribute names match the CDATA property name.
			for (int i = 0; i < attributes.Length; i++)
			{
				// If the attribute does match this then change the name.
				if (attributes[i].PropertyName == DataClassCodeGen.CDATAPropertyName)
					attributes[i].ChangePropertyName(string.Format("{0}Attribute", attributes[i].PropertyName));
			}
		}

		/// <summary>
		///   Instantiates a new <see cref="ElementInfo"/> object containing a Text child XML node.
		/// </summary>
		/// <param name="name">Name of the XML nodes.</param>
		/// <param name="attributes"><see cref="AttributeInfo"/> objects associated with the XML nodes.</param>
		/// <param name="children">Child <see cref="ElementInfo"/> objects.</param>
		/// <param name="textIsOptional">True if the Text child XML node is optional, false otherwise.</param>
		/// <param name="textDataType"><see cref="DataType"/> of the Text.</param>
		/// <param name="textValues">Values of the text found in the element. Can be empty.</param>
		/// <exception cref="ArgumentNullException"><i>name</i>, <i>attributes</i>, or <i>children</i> is a null reference.</exception>
		/// <exception cref="ArgumentException"><i>name</i> is an empty string or is entirely made up of whitespace.</exception>
		public ElementInfo(string name, AttributeInfo[] attributes, ElementInfo[] children, bool textIsOptional, DataType textDataType, string[] textValues)
			: this(name, attributes, children)
		{
			HasText = true;
			TextIsOptional = textIsOptional;
			TextDataType = textDataType;
			TextValueList = textValues;

			// Verify that none of the attribute names match the Text property name.
			for (int i = 0; i < attributes.Length; i++)
			{
				// If the attribute does match this then change the name.
				if (attributes[i].PropertyName == DataClassCodeGen.TextPropertyName)
					attributes[i].ChangePropertyName(string.Format("{0}Attribute", attributes[i].PropertyName));
			}
		}

		/// <summary>
		///   Instantiates a new <see cref="ElementInfo"/> object containing a Text and CDATA child XML nodes.
		/// </summary>
		/// <param name="name">Name of the XML nodes.</param>
		/// <param name="attributes"><see cref="AttributeInfo"/> objects associated with the XML nodes.</param>
		/// <param name="children">Child <see cref="ElementInfo"/> objects.</param>
		/// <param name="optionalCDATA">True if the CDATA child XML node is optional, false otherwise.</param>
		/// <param name="textIsOptional">True if the Text child XML node is optional, false otherwise.</param>
		/// <param name="textDataType"><see cref="DataType"/> of the Text.</param>
		/// <param name="textValues">Values of the text found in the element. Can be empty.</param>
		/// <exception cref="ArgumentNullException"><i>name</i>, <i>attributes</i>, or <i>children</i> is a null reference.</exception>
		/// <exception cref="ArgumentException"><i>name</i> is an empty string or is entirely made up of whitespace.</exception>
		public ElementInfo(string name, AttributeInfo[] attributes, ElementInfo[] children, bool optionalCDATA, bool textIsOptional, DataType textDataType, string[] textValues)
			: this(name, attributes, children, optionalCDATA)
		{
			HasText = true;
			TextIsOptional = textIsOptional;
			TextDataType = textDataType;
			TextValueList = textValues;

			// Verify that none of the attribute names match the Text property name.
			for (int i = 0; i < attributes.Length; i++)
			{
				// If the attribute does match this then change the name.
				if (attributes[i].PropertyName == DataClassCodeGen.TextPropertyName)
					attributes[i].ChangePropertyName(string.Format("{0}Attribute", attributes[i].PropertyName));
			}
		}

		/// <summary>
		///   Changes the data class name associated with this element.
		/// </summary>
		/// <param name="newName">New name of the data class.</param>
		/// <exception cref="ArgumentNullException"><i>newName</i> is a null reference.</exception>
		/// <exception cref="ArgumentException"><i>newName</i> is an empty string or is entirely made up of whitespace.</exception>
		public void ChangeClassName(string newName)
		{
			if (newName == null)
				throw new ArgumentNullException("newName");
			if (newName.Length == 0)
				throw new ArgumentException("newName is an empty string.");
			newName = newName.Trim();
			if (newName.Length == 0)
				throw new ArgumentException("newName is made up entirely of whitespace.");

			ClassName = newName;
			ChildArrayNameVariable = GenerateChildArrayNameVariable(newName);
			ChildArrayNameProperty = GenerateChildArrayNameProperty(newName);
		}

		/// <summary>
		///   Generates the property name of the child array.
		/// </summary>
		/// <param name="className">Name of the data class.</param>
		/// <returns>Name of the property.</returns>
		private string GenerateChildArrayNameProperty(string className)
		{
			return string.Format("Child{0}Array", className);
		}

		/// <summary>
		///   Generates the variable name of the child array.
		/// </summary>
		/// <param name="className">Name of the data class.</param>
		/// <returns>Name of the variable.</returns>
		private string GenerateChildArrayNameVariable(string className)
		{
			return string.Format("child{0}Array", className);
		}

		/// <summary>
		///   Generates a class name from the XML node name.
		/// </summary>
		/// <param name="name">XML node name.</param>
		/// <returns>Generated class name.</returns>
		private static string GenerateClassName(string name)
		{
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

		#endregion Methods
	}
}
