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
using System.IO;
using System.Text;
using XMLToDataClass.Data;

namespace XMLToDataClass
{
	/// <summary>
	///   Defines methods used to generate a class for importing data into the data classes.
	/// </summary>
	public static class ImporterCodeGen
	{
		/// <summary>
		///   Generates the code to read the specified attribute.
		/// </summary>
		/// <param name="name">Name of the XML element associated with the attribute.</param>
		/// <param name="attrib"><see cref="AttributeInfo"/> object containing the information about the attribute.</param>
		/// <returns>String containging the parsing code.</returns>
		private static string GenerateAttributeReaderCode(string name, AttributeInfo attrib)
		{
			string ws3 = CodeGenHelper.CreateWhiteSpace(3);
			string ws4 = CodeGenHelper.CreateWhiteSpace(4);

			StringBuilder sb = new StringBuilder();
			sb.AppendLine(string.Format("{0}// Read the {1} attribute.", ws3, attrib.Name));
			sb.AppendLine(string.Format("{0}attrib = node.Attributes[\"{1}\"];", ws3, attrib.Name));
			sb.AppendLine();

			string local = CodeGenHelper.GetCamelCase(attrib.PropertyName);
			string typeString = DataTypeUtility.GetDataTypeString(attrib.AttributeType);

			if (DataTypeUtility.IsNullableType(attrib.AttributeType) && attrib.IsOptional)
				sb.AppendLine(string.Format("{0}{1}? {2};", ws3, typeString, local));
			else
				sb.AppendLine(string.Format("{0}{1} {2};", ws3, typeString, local));

			sb.AppendLine(string.Format("{0}if(attrib == null)", ws3));
			sb.AppendLine(string.Format("{0}{{", ws3));

			if (attrib.IsOptional)
				sb.AppendLine(string.Format("{0}{1} = null;", ws4, local));
			else
				sb.AppendLine(string.Format("{0}throw new InvalidDataException(\"A required XML Attribute ({1}) was not found in the XML element ({2}).\");", ws4, attrib.Name, name));

			sb.AppendLine(string.Format("{0}}}", ws3));
			sb.AppendLine(string.Format("{0}else", ws3));
			sb.AppendLine(string.Format("{0}{{", ws3));

			if (attrib.AttributeType == DataType.String)
			{
				sb.AppendLine(string.Format("{0}{1} = attrib.Value;", ws4, local));
			}
			else
			{
				sb.AppendLine(string.Format("{0}string {1}String = attrib.Value;", ws4, local));
				sb.AppendLine(string.Format("{0}if({1}String == null || {1}String.Length == 0)", ws4, local));
				sb.AppendLine(string.Format("{0}throw new InvalidDataException(\"An XML integer Attribute ({1}) contained an empty or null string in the XML element ({2}).\");", CodeGenHelper.CreateWhiteSpace(5), attrib.Name, name));
				sb.AppendLine(string.Format("{0}{1} = Parse{2}({1}String);", ws4, local, Enum.GetName(typeof(DataType), attrib.AttributeType)));
			}

			sb.AppendLine(string.Format("{0}}}", ws3));
			return sb.ToString();
		}

		/// <summary>
		///   Returns a string containing the boolean value parsing method.
		/// </summary>
		/// <returns>String containing the method.</returns>
		/// <remarks>Does not contain an empty line after the method.</remarks>
		private static string GenerateBooleanParsingMethod()
		{
			string ws2 = CodeGenHelper.CreateWhiteSpace(2);
			string ws3 = CodeGenHelper.CreateWhiteSpace(3);
			string ws4 = CodeGenHelper.CreateWhiteSpace(4);

			string summary = "Parses a boolean value from a string representation.";
			string[] parameters = new string[] { "value" };
			string[] descriptions = new string[] { "String representation of the boolean value (\"true\" or \"false\").  String is not case sensitive." };
			string returns = "True if the <i>value</i> represents a true string, false if it represents a false one.";
			string[] exceptions = new string[] { "InvalidDateException" };
			string[] eDescriptions = new string[] { "The boolean value specified is not a valid string representation." };

			StringBuilder sb = new StringBuilder();
			sb.Append(CodeGenHelper.CreateDocumentation(summary, parameters, descriptions, returns, null, exceptions, eDescriptions, 2));
			sb.AppendLine(string.Format("{0}private static bool ParseBoolean(string value)", ws2));
			sb.AppendLine(string.Format("{0}{{", ws2));
			sb.AppendLine(string.Format("{0}if (string.Compare(value, \"true\", true) == 0)", ws3));
			sb.AppendLine(string.Format("{0}return true;", ws4));
			sb.AppendLine(string.Format("{0}if (string.Compare(value, \"false\", true) == 0)", ws3));
			sb.AppendLine(string.Format("{0}return false;", ws4));
			sb.AppendLine();
			sb.AppendLine(string.Format("{0}throw new InvalidDataException(string.Format(\"The Boolean value specified ({{1}}) is not a value boolean string representation (\\\"true\\\" or \\\"false\\\").\", value));", ws3));
			sb.AppendLine(string.Format("{0}}}", ws2));
			return sb.ToString();
		}

