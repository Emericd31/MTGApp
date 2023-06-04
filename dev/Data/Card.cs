namespace BlazorApp.Data
{
	/// <summary>Card object.</summary>
	public class Card
	{
		/// <summary>Name.</summary>
		public string Name { get; set; }

		/// <summary>URL of the image card.</summary>
		public string ImgUrl { get; set; }

		/// <summary>Prices.</summary>
		public Price Prices { get; set; }

		/// <summary>Unique identifier.</summary>
		public string UID { get; set; }

		/// <summary>Expansion code.</summary>
		public string SetCode { get; set; }

		/// <summary>Card colors.</summary>
		public List<ECardColor> Colors { get; set; }

		/// <summary>Constructor.</summary>
		/// <param name="uid">Unique identifier.</param>
		/// <param name="name">Name.</param>
		/// <param name="imgUrl">URL of the image card.</param>
		/// <param name="prices">Prices.</param>
		/// <param name="setCode">Expansion code.</param>
		public Card(string uid, string name, string imgUrl, Price prices, string setCode, List<ECardColor>? colors = null)
		{
			UID = uid;
			Name = name;
			ImgUrl = imgUrl;
			Prices = prices;
			SetCode = setCode;
			Colors = colors == null ? new List<ECardColor>() : colors;
		}
	}
}
