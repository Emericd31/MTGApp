using BlazorApp.Data;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Shared
{
	/// <summary>Class that handles data and treatment for Card component.</summary>
	public class CardComponentBase : ComponentBase
	{
		#region Protected Properties

		/// <summary>Background card component color.</summary>
		protected string color = "lightgray";

		/// <summary>Border card component width.</summary>
		protected string borderWidth = "0px";

		/// <summary>Number of cards to add.</summary>
		protected int NbCardToAdd { get; set; } = 1;

		/// <summary>Number of cards to remove.</summary>
		protected int NbCardToRemove { get; set; } = 1;

		#endregion

		#region Public Properties

		[Parameter]
		/// <summary>Card object.</summary>
		public Card? Card { get; set; }

		[Parameter]
		/// <summary>Number of copies of the card currently in collection.</summary>
		public int NbCardInCollection { get; set; } = 0;

		[Parameter]
		/// <summary>Boolean indicating if add card in collection is enabled for this card.</summary>
		/// <remarks>Graphical elements are displayed depending on this boolean</remarks>
		public bool AllowAddToCollection { get; set; }

		[Parameter]
		/// <summary>Boolean indicating if remove card in collection is enabled for this card.</summary>
		/// <remarks>Graphical elements are displayed depending on this boolean</remarks>
		public bool AllowRemoveFromCollection { get; set; }

		#endregion

		#region Protected Methods

		/// <summary>Method called when page is initialized.</summary>
		protected override Task OnInitializedAsync()
		{
			if (NbCardInCollection == 0 && Card != null && DataService.Instance.MyCollection.Cards.ContainsKey(Card.UID))
			{
				NbCardInCollection = DataService.Instance.MyCollection.Cards[Card.UID].nbCard;
				color = "#d4bb83";
				borderWidth = "2px";
			}
			return base.OnInitializedAsync();
		}

		/// <summary>Method called when parameters are set.</summary>
		protected override void OnParametersSet()
		{
			if (NbCardInCollection == 0 && Card != null && DataService.Instance.MyCollection.Cards.ContainsKey(Card.UID))
			{
				NbCardInCollection = DataService.Instance.MyCollection.Cards[Card.UID].nbCard;
				color = "#d4bb83";
				borderWidth = "2px";
			}

			if (NbCardInCollection > 0 && Card != null)
			{
				if (!DataService.Instance.MyCollection.Cards.ContainsKey(Card.UID))
				{
					borderWidth = "0px";
					color = "lightgray";
					NbCardInCollection = 0;
				}
				else
				{
					NbCardInCollection = DataService.Instance.MyCollection.Cards[Card.UID].nbCard;
					color = "#d4bb83";
					borderWidth = "2px";
				}
			}
		}

		/// <summary>Method called when add button is clicked.</summary>
		protected void OnClickAdd()
		{
			if (Card != null)
			{
				DataService.Instance.MyCollection.ManageCard(Card, NbCardToAdd, ECollectionAction.ADD);
				NbCardInCollection += NbCardToAdd;
				NbCardToAdd = 1;
				color = "#d4bb83";
				borderWidth = "2px";
			}
		}

		/// <summary>Method called when remove button is clicked.</summary>
		protected void OnClickRemove()
		{
			if (Card != null)
			{
				DataService.Instance.MyCollection.ManageCard(Card, NbCardToRemove, ECollectionAction.REMOVE);
				var nbCardRemoved = NbCardToRemove;
				NbCardToRemove = 1;
				NbCardInCollection -= nbCardRemoved;
			}
		}

		#endregion
	}
}