		/// <summary>
		///   Generates the code for the data class reader methods.
		/// </summary>
		/// <param name="name">XML node name of the data class to generate the reader for.</param>
		/// <param name="attribs"><see cref="AttributeInfo"/>s associated with the XML node.</param>
		/// <param name="children">String array of the child nodes associated with this node.</param>
		/// <param name="isPrivate">True if the method is private, false if public.</param>
		/// <returns>String containing all the code for the method.</returns>
		/// <remarks>Does not put a blank line after the method.</remarks>
		private static string GenerateDataClassReadMethod(ElementInfo info, bool isPrivate)
		{
			string summary = string.Format("Reads the <i>node</i> into a <see cref=\"{0}\"/> data object.", info.ClassName);
			string[] parameters = new string[] { "node" };
			string[] descriptions = new string[] { "<see cref=\"XmlNode\"/> containing the data to extract." };
			string returns = string.Format("<see cref=\"{0}\"/> object containing the nodes' data.", info.ClassName);
			string[] exceptions = new string[2];
			string[] exceptionDescriptions = new string[2];
			exceptions[0] = "ArgumentNullException";
			exceptionDescriptions[0] = "<i>node</i> is a null reference.";
			exceptions[1] = "InvalidDataException";
			exceptionDescriptions[1] = "An error occurred while reading the data into the node, or one of it's child nodes.";

			string visibility = "public";
			if (isPrivate)
				visibility = "private";

			string ws2 = CodeGenHelper.CreateWhiteSpace(2);
			string ws3 = CodeGenHelper.CreateWhiteSpace(3);
			string ws4 = CodeGenHelper.CreateWhiteSpace(4);
			string ws5 = CodeGenHelper.CreateWhiteSpace(5);
			string methodName = GetDataClassReadMethodName(info.ClassName);
			string textName = CodeGenHelper.GetCamelCase(DataClassCodeGen.TextPropertyName);
			string dataName = CodeGenHelper.GetCamelCase(DataClassCodeGen.CDATAPropertyName);

			StringBuilder sb = new StringBuilder();
			sb.Append(CodeGenHelper.CreateDocumentation(summary, parameters, descriptions, returns, null, exceptions, exceptionDescriptions, 2));
			sb.AppendLine(string.Format("{0}{1} static {2} {3}(XmlNode node)", ws2, visibility, info.ClassName, methodName));
			sb.AppendLine(string.Format("{0}{{", ws2));
			sb.AppendLine(string.Format("{0}if(node == null)", ws3));
			sb.AppendLine(string.Format("{0}throw new ArgumentNullException(\"node\");", ws4));
			sb.AppendLine();

			if (info.Attributes.Length > 0)
			{
				sb.AppendLine(string.Format("{0}XmlAttribute attrib;", ws3));
				sb.AppendLine();

				for (int i = 0; i < info.Attributes.Length; i++)
					sb.AppendLine(GenerateAttributeReaderCode(info.Name, info.Attributes[i]));
			}

			// Parse the child nodes if needed.
			if (info.HasText || info.HasCDATA || info.Children.Length > 0)
			{
				sb.AppendLine(string.Format("{0}// Read the child objects.", ws3));
				for (int i = 0; i < info.Children.Length; i++)
					sb.AppendLine(string.Format("{0}List<{1}> {2}List = new List<{1}>();", ws3, info.Children[i].ClassName, info.Children[i].ChildArrayNameVariable));

				string typeString = DataTypeUtility.GetDataTypeString(info.TextDataType);
				if (info.HasText)
				{
					if (info.TextDataType == DataType.String)
					{
						sb.AppendLine(string.Format("{0}string {1} = null;", ws3, textName));
					}
					else
					{
						sb.AppendLine(string.Format("{0}string {1}String = null;", ws3, textName));
						if (DataTypeUtility.IsNullableType(info.TextDataType))
							sb.AppendLine(string.Format("{0}{1}? {2} = null;", ws3, typeString, textName));
						else
							sb.AppendLine(string.Format("{0}{1} {2} = null;", ws3, typeString, textName));
					}
				}

				if (info.HasCDATA)
					sb.AppendLine(string.Format("{0}string {1} = null;", ws3, dataName));

				sb.AppendLine(string.Format("{0}foreach(XmlNode child in node.ChildNodes)", ws3));
				sb.AppendLine(string.Format("{0}{{", ws3));
				for (int i = 0; i < info.Children.Length; i++)
				{
					string readMethodName = GetDataClassReadMethodName(info.Children[i].ClassName);
					sb.AppendLine(string.Format("{0}if(child.NodeType == XmlNodeType.Element && child.Name == \"{1}\")", ws4, info.Children[i].Name));
					sb.AppendLine(string.Format("{0}{1}List.Add({2}(child));", ws5, info.Children[i].ChildArrayNameVariable, readMethodName));
				}

				if(info.HasText)
				{
					sb.AppendLine(string.Format("{0}if(child.NodeType == XmlNodeType.Text)", ws4));
					sb.AppendLine(string.Format("{0}{{", ws4));
					if (info.TextDataType == DataType.String)
					{
						sb.AppendLine(string.Format("{0}{1} = child.Value;", ws5, textName));
					}
					else
					{
						sb.AppendLine(string.Format("{0}{1}String = child.Value;", ws5, textName));
						sb.AppendLine(string.Format("{0}if({1}String == null || {1}String.Length == 0)", ws5, textName));
						sb.AppendLine(string.Format("{0}throw new InvalidDataException(\"The Text component contained an empty or null string in the XML element ({1}).\");", CodeGenHelper.CreateWhiteSpace(6), info.Name));
						sb.AppendLine(string.Format("{0}{1} = Parse{2}({1}String);", ws5, textName, Enum.GetName(typeof(DataType), info.TextDataType)));
					}
					sb.AppendLine(string.Format("{0}}}", ws4));
				}

				if (info.HasCDATA)
				{
					sb.AppendLine(string.Format("{0}if(child.NodeType == XmlNodeType.CDATA)", ws4));
					sb.AppendLine(string.Format("{0}{1} = child.Value;", ws5, dataName));
				}

				sb.AppendLine(string.Format("{0}}}", ws3));
				for (int i = 0; i < info.Children.Length; i++)
					sb.AppendLine(string.Format("{0}{1}[] {2} = {2}List.ToArray();", ws3, info.Children[i].ClassName, info.Children[i].ChildArrayNameVariable));
				sb.AppendLine();
			}

			if (info.HasText && !info.TextIsOptional)
			{
				if(DataTypeUtility.IsNullableType(info.TextDataType))
					sb.AppendLine(string.Format("{0}if(!{1}.HasValue)", ws3, textName));
				else
					sb.AppendLine(string.Format("{0}if({1} == null)", ws3, textName));
				sb.AppendLine(string.Format("{0}throw new InvalidDataException(\"The Text component was not optional, but could not be found in the XML element ({1}).\");", ws4, info.Name));
			}

			// Create the call to the object constructor.
			sb.Append(string.Format("{0}return new {1}(", ws3, info.ClassName));

			// Add text parameters.
			if(info.HasText)
			{
				if(DataTypeUtility.IsNullableType(info.TextDataType) && !info.TextIsOptional)
					sb.Append(string.Format("{0}.Value", textName));
				else
					sb.Append(textName);
				if(info.HasCDATA || info.Attributes.Length > 0 || info.Children.Length > 0)
					sb.Append(", ");
			}

			// Add CDATA parameters.
			if(info.HasCDATA)
			{
				sb.Append(dataName);
				if (info.Attributes.Length > 0 || info.Children.Length > 0)
					sb.Append(", ");
			}

			// Add attribute parameters.
			for (int i = 0; i < info.Attributes.Length; i++)
			{
				sb.Append(CodeGenHelper.GetCamelCase(info.Attributes[i].PropertyName));

				if (i < info.Attributes.Length - 1 || info.Children.Length > 0)
					sb.Append(", ");
			}

			// Add child element parameters.
			for (int i = 0; i < info.Children.Length; i++)
			{
				sb.Append(info.Children[i].ChildArrayNameVariable);

				if (i < info.Children.Length - 1)
					sb.Append(", ");
			}
			sb.AppendLine(");");

			sb.AppendLine(string.Format("{0}}}", ws2));
			return sb.ToString();
		}

