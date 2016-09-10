//********************************************************************************************************************************
// Filename:    ElementInfo.cs
// Owner:       Richard Dunkley
// Description: Class which represents the an XML element.
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
using System.Text;
using System.Xml;

namespace XMLToDataClass.Data
{
	/// <summary>
	///   Contains information about all the XML nodes in an XML file that contain the same name.
	/// </summary>
	public class ElementInfo
	{
		#region Properties

		/// <summary>
		///   <see cref="AttributeInfo"/> objects containing information about all the possible attributes of the XML element.
		/// </summary>
		public AttributeInfo[] Attributes { get; private set; }

		/// <summary>
		///   Contains information about the CDATA node of the element. Null if none of the elements contain a CDATA node.
		/// </summary>
		public CDataInfo CDATA { get; set; }

		/// <summary>
		///   Contains information about the Text node of the element. Null if none of the elements contain a Text node.
		/// </summary>
		public TextInfo Text { get; set; }

		/// <summary>
		///   Child <see cref="ElementInfo"/> objects containing information about all the possible child XML nodes contained within these nodes.
		/// </summary>
		public ElementInfo[] Children { get; set; }

		/// <summary>
		///   Gets the data class name associated with the XML node.
		/// </summary>
		public string ClassName { get; set; }

		/// <summary>
		///   Gets the XML node name.
		/// </summary>
		public string Name { get; private set; }

		#endregion Properties

		#region Methods

		public ElementInfo(XmlNode[] nodes, bool ignoreCase)
		{
			if (nodes == null)
				throw new ArgumentNullException("nodes");
			if (nodes.Length == 0)
				throw new ArgumentException("nodes is an empty array");

			// Verify all the nodes are elements and have the same element name.
			string elementName = null;
			foreach(XmlNode node in nodes)
			{
				if (node.NodeType != XmlNodeType.Element)
					throw new ArgumentException("nodes contained an XmlNode that was not an element node type.");
				if (elementName == null)
				{
					elementName = node.Name;
				}
				else
				{
					if (string.Compare(elementName, node.Name, ignoreCase) != 0)
						throw new ArgumentException("One or more of the nodes provided do not contain the same element name");
				}
			}

			Name = elementName;
			if (ignoreCase)
				Name = elementName.ToLower();
			ClassName = GenerateClassName(Name);

			// Parse all the attribute names in the nodes.
			string[] attribNames = AttributeInfo.GetAllAttributeNames(nodes, ignoreCase);

			// Determine the attributes.
			Attributes = new AttributeInfo[attribNames.Length];
			for (int i = 0; i < attribNames.Length; i++)
				Attributes[i] = new AttributeInfo(attribNames[i], nodes, ignoreCase);

			// Verify that none of the attribute property names match the element class name.
			foreach(AttributeInfo attrib in Attributes)
			{
				// If the attribute does match then change the name.
				if(string.Compare(attrib.Info.PropertyName, ClassName, false) == 0)
					attrib.Info.PropertyName = string.Format("{0}Attribute", attrib.Info.PropertyName);
			}

			// Verify that no two attribute names have the same property name.
			List<string> propertyNames = new List<string>(Attributes.Length);
			foreach(AttributeInfo attrib in Attributes)
			{
				string propertyName = attrib.Info.PropertyName;
				int index = 1;
				string newName = propertyName;
				while (propertyNames.Contains(newName))
				{
					newName = string.Format("{0}{1}", propertyName, index);
					index++;
				}

				if (newName != attrib.Info.PropertyName)
					attrib.Info.PropertyName = newName;
				propertyNames.Add(newName);
			}

			// CDATA
			CDATA = new CDataInfo(nodes, ignoreCase);

			// Verify that none of the attribute names match the CDATA property.
			if (CDATA.Include)
			{
				foreach (AttributeInfo attrib in Attributes)
				{
					// If the attribute does match this then change the name.
					if (string.Compare(attrib.Info.PropertyName, CDATA.Info.PropertyName, false) == 0)
						attrib.Info.PropertyName = string.Format("{0}Attribute", attrib.Info.PropertyName);
				}
			}

			// Text
			Text = new TextInfo(nodes, ignoreCase);

			if(Text.Include)
			{
				// Verify that none of the attribute names match the text property.
				foreach(AttributeInfo attrib in Attributes)
				{
					// If the attribute does match this then change the name.
					if (string.Compare(attrib.Info.PropertyName, Text.Info.PropertyName, false) == 0)
						attrib.Info.PropertyName = string.Format("{0}Attribute", attrib.Info.PropertyName);
				}
			}
		}

