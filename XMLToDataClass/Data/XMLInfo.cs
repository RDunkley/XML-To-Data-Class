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
		#region Properties

		public ElementInfo RootNode { get; private set; }

		public Dictionary<string, ElementInfo> Elements { get; private set; }

		public bool HierarchyMaintained { get; private set; }

		public string FilePath { get; private set; }

		public string MainClassName { get; set; }

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
			Dictionary<string, ElementInfo> xmlElements = new Dictionary<string, ElementInfo>();
			foreach(string key in xmlNodesByName.Keys)
				xmlElements.Add(key, new ElementInfo(xmlNodesByName[key]));
			AddChildElements(xmlNodesByName, xmlElements, maintainHierarchy);
			RootNode = GetRootNode(doc, xmlElements);
			Elements = xmlElements;
			MainClassName = StringUtility.GetUpperCamelCase(Path.GetFileNameWithoutExtension(filePath));
			FilePath = filePath;
			HierarchyMaintained = maintainHierarchy;
		}

		private ElementInfo GetRootNode(XmlDocument doc, Dictionary<string, ElementInfo> xmlElements)
		{
			foreach(XmlNode child in doc.ChildNodes)
			{
				if(child.NodeType == XmlNodeType.Element)
				{
					foreach(ElementInfo element in xmlElements.Values)
					{
						if (string.Compare(child.Name, element.Name, false) == 0)
							return element;
					}
				}
			}

			throw new InvalidOperationException("Unable to locate a root node in the XML file");
		}

		private void GetHeaderInformation(XmlDocument doc)
		{
			XmlDeclaration dec = doc.FirstChild as XmlDeclaration;
			if(dec != null)
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
		private void AddChildElements(Dictionary<string, XmlNode[]> nodesByName, Dictionary<string, ElementInfo> elementsByName, bool maintainHierarchy)
		{
			foreach (string name in nodesByName.Keys)
			{
				// Get all the child node names.
				List<string> childNames = new List<string>();
				foreach (XmlNode node in nodesByName[name])
				{
					foreach (XmlNode child in node.ChildNodes)
					{
						string childName = child.Name;
						if (child.NodeType == XmlNodeType.Element && !childNames.Contains(childName))
							childNames.Add(childName);
					}
				}

				// Sort the child names.
				childNames.Sort();

				// Build the child element array.
				List<ElementInfo> childElements = new List<ElementInfo>(childNames.Count);
				foreach (string childName in childNames)
				{
					string elementName = childName;
					if (maintainHierarchy)
						elementName = string.Format("{0}.{1}", name, childName);
					childElements.Add(elementsByName[elementName]);
				}

				// Add the array to the element.
				elementsByName[name].Children = childElements.ToArray();
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
			List<ElementInfo> nodeList = new List<ElementInfo>(Elements.Count);
			nodeList.AddRange(Elements.Values);
			return nodeList.ToArray();
		}

		public CSCodeGen.FileInfo[] GenerateClassFiles(string nameSpace)
		{
			// Add the main class.
			List<CSCodeGen.FileInfo> fileList = new List<CSCodeGen.FileInfo>();
			ClassInfo main = GenerateMainClass();
			main.AddUsing("System.Xml");
			main.AddUsing("System.IO");
			main.AddUsing("System.Security");
			fileList.Add(new CSCodeGen.FileInfo(nameSpace, main));

			if (HierarchyMaintained)
			{
				main.AddChildClass(RootNode.GenerateDataClass(HierarchyMaintained));
			}
			else
			{
				ElementInfo[] nodes = GetAllNodes();
				foreach (ElementInfo info in nodes)
					fileList.Add(new CSCodeGen.FileInfo(nameSpace, info.GenerateDataClass(HierarchyMaintained)));
			}

			return fileList.ToArray();
		}

		private ClassInfo GenerateMainClass()
		{
			string summary = string.Format("Provides the methods to import and export data to/from an XML file. The schema was taken from the {0} file.", Path.GetFileName(FilePath));
			ClassInfo info = new ClassInfo("public partial", MainClassName, null, summary);

			// Add properties for each root element.
			summary = string.Format("Contains the root {0} element in the XML file.", RootNode.Name);
			info.Properties.Add(new PropertyInfo("public", string.Format("{0}", RootNode.ClassName), "Root", summary, null, null, "private"));

			info.Methods.Add(GenerateImporterMethod());
			info.Methods.Add(GenerateExporterMethod());
			return info;
		}

		private MethodInfo GenerateImporterMethod()
		{
			// Append the signature of the method.
			string summary = "Imports data from an XML file.";
			MethodInfo method = new MethodInfo("public", "void", "ImportFromXML", summary);

			method.Parameters.Add(new ParameterInfo("string", "filePath", "Path to the XML file containing the data to be imported.", false, false));
			method.Exceptions.Add(new ExceptionInfo("ArgumentException", "<paramref name=\"filePath\"/> is an invalid file path."));
			method.Exceptions.Add(new ExceptionInfo("InvalidOperationException", "<paramref name=\"filePath\"/> could not be opened."));
			method.Exceptions.Add(new ExceptionInfo("InvalidDataException", "An error occurred while parsing the XML data."));

			method.CodeLines.Add(string.Empty);
			method.CodeLines.Add("XmlDocument doc = new XmlDocument();");
			method.CodeLines.Add("try");
			method.CodeLines.Add("{");
			method.CodeLines.Add("	doc.Load(filePath);");
			method.CodeLines.Add("}");
			method.CodeLines.Add("catch(ArgumentException e)");
			method.CodeLines.Add("{");
			method.CodeLines.Add("	throw new ArgumentException(\"filePath was not a valid XML file path.\", e);");
			method.CodeLines.Add("}");
			method.CodeLines.Add("catch(PathTooLongException e)");
			method.CodeLines.Add("{");
			method.CodeLines.Add("	throw new ArgumentException(\"filePath was not a valid XML file path.\", e);");
			method.CodeLines.Add("}");
			method.CodeLines.Add("catch(DirectoryNotFoundException e)");
			method.CodeLines.Add("{");
			method.CodeLines.Add("	throw new ArgumentException(\"filePath was not a valid XML file path.\", e);");
			method.CodeLines.Add("}");
			method.CodeLines.Add("catch(NotSupportedException e)");
			method.CodeLines.Add("{");
			method.CodeLines.Add("	throw new InvalidOperationException(\"filePath referenced a file that is in an invalid format.\", e);");
			method.CodeLines.Add("}");
			method.CodeLines.Add("catch(FileNotFoundException e)");
			method.CodeLines.Add("{");
			method.CodeLines.Add("	throw new InvalidOperationException(\"filePath referenced a file that could not be found.\", e);");
			method.CodeLines.Add("}");
			method.CodeLines.Add("catch(IOException e)");
			method.CodeLines.Add("{");
			method.CodeLines.Add("	throw new InvalidOperationException(\"filePath referenced a file that could not be opened.\", e);");
			method.CodeLines.Add("}");
			method.CodeLines.Add("catch(UnauthorizedAccessException e)");
			method.CodeLines.Add("{");
			method.CodeLines.Add("	throw new InvalidOperationException(\"filePath referenced a file that could not be opened.\", e);");
			method.CodeLines.Add("}");
			method.CodeLines.Add("catch(SecurityException e)");
			method.CodeLines.Add("{");
			method.CodeLines.Add("	throw new InvalidOperationException(\"filePath referenced a file that could not be opened.\", e);");
			method.CodeLines.Add("}");
			method.CodeLines.Add("catch(XmlException e)");
			method.CodeLines.Add("{");
			method.CodeLines.Add("	throw new InvalidOperationException(\"filePath referenced a file that does not contain valid XML.\", e);");
			method.CodeLines.Add("}");
			method.CodeLines.Add(string.Empty);

			// Add the list objects.
			method.CodeLines.Add("XmlElement root = doc.DocumentElement;");
			method.CodeLines.Add("if(root.NodeType != XmlNodeType.Element)");
			method.CodeLines.Add("	throw new InvalidDataException(\"The root node is not an element node.\");");
			method.CodeLines.Add(string.Format("if(string.Compare(root.Name, \"{0}\", false) != 0)", RootNode.Name));
			method.CodeLines.Add(string.Format("	throw new InvalidDataException(\"The root node is not a '{0}' named node.\");", RootNode.Name));
			method.CodeLines.Add(string.Format("Root = new {0}(root);", RootNode.ClassName));
			return method;
		}

		private MethodInfo GenerateExporterMethod()
		{
			string summary = "Exports data to an XML file.";
			MethodInfo method = new MethodInfo("public", "void", "ExportToXML", summary);

			method.Parameters.Add(new ParameterInfo("string", "filePath", "Path to the XML file to be written to. If file exists all contents will be destroyed.", false, false));
			method.Exceptions.Add(new ExceptionInfo("ArgumentException", "<paramref name=\"filePath\"/> is an invalid file path."));
			method.Exceptions.Add(new ExceptionInfo("InvalidOperationException", "<paramref name=\"filePath\"/> could not be opened."));

			method.CodeLines.Add("XmlDocument doc = new XmlDocument();");
			if (Version != null || Encoding != null)
			{
				string versionString = Version;
				if (versionString == null)
					versionString = string.Empty;
				string encodingString = Encoding;
				if (encodingString == null)
					encodingString = string.Empty;
				method.CodeLines.Add(string.Format("XmlDeclaration dec = doc.CreateXmlDeclaration(\"{0}\", \"{1}\", null);", versionString, encodingString));
				method.CodeLines.Add("doc.InsertBefore(dec, doc.DocumentElement);");
			}

			method.CodeLines.Add("XmlElement root = Root.CreateElement(doc);");
			method.CodeLines.Add("doc.AppendChild(root);");
			method.CodeLines.Add("doc.Save(filePath);");
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
				throw new ArgumentException(string.Format("The directory specified in codeOutputFolder ({0}) does not exist.", codeOutputFolder));

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

			XmlAttribute attrib = root.Attributes.Append(doc.CreateAttribute("MainClassName"));
			attrib.Value = MainClassName;

			// Store the additional settings from the caller.
			attrib = root.Attributes.Append(doc.CreateAttribute("OutputFolder"));
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
			foreach (ElementInfo element in Elements.Values)
				element.Save(doc, root);

			doc.Save(filePath);
		}

		public void Load(string filePath, out string outputFolder, out string nameSpace, out bool? genProject, out string projectName, out bool? genSolution, out string solutionName)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(filePath);

			outputFolder = null;
			nameSpace = null;
			genProject = null;
			projectName = null;
			genSolution = null;
			solutionName = null;

			foreach(XmlNode node in doc.ChildNodes)
			{
				if(node.NodeType == XmlNodeType.Element && string.Compare(node.Name, "save_data", true) == 0)
				{
					XmlAttribute attrib = node.Attributes["MainClassName"];
					if (attrib != null)
						MainClassName = attrib.Value;
					attrib = node.Attributes["OutputFolder"];
					if (attrib != null)
						outputFolder = attrib.Value;
					attrib = node.Attributes["NameSpace"];
					if (attrib != null)
						nameSpace = attrib.Value;
					attrib = node.Attributes["GenerateProject"];
					if (attrib != null)
					{
						bool temp;
						if (bool.TryParse(attrib.Value, out temp))
							genProject = temp;
					}
					attrib = node.Attributes["ProjectName"];
					if (attrib != null)
						projectName = attrib.Value;
					attrib = node.Attributes["GenerateSolution"];
					if (attrib != null)
					{
						bool temp;
						if (bool.TryParse(attrib.Value, out temp))
							genSolution = temp;
					}
					attrib = node.Attributes["SolutionName"];
					if (attrib != null)
						solutionName = attrib.Value;

					// Load all the element configurations.
					foreach (ElementInfo element in Elements.Values)
						element.Load(node);
					break;
				}
			}
		}

		#endregion Methods
	}
}
