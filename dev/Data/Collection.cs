namespace BlazorApp.Data
{
	/// <summary>Collection object.</summary>
	public class Collection
	{
		/// <summary>Name.</summary>
		public string Name { get; set; } = "";

		/// <summary>Description.</summary>
		public string Description { get; set; } = "";

		/// <summary>Total number of cards (by print).</summary>
		public int NbCards { get; set; }

		/// <summary>Price of the collection in euro.</summary>
		public float EURPrice { get; set; }

		/// <summary>Number of cards without their euros prices.</summary>
		public int EURCardNotValued { get; set; }

		/// <summary>Price of the collection in dollars US.</summary>
		public float USDPrice { get; set; }

		/// <summary>Number of cards without their dollars US prices.</summary>
		public int USDCardNotValued { get; set; }

		/// <summary>
		/// Dictionary containing all cards in collection.
		/// Key : unique identifier of the card.
		/// Value : tuple with the card and the number of copies of this card.
		/// </summary>
		public ObservableDictionary<string, (Card card, int nbCard)> Cards { get; set; }

		/// <summary>Constructor.</summary>
		/// <param name="name">Name.</param>
		/// <param name="description">Description.</param>
		public Collection(string name, string description)
		{
			Name = name;
			Description = description;
			NbCards = 0;
			Cards = new ObservableDictionary<string, (Card, int)>();
		}

		/// <summary>Manages card in collection.</summary>
		/// <param name="card">Card.</param>
		/// <param name="nbCard">NbCard.</param>
		/// <param name="action">Action.</param>
		public void ManageCard(Card card, int nbCard, ECollectionAction action)
		{
			if (card == null || nbCard <= 0)
				return;

			int multipler = 1;
			switch (action)
			{
				case ECollectionAction.ADD:
					multipler = 1;
					break;
				case ECollectionAction.REMOVE:
					multipler = -1;
					break;
				default:
					break;
			}

			var set = DataService.Instance.Sets.FirstOrDefault(set => set.Code == card.SetCode);
			if (set == null)
				return;

			NbCards += nbCard * multipler;
			set.CardInCollection += nbCard * multipler;
			ManageEuroPrice(card, nbCard, multipler);
			ManageDollarPrice(card, nbCard, multipler);

			switch (action)
			{
				case ECollectionAction.ADD:
					if (Cards.ContainsKey(card.UID))
					{
						var currentNbCard = Cards[card.UID].nbCard;
						Cards[card.UID] = (card, currentNbCard + (nbCard * multipler));
					}
					else
						Cards.Add(card.UID, (card, nbCard * multipler));
					break;
				case ECollectionAction.REMOVE:
					if (Cards.ContainsKey(card.UID))
					{
						var currentNbCard = Cards[card.UID].nbCard;
						if (currentNbCard == nbCard)
							Cards.Remove(card.UID);
						else
							Cards[card.UID] = (card, currentNbCard + (nbCard * multipler));
					}
					break;
				default:
					break;
			}
		}

		/// <summary>Manages euros price.</summary>
		/// <param name="card">Card.</param>
		/// <param name="nbCard">NbCard.</param>
		/// <param name="multiplier">Multiplier.</param>
		private void ManageEuroPrice(Card card, int nbCard, int multiplier)
		{
			if (float.TryParse(card.Prices?.EUR?.Replace('.', ','), out float eurPrice))
				EURPrice += (eurPrice * nbCard) * multiplier;
			else
				EURCardNotValued += nbCard * multiplier;
		}

		/// <summary>Manages dollars price.</summary>
		/// <param name="card">Card.</param>
		/// <param name="nbCard">NbCard.</param>
		/// <param name="multiplier">Multiplier.</param>
		private void ManageDollarPrice(Card card, int nbCard, int multiplier)
		{
			if (float.TryParse(card.Prices?.USD?.Replace('.', ','), out float usdPrice))
				USDPrice += (usdPrice * nbCard) * multiplier;
			else
				USDCardNotValued += nbCard * multiplier;
		}
	}
}