		/// <summary>
		///   Generates the property name of the child array.
		/// </summary>
		/// <param name="className">Name of the data class.</param>
		/// <returns>Name of the property.</returns>
		private string GenerateChildArrayNameProperty(string className)
		{
			return string.Format("{0}s", className);
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

		public ClassInfo GenerateDataClass(bool preserveHierarchy, bool ignoreCase)
		{
			ClassInfo info = new ClassInfo("public partial", ClassName, null, string.Format("In memory representation of the XML element \"{0}\".", Name));
			info.AddUsing("System.Xml");
			info.AddUsing("System.IO");

			List<DataInfo> dataList = new List<DataInfo>();

			// Add text property.
			if (Text.Include)
				dataList.Add(Text.Info);

			// Add CDATA property.
			if (CDATA.Include)
				dataList.Add(CDATA.Info);

			// Add properties to represent the attributes.
			foreach (AttributeInfo attrib in Attributes)
				dataList.Add(attrib.Info);

			foreach(DataInfo data in dataList)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append(string.Format("Gets or sets the value of the child {0} element.", data.Name));
				if(data.SelectedDataType == DataType.String)
				{
					if (data.IsOptional)
						sb.Append(" Can be null.");
					if (data.CanBeEmpty)
						sb.Append(" Can be empty.");
				}
				else
				{
					if (data.IsOptional || data.CanBeEmpty)
						sb.Append(" Can be null.");
				}
				
				info.Properties.Add(new PropertyInfo("public", data.GetDataTypeString(), data.PropertyName, sb.ToString()));

				// Add any additional properties.
				info.Properties.AddRange(data.SelectedDataTypeObject.GenerateAdditionalProperties());

				// Add any additional enums.
				info.Enums.AddRange(data.SelectedDataTypeObject.GenerateAdditionalEnums());

				// Add any custom usings.
				info.AddUsings(data.GetSelectedUsings());
			}

			// Add properties to represent the child nodes.
			foreach (ElementInfo child in Children)
			{
				string summary = "Gets or sets the child XML elements.";
				info.Properties.Add(new PropertyInfo("public", string.Format("{0}[]", child.ClassName), GenerateChildArrayNameProperty(child.ClassName), summary, null, null, "private"));
			}

			// Create the constructors.
			info.Constructors.Add(GenerateDataClassConstructor());
			info.Constructors.Add(GenerateDataClassXmlNodeConstructor(ignoreCase));

			// Add additional methods.
			info.Methods.AddRange(GenerateMethods(dataList.ToArray()));
			info.Methods.Add(GenerateCreateElementMethod());

			if(preserveHierarchy)
			{
				// Add additional sub-classes.
				foreach (ElementInfo child in Children)
				{
					ClassInfo childClass = child.GenerateDataClass(preserveHierarchy, ignoreCase);
					info.AddChildClass(childClass);
				}
			}

			return info;
		}

