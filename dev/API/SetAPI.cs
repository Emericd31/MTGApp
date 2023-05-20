using BlazorApp.Data;
using Newtonsoft.Json;

namespace BlazorApp.API
{
	/// <summary>Class that handles the retrieval of data related to expansion.</summary>
	public static class SetAPI
	{
		#region Public Methods

		/// <summary>Gets all available expansions.</summary>
		/// <returns>List of available expansions.</returns>
		public static async Task<List<Set>> GetSets()
		{
			return Task.Run(() =>
			{
				var images = new List<Set>();
				using var client = new HttpClient();


				var uri = new Uri("https://api.scryfall.com/sets/");


				var result = client.GetStringAsync(uri).Result;

				JsonTextReader reader = new JsonTextReader(new StringReader(result));
				dynamic? deserialized = JsonConvert.DeserializeObject(result);

				var sets = deserialized.data;

				foreach (var set in sets)
				{
					if (set != null)
					{
						var imgUrl = set.icon_svg_uri.Value;
						var setName = set.name.Value;
						var setCode = set.code.Value;
						var setTypeValue = set.set_type.Value;
						ESetType setType;
						switch (setTypeValue)
						{
							case "expansion":
								setType = ESetType.EXPANSION;
								break;
							case "promo":
								setType = ESetType.PROMO;
								break;
							case "commander":
								setType = ESetType.COMMANDER;
								break;
							case "funny":
								setType = ESetType.FUNNY;
								break;
							case "token":
								setType = ESetType.TOKEN;
								break;
							default:
								setType = ESetType.OTHERS;
								break;
						}

						var releaseDate = set.released_at.Value;
						var test = releaseDate.GetType();
						var cardCount = set.card_count.Value;
						var parentSetCode = set.parent_set_code?.Value ?? "";
						var isDigital = set.digital.Value;

						images.Add(new Set(setName, imgUrl, setCode, releaseDate, setType, cardCount, parentSetCode, isDigital));
					}
				}
				return images;
			}).Result;
		}

		#endregion
	}
}
