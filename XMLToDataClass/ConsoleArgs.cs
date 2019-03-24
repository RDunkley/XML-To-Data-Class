//********************************************************************************************************************************
// Filename:    ConsoleArgs.cs
// Owner:       Richard Dunkley
// Description: This class uses reflection to analyze a class for attributes that is uses to pull arguments from the command line
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
		private string mUsageStatement;

		#endregion Fields

		#region Methods

		/// <summary>
		///   Instantiates a new <see cref="Usage"/> <see cref="Attribute"/> object.
		/// </summary>
		/// <param name="usage">
		///   Usage statement to include at the start of the help text. Should direct the user how to use the command line
		///   options. Can be null, but the help text will not be complete.
		/// </param>
		public Usage(string usage)
		{
			mUsageStatement = usage;
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
	/// 
	/// </summary>
	public static class ConsoleArgs<T> where T : class
	{
		#region Classes

		private class ArgumentInfo
		{
			public PropertyInfo Prop { get; set; }
			public Argument Arg { get; set; }

			public ArgumentInfo(PropertyInfo prop, Argument arg)
			{
				Prop = prop;
				Arg = arg;
			}
		}

		#endregion Classes

		#region Enums

		/// <summary>
		///   Enumerates the various states of a command argument.
		/// </summary>
		private enum CommandState
		{
			Start,
			Flag,
			StartKey,
			Key,
			Equal,
			StartValue,
			Value,
			Quote,
			QuoteEnd,
		}

		#endregion Enums

		public static string GetHelpText()
		{
			// Parse the type information.
			Type settingsType = typeof(T);
			Attribute usageAttr = GetUsageAttribute(settingsType);

			StringBuilder helpText = new StringBuilder();
			Usage use = (Usage)usageAttr;
			helpText.AppendLine(use.GetUsageStatement());
			PropertyInfo[] props = settingsType.GetProperties();

			foreach (PropertyInfo prop in props)
			{
				if (!prop.CanWrite)
					throw new InvalidOperationException(string.Format("An attempt was made to use a property {0} that is not writeable", prop.Name));

				Attribute propAttr = GetArgumentAttribute(prop);
				if (propAttr != null)
				{
					Argument arg = (Argument)propAttr;
					if (prop.PropertyType == typeof(bool))
					{
						AddFlagPropertyToHelpText(helpText, arg, prop);
					}
					else
					{
						if (prop.PropertyType.IsArray)
						{
							AddArrayProperty(helpText, arg, prop);
						}
						else
						{
							AddProperty(helpText, arg, prop);
						}
					}
					//throw new InvalidOperationException(string.Format("An attempt was made to use a property {0} whose type is not supported.", prop.Name));
				}
			}
			return helpText.ToString();
		}

		private static void GetLookupTables(out Dictionary<string, ArgumentInfo> flagLookup, out Dictionary<string, ArgumentInfo> arrayLookup, out Dictionary<string, ArgumentInfo> singleItemLookup)
		{
			flagLookup = new Dictionary<string, ArgumentInfo>();
			arrayLookup = new Dictionary<string, ArgumentInfo>();
			singleItemLookup = new Dictionary<string, ArgumentInfo>();

			// Parse the type information.
			Type settingsType = typeof(T);
			PropertyInfo[] props = settingsType.GetProperties();

			foreach (PropertyInfo prop in props)
			{
				if (!prop.CanWrite)
					throw new InvalidOperationException(string.Format("An attempt was made to use a property {0} that is not writeable", prop.Name));

				Attribute propAttr = GetArgumentAttribute(prop);
				if (propAttr != null)
				{
					Argument arg = (Argument)propAttr;
					if (prop.PropertyType == typeof(bool))
					{
						string[] keys = GetKeys(arg);
						foreach (string key in keys)
							flagLookup.Add(key, new ArgumentInfo(prop, arg));
					}
					else
					{
						if (prop.PropertyType.IsArray)
						{
							string[] keys = GetKeys(arg);
							foreach (string key in keys)
								arrayLookup.Add(key, new ArgumentInfo(prop, arg));
						}
						else
						{
							string[] keys = GetKeys(arg);
							foreach (string key in keys)
								singleItemLookup.Add(key, new ArgumentInfo(prop, arg));
						}
					}
					//throw new InvalidOperationException(string.Format("An attempt was made to use a property {0} whose type is not supported.", prop.Name));
				}
			}
		}

		private static bool IsNextWord(string line, int index, string word)
		{
			if (index < 0 || index > line.Length - 1)
				return false;
			if (index + word.Length > line.Length - 1)
				return false;
			for(int i = 0; i < word.Length; i++)
			{
				if (line[index + i] != word[i])
					return false;
			}
			return true;
		}

		private static string[] SortByLargestLength(string[] keys)
		{
			if (keys.Length == 0)
				return new string[0];

			// Generate the lookup table of the sizes and lengths.
			Dictionary<int, List<string>> sizeLookup = new Dictionary<int, List<string>>(keys.Length);
			foreach(string key in keys)
			{
				if (!sizeLookup.ContainsKey(key.Length))
					sizeLookup.Add(key.Length, new List<string>());
				sizeLookup[key.Length].Add(key);
			}

			// Store into array.
			List<int> keyList = new List<int>(sizeLookup.Keys);
			keyList.Sort();
			string[] returnKeys = new string[keys.Length];
			int index = keys.Length - 1;
			foreach(int keyLength in keyList)
			{
				foreach(string key in sizeLookup[keyLength])
					returnKeys[index--] = key;
			}
			return returnKeys;

		}

		private static Dictionary<string,string[]> ParseArguments(string commandLine)
		{
			commandLine = commandLine.TrimStart('"');
			string fullAssemblyPath = Assembly.GetExecutingAssembly().Location;
			if (commandLine.StartsWith(fullAssemblyPath))
				commandLine = commandLine.Substring(fullAssemblyPath.Length);
			commandLine = commandLine.TrimStart('"');

			Dictionary<string, List<string>> argLookup = new Dictionary<string, List<string>>();
			int index = 0;
			string key = null;
			StringBuilder value = new StringBuilder();
			CommandState state = CommandState.Flag;
			while (index < commandLine.Length)
			{
				switch (state)
				{
					case CommandState.Flag:
						if (IsNextWord(commandLine, index, "--"))
						{
							value.Clear();
							index += 2;
							state = CommandState.StartKey;
						}
						else if (IsNextWord(commandLine, index, "-"))
						{
							value.Clear();
							index += 1;
							state = CommandState.StartKey;
						}
						else if (commandLine[index] == ' ')
						{
							index += 1;
						}
						else
						{
							throw new InvalidOperationException();
						}
						break;
					case CommandState.StartKey:
						if (char.IsLetter(commandLine[index]))
						{
							value.Append(commandLine[index]);
							index += 1;
							state = CommandState.Key;
						}
						else
						{
							throw new InvalidOperationException();
						}
						break;
					case CommandState.Key:
						if (commandLine[index] == ' ')
						{
							// Space reached so store the flag.
							string valueString = value.ToString();
							if (argLookup.ContainsKey(valueString))
								throw new InvalidOperationException();

							index += 1;
							state = CommandState.Flag;
							value.Clear();
							argLookup.Add(valueString, null);
						}
						else if (commandLine[index] == '=')
						{
							// '=' was reached so we have a corresponding value or array.
							string valueString = value.ToString();
							if (argLookup.ContainsKey(valueString))
								throw new InvalidOperationException();

							index += 1;
							state = CommandState.StartValue;
							value.Clear();
							argLookup.Add(valueString, new List<string>());
							key = valueString;
						}
						else if (char.IsLetterOrDigit(commandLine[index]))
						{
							value.Append(commandLine[index]);
							index += 1;
						}
						else
						{
							throw new InvalidOperationException();
						}
						break;
					case CommandState.StartValue:
						if (commandLine[index] == ' ')
						{
							throw new InvalidOperationException();
						}
						else if (commandLine[index] == ',')
						{
							// Empty string in an array.
							index += 1;
							argLookup[key].Add(string.Empty);
						}
						else if (commandLine[index] == '"')
						{
							index += 1;
							state = CommandState.Quote;
						}
						else
						{
							value.Append(commandLine[index]);
							index += 1;
							state = CommandState.Value;
						}
						break;
					case CommandState.Value:
						if (commandLine[index] == ' ')
						{
							// Finished with all the value.
							argLookup[key].Add(value.ToString());
							index += 1;
							value.Clear();
							key = null;
							state = CommandState.Flag;
						}
						else if (commandLine[index] == ',')
						{
							// Finished with the value.
							argLookup[key].Add(value.ToString());
							index += 1;
							value.Clear();
							state = CommandState.StartValue;
						}
						else
						{
							value.Append(commandLine[index]);
							index += 1;
						}
						break;
					case CommandState.Quote:
						if (IsNextWord(commandLine, index, "\" "))
						{
							// Finished with the quoted value and all the other values.
							argLookup[key].Add(value.ToString());
							index += 2;
							value.Clear();
							key = null;
							state = CommandState.Flag;
						}
						else if (IsNextWord(commandLine, index, "\","))
						{
							// Finished with the quoted value.
							argLookup[key].Add(value.ToString());
							index += 2;
							value.Clear();
							state = CommandState.StartValue;
						}
						else if (IsNextWord(commandLine, index, "\\\""))
						{
							// Escaped quote, so convert to normal quote.
							value.Append('"');
							index += 2;
						}
						else if(commandLine[index] == '"')
						{
							// Finished with the quoted value and all the other values.
							argLookup[key].Add(value.ToString());
							index += 1;
							value.Clear();
							key = null;
							state = CommandState.Flag;
						}
						else 
						{
							value.Append(commandLine[index]);
							index += 1;
						}
						break;
				}
			}

			if(value.Length != 0)
			{
				if(key != null)
				{
					// Array or single item.
					if (!argLookup.ContainsKey(key))
						argLookup.Add(key, new List<string>());
					argLookup[key].Add(value.ToString());
				}
				else
				{
					// Flag
					string valueString = value.ToString();
					if (argLookup.ContainsKey(valueString))
						throw new InvalidOperationException();
					argLookup.Add(valueString, null);
				}
			}

			Dictionary<string, string[]> returnLookup = new Dictionary<string, string[]>();
			foreach (string foundKey in argLookup.Keys)
			{
				if (argLookup[foundKey] == null)
					returnLookup.Add(foundKey, null);
				else
					returnLookup.Add(foundKey, argLookup[foundKey].ToArray());
			}
			return returnLookup;
		}

		private static Dictionary<string, string[]> ValidateArgLookup(Dictionary<string, string[]> argLookup, Dictionary<string, ArgumentInfo> flags, Dictionary<string, ArgumentInfo> arrays, Dictionary<string, ArgumentInfo> singles)
		{
			// Validate that all keys are accounted for and have the right number of values.
			List<string> keys = new List<string>(argLookup.Keys);
			foreach(string key in keys)
			{
				if(singles.ContainsKey(key))
				{
					if (argLookup[key] == null || argLookup[key].Length != 1)
						throw new InvalidOperationException();
				}
				else if(arrays.ContainsKey(key))
				{
					if (argLookup[key] == null || argLookup[key].Length < 1)
						throw new InvalidOperationException();
				}
				else if(flags.ContainsKey(key))
				{
					if(argLookup[key] != null)
						throw new InvalidOperationException();
				}
				else
				{
					if (argLookup[key] == null)
					{
						// See if it is a group of flags.
						foreach (char character in key)
						{
							string newKey = character.ToString();
							if (flags.ContainsKey(newKey))
							{
								if(argLookup.ContainsKey(newKey))
									throw new InvalidOperationException();
								argLookup.Add(newKey, null);
							}
							else
							{
								throw new InvalidOperationException();
							}
						}
						argLookup.Remove(key);
					}
					else
					{
						throw new InvalidOperationException();
					}
				}
			}

			return argLookup;
		}

		public static void PopulateSettings(T settingsObject)
		{
			if (settingsObject == null)
				throw new ArgumentNullException("settingsObject");

			Dictionary<string, string[]> argLookup = ParseArguments(Environment.CommandLine);

			Dictionary<string, ArgumentInfo> flagPropLookup;
			Dictionary<string, ArgumentInfo> arrayPropLookup;
			Dictionary<string, ArgumentInfo> singleItemPropLookup;
			GetLookupTables(out flagPropLookup, out arrayPropLookup, out singleItemPropLookup);

			argLookup = ValidateArgLookup(argLookup, flagPropLookup, arrayPropLookup, singleItemPropLookup);

			foreach(string arg in argLookup.Keys)
			{
				if(singleItemPropLookup.ContainsKey(arg))
				{
					// Single Item.
					singleItemPropLookup[arg].Prop.SetValue(settingsObject, Convert.ChangeType(argLookup[arg][0], singleItemPropLookup[arg].Prop.PropertyType));
				}
				else if(arrayPropLookup.ContainsKey(arg))
				{
					// Array.
					arrayPropLookup[arg].Prop.SetValue(settingsObject, Convert.ChangeType(argLookup[arg], arrayPropLookup[arg].Prop.PropertyType));
				}
				else
				{
					// Boolean.
					flagPropLookup[arg].Prop.SetValue(settingsObject, true);
				}
			}

			/*foreach (string arg in args)
			{
				string[] splits = arg.Split('=');
				if (splits.Length > 1)
				{
					// Not a boolean value.
					if (singleItemPropLookup.ContainsKey(splits[0]))
					{
						// Single item.
						singleItemPropLookup[splits[0]].Prop.SetValue(settingsObject, Convert.ChangeType(arg.Substring(splits[0].Length + 1), singleItemPropLookup[splits[0]].Prop.PropertyType));
					}
					else if (arrayPropLookup.ContainsKey(splits[0]))
					{
						// Array.
						string[] array = ParseArray(arg.Substring(splits[0].Length + 1));
						arrayPropLookup[splits[0]].Prop.SetValue(settingsObject, Convert.ChangeType(array, arrayPropLookup[splits[0]].Prop.PropertyType));
					}
					else
					{
						throw new InvalidOperationException(string.Format("The flag specified: {0} is not a recognized flag.", splits[0]));
					}
				}
				else
				{
					// Boolean value.
					if (!flagPropLookup.ContainsKey(arg))
						throw new InvalidOperationException(string.Format("The flag specified: {0} is not a recognized flag.", arg));
					flagPropLookup[arg].Prop.SetValue(settingsObject, true);
				}
			}*/
		}

		/*private static string[] ParseArray(string value)
		{
			value = value.Trim();
			if(value[0] == '"' && value[value.Length-1] == '"')
			{
				// Items are in quotes.
				value = value.Trim('"');
				return value.Split(new string[] { "\",\"" }, StringSplitOptions.None);
			}
			return value.Split(',');
		}*/

		/// <summary>
		///   Gets the <see cref="Usage"/> <see cref="Attribute"/> from the provided class type.
		/// </summary>
		/// <param name="source"><see cref="Type"/> of the settings class.</param>
		/// <returns>Usage <see cref="Attribute"/> found on the class type.</returns>
		/// <exception cref="InvalidOperationException">
		///   There was more than one usage attribute applied to the class, the class type does not contain a usage attribute,
		///   or the usage attribute type could not be loaded.
		/// </exception>
		private static Attribute GetUsageAttribute(Type source)
		{
			Attribute attr;
			try
			{
				attr = Attribute.GetCustomAttribute(source, typeof(Usage));
			}
			catch (AmbiguousMatchException e)
			{
				throw new InvalidOperationException("More than one 'Usage' attribute is tied to the settings object class.", e);
			}
			catch (TypeLoadException e)
			{
				throw new InvalidOperationException("An error occurred while loading the attribute type for the 'Usage' attribute tied to the settings object class.", e);
			}

			if (attr == null)
				throw new InvalidOperationException("The settings object's class does not contain a 'Usage' attribute.");
			return attr;
		}

		/// <summary>
		///   Gets the <see cref="Argument"/> <see cref="Attribute"/> from the provided <see cref="PropertyInfo"/>.
		/// </summary>
		/// <param name="source"><see cref="PropertyInfo"/> object pulled from a property in a settings class.</param>
		/// <returns>
		///   <see cref="Argument"/> <see cref="Attribute"/> associated with the property or null if not attribute was applied
		///   to the property.
		/// </returns>
		private static Attribute GetArgumentAttribute(PropertyInfo source)
		{
			Attribute attr;
			try
			{
				attr = Attribute.GetCustomAttribute(source, typeof(Argument));
			}
			catch (AmbiguousMatchException e)
			{
				throw new InvalidOperationException("More than one 'Argument' attribute is tied to the property.", e);
			}
			catch (TypeLoadException e)
			{
				throw new InvalidOperationException("An error occurred while loading the attribute type for the 'Argument' attribute tied to the property.", e);
			}
			return attr;
		}

		/// <summary>
		///   Adds a single flag object to the help text.
		/// </summary>
		/// <param name="sb"></param>
		/// <param name="arg"></param>
		/// <param name="prop"></param>
		private static void AddFlagPropertyToHelpText(StringBuilder sb, Argument arg, PropertyInfo prop)
		{
			sb.AppendLine("\t" + GetKeyString(arg));
			if (!arg.Required)
				sb.AppendLine("\t\t[Optional] - " + arg.GetDescription());
			else
				sb.AppendLine("\t\t" + arg.GetDescription());
		}

		private static void AddProperty(StringBuilder sb, Argument arg, PropertyInfo prop)
		{
			sb.AppendLine("\t" + GetKeyString(arg) + "=" + prop.Name);
			if (!arg.Required)
				sb.AppendLine("\t\t[Optional] - " + arg.GetDescription());
			else
				sb.AppendLine("\t\t" + arg.GetDescription());
		}

		private static void AddArrayProperty(StringBuilder sb, Argument arg, PropertyInfo prop)
		{
			sb.AppendLine("\t" + GetKeyString(arg) + "=" + prop.Name + ",...");
			if (!arg.Required)
				sb.AppendLine("\t\t[Optional] - " + arg.GetDescription());
			else
				sb.AppendLine("\t\t" + arg.GetDescription());
		}

		private static string[] GetKeys(Argument arg)
		{
			List<string> list = new List<string>(4);
			list.Add(arg.GetSingleChar().ToString());
			if(!string.IsNullOrEmpty(arg.Word))
				list.Add(arg.Word);
			return list.ToArray();
		}

		private static string GetKeyString(Argument arg)
		{
			StringBuilder sb = new StringBuilder();
			string[] keys = GetKeys(arg);
			bool first = true;
			foreach(string key in keys)
			{
				if(first)
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
	}
}
