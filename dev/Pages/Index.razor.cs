using BlazorApp.Data;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Pages
{
	/// <summary>Class that handles data and treatment for Index page.</summary>
	public class IndexBase : ComponentBase
	{
		#region Protected Properties 

		/// <summary>
		/// Dictionary containing all cards in collection.
		/// Key : unique identifier of the card.
		/// Value : tuple with the card and the number of copies of this card.
		/// </summary>
		protected ObservableDictionary<string, (Card card, int nb)> Collection => DataService.Instance.MyCollection.Cards;

		/// <summary>Dictionary containing all cards in collection to be displayed.</summary>
		protected Dictionary<string, (Card card, int nb)> CollectionView { get; set; } = new Dictionary<string, (Card card, int nb)>();

		/// <summary>Css class for green deck icon.</summary>
		protected string CssClassGreen { get; set; } = "colorIconNotSelected";

		/// <summary>Css class for blue deck icon.</summary>
		protected string CssClassBlue { get; set; } = "colorIconNotSelected";

		/// <summary>Css class for red deck icon.</summary>
		protected string CssClassRed { get; set; } = "colorIconNotSelected";

		/// <summary>Css class for white deck icon.</summary>
		protected string CssClassWhite { get; set; } = "colorIconNotSelected";

		/// <summary>Css class for black deck icon.</summary>
		protected string CssClassBlack { get; set; } = "colorIconNotSelected";

		/// <summary>Css class for artifact deck icon.</summary>
		protected string CssClassArtifact { get; set; } = "colorIconNotSelected";

		/// <summary>Css class for bicolor deck icon.</summary>
		protected string CssClassBicolor { get; set; } = "colorIconNotSelected";

		/// <summary>Css class for common rarity icon.</summary>
		protected string CssClassCommon { get; set; } = "rarityIconNotSelected";

		/// <summary>Css class for uncommon rarity icon.</summary>
		protected string CssClassUncommon { get; set; } = "rarityIconNotSelected";

		/// <summary>Css class for rare rarity icon.</summary>
		protected string CssClassRare { get; set; } = "rarityIconNotSelected";

		/// <summary>Css class for mythic rarity icon.</summary>
		protected string CssClassMythic { get; set; } = "rarityIconNotSelected";

		/// <summary>Number of displayed cards.</summary>
		protected int NbDisplayedCards { get; set; }

		#endregion

		#region Protected Methods

		/// <summary>Method called when page is initialized.</summary>
		protected override async Task OnInitializedAsync()
		{
			DataService.Instance.MyCollection.Cards.CollectionChanged += RefreshCollection;
			//CollectionView = new Dictionary<string, (Card card, int nb)>(Collection);
		}

		/// <summary>Displays specified rarity or not depending on actual state.</summary>
		/// <param name="rarity">Rarity icon selected/unselected.</param>
		protected void DisplayRarity(ECardRarity rarity)
		{
			switch (rarity)
			{
				case ECardRarity.UNKNWOWN:
					break;
				case ECardRarity.COMMON:
					CssClassCommon = CssClassCommon.Contains("rarityIconSelected") ? "rarityIconNotSelected" : "rarityIconSelected";
					ChangeDisplayedCards();
					break;
				case ECardRarity.UNCOMMON:
					CssClassUncommon = CssClassUncommon.Contains("rarityIconSelected") ? "rarityIconNotSelected" : "rarityIconSelected";
					ChangeDisplayedCards();
					break;
				case ECardRarity.RARE:
					CssClassRare = CssClassRare.Contains("rarityIconSelected") ? "rarityIconNotSelected" : "rarityIconSelected";
					ChangeDisplayedCards();
					break;
				case ECardRarity.MYTHIC:
					CssClassMythic = CssClassMythic.Contains("rarityIconSelected") ? "rarityIconNotSelected" : "rarityIconSelected";
					ChangeDisplayedCards();
					break;
				default:
					break;
			}
		}

		/// <summary>Displays specified color or not depending on actual state.</summary>
		/// <param name="cardColor">Color.</param>
		protected void DisplayColor(ECardColor cardColor)
		{
			switch (cardColor)
			{
				case ECardColor.GREEN:
					CssClassGreen = CssClassGreen.Contains("colorIconSelected") ? "colorIconNotSelected" : "colorIconSelected";
					ChangeDisplayedCards();
					break;
				case ECardColor.BLUE:
					CssClassBlue = CssClassBlue.Contains("colorIconSelected") ? "colorIconNotSelected" : "colorIconSelected";
					ChangeDisplayedCards();
					break;
				case ECardColor.RED:
					CssClassRed = CssClassRed.Contains("colorIconSelected") ? "colorIconNotSelected" : "colorIconSelected";
					ChangeDisplayedCards();
					break;
				case ECardColor.WHITE:
					CssClassWhite = CssClassWhite.Contains("colorIconSelected") ? "colorIconNotSelected" : "colorIconSelected";
					ChangeDisplayedCards();
					break;
				case ECardColor.BLACK:
					CssClassBlack = CssClassBlack.Contains("colorIconSelected") ? "colorIconNotSelected" : "colorIconSelected";
					ChangeDisplayedCards();
					break;
				case ECardColor.ARTIFACT:
					CssClassArtifact = CssClassArtifact.Contains("colorIconSelected") ? "colorIconNotSelected" : "colorIconSelected";
					ChangeDisplayedCards();
					break;
				case ECardColor.BICOLOR:
					CssClassBicolor = CssClassBicolor.Contains("colorIconSelected") ? "colorIconNotSelected" : "colorIconSelected";
					ChangeDisplayedCards();
					break;
				default:
					break;
			}
		}

		#endregion

		#region Private Methods

		/// <summary>Refreshes the collection.</summary>
		/// <param name="_">Parameter not used.</param>
		/// <param name="__">Parameter not used.</param>
		private void RefreshCollection(object _, EventArgs __)
		{
			InvokeAsync(() => StateHasChanged());
		}

		/// <summary>Change the list of displayed cards depending on selected color.</summary>
		private void ChangeDisplayedCards()
		{
			CollectionView = new Dictionary<string, (Card card, int nbCard)>(Collection.Where(
					value => (CssClassBlack.Contains("colorIconSelected") && value.Value.card.Colors.Contains(ECardColor.BLACK) && value.Value.card.Colors.Count() == 1)
							|| (CssClassBlue.Contains("colorIconSelected") && value.Value.card.Colors.Contains(ECardColor.BLUE) && value.Value.card.Colors.Count() == 1)
							|| (CssClassWhite.Contains("colorIconSelected") && value.Value.card.Colors.Contains(ECardColor.WHITE) && value.Value.card.Colors.Count() == 1)
							|| (CssClassGreen.Contains("colorIconSelected") && value.Value.card.Colors.Contains(ECardColor.GREEN) && value.Value.card.Colors.Count() == 1)
							|| (CssClassRed.Contains("colorIconSelected") && value.Value.card.Colors.Contains(ECardColor.RED) && value.Value.card.Colors.Count() == 1)
							|| (CssClassBicolor.Contains("colorIconSelected") && value.Value.card.Colors.Count() > 1)
							|| (CssClassArtifact.Contains("colorIconSelected") && (value.Value.card.Colors.Count() == 0 || value.Value.card.Colors.Contains(ECardColor.ARTIFACT)))
							)
					);

			CollectionView = new Dictionary<string, (Card card, int nbCard)>(CollectionView.Where(
				value => (CssClassCommon.Contains("rarityIconSelected") && value.Value.card.Rarity.Equals(ECardRarity.COMMON))
							|| (CssClassUncommon.Contains("rarityIconSelected") && value.Value.card.Rarity.Equals(ECardRarity.UNCOMMON))
							|| (CssClassRare.Contains("rarityIconSelected") && value.Value.card.Rarity.Equals(ECardRarity.RARE))
							|| (CssClassMythic.Contains("rarityIconSelected") && value.Value.card.Rarity.Equals(ECardRarity.MYTHIC))
					)
				);

			NbDisplayedCards = CollectionView.Count();
		}

		#endregion
	}
}
