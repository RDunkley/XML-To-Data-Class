//********************************************************************************************************************************
// Filename:    AttributeInfo.cs
// Owner:       Richard Dunkley
// Description: Class which represents the information contained in an XML attribute.
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
using System.Collections.Generic;
using System.Xml;

namespace XMLToDataClass.Data
{
	public class AttributeInfo
	{
		#region Properties

		/// <summary>
		///   Contains information on the data portion of the attribute.
		/// </summary>
		public DataInfo Info { get; private set; }

		#endregion Properties

		#region Methods

		public AttributeInfo(string attributeName, XmlNode[] nodes, bool ignoreCase)
		{
			if (attributeName == null)
				throw new ArgumentNullException("attributeName");
			if (attributeName.Length == 0)
				throw new ArgumentException("attributeName is an empty string");
			if (nodes == null)
				throw new ArgumentNullException("nodes");
			if (nodes.Length == 0)
				throw new ArgumentException("nodes is an empty array");

			string[] possibleValues = FindAllPossibleValues(attributeName, nodes, ignoreCase);
			bool isOptional = IsOptional(attributeName, nodes, ignoreCase);
			bool canBeEmpty = CanBeEmpty(attributeName, nodes, ignoreCase);
			Info = new DataInfo(attributeName, possibleValues, isOptional, canBeEmpty, ignoreCase);
		}

		/// <summary>
		///   Finds all the possible values in the <i>nodes</i> that contain the specified attribute name.
		/// </summary>
		/// <param name="attribName"></param>
		/// <param name="nodes"></param>
		/// <param name="ignoreCase"></param>
		/// <returns></returns>
		private string[] FindAllPossibleValues(string attribName, XmlNode[] nodes, bool ignoreCase)
		{
			List<string> valueList = new List<string>();
			foreach (XmlNode node in nodes)
			{
				foreach (XmlAttribute attrib in node.Attributes)
				{
					if (string.Compare(attribName, attrib.Name, ignoreCase) == 0 && attrib.Value.Length > 0)
						valueList.Add(attrib.Value);
				}
			}
			return valueList.ToArray();
		}

		/// <summary>
		///   Determines if the attribute string can be empty or not.
		/// </summary>
		/// <param name="attribName">Name of the attribute.</param>
		/// <param name="nodes">Array of <see cref="XmlNode"/>s to check to determine if attribute is optional.</param>
		/// <returns>True if the attribute can be empty, false otherwise.</returns>
		/// <remarks>The attribute can be empty if one of the nodes contains an empty string. If all of the nodes contain a non-empty string then it can not be empty..</remarks>
		private bool CanBeEmpty(string attribName, XmlNode[] nodes, bool ignoreCase)
		{
			foreach (XmlNode node in nodes)
			{
				foreach (XmlAttribute attrib in node.Attributes)
				{
					if (string.Compare(attribName, attrib.Name, ignoreCase) == 0)
					{
						if (attrib.Value.Length == 0)
							return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		///   Determines if the attribute is optional or not.
		/// </summary>
		/// <param name="attribName">Name of the attribute.</param>
		/// <param name="nodes">Array of <see cref="XmlNode"/>s to check to determine if attribute is optional.</param>
		/// <returns>True if the attribute is optional, false otherwise.</returns>
		/// <remarks>The attribute is not optional if all the nodes contain the attribute, if one node does not contain the attribute then it is considered optional.</remarks>
		private bool IsOptional(string attribName, XmlNode[] nodes, bool ignoreCase)
		{
			foreach (XmlNode node in nodes)
			{
				bool found = false;
				foreach (XmlAttribute attrib in node.Attributes)
				{
					if (string.Compare(attribName, attrib.Name, ignoreCase) == 0)
						found = true;
				}

				if (!found)
					return true;
			}
			return false;
		}

		/// <summary>
		///   Parses the XML node attribute names from the list of <see cref="XmlNode"/>s.
		/// </summary>
		/// <param name="nodes">Array of <see cref="XmlNode"/>s to be parsed.</param>
		/// <returns>Array of all possible attributes found in the nodes.  The array is sorted by alphabetical order.</returns>
		public static string[] GetAllAttributeNames(XmlNode[] nodes, bool ignoreCase)
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

			if (!ignoreCase)
			{
				names.Sort();
				return names.ToArray();
			}

			// Remove any duplicates and return lower case values(duplicates because of case insensitivity).
			List<string> lowerCaseList = new List<string>();
			foreach (string name in names)
			{
				string lowerName = name.ToLower();
				if (!lowerCaseList.Contains(lowerName))
					lowerCaseList.Add(lowerName);
			}

			lowerCaseList.Sort();
			return lowerCaseList.ToArray();
		}

		public void Save(XmlDocument doc, XmlNode parent)
		{
			Info.Save(doc, parent);
		}

		public void Load(XmlNode parent, bool ignoreCase)
		{
			Info.Load(parent, ignoreCase);
		}

		#endregion Methods
	}
}