		/// <summary>
		///   Gets the data class's read method display name.
		/// </summary>
		/// <param name="className">Display class name of the data class.</param>
		/// <returns>String representing the data class's read method name.</returns>
		/// <exception cref="ArgumentNullException"><i>className</i> is a null reference.</exception>
		/// <exception cref="ArgumentException"><i>className</i> is an empty string.</exception>
		private static string GetDataClassReadMethodName(string className)
		{
			if (className == null)
				throw new ArgumentNullException("className");
			if (className.Length == 0)
				throw new ArgumentException("className is an empty string.");

			return string.Format("Read{0}Node", className);
		}

		/// <summary>
		///   Returns a string containing the date/time value parsing method.
		/// </summary>
		/// <returns>String containing the method.</returns>
		/// <remarks>Does not contain an empty line after the method.</remarks>
		private static string GenerateDateTimeParsingMethod()
		{
			string ws2 = CodeGenHelper.CreateWhiteSpace(2);
			string ws3 = CodeGenHelper.CreateWhiteSpace(3);
			string ws4 = CodeGenHelper.CreateWhiteSpace(4);

			string summary = "Parses a DateTime value from a string representation.";
			string[] parameters = new string[] { "value" };
			string[] descriptions = new string[] { "String representation of the DateTime value." };
			string returns = "<see cref=\"DateTime\"/> object parsed from the <i>value</i> string.";
			string[] exceptions = new string[] { "InvalidDateException" };
			string[] eDescriptions = new string[] { "The date time string specified is not a valid date time string representation." };

			StringBuilder sb = new StringBuilder();
			sb.Append(CodeGenHelper.CreateDocumentation(summary, parameters, descriptions, returns, null, exceptions, eDescriptions, 2));
			sb.AppendLine(string.Format("{0}private static DateTime ParseDateTime(string value)", ws2));
			sb.AppendLine(string.Format("{0}{{", ws2));
			sb.AppendLine(string.Format("{0}try", ws3));
			sb.AppendLine(string.Format("{0}{{", ws3));
			sb.AppendLine(string.Format("{0}return DateTime.Parse(value);", ws4));
			sb.AppendLine(string.Format("{0}}}", ws3));
			sb.AppendLine(string.Format("{0}catch (FormatException e)", ws3));
			sb.AppendLine(string.Format("{0}{{", ws3));
			sb.AppendLine(string.Format("{0}throw new InvalidDataException(string.Format(\"The DateTime value specified ({{0}}) is not a valid DateTime string representation: {{1}}.\", value, e.Message), e);", ws4));
			sb.AppendLine(string.Format("{0}}}", ws3));
			sb.AppendLine(string.Format("{0}}}", ws2));
			return sb.ToString();
		}

