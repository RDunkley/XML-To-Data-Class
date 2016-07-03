//********************************************************************************************************************************
// Filename:    CDATAInfo.cs
// Owner:       Richard Dunkley
// Description: Class which represents the information contained in a CDATA element.
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
	public class CDataInfo
	{
		#region Properties

		/// <summary>
		///   Contains information on the data portion of the attribute.
		/// </summary>
		public DataInfo Info { get; private set; }

		public bool Include { get; set; }

		#endregion Properties

		#region Methods

		public CDataInfo(XmlNode[] nodes, bool ignoreCase)
		{
			if (nodes == null)
				throw new ArgumentNullException("nodes");
			if (nodes.Length == 0)
				throw new ArgumentException("nodes is an empty array");

			string[] possibleValues = FindAllPossibleValues(nodes);
			bool isOptional = IsOptional(nodes);
			bool canBeEmpty = CanBeEmpty(nodes);
			Info = new DataInfo("CDATA", possibleValues, isOptional, canBeEmpty, ignoreCase);
			if (possibleValues.Length == 0)
				Include = false;
			else
				Include = true;
		}

		/// <summary>
		///   Finds all the possible values of CDATA in the <i>nodes</i>.
		/// </summary>
		/// <param name="nodes"></param>
		/// <returns></returns>
		private string[] FindAllPossibleValues(XmlNode[] nodes)
		{
			List<string> valueList = new List<string>();
			foreach (XmlNode node in nodes)
			{
				if(node.HasChildNodes)
				{
					foreach(XmlNode child in node.ChildNodes)
					{
						if(child.NodeType == XmlNodeType.CDATA)
						{
							XmlCDataSection cDataNode = (XmlCDataSection)child;
							if (cDataNode.Data != null && cDataNode.Data.Length > 0)
								valueList.Add(cDataNode.Data);
						}
					}
				}
			}
			return valueList.ToArray();
		}

		private bool CanBeEmpty(XmlNode[] nodes)
		{
			bool canBeEmpty = false;
			foreach (XmlNode node in nodes)
			{
				if (node.HasChildNodes)
				{
					foreach (XmlNode child in node.ChildNodes)
					{
						if (child.NodeType == XmlNodeType.CDATA)
						{
							XmlCDataSection cDataNode = (XmlCDataSection)child;
							if (cDataNode.Data.Length == 0)
								canBeEmpty = true;
						}
					}
				}
			}
			return canBeEmpty;
		}

		private bool IsOptional(XmlNode[] nodes)
		{
			foreach (XmlNode node in nodes)
			{
				if (node.HasChildNodes)
				{
					bool foundInThisChild = false;
					foreach (XmlNode child in node.ChildNodes)
					{
						if (child.NodeType == XmlNodeType.CDATA)
						{
							foundInThisChild = true;
							break;
						}
					}

					if (!foundInThisChild)
						return true;
				}
				else
				{
					return true;
				}
			}
			return false;
		}

		public static bool HasCDATA(XmlNode[] nodes)
		{
			foreach (XmlNode node in nodes)
			{
				if (node.HasChildNodes)
				{
					foreach (XmlNode child in node.ChildNodes)
					{
						if (child.NodeType == XmlNodeType.CDATA)
							return true;
					}
				}
			}
			return false;
		}

		#endregion Methods
	}
}
