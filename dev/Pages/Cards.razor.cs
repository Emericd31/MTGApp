using BlazorApp.API;
using BlazorApp.Data;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Pages
{
	/// <summary>Class that handles data and treatment for Cards page.</summary>
	public class CardsBase : ComponentBase
	{
		#region Private Properties

		/// <summary>Boolean indicating if owned cards are displayed.</summary>
		private bool _displayOwnedCards { get; set; } = true;

		/// <summary>Boolean indicating if not owned cards are displayed.</summary>
		private bool _displayNotOwnedCards { get; set; } = true;

		#endregion Private Properties


		#region Protected Properties

		/// <summary>Page title.</summary>
		protected string PageTitle { get; set; } = "Liste de cartes : ";

		/// <summary>String corresponding to total cards.</summary>
		protected string TotalCards { get; set; } = "0";

		/// <summary>List of cards.</summary>
		protected List<Card>? Cards { get; set; } = null;

		/// <summary>List of cards to display.</summary>
		protected List<Card>? DisplayedCards { get; set; } = null;

		/// <summary>List of cards owned in this set.</summary>
		protected List<Card>? CardsInCollection { get; set; } = new List<Card>();

		/// <summary>Gets or sets <see cref="_displayOwnedCards"/>.</summary>
		protected bool DisplayOwnedCards
		{
			get { return _displayOwnedCards; }
			set
			{
				_displayOwnedCards = value;
				ChangeDisplayedCards();
			}
		}

		/// <summary>Gets or sets <see cref="_displayNotOwnedCards"/>.</summary>
		protected bool DisplayNotOwnedCards
		{
			get { return _displayNotOwnedCards; }
			set
			{
				_displayNotOwnedCards = value;
				ChangeDisplayedCards();
			}
		}

		#endregion Protected Properties

		#region Public Properties

		[Parameter]
		/// <summary>Three letters expansion code.</summary>
		public string? SetCode { get; set; }

		#endregion

		#region Protected Methods

		/// <summary>Method called after each rendering.</summary>
		/// <param name="firstRender">Boolean indicating if it's the first rendering.</param>
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender && SetCode != null)
			{
				var result = await CardAPI.GetCards(SetCode).ConfigureAwait(false);
				Cards = result.cards;
				DisplayedCards = result.cards;
				TotalCards = result.totalCards.ToString();
				CardsInCollection = Cards.Where(card => DataService.Instance.MyCollection.Cards.Any(c => c.Value.card.UID == card.UID)).ToList();
				StateHasChanged();
			}
		}

		#endregion Public Properties

		#region Private Methods

		/// <summary>Change the list of displayed cards depending on selected filters.</summary>
		private void ChangeDisplayedCards()
		{
			if (CardsInCollection != null)
			{
				DisplayedCards = Cards?.Where(card =>
					DisplayOwnedCards && CardsInCollection.Any(c => c.UID == card.UID)
					|| DisplayNotOwnedCards && !CardsInCollection.Any(c => c.UID == card.UID)
				).ToList();
			}
		}

		#endregion Private Methods
	}
}