		/// <summary>
		///   Generates the file import method.
		/// </summary>
		/// <param name="lookup"><see cref="XMLInfo"/> object containing the node lookup information.</param>
		/// <returns>String containing the code of the file importer method.</returns>
		private static string GenerateFileImporterMethod(ElementInfo[] rootClasses)
		{
			// Append the signature of the method.
			string summary = "Imports data from an XML file.";

			List<string> parameters = new List<string>();
			List<string> descriptions = new List<string>();
			parameters.Add("filePath");
			descriptions.Add("Path to the XML file to be imported.");
			string[] camels = new string[rootClasses.Length];
			for (int i = 0; i < rootClasses.Length; i++)
			{
				camels[i] = CodeGenHelper.GetCamelCase(rootClasses[i].ClassName);
				parameters.Add(rootClasses[i].ChildArrayNameVariable);
				descriptions.Add(string.Format("Array of <see cref=\"{0}\"/> objects contained in the root node of the XML file.", rootClasses[i].ClassName));
			}

			string[] exceptions = new string[4];
			string[] exceptionDescriptions = new string[4];
			exceptions[0] = "ArgumentNullException";
			exceptionDescriptions[0] = "<i>filePath</i> is a null reference.";
			exceptions[1] = "ArgumentException";
			exceptionDescriptions[1] = "<i>filePath</i> is an invalid file path.";
			exceptions[2] = "InvalidOperationException";
			exceptionDescriptions[2] = "<i>filePath</i> could not be opened.";
			exceptions[3] = "InvalidDataException";
			exceptionDescriptions[3] = "An error occurred while parsing the XML data.";

			StringBuilder sb = new StringBuilder();
			string ws2 = CodeGenHelper.CreateWhiteSpace(2);
			sb.Append(CodeGenHelper.CreateDocumentation(summary, parameters.ToArray(), descriptions.ToArray(), null, null, exceptions, exceptionDescriptions, 2));
			sb.Append(string.Format("{0}public static void ImportFromXML(string filePath, ", ws2));
			for (int i = 0; i < rootClasses.Length; i++)
			{
				sb.Append(string.Format("out {0}[] {1}", rootClasses[i].ClassName, parameters[i + 1]));
				if (i != rootClasses.Length - 1)
					sb.Append(", ");
			}
			sb.AppendLine(")");

			string ws3 = CodeGenHelper.CreateWhiteSpace(3);
			string ws4 = CodeGenHelper.CreateWhiteSpace(4);
			sb.AppendLine(string.Format("{0}{{", ws2));
			sb.AppendLine(string.Format("{0}if(filePath == null)", ws3));
			sb.AppendLine(string.Format("{0}throw new ArgumentNullException(\"filePath\");", ws4));
			sb.AppendLine();
			sb.AppendLine(string.Format("{0}XmlDocument doc = new XmlDocument();", ws3));
			sb.AppendLine(string.Format("{0}try", ws3));
			sb.AppendLine(string.Format("{0}{{", ws3));
			sb.AppendLine(string.Format("{0}doc.Load(filePath);", ws4));
			sb.AppendLine(string.Format("{0}}}", ws3));
			sb.AppendLine(string.Format("{0}catch(ArgumentException e)", ws3));
			sb.AppendLine(string.Format("{0}{{", ws3));
			sb.AppendLine(string.Format("{0}throw new ArgumentException(\"filePath was not a valid XML file path.\", e);", ws4));
			sb.AppendLine(string.Format("{0}}}", ws3));
			sb.AppendLine(string.Format("{0}catch(PathTooLongException e)", ws3));
			sb.AppendLine(string.Format("{0}{{", ws3));
			sb.AppendLine(string.Format("{0}throw new ArgumentException(\"filePath was not a valid XML file path.\", e);", ws4));
			sb.AppendLine(string.Format("{0}}}", ws3));
			sb.AppendLine(string.Format("{0}catch(DirectoryNotFoundException e)", ws3));
			sb.AppendLine(string.Format("{0}{{", ws3));
			sb.AppendLine(string.Format("{0}throw new ArgumentException(\"filePath was not a valid XML file path.\", e);", ws4));
			sb.AppendLine(string.Format("{0}}}", ws3));
			sb.AppendLine(string.Format("{0}catch(NotSupportedException e)", ws3));
			sb.AppendLine(string.Format("{0}{{", ws3));
			sb.AppendLine(string.Format("{0}throw new InvalidOperationException(\"filePath referenced a file that is in an invalid format.\", e);", ws4));
			sb.AppendLine(string.Format("{0}}}", ws3));
			sb.AppendLine(string.Format("{0}catch(FileNotFoundException e)", ws3));
			sb.AppendLine(string.Format("{0}{{", ws3));
			sb.AppendLine(string.Format("{0}throw new InvalidOperationException(\"filePath referenced a file that could not be found.\", e);", ws4));
			sb.AppendLine(string.Format("{0}}}", ws3));
			sb.AppendLine(string.Format("{0}catch(IOException e)", ws3));
			sb.AppendLine(string.Format("{0}{{", ws3));
			sb.AppendLine(string.Format("{0}throw new InvalidOperationException(\"filePath referenced a file that could not be opened.\", e);", ws4));
			sb.AppendLine(string.Format("{0}}}", ws3));
			sb.AppendLine(string.Format("{0}catch(UnauthorizedAccessException e)", ws3));
			sb.AppendLine(string.Format("{0}{{", ws3));
			sb.AppendLine(string.Format("{0}throw new InvalidOperationException(\"filePath referenced a file that could not be opened.\", e);", ws4));
			sb.AppendLine(string.Format("{0}}}", ws3));
			sb.AppendLine(string.Format("{0}catch(SecurityException e)", ws3));
			sb.AppendLine(string.Format("{0}{{", ws3));
			sb.AppendLine(string.Format("{0}throw new InvalidOperationException(\"filePath referenced a file that could not be opened.\", e);", ws4));
			sb.AppendLine(string.Format("{0}}}", ws3));
			sb.AppendLine();

			// Add the list objects.
			for (int i = 0; i < rootClasses.Length; i++)
				sb.AppendLine(string.Format("{0}List<{1}> {2}List = new List<{1}>();", ws3, rootClasses[i].ClassName, camels[i]));
			sb.AppendLine();

			sb.AppendLine(string.Format("{0}foreach(XmlNode child in doc.ChildNodes)", ws3));
			sb.AppendLine(string.Format("{0}{{", ws3));
			sb.AppendLine(string.Format("{0}if(child.NodeType == XmlNodeType.Element)", ws4));
			sb.AppendLine(string.Format("{0}{{", ws4));

			string ws5 = CodeGenHelper.CreateWhiteSpace(5);
			string ws6 = CodeGenHelper.CreateWhiteSpace(6);
			for (int i = 0; i < rootClasses.Length; i++)
			{
				if (i == 0)
					sb.Append(string.Format("{0}if", ws5));
				else
					sb.Append(string.Format("{0}else if", ws5));

				sb.AppendLine(string.Format("(child.Name == \"{0}\")", rootClasses[i].Name));

				string methodName = GetDataClassReadMethodName(rootClasses[i].ClassName);
				sb.AppendLine(string.Format("{0}{1}List.Add({2}(child));", ws6, camels[i], methodName));
			}
			sb.AppendLine(string.Format("{0}}}", ws4));
			sb.AppendLine(string.Format("{0}}}", ws3));
			sb.AppendLine();

			sb.AppendLine(string.Format("{0}// Output the data class arrays.", ws3));
			for (int i = 0; i < rootClasses.Length; i++)
				sb.AppendLine(string.Format("{0}{1} = {2}List.ToArray();", ws3, parameters[i + 1], camels[i]));

			sb.AppendLine(string.Format("{0}}}", ws2));
			return sb.ToString();
		}

