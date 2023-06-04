namespace BlazorApp.Helpers
{
	/// <summary>Class that handles object generators.</summary>
	public static class Generators
	{
		#region Private Properties

		/// <summary>Random object.</summary>
		private static Random random = new Random();

		#endregion

		#region Public Methods

		/// <summary>Generates a string.</summary>
		/// <param name="length">Length of the string to generate.</param>
		/// <returns>The generated string.</returns>
		public static string GenerateString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}

		#endregion
	}
}
