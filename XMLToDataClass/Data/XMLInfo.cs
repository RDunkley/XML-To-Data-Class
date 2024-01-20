//********************************************************************************************************************************
// Filename:    XMLInfo.cs
// Owner:       Richard Dunkley
// Description: Class which represents information in an XML file.
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
using System.IO;
using System.Security;
using System.Xml;

namespace XMLToDataClass.Data
{
	/// <summary>
	///   Stores information extracted from an XML file.
	/// </summary>
	public class XMLInfo
	{
		#region Enums

		private enum XmlType
		{
			File,
			Stream,
			Text,
			XML,
		}

		#endregion

		#region Properties

		public ElementInfo RootNode { get; private set; }

		public Dictionary<string, Dictionary<string, ElementInfo>> Elements { get; private set; }

		public bool HierarchyMaintained { get; private set; }

		public string FilePath { get; private set; }

		public string Version { get; set; }

		public string Encoding { get; set; }

		#endregion Properties

		#region Methods

		/// <summary>
		///   Instantiates a new <see cref="XMLInfo"/> object using the specified <see cref="XmlDocument"/>.
		/// </summary>
		/// <param name="doc"><see cref="XmlDocument"/> object to be parsed.</param>
		/// <param name="maintainHierarchy">True if the hierarchy of the XMl should be maintained, false otherwise.</param>
		/// <exception cref="ArgumentNullException"><paramref name="doc"/> is a null reference.</exception>
		/// <exception cref="InvalidDataException">The XML file could not be loaded.</exception>
		public XMLInfo(string filePath, bool maintainHierarchy)
		{
			if (filePath == null)
				throw new ArgumentNullException("filePath");
			if (filePath.Length == 0)
				throw new ArgumentException("filePath is an empty string.");

			XmlDocument doc = new XmlDocument();
			try
			{
				doc.Load(filePath);
			}
			catch (XmlException e)
			{
				throw new InvalidDataException(string.Format("The XML file specified ({0}) does not contain valid XML ({1}).", filePath, e.Message), e);
			}
			catch (ArgumentException e)
			{
				throw new InvalidDataException(string.Format("The path to the XML file ({0}) is not a valid path.", filePath), e);
			}
			catch (PathTooLongException e)
			{
				throw new InvalidDataException(string.Format("The path to the XML file ({0}) exceeds the maximum path length.", filePath), e);
			}
			catch (DirectoryNotFoundException e)
			{
				throw new InvalidDataException(string.Format("The path to the XML file ({0}) is not valid.", filePath), e);
			}
			catch (FileNotFoundException e)
			{
				throw new InvalidDataException(string.Format("The XML file specified {0} could not be found.", filePath), e);
			}
			catch (IOException e)
			{
				throw new InvalidDataException(string.Format("Unable to open the XML file ({0}).", filePath), e);
			}
			catch (UnauthorizedAccessException e)
			{
				throw new InvalidDataException(string.Format("Unable to access the XML file specified ({0}).", filePath), e);
			}
			catch (NotSupportedException e)
			{
				throw new InvalidDataException(string.Format("The XML file specified {0} is in an invalid format.", filePath), e);
			}
			catch (SecurityException e)
			{
				throw new InvalidDataException(string.Format("Unable to access the XML file specified ({0}).", filePath), e);
			}

			GetHeaderInformation(doc);
			Dictionary<string, XmlNode[]> xmlNodesByName;
			if (maintainHierarchy)
				xmlNodesByName = BuildUniqueHierarchicalElementTable(doc);
			else
				xmlNodesByName = BuildUniqueElementTable(doc);

			// Generate the element objects.
			Dictionary<string, Dictionary<string, ElementInfo>> xmlElements = new Dictionary<string, Dictionary<string, ElementInfo>>();
			foreach (string key in xmlNodesByName.Keys)
			{
				ElementInfo element = new ElementInfo(xmlNodesByName[key]);
				if (!xmlElements.ContainsKey(element.NameSpace))
					xmlElements.Add(element.NameSpace, new Dictionary<string, ElementInfo>());
				xmlElements[element.NameSpace].Add(key, element);
			}
			AddChildElements(xmlNodesByName, xmlElements, maintainHierarchy);
			RootNode = GetRootNode(doc, xmlElements);
			Elements = xmlElements;
			FilePath = filePath;
			HierarchyMaintained = maintainHierarchy;
		}

