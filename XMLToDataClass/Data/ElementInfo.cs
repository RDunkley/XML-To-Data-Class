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
		#region Enumerations

		/// <summary>
		///   Specifies the possible access of the class that is created.
		/// </summary>
		public enum Access
		{
			#region Names

			/// <summary>
			///   Element is created as an internal class.
			/// </summary>
			Internal,

			/// <summary>
			///   Element is created as a public class.
			/// </summary>
			Public,

			#endregion Names
		}

		#endregion Enumerations

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
		///   Gets or sets the summary documentation section of the class.
		/// </summary>
		public string Summary { get; set; }

		/// <summary>
		///   Gets or sets the remarks documentation section of the class.
		/// </summary>
		public string Remarks { get; set; }

		/// <summary>
		///   Gets the XML node name.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		///   Gets or sets the accessibility of the class.
		/// </summary>
		public Access Accessibility { get; set; }

		#endregion Properties

		#region Methods

		public ElementInfo(XmlNode[] nodes)
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
					if (string.Compare(elementName, node.Name, false) != 0)
						throw new ArgumentException("One or more of the nodes provided do not contain the same element name");
				}
			}

			Name = elementName;
			ClassName = StringUtility.GetUpperCamelCase(elementName);
			Summary = string.Format("In memory representation of the XML element \"{0}\".", Name);
			Remarks = null;
			Accessibility = Access.Public;

			// Parse all the attribute names in the nodes.
			string[] attribNames = AttributeInfo.GetAllAttributeNames(nodes);

			// Determine the attributes.
			Attributes = new AttributeInfo[attribNames.Length];
			for (int i = 0; i < attribNames.Length; i++)
				Attributes[i] = new AttributeInfo(attribNames[i], nodes);

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
			CDATA = new CDataInfo(nodes);

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
			Text = new TextInfo(nodes);

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

		public ClassInfo GenerateDataClass(bool preserveHierarchy)
		{
			if (string.IsNullOrWhiteSpace(Remarks))
				Remarks = null;

			string accessibility = "public partial";
			if (Accessibility == Access.Internal)
				accessibility = "internal partial";

			ClassInfo info = new ClassInfo(accessibility, ClassName, null, Summary, Remarks);
			info.AddUsing("System.Xml");
			info.AddUsing("System.IO");

			List<DataInfo> dataList = new List<DataInfo>();

			// Add Ordinal Property.
			if ((Text.Include && Text.Info.PropertyName == "Ordinal") || (CDATA.Include && CDATA.Info.PropertyName == "Ordinal"))
				throw new InvalidOperationException("An attempt was made to create a property names 'Ordinal', but 'Add Ordinal' was specified in the settings which generates a property of this name.");
			foreach (AttributeInfo attrib in Attributes)
			{
				if (attrib.Info.PropertyName == "Ordinal")
					throw new InvalidOperationException("An attempt was made to create a property names 'Ordinal', but 'Add Ordinal' was specified in the settings which generates a property of this name.");
			}

			string remarks = "If the value is -1, then this object was not created from an XML node and the property has not been set.";
			info.Properties.Add(new PropertyInfo("public", "int", "Ordinal", "Gets the index of this object in relation to the other child element of this object's parent.", remarks));

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
				// Add data property.
				info.Properties.Add(data.GetProperty());

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
				info.Properties.Add(new PropertyInfo(Enum.GetName(typeof(Access), child.Accessibility).ToLower(), string.Format("{0}[]", child.ClassName), GenerateChildArrayNameProperty(child.ClassName), summary, null, null, "private"));
			}

			// Create the constructors.
			ConstructorInfo first = GenerateDataClassConstructor();
			first.OverloadedSummary = string.Format("Instantiates a new <see cref=\"{0}\"/> object.", ClassName);
			info.Constructors.Add(first);
			info.Constructors.Add(GenerateDataClassXmlNodeConstructor());
			info.Methods.Add(GenerateXmlNodeParsingMethod());

			// Add additional methods.
			info.Methods.AddRange(GenerateMethods(dataList.ToArray()));
			info.Methods.Add(GenerateCreateElementMethod());

			if(preserveHierarchy)
			{
				// Add additional sub-classes.
				foreach (ElementInfo child in Children)
				{
					ClassInfo childClass = child.GenerateDataClass(preserveHierarchy);
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
				"Creates an XML element for this object using the provided <see cref=\"XmlDocument\"/> object.",
				null,
				"<see cref=\"XmlElement\"/> object containing this classes data."
			);

			method.Parameters.Add(new ParameterInfo
			(
				"XmlDocument",
				"doc",
				"<see cref=\"XmlDocument\"/> object to generate the element from.",
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

			if (Children.Length > 0)
			{
				method.CodeLines.Add("// Build up dictionary of indexes and corresponding items.");
				method.CodeLines.Add("Dictionary<int, object> lookup = new Dictionary<int, object>();");
				foreach (ElementInfo element in Children)
				{
					method.CodeLines.Add(string.Empty);
					method.CodeLines.Add(string.Format("foreach({0} child in {1})", element.ClassName, GenerateChildArrayNameProperty(element.ClassName)));
					method.CodeLines.Add("{");
					method.CodeLines.Add("	if(lookup.ContainsKey(child.Ordinal))");
					method.CodeLines.Add("		throw new InvalidOperationException(\"An attempt was made to generate the XML element with two child elements with the same ordinal.Ordinals must be unique across all child objects.\");");
					method.CodeLines.Add("	lookup.Add(child.Ordinal, child); ");
					method.CodeLines.Add("}");
				}

				method.CodeLines.Add(string.Empty);
				method.CodeLines.Add("// Sort the keys.");
				method.CodeLines.Add("List<int> keys = lookup.Keys.ToList();");
				method.CodeLines.Add("keys.Sort();");

				method.CodeLines.Add(string.Empty);
				method.CodeLines.Add("foreach (int key in keys)");
				method.CodeLines.Add("{");
				foreach (ElementInfo element in Children)
				{
					method.CodeLines.Add(string.Format("	if(lookup[key] is {0})", element.ClassName));
					method.CodeLines.Add(string.Format("		returnElement.AppendChild((({0})lookup[key]).CreateElement(doc));", element.ClassName));
				}
				method.CodeLines.Add("}");
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

		/// <summary>
		///   Generates a class constructor from the element information.
		/// </summary>
		/// <returns></returns>
		private ConstructorInfo GenerateDataClassConstructor()
		{
			// Determine if method is internal or public.
			string accessibility = "public";
			foreach (ElementInfo eInfo in Children)
			{
				if (eInfo.Accessibility != Access.Public)
				{
					accessibility = "internal";
					break;
				}
			}

			string summary = string.Format("Instantiates a new <see cref=\"{0}\"/> object using the provided information.", ClassName);
			ConstructorInfo cInfo = new ConstructorInfo(accessibility, ClassName, summary);

			// Add parameters for each property.
			if (Text.Include)
			{
				string name = StringUtility.GetLowerCamelCase(Text.Info.PropertyName, true);
				bool? canBeNull = DetermineDataConstructorNullCheck(Text.Info);
				bool? canBeEmpty = DetermineDataContructorEmptyCheck(Text.Info);
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
				bool? canBeNull = DetermineDataConstructorNullCheck(CDATA.Info);
				bool? canBeEmpty = DetermineDataContructorEmptyCheck(CDATA.Info);
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
				bool? canBeNull = DetermineDataConstructorNullCheck(attrib.Info);
				bool? canBeEmpty = DetermineDataContructorEmptyCheck(attrib.Info);
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

			cInfo.CodeLines.Add("Ordinal = -1;");
			if (Children.Length != 0)
			{
				cInfo.CodeLines.Add(string.Empty);
				cInfo.CodeLines.Add("// Compute the maximum index used on any child items.");
				cInfo.CodeLines.Add("int maxIndex = 0;");
				foreach (ElementInfo eInfo in Children)
				{
					string propertyName = GenerateChildArrayNameProperty(eInfo.ClassName);
					cInfo.CodeLines.Add(string.Format("foreach({0} item in {1})", eInfo.ClassName, propertyName));
					cInfo.CodeLines.Add("{");
					cInfo.CodeLines.Add("	if(item.Ordinal >= maxIndex)");
					cInfo.CodeLines.Add("		maxIndex = item.Ordinal + 1; // Set to first index after this index.");
					cInfo.CodeLines.Add("}");
				}

				cInfo.CodeLines.Add(string.Empty);
				cInfo.CodeLines.Add("// Assign ordinal for any child items that don't have it set (-1).");
				foreach (ElementInfo eInfo in Children)
				{
					string propertyName = GenerateChildArrayNameProperty(eInfo.ClassName);
					cInfo.CodeLines.Add(string.Format("foreach({0} item in {1})", eInfo.ClassName, propertyName));
					cInfo.CodeLines.Add("{");
					cInfo.CodeLines.Add("	if(item.Ordinal == -1)");
					cInfo.CodeLines.Add("		item.Ordinal = maxIndex++;");
					cInfo.CodeLines.Add("}");
				}
			}

			return cInfo;
		}

		private bool? DetermineDataConstructorNullCheck(DataInfo info)
		{
			// Here is a truth table for the following logic:
			//
			// IsOptional	CanBeEmpty	IsNullable	IsArray	CanBeNull
			// 0			0			0			0		0
			// 1			0			0			0		1
			// 0			1			0			0		1
			// 1			1			0			0		1
			// 0			0			1			0		NULL
			// 1			0			1			0		1
			// 0			1			1			0		NULL
			// 1			1			1			0		1
			// 0			0			0			1		0
			// 1			0			0			1		1
			// 0			1			0			1		0
			// 1			1			0			1		1
			// 0			0			1			1		NULL
			// 1			0			1			1		1
			// 0			1			1			1		NULL
			// 1			1			1			1		1
			if (!info.SelectedDataTypeObject.IsArray)
			{
				if(!info.SelectedDataTypeObject.IsNullable)
				{
					return info.IsOptional || info.CanBeEmpty;
				}
				else
				{
					if (!info.IsOptional)
						return null;
					else
						return true;
				}
			}
			else
			{
				if(!info.SelectedDataTypeObject.IsNullable)
				{
					return info.IsOptional;
				}
				else
				{
					if (info.IsOptional)
						return true;
					else
						return null;
				}
			}
		}

		private bool? DetermineDataContructorEmptyCheck(DataInfo info)
		{
			// Here is a truth table for the following logic:
			//
			// IsOptional	CanBeEmpty	IsNullable	IsArray	CanBeEmpty
			// 0			0			0			0		NULL
			// 1			0			0			0		NULL
			// 0			1			0			0		NULL
			// 1			1			0			0		NULL
			// 0			0			1			0		NULL
			// 1			0			1			0		NULL
			// 0			1			1			0		NULL
			// 1			1			1			0		NULL
			// 0			0			0			1		0
			// 1			0			0			1		0
			// 0			1			0			1		1
			// 1			1			0			1		1
			// 0			0			1			1		NULL
			// 1			0			1			1		NULL
			// 0			1			1			1		NULL
			// 1			1			1			1		NULL

			if (!info.SelectedDataTypeObject.IsArray)
				return null;
			else
			{
				if (info.SelectedDataTypeObject.IsNullable)
					return null;
				else
					return info.CanBeEmpty;
			}
		}

		private MethodInfo GenerateXmlNodeParsingMethod()
		{
			string summary = string.Format("Parses an XML node and populates the data into this object.");
			MethodInfo info = new MethodInfo("public", "void", "ParseXmlNode", summary);
			info.Parameters.Add(new ParameterInfo("XmlNode", "node", "<see cref=\"XmlNode\"/> containing the data to extract.", false));
			info.Parameters.Add(new ParameterInfo("int", "ordinal", "Index of the <see cref=\"XmlNode\"/> in it's parent elements."));
			info.Exceptions.Add(new ExceptionInfo("ArgumentException", string.Format("<paramref name=\"node\"/> does not correspond to a {0} node.", Name)));
			info.Exceptions.Add(new ExceptionInfo("InvalidDataException", "An error occurred while reading the data into the node, or one of it's child nodes."));

			info.CodeLines.Add(string.Format("if(string.Compare(node.Name, \"{0}\", false) != 0)", Name));
			info.CodeLines.Add(string.Format("	throw new ArgumentException(\"node does not correspond to a {0} node.\");", Name));

			string textName = StringUtility.GetLowerCamelCase(Text.Info.PropertyName, true);
			string dataName = StringUtility.GetLowerCamelCase(CDATA.Info.PropertyName, true);

			if (Attributes.Length > 0)
			{
				info.CodeLines.Add(string.Empty);
				info.CodeLines.Add("XmlAttribute attrib;");

				foreach (AttributeInfo attrib in Attributes)
				{
					info.CodeLines.Add(string.Empty);
					info.CodeLines.Add(string.Format("// {0}", attrib.Info.Name));
					info.CodeLines.Add(string.Format("attrib = node.Attributes[\"{0}\"];", attrib.Info.Name));
					string space = string.Empty;
					if (!attrib.Info.IsOptional)
					{
						info.CodeLines.Add("if(attrib == null)");
						info.CodeLines.Add(string.Format("	throw new InvalidDataException(\"An XML string Attribute ({0}) is not optional, but was not found in the XML element ({1}).\");", attrib.Info.Name, Name));
					}
					else
					{
						info.CodeLines.Add("if(attrib == null)");
						info.CodeLines.Add(string.Format("	{0} = null;", attrib.Info.PropertyName));
						info.CodeLines.Add("else");
						space = "	";
					}
					info.CodeLines.Add(string.Format("{0}{1}(attrib.Value);", space, GetImportMethodName(attrib.Info.PropertyName)));
				}
			}

			// Parse the child nodes if needed.
			if (Text.Include || CDATA.Include || Children.Length > 0)
			{
				info.CodeLines.Add(string.Empty);
				info.CodeLines.Add("// Read the child objects.");
				foreach (ElementInfo child in Children)
					info.CodeLines.Add(string.Format("List<{0}> {1}List = new List<{0}>();", child.ClassName, StringUtility.GetLowerCamelCase(GenerateChildArrayNameProperty(child.ClassName), true)));

				if (Text.Include)
					info.CodeLines.Add(string.Format("bool {0}Found = false;", textName));

				if (CDATA.Include)
					info.CodeLines.Add(string.Format("bool {0}Found = false;", dataName));

				info.CodeLines.Add("int index = 0;");
				info.CodeLines.Add("foreach(XmlNode child in node.ChildNodes)");
				info.CodeLines.Add("{");
				foreach (ElementInfo child in Children)
				{
					info.CodeLines.Add(string.Format("	if(child.NodeType == XmlNodeType.Element && child.Name == \"{0}\")", child.Name));
					info.CodeLines.Add(string.Format("		{0}List.Add(new {1}(child, index++));", StringUtility.GetLowerCamelCase(GenerateChildArrayNameProperty(child.ClassName), true), child.ClassName));
				}

				if (Text.Include)
				{
					info.CodeLines.Add("	if(child.NodeType == XmlNodeType.Text)");
					info.CodeLines.Add("	{");
					info.CodeLines.Add(string.Format("		{0}(child.Value);", GetImportMethodName(Text.Info.PropertyName)));
					info.CodeLines.Add(string.Format("		{0}Found = true;", textName));
					info.CodeLines.Add("	}");
				}

				if (CDATA.Include)
				{
					info.CodeLines.Add("	if(child.NodeType == XmlNodeType.CDATA)");
					info.CodeLines.Add("	{");
					info.CodeLines.Add(string.Format("		{0}(child.Value);", GetImportMethodName(CDATA.Info.PropertyName)));
					info.CodeLines.Add(string.Format("		{0}Found = true;", dataName));
					info.CodeLines.Add("	}");
				}

				info.CodeLines.Add("}");
				foreach (ElementInfo child in Children)
				{
					string propertyName = GenerateChildArrayNameProperty(child.ClassName);
					info.CodeLines.Add(string.Format("{0} = {1}List.ToArray();", propertyName, StringUtility.GetLowerCamelCase(propertyName, true)));
				}
				if (Text.Include)
				{
					info.CodeLines.Add(string.Empty);
					info.CodeLines.Add(string.Format("if(!{0}Found)", textName));
					if (Text.Info.IsOptional)
						info.CodeLines.Add(string.Format("	{0} = null;", Text.Info.PropertyName));
					else
						info.CodeLines.Add(string.Format("	throw new InvalidDataException(\"An XML child Text node is not optional, but was not found in the XML element ({0}).\");", Name));
				}
				if (CDATA.Include)
				{
					info.CodeLines.Add(string.Empty);
					info.CodeLines.Add(string.Format("if(!{0}Found)", dataName));
					if (CDATA.Info.IsOptional)
						info.CodeLines.Add(string.Format("	{0} = null;", CDATA.Info.PropertyName));
					else
						info.CodeLines.Add(string.Format("	throw new InvalidDataException(\"An XML child CDATA node is not optional, but was not found in the XML element ({0}).\");", Name));
				}
				info.CodeLines.Add(string.Empty);
			}

			info.CodeLines.Add("Ordinal = ordinal;");
			return info;
		}

		private ConstructorInfo GenerateDataClassXmlNodeConstructor()
		{
			string summary = string.Format("Instantiates a new <see cref=\"{0}\"/> object from an <see cref=\"XmlNode\"/> object.", ClassName);
			ConstructorInfo cInfo = new ConstructorInfo("public", ClassName, summary);

			cInfo.Parameters.Add(new ParameterInfo("XmlNode", "node", "<see cref=\"XmlNode\"/> containing the data to extract.", false));
			cInfo.Parameters.Add(new ParameterInfo("int", "ordinal", "Index of the <see cref=\"XmlNode\"/> in it's parent elements."));
			cInfo.Exceptions.Add(new ExceptionInfo("ArgumentException", string.Format("<paramref name=\"node\"/> does not correspond to a {0} node or is not an 'Element' type node or <paramref name=\"ordinal\"/> is negative.", Name)));
			cInfo.Exceptions.Add(new ExceptionInfo("InvalidDataException", "An error occurred while reading the data into the node, or one of it's child nodes."));

			cInfo.CodeLines.Add("if(ordinal < 0)");
			cInfo.CodeLines.Add("	throw new ArgumentException(\"the ordinal specified is negative.\");");
			cInfo.CodeLines.Add("if(node.NodeType != XmlNodeType.Element)");
			cInfo.CodeLines.Add("	throw new ArgumentException(\"node is not of type 'Element'.\");");

			cInfo.CodeLines.Add(string.Empty);
			cInfo.CodeLines.Add("ParseXmlNode(node, ordinal);");

			return cInfo;
		}

		public void Save(XmlDocument doc, XmlNode parent)
		{
			XmlElement element = doc.CreateElement(Name);
			XmlAttribute attrib = element.Attributes.Append(doc.CreateAttribute("ClassName"));
			attrib.Value = ClassName;

			attrib = element.Attributes.Append(doc.CreateAttribute("Summary"));
			attrib.Value = Summary;

			attrib = element.Attributes.Append(doc.CreateAttribute("Remarks"));
			attrib.Value = Remarks;

			attrib = element.Attributes.Append(doc.CreateAttribute("Accessibility"));
			attrib.Value = Enum.GetName(typeof(Access), Accessibility);

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

		public void Load(XmlNode parent)
		{
			foreach(XmlNode node in parent.ChildNodes)
			{
				if(node.NodeType == XmlNodeType.Element && string.Compare(node.Name, Name, false) == 0)
				{
					XmlAttribute attrib = node.Attributes["ClassName"];
					if(attrib != null)
						ClassName = attrib.Value;

					attrib = node.Attributes["Summary"];
					if (attrib != null)
						Summary = attrib.Value;

					attrib = node.Attributes["Remarks"];
					if (attrib != null)
						Remarks = attrib.Value;

					attrib = node.Attributes["Accessibility"];
					if (attrib != null)
						Accessibility = (Access)Enum.Parse(typeof(Access), attrib.Value);

					if (Attributes != null && Attributes.Length > 0)
					{
						foreach (AttributeInfo info in Attributes)
							info.Load(node);
					}

					if (CDATA != null)
						CDATA.Load(node);

					if (Text != null)
						Text.Load(node);
				}
			}
		}

		#endregion Methods
	}
}
