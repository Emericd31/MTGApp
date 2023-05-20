namespace BlazorApp.Data
{
	/// <summary>Price object.</summary>
	public class Price
	{
		/// <summary>US dollars card price (no foil).</summary>
		public string USD { get; set; }

		/// <summary>US dollars card price (foil).</summary>
		public string USDFoil { get; set; }

		/// <summary>Euros card price (no foil).</summary>
		public string EUR { get; set; }

		/// <summary>Euros card price (foil).</summary>
		public string EURFoil { get; set; }

		/// <summary>Constructor.</summary>
		/// <param name="uSD">US dollars card price (no foil).</param>
		/// <param name="uSDFoil">US dollars card price (foil).</param>
		/// <param name="eUR">Euros card price (no foil).</param>
		/// <param name="eURFoil">Euros card price (foil).</param>
		public Price(string uSD, string uSDFoil, string eUR, string eURFoil)
		{
			USD = uSD;
			USDFoil = uSDFoil;
			EUR = eUR;
			EURFoil = eURFoil;
		}
	}
}