		private MethodInfo GenerateCreateElementMethod()
		{
			MethodInfo method = new MethodInfo
			(
				"public",
				"XmlElement",
				"CreateElement",
				"Creates an XML element for this object using the provided <see cref=\"XmlDocument\"> object.",
				null,
				"<see cref=\"XmlElement\"> object containing this classes data."
			);

			method.Parameters.Add(new ParameterInfo
			(
				"XmlDocument",
				"doc",
				"<see cref=\"XmlDocument\"> object to generate the element from.",
				false,
				null
			));

			method.CodeLines.Add(string.Format("XmlElement returnElement = doc.CreateElement(\"{0}\");", Name));
			if(Text.Include || CDATA.Include || Attributes.Length > 0)
			{
				method.CodeLines.Add(string.Empty);
				method.CodeLines.Add("string valueString;");
			}

			if (Text.Include)
			{
				method.CodeLines.Add(string.Empty);
				method.CodeLines.Add(string.Format("valueString = {0}();", GetExportMethodName(Text.Info.PropertyName)));
				if(Text.Info.IsOptional)
				{
					method.CodeLines.Add("if(valueString != null)");
					method.CodeLines.Add("{");
					method.CodeLines.Add("	XmlText textNode = doc.CreateTextNode(valueString);");
					method.CodeLines.Add("	returnElement.AppendChild(textNode);");
					method.CodeLines.Add("}");
				}
				else
				{
					method.CodeLines.Add("XmlText textNode = doc.CreateTextNode(valueString);");
					method.CodeLines.Add("returnElement.AppendChild(textNode);");
				}
			}

			if (CDATA.Include)
			{
				method.CodeLines.Add(string.Empty);
				method.CodeLines.Add(string.Format("valueString = {0}();", GetExportMethodName(CDATA.Info.PropertyName)));
				if (CDATA.Info.IsOptional)
				{
					method.CodeLines.Add("if(valueString != null)");
					method.CodeLines.Add("{");
					method.CodeLines.Add("	XmlCDataSection cDATASection = doc.CreateCDataSection(valueString);");
					method.CodeLines.Add("	returnElement.AppendChild(cDATASection);");
					method.CodeLines.Add("}");
				}
				else
				{
					method.CodeLines.Add("XmlCDataSection cDATASection = doc.CreateCDataSection(valueString);");
					method.CodeLines.Add("returnElement.AppendChild(cDATASection);");
				}
			}

			foreach (AttributeInfo attrib in Attributes)
			{
				method.CodeLines.Add(string.Empty);
				method.CodeLines.Add(string.Format("// {0}", attrib.Info.Name));
				method.CodeLines.Add(string.Format("valueString = {0}();", GetExportMethodName(attrib.Info.PropertyName)));
				string addSpace = "";
				if (attrib.Info.IsOptional)
				{
					method.CodeLines.Add("if(valueString != null)");
					addSpace = "	";
				}
				method.CodeLines.Add(string.Format("{0}returnElement.SetAttribute(\"{1}\", valueString);", addSpace, attrib.Info.Name));
			}

			foreach(ElementInfo element in Children)
			{
				method.CodeLines.Add(string.Empty);
				method.CodeLines.Add(string.Format("foreach({0} child in {1})", element.ClassName, GenerateChildArrayNameProperty(element.ClassName)));
				method.CodeLines.Add("	returnElement.AppendChild(child.CreateElement(doc));");
			}
			method.CodeLines.Add("return returnElement;");

			return method;
		}

		private MethodInfo[] GenerateMethods(DataInfo[] dataArray)
		{
			List<MethodInfo> methodList = new List<MethodInfo>(dataArray.Length);
			foreach(DataInfo data in dataArray)
				methodList.Add(GenerateImportMethod(data));
			foreach (DataInfo data in dataArray)
				methodList.Add(GenerateExportMethod(data));
			return methodList.ToArray();
		}

		private string GetImportMethodName(string propertyName)
		{
			return string.Format("Set{0}FromString", propertyName);
		}

		private MethodInfo GenerateImportMethod(DataInfo info)
		{
			string methodName = GetImportMethodName(info.PropertyName);
			MethodInfo method = new MethodInfo
			(
				"public",
				"void",
				methodName,
				string.Format("Parses a string value and stores the data in {0}.", info.PropertyName)
			);

			method.Parameters.Add(new ParameterInfo
			(
				"string",
				"value",
				"String representation of the value.",
				null,
				null
			));

			if (!info.IsOptional && !info.CanBeEmpty)
			{
				method.Exceptions.Add(new ExceptionInfo
				(
					"InvalidDataException",
					"The string value is a null reference or an empty string."
				));
			}
			else
			{
				if (!info.IsOptional)
				{
					method.Exceptions.Add(new ExceptionInfo
					(
						"InvalidDataException",
						"The string value is a null reference."
					));
				}

				if (!info.CanBeEmpty)
				{
					method.Exceptions.Add(new ExceptionInfo
					(
						"InvalidDataException",
						"The string value is an empty string."
					));
				}
			}

			method.Exceptions.Add(new ExceptionInfo
			(
				"InvalidDataException",
				"The string value could not be parsed."
			));

			method.CodeLines.AddRange(info.SelectedDataTypeObject.GenerateImportMethodCode());
			return method;
		}

