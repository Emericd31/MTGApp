namespace BlazorApp.Helpers
{
	/// <summary>Class that handles css class builder.</summary>
	public static class CssClassInlineBuilder
	{
		/// <summary>Generates string containing all css classes given an object.</summary>
		/// <param name="obj">Object to extract properties (list of booleans).</param>
		/// <returns>String containing css classes.</returns>
		/// <example>
		/// bool1 = true;
		/// bool2 = false;
		/// bool3 = true;
		/// CssClass(new {bool1, bool2, bool3}) => "bool1 bool3"
		/// </example>
		public static string CssClass(object obj)
		{
			var boolPropNames = obj.GetType()
			  // Enumarate all properties of the argument object.
			  .GetProperties()
			  // Filter the properties only it's type is "bool".
			  .Where(p => p.PropertyType == typeof(bool))
			  // Filter the properties only that value is "true".
			  .Where(p => (bool)p.GetGetMethod().Invoke(obj, null))
			  // Collect names of properties, and convert it to lower case.
			  .Select(p => p.Name.ToLower());

			// Concatenate names of filtered properties with space separator.
			return string.Join(' ', boolPropNames);
		}
	}
}