		private ElementInfo GetRootNode(XmlDocument doc, Dictionary<string, Dictionary<string, ElementInfo>> xmlElements)
		{
			foreach(XmlNode child in doc.ChildNodes)
			{
				if(child.NodeType == XmlNodeType.Element)
				{
					return xmlElements[child.Prefix][child.Name];
				}
			}

			throw new InvalidOperationException("Unable to locate a root node in the XML file");
		}

		private void GetHeaderInformation(XmlDocument doc)
		{
			if (doc.FirstChild is XmlDeclaration dec)
			{
				Version = dec.Version;
				Encoding = dec.Encoding;
			}
		}

		/// <summary>
		///   Adds the child elements to the element lookup table.
		/// </summary>
		/// <param name="nodesByName">Lookup table of each <see cref="XmlNode"/> by it's name.</param>
		/// <param name="elementsByName">Lookup table of each <see cref="ElementInfo"/> by it's <see cref="XmlNode"/> name.</param>
		private void AddChildElements(Dictionary<string, XmlNode[]> nodesByName, Dictionary<string, Dictionary<string, ElementInfo>> elementsByName, bool maintainHierarchy)
		{
			foreach (string name in nodesByName.Keys)
			{
				// Get all the child node names.
				SortedDictionary<string, XmlNode> childLookup = new SortedDictionary<string, XmlNode>();
				foreach (XmlNode node in nodesByName[name])
				{
					foreach (XmlNode child in node.ChildNodes)
					{
						string childName = child.Name;
						if (child.NodeType == XmlNodeType.Element && !childLookup.ContainsKey(childName))
							childLookup.Add(childName, child);
					}
				}

				// Build the child element array.
				List<ElementInfo> childElements = new List<ElementInfo>(childLookup.Count);
				foreach (string childName in childLookup.Keys)
				{
					string elementName = childName;
					if (maintainHierarchy)
						elementName = string.Format("{0}.{1}", name, childName);
					childElements.Add(elementsByName[childLookup[childName].Prefix][elementName]);
				}

				// Add the array to the element.
				elementsByName[nodesByName[name][0].Prefix][name].Children = childElements.ToArray();
			}
		}

		/// <summary>
		///   Add's the <see cref="XmlNode"/> names to the lookup table.
		/// </summary>
		/// <param name="node"><see cref="XmlNode"/> to pull the names from.</param>
		/// <param name="lookup">Lookup table to add the information to.</param>
		/// <remarks>This method is called recursively to cover all descendant nodes.</remarks>
		private void AddToNodeLookupByName(XmlNode node, Dictionary<string, List<XmlNode>> lookup)
		{
			foreach (XmlNode child in node.ChildNodes)
			{
				if (child.NodeType == XmlNodeType.Element)
				{
					string childName = child.Name;

					// Add the child to the lookup table.
					if (!lookup.ContainsKey(childName))
						lookup.Add(childName, new List<XmlNode>());
					lookup[childName].Add(child);

					// Add any children of the child to the node as well.
					AddToNodeLookupByName(child, lookup);
				}
			}
		}

		private void AddtoNodeLookupByHeirarchicalName(string heirarchicalName, XmlNode node, Dictionary<string, List<XmlNode>> lookup)
		{
			foreach(XmlNode child in node.ChildNodes)
			{
				if(child.NodeType == XmlNodeType.Element)
				{
					string childName = child.Name;

					// Add the child to the lookup table.
					string name;
					if (heirarchicalName == null)
						name = childName;
					else
						name = string.Format("{0}.{1}", heirarchicalName, childName);
					if (!lookup.ContainsKey(name))
						lookup.Add(name, new List<XmlNode>());
					lookup[name].Add(child);

					// Add any children of the child to the node as well.
					AddtoNodeLookupByHeirarchicalName(name, child, lookup);
				}
			}
		}

		/// <summary>
		///   Parses the <see cref="XmlDocument"/> object to create a lookup table of the names of the nodes.
		/// </summary>
		/// <param name="doc"><see cref="XmlDocument"/> to be parsed.</param>
		/// <returns>Lookup table used to find all the <see cref="XmlNode"/>s with a specified name.</returns>
		private Dictionary<string, XmlNode[]> BuildUniqueElementTable(XmlDocument doc)
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

