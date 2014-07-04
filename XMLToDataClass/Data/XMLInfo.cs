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
using System.Xml;

namespace XMLToDataClass.Data
{
	/// <summary>
	///   Stores information extracted from an XML file.
	/// </summary>
	public class XMLInfo
	{
		#region Properties

		public ElementInfo[] RootElements { get; private set; }

		public ElementInfo[] AllElements { get; private set; }

		#endregion Properties

		#region Methods

		/// <summary>
		///   Instantiates a new <see cref="XMLInfo"/> object using the specified <see cref="XmlDocument"/>.
		/// </summary>
		/// <param name="doc"><see cref="XmlDocument"/> object to be parsed.</param>
		/// <exception cref="ArgumentNullException"><i>doc</i> is a null reference.</exception>
		public XMLInfo(XmlDocument doc)
		{
			if (doc == null)
				throw new ArgumentNullException("doc");

			// Verify that all the nodes are lower case.
			//VerifyNodeNamesAreLowerCase(doc);

			Dictionary<string, XmlNode[]> xmlNodesByName = ParseAllXmlNodesFromXmlNodeName(doc);
			Dictionary<string, ElementInfo> xmlElements = ParseElements(xmlNodesByName);
			AddChildElements(xmlNodesByName, xmlElements);

			// Determine if multiple node names are the same when converted to a class name.
			List<string> classNames = new List<string>(xmlElements.Count);
			foreach (ElementInfo el in xmlElements.Values)
			{
				string className = el.ClassName;
				int index = 1;
				while (classNames.Contains(className))
				{
					// Add an incrementing number to the end of the name until we have a unique name.
					className = string.Format("{0}{1}", el.ClassName, index);
				}

				// Change the name if needed.
				if(className != el.ClassName)
					el.ChangeClassName(className);
				classNames.Add(className);
			}

			// Create a list of element names and sort it.
			List<string> elementNames = new List<string>(xmlElements.Count);
			elementNames.AddRange(xmlElements.Keys);
			elementNames.Sort();

			// Create the all elements array.
			AllElements = new ElementInfo[elementNames.Count];
			for (int i = 0; i < elementNames.Count; i++)
				AllElements[i] = xmlElements[elementNames[i]];

			// Create the root elements array.
			string[] rootNames = ParseRootNodeNames(doc);
			RootElements = new ElementInfo[rootNames.Length];
			for (int i = 0; i < rootNames.Length; i++)
				RootElements[i] = xmlElements[rootNames[i]];
		}

		/// <summary>
		///   Adds the child elements to the element lookup table.
		/// </summary>
		/// <param name="nodesByName">Lookup table of each <see cref="XmlNode"/> by it's name.</param>
		/// <param name="elementsByName">Lookup table of each <see cref="ElementInfo"/> by it's <see cref="XmlNode"/> name.</param>
		private static void AddChildElements(Dictionary<string, XmlNode[]> nodesByName, Dictionary<string, ElementInfo> elementsByName)
		{
			foreach (string name in nodesByName.Keys)
			{
				// Get all the child node names.
				List<string> childNames = new List<string>();
				foreach (XmlNode node in nodesByName[name])
				{
					foreach (XmlNode child in node.ChildNodes)
					{
						if (child.NodeType == XmlNodeType.Element && !childNames.Contains(child.Name))
							childNames.Add(child.Name);
					}
				}

				// Sort the child by names.
				childNames.Sort();

				// Build the child element array.
				List<ElementInfo> childElements = new List<ElementInfo>(childNames.Count);
				foreach (string childName in childNames)
					childElements.Add(elementsByName[childName]);

				// Add the array to the element.
				elementsByName[name].Children = childElements.ToArray();
			}
		}

		/// <summary>
		///   Generates <see cref="ElementInfo"/> data objects for each <see cref="XmlNode"/> name.
		/// </summary>
		/// <param name="nodesByName">Lookup table of each <see cref="XmlNode"/> by it's name.</param>
		/// <returns>Lookup table of each <see cref="ElementInfo"/> by it's <see cref="XmlNode"/> name.</returns>
		private static Dictionary<string, ElementInfo> ParseElements(Dictionary<string, XmlNode[]> nodesByName)
		{
			Dictionary<string, ElementInfo> lookup = new Dictionary<string, ElementInfo>(nodesByName.Count);
			foreach (string name in nodesByName.Keys)
			{
				// Parse all the attribute names in the nodes.
				string[] attribNames = GetAllAttributeNames(nodesByName[name]);

				// Determine the attributes.
				AttributeInfo[] attribs = new AttributeInfo[attribNames.Length];
				for (int i = 0; i < attribNames.Length; i++)
				{
					AttributeInfo newAttrib = new AttributeInfo(attribNames[i]);
					newAttrib.AttributeType = DetermineAttributeType(attribNames[i], nodesByName[name]);
					newAttrib.IsOptional = IsAttributeOptional(attribNames[i], nodesByName[name]);
					newAttrib.CanBeEmpty = CanAttributeBeEmpty(attribNames[i], nodesByName[name]);
					attribs[i] = newAttrib;
				}

				// Determine if it can have CDATA.
				bool CDATAoptional;
				bool CDATAfound = ParseCDATA(nodesByName[name], out CDATAoptional);

				// Determine if it can have Text.
				bool textOptional;
				DataType type;
				bool textFound = ParseNodeText(nodesByName[name], out textOptional, out type);

				// Create an element object without the child Element objects.
				if(CDATAfound)
				{
					if(textFound)
						lookup.Add(name, new ElementInfo(name, attribs, new ElementInfo[0], CDATAoptional, textOptional, type));
					else
						lookup.Add(name, new ElementInfo(name, attribs, new ElementInfo[0], CDATAoptional));
				}
				else
				{
					if(textFound)
						lookup.Add(name, new ElementInfo(name, attribs, new ElementInfo[0], textOptional, type));
					else
						lookup.Add(name, new ElementInfo(name, attribs, new ElementInfo[0]));
				}
			}

			return lookup;
		}

