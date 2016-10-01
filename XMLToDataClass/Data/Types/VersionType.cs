//********************************************************************************************************************************
// Filename:    VersionType.cs
// Owner:       Richard Dunkley
// Description: Class which represents a Version value in an XML file.
//********************************************************************************************************************************
// Copyright © Richard Dunkley 2016
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the
// License. You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0  Unless required by applicable
// law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and
// limitations under the License.
//********************************************************************************************************************************
using System;
using System.Collections.Generic;
using System.Xml;
using CSCodeGen;

namespace XMLToDataClass.Data.Types
{
	public class VersionType : BaseType
	{
		#region Properties

		public static bool DefaultAllowBuild { get; set; }

		public static bool DefaultAllowRevision { get; set; }

		public bool AllowBuild { get; set; }

		public bool AllowRevision { get; set; }

		#endregion Properties

		#region Methods

		static VersionType()
		{
			DefaultAllowBuild = true;
			DefaultAllowRevision = true;
		}

		public VersionType(DataInfo info, string[] possibleValues, bool ignoreCase) : base(info, possibleValues, ignoreCase)
		{
			AllowBuild = DefaultAllowBuild;
			AllowRevision = DefaultAllowRevision;

			Type = DataType.Version;
			IsNullable = false;
			DisplayName = "Version";
		}

		/// <summary>
		///   String of the C# representive data type.
		/// </summary>
		/// <returns>String containing the C# data type.</returns>
		public override string GetDataTypeString()
		{
			return "Version";
		}

		public override EnumInfo[] GenerateAdditionalEnums()
		{
			if (!AllowBuild && !AllowRevision)
				return new EnumInfo[0];

			EnumInfo[] enums = new EnumInfo[1];
			enums[0] = new EnumInfo(
				"public",
				GetEnumTypeName(mInfo.PropertyName),
				"Enumerates the various version types that can be parsed into a version"
			);
			enums[0].Values.Add(new EnumValueInfo("MajorMinor", null, "Only the major and minor part were provided."));

			if(AllowBuild || AllowRevision)
				enums[0].Values.Add(new EnumValueInfo("MajorMinorBuild", null, "The major, minor, and build were provided."));

			if(AllowRevision)
				enums[0].Values.Add(new EnumValueInfo("MajorMinorBuildRevision", null, "The major, minor, build, and revision were provided."));
			return enums;
		}

		/// <summary>
		///   Gets the enumeration type's name from the property name.
		/// </summary>
		/// <param name="propertyName">Property name of the corresponding <see cref="DataInfo"/> object.</param>
		/// <returns>Enumeration type's name.</returns>
		private string GetEnumTypeName(string propertyName)
		{
			return string.Format("{0}VersionType", propertyName);
		}

		/// <summary>
		///   Gets the stored enumeration property's name.
		/// </summary>
		/// <param name="propertyName">Property name of the corresponding <see cref="DataInfo"/> object.</param>
		/// <returns>Stored enumeration property name.</returns>
		private string GetEnumPropertyName(string propertyName)
		{
			return string.Format("{0}ParsedType", propertyName);
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

			if (AllowBuild || AllowRevision)
			{
				propList.Add(new PropertyInfo
				(
					"public",
					GetEnumTypeName(mInfo.PropertyName),
					GetEnumPropertyName(mInfo.PropertyName),
					string.Format("Stores how the {0} version is converted to an XML string", mInfo.PropertyName)
				));
			}
			return propList.ToArray();
		}

		/// <summary>
		///   Generates the export method code for the version.
		/// </summary>
		/// <returns>String array representing the export method code.</returns>
		public override string[] GenerateExportMethodCode()
		{
			string enumPropertyName = GetEnumPropertyName(mInfo.PropertyName);
			string enumTypeName = GetEnumTypeName(mInfo.PropertyName);

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

			if (AllowBuild || AllowRevision)
			{
				codeLines.Add(string.Format("if({0} == {1}.MajorMinorBuild)", enumPropertyName, enumTypeName));
				codeLines.Add(string.Format("	return {0}.ToString(3);", mInfo.PropertyName));

				if (AllowRevision)
				{
					codeLines.Add(string.Format("else if({0} == {1}.MajorMinorBuildRevision)", enumPropertyName, enumTypeName));
					codeLines.Add(string.Format("	return {0}.ToString(4);", mInfo.PropertyName));
				}
			}
			codeLines.Add(string.Format("return {0}.ToString(2);", mInfo.PropertyName));
			return codeLines.ToArray();
		}