		private Dictionary<string, XmlNode[]> BuildUniqueHierarchicalElementTable(XmlDocument doc)
		{
			Dictionary<string, List<XmlNode>> lookup = new Dictionary<string, List<XmlNode>>();
			AddtoNodeLookupByHeirarchicalName(null, doc, lookup);

			// Sort the names.
			List<string> sortList = new List<string>(lookup.Count);
			sortList.AddRange(lookup.Keys);
			sortList.Sort();

			Dictionary<string, XmlNode[]> returnLookup = new Dictionary<string, XmlNode[]>(lookup.Count);
			foreach (string name in sortList)
				returnLookup.Add(name, lookup[name].ToArray());
			return returnLookup;
		}

		public ElementInfo[] GetAllNodes()
		{
			List<ElementInfo> nodeList = new List<ElementInfo>();
			foreach(string namespc in Elements.Keys)
				nodeList.AddRange(Elements[namespc].Values);
			return nodeList.ToArray();
		}

		public ElementInfo[] GetAllNamespaceNodes(string nameSpace)
		{
			if (!Elements.ContainsKey(nameSpace))
				throw new ArgumentException($"The name space specified ({nameSpace}) was not found in the XML file.");

			List<ElementInfo> nodeList = new List<ElementInfo>(Elements[nameSpace].Count);
			nodeList.AddRange(Elements[nameSpace].Values);
			return nodeList.ToArray();
		}

		public string[] GetAllNamespaces()
		{
			List<string> names = new List<string>(Elements.Count);
			names.AddRange(Elements.Keys);
			return names.ToArray();
		}

		private string GetXmlFileVersionProperty(List<PropertyInfo> props)
		{
			string name = "XmlFileVersion";
			bool found = false;
			foreach(PropertyInfo prop in props)
			{
				if(string.Compare(prop.Name, name, false) == 0)
				{
					found = true;
					break;
				}
			}

			if (!found)
				return name;

			name = "XmlVersion";
			found = false;
			foreach (PropertyInfo prop in props)
			{
				if (string.Compare(prop.Name, name, false) == 0)
				{
					found = true;
					break;
				}
			}

			if (!found)
				return name;

			name = "Xml_File_Version";
			found = false;
			foreach (PropertyInfo prop in props)
			{
				if (string.Compare(prop.Name, name, false) == 0)
				{
					found = true;
					break;
				}
			}

			if (!found)
				return name;

			name = "Xml_Version";
			found = false;
			foreach (PropertyInfo prop in props)
			{
				if (string.Compare(prop.Name, name, false) == 0)
				{
					found = true;
					break;
				}
			}

			if (!found)
				return name;

			// Give up.
			throw new InvalidOperationException("Could not find an available property name for the XML Version in the root XML node class (attempted: XmlFileVersion, XmlVersion, Xml_File_Version, and Xml_Version). One of these property names must be free to generate the code.");
		}

		private string GetXmlFileEncodingProperty(List<PropertyInfo> props)
		{
			string name = "XmlFileEncoding";
			bool found = false;
			foreach (PropertyInfo prop in props)
			{
				if (string.Compare(prop.Name, name, false) == 0)
				{
					found = true;
					break;
				}
			}

			if (!found)
				return name;

			name = "XmlEncoding";
			found = false;
			foreach (PropertyInfo prop in props)
			{
				if (string.Compare(prop.Name, name, false) == 0)
				{
					found = true;
					break;
				}
			}

			if (!found)
				return name;

			name = "Xml_File_Encoding";
			found = false;
			foreach (PropertyInfo prop in props)
			{
				if (string.Compare(prop.Name, name, false) == 0)
				{
					found = true;
					break;
				}
			}

			if (!found)
				return name;

			name = "Xml_Encoding";
			found = false;
			foreach (PropertyInfo prop in props)
			{
				if (string.Compare(prop.Name, name, false) == 0)
				{
					found = true;
					break;
				}
			}

			if (!found)
				return name;

			// Give up.
			throw new InvalidOperationException("Could not find an available property name for the XML Encoding in the root XML node class (attempted: XmlFileEncoding, XmlEncoding, Xml_File_Encoding, and Xml_Encoding). One of these property names must be free to generate the code.");
		}