		/// <summary>
		///   Returns a string containing the floating point value parsing method.
		/// </summary>
		/// <param name="type"><see cref="DataType"/> representing a <see cref="DataType.Double"/> or <see cref="DataType.Float"/> type.</param>
		/// <returns>String containing the method.</returns>
		/// <remarks>Does not contain an empty line after the method.</remarks>
		/// <exception cref="InvalidOperationException">The <i>type</i> provided is not a recognized floating point type.</exception>
		private static string GenerateFloatParsingMethod(DataType type)
		{
			if (type != DataType.Double && type != DataType.Float)
				throw new InvalidOperationException(string.Format("An attempt was made to generate a floating point parsing method with a non-floating point type ({0}).", Enum.GetName(typeof(DataType), type)));

			string ws2 = CodeGenHelper.CreateWhiteSpace(2);
			string ws3 = CodeGenHelper.CreateWhiteSpace(3);
			string ws4 = CodeGenHelper.CreateWhiteSpace(4);
			string typeString = DataTypeUtility.GetDataTypeString(type);

			string summary = string.Format("Parses a {0} value from a string representation.", typeString);
			string[] parameters = new string[] { "value" };
			string[] descriptions = new string[] { string.Format("String representation of the {0} value.", typeString) };
			string returns = string.Format("<see cref=\"{0}\"/> object parsed from the <i>value</i> string.", typeString);
			string[] exceptions = new string[] { "InvalidDataException" };
			string[] eDescriptions = new string[] { string.Format("The {0} string sepcified is not in a valid format or is larger or smaller than the min and max values.", typeString) };

			StringBuilder sb = new StringBuilder();
			sb.Append(CodeGenHelper.CreateDocumentation(summary, parameters, descriptions, returns, null, exceptions, eDescriptions, 2));
			sb.AppendLine(string.Format("{0}private static {1} Parse{2}(string value)", ws2, typeString, Enum.GetName(typeof(DataType), type)));
			sb.AppendLine(string.Format("{0}{{", ws2));
			sb.AppendLine(string.Format("{0}try", ws3));
			sb.AppendLine(string.Format("{0}{{", ws3));
			sb.AppendLine(string.Format("{0}return {1}.Parse(value);", ws4, typeString));
			sb.AppendLine(string.Format("{0}}}", ws3));
			sb.AppendLine(string.Format("{0}catch (FormatException e)", ws3));
			sb.AppendLine(string.Format("{0}{{", ws3));
			sb.AppendLine(string.Format("{0}throw new InvalidDataException(string.Format(\"The {1} value specified ({{0}}) is not in a valid double string format: {{1}}.\", value, e.Message), e);", ws4, typeString));
			sb.AppendLine(string.Format("{0}}}", ws3));
			sb.AppendLine(string.Format("{0}catch (OverflowException e)", ws3));
			sb.AppendLine(string.Format("{0}{{", ws3));
			sb.AppendLine(string.Format("{0}throw new InvalidDataException(string.Format(\"The {1} value specified ({{0}}) was larger or smaller than a {1} value (Min: {{1}}, Max: {{2}}).\", value, {1}.MinValue.ToString(), {1}.MaxValue.ToString()), e);", ws4, typeString));
			sb.AppendLine(string.Format("{0}}}", ws3));
			sb.AppendLine(string.Format("{0}}}", ws2));
			return sb.ToString();
		}