		/// <summary>
		///   Generates the import method code for the Version.
		/// </summary>
		/// <returns>String array representing the import method code.</returns>
		public override string[] GenerateImportMethodCode()
		{
			List<string> codeLines = new List<string>();
			codeLines.Add("if (value == null)");
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

			string enumPropertyName = GetEnumPropertyName(mInfo.PropertyName);
			string enumTypeName = GetEnumTypeName(mInfo.PropertyName);

			codeLines.Add("string[] splits = value.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);");
			codeLines.Add("if(splits.Length < 2)");
			codeLines.Add(string.Format("	throw new InvalidDataException(string.Format(\"The Version value ({{0}}) for '{0}' does not contain at least two components separated by a period (Ex: <major>.<minor>)\", value));", mInfo.Name));

			// Check for error conditions.
			if(AllowBuild)
			{
				if(AllowRevision)
				{
					codeLines.Add("if(splits.Length > 4)");
					codeLines.Add(string.Format("	throw new InvalidDataException(string.Format(\"The Version value ({{0}}) for '{0}' has more than 4 components separated by a period. A version is limited to four at most (Ex: <major>.<minor>.<build>.<revision>)\", value));", mInfo.Name));
				}
				else
				{
					codeLines.Add("if(splits.Length > 3)");
					codeLines.Add(string.Format("	throw new InvalidDataException(string.Format(\"The Version value ({{0}}) for '{0}' has more than 3 components separated by a period. This version is limited to three at most (Ex: <major>.<minor>.<build>)\", value));", mInfo.Name));
				}
			}
			else
			{
				if (AllowRevision)
				{
					codeLines.Add("if(splits.Length > 4)");
					codeLines.Add(string.Format("	throw new InvalidDataException(string.Format(\"The Version value ({{0}}) for '{0}' has more than 4 components separated by a period. A version is limited to four at most (Ex: <major>.<minor>.<build>.<revision>)\", value));", mInfo.Name));
				}
				else
				{
					codeLines.Add("if(splits.Length > 2)");
					codeLines.Add(string.Format("	throw new InvalidDataException(string.Format(\"The Version value ({{0}}) for '{0}' has more than 2 components separated by a period. This version is limited to two at most (Ex: <major>.<minor>)\", value));", mInfo.Name));
				}
			}

			// Set the enumeration if needed.
			if (AllowBuild || AllowRevision)
			{
				codeLines.Add(string.Empty);
				codeLines.Add("if(splits.Length == 3)");
				codeLines.Add(string.Format("	{0} = {1}.MajorMinorBuild;", enumPropertyName, enumTypeName));

				if (AllowRevision)
				{
					codeLines.Add("else if(splits.Length == 4)");
					codeLines.Add(string.Format("	{0} = {1}.MajorMinorBuildRevision;", enumPropertyName, enumTypeName));
				}

				codeLines.Add("else");
				codeLines.Add(string.Format("{0} = {1}.MajorMinor;", enumPropertyName, enumTypeName));
			}

			codeLines.Add(string.Empty);
			codeLines.Add("try");
			codeLines.Add("{");
			codeLines.Add("	int major = int.Parse(splits[0]);");
			codeLines.Add("	int minor = int.Parse(splits[1]);");
			if (AllowBuild || AllowRevision)
			{
				codeLines.Add("	if(splits.Length == 2)");
				codeLines.Add("	{");
				codeLines.Add(string.Format("		{0} = new Version(major, minor);", mInfo.PropertyName));
				codeLines.Add("		return;");
				codeLines.Add("	}");
				codeLines.Add("	int build = int.Parse(splits[2]);");

				if(AllowRevision)
				{
					codeLines.Add("	if(splits.Length == 3)");
					codeLines.Add("	{");
					codeLines.Add(string.Format("		{0} = new Version(major, minor, build);", mInfo.PropertyName));
					codeLines.Add("		return;");
					codeLines.Add("	}");
					codeLines.Add(string.Format("	{0} = new Version(major, minor, build, int.Parse(splits[3]));", mInfo.PropertyName));
					codeLines.Add("	return;");
				}
				else
				{
					codeLines.Add(string.Format("	{0} = new Version(major, minor, build);", mInfo.PropertyName));
					codeLines.Add("	return;");
				}
			}
			else
			{
				codeLines.Add(string.Format("	{0} = new Version(major, minor);", mInfo.PropertyName));
				codeLines.Add("	return;");
			}
			codeLines.Add("}");
			codeLines.Add("catch(Exception e)");
			codeLines.Add("{");
			codeLines.Add(string.Format("	throw new InvalidDataException(string.Format(\"The Version value ({{0}}) for '{0}' is not valid. See Inner Exception.\", value), e);", mInfo.Name));
			codeLines.Add("}");

			return codeLines.ToArray();
		}