		private string GetExportMethodName(string propertyName)
		{
			return string.Format("Get{0}String", propertyName);
		}

		private MethodInfo GenerateExportMethod(DataInfo info)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("String representing the value.");
			if (info.IsOptional)
				sb.Append(" Can be null.");
			if (info.CanBeEmpty)
				sb.Append(" Can be empty.");

			string methodName = GetExportMethodName(info.PropertyName);
			MethodInfo method = new MethodInfo
			(
				"public",
				"string",
				methodName,
				string.Format("Gets a string representation of {0}.", info.PropertyName),
				null,
				sb.ToString()
			);

			method.CodeLines.AddRange(info.SelectedDataTypeObject.GenerateExportMethodCode());
			return method;
		}

		private ConstructorInfo GenerateDataClassConstructor()
		{
			string summary = string.Format("Instantiates a new {0} object using the provided information.", ClassName);
			ConstructorInfo cInfo = new ConstructorInfo("public", ClassName, summary);

			// Add parameters for each property.
			if (Text.Include)
			{
				string name = StringUtility.GetLowerCamelCase(Text.Info.PropertyName, true);
				bool? canBeNull;
				bool? canBeEmpty;
				DetermineDataConstructorInputChecks(Text.Info, out canBeNull, out canBeEmpty);
				cInfo.Parameters.Add(new ParameterInfo
				(
					Text.Info.GetDataTypeString(),
					name,
					"Child Text element value.",
					canBeNull,
					canBeEmpty
				));
				cInfo.CodeLines.Add(string.Format("{0} = {1};", Text.Info.PropertyName, name));
			}

			if (CDATA.Include)
			{
				string name = StringUtility.GetLowerCamelCase(CDATA.Info.PropertyName, true);
				bool? canBeNull;
				bool? canBeEmpty;
				DetermineDataConstructorInputChecks(CDATA.Info, out canBeNull, out canBeEmpty);
				cInfo.Parameters.Add(new ParameterInfo
				(
					CDATA.Info.GetDataTypeString(),
					name,
					"Child CDATA element value.",
					canBeNull,
					canBeEmpty
				));
				cInfo.CodeLines.Add(string.Format("{0} = {1};", CDATA.Info.PropertyName, name));
			}

			// Add parameters for each attribute.
			foreach (AttributeInfo attrib in Attributes)
			{
				string name = StringUtility.GetLowerCamelCase(attrib.Info.PropertyName, true);
				bool? canBeNull;
				bool? canBeEmpty;
				DetermineDataConstructorInputChecks(attrib.Info, out canBeNull, out canBeEmpty);
				cInfo.Parameters.Add(new ParameterInfo
				(
					attrib.Info.GetDataTypeString(),
					name,
					string.Format("\'{0}\' {1} attribute contained in the XML element.", attrib.Info.Name, attrib.Info.SelectedDataTypeObject.DisplayName),
					canBeNull,
					canBeEmpty
				));
				cInfo.CodeLines.Add(string.Format("{0} = {1};", attrib.Info.PropertyName, name));
			}

			// Add parameters for each child collection.
			foreach (ElementInfo eInfo in Children)
			{
				string typeString = string.Format("{0}[]", eInfo.ClassName);
				string propertyName = GenerateChildArrayNameProperty(eInfo.ClassName);
				string variableName = StringUtility.GetLowerCamelCase(propertyName, true);
				cInfo.Parameters.Add(new ParameterInfo
				(
					typeString,
					variableName,
					string.Format("Array of {0} elements which are child elements of this node.", eInfo.Name),
					false,
					true
				));
				cInfo.CodeLines.Add(string.Format("{0} = {1};", propertyName, variableName));
			}

			return cInfo;
		}

