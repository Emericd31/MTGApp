namespace BlazorApp.Data
{
	/// <summary>Expansion object.</summary>
	public class Set
	{
		/// <summary>Name.</summary>
		public string Name { get; set; }

		/// <summary>URL of the expansion logo.</summary>
		public string ImgUrl { get; set; }

		/// <summary>Three letters code of the expansion.</summary>
		public string Code { get; set; }

		/// <summary>Release date.</summary>
		public DateTime? ReleaseDate { get; set; }

		/// <summary>Type.</summary>
		public ESetType SetType { get; set; }

		/// <summary>Number of cards in the expansion.</summary>
		public long CardCount { get; set; }

		/// <summary>Parent expansion three letters code (empty if none).</summary>
		public string ParentSetCode { get; set; }

		/// <summary>Boolean indicating if the expansion is digital.</summary>
		public bool IsDigital { get; set; }

		/// <summary>Number of cards in the collection.</summary>
		public int CardInCollection { get; set; }

		/// <summary>Constructor.</summary>
		/// <param name="name">Name.</param>
		/// <param name="imgUrl">URL of the expansion logo.</param>
		/// <param name="code">Three letters code of the expansion.</param>
		/// <param name="releaseDate">Release date.</param>
		/// <param name="setType">Type.</param>
		/// <param name="cardCount">Number of cards in the expansion.</param>
		/// <param name="parentSetCode">Parent expansion three letters code (empty if none).</param>
		/// <param name="isDigital">Boolean indicating if the expansion is digital.</param>
		public Set(string name, string imgUrl, string code, string releaseDate, ESetType setType, long cardCount, string parentSetCode, bool isDigital)
		{
			Name = name;
			ImgUrl = imgUrl;
			Code = code;
			if (DateTime.TryParse(releaseDate, out DateTime releaseDateR))
			{
				ReleaseDate = releaseDateR;
			}
			SetType = setType;
			CardCount = cardCount;
			ParentSetCode = parentSetCode;
			IsDigital = isDigital;
			CardInCollection = 0;
		}
	}
}