		public CSCodeGen.FileInfo[] GenerateClassFiles(string nameSpace)
		{
			// Add the main class.
			List<CSCodeGen.FileInfo> fileList = new List<CSCodeGen.FileInfo>();
			string fileExtension = null;
			if (!string.IsNullOrEmpty(Properties.Settings.Default.FileExtensionAddition))
				fileExtension = Properties.Settings.Default.FileExtensionAddition;

			if (HierarchyMaintained)
			{
				ClassInfo rootClass = RootNode.GenerateDataClass(HierarchyMaintained);
				AddAdditionalItemsToRoot(rootClass);
				fileList.Add(new CSCodeGen.FileInfo(nameSpace, rootClass, null, null, fileExtension));
			}
			else
			{
				ElementInfo[] nodes = GetAllNodes();
				foreach (ElementInfo info in nodes)
				{
					ClassInfo classInfo = info.GenerateDataClass(HierarchyMaintained);
					if(info == RootNode)
						AddAdditionalItemsToRoot(classInfo);
					fileList.Add(new CSCodeGen.FileInfo(nameSpace, classInfo, null, null, fileExtension));
				}
			}
			return fileList.ToArray();
		}

		private void AddAdditionalItemsToRoot(ClassInfo classInfo)
		{
			string xmlVersionName = GetXmlFileVersionProperty(classInfo.Properties);
			string xmlEncodingName = GetXmlFileEncodingProperty(classInfo.Properties);
			foreach (ConstructorInfo con in classInfo.Constructors)
			{
				con.CodeLines.Add(string.Format("{0} = mDefaultXMLVersion;", xmlVersionName));
				con.CodeLines.Add(string.Format("{0} = mDefaultXMLEncoding;", xmlEncodingName));
			}
			classInfo.Fields.Add(new FieldInfo("private const", "string", "mDefaultXMLVersion", "Default version of the XML file generated from this object.", null, "\"1.0\""));
			classInfo.Fields.Add(new FieldInfo("private const", "string", "mDefaultXMLEncoding", "Default encoding of the XML file generated from this object.", null, "\"UTF-8\""));
			classInfo.Properties.Add(new PropertyInfo("public", "string", xmlVersionName, "Version of the XML file this root node will be contained in.", "Defaults to '1.0'"));
			classInfo.Properties.Add(new PropertyInfo("public", "string", xmlEncodingName, "Encoding of the XML file this root node will be contained in.", "Defaults to 'UTF-8'"));
			classInfo.Constructors.Add(GenerateXMLFileConstructor(xmlVersionName, xmlEncodingName, XmlType.File));
			classInfo.Constructors.Add(GenerateXMLFileConstructor(xmlVersionName, xmlEncodingName, XmlType.Stream));
			classInfo.Constructors.Add(GenerateXMLFileConstructor(xmlVersionName, xmlEncodingName, XmlType.Text));
			classInfo.Constructors.Add(GenerateXMLFileConstructor(xmlVersionName, xmlEncodingName, XmlType.XML));
			classInfo.Methods.Add(GenerateExporterMethod(xmlVersionName, xmlEncodingName, XmlType.File));
			classInfo.Methods.Add(GenerateExporterMethod(xmlVersionName, xmlEncodingName, XmlType.Stream));
			classInfo.Methods.Add(GenerateExporterMethod(xmlVersionName, xmlEncodingName, XmlType.Text));
			classInfo.Methods.Add(GenerateExporterMethod(xmlVersionName, xmlEncodingName, XmlType.XML));
			classInfo.Methods.Add(GenerateImporterMethod(xmlVersionName, xmlEncodingName, XmlType.File));
			classInfo.Methods.Add(GenerateImporterMethod(xmlVersionName, xmlEncodingName, XmlType.Stream));
			classInfo.Methods.Add(GenerateImporterMethod(xmlVersionName, xmlEncodingName, XmlType.Text));
			classInfo.Methods.Add(GenerateImporterMethod(xmlVersionName, xmlEncodingName, XmlType.XML));
			classInfo.AddUsing("System.Xml");
			classInfo.AddUsing("System.IO");
			classInfo.AddUsing("System.Security");
		}

