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
	///   Defines methods used to generate a class for exporting the data objects to an XML file.
	/// </summary>
	public static class ExporterCodeGen
	{
		private static string GenerateAttributeWriterCode(string name, AttributeInfo attrib)
		{
			return "";
		}

		/// <summary>
		///   Returns a string containing the boolean value writing method.
		/// </summary>
		/// <returns>String containing the method.</returns>
		/// <remarks>Does not contain an empty line after the method.</remarks>
		private static string GenerateBooleanWritingMethod()
		{
			string ws2 = CodeGenHelper.CreateWhiteSpace(2);
			string ws3 = CodeGenHelper.CreateWhiteSpace(3);
			string ws4 = CodeGenHelper.CreateWhiteSpace(4);

			string summary = "Writes a boolean value to a string representation.";
			string[] parameters = new string[] { "value" };
			string[] descriptions = new string[] { "Boolean value." };
			string returns = "String representation of the boolean value (\"true\" or \"false\").";

			StringBuilder sb = new StringBuilder();
			sb.Append(CodeGenHelper.CreateDocumentation(summary, parameters, descriptions, returns, null, null, null, 2));
			sb.AppendLine(string.Format("{0}private static string WriteBoolean(bool value)", ws2));
			sb.AppendLine(string.Format("{0}{{", ws2));
			sb.AppendLine(string.Format("{0}if (value)", ws3));
			sb.AppendLine(string.Format("{0}return \"true\";", ws4));
			sb.AppendLine(string.Format("{0}return \"false\";", ws3));
			sb.AppendLine(string.Format("{0}}}", ws2));
			return sb.ToString();
		}

		/// <summary>
		///   Returns a string containing the date/time value writing method.
		/// </summary>
		/// <returns>String containing the method.</returns>
		/// <remarks>Does not contain an empty line after the method.</remarks>
		private static string GenerateDateTimeWritingMethod()
		{
			string ws2 = CodeGenHelper.CreateWhiteSpace(2);
			string ws3 = CodeGenHelper.CreateWhiteSpace(3);
			string ws4 = CodeGenHelper.CreateWhiteSpace(4);

			string summary = "Writes a DateTime value to a string representation.";
			string[] parameters = new string[] { "value" };
			string[] descriptions = new string[] { "<see cref=\"DateTime\"/> object." };
			string returns = "String representation of the DateTime value.";

			StringBuilder sb = new StringBuilder();
			sb.Append(CodeGenHelper.CreateDocumentation(summary, parameters, descriptions, returns, null, null, null, 2));
			sb.AppendLine(string.Format("{0}private static string WriteDateTime(DateTime value)", ws2));
			sb.AppendLine(string.Format("{0}{{", ws2));
			sb.AppendLine(string.Format("{0}return value.ToString();", ws3));
			sb.AppendLine(string.Format("{0}}}", ws2));
			return sb.ToString();
		}

		/// <summary>
		///   Returns a string containing the floating point value writing method.
		/// </summary>
		/// <param name="type"><see cref="DataType"/> representing a <see cref="DataType.Double"/> or <see cref="DataType.Float"/> type.</param>
		/// <returns>String containing the method.</returns>
		/// <remarks>Does not contain an empty line after the method.</remarks>
		/// <exception cref="InvalidOperationException">The <i>type</i> provided is not a recognized floating point type.</exception>
		private static string GenerateFloatWritingMethod(DataType type)
		{
			if (type != DataType.Double && type != DataType.Float)
				throw new InvalidOperationException(string.Format("An attempt was made to generate a floating point parsing method with a non-floating point type ({0}).", Enum.GetName(typeof(DataType), type)));

			string ws2 = CodeGenHelper.CreateWhiteSpace(2);
			string ws3 = CodeGenHelper.CreateWhiteSpace(3);
			string typeString = DataTypeUtility.GetDataTypeString(type);

			string summary = string.Format("Writes a {0} value to a string representation.", typeString);
			string[] parameters = new string[] { "value" };
			string[] descriptions = new string[] { string.Format("<see cref=\"{0}\"/> object.", typeString) };
			string returns = string.Format("String representation of the {0} value.", typeString);

			StringBuilder sb = new StringBuilder();
			sb.Append(CodeGenHelper.CreateDocumentation(summary, parameters, descriptions, returns, null, null, null, 2));
			sb.AppendLine(string.Format("{0}private static string Write{2}({1} value)", ws2, typeString, Enum.GetName(typeof(DataType), type)));
			sb.AppendLine(string.Format("{0}{{", ws2));
			sb.AppendLine(string.Format("{0}value.ToString()", ws3));
			sb.AppendLine(string.Format("{0}}}", ws2));
			return sb.ToString();
		}

		/// <summary>
		///   Generates the class that will export to an XML file.
		/// </summary>
		/// <param name="lookup"><see cref="XMLInfo"/> containing lookup information on the XML nodes.</param>
		/// <param name="codeOutputFolder">Path to the folder that will contain the generated code file.</param>
		/// <param name="nameSpace">Namespace that the class should belong to.</param>
		/// <remarks>If the class file already exists it will be overwritten.</remarks>
		/// <exception cref="ArgumentNullException">One of the input parameters is a null reference.</exception>
		/// <exception cref="ArgumentException"><i>codeOutputFolder</i> is not valid.</exception>
		/// <exception cref="ArgumentException"><i>nameSpace</i> is an empty string.</exception>
		public static void GenerateExporterClass(XMLInfo lookup, string codeOutputFolder, string nameSpace)
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

			string fileName = "Exporter.cs";
			using (StreamWriter wr = new StreamWriter(Path.Combine(codeOutputFolder, fileName)))
			{
				wr.Write(CodeGenHelper.CreateFileHeader(fileName, "Exports the data objects to an XML file."));
				wr.WriteLine("using System;");
				wr.WriteLine("using System.Collections.Generic;");
				wr.WriteLine("using System.Globalization;");
				wr.WriteLine("using System.IO;");
				wr.WriteLine("using System.Security;");
				wr.WriteLine("using System.Xml;");
				wr.WriteLine();
				wr.WriteLine(string.Format("namespace {0}", nameSpace));
				wr.WriteLine("{");
				string summary = "Exports the corresponding data classes to an <see cref=\"XmlDocument\".";
				string ws = CodeGenHelper.CreateWhiteSpace(1);
				wr.Write(CodeGenHelper.CreateDocumentation(summary, null, null, null, null, null, null, 1));
				wr.WriteLine(string.Format("{0}public static class Exporter", ws));
				wr.WriteLine(string.Format("{0}{{", ws));

				// Determine which writing methods should be public.
				List<ElementInfo> publicClasses = new List<ElementInfo>();
				publicClasses.AddRange(lookup.RootElements);

				// Create writing methods for each class.
				foreach (ElementInfo el in lookup.AllElements)
				{
					bool isPrivate = true;
					if (publicClasses.Contains(el))
						isPrivate = false;

					//wr.WriteLine(GenerateDataClassReadMethod(el, isPrivate));
				}

				// Add the number parsing method.
				wr.WriteLine(GenerateValueWritingMethods(lookup));

				// Add the file parsing method.
				//wr.WriteLine(GenerateFileImporterMethod(lookup.RootElements));

				wr.WriteLine(string.Format("{0}}}", ws));
				wr.WriteLine("}");
			}
		}

		/// <summary>
		///   Returns a string containing the integer value writing method.
		/// </summary>
		/// <param name="type"><see cref="DataType"/> representing the integer type.</param>
		/// <returns>String containing the method.</returns>
		/// <remarks>Does not contain an empty line after the method.</remarks>
		/// <exception cref="InvalidOperationException">The <i>type</i> provided is not a recognized integer type.</exception>
		private static string GenerateIntegerWritingMethod(DataType type)
		{
			if (type != DataType.Short && type != DataType.Int && type != DataType.Long && type != DataType.SByte &&
				type != DataType.UShort && type != DataType.UInt && type != DataType.ULong && type != DataType.Byte)
			{
				throw new InvalidOperationException(string.Format("An attempt was made to generate an integer parsing method with a non-integer type ({0}).", Enum.GetName(typeof(DataType), type)));
			}

			string typeString = DataTypeUtility.GetDataTypeString(type);
			string summary = string.Format("Writes the <i>value</i> to a string representation of the {0}.", typeString);
			string[] parameters = new string[] { "value", "inHex" };
			string[] descriptions = new string[] { "<see cref=\"{0}\"/> value.", "True if the value should be written as a hex value, false otherwise" };
			string returns = string.Format("String representation.", typeString);

			StringBuilder sb = new StringBuilder();
			string ws2 = CodeGenHelper.CreateWhiteSpace(2);
			string ws3 = CodeGenHelper.CreateWhiteSpace(3);
			string ws4 = CodeGenHelper.CreateWhiteSpace(4);
			string ws5 = CodeGenHelper.CreateWhiteSpace(5);
			string ws6 = CodeGenHelper.CreateWhiteSpace(6);
			string ws7 = CodeGenHelper.CreateWhiteSpace(7);
			sb.Append(CodeGenHelper.CreateDocumentation(summary, parameters, descriptions, returns, null, null, null, 2));
			sb.AppendLine(string.Format("{0}private static string Write{2}({1} value, bool inHex)", ws2, typeString, Enum.GetName(typeof(DataType), type)));
			sb.AppendLine(string.Format("{0}{{", ws2));
			sb.AppendLine(string.Format("{0}if(inHex)", ws3));
			sb.AppendLine(string.Format("{0}return string.Format(\"0x{{0}}\", value.ToString(\"X\"));", ws4));
			sb.AppendLine(string.Format("{0}else", ws3));
			sb.AppendLine(string.Format("{0}return value.ToString();", ws4));
			sb.AppendLine(string.Format("{0}}}", ws2));
			return sb.ToString();
		}

		/// <summary>
		///   Gets a string containing the writing method corresponding to the specified <i>type</i>.
		/// </summary>
		/// <param name="type"><see cref="DataType"/> to get the method for.</param>
		/// <returns>String containing the method.</returns>
		/// <remarks>The return string contains just the method and not an emtpy line after.</remarks>
		private static string GenerateValueWritingMethod(DataType type)
		{
			switch (type)
			{
				case DataType.Boolean:
					return GenerateBooleanWritingMethod();
				case DataType.DateTime:
					return GenerateDateTimeWritingMethod();
				case DataType.Double:
				case DataType.Float:
					return GenerateFloatWritingMethod(type);
				case DataType.Short:
				case DataType.Int:
				case DataType.Long:
				case DataType.SByte:
				case DataType.UShort:
				case DataType.UInt:
				case DataType.ULong:
				case DataType.Byte:
					return GenerateIntegerWritingMethod(type);
				default:
					return string.Empty;
			}
		}

		/// <summary>
		///   Generates the code for the generic number writer method.
		/// </summary>
		/// <returns>String containing all the code for the method.</returns>
		/// <remarks>Does not put a blank line after the method.</remarks>
		private static string GenerateValueWritingMethods(XMLInfo lookup)
		{
			StringBuilder sb = new StringBuilder();
			DataType[] valArray = lookup.GetAllDataTypes();
			foreach (DataType type in valArray)
				sb.AppendLine(GenerateValueWritingMethod(type));
			return sb.ToString();
		}
	}
}
