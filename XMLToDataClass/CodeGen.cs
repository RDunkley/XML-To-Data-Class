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
using System.IO;
using System.Security;
using System.Xml;
using XMLToDataClass.Data;

namespace XMLToDataClass
{
	/// <summary>
	///   Used to generate data classes from XML documents.
	/// </summary>
	public static class CodeGen
	{
		#region Methods

		/// <summary>
		///   Generates the code classes in the specified folder.
		/// </summary>
		/// <param name="lookup"><see cref="XMLInfo"/> object containing the XML file information.</param>
		/// <param name="codeOutputFolder">Folder for the code files to be generated in.</param>
		/// <exception cref="ArgumentNullException"><i>lookup</i> or <i>codeOutputFolder</i> is a null reference.</exception>
		/// <exception cref="ArgumentException"><i>codeOutputFolder</i> is a non-existing folder or invalid folder path.</exception>
		/// <remarks>All code files with the same name in <i>codeOutputFolder</i> as the newly generated classes will be overwritten.</remarks>
		public static void GenerateCodeClasses(XMLInfo lookup, string codeOutputFolder)
		{
			if (lookup == null)
				throw new ArgumentNullException("lookup");
			if (codeOutputFolder == null)
				throw new ArgumentNullException("codeOutputFolder");

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

			// Create all the data classes.
			foreach (ElementInfo el in lookup.AllElements)
				DataClassCodeGen.GenerateDataClass(el, codeOutputFolder);

			// Create the main importer class.
			ImporterCodeGen.GenerateImporterClass(lookup, codeOutputFolder);

			// Create the main exporter class.
			ExporterCodeGen.GenerateExporterClass(lookup, codeOutputFolder);
		}

		/// <summary>
		///   Parses the specified XML file and generates code in the specified output folder.
		/// </summary>
		/// <param name="filePath">XML file to generate the code from.</param>
		/// <exception cref="ArgumentNullException"><i>filePath</i> is a null reference.</exception>
		/// <exception cref="ArgumentException"><i>filePath</i> is an empty string.</exception>
		/// <exception cref="InvalidDataException">An error occurred while parsing the XML file's data.</exception>
		public static XMLInfo ParseXML(string filePath)
		{
			if(filePath == null)
				throw new ArgumentNullException("filePath");
			if(filePath.Length == 0)
				throw new ArgumentException("filePath is an empty string.");

			XmlDocument doc = new XmlDocument();
			try
			{
				doc.Load(filePath);
			}
			catch (XmlException e)
			{
				throw new InvalidDataException(string.Format("The XML file specified ({0}) does not contain valid XML.", filePath), e);
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

			// Generate the lookup tables.
			return new XMLInfo(doc);
		}

		#endregion Methods
	}
}