		private MethodInfo GenerateImporterMethod(string versionProperty, string encodingProperty, XmlType type)
		{
			string summary = null;
			switch(type)
			{
				case XmlType.File:
					summary = "Imports data from an XML file.";
					break;
				case XmlType.Stream:
					summary = "Imports data from an XML stream.";
					break;
				case XmlType.Text:
					summary = "Imports data from an XML text reader.";
					break;
				case XmlType.XML:
					summary = "Imports data from an XML reader.";
					break;
			}
			MethodInfo method = new MethodInfo("public", "void", "ImportFromXML", summary);

			switch(type)
			{
				case XmlType.File:
					method.Parameters.Add(new ParameterInfo("string", "filePath", "Path to the XML file containing the data to be imported.", false, false));
					method.Exceptions.Add(new ExceptionInfo("ArgumentException", "<paramref name=\"filePath\"/> is invalid or an error occurred while accessing it."));
					break;
				case XmlType.Stream:
					method.Parameters.Add(new ParameterInfo("Stream", "stream", "Stream containing the XML file data.", false));
					method.Exceptions.Add(new ExceptionInfo("ArgumentException", "<paramref name=\"stream\"/> did not contain valid XML."));
					break;
				case XmlType.Text:
					method.Parameters.Add(new ParameterInfo("TextReader", "reader", "TextReader object containing the XML file data.", false));
					method.Exceptions.Add(new ExceptionInfo("ArgumentException", "A parsing error occurred while attempting to load the XML from <paramref name=\"reader\"/>."));
					break;
				case XmlType.XML:
					method.Parameters.Add(new ParameterInfo("XmlReader", "reader", "XmlReader object containing the XML file data.", false));
					method.Exceptions.Add(new ExceptionInfo("ArgumentException", "A parsing error occurred while attempting to load the XML from <paramref name=\"reader\"/>."));
					break;
			}
			method.Exceptions.Add(new ExceptionInfo("InvalidDataException", "The XML was valid, but an error occurred while extracting the data from it."));

			method.CodeLines.Add(string.Empty);
			method.CodeLines.Add("XmlDocument doc = new XmlDocument();");
			method.CodeLines.Add("try");
			method.CodeLines.Add("{");
			switch(type)
			{
				case XmlType.File:
					method.CodeLines.Add("	doc.Load(filePath);");
					method.CodeLines.Add("}");
					method.CodeLines.Add("catch(PathTooLongException e)");
					method.CodeLines.Add("{");
					method.CodeLines.Add("	throw new ArgumentException(string.Format(\"The file path specified ({0}) is not valid ({1}).\", filePath, e.Message), nameof(filePath), e);");
					method.CodeLines.Add("}");
					method.CodeLines.Add("catch(DirectoryNotFoundException e)");
					method.CodeLines.Add("{");
					method.CodeLines.Add("	throw new ArgumentException(string.Format(\"The file path specified ({0}) is not valid ({1}).\", filePath, e.Message), nameof(filePath), e);");
					method.CodeLines.Add("}");
					method.CodeLines.Add("catch(NotSupportedException e)");
					method.CodeLines.Add("{");
					method.CodeLines.Add("	throw new ArgumentException(string.Format(\"The file path specified ({0}) is not valid ({1}).\", filePath, e.Message), nameof(filePath), e);");
					method.CodeLines.Add("}");
					method.CodeLines.Add("catch(FileNotFoundException e)");
					method.CodeLines.Add("{");
					method.CodeLines.Add("	throw new ArgumentException(string.Format(\"The file could not be located at the path specified ({0}).\", filePath), nameof(filePath), e);");
					method.CodeLines.Add("}");
					method.CodeLines.Add("catch(IOException e)");
					method.CodeLines.Add("{");
					method.CodeLines.Add("	throw new ArgumentException(string.Format(\"An I/O error occurred ({0}) while opening the file specified ({1}).\", e.Message, filePath), nameof(filePath), e);");
					method.CodeLines.Add("}");
					method.CodeLines.Add("catch(UnauthorizedAccessException e)");
					method.CodeLines.Add("{");
					method.CodeLines.Add("	throw new ArgumentException(string.Format(\"Unable to access the file path specified ({0}).\", filePath), nameof(filePath), e);");
					method.CodeLines.Add("}");
					method.CodeLines.Add("catch(SecurityException e)");
					method.CodeLines.Add("{");
					method.CodeLines.Add("	throw new ArgumentException(string.Format(\"The caller doesn't have the required permissions to access the file path specified ({0}).\", filePath), nameof(filePath), e);");
					method.CodeLines.Add("}");
					method.CodeLines.Add("catch(XmlException e)");
					method.CodeLines.Add("{");
					method.CodeLines.Add("	throw new ArgumentException(string.Format(\"Unable to parse the XML from the file specified ({0}). Error message: {1}.\", filePath, e.Message), nameof(filePath), e);");
					method.CodeLines.Add("}");
					break;
				case XmlType.Stream:
					method.CodeLines.Add("	doc.Load(stream);");
					method.CodeLines.Add("}");
					method.CodeLines.Add("catch(XmlException e)");
					method.CodeLines.Add("{");
					method.CodeLines.Add("	throw new ArgumentException(string.Format(\"Unable to parse the XML from the stream. Error message: {0}.\", e.Message), nameof(stream), e);");
					method.CodeLines.Add("}");
					break;
				default:
					method.CodeLines.Add("	doc.Load(reader);");
					method.CodeLines.Add("}");
					method.CodeLines.Add("catch(XmlException e)");
					method.CodeLines.Add("{");
					method.CodeLines.Add("	throw new ArgumentException(string.Format(\"Unable to parse the XML from the reader. Error message: {0}.\", e.Message), nameof(reader), e);");
					method.CodeLines.Add("}");
					break;
			}

			method.CodeLines.Add(string.Empty);
			method.CodeLines.Add("// Pull the version and encoding");
			method.CodeLines.Add("XmlDeclaration dec = doc.FirstChild as XmlDeclaration;");
			method.CodeLines.Add("if(dec != null)");
			method.CodeLines.Add("{");
			method.CodeLines.Add(string.Format("	{0} = dec.Version;", versionProperty));
			method.CodeLines.Add(string.Format("	{0} = dec.Encoding;", encodingProperty));
			method.CodeLines.Add("}");
			method.CodeLines.Add("else");
			method.CodeLines.Add("{");
			method.CodeLines.Add(string.Format("	{0} = mDefaultXMLVersion;", versionProperty));
			method.CodeLines.Add(string.Format("	{0} = mDefaultXMLEncoding;", encodingProperty));
			method.CodeLines.Add("}");
			method.CodeLines.Add(string.Empty);

			// Add the list objects.
			method.CodeLines.Add("XmlElement root = doc.DocumentElement;");
			method.CodeLines.Add("if(root.NodeType != XmlNodeType.Element)");
			method.CodeLines.Add("	throw new InvalidDataException(\"The root node is not an element node.\");");
			method.CodeLines.Add(string.Format("if(string.Compare(root.Name, \"{0}\", false) != 0)", RootNode.FullName));
			method.CodeLines.Add(string.Format("	throw new InvalidDataException(string.Format(\"The root element name is not the one expected (Actual: '{{0}}', Expected: '{0}').\", root.Name));", RootNode.FullName));
			method.CodeLines.Add(string.Empty);
			method.CodeLines.Add("ParseXmlNode(root, 0);");
			return method;
		}

