using BlazorApp.Data;
using BlazorApp.Helpers;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Shared
{
	/// <summary>Class that handles data and treatment for Main Layout component.</summary>
	public class MainLayoutBase : LayoutComponentBase
	{
		/// <summary>Method called after each rendering.</summary>
		/// <param name="firstRender">Boolean indicating if it's the first rendering.</param>
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender && !DataService.Instance.HasBeenInitialized)
			{
				try
				{
					await DataService.Instance.InitializeData().ConfigureAwait(false);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"An error occurred while initalizing data : {ex.Message} {ex.InnerException} {ex.StackTrace}");
				}

				try
				{
					var currentFolder = Directory.GetCurrentDirectory();
					DataService.Instance.ApplicationDataFolder = Path.Combine(currentFolder, "UserData");

					if (!Directory.Exists(DataService.Instance.ApplicationDataFolder))
						Directory.CreateDirectory(DataService.Instance.ApplicationDataFolder);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"An error occurred while initalizing data folder : {ex.Message} {ex.InnerException} {ex.StackTrace}");
				}

				// Import collection 
				try
				{
					if (DataService.Instance.ApplicationDataFolder != null)
					{
						var collectionFilePath = Path.Combine(DataService.Instance.ApplicationDataFolder, "MyCollection.json");

						if (!File.Exists(collectionFilePath))
							JsonImportExport.SaveCollection(collectionFilePath);
						else
							JsonImportExport.ImportCollection(collectionFilePath);
					}
					else
						Console.WriteLine($"Unable to import collection, application data folder is null");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"An error occurred while importing collection : {ex.Message} {ex.InnerException} {ex.StackTrace}");
				}

				// Import decks
				try
				{
					if (DataService.Instance.ApplicationDataFolder != null)
					{
						var deckFolderPath = Path.Combine(DataService.Instance.ApplicationDataFolder, "Decks");
						if (!Directory.Exists(deckFolderPath))
							Directory.CreateDirectory(deckFolderPath);
						JsonImportExport.ImportAllDecks(deckFolderPath);
					}
					else
						Console.WriteLine($"Unable to import decks, application data folder is null");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"An error occurred while importing decks : {ex.Message} {ex.InnerException} {ex.StackTrace}");
				}

				DataService.Instance.HasBeenInitialized = true;
			}
		}
	}
}
