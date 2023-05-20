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

				Collection collectionFile = JsonConvert.DeserializeObject<Collection>(json);
				if (collectionFile != null)
				{
					DataService.Instance.MyCollection.Name = collectionFile.Name;
					DataService.Instance.MyCollection.Description = collectionFile.Description;
					foreach (var card in collectionFile.Cards)
						DataService.Instance.MyCollection.ManageCard(card.Value.card, card.Value.nbCard, ECollectionAction.ADD);
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
	}
}
