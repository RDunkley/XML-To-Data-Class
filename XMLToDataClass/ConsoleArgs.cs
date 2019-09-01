//********************************************************************************************************************************
// Filename:    ConsoleArgs.cs
// Owner:       Richard Dunkley
// Description: This class uses reflection to analyze a class for attributes that it uses to pull arguments from the command line
//              and place the arguments in the properties of the class.
//********************************************************************************************************************************
// Copyright © Richard Dunkley 2019
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the
// License. You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0  Unless required by applicable
// law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and
// limitations under the License.
//********************************************************************************************************************************
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
	/// <summary>
	///   Attribute to apply to classes.
	/// </summary>
	/// <remarks>
	///   This attribute determines that the corresponding class will be used for command line arguments. It also outlines the
	///   usage text to be added to the help text.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Class)]
	public class Usage : Attribute
	{
		#region Fields

		/// <summary>
		///   Stores the usage statement associated with the settings class.
		/// </summary>
		private readonly string mUsageStatement;

		#endregion Fields

		#region Methods

		/// <summary>
		///   Instantiates a new <see cref="Usage"/> <see cref="Attribute"/> object.
		/// </summary>
		/// <param name="usage">
		///   Usage statement to include at the start of the help text. Should direct the user how to use the command line
		///   options. Can be null, but the help text will not be complete.
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="usage"/> is a null reference.</exception>
		public Usage(string usage)
		{
			mUsageStatement = usage ?? throw new ArgumentNullException("usage");
		}

		/// <summary>
		///   Gets the usage statement pulled from the class attribute.
		/// </summary>
		/// <returns>String containing the usage statement.</returns>
		public string GetUsageStatement()
		{
			return mUsageStatement;
		}

		#endregion Methods
	}

	/// <summary>
	///   Attribute to apply to the individual properties of a class to determine which should be used for argument input.
	/// </summary>
	/// <remarks>
	///   This attribute flags the property so that <see cref="ConsoleArgs"/> class knows to use it to parse arguments into. The
	///   accepted property types are: Boolean, Byte, Char, DateTime, System.DBNull, Decimal, Double, System.Enum, Int16, Int32,
	///   Int64, SByte, Single, String, UInt16, UInt32, and UInt64. It also supports arrays of the previous types.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Property)]
	public class Argument : Attribute
	{
		#region Fields

		/// <summary>
		///   Determines if the argument is required or not.
		/// </summary>
		/// <remarks>This value is optional and defaults to false.</remarks>
		public bool Required;

		/// <summary>
		///   Single character keyword used to identify the corresponding argument.
		/// </summary>
		/// <remarks>
		///   Each input argument has a shorthand keyword and an optional longhand keyword to identify it. This is the shorthand version
		///   This parameter is required in the attribute.
		/// </remarks>
		private readonly char SingleChar;

		/// <summary>
		///   Contains a description of the argument associated with the property. 
		/// </summary>
		/// <remarks>
		///   This description is used in the help text to identify to the user what the corresponding argument does. This
		///   parameter is required in the attribute.
		/// </remarks>
		private readonly string Description;

		/// <summary>
		///   Contains the word used for the longhand version of the input keyword.
		/// </summary>
		/// <remarks>
		///   Each input argument can optionally have a longhand version that is composed of a word. This parameter is not
		///   required, but is available if something beyond the single character is desired.
		/// </remarks>
		public string Word;

		#endregion Fields

		#region Methods

		/// <summary>
		///   Instatiates a new <see cref="Argument"/> attribute.
		/// </summary>
		/// <param name="singleChar">
		///   Single character keyword used to identify the argument. Must be a lower case or upper case letter. Character is case
		///   sensitive.
		/// </param>
		/// <param name="description">
		///   Description of the argument associated with the property. Used for the help text. Can be null, but the corresponding
		///   help text will contain an empty string.
		/// </param>
		public Argument(char singleChar, string description)
		{
			if (!Char.IsLetter(singleChar))
				throw new ArgumentException(string.Format("The singleChar specified ({0}) is not a letter.", singleChar));

			SingleChar = singleChar;
			Description = description;
			Required = false;
		}

		/// <summary>
		///   Gets the single character keyword associated with the corresponding argument.
		/// </summary>
		/// <returns>Single character letter keyword.</returns>
		public char GetSingleChar()
		{
			return SingleChar;
		}

		/// <summary>
		///   Gets the description of the argument.
		/// </summary>
		/// <returns>String containing the description.</returns>
		public string GetDescription()
		{
			return Description;
		}

		#endregion Methods
	}

	/// <summary>
	/// Static class used to parse the command line attributes and populate a class object.
	/// </summary>
	public static class ConsoleArgs<T> where T : class
	{
		#region Classes

		/// <summary>
		///   Parses the type and generates lookup tables for the properties, based on their arguments.
		/// </summary>
		private class PropertyLookup
		{
			#region Enumerations

			/// <summary>
			///   Enumerates the various property types supported by the <see cref="ConsoleArgs{T}"/> class.
			/// </summary>
			public enum PropType
			{
				/// <summary>
				///   Represents no property type or property type that was not recognized.
				/// </summary>
				None,

				/// <summary>
				///   Represents a single flag type or boolean property.
				/// </summary>
				Flag,

				/// <summary>
				///   Represents an array of value types.
				/// </summary>
				Array,

				/// <summary>
				///   Represents a single value type.
				/// </summary>
				Single,
			}

			#endregion Enumerations

			#region Fields

			/// <summary>
			///   Lookup table containing the attribute key and associated property information for array properties.
			/// </summary>
			private Dictionary<string, PropertyInfo> mArrayProperties = new Dictionary<string, PropertyInfo>();

			/// <summary>
			///   Lookup table containing the attribute key and associated property information for flag properties (boolean).
			/// </summary>
			private Dictionary<string, PropertyInfo> mFlagProperties = new Dictionary<string, PropertyInfo>();

			/// <summary>
			///   Lookup table containing the attribute key and associated property information for value properties.
			/// </summary>
			private Dictionary<string, PropertyInfo> mValueProperties = new Dictionary<string, PropertyInfo>();

			#endregion Fields

			#region Properties

			/// <summary>
			///   Gets an array of all the <see cref="PropertyInfo"/> objects that are tied to an argument.
			/// </summary>
			public PropertyInfo[] PropertiesWithArguments { get; private set; }

			/// <summary>
			///   Gets the usage string associated with the class.
			/// </summary>
			public string UsageString { get; private set; }

			#endregion Properties

			#region Methods

			/// <summary>
			///   Instantiates a new <see cref="PropertyLookup"/> type based on the specified <see cref="Type"/>.
			/// </summary>
			/// <param name="type"><see cref="Type"/> to parse for properties containing <see cref="Argument"/> attributes.</param>
			/// <exception cref="ArgumentException">
			///   <paramref name="type"/> is not a class type, does not have a <see cref="Usage"/> attribute or an invalid one,
			///   or has a property with an invalid 'Argument' attribute.
			/// </exception>
			/// <exception cref="ArgumentNullException"><paramref name="type"/> is a null reference.</exception>
			public PropertyLookup(Type type)
			{
				if (type == null)
					throw new ArgumentNullException("type");
				if (!type.IsClass)
					throw new ArgumentException("The type specified is not a class type.", "type");

				UsageString = GetUsageStatement(type);

				// Parse the type information.
				PropertyInfo[] props = type.GetProperties();
				List<PropertyInfo> propList = new List<PropertyInfo>();
				foreach (PropertyInfo prop in props)
				{
					if (prop.CanWrite)
					{
						Attribute propAttr = null;
						try
						{
							propAttr = GetArgumentAttribute(prop);
						}
						catch(ArgumentException e)
						{
							throw new ArgumentException(
								string.Format("The type specified contained a property ({0}) with an invalid 'Argument' attribute ({1}).",
								prop.Name, e.Message), e);
						}

						if (propAttr != null)
						{
							propList.Add(prop);

							Argument arg = (Argument)propAttr;
							if (prop.PropertyType == typeof(bool))
							{
								string[] keys = GetKeys(arg);
								foreach (string key in keys)
									mFlagProperties.Add(key, prop);
							}
							else
							{
								if (prop.PropertyType.IsArray)
								{
									string[] keys = GetKeys(arg);
									foreach (string key in keys)
										mArrayProperties.Add(key, prop);
								}
								else
								{
									string[] keys = GetKeys(arg);
									foreach (string key in keys)
										mValueProperties.Add(key, prop);
								}
							}
						}
					}
				}

				PropertiesWithArguments = propList.ToArray();
			}

			/// <summary>
			///   Generates the help text associated with the property.
			/// </summary>
			/// <param name="prop"><see cref="PropertyInfo"/> object to generate the help text for.</param>
			/// <returns>Multi-line string containing the help text associated with the property.</returns>
			/// <exception cref="ArgumentException"><paramref name="prop"/> does not match a property in <see cref="PropertiesWithArguments"/>.</exception>
			/// <exception cref="ArgumentNullException"><paramref name="prop"/> is a null reference.</exception>
			public string GeneratePropertyHelpText(PropertyInfo prop)
			{
				if (prop == null)
					throw new ArgumentNullException("prop");

				List<PropertyInfo> propList = new List<PropertyInfo>(PropertiesWithArguments);
				if (!propList.Contains(prop))
					throw new ArgumentException("The PropertyInfo object provided does not match a property that contains an 'Argument' attribute.", "prop");

				if (mFlagProperties.ContainsValue(prop))
					return GenerateFlagPropertyHelpText(prop);
				if (mArrayProperties.ContainsValue(prop))
					return GenerateArrayPropertyHelpText(prop);
				return GenerateValuePropertyHelpText(prop);
			}

			/// <summary>
			///  Generates the help text for an array property object.
			/// </summary>
			/// <param name="prop"><see cref="PropertyInfo"/> object containing the array property information.</param>
			/// <exception cref="ArgumentException">
			///   <paramref name="prop"/> does not contain an <see cref="Argument"/> <see cref="Attribute"/>.
			/// </exception>
			private string GenerateArrayPropertyHelpText(PropertyInfo prop)
			{
				Argument arg = (Argument)GetArgumentAttribute(prop);

				StringBuilder sb = new StringBuilder();
				sb.AppendLine("\t" + GenerateKeyString(arg) + "=" + prop.Name + ",...");
				if (!arg.Required)
					sb.AppendLine("\t\t[Optional] - " + arg.GetDescription());
				else
					sb.AppendLine("\t\t" + arg.GetDescription());
				return sb.ToString();
			}

			/// <summary>
			///   Generates the help text for a flag property object.
			/// </summary>
			/// <param name="sb"><see cref="StringBuilder"/> object used to build up the help text string.</param>
			/// <param name="arg"><see cref="Argument"/> object containing the single flag attribute information.</param>
			/// <param name="prop"><see cref="PropertyInfo"/> object containing the boolean property information.</param>
			/// <exception cref="ArgumentException">
			///   <paramref name="prop"/> does not contain an <see cref="Argument"/> <see cref="Attribute"/>.
			/// </exception>
			private string GenerateFlagPropertyHelpText(PropertyInfo prop)
			{
				Argument arg = (Argument)GetArgumentAttribute(prop);

				StringBuilder sb = new StringBuilder();
				sb.AppendLine("\t" + GenerateKeyString(arg));
				if (!arg.Required)
					sb.AppendLine("\t\t[Optional] - " + arg.GetDescription());
				else
					sb.AppendLine("\t\t" + arg.GetDescription());
				return sb.ToString();
			}

			/// <summary>
			///   Generates the help text for a property object.
			/// </summary>
			/// <param name="sb"><see cref="StringBuilder"/> object used to build up the help text string.</param>
			/// <param name="arg"><see cref="Argument"/> object containing the attribute information.</param>
			/// <param name="prop"><see cref="PropertyInfo"/> object containing the property information.</param>
			/// <exception cref="ArgumentException">
			///   <paramref name="prop"/> does not contain an <see cref="Argument"/> <see cref="Attribute"/>.
			/// </exception>
			private string GenerateValuePropertyHelpText(PropertyInfo prop)
			{
				Argument arg = (Argument)GetArgumentAttribute(prop);

				StringBuilder sb = new StringBuilder();
				sb.AppendLine("\t" + GenerateKeyString(arg) + "=" + prop.Name);
				if (!arg.Required)
					sb.AppendLine("\t\t[Optional] - " + arg.GetDescription());
				else
					sb.AppendLine("\t\t" + arg.GetDescription());
				return sb.ToString();
			}

			/// <summary>
			///   Generates a string of all the possible keys associated with the attribute.
			/// </summary>
			/// <param name="arg"><see cref="Argument"/> object contiaining information about the argument.</param>
			/// <returns>String containing a list of all the possible keys associated with <paramref name="arg"/>.</returns>
			private string GenerateKeyString(Argument arg)
			{
				StringBuilder sb = new StringBuilder();
				string[] keys = GetKeys(arg);
				bool first = true;
				foreach (string key in keys)
				{
					if (first)
					{
						sb.Append(key);
						first = false;
					}
					else
					{
						sb.Append(",");
						sb.Append(key);
					}
				}
				return sb.ToString();
			}

			/// <summary>
			///   Gets the <see cref="Argument"/> <see cref="Attribute"/> from the provided <see cref="PropertyInfo"/>.
			/// </summary>
			/// <param name="source"><see cref="PropertyInfo"/> object pulled from a property in a settings class.</param>
			/// <returns>
			///   <see cref="Argument"/> <see cref="Attribute"/> associated with the property or null if no argument attribute was applied
			///   to the property.
			/// </returns>
			/// <exception cref="ArgumentException">More than one 'Argument' attribute was applied to the property, or the attribute type couldn't be loaded.</exception>
			private Attribute GetArgumentAttribute(PropertyInfo source)
			{
				Attribute attr;
				try
				{
					attr = Attribute.GetCustomAttribute(source, typeof(Argument));
				}
				catch (AmbiguousMatchException e)
				{
					throw new ArgumentException("More than one 'Argument' attribute is tied to the property.", e);
				}
				catch (TypeLoadException e)
				{
					throw new ArgumentException("An error occurred while loading the attribute type for the 'Argument' attribute tied to the property.", e);
				}
				return attr;
			}

			/// <summary>
			///   Gets the various keys associated with an argument.
			/// </summary>
			/// <param name="arg"><see cref="Argument"/> to generate the various possible keys from.</param>
			/// <returns>Array of keys that are associated with the <paramref name="arg"/>.</returns>
			private static string[] GetKeys(Argument arg)
			{
				List<string> list = new List<string>(4)
				{
					arg.GetSingleChar().ToString()
				};
				if (!string.IsNullOrEmpty(arg.Word))
					list.Add(arg.Word);
				return list.ToArray();
			}

			/// <summary>
			///   Gets the <see cref="PropertyInfo"/> object associated with the specified attribute key.
			/// </summary>
			/// <param name="key">Attribute key specified by a <see cref="Argument"/> attribute in a property in the type passed into this class's constructor.</param>
			/// <returns><see cref="PropertyInfo"/> associated with the key or null if no <see cref="PropertyInfo"/> was found.</returns>
			/// <exception cref="ArgumentNullException"><paramref name="key"/> is a null reference.</exception>
			public PropertyInfo GetPropertyInfo(string key)
			{
				if (key == null)
					throw new ArgumentNullException("key");

				if (mFlagProperties.ContainsKey(key))
					return mFlagProperties[key];
				if (mArrayProperties.ContainsKey(key))
					return mArrayProperties[key];
				if (mValueProperties.ContainsKey(key))
					return mValueProperties[key];
				return null;
			}

			/// <summary>
			///   Gets the <see cref="PropType"/> associated with the specified attribute key.
			/// </summary>
			/// <param name="key">Attribute key specified by a <see cref="Argument"/> attribute in a property in the type passed into this class's constructor.</param>
			/// <returns><see cref="PropType"/> associated with the key or <see cref="PropType"/>.<see cref="PropType.None"/> if it was not found.</returns>
			/// <exception cref="ArgumentNullException"><paramref name="key"/> is a null reference.</exception>
			public PropType GetPropertyType(string key)
			{
				if (key == null)
					throw new ArgumentNullException("key");

				if (mFlagProperties.ContainsKey(key))
					return PropType.Flag;
				if (mArrayProperties.ContainsKey(key))
					return PropType.Array;
				if (mValueProperties.ContainsKey(key))
					return PropType.Single;
				return PropType.None;
			}

			/// <summary>
			///   Gets the <see cref="Usage"/> <see cref="Attribute"/> from the provided class type.
			/// </summary>
			/// <param name="source"><see cref="Type"/> of the settings class.</param>
			/// <returns>Usage <see cref="Attribute"/> found on the class type.</returns>
			/// <exception cref="ArgumentException">
			///   There was more than one usage attribute applied to the class, the class type does not contain a usage attribute,
			///   or the usage attribute type could not be loaded.
			/// </exception>
			private string GetUsageStatement(Type source)
			{
				Attribute attr;
				try
				{
					attr = Attribute.GetCustomAttribute(source, typeof(Usage));
				}
				catch (AmbiguousMatchException e)
				{
					throw new ArgumentException("More than one 'Usage' attribute is tied to the type.", e);
				}
				catch (TypeLoadException e)
				{
					throw new ArgumentException("An error occurred while loading the attribute type for the 'Usage' attribute tied to the type.", e);
				}

				if (attr == null)
					throw new ArgumentException("The class does not contain a 'Usage' attribute.");

				Usage use = (Usage)attr;
				return use.GetUsageStatement();
			}

			#endregion Methods
		}

		/// <summary>
		///   Parse the command line and generates lookup tables based on the arguments found.
		/// </summary>
		private class ConsoleArgInfo
		{
			#region Fields

			/// <summary>
			///   Stores the command line string passed into the constructor.
			/// </summary>
			private readonly string mCommandLine;

			/// <summary>
			///   Lookup table of the matches found in the command line.
			/// </summary>
			private Dictionary<string, Match> mMatches = new Dictionary<string, Match>();

			/// <summary>
			///   <see cref="PropertyInfo"/> containing the property and argument information on the type.
			/// </summary>
			private PropertyLookup mProps;

			/// <summary>
			///   Lookup table of the <see cref="PropertyInfo"/> object associated with each argument found.
			/// </summary>
			private Dictionary<string, PropertyInfo> mTagProperties = new Dictionary<string, PropertyInfo>();

			/// <summary>
			///   Lookkup table of the <see cref="PropertyLookup.PropType"/> for the property associated with each argument found.
			/// </summary>
			private Dictionary<string, PropertyLookup.PropType> mTagType = new Dictionary<string, PropertyLookup.PropType>();

			#endregion Fields

			#region Methods

			/// <summary>
			///   Instantiates a new <see cref="ConsoleArgInfo"/> object using the provided command line and <see cref="PropertyLookup"/> object.
			/// </summary>
			/// <param name="commandLine">Command line to be parsed.</param>
			/// <param name="props"><see cref="PropertyLookup"/> object containing the argument and property information for the destination class.</param>
			/// <param name="throwErrorOnNotFound">True if the constructor should throw an exception if an argument couldn't be located in <paramref name="props"/>.</param>
			/// <exception cref="InvalidOperationException">
			///   The found argument did not match the corresponding property type or the argument had no corresponding property type and <paramref name="throwErrorOnNotFound"/> was true.
			/// </exception>
			public ConsoleArgInfo(string commandLine, PropertyLookup props, bool throwErrorOnNotFound = true)
			{
				mProps = props ?? throw new ArgumentNullException("props");
				if (string.IsNullOrEmpty(commandLine))
				{
					mCommandLine = string.Empty;
					return;
				}

				mCommandLine = commandLine;
				TokenizeCommandLine();
				//SortTokens();
				FindProperties(throwErrorOnNotFound);
			}

			/// <summary>
			///   Gets an array of all the string values contained in an array argument.
			/// </summary>
			/// <param name="tag">Tag of the array argument.</param>
			/// <returns>Array of the string values.</returns>
			private string[] GetValues(string tag)
			{
				int count = mMatches[tag].Groups["value"].Captures.Count;
				string[] returnArray = new string[count];
				for (int i = 0; i < count; i++)
					returnArray[i] = mMatches[tag].Groups["value"].Captures[i].Value;
				return returnArray;
			}

			/// <summary>
			///   Associates the arguments with their corresponding properties.
			/// </summary>
			/// <param name="throwErrorOnNotFound">True if an error should be thrown if a corresponding property could not be located, false to ignore the argument.</param>
			/// <exception cref="InvalidOperationException">
			///   The found argument did not match the corresponding property type or the argument had no corresponding property type and <paramref name="throwErrorOnNotFound"/> was true.
			/// </exception>
			private void FindProperties(bool throwErrorOnNotFound)
			{
				foreach(string tag in mMatches.Keys)
				{
					int keyIndex = mMatches[tag].Groups["tag"].Index;
					PropertyLookup.PropType propType = mProps.GetPropertyType(tag);
					if (propType == PropertyLookup.PropType.Single)
					{
						if (mMatches[tag].Groups["value"].Captures.Count == 0)
						{
							// Single found, but no following value was present.
							string[] lines = GenerateErrorString(mCommandLine, keyIndex);
							throw new InvalidOperationException(string.Format("ERROR: Found a tag ({0}) corresponding to a single value, but no value was found after the tag (Ex: --tag <value> or --tag=<value>).\n{1}\n{2}", keyIndex, lines[0], lines[1]));
						}
						else if(mMatches[tag].Groups["value"].Captures.Count != 1)
						{
							// Single found, but multiple values are present.
							string[] lines = GenerateErrorString(mCommandLine, keyIndex);
							throw new InvalidOperationException(string.Format("ERROR: Found a tag ({0}) corresponding to a single value, but multiple values were found after the tag. Only one is allowed for this type.\n{1}\n{2}", keyIndex, lines[0], lines[1]));
						}
						else
						{
							mTagProperties.Add(tag, mProps.GetPropertyInfo(tag));
							mTagType.Add(tag, PropertyLookup.PropType.Single);
						}
					}
					else if(propType == PropertyLookup.PropType.Flag)
					{
						int count = mMatches[tag].Groups["value"].Captures.Count;
						if (count > 1 || (count == 1 && mMatches[tag].Groups["value"].Captures[0].Value != string.Empty))
						{
							// Flag found, but a value was also present.
							string[] lines = GenerateErrorString(mCommandLine, keyIndex);
							throw new InvalidOperationException(string.Format("ERROR: Found a tag ({0}) corresponding to a flag, but one or more values were found after the tag.\n{1}\n{2}", keyIndex, lines[0], lines[1]));
						}
						else
						{
							mTagProperties.Add(tag, mProps.GetPropertyInfo(tag));
							mTagType.Add(tag, PropertyLookup.PropType.Flag);
						}
					}
					else if(propType == PropertyLookup.PropType.Array)
					{
						mTagProperties.Add(tag, mProps.GetPropertyInfo(tag));
						mTagType.Add(tag, PropertyLookup.PropType.Array);
					}
					else
					{
						if(throwErrorOnNotFound)
						{
							string[] lines = GenerateErrorString(mCommandLine, keyIndex);
							throw new InvalidOperationException(string.Format("ERROR: Found a tag ({0}), but it does not appear to be a valid command line argument.\n{1}\n{2}", keyIndex, lines[0], lines[1]));
						}
					}
				}
			}

			/// <summary>
			///   Populates the properties in the <paramref name="settingsObject"/>.
			/// </summary>
			/// <param name="settingsObject">Object to populate the properties of.</param>
			/// <exception cref="InvalidOperationException">An error occurred while attempting to set the property. See Inner Exception.</exception>
			public void PopulateProperties(object settingsObject)
			{
				foreach (string tag in mTagProperties.Keys)
				{
					try
					{
						int keyIndex = mMatches[tag].Groups["tag"].Index;
						if (mTagType[tag] == PropertyLookup.PropType.Single)
						{
							string value = GetValues(tag)[0];
							mTagProperties[tag].SetValue(settingsObject, Convert.ChangeType(value, mTagProperties[tag].PropertyType));
						}
						else if (mTagType[tag] == PropertyLookup.PropType.Array)
						{
							string[] values = GetValues(tag);
							Type elementType = mTagProperties[tag].PropertyType.GetElementType();
							Array arrayList = Array.CreateInstance(elementType, values.Length);
							for (int i = 0; i < values.Length; i++)
								arrayList.SetValue(Convert.ChangeType(values[i], elementType), i);
							mTagProperties[tag].SetValue(settingsObject, arrayList);
						}
						else
						{
							mTagProperties[tag].SetValue(settingsObject, true);
						}
					}
					catch (Exception e)
					{
						if (e is ArgumentException || e is TargetException || e is TargetInvocationException || e is InvalidCastException
							|| e is FormatException || e is OverflowException)
						{
							throw new InvalidOperationException(string.Format("An attempt to set the property associated with an argument ({0}) was unsuccessful ({1}).", tag, e.Message), e);
						}
						throw;
					}
				}
			}

			/// <summary>
			///   Uses regular expressions to find arguments in the command line and generate the <see cref="mMatches"/> lookup table.
			/// </summary>
			private void TokenizeCommandLine()
			{
				// -(-)?([A-Za-z][A-Za-z0-9]*)+( )*(=)?( )*(\"([^\"]*)\"|([^-,\s][^,\s]*)|)((\s)*,(\s)*(\"([^\"]*)\"|([^,\s]*)))*
				string pattern = " -(-)?" + // Opening --
					"(?<tag>[\\p{Lu}\\p{Ll}\\p{Lt}\\p{Lm}\\p{Lo}][\\p{Lu}\\p{Ll}\\p{Lt}\\p{Lm}\\p{Lo}\\p{Nl}\\p{Mn}\\p{Mc}\\p{Nd}\\p{Pc}\\p{Cf}]*)+" + // tag
					"(\\s)*(=)?(\\s)*" + // ' = ' option
										 //"(?<value>\"[^\"]*\"|([^-,\\s][^,\\s]*)|)" + // 1st value.
					"(\"(?<value>[^\"]*)\"|(?<value>[^-,\\s][^,\\s]*)|)" + // 1st value.
					"((\\s)*,(\\s)*(\"(?<value>[^\"]*)\"|(?<value>[^,\\s]*)|))*"; // other values.
				Regex r = new Regex(pattern, RegexOptions.Singleline|RegexOptions.ExplicitCapture);

				Match m = r.Match(mCommandLine);
				while(m.Success)
				{
					if (mMatches.ContainsKey(m.Groups["tag"].Value))
						throw new InvalidOperationException();
					mMatches.Add(m.Groups["tag"].Value, m);
					m = m.NextMatch();
				}
			}

			#endregion Methods
		}

		#endregion Classes

		#region Methods

		/// <summary>
		///   Generates an error string from the specified command line and the index where the error occurred.
		/// </summary>
		/// <param name="commandLine">Command line being parsed when the error occurred.</param>
		/// <param name="index">Index in the command line where the error occurred.</param>
		/// <returns>String array containing a string on where the error occurred. This allows a user to quickly locate the error.</returns>
		private static string[] GenerateErrorString(string commandLine, int index)
		{
			int before;
			if (index > 20)
				before = 20;
			else
				before = index;

			int after;
			if (commandLine.Length > index + 20)
				after = 20;
			else
				after = commandLine.Length - index;

			string[] lines = new string[2];
			lines[0] = commandLine.Substring(index - before, after + before);
			lines[1] = string.Format("{0}^{1}", GenerateSpaceString(before), GenerateSpaceString(after));
			return lines;
		}

		/// <summary>
		///   Generates the help text to display to the user for usage explanation.
		/// </summary>
		/// <returns>Multi-line string containing information on how to use the application based on the attributes found in the specified type.</returns>
		/// <remarks>The text is built from the descriptions in the added attributes.</remarks>
		public static string GenerateHelpText()
		{
			// Parse the type information.
			Type settingsType = typeof(T);
			PropertyLookup pl = new PropertyLookup(settingsType);

			StringBuilder helpText = new StringBuilder();
			helpText.AppendLine(pl.UsageString);

			foreach (PropertyInfo prop in pl.PropertiesWithArguments)
				helpText.Append(pl.GeneratePropertyHelpText(prop));

			return helpText.ToString();
		}

		/// <summary>
		///   Generates a string containing the specified number of spaces.
		/// </summary>
		/// <param name="count">Number of spaces contained in the string.</param>
		/// <returns>An empty string if count is less than one, or a string of spaces with a length equal to <paramref name="count"/>.</returns>
		private static string GenerateSpaceString(int count)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < count; i++)
				sb.Append(" ");
			return sb.ToString();
		}

		/// <summary>
		///   Populates the properties in <paramref name="settingsObject"/> based on values provided in <paramref name="commandLine"/>.
		/// </summary>
		/// <param name="commandLine">Command line string used to pull the settings from.</param>
		/// <param name="settingsObject">Class object containing properties to be populated.</param>
		/// <param name="throwErrorOnNotFound">
		///   True if an exception should be thrown if a command line attribute is found that does not have a corresponding property in
		///   <paramref name="settingsObject"/>, false if the attribute should be ignored.
		/// </param>
		public static void Populate(string commandLine, T settingsObject, bool throwErrorOnNotFound = true)
		{
			if (string.IsNullOrEmpty(commandLine))
				return;

			if (settingsObject == null)
				throw new ArgumentNullException("settingsObject");

			PropertyLookup propTable = new PropertyLookup(typeof(T));
			ConsoleArgInfo argInfo = new ConsoleArgInfo(commandLine, propTable, throwErrorOnNotFound);
			argInfo.PopulateProperties(settingsObject);
		}

		#endregion Methods
	}
}