		/// <summary>
		///   Generates the class that will import the XML file.
		/// </summary>
		/// <param name="lookup"><see cref="XMLInfo"/> containing lookup information on the XML nodes.</param>
		/// <param name="codeOutputFolder">Path to the folder that will contain the generated code file.</param>
		/// <param name="nameSpace">Namespace that the class should belong to.</param>
		/// <remarks>If the class file already exists it will be overwritten.</remarks>
		/// <exception cref="ArgumentNullException">One of the input parameters is a null reference.</exception>
		/// <exception cref="ArgumentException"><i>codeOutputFolder</i> is not valid.</exception>
		/// <exception cref="ArgumentException"><i>nameSpace</i> is an empty string.</exception>
		public static void GenerateImporterClass(XMLInfo lookup, string codeOutputFolder, string nameSpace)
		{
			if (lookup == null)
				throw new ArgumentNullException("lookup");

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

			string fileName = "Importer.cs";
			using (StreamWriter wr = new StreamWriter(Path.Combine(codeOutputFolder, fileName)))
			{
				wr.Write(CodeGenHelper.CreateFileHeader(fileName, "Imports an XML file into data objects in memory."));
				wr.WriteLine("using System;");
				wr.WriteLine("using System.Collections.Generic;");
				wr.WriteLine("using System.Globalization;");
				wr.WriteLine("using System.IO;");
				wr.WriteLine("using System.Security;");
				wr.WriteLine("using System.Xml;");
				wr.WriteLine();
				wr.WriteLine(string.Format("namespace {0}", nameSpace));
				wr.WriteLine("{");
				string summary = "Imports an <see cref=\"XmlDocument\" into the corresponding data classes.";
				string ws = CodeGenHelper.CreateWhiteSpace(1);
				wr.Write(CodeGenHelper.CreateDocumentation(summary, null, null, null, null, null, null, 1));
				wr.WriteLine(string.Format("{0}public static class Importer", ws));
				wr.WriteLine(string.Format("{0}{{", ws));

				// Determine which parsing methods should be public.
				List<ElementInfo> publicClasses = new List<ElementInfo>();
				publicClasses.AddRange(lookup.RootElements);

				// Create parsing methods for each class.
				foreach (ElementInfo el in lookup.AllElements)
				{
					bool isPrivate = true;
					if (publicClasses.Contains(el))
						isPrivate = false;

					wr.WriteLine(GenerateDataClassReadMethod(el, isPrivate));
				}

				// Add the number parsing method.
				wr.WriteLine(GenerateValueParsingMethods(lookup));

				// Add the file parsing method.
				wr.WriteLine(GenerateFileImporterMethod(lookup.RootElements));

				wr.WriteLine(string.Format("{0}}}", ws));
				wr.WriteLine("}");
			}
		}

