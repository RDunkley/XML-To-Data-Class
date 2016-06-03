using CSCodeGen;

namespace XMLToDataClass.Data.Types
{
	public interface IDataType
	{
		#region Properties

		/// <summary>
		///   String of the C# representive data type.
		/// </summary>
		string DataTypeString { get; }

		/// <summary>
		///   Name of the data type which can be displayed to the user.
		/// </summary>
		string DisplayName { get; }

		/// <summary>
		///   True if the data type is nullable, false otherwise.
		/// </summary>
		bool IsNullable { get; }

		/// <summary>
		///   <see cref="DataType"/> of this data type.
		/// </summary>
		DataType Type { get; }

		#endregion properties

		#region Methods

		/// <summary>
		///   Generates additional enumerations used by the import or export methods.
		/// </summary>
		/// <returns><see cref="EnumInfo"/> array represnenting additional fields needed by the import/export methods. Can be empty.</returns>
		/// <exception cref="InvalidOperationException">An attempt was made to generate the enumerations, but the current objects settings are not in a state in which this is possible.</exception>
		EnumInfo[] GenerateAdditionalEnums();

		/// <summary>
		///   Generates additional properties used by the import or export methods.
		/// </summary>
		/// <returns><see cref="PropertyInfo"/> array representing additional properties needed by the import/export methods. Can be empty.</returns>
		/// <remarks>These properties are typically used to persist state from import to export.</remarks>
		/// <exception cref="InvalidOperationException">An attempt was made to generate the properties, but the current objects settings are not in a state in which this is possible.</exception>
		PropertyInfo[] GenerateAdditionalProperties();

		/// <summary>
		///   Generates the export method code for the data type.
		/// </summary>
		/// <returns>String array representing the export method code.</returns>
		/// <exception cref="InvalidOperationException">An attempt was made to generate the code, but the current objects settings are not in a state in which this is possible.</exception>
		string[] GenerateExportMethodCode();

		/// <summary>
		///   Generates the import method code for the data type.
		/// </summary>
		/// <returns>String array representing the import method code.</returns>
		/// <exception cref="InvalidOperationException">An attempt was made to generate the code, but the current objects settings are not in a state in which this is possible.</exception>
		string[] GenerateImportMethodCode();

		/// <summary>
		///   Returns a list of the possible values that are invalid based on the current data type settings.
		/// </summary>
		/// <returns>Array of invalid values. Can be empty if no invalid values are found.</returns>
		string[] GetInvalidValues();

		/// <summary>
		///   Determines whether the data type can parse all the possible values or not.
		/// </summary>
		/// <returns>True if it can't parse all the values, false otherwise.</returns>
		bool HasInvalidValues();

		/// <summary>
		///   Attempts to parse a value to the data type, using it's current settings.
		/// </summary>
		/// <param name="value">String value to be parsed.</param>
		/// <returns>True if the string can be parsed to this value type, false otherwise.</returns>
		bool TryParse(string value);

		#endregion Methods
	}
}
