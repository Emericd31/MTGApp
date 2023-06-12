using System.Collections.Specialized;
using BlazorApp.Data;
using BlazorApp.Helpers;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Shared
{
	/// <summary>Class that handles data and treatment for Topbar component.</summary>
	public class TopbarBase : ComponentBase
	{
		#region Private Properties

		/// <summary>Number of cards in collection.</summary>
		private int NbCards => DataService.Instance.MyCollection.NbCards;

		/// <summary>Total price of the collection in euros.</summary>
		private float EURPrice => DataService.Instance.MyCollection.EURPrice;

		/// <summary>Number of cards without price in euros.</summary>
		private int EURCardNotValued => DataService.Instance.MyCollection.EURCardNotValued;

		/// <summary>Total price of the collection in dollars.</summary>
		private float USDPrice => DataService.Instance.MyCollection.USDPrice;

		/// <summary>Number of cards without price in dollars.</summary>
		private int USDCardNotValued => DataService.Instance.MyCollection.USDCardNotValued;

		/// <summary>Timer handling time when saved text is displayed.</summary>
		private System.Timers.Timer _displayTimer;

		#endregion

		#region Protected Properties

		/// <summary>Saved text.</summary>
		protected bool DisplaySaveText { get; set; }

		/// <summary>Title.</summary>
		protected string Title { get; set; } = "";

		#endregion

		#region Constructor.

		/// <summary>Constructor.</summary>
		public TopbarBase()
		{
			Title = NbCards + " Cartes possédées (" + Math.Abs(EURPrice).ToString("0.##") + "€ avec " + EURCardNotValued + " cartes sans prix, " + Math.Abs(USDPrice).ToString("0.##") + "$ avec " + USDCardNotValued + " cartes sans prix)";
			DataService.Instance.MyCollection.Cards.CollectionChanged += HandleChange;
			// Create a timer and set a two second interval.
			_displayTimer = new System.Timers.Timer();
			_displayTimer.Interval = 1500;

			// Hook up the Elapsed event for the timer. 
			_displayTimer.Elapsed += OnDisplayTimerElapsed;
		}

		#endregion

		#region Private Methods

		/// <summary>Method called when timer elapse.</summary>
		/// <param name="_">Paramter.</param>
		/// <param name="__">Parameter not used.</param>
		private void OnDisplayTimerElapsed(Object _, System.Timers.ElapsedEventArgs __)
		{
			DisplaySaveText = false;
			InvokeAsync(() => StateHasChanged());
			_displayTimer?.Stop();
		}

		/// <summary>Method called when collection changed.</summary>
		/// <param name="_">Parameter not used.</param>
		/// <param name="__">Parameter not used.</param>
		private void HandleChange(object _, NotifyCollectionChangedEventArgs __)
		{
			InvokeAsync(() =>
			{
				Title = NbCards + " Cartes possédées (" + Math.Abs(EURPrice).ToString("0.##") + "€ avec " + EURCardNotValued + " cartes sans prix, " + Math.Abs(USDPrice).ToString("0.##") + "$ avec " + USDCardNotValued + " cartes sans prix)";
				StateHasChanged();
			});
		}

		#endregion

		#region Protected Methods

		/// <summary>Method called on save button click.</summary>
		protected void Save()
		{
			if (DataService.Instance.ApplicationDataFolder != null)
			{
				var collectionFilePath = Path.Combine(DataService.Instance.ApplicationDataFolder, "MyCollection.json");
				JsonImportExport.SaveCollection(collectionFilePath);

				var deckFolderPath = Path.Combine(DataService.Instance.ApplicationDataFolder, "Decks");
				JsonImportExport.SaveDecks(deckFolderPath);

				DisplaySaveText = true;
				_displayTimer.Start();
			}
		}

		#endregion
	}
}
