//********************************************************************************************************************************
// Filename:    IPAddressType.cs
// Owner:       Richard Dunkley
// Description: Provides support for the C# IPAddress type.
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
using System.Collections.Generic;
using System.Net;
using System.Xml;

namespace XMLToDataClass.Data.Types
{
	public class IPAddressType : BaseType
	{
		#region Methods

		public IPAddressType(DataInfo info, string[] possibleValues) : base(info, possibleValues)
		{
			Type = DataType.IPAddress;
			IsNullable = false;
			DisplayName = "IPAddress";

			Usings.Add("System.Net");
		}

		/// <summary>
		///   String of the C# representative data type.
		/// </summary>
		/// <returns>String containing the C# data type.</returns>
		public override string GetDataTypeString()
		{
			return "IPAddress";
		}

		public override EnumInfo[] GenerateAdditionalEnums()
		{
			return new EnumInfo[0];
		}

		/// <summary>
		///   Generates additional properties used by the import or export methods.
		/// </summary>
		/// <returns><see cref="PropertyInfo"/> array representing additional properties needed by the import/export methods. Can be empty.</returns>
		/// <remarks>These properties are typically used to persist state from import to export.</remarks>
		/// <exception cref="InvalidOperationException">An attempt was made to generate a boolean parsing method, but none of the valid boolean string pairs were allowed. At least one boolean string pair (Ex: 'true/false', 'yes/no', or '0/1') must be allowed.</exception>
		public override PropertyInfo[] GenerateAdditionalProperties()
		{
			List<PropertyInfo> propList = new List<PropertyInfo>();
			if (mInfo.IsOptional && mInfo.CanBeEmpty)
			{
				// Can't tell empty and null apart from null version so store the info.
				propList.Add(new PropertyInfo
				(
					"public",
					"bool",
					string.Format("{0}NullIsEmpty", mInfo.PropertyName),
					string.Format("True if the null {0} Version should be represented as an empty string in XML, false if it shouldn't be included.", mInfo.PropertyName)
				));
			}
			return propList.ToArray();
		}

		/// <summary>
		///   Generates the export method code for the IP address.
		/// </summary>
		/// <returns>String array representing the export method code.</returns>
		public override string[] GenerateExportMethodCode()
		{
			List<string> codeLines = new List<string>();

			if (mInfo.IsOptional)
			{
				codeLines.Add(string.Format("if({0} == null)", mInfo.PropertyName));
				if (mInfo.CanBeEmpty)
				{
					codeLines.Add("{");
					codeLines.Add(string.Format("	if({0}NullIsEmpty)", mInfo.PropertyName));
					codeLines.Add("		return string.Empty;");
					codeLines.Add("	else");
					codeLines.Add("		return null;");
					codeLines.Add("}");
				}
				else
				{
					codeLines.Add("	return null;");
				}
			}
			else
			{
				if (mInfo.CanBeEmpty)
				{
					codeLines.Add(string.Format("if({0} == null)", mInfo.PropertyName));
					codeLines.Add("	return string.Empty;");
				}
			}

			codeLines.Add(string.Format("return {0}.ToString();", mInfo.PropertyName));
			return codeLines.ToArray();
		}

		/// <summary>
		///   Generates the import method code for the IP address.
		/// </summary>
		/// <returns>String array representing the import method code.</returns>
		public override string[] GenerateImportMethodCode()
		{
			List<string> codeLines = new List<string>();
			codeLines.Add("if(value == null)");
			if (mInfo.IsOptional)
			{
				codeLines.Add("{");
				codeLines.Add(string.Format("	{0} = null;", mInfo.PropertyName));
				if (mInfo.CanBeEmpty)
					codeLines.Add(string.Format("	{0}NullIsEmpty = false;", mInfo.PropertyName));
				codeLines.Add("	return;");
				codeLines.Add("}");
			}
			else
			{
				codeLines.Add(string.Format("	throw new InvalidDataException(\"The string value for '{0}' is a null reference.\");", mInfo.Name));
			}

			codeLines.Add("if(value.Length == 0)");
			if (mInfo.CanBeEmpty)
			{
				codeLines.Add("{");
				codeLines.Add(string.Format("	{0} = null;", mInfo.PropertyName));
				if (mInfo.IsOptional)
					codeLines.Add(string.Format("	{0}NullIsEmpty = true;", mInfo.PropertyName));
				codeLines.Add("	return;");
				codeLines.Add("}");
			}
			else
			{
				codeLines.Add(string.Format("	throw new InvalidDataException(\"The string value for '{0}' is an empty string.\");", mInfo.Name));
			}

			codeLines.Add(string.Format("{0} = IPAddress.Parse(value);", mInfo.PropertyName));
			return codeLines.ToArray();
		}

		/// <summary>
		///   Loads the configuration properties from XML node.
		/// </summary>
		/// <param name="parent">Parent XML node containing the child settings elements.</param>
		public override void Load(XmlNode parent)
		{
			// No settings to load.
		}

		/// <summary>
		///   Saves the types configuration properties to XML child elements.
		/// </summary>
		/// <param name="doc"><see cref="XmlDocument"/> object representing the XML document to be written.</param>
		/// <param name="parent">Parent <see cref="XmlNode"/> to append the child settings to.</param>
		public override void Save(XmlDocument doc, XmlNode parent)
		{
			// No settings to save.
		}

		public override bool TryParse(string value)
		{
			if (value == null)
				return false;
			if (value.Length == 0)
				return false;

			IPAddress address;
			return IPAddress.TryParse(value, out address);
		}

		#endregion Methods
	}
}
