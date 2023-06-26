using BlazorApp.Data;
using Newtonsoft.Json;

namespace BlazorApp.Helpers
{
	/// <summary>Class that handles json import/export methods</summary>
	public static class JsonImportExport
	{
		/// <summary>Imports collection from json file.</summary>
		/// <param name="jsonFilePath">Json file path.</param>
		public static void ImportCollection(string jsonFilePath)
		{
			using (StreamReader r = new StreamReader(jsonFilePath))
			{
				string json = r.ReadToEnd();
				JsonTextReader reader = new JsonTextReader(new StringReader(json));

				var collectionFile = JsonConvert.DeserializeObject<Collection>(json);
				if (collectionFile != null)
				{
					DataService.Instance.MyCollection.Name = collectionFile.Name;
					DataService.Instance.MyCollection.Description = collectionFile.Description;
					foreach (var card in collectionFile.Cards)
						DataService.Instance.MyCollection.AddOrRemoveCard(card.Value.card, card.Value.nbCard, ECollectionAction.ADD);
				}
			}
		}

		/// <summary>Creates collection into json file.</summary>
		/// <param name="jsonFilePath">Json file path.</param>
		private static void CreateCollection(string jsonFilePath)
		{
			File.Create(jsonFilePath).Close();
			var collection = JsonConvert.SerializeObject(DataService.Instance.MyCollection);

			using (var sw = new StreamWriter(jsonFilePath, true))
				sw.WriteLine(collection);
		}

		/// <summary>Saves collection into json file.</summary>
		/// <param name="jsonFilePath">Json file path.</param>
		public static void SaveCollection(string jsonFilePath)
		{
			if (!File.Exists(jsonFilePath))
				CreateCollection(jsonFilePath);
			else
			{
				var collection = JsonConvert.SerializeObject(DataService.Instance.MyCollection);
				File.WriteAllText(jsonFilePath, collection);
			}
		}

		#region Deck Import Export

		/// <summary>Import all decks found in folder path.</summary>
		/// <param name="decksFolderPath">Decks foler path.</param>
		public static void ImportAllDecks(string decksFolderPath)
		{
			string[] filePaths = Directory.GetFiles(decksFolderPath);
			foreach (string filePath in filePaths)
			{
				var fileName = Path.GetFileName(filePath);
				if (fileName.Contains("deck_"))
					ImportDeck(filePath);
			}
		}

		/// <summary>Saves all decks in json files.</summary>
		/// <param name="decksFolderPath">Decks folder path.</param>
		/// <remarks>File are named as : $"deck_{deck.Id}.json"</remarks>
		/// <example>deck_EJBR3PZ9D2.json.</example>
		public static void SaveDecks(string decksFolderPath)
		{
			foreach (var deck in DataService.Instance.MyDecks)
			{
				if (!string.IsNullOrEmpty(deck.Id))
				{
					var deckFilePath = Path.Combine(decksFolderPath, $"deck_{deck.Id}.json");
					SaveDeck(deckFilePath, deck);
				}
			}
		}

		/// <summary>Imports a deck from json file.</summary>
		/// <param name="jsonFilePath">Json file path.</param>
		private static void ImportDeck(string jsonFilePath)
		{
			using (StreamReader r = new StreamReader(jsonFilePath))
			{
				string json = r.ReadToEnd();
				JsonTextReader reader = new JsonTextReader(new StringReader(json));

				if (!string.IsNullOrEmpty(json))
				{
					var collection = JsonConvert.DeserializeObject<Collection>(json);
					if (collection != null)
						DataService.Instance.MyDecks.Add(collection);
				}
			}
		}

		/// <summary>Saves deck into json file.</summary>
		/// <param name="jsonFilePath">Json file path.</param>
		/// <param name="collection">Deck to save.</param>
		public static void SaveDeck(string jsonFilePath, Collection collection)
		{
			if (!File.Exists(jsonFilePath))
				CreateDeck(jsonFilePath, collection);
			else
			{
				var jsonCollection = JsonConvert.SerializeObject(collection);
				File.WriteAllText(jsonFilePath, jsonCollection);
			}
		}

		/// <summary>Creates a deck into json file.</summary>
		/// <param name="jsonFilePath">Json file path.</param>
		/// <param name="collection">Collection to save.</param>
		private static void CreateDeck(string jsonFilePath, Collection collection)
		{
			File.Create(jsonFilePath).Close();
			var jsonCollection = JsonConvert.SerializeObject(collection);

			using (var sw = new StreamWriter(jsonFilePath, true))
				sw.WriteLine(jsonCollection);
		}

		#endregion
	}
}