		private void DetermineDataConstructorInputChecks(DataInfo info, out bool? canBeNull, out bool? canBeEmpty)
		{
			// Here is a truth table for the following logic:
			//
			// IsOptional	CanBeEmpty	IsNullable	CanBeNull	CanBeEmpty
			// 1			1			0			1			1
			// 0			1			0			0			1
			// 1			0			0			1			0
			// 0			0			0			0			0
			// 0			1			1			NULL		NULL
			// 0			0			1			NULL		NULL
			// 1			0			1			1			NULL
			// 1			1			1			1			NULL
			canBeNull = null;
			canBeEmpty = null;
			if (info.SelectedDataTypeObject.IsNullable)
			{
				if (info.IsOptional)
					canBeNull = true;
			}
			else
			{
				canBeNull = info.IsOptional;
				canBeEmpty = info.CanBeEmpty;
			}
		}

		private ConstructorInfo GenerateDataClassXmlNodeConstructor(bool ignoreCase)
		{
			string summary = string.Format("Instantiates a new {0} object from an <see=cref=\"XmlNode\"/> object.", ClassName);
			ConstructorInfo cInfo = new ConstructorInfo("public", ClassName, summary);

			cInfo.Parameters.Add(new ParameterInfo("XmlNode", "node", "<see cref=\"XmlNode\"/> containing the data to extract.", false));
			cInfo.Exceptions.Add(new ExceptionInfo("ArgumentException", string.Format("<i>node</i> does not correspond to a {0} node or is not an 'Element' type node.", Name)));
			cInfo.Exceptions.Add(new ExceptionInfo("InvalidDataException", "An error occurred while reading the data into the node, or one of it's child nodes."));

			cInfo.CodeLines.Add("if (node.NodeType != XmlNodeType.Element)");
			cInfo.CodeLines.Add("	throw new ArgumentException(\"node is not of type 'Element'.\");");
			cInfo.CodeLines.Add(string.Format("if(string.Compare(node.Name, \"{0}\", {1}) != 0)", Name, ignoreCase.ToString().ToLower()));
			cInfo.CodeLines.Add(string.Format("	throw new ArgumentException(\"node does not correspond to a {0} node.\");", Name));

			string textName = StringUtility.GetLowerCamelCase(Text.Info.PropertyName, true);
			string dataName = StringUtility.GetLowerCamelCase(CDATA.Info.PropertyName, true);

			if (Attributes.Length > 0)
			{
				cInfo.CodeLines.Add(string.Empty);
				cInfo.CodeLines.Add("XmlAttribute attrib;");

				foreach (AttributeInfo attrib in Attributes)
				{
					cInfo.CodeLines.Add(string.Empty);
					cInfo.CodeLines.Add(string.Format("// {0}", attrib.Info.Name));
					cInfo.CodeLines.Add(string.Format("attrib = node.Attributes[\"{0}\"];", attrib.Info.Name));
					string space = string.Empty;
					if (!attrib.Info.IsOptional)
					{
						cInfo.CodeLines.Add("if(attrib == null)");
						cInfo.CodeLines.Add(string.Format("	throw new InvalidDataException(\"An XML string Attribute ({0}) is not optional, but was not found in the XML element ({1}).\");", attrib.Info.Name, Name));
					}
					else
					{
						cInfo.CodeLines.Add("if(attrib == null)");
						cInfo.CodeLines.Add(string.Format("	{0} = null;", attrib.Info.PropertyName));
						cInfo.CodeLines.Add("else");
						space = "	";
						
					}
					cInfo.CodeLines.Add(string.Format("{0}{1}(attrib.Value);", space, GetImportMethodName(attrib.Info.PropertyName)));
				}
			}

			// Parse the child nodes if needed.
			if (Text.Include || CDATA.Include || Children.Length > 0)
			{
				cInfo.CodeLines.Add(string.Empty);
				cInfo.CodeLines.Add("// Read the child objects.");
				foreach (ElementInfo child in Children)
					cInfo.CodeLines.Add(string.Format("List<{0}> {1}List = new List<{0}>();", child.ClassName, StringUtility.GetLowerCamelCase(GenerateChildArrayNameProperty(child.ClassName), true)));

				if (Text.Include)
					cInfo.CodeLines.Add(string.Format("bool {0}Found = false;", textName));

				if (CDATA.Include)
					cInfo.CodeLines.Add(string.Format("bool {0}Found = false;", dataName));

				cInfo.CodeLines.Add("foreach(XmlNode child in node.ChildNodes)");
				cInfo.CodeLines.Add("{");
				foreach (ElementInfo child in Children)
				{
					cInfo.CodeLines.Add(string.Format("	if(child.NodeType == XmlNodeType.Element && child.Name == \"{0}\")", child.Name));
					cInfo.CodeLines.Add(string.Format("		{0}List.Add(new {1}(child));", StringUtility.GetLowerCamelCase(GenerateChildArrayNameProperty(child.ClassName), true), child.ClassName));
				}

				if (Text.Include)
				{
					cInfo.CodeLines.Add("	if(child.NodeType == XmlNodeType.Text)");
					cInfo.CodeLines.Add("	{");
					cInfo.CodeLines.Add(string.Format("		{0}(child.Value);", GetImportMethodName(Text.Info.PropertyName)));
					cInfo.CodeLines.Add(string.Format("		{0}Found = true;", textName));
					cInfo.CodeLines.Add("	}");
				}

				if (CDATA.Include)
				{
					cInfo.CodeLines.Add("	if(child.NodeType == XmlNodeType.CDATA)");
					cInfo.CodeLines.Add("	{");
					cInfo.CodeLines.Add(string.Format("		{0}(child.Value);", GetImportMethodName(CDATA.Info.PropertyName)));
					cInfo.CodeLines.Add(string.Format("		{0}Found = true;", dataName));
					cInfo.CodeLines.Add("	}");
				}

				cInfo.CodeLines.Add("}");
				foreach (ElementInfo child in Children)
				{
					string propertyName = GenerateChildArrayNameProperty(child.ClassName);
					cInfo.CodeLines.Add(string.Format("{0} = {1}List.ToArray();", propertyName, StringUtility.GetLowerCamelCase(propertyName, true)));
				}
				if (Text.Include)
				{
					cInfo.CodeLines.Add(string.Empty);
					cInfo.CodeLines.Add(string.Format("if(!{0}Found)", textName));
					if (Text.Info.IsOptional)
						cInfo.CodeLines.Add(string.Format("	{0} = null;", Text.Info.PropertyName));
					else
						cInfo.CodeLines.Add(string.Format("	throw new InvalidDataException(\"An XML child Text node is not optional, but was not found in the XML element ({0}).\");", Name));
				}
				if (CDATA.Include)
				{
					cInfo.CodeLines.Add(string.Empty);
					cInfo.CodeLines.Add(string.Format("if(!{0}Found)", dataName));
					if (CDATA.Info.IsOptional)
						cInfo.CodeLines.Add(string.Format("	{0} = null;", CDATA.Info.PropertyName));
					else
						cInfo.CodeLines.Add(string.Format("	throw new InvalidDataException(\"An XML child CDATA node is not optional, but was not found in the XML element ({0}).\");", Name));
				}
				cInfo.CodeLines.Add(string.Empty);
			}

			return cInfo;
		}