		private ConstructorInfo GenerateXMLFileConstructor(string versionProperty, string encodingProperty, XmlType type)
		{
			// Append the signature of the method.
			string summary = string.Format("Instantiates a new <see cref=\"{0}\"/> object from an XML file.", RootNode.ClassName);
			ConstructorInfo method = new ConstructorInfo("public", RootNode.ClassName, summary);

			switch (type)
			{
				case XmlType.File:
					method.Parameters.Add(new ParameterInfo("string", "filePath", "Path to the XML file containing the data to be imported.", false, false));
					method.Exceptions.Add(new ExceptionInfo("ArgumentException", "<paramref name=\"filePath\"/> is invalid or an error occurred while accessing it."));
					method.CodeLines.Add(string.Empty);
					method.CodeLines.Add("ImportFromXML(filePath);");
					break;
				case XmlType.Stream:
					method.Parameters.Add(new ParameterInfo("Stream", "stream", "Stream containing the XML file data.", false));
					method.Exceptions.Add(new ExceptionInfo("ArgumentException", "<paramref name=\"stream\"/> did not contain valid XML."));
					method.CodeLines.Add(string.Empty);
					method.CodeLines.Add("ImportFromXML(stream);");
					break;
				case XmlType.Text:
					method.Parameters.Add(new ParameterInfo("TextReader", "reader", "TextReader object containing the XML file data.", false));
					method.Exceptions.Add(new ExceptionInfo("ArgumentException", "A parsing error occurred while attempting to load the XML from <paramref name=\"reader\"/>."));
					method.CodeLines.Add(string.Empty);
					method.CodeLines.Add("ImportFromXML(reader);");
					break;
				case XmlType.XML:
					method.Parameters.Add(new ParameterInfo("XmlReader", "reader", "XmlReader object containing the XML file data.", false));
					method.Exceptions.Add(new ExceptionInfo("ArgumentException", "A parsing error occurred while attempting to load the XML from <paramref name=\"reader\"/>."));
					method.CodeLines.Add(string.Empty);
					method.CodeLines.Add("ImportFromXML(reader);");
					break;
			}
			method.Exceptions.Add(new ExceptionInfo("InvalidDataException", "An error occurred while parsing the XML data."));
			return method;
		}

