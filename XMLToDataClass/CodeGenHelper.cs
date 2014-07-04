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
using System.Text;

namespace XMLToDataClass
{
	/// <summary>
	///   Helper class for generating C# code.
	/// </summary>
	public static class CodeGenHelper
	{
		#region Properties

		/// <summary>
		///   Maximum number of columns per line.
		/// </summary>
		public static int MAX_COL_IN_LINE { get; set; }

		/// <summary>
		///   Number of spaces per tab.
		/// </summary>
		public static int SPACES_PER_TAB { get; set; }

		/// <summary>
		///   True if tabs should be used.  False if spaces should be used.
		/// </summary>
		public static bool USE_TABS { get; set; }

		#endregion Properties

		#region Methods

		/// <summary>
		///   Static constructor for the <see cref="CodeGenHelper"/> class which initializes some default values.
		/// </summary>
		static CodeGenHelper()
		{
			// Set default configuration settings.
			MAX_COL_IN_LINE = 129;
			SPACES_PER_TAB = 4;
			USE_TABS = true;
		}

		/// <summary>
		///   Adds a comment string to the end of the line.
		/// </summary>
		/// <param name="text">Comment string to be added.</param>
		/// <param name="initialOffset">Character column offset from the left side of the page.</param>
		/// <param name="newLineText">If the comment is too long then this string is used to specify what text to place before the comment starts up again.</param>
		/// <returns>String containing the comment.</returns>
		/// <remarks>
		///   If the line is longer than <see cref="MAX_COL_IN_LINE"/> then it will be split to multiple lines.  If a 
		///   space could not be found on the string to break it up on then the string will be broken on the boundary 
		///   and a hyphen added.
		/// </remarks>
		public static string AddCommentToEndOfLine(string text, int initialOffset, string newLineText)
		{
			if (text == null)
				return string.Empty;
			if (newLineText == null)
				newLineText = string.Empty;

			if (initialOffset < 0)
				initialOffset = 0;

			StringBuilder sb = new StringBuilder();

			int index = 0;
			string addText = string.Empty;
			while (index < text.Length - 1)
			{
				int length = text.Length - index;
				if (length > MAX_COL_IN_LINE - initialOffset)
				{
					int lineLength = MAX_COL_IN_LINE - initialOffset;
					length = text.LastIndexOf(" ", index + lineLength, lineLength);
					if (length == -1)
					{
						// Break the file on the column mark and place a hyphen.
						length = MAX_COL_IN_LINE - initialOffset - 1;
						sb.Append(text.Substring(index, length));
						sb.AppendLine("-");
					}
					else
					{
						length -= index;
						addText = string.Empty;
						sb.AppendLine(text.Substring(index, length).Trim());
					}

					initialOffset = newLineText.Length;
					sb.Append(newLineText);
				}
				else
				{
					sb.AppendLine(text.Substring(index, length).Trim());
				}

				index += length;
			}

			return sb.ToString();
		}

		/// <summary>
		///   Generates the <see cref="ArgumentNullException"/> exception description string for parameters that cannot be null in the data class constructor.
		/// </summary>
		/// <param name="parameters">Array of parameters names that will throw the exception if null.</param>
		/// <returns>String containing the exception string.</returns>
		/// <exception cref="ArgumentNullException"><i>parameters</i> is a null reference.</exception>
		/// <exception cref="ArgumentException"><i>parameters</i> is an empty array.</exception>
		/// <remarks>The return string does contain end-of-line characters so the code can be inserted into strings as is.</remarks>
		public static string CreateArgumentNullExceptionString(string[] parameters)
		{
			if (parameters == null)
				throw new ArgumentNullException("parameters");
			if (parameters.Length == 0)
				throw new ArgumentException("parameters is an empty array");

			StringBuilder errorMsg = new StringBuilder();
			if (parameters.Length == 1)
			{
				errorMsg.Append(string.Format("<i>{0}</i> is a null reference.", parameters[0]));
				return errorMsg.ToString();
			}

			for (int i = 0; i < parameters.Length; i++)
			{
				errorMsg.Append(string.Format("<i>{0}</i>", parameters[i]));
				if (i == parameters.Length - 2)
					errorMsg.Append(", or ");
				else if (i == parameters.Length - 1)
					errorMsg.Append(" ");
				else
					errorMsg.Append(", ");
			}
			errorMsg.Append("is a null reference.");
			return errorMsg.ToString();
		}