		/// <summary>
		///   Returns a string containing the integer value parsing method.
		/// </summary>
		/// <param name="type"><see cref="DataType"/> representing the integer type.</param>
		/// <returns>String containing the method.</returns>
		/// <remarks>Does not contain an empty line after the method.</remarks>
		/// <exception cref="InvalidOperationException">The <i>type</i> provided is not a recognized integer type.</exception>
		private static string GenerateIntegerParsingMethod(DataType type)
		{
			if (type != DataType.Short && type != DataType.Int && type != DataType.Long && type != DataType.SByte &&
				type != DataType.UShort && type != DataType.UInt && type != DataType.ULong && type != DataType.Byte)
			{
				throw new InvalidOperationException(string.Format("An attempt was made to generate an integer parsing method with a non-integer type ({0}).", Enum.GetName(typeof(DataType), type)));
			}

			string typeString = DataTypeUtility.GetDataTypeString(type);
			int size = DataTypeUtility.GetIntegerMaxBitLength(type);

			string summary = string.Format("Parses the <i>value</i> string and returns the parsed integer {0} value.", typeString);
			string[] parameters = new string[] { "value" };
			string[] descriptions = new string[] { "String to be parsed." };
			string returns = string.Format("<see cref=\"{0}\"/> value parsed from the string.", typeString);
			string[] exceptions = new string[] { "InvalidDataException" };
			string[] eDescriptions = new string[] { string.Format("The {0} string provided was not valid.", typeString) };

			StringBuilder sb = new StringBuilder();
			string ws2 = CodeGenHelper.CreateWhiteSpace(2);
			string ws3 = CodeGenHelper.CreateWhiteSpace(3);
			string ws4 = CodeGenHelper.CreateWhiteSpace(4);
			string ws5 = CodeGenHelper.CreateWhiteSpace(5);
			string ws6 = CodeGenHelper.CreateWhiteSpace(6);
			string ws7 = CodeGenHelper.CreateWhiteSpace(7);
			sb.Append(CodeGenHelper.CreateDocumentation(summary, parameters, descriptions, returns, null, exceptions, eDescriptions, 2));
			sb.AppendLine(string.Format("{0}private static {1} Parse{2}(string value)", ws2, typeString, Enum.GetName(typeof(DataType), type)));
			sb.AppendLine(string.Format("{0}{{", ws2));
			sb.AppendLine(string.Format("{0}try", ws3));
			sb.AppendLine(string.Format("{0}{{", ws3));
			sb.AppendLine(string.Format("{0}// Check if the number is specified as a hexedecimal number.", ws4));
			sb.AppendLine(string.Format("{0}if (value.Length > 2 && value[0] == '0' && char.ToLower(value[1]) == 'x')", ws4));
			sb.AppendLine(string.Format("{0}return {1}.Parse(value.Substring(2), NumberStyles.AllowHexSpecifier);", ws5, typeString));
			sb.AppendLine();
			sb.AppendLine(string.Format("{0}// Check if the number is a hexedecimal number.", ws4));
			sb.AppendLine(string.Format("{0}if (char.ToLower(value[value.Length - 1]) == 'h')", ws4));
			sb.AppendLine(string.Format("{0}return {1}.Parse(value.Substring(0, value.Length - 1), NumberStyles.AllowHexSpecifier);", ws5, typeString));
			sb.AppendLine();
			sb.AppendLine(string.Format("{0}// Check if the number is a binary number.", ws4));
			sb.AppendLine(string.Format("{0}if (char.ToLower(value[value.Length - 1]) == 'b')", ws4));
			sb.AppendLine(string.Format("{0}{{", ws4));
			sb.AppendLine(string.Format("{0}if (value.Length - 1 > {1})", ws5, size));
			sb.AppendLine(string.Format("{0}throw new InvalidDataException(string.Format(\"The {1} value specified ({{0}}) was determined to be a binary type but had more bits ({{1}}) than can be contained in the {1} ({2}).\", value, value.Length));", ws6, typeString, size));
			sb.AppendLine();
			sb.AppendLine(string.Format("{0}string bitString = value.Substring(0, value.Length - 1);", ws5));
			sb.AppendLine(string.Format("{0}{1} intValue = 0;", ws5, typeString));
			sb.AppendLine(string.Format("{0}for (int i = 0; i < bitString.Length; i++)", ws5));
			sb.AppendLine(string.Format("{0}{{", ws5));
			sb.AppendLine(string.Format("{0}if (bitString[i] == '1')", ws6));
			sb.AppendLine(string.Format("{0}{{", ws6));
			sb.AppendLine(string.Format("{0}intValue *= 2;", ws7));
			sb.AppendLine(string.Format("{0}intValue += 1;", ws7));
			sb.AppendLine(string.Format("{0}}}", ws6));
			sb.AppendLine(string.Format("{0}else if (bitString[i] == '0')", ws6));
			sb.AppendLine(string.Format("{0}{{", ws6));
			sb.AppendLine(string.Format("{0}intValue *= 2;", ws7));
			sb.AppendLine(string.Format("{0}}}", ws6));
			sb.AppendLine(string.Format("{0}else", ws6));
			sb.AppendLine(string.Format("{0}{{", ws6));
			sb.AppendLine(string.Format("{0}throw new InvalidDataException(string.Format(\"The {1} value specified ({{0}}) in binary format contained a bit value ({{1}}) at index {{2}} that was not a 0 or 1.\", value, bitString[i], i));", ws7, typeString));
			sb.AppendLine(string.Format("{0}}}", ws6));
			sb.AppendLine(string.Format("{0}}}", ws5));
			sb.AppendLine(string.Format("{0}return intValue;", ws5));
			sb.AppendLine(string.Format("{0}}}", ws4));
			sb.AppendLine();
			sb.AppendLine(string.Format("{0}// Attempt to parse the number as just an integer.", ws4));
			sb.AppendLine(string.Format("{0}return {1}.Parse(value, NumberStyles.Integer);", ws4, typeString));
			sb.AppendLine(string.Format("{0}}}", ws3));
			sb.AppendLine(string.Format("{0}catch (FormatException e)", ws3));
			sb.AppendLine(string.Format("{0}{{", ws3));
			sb.AppendLine(string.Format("{0}throw new InvalidDataException(string.Format(\"The {1} value specified ({{0}}) is not in a valid {1} string format: {{1}}.\", value, e.Message), e);", ws4, typeString));
			sb.AppendLine(string.Format("{0}}}", ws3));
			sb.AppendLine(string.Format("{0}catch (OverflowException e)", ws3));
			sb.AppendLine(string.Format("{0}{{", ws3));
			sb.AppendLine(string.Format("{0}throw new InvalidDataException(string.Format(\"The {1} value specified ({{0}}) was larger or smaller than a {1} value (Min: {{1}}, Max: {{2}}).\", value, {1}.MinValue.ToString(), {1}.MaxValue.ToString()), e);", ws4, typeString));
			sb.AppendLine(string.Format("{0}}}", ws3));
			sb.AppendLine(string.Format("{0}}}", ws2));
			return sb.ToString();
		}