		public void Save(XmlDocument doc, XmlNode parent)
		{
			XmlElement element = doc.CreateElement(Name);
			XmlAttribute attrib = element.Attributes.Append(doc.CreateAttribute("ClassName"));
			attrib.Value = ClassName;

			// Save the attributes.
			if (Attributes != null && Attributes.Length > 0)
			{
				foreach (AttributeInfo info in Attributes)
					info.Save(doc, element);
			}

			if(CDATA != null)
				CDATA.Save(doc, element);

			if (Text != null)
				Text.Save(doc, element);

			parent.AppendChild(element);
		}

		public void Load(XmlNode parent, bool ignoreCase)
		{
			foreach(XmlNode node in parent.ChildNodes)
			{
				if(node.NodeType == XmlNodeType.Element && string.Compare(node.Name, Name, ignoreCase) == 0)
				{
					XmlAttribute attrib = node.Attributes["ClassName"];
					if(attrib != null)
						ClassName = attrib.Value;

					if (Attributes != null && Attributes.Length > 0)
					{
						foreach (AttributeInfo info in Attributes)
							info.Load(node, ignoreCase);
					}

					if (CDATA != null)
						CDATA.Load(node, ignoreCase);

					if (Text != null)
						Text.Load(node, ignoreCase);
				}
			}
		}

		#endregion Methods
	}
}
