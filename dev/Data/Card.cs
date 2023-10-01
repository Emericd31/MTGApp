namespace BlazorApp.Data
{
	/// <summary>Card object.</summary>
	public class Card
	{
		/// <summary>Name.</summary>
		public string Name { get; set; }

		/// <summary>Text of the card.</summary>
		public string Text { get; set; }

		/// <summary>Artist.</summary>
		public string Artist { get; set; }

		/// <summary>URL of the image card.</summary>
		public string ImgUrl { get; set; }

		/// <summary>List of keywords.</summary>
		public List<string> Keywords { get; set; }

		/// <summary>Boolean indicating if keywords are initialized.</summary>
		public bool KeywordsInitialized { get; set; }

		/// <summary>List of card types.</summary>
		public List<ECardType> Types { get; set; }

		/// <summary>Prices.</summary>
		public Price Prices { get; set; }

		/// <summary>Unique identifier.</summary>
		public string UID { get; set; }

		/// <summary>Expansion code.</summary>
		public string SetCode { get; set; }

		/// <summary>Card colors.</summary>
		public List<ECardColor> Colors { get; set; }

		/// <summary>Card rarity.</summary>
		public ECardRarity Rarity { get; set; }

		/// <summary>Constructor.</summary>
		/// <param name="uid">Unique identifier.</param>
		/// <param name="name">Name.</param>
		/// <param name="text">Text.</param>
		/// <param name="artist">Artist.</param>
		/// <param name="imgUrl">URL of the image card.</param>
		/// <param name="prices">Prices.</param>
		/// <param name="setCode">Expansion code.</param>
		/// <param name="colors">Card colors.</param>
		/// <param name="rarity">Card rarity.</param>
		/// <param name="keywords">Keywords.</param>
		/// <param name="types">List of card types.</param>
		public Card(string uid, string name, string text, string artist, string imgUrl, Price prices, string setCode, List<ECardColor>? colors = null, ECardRarity rarity = ECardRarity.UNKNWOWN, List<string>? keywords = null, List<ECardType>? types = null)
		{
			UID = uid;
			Name = name;
			Text = text;
			Artist = artist;
			ImgUrl = imgUrl;
			Prices = prices;
			SetCode = setCode;
			Colors = colors == null ? new List<ECardColor>() : colors;
			Rarity = rarity;
			Keywords = keywords == null ? new List<string>() : keywords;
			KeywordsInitialized = true;
			Types = types == null ? new List<ECardType>() : types;
		}
	}
}
