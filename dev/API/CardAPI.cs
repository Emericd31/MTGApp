using BlazorApp.Data;
using BlazorApp.Pages;
using Newtonsoft.Json;

namespace BlazorApp.API
{
	/// <summary>Class that handles the retrieval of data related to card.</summary>
	public static class CardAPI
	{
		#region Private Methods

		/// <summary>Gets cards by URL.</summary>
		/// <param name="url"></param>
		/// <param name="limit"></param>
		/// <returns></returns>
		private static async Task<(bool hasMoreResult, string urlResult, List<Card>, long totalCards)> GetCardsByURL(string url, int limit = -1)
		{
			return Task.Run(async () =>
			{
				var cardsResult = new List<Card>();

				using var client = new HttpClient();


				var uri = new Uri(url);
				var requestResult = client.GetStringAsync(uri).Result;

				JsonTextReader reader = new JsonTextReader(new StringReader(requestResult));
				dynamic? deserialized = JsonConvert.DeserializeObject(requestResult);

				long totalCards = ((long)deserialized.total_cards.Value);

				bool hasMore = deserialized.has_more.Value;
				if (limit > 0 && totalCards > limit)
					hasMore = false;

				var nextPage = "";
				if (hasMore)
					nextPage = deserialized.next_page.Value;


				var cards = deserialized.data;

				foreach (var card in cards)
				{
					if (card != null)
					{
						var imgUrl = card.image_uris?.normal.Value;
						if (imgUrl == null)
						{
							imgUrl = card.card_faces[0]?.image_uris?.normal.Value;
						}
						var cardName = card.name.Value;

						var cardId = card.id.Value;

						Price cardPrice = new Price(card.prices.usd.Value, card.prices.usd_foil.Value, card.prices.eur.Value, card.prices.eur_foil.Value);

						var setCode = card.set.Value;

						cardsResult.Add(new Card(cardId, cardName, imgUrl, cardPrice, setCode));
					}
				}

				return (hasMoreResult: hasMore, uriResult: nextPage, cardsResult, totalCards);
			}).Result;
		}

		#endregion

		#region Public Methods

		/// <summary>Gets cards given an expansion code.</summary>
		/// <param name="setCode">Three letters expansion code.</param>
		/// <returns>List of cards.</returns>
		public static async Task<(long totalCards, List<Card> cards)> GetCards(string setCode)
		{
			return Task.Run(async () =>
			{
				var cards = new List<Card>();
				long totalCards = 0;

				bool hasMore = true;
				var url = $"https://api.scryfall.com/cards/search?include_extras=true&include_variations=true&order=set&q=e%3A{setCode}&unique=prints";

				while (hasMore)
				{
					(bool hasMoreResult, string urlResult, List<Card> cards, long totalCards) list = await GetCardsByURL(url).ConfigureAwait(false);
					hasMore = list.hasMoreResult;
					url = list.urlResult;
					totalCards = list.totalCards;
					cards.AddRange(list.cards);

				}

				return (totalCards, cards);
			}).Result;
		}

		/// <summary>Searches card given it's code and it's set code.</summary>
		/// <param name="cardNumber">Card code.</param>
		/// <param name="setCode">Set code.</param>
		/// <returns>A tuple representing the list of cards and the total number of cards found.</returns>
		public static async Task<(List<Card> cards, long totalCards)> SearchCards(string cardNumber, string setCode)
		{
			return Task.Run(async () =>
			{
				try
				{
					var cards = new List<Card>();
					long totalCards = 0;

					bool hasMore = true;
					var url = $"https://api.scryfall.com/cards/search?q=number%3A{cardNumber}+set%3A{setCode}";

					while (hasMore)
					{
						(bool hasMoreResult, string urlResult, List<Card> cards, long totalCards) list = await GetCardsByURL(url).ConfigureAwait(false);
						hasMore = list.hasMoreResult;
						url = list.urlResult;
						totalCards = list.totalCards;
						cards.AddRange(list.cards);

					}
					
					return (cards, totalCards);
				}
				catch
				{
					return (new List<Card>(), 0);
				}

			}).Result;
		}

		/// <summary>Gets a card given it's set code and MTG code.</summary>
		/// <param name="setCode">Three letters expansion code.</param>
		/// <param name="mtgCode">Magic The Gathering code.</param>
		/// <returns>A card.</returns>
		public static async Task<Card?> GetCard(string setCode, string mtgCode)
		{
			return Task.Run(async () =>
			{
				try
				{
					Card? result = null;
					using var client = new HttpClient();

					var uri = new Uri("https://api.scryfall.com/cards/" + setCode.ToLower() + "/" + mtgCode);
					var requestResult = client.GetStringAsync(uri).Result;

					JsonTextReader reader = new JsonTextReader(new StringReader(requestResult));
					dynamic? card = JsonConvert.DeserializeObject(requestResult);

					if (card != null)
					{
						var imgUrl = card.image_uris?.normal.Value;
						if (imgUrl == null)
						{
							imgUrl = card.card_faces[0]?.image_uris?.normal.Value;
						}
						var cardName = card.name.Value;

						var cardId = card.id.Value;

						Price cardPrice = new Price(card.prices.usd.Value, card.prices.usd_foil.Value, card.prices.eur.Value, card.prices.eur_foil.Value);
						var setCode = card.set.Value;

						result = new Card(cardId, cardName, imgUrl, cardPrice, setCode);
					}
					return result;
				}
				catch
				{
					return null;
				}
			}).Result;
		}

		/// <summary>Searches cards containing a specific string in it's name.</summary>
		/// <param name="searchInput">String value.</param>
		/// <param name="limit">Maximum number of cards before cancelling the search action.</param>
		/// <returns>A tuple representing the list of cards and the total number of cards found.</returns>
		public static async Task<(List<Card> cards, long totalCards)> SearchCards(string searchInput, int limit = -1)
		{
			return Task.Run(async () =>
			{
				try
				{

					var cards = new List<Card>();
					long totalCards = 0;

					bool hasMore = true;
					var url = "https://api.scryfall.com/cards/search?q=" + searchInput + "&unique=prints";

					while (hasMore)
					{
						(bool hasMoreResult, string urlResult, List<Card> cards, long totalCards) list = await GetCardsByURL(url, limit).ConfigureAwait(false);
						hasMore = list.hasMoreResult;
						url = list.urlResult;
						cards.AddRange(list.cards);
						totalCards = list.totalCards;
					}

					return (cards, totalCards);
				}
				catch
				{
					return (new List<Card>(), 0);
				}
			}).Result;

		}

		#endregion

	}
}