		private MethodInfo GenerateExporterMethod(string versionPropName, string encodingPropName, XmlType type)
		{
			string summary = "Exports data to an XML file.";
			MethodInfo method = new MethodInfo("public", "void", "ExportToXML", summary);
			switch (type)
			{
				case XmlType.File:
					method.Parameters.Add(new ParameterInfo("string", "filePath", "Path to the XML file to be written to. If file exists all contents will be destroyed.", false, false));
					method.Exceptions.Add(new ExceptionInfo("ArgumentException", "<paramref name=\"filePath\"/> is invalid or an error occurred while accessing it."));
					break;
				case XmlType.Stream:
					method.Parameters.Add(new ParameterInfo("Stream", "stream", "Stream to write the XML to.", false));
					break;
				case XmlType.Text:
					method.Parameters.Add(new ParameterInfo("TextWriter", "writer", "TextWriter object to write the XML to.", false));
					break;
				case XmlType.XML:
					method.Parameters.Add(new ParameterInfo("XmlWriter", "writer", "XmlWriter object to write the XML to.", false));
					break;
			}
			method.CodeLines.Add("XmlDocument doc = new XmlDocument();");
			method.CodeLines.Add(string.Format("XmlDeclaration dec = doc.CreateXmlDeclaration({0}, {1}, null);", versionPropName, encodingPropName));
			method.CodeLines.Add("doc.InsertBefore(dec, doc.DocumentElement);");
			method.CodeLines.Add(string.Empty);

			method.CodeLines.Add("XmlElement root = CreateElement(doc);");
			method.CodeLines.Add("doc.AppendChild(root);");
			switch (type)
			{
				case XmlType.File:
					method.CodeLines.Add("doc.Save(filePath);");
					break;
				case XmlType.Stream:
					method.CodeLines.Add("doc.Save(stream);");
					break;
				default:
					method.CodeLines.Add("doc.Save(writer);");
					break;
			}
			
			return method;
		}

		public void GenerateCode(string codeOutputFolder, string nameSpace, string projectName, string solutionName)
		{
			if (codeOutputFolder == null)
				throw new ArgumentNullException("codeOutputFolder");
			if (nameSpace == null)
				throw new ArgumentNullException("nameSpace");
			if (nameSpace.Length == 0)
				throw new ArgumentException("nameSpace cannot be an empty string");

			try
			{
				codeOutputFolder = Path.GetFullPath(codeOutputFolder);
			}
			catch (Exception e)
			{
				throw new ArgumentException(string.Format("The directory specified in codeOutputFolder ({0}) is not valid. See inner exception.", codeOutputFolder), e);
			}

			if (!Directory.Exists(codeOutputFolder))
				Directory.CreateDirectory(codeOutputFolder);
				//throw new ArgumentException(string.Format("The directory specified in codeOutputFolder ({0}) does not exist.", codeOutputFolder));

			CSCodeGen.FileInfo[] files = GenerateClassFiles(nameSpace);

			// Generate the code.
			if (projectName == null)
			{
				foreach (CSCodeGen.FileInfo file in files)
					file.WriteToFile(codeOutputFolder);
			}
			else
			{
				string relativePath = null;
				if (solutionName != null)
					relativePath = projectName;
				CSCodeGen.ProjectInfo proj = new CSCodeGen.ProjectInfo(projectName, "bin\\Debug\\", "bin\\Release\\", CSCodeGen.ProjectType.Library, relativePath);
				proj.References.Add(new CSCodeGen.ProjectReferenceAssembly("System"));
				proj.References.Add(new CSCodeGen.ProjectReferenceAssembly("System.Core"));
				proj.References.Add(new CSCodeGen.ProjectReferenceAssembly("System.Data"));
				proj.References.Add(new CSCodeGen.ProjectReferenceAssembly("System.Xml"));
				foreach (CSCodeGen.FileInfo file in files)
					proj.AddFile(file);

				if (solutionName == null)
				{
					proj.WriteToFiles(codeOutputFolder);
				}
				else
				{
					CSCodeGen.SolutionInfo sol = new CSCodeGen.SolutionInfo(solutionName);
					sol.Projects.Add(proj);
					sol.WriteToFiles(codeOutputFolder);
				}
			}
		}

