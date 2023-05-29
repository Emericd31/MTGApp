using BlazorApp.API;
using BlazorApp.Data;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Pages
{
	/// <summary>Class that handles data and treatment for Decks page.</summary>
	public class DecksBase : ComponentBase
	{
		#region Private Properties

		/// <summary>Deck title.</summary>
		private string? _deckTitle { get; set; }

		/// <summary>Deck value (containing the list of cards and their numbers).</summary>
		private string? _deckValue { get; set; }

		#endregion

		#region Protected Properties

		/// <summary>Boolean indicating if the import popup should be displayed.</summary>
		protected bool ShowPopup { get; set; }

		/// <summary>Boolean indicating if an error occurred while importing the deck.</summary>
		protected bool IsSuccess { get; set; } = true;

		/// <summary>Message containing the result of the import.</summary>
		protected string? Message { get; set; }

		/// <summary>Boolean indicating if an import action is in progress.</summary>
		public bool IsLoading { get; set; }

		/// <summary>Message shown while importing the deck.</summary>
		protected string LoadingMessage { get; set; } = "Import en cours";

		/// <summary>Boolean indicating if the "import" button is disabled.</summary>
		protected bool IsDisabled { get; set; }

		/// <summary>Gets or sets <see cref="_deckTitle"/>.</summary>
		protected string? DeckTitle
		{
			get { return _deckTitle; }
			set
			{
				_deckTitle = value;

				if (DataService.Instance.MyDecks.Any(deck => deck.Name == _deckTitle))
				{
					Message = "Un deck avec ce nom existe déjà parmi vos decks !";
					IsDisabled = true;
				}
				else
				{
					Message = "   ";
					if (CheckFields())
						IsDisabled = false;
					else
						IsDisabled = true;
				}

			}
		}

		/// <summary>Gets or sets <see cref="_deckValue"/>.</summary>
		protected string? DeckValue
		{
			get { return _deckValue; }
			set
			{
				_deckValue = value;
				if (CheckFields() && !DataService.Instance.MyDecks.Any(deck => deck.Name == _deckTitle))
					IsDisabled = false;
				else
					IsDisabled = true;
			}
		}

		#endregion

		#region Private Methods

		/// <summary>Checks if all required fields (deck title and deck value) are filled.</summary>
		/// <returns>True if all required fields are filled (false otherwise).</returns>
		private bool CheckFields()
		{
			return (!string.IsNullOrEmpty(_deckTitle) && !string.IsNullOrEmpty(_deckValue));
		}

		#endregion

		#region Protected Methods

		/// <summary>Displays or hides the import popup.</summary>
		protected void DisplayPopup()
		{
			ShowPopup = !ShowPopup;
		}

		/// <summary>Closes the import popup.</summary>
		protected void ClosePopup()
		{
			ShowPopup = false;
		}

		/// <summary>Sets the import result.</summary>
		/// <param name="isSuccess">Boolean indicating if an error occurred while importing the deck.</param>
		/// <param name="message">Message containing the result of the import.</param>
		private void SetResultMeessage(bool isSuccess, string message)
		{
			IsSuccess = isSuccess;
			Message = message;
			if (!isSuccess)
				IsLoading = false;
		}

		/// <summary>Imports the deck.</summary>
		protected async Task ImportDeck()
		{
			SetResultMeessage(isSuccess: true, "");
			IsLoading = true;
			await Task.Run(async () =>
			{
				if (DataService.Instance.MyDecks.Any(deck => deck.Name == _deckTitle))
				{
					SetResultMeessage(isSuccess: false, "Un deck avec ce nom existe déjà parmi vos decks !");
					return;
				}

				if (_deckTitle == null || _deckValue == null)
				{
					SetResultMeessage(isSuccess: false, "L'une des données nécessaire est nulle !");
					return;
				}

				var totalNumberOfCards = 0;
				var NbErrorCardImport = 0;

				Collection newDeck = new Collection(_deckTitle, "No description");

				string[] result = _deckValue.Split('\n');
				foreach (string s in result)
				{
					string[] currentLine = s.Split(' ');
					if (currentLine.Length > 1)
					{
						try
						{
							var nbOfCopies = currentLine[0];
							var mtgCode = currentLine[currentLine.Length - 1];
							var setCode = currentLine[currentLine.Length - 2].Trim('(', ')');
							var card = await CardAPI.GetCard(setCode, mtgCode);

							if (card != null && int.TryParse(nbOfCopies, out int nbCard))
							{
								totalNumberOfCards += nbCard;
								newDeck.ManageCard(card, nbCard, ECollectionAction.ADD);
							}
							else
							{
								if (int.TryParse(nbOfCopies, out int nbCardError))
									NbErrorCardImport += nbCardError;
							}
						}
						catch (Exception e)
						{
							Console.WriteLine($"[Decks.razor.cs - ImportDeck] An error occurred while importing the deck : {e.Message} {e.InnerException} {e.StackTrace}");
						}
					}
				}

				if (newDeck.NbCards > 0)
					DataService.Instance.MyDecks.Add(newDeck);

				SetResultMeessage(isSuccess: true, $"Le deck a été importé avec succès ! ({totalNumberOfCards}/{totalNumberOfCards + NbErrorCardImport} cartes)");
				DeckTitle = "";
				DeckValue = "";
			});
			IsLoading = false;
		}

		#endregion
	}
}