		/// <summary>
		///   Generates the <see cref="ArgumentException"/> exception string for string parameters that cannot be empty in the data class constructor.
		/// </summary>
		/// <param name="parameters">Array of parameters names that will throw the exception if null.</param>
		/// <returns>String containing the exception string.</returns>
		/// <exception cref="ArgumentNullException"><i>parameters</i> is a null reference.</exception>
		/// <exception cref="ArgumentException"><i>parameters</i> is an empty array.</exception>
		/// <remarks>The return string does contain end-of-line characters so the code can be inserted into strings as is.</remarks>
		public static string CreateArgumentEmptyExceptionString(string[] parameters)
		{
			if (parameters == null)
				throw new ArgumentNullException("parameters");
			if (parameters.Length == 0)
				throw new ArgumentException("parameters is an empty array");

			StringBuilder errorMsg = new StringBuilder();
			if (parameters.Length == 1)
			{
				errorMsg.Append(string.Format("<i>{0}</i> is an empty string.", parameters[0]));
				return errorMsg.ToString();
			}

			for (int i = 0; i < parameters.Length; i++)
			{
				errorMsg.Append(string.Format("<i>{0}</i>", parameters[i]));
				if (i == parameters.Length - 2)
					errorMsg.Append(", or ");
				else if (i == parameters.Length - 1)
					errorMsg.Append(" ");
				else
					errorMsg.Append(", ");
			}
			errorMsg.Append("is an empty string.");
			return errorMsg.ToString();
		}

		/// <summary>
		///   Create documentation for a Field, Property, Event, Method, delegate, etc.
		/// </summary>
		/// <param name="summary">Summary to be placed in the summary portion of the documentation.</param>
		/// <param name="parameters">Array of parameters for the Method. Can be null.</param>
		/// <param name="parameterDescriptions">Description of the parameters for the Method. Must be array of same size as <i>parameters</i>. Can be null if <i>parameters</i> is null.</param>
		/// <param name="returns">String to be placed in the returns section of the documentation. Can be null.</param>
		/// <param name="remarks">String to be placed in the remarsk section of the documentation. Can be null.</param>
		/// <param name="numTabs">Number of tabs to place before the documentation starts.</param>
		/// <returns>String containing the documentation, including the final carriage return/line feed.</returns>
		/// <remarks>Any of the parameters that are null will be left out of the documentation.</remarks>
		public static string CreateDocumentation(string summary, string[] parameters, string[] parameterDescriptions, string returns, string remarks, string[] exceptions, string[] exceptionDescriptions, int numTabs)
		{
			if (summary == null)
				throw new ArgumentNullException("summary");
			if (parameters != null)
			{
				if (parameterDescriptions == null)
					throw new ArgumentNullException("parameterDescriptions");
				if (parameters.Length != parameterDescriptions.Length)
					throw new ArgumentException(string.Format("The length of the parameterDescriptions array specified ({0}) does not match the length of the parameters array specified ({1})", parameterDescriptions.Length, parameters.Length));
			}
			if (exceptions != null)
			{
				if (exceptionDescriptions == null)
					throw new ArgumentNullException("exceptionDescriptions");
				if (exceptions.Length != exceptionDescriptions.Length)
					throw new ArgumentException(string.Format("The length of the exceptionDescriptions array specified ({0}) does not match the length of the exceptions array specified ({1})", exceptionDescriptions.Length, exceptions.Length));
			}
			if (numTabs < 0)
				numTabs = 0;

			StringBuilder sb = new StringBuilder();
			sb.Append(CreateGeneralDocumentationElement("summary", summary, numTabs));
			if (parameters != null)
			{
				for (int i = 0; i < parameters.Length; i++)
					sb.Append(CreateParamDocumentationElement(parameters[i], parameterDescriptions[i], numTabs));
			}
			if (returns != null)
				sb.Append(CreateGeneralDocumentationElement("returns", returns, numTabs));
			if (remarks != null)
				sb.Append(CreateGeneralDocumentationElement("remarks", remarks, numTabs));
			if (exceptions != null)
			{
				for (int i = 0; i < exceptions.Length; i++)
					sb.Append(CreateExceptionDocumentationElement(exceptions[i], exceptionDescriptions[i], numTabs));
			}

			return sb.ToString();
		}