		/// <summary>
		///   Parses the XML node attribute names from the list of <see cref="XmlNode"/>s.
		/// </summary>
		/// <param name="nodes">Array of <see cref="XmlNode"/>s to be parsed.</param>
		/// <returns>Array of all possible attributes found in the nodes.  The array is sorted by alphabetical order.</returns>
		private static string[] GetAllAttributeNames(XmlNode[] nodes)
		{
			List<string> names = new List<string>();
			foreach (XmlNode node in nodes)
			{
				foreach (XmlAttribute attrib in node.Attributes)
				{
					if (!names.Contains(attrib.Name))
						names.Add(attrib.Name);
				}
			}

			// Sort the list.
			names.Sort();

			return names.ToArray();
		}

		/// <summary>
		///   Add's the <see cref="XmlNode"/> names to the lookup table.
		/// </summary>
		/// <param name="node"><see cref="XmlNode"/> to pull the names from.</param>
		/// <param name="lookup">Lookup table to add the information to.</param>
		/// <remarks>This method is called recursively to cover all decendant nodes.</remarks>
		private static void AddToNodeLookupByName(XmlNode node, Dictionary<string, List<XmlNode>> lookup)
		{
			foreach (XmlNode child in node.ChildNodes)
			{
				if (child.NodeType == XmlNodeType.Element)
				{
					// Add the child to the lookup table.
					if (!lookup.ContainsKey(child.Name))
						lookup.Add(child.Name, new List<XmlNode>());
					lookup[child.Name].Add(child);

					// Add any children of the child to the node as well.
					AddToNodeLookupByName(child, lookup);
				}
			}
		}

		/// <summary>
		///   Determines if the attribute string can be empty or not.
		/// </summary>
		/// <param name="attribName">Name of the attribute.</param>
		/// <param name="nodes">Array of <see cref="XmlNode"/>s to check to determine if attribute is optional.</param>
		/// <returns>True if the attribute can be empty, false otherwise.</returns>
		/// <remarks>The attribute can be empty if one of the nodes contains an empty string. If all of the nodes contain a non-empty string then it can not be empty..</remarks>
		private static bool CanAttributeBeEmpty(string attribName, XmlNode[] nodes)
		{
			foreach (XmlNode node in nodes)
			{
				XmlAttribute attrib = node.Attributes[attribName];
				if (attrib != null)
				{
					if (attrib.Value.Length == 0)
						return true;
				}
			}
			return false;
		}

		/// <summary>
		///   Determines the attribute type from the list of <see cref="XmlNode"/>s containing the attribute.
		/// </summary>
		/// <param name="attribName">Name of the attribute.</param>
		/// <param name="nodes">Array of <see cref="XmlNode"/>s containing the specified attribute.</param>
		/// <returns><see cref="DataType"/> object denoting the type of the attribute.</returns>
		private static DataType DetermineAttributeType(string attribName, XmlNode[] nodes)
		{
			DataType? type = null;
			foreach (XmlNode node in nodes)
			{
				XmlAttribute attrib = node.Attributes[attribName];
				if (attrib != null)
				{
					if(attrib.Value != null && attrib.Value.Length > 0)
						type = DataTypeUtility.DetermineAttributeType(attrib.Value, type);
				}
			}

			if(type.HasValue)
				return type.Value;

			// If all the attributes are empty or null then assume it is a string.
			return DataType.String;
		}

		/// <summary>
		///   Gets all the <see cref="DataType"/>s used in the XML file.
		/// </summary>
		/// <returns>Array of <see cref="AttributeInfo"/>.<see cref="DataType"/>s contained in the XML file.</returns>
		public DataType[] GetAllDataTypes()
		{
			List<DataType> typeList = new List<DataType>();
			foreach (ElementInfo el in AllElements)
			{
				// Pull the type from the Text.
				if (el.HasText && !typeList.Contains(el.TextDataType))
					typeList.Add(el.TextDataType);

				// Pull all the data types from the attributes.
				foreach (AttributeInfo attrib in el.Attributes)
				{
					if (!typeList.Contains(attrib.AttributeType))
						typeList.Add(attrib.AttributeType);
				}
			}
			return typeList.ToArray();
		}

