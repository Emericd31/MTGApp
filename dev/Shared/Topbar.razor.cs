using System.Collections.Specialized;
using BlazorApp.API;
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

		/// <summary>Percentage of scanned cards.</summary>
		protected int Percentage { get; set; }

		/// <summary>Boolean indicating if data loading action is in progress.</summary>
		protected bool IsLoading { get; set; }

		/// <summary>Displayed text when data loading action is in progress.</summary>
		protected string? Text { get; set; }

		/// <summary>Cancellation token to stop a loading action in progress.</summary>
		protected CancellationTokenSource CancellationToken { get; set; } = new CancellationTokenSource();

		#endregion

		#region Constructor.

		/// <summary>Constructor.</summary>
		public TopbarBase()
		{
			Title = NbCards + " Cartes possédées (" + Math.Abs(EURPrice).ToString("0.##") + "€ avec " + EURCardNotValued + " cartes sans prix, " + Math.Abs(USDPrice).ToString("0.##") + "$ avec " + USDCardNotValued + " cartes sans prix)";
			DataService.Instance.MyCollection.Cards.PropertyChanged += Cards_PropertyChanged;
			// Create a timer and set a two second interval.
			_displayTimer = new System.Timers.Timer();
			_displayTimer.Interval = 1500;

			// Hook up the Elapsed event for the timer. 
			_displayTimer.Elapsed += OnDisplayTimerElapsed;
		}

		/// <summary>Method called when card property change in collection.</summary>
		/// <param name="_">Parameter not used.</param>
		/// <param name="__">Parameter not used.</param>
		private void Cards_PropertyChanged(object? _, System.ComponentModel.PropertyChangedEventArgs __)
		{
			InvokeAsync(() =>
			{
				Title = NbCards + " Cartes possédées (" + Math.Abs(EURPrice).ToString("0.##") + "€ avec " + EURCardNotValued + " cartes sans prix, " + Math.Abs(USDPrice).ToString("0.##") + "$ avec " + USDCardNotValued + " cartes sans prix)";
				StateHasChanged();
			});
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

		/// <summary>Stop loading data.</summary>
		protected void StopLoadingData()
		{
			CancellationToken.Cancel();
		}

		/// <summary>Loads missing data.</summary>
		protected async Task LoadCollectionData()
		{
			try
			{
				IsLoading = true;
				Percentage = 0;
				var currentCards = 0;
				var totalCards = DataService.Instance.MyCollection.Cards.Count();
				Text = $"0/{totalCards}";
				foreach (var card in DataService.Instance.MyCollection.Cards.ToList())
				{
					if (CancellationToken.IsCancellationRequested)
					{
						CancellationToken = new CancellationTokenSource();
						break;
					}
					if (card.Value.card.Types.Count() == 0 || card.Value.card.Colors.Count() == 0 || card.Value.card.Rarity == ECardRarity.UNKNWOWN || string.IsNullOrEmpty(card.Value.card.Text) || !card.Value.card.KeywordsInitialized || string.IsNullOrEmpty(card.Value.card.Artist))
					{
						var cardResult = CardAPI.GetCard(card.Value.card.UID).Result;
						if (cardResult != null)
							DataService.Instance.MyCollection.EditCard(card.Value.card.UID, cardResult, updatePrice: false);

						await Task.Delay(50);
					}

					currentCards++;
					UpdatePercentage(currentCards, totalCards);
					await Task.Delay(1);
				}
				IsLoading = false;

			}
			catch (Exception ex)
			{

			}
		}

		/// <summary>Updates loading data percentage.</summary>
		/// <param name="currentCards">Current number of scanned cards.</param>
		/// <param name="totalCards">Total number of scanned cards.</param>
		private async Task UpdatePercentage(int currentCards, int totalCards)
		{
			Percentage = currentCards * 100 / totalCards;
			Text = $"{currentCards}/{totalCards}";
			StateHasChanged();
			await Task.Delay(1);
		}

		#endregion
	}
}