		/// <summary>
		///   Creates a exception documentation XML element.
		/// </summary>
		/// <param name="exceptionName">Name of the exception.</param>
		/// <param name="description">Description of when the exception will occur.</param>
		/// <param name="numTabs">Number of tabs to place before the documentation starts.</param>
		/// <exception cref="ArgumentException"></exception>
		/// <returns>String containing the XML element documentation, including the carriage return/line feed.</returns>
		private static string CreateExceptionDocumentationElement(string exceptionName, string description, int numTabs)
		{
			StringBuilder sb = new StringBuilder();
			string ws = CreateWhiteSpace(numTabs);
			string newLine = string.Format("{0}///   ", ws);
			if (MAX_COL_IN_LINE - ws.Length - 36 - exceptionName.Length < description.Length)
			{
				// Place the tags on a separate line.
				sb.AppendLine(string.Format("{0}/// <exception cref=\"{1}\">", ws, exceptionName));
				sb.Append(newLine);
				sb.Append(AddCommentToEndOfLine(description, newLine.Length, newLine));
				sb.AppendLine(string.Format("{0}/// </exception>", ws));
			}
			else
			{
				// Place on the same line.
				sb.AppendLine(string.Format("{0}/// <exception cref=\"{1}\">{2}</exception>", ws, exceptionName, description));
			}
			return sb.ToString();
		}

		/// <summary>
		///   Creates a file header based on the filename.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="description">Description of the file.</param>
		/// <returns>String containing the file header, including line ending and carriage return.</returns>
		public static string CreateFileHeader(string fileName, string description)
		{
			string newLine = "///              ";
			StringBuilder sb = new StringBuilder();
			sb.Append("/// Filename:    ");
			sb.Append(AddCommentToEndOfLine(fileName, 17, newLine));
			sb.Append("/// Description: ");
			sb.Append(AddCommentToEndOfLine(description, 17, newLine));
			return sb.ToString();
		}

		/// <summary>
		///   Creates a documentation XML element.
		/// </summary>
		/// <param name="tagName">Name of the XML element tag.</param>
		/// <param name="text">Text contained inside the XML element.</param>
		/// <param name="numTabs">Number of tabs to place before the documentation starts.</param>
		/// <returns>String containing the XML element documentation, including the carriage return/line feed.</returns>
		private static string CreateGeneralDocumentationElement(string tagName, string text, int numTabs)
		{
			StringBuilder sb = new StringBuilder();
			string ws = CreateWhiteSpace(numTabs);
			string newLine = string.Format("{0}///   ", ws);
			if (MAX_COL_IN_LINE - ws.Length - 9 - (2*tagName.Length) < text.Length)
			{
				// Place the tags on a separate line.
				sb.AppendLine(string.Format("{0}/// <{1}>", ws, tagName));
				sb.Append(newLine);
				sb.AppendLine(AddCommentToEndOfLine(text, newLine.Length, newLine));
				sb.AppendLine(string.Format("{0}/// </{1}>", ws, tagName));
			}
			else
			{
				// Place on the same line.
				sb.AppendLine(string.Format("{0}/// <{1}>{2}</{1}>", ws, tagName, text));
			}
			return sb.ToString();
		}

