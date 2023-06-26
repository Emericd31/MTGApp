using BlazorApp.Pages;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace BlazorApp.Data
{
	/// <summary>Collection object.</summary>
	public class Collection
	{
		/// <summary>Unique identifier.</summary>
		public string? Id { get; set; }

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

		/// <summary>Collection card colors.</summary>
		public List<ECardColor> Colors { get; set; }

		/// <summary>
		/// Dictionary containing all cards in collection.
		/// Key : unique identifier of the card.
		/// Value : tuple with the card and the number of copies of this card.
		/// </summary>
		public ObservableDictionary<string, (Card card, int nbCard)> Cards { get; set; }

		/// <summary>Collection artwork.</summary>
		public Artwork? Artwork { get; set; }

		/// <summary>Constructor.</summary>
		/// <param name="name">Name.</param>
		/// <param name="description">Description.</param>
		/// <param name="artwork">Collection artwork.</param>
		public Collection(string name, string description, Artwork? artwork = null)
		{
			Name = name;
			Description = description;
			NbCards = 0;
			Cards = new ObservableDictionary<string, (Card, int)>();
			Colors = new List<ECardColor>();
			if (artwork != null)
				Artwork = artwork;
		}

		/// <summary>Adds/Removes card in collection.</summary>
		/// <param name="card">Card.</param>
		/// <param name="nbCard">NbCard.</param>
		/// <param name="action">Action.</param>
		public void AddOrRemoveCard(Card card, int nbCard, ECollectionAction action)
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

		/// <summary>Edits a card.</summary>
		/// <param name="cardUID">Card unique identifier.</param>
		/// <param name="newCard">New card values.</param>
		/// <param name="updateColor">Boolean indicating if colors should be updated.</param>
		/// <param name="updateRarity">Boolean indicating if rarity should be updated.</param>
		/// <param name="updatePrice">Boolean indicating if price should be updated.</param>
		/// <param name="updateText">Boolean indicating if text should be updated.</param>
		/// <param name="updateKeywords">Boolean indicating if keywords should be updated.</param>
		/// <param name="updateArtist">Boolean indicating if artist should be updated.</param>
		public void EditCard(string cardUID, Card newCard, bool updateColor = true, bool updateRarity = true, bool updatePrice = true, bool updateText = true, bool updateKeywords = true, bool updateArtist = true)
		{
			if (string.IsNullOrEmpty(cardUID))
				return;

			if ((updateText || updateColor || updateRarity || updatePrice || updateKeywords || updateArtist) && newCard != null && Cards.ContainsKey(cardUID))
			{
				var currentCard = Cards[cardUID];

				// Updates card text if necesary
				if (updateText)
					currentCard.card.Text = string.IsNullOrEmpty(newCard.Text) ? "No text" : newCard.Text;

				// Updates colors if necesary
				if (updateColor && currentCard.card.Colors.Count() == 0)
				{
					Console.WriteLine("J'update la couleurs");
					if (newCard.Colors.Count() == 0)
						currentCard.card.Colors.Add(ECardColor.ARTIFACT);
					else
						currentCard.card.Colors = newCard.Colors;
				}

				// Updates keyword if necesary
				if (updateKeywords)
				{
					currentCard.card.Keywords = newCard.Keywords;
					currentCard.card.KeywordsInitialized = true;
				}

				// Updates rarity
				if (updateRarity)
					currentCard.card.Rarity = newCard.Rarity;

				// Updates artist
				if (updateArtist)
					currentCard.card.Artist = newCard.Artist;

				// Calculates new prices
				if (updatePrice && newCard.Prices != null)
				{
					Console.WriteLine("J'update le prix");
					var newPrices = CalculatesNewCardPrices(currentCard, newCard);
					EURPrice += newPrices.EURPrice;
					USDPrice += newPrices.USDPrice;

					currentCard.card.Prices = newCard.Prices;
				}

				Cards[cardUID] = currentCard;
			}
		}

		/// <summary>Calculates new card price influence on collection total price.</summary>
		/// <param name="currentCardData">Current card date.</param>
		/// <param name="newCard">New card values.</param>
		/// <returns>The difference between the old and the new price.</returns>
		private (float EURPrice, float USDPrice) CalculatesNewCardPrices((Card currentCard, int nbCard) currentCardData, Card newCard)
		{
			(float EURPrice, float USDPrice) result = (0.0f, 0.0f);

			if (currentCardData.currentCard != null && newCard != null)
			{
				// EUR Prices
				bool currentCardHasEURValue = false;
				if (float.TryParse(currentCardData.currentCard.Prices?.EUR?.Replace('.', ','), out float currentEURPrice))
				{
					result.EURPrice -= (currentEURPrice * currentCardData.nbCard);
					currentCardHasEURValue = true;
				}

				bool newCardHasEURValue = false;
				if (float.TryParse(newCard.Prices?.EUR?.Replace('.', ','), out float newEURPrice))
				{
					result.EURPrice += (newEURPrice * currentCardData.nbCard);
					newCardHasEURValue = true;
				}

				if (!currentCardHasEURValue && newCardHasEURValue)
					EURCardNotValued -= currentCardData.nbCard;
				else if (currentCardHasEURValue && !newCardHasEURValue)
					EURCardNotValued += currentCardData.nbCard;

				// USD Prices
				bool currentCardHasUSDValue = false;
				if (float.TryParse(currentCardData.currentCard.Prices?.USD?.Replace('.', ','), out float currentUSDPrice))
				{
					result.USDPrice -= (currentUSDPrice * currentCardData.nbCard);
					currentCardHasUSDValue = true;
				}

				bool newCardHasUSDValue = false;
				if (float.TryParse(newCard.Prices?.USD?.Replace('.', ','), out float newUSDPrice))
				{
					result.USDPrice += (newUSDPrice * currentCardData.nbCard);
					newCardHasUSDValue = true;
				}

				if (!currentCardHasUSDValue && newCardHasUSDValue)
					USDCardNotValued -= currentCardData.nbCard;
				else if (currentCardHasUSDValue && !newCardHasUSDValue)
					USDCardNotValued += currentCardData.nbCard;


				if (result.EURPrice != 0.0f)
				{

				}

				if (result.USDPrice != 0.0f)
				{

				}
			}

			return result;
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