		public void Save(string filePath, string outputFolder, string nameSpace, bool genProject, string projectName, bool genSolution, string solutionName)
		{
			XmlDocument doc = new XmlDocument();
			XmlElement root = doc.CreateElement("save_data");
			doc.AppendChild(root);

			// Store the additional settings from the caller.
			XmlAttribute attrib = root.Attributes.Append(doc.CreateAttribute("OutputFolder"));
			attrib.Value = outputFolder;
			attrib = root.Attributes.Append(doc.CreateAttribute("NameSpace"));
			attrib.Value = nameSpace;
			attrib = root.Attributes.Append(doc.CreateAttribute("GenerateProject"));
			attrib.Value = genProject.ToString();
			attrib = root.Attributes.Append(doc.CreateAttribute("ProjectName"));
			attrib.Value = projectName;
			attrib = root.Attributes.Append(doc.CreateAttribute("GenerateSolution"));
			attrib.Value = genSolution.ToString();
			attrib = root.Attributes.Append(doc.CreateAttribute("SolutionName"));
			attrib.Value = solutionName;


			// Save all the element configuration.
			foreach (string namespc in Elements.Keys)
			{
				foreach (ElementInfo element in Elements[namespc].Values)
					element.Save(doc, root);
			}

			doc.Save(filePath);
		}

		public void Load(Stream fileStream, out string outputFolder, out string nameSpace, out bool? genProject, out string projectName, out bool? genSolution, out string solutionName)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(fileStream);

			outputFolder = null;
			nameSpace = null;
			genProject = null;
			projectName = null;
			genSolution = null;
			solutionName = null;

			foreach (XmlNode node in doc.ChildNodes)
			{
				if (node.NodeType == XmlNodeType.Element && string.Compare(node.Name, "save_data", true) == 0)
				{
					XmlAttribute attrib = node.Attributes["OutputFolder"];
					if (attrib != null)
						outputFolder = attrib.Value;
					attrib = node.Attributes["NameSpace"];
					if (attrib != null)
						nameSpace = attrib.Value;
					attrib = node.Attributes["GenerateProject"];
					if (attrib != null)
					{
						if (bool.TryParse(attrib.Value, out bool temp))
							genProject = temp;
					}
					attrib = node.Attributes["ProjectName"];
					if (attrib != null)
						projectName = attrib.Value;
					attrib = node.Attributes["GenerateSolution"];
					if (attrib != null)
					{
						if (bool.TryParse(attrib.Value, out bool temp))
							genSolution = temp;
					}
					attrib = node.Attributes["SolutionName"];
					if (attrib != null)
						solutionName = attrib.Value;

					// Load all the element configurations.
					foreach (string namespc in Elements.Keys)
					{
						foreach (ElementInfo element in Elements[namespc].Values)
							element.Load(node);
					}
					break;
				}
			}
		}

		public void Load(string filePath, out string outputFolder, out string nameSpace, out bool? genProject, out string projectName, out bool? genSolution, out string solutionName)
		{
			using (FileStream stream = new FileStream(filePath, FileMode.Open))
			{
				Load(stream, out outputFolder, out nameSpace, out genProject, out projectName, out genSolution, out solutionName);
			}
		}

		#endregion Methods
	}
}
