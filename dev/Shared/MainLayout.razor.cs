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
				await DataService.Instance.InitializeData().ConfigureAwait(false);

				var currentFolder = Directory.GetCurrentDirectory();
				DataService.Instance.ApplicationDataFolder = Path.Combine(currentFolder, "UserData");

				if (!Directory.Exists(DataService.Instance.ApplicationDataFolder))
					Directory.CreateDirectory(DataService.Instance.ApplicationDataFolder);

				var collectionFilePath = Path.Combine(DataService.Instance.ApplicationDataFolder, "MyCollection.json");

				if (!File.Exists(collectionFilePath))
					JsonImportExport.SaveCollection(collectionFilePath);
				else
					JsonImportExport.ImportCollection(collectionFilePath);
				DataService.Instance.HasBeenInitialized = true;
			}
		}
	}
}
