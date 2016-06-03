using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLToDataClass.Data
{
	public class TextInfo
	{
		#region Properties

		/// <summary>
		///   Contains information on the data portion of the attribute.
		/// </summary>
		public DataInfo Info { get; private set; }

		public bool Include { get; set; }

		#endregion Properties

		#region Methods

		public TextInfo(XmlNode[] nodes, bool ignoreCase)
		{
			if (nodes == null)
				throw new ArgumentNullException("nodes");
			if (nodes.Length == 0)
				throw new ArgumentException("nodes is an empty array");

			string[] possibleValues = FindAllPossibleValues(nodes);
			bool isOptional = IsOptional(nodes);
			bool canBeEmpty = CanBeEmpty(nodes);
			Info = new DataInfo("Text", possibleValues, isOptional, canBeEmpty, ignoreCase);
			if (possibleValues.Length == 0)
				Include = false;
			else
				Include = true;
		}

		/// <summary>
		///   Finds all the possible values of Text in the <i>nodes</i>.
		/// </summary>
		/// <param name="nodes"></param>
		/// <returns></returns>
		private string[] FindAllPossibleValues(XmlNode[] nodes)
		{
			List<string> valueList = new List<string>();
			foreach (XmlNode node in nodes)
			{
				if (node.HasChildNodes)
				{
					foreach (XmlNode child in node.ChildNodes)
					{
						if (child.NodeType == XmlNodeType.CDATA)
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
						if (child.NodeType == XmlNodeType.Text)
						{
							if (child.InnerText.Length == 0)
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
						if (child.NodeType == XmlNodeType.Text)
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

		public static bool HasText(XmlNode[] nodes)
		{
			foreach (XmlNode node in nodes)
			{
				if (node.HasChildNodes)
				{
					foreach (XmlNode child in node.ChildNodes)
					{
						if (child.NodeType == XmlNodeType.Text)
							return true;
					}
				}
			}
			return false;
		}

		#endregion Methods
	}
}