		/// <summary>
		///   Gets a string containing the parsing method corresponding to the specified <i>type</i>.
		/// </summary>
		/// <param name="type"><see cref="DataType"/> to get the method for.</param>
		/// <returns>String containing the method.</returns>
		/// <remarks>The return string contains just the method and not an emtpy line after.</remarks>
		private static string GenerateValueParsingMethod(DataType type)
		{
			switch (type)
			{
				case DataType.Boolean:
					return GenerateBooleanParsingMethod();
				case DataType.DateTime:
					return GenerateDateTimeParsingMethod();
				case DataType.Double:
				case DataType.Float:
					return GenerateFloatParsingMethod(type);
				case DataType.Short:
				case DataType.Int:
				case DataType.Long:
				case DataType.SByte:
				case DataType.UShort:
				case DataType.UInt:
				case DataType.ULong:
				case DataType.Byte:
					return GenerateIntegerParsingMethod(type);
				default:
					return string.Empty;
			}
		}

		/// <summary>
		///   Generates the code for the generic number reader method.
		/// </summary>
		/// <returns>String containing all the code for the method.</returns>
		/// <remarks>Does not put a blank line after the method.</remarks>
		private static string GenerateValueParsingMethods(XMLInfo lookup)
		{
			StringBuilder sb = new StringBuilder();
			DataType[] valArray = lookup.GetAllDataTypes();
			foreach (DataType type in valArray)
				sb.AppendLine(GenerateValueParsingMethod(type));
			return sb.ToString();
		}
	}
}