		/// <summary>
		///   Loads the configuration properties from XML node.
		/// </summary>
		/// <param name="parent">Parent XML node containing the child settings elements.</param>
		public override void Load(XmlNode parent)
		{
			foreach (XmlNode node in parent.ChildNodes)
			{
				if (node.NodeType == XmlNodeType.Element && string.Compare(node.Name, "setting", true) == 0)
				{
					XmlAttribute nameAttrib = node.Attributes["name"];
					XmlAttribute valueAttrib = node.Attributes["value"];
					if (nameAttrib != null && valueAttrib != null)
					{
						// Set AllowBuild if found.
						if (nameAttrib.Value == "AllowBuild")
						{
							bool value;
							if (bool.TryParse(valueAttrib.Value, out value))
								AllowBuild = value;
						}

						// Set AllowRevision if found.
						if (nameAttrib.Value == "AllowRevision")
						{
							bool value;
							if (bool.TryParse(valueAttrib.Value, out value))
								AllowRevision = value;
						}
					}
				}
			}
		}

		/// <summary>
		///   Saves the types configuration properties to XML child elements.
		/// </summary>
		/// <param name="doc"><see cref="XmlDocument"/> object representing the XML document to be written.</param>
		/// <param name="parent">Parent <see cref="XmlNode"/> to append the child settings to.</param>
		public override void Save(XmlDocument doc, XmlNode parent)
		{
			// Add AllowBuild setting.
			XmlElement element = doc.CreateElement("setting");
			XmlAttribute attrib = element.Attributes.Append(doc.CreateAttribute("name"));
			attrib.Value = "AllowBuild";
			attrib = element.Attributes.Append(doc.CreateAttribute("value"));
			attrib.Value = AllowBuild.ToString();
			parent.AppendChild(element);

			// Add AllowRevision setting.
			element = doc.CreateElement("setting");
			attrib = element.Attributes.Append(doc.CreateAttribute("name"));
			attrib.Value = "AllowRevision";
			attrib = element.Attributes.Append(doc.CreateAttribute("value"));
			attrib.Value = AllowRevision.ToString();
			parent.AppendChild(element);
		}

		public override bool TryParse(string value)
		{
			return TryParse(value, AllowBuild, AllowRevision);
		}

		/// <summary>
		///   Attempts to parse a value to a Version, using the specified settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <param name="allowBuild">True if the build component of the version is allowed, false if not.</param>
		/// <param name="allowRevision">True if the revision component of the version is allowed, false if not.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		private static bool TryParse(string value, bool allowBuild, bool allowRevision)
		{
			if (value == null)
				return false;
			if (value.Length == 0)
				return false;

			string[] splits = value.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
			if (splits.Length < 2)
				return false;

			// Check for error conditions.
			if (allowBuild)
			{
				if (allowRevision)
				{
					if(splits.Length > 4)
						return false;
				}
				else
				{
					if (splits.Length > 3)
						return false;
				}
			}
			else
			{
				if (allowRevision)
				{
					if(splits.Length > 4)
						return false;
				}
				else
				{
					if (splits.Length > 2)
						return false;
				}
			}

			int major;
			if (!int.TryParse(splits[0], out major))
				return false;
			if (major < 0)
				return false;
			int minor;
			if (!int.TryParse(splits[1], out minor))
				return false;
			if (minor < 0)
				return false;
			if (allowBuild || allowRevision)
			{
				if (splits.Length == 2)
					return true;

				int build;
				if (!int.TryParse(splits[2], out build))
					return false;

				if (allowRevision)
				{
					if (splits.Length == 3)
						return true;
					int revision;
					if (!int.TryParse(splits[3], out revision))
						return false;
					return true;
				}
				else
				{
					return true;
				}
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		///   Attempts to parse a value to a Version, using the default settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		public static bool TryParseWithDefaults(string value)
		{
			return TryParse(value, DefaultAllowBuild, DefaultAllowRevision);
		}

		#endregion Methods
	}
}