		/// <summary>
		///   Creates a parameter documentation XML element.
		/// </summary>
		/// <param name="paramName">Name of the parameter.</param>
		/// <param name="description">Description of the parameter.</param>
		/// <param name="numTabs">Number of tabs to place before the documentation starts.</param>
		/// <returns>String containing the XML element documentation, including the carriage return/line feed.</returns>
		private static string CreateParamDocumentationElement(string paramName, string description, int numTabs)
		{
			StringBuilder sb = new StringBuilder();
			string ws = CreateWhiteSpace(numTabs);
			string newLine = string.Format("{0}///   ", ws);
			if (MAX_COL_IN_LINE - ws.Length - 27 - paramName.Length < description.Length)
			{
				// Place the tags on a separate line.
				sb.AppendLine(string.Format("{0}/// <param name=\"{1}\">", ws, paramName));
				sb.Append(newLine);
				sb.AppendLine(AddCommentToEndOfLine(description, newLine.Length, newLine));
				sb.AppendLine(string.Format("{0}/// </param>", ws));
			}
			else
			{
				// Place on the same line.
				sb.AppendLine(string.Format("{0}/// <param name=\"{1}\">{2}</param>", ws, paramName, description));
			}
			return sb.ToString();
		}

		/// <summary>
		///   Gets a string representing the whitespace for the specified number of tabs.
		/// </summary>
		/// <param name="numberOfTabs">Number of tabs to generate the whitespace to cover.  If this parameter is less than 1 than an empty string is returned.</param>
		/// <returns>String representing the white space of tab size specified.</returns>
		/// <remarks>If <see cref="USE_TABS"/> is true then this method returns the whitespace in tabs, otherwise it returns it in spaces according to <see cref="SPACES_PER_TAB"/>.</remarks>
		public static string CreateWhiteSpace(int numberOfTabs)
		{
			if (numberOfTabs < 1)
				return string.Empty;

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < numberOfTabs; i++)
			{
				if (USE_TABS)
				{
					sb.Append("	");
				}
				else
				{
					for (int j = 0; j < SPACES_PER_TAB; j++)
						sb.Append(" ");
				}
			}
			return sb.ToString();
		}

		/// <summary>
		///   Gets the camel case of the provided name.
		/// </summary>
		/// <param name="name">Name to convert to camel case.</param>
		/// <returns>Name in camel case.</returns>
		/// <exception cref="ArgumentNullException"><i>name</i> is a null reference.</exception>
		/// <exception cref="ArgumentException"><i>name</i> is an empty string.</exception>
		/// <remarks>
		///   This method will also remove '_' characters and treat them as word breaks. For example, this_word would go to 
		///   thisWord.  It also modifies words that could conflict with C# keywords such as "default" and changes them to "defaultValue".
		/// </remarks>
		public static string GetCamelCase(string name)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (name.Length == 0)
				throw new ArgumentException("name is an empty string.");

			//StringBuilder builder = new StringBuilder();
			//bool capNext = false;
			//for (int i = 0; i < name.Length; i++)
			//{
			//	if (name[i] == '_')
			//	{
			//		capNext = true;
			//	}
			//	else
			//	{
			//		if (capNext)
			//		{
			//			builder.Append(char.ToUpper(name[i]));
			//			capNext = false;
			//		}
			//		else
			//			builder.Append(name[i]);
			//	}
			//}
			//
			//string value = builder.ToString();
			if (name.Length == 1)
				return name.ToLower();

			string value = string.Format("{0}{1}", char.ToLower(name[0]), name.Substring(1));

			// Avoid certain C# key words.
			if (value == "default")
				return "defaultValue";

			return value;
		}

		#endregion Methods
	}
}