		/// <summary>
		///   Determines if the attribute is optional or not.
		/// </summary>
		/// <param name="attribName">Name of the attribute.</param>
		/// <param name="nodes">Array of <see cref="XmlNode"/>s to check to determine if attribute is optional.</param>
		/// <returns>True if the attribute is optional, false otherwise.</returns>
		/// <remarks>The attribute is not optional if all the nodes contain the attribute, if one node does not contain the attribute then it is considered optional.</remarks>
		private static bool IsAttributeOptional(string attribName, XmlNode[] nodes)
		{
			foreach (XmlNode node in nodes)
			{
				XmlAttribute attrib = node.Attributes[attribName];
				if (attrib == null)
					return true;
			}
			return false;
		}

		/// <summary>
		///   Parses the <see cref="XmlDocument"/> object to create a lookup table of the names of the nodes.
		/// </summary>
		/// <param name="doc"><see cref="XmlDocument"/> to be parsed.</param>
		/// <returns>Lookup table used to find all the <see cref="XmlNode"/>s with a specified name.</returns>
		private static Dictionary<string, XmlNode[]> ParseAllXmlNodesFromXmlNodeName(XmlDocument doc)
		{
			Dictionary<string, List<XmlNode>> lookup = new Dictionary<string, List<XmlNode>>();
			AddToNodeLookupByName(doc, lookup);

			// Sort the names.
			List<string> sortList = new List<string>(lookup.Count);
			sortList.AddRange(lookup.Keys);
			sortList.Sort();

			Dictionary<string, XmlNode[]> returnLookup = new Dictionary<string, XmlNode[]>(lookup.Count);
			foreach (string name in sortList)
				returnLookup.Add(name, lookup[name].ToArray());
			return returnLookup;
		}

		/// <summary>
		///   Parses the <i>nodes</i> and determines if a CDATA child elements exist.
		/// </summary>
		/// <param name="nodes">Array of <see cref="XmlNode"/>s to be parsed.</param>
		/// <param name="isOptional">True if the CDATA was in some, but not in others. Only valid if method returns true.</param>
		/// <returns>True if the CDATA child element was found in at least one of the <i>nodes</i>, false otherwise.</returns>
		private static bool ParseCDATA(XmlNode[] nodes, out bool isOptional)
		{
			isOptional = false;
			bool foundAtLeastOne = false;
			foreach (XmlNode node in nodes)
			{
				if (node.HasChildNodes)
				{
					bool thisChild = false;
					foreach (XmlNode child in node.ChildNodes)
					{
						if (child.NodeType == XmlNodeType.CDATA)
						{
							// Mark that we found at least one containing the CDATA.
							foundAtLeastOne = true;
							thisChild = true;
						}
					}

					if (!thisChild)
					{
						// Since we found one without the CDATA then mark it as optional.
						isOptional = true;
					}
				}
				else
				{
					// Since we found one without the CDATA then mark it as optional.
					isOptional = true;
				}
			}
			return foundAtLeastOne;
		}

		/// <summary>
		///   Parses the <i>nodes</i> and determines if a Text child elements exist.
		/// </summary>
		/// <param name="nodes">Array of <see cref="XmlNode"/>s to be parsed.</param>
		/// <param name="isOptional">True if the Text was in some, but not in others. Only valid if method returns true.</param>
		/// <param name="dataType"><see cref="DataType"/> of the text. Only valid if method returns true.</param>
		/// <returns>True if the Text child element was found in at least one of the <i>nodes</i>, false otherwise.</returns>
		private static bool ParseNodeText(XmlNode[] nodes, out bool isOptional, out DataType dataType)
		{
			isOptional = false;
			dataType = DataType.String;
			DataType? type = null;
			foreach (XmlNode node in nodes)
			{
				if (node.HasChildNodes)
				{
					XmlNode text = null;
					foreach (XmlNode child in node.ChildNodes)
					{
						if (child.NodeType == XmlNodeType.Text)
							text = child;
					}

					if (text == null)
					{
						// Since we found one without the Text then mark it as optional.
						isOptional = true;
					}
					else
					{
						// Parse the found text into a data type.
						type = DataTypeUtility.DetermineAttributeType(text.InnerText, type);
					}
				}
				else
				{
					// Since we found one without the Text then mark it as optional.
					isOptional = true;
				}
			}

			if (!type.HasValue)
				return false;

			dataType = type.Value;
			return true;
		}

		/// <summary>
		///   Parses the XML document to determine the root node names.
		/// </summary>
		/// <param name="doc"><see cref="XmlDocument"/> object to parse.</param>
		/// <returns>String array of all the names of the <see cref="XmlNode"/>s in the document.</returns>
		private string[] ParseRootNodeNames(XmlDocument doc)
		{
			List<string> topNames = new List<string>();
			foreach (XmlNode child in doc.ChildNodes)
			{
				if (child.NodeType == XmlNodeType.Element)
				{
					if (!topNames.Contains(child.Name))
						topNames.Add(child.Name);
				}
			}

			// Sort the names.
			topNames.Sort();

			return topNames.ToArray();
		}

		#endregion Methods
	}
}
