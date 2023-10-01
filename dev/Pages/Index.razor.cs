using BlazorApp.Data;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Pages
{
	/// <summary>Class that handles data and treatment for Index page.</summary>
	public class IndexBase : ComponentBase
	{
		#region Private Properties

		/// <summary>Boolean indicating if creatures are displayed.</summary>
		private bool _displayCreatures { get; set; } = true;

		/// <summary>Boolean indicating if sorcery are displayed.</summary>
		private bool _displaySorcery { get; set; }

		/// <summary>Boolean indicating if enchantments are displayed.</summary>
		private bool _displayEnchantments { get; set; }

		/// <summary>Boolean indicating if instants are displayed.</summary>
		private bool _displayInstants { get; set; }

		/// <summary>Boolean indicating if artifacts are displayed.</summary>
		private bool _displayArtifacts { get; set; }

		/// <summary>Boolean indicating if sieges are displayed.</summary>
		private bool _displaySieges { get; set; }

		/// <summary>Boolean indicating if planeswalkers are displayed.</summary>
		private bool _displayPlaneswalkers { get; set; }

		/// <summary>Boolean indicating if lands are displayed.</summary>
		private bool _displayLands { get; set; }

		/// <summary>Boolean indicating if legendaries are displayed.</summary>
		private bool _displayLegendaries { get; set; }

		/// <summary>Boolean indicating if filters are inclusive or not.</summary>
		private bool _displayInclusives { get; set; } = true;

		#endregion

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

		/// <summary>Gets or sets <see cref="_displayCreatures"/>.</summary>
		protected bool DisplayCreatures
		{
			get { return _displayCreatures; }
			set
			{
				_displayCreatures = value;
				ChangeDisplayedCards();
			}
		}

		/// <summary>Gets or sets <see cref="_displaySorcery"/>.</summary>
		protected bool DisplaySorcery
		{
			get { return _displaySorcery; }
			set
			{
				_displaySorcery = value;
				ChangeDisplayedCards();
			}
		}

		/// <summary>Gets or sets <see cref="_displayEnchantments"/>.</summary>
		protected bool DisplayEnchantments
		{
			get { return _displayEnchantments; }
			set
			{
				_displayEnchantments = value;
				ChangeDisplayedCards();
			}
		}

		/// <summary>Gets or sets <see cref="_displayInstants"/>.</summary>
		protected bool DisplayInstants
		{
			get { return _displayInstants; }
			set
			{
				_displayInstants = value;
				ChangeDisplayedCards();
			}
		}

		/// <summary>Gets or sets <see cref="_displayArtifacts"/>.</summary>
		protected bool DisplayArtifacts
		{
			get { return _displayArtifacts; }
			set
			{
				_displayArtifacts = value;
				ChangeDisplayedCards();
			}
		}

		/// <summary>Gets or sets <see cref="_displaySieges"/>.</summary>
		protected bool DisplaySieges
		{
			get { return _displaySieges; }
			set
			{
				_displaySieges = value;
				ChangeDisplayedCards();
			}
		}

		/// <summary>Gets or sets <see cref="_displayPlaneswalkers"/>.</summary>
		protected bool DisplayPlaneswalkers
		{
			get { return _displayPlaneswalkers; }
			set
			{
				_displayPlaneswalkers = value;
				ChangeDisplayedCards();
			}
		}

		/// <summary>Gets or sets <see cref="_displayLands"/>.</summary>
		protected bool DisplayLands
		{
			get { return _displayLands; }
			set
			{
				_displayLands = value;
				ChangeDisplayedCards();
			}
		}

		/// <summary>Gets or sets <see cref="_displayLegendaries"/>.</summary>
		protected bool DisplayLegendaries
		{
			get { return _displayLegendaries; }
			set
			{
				_displayLegendaries = value;
				ChangeDisplayedCards();
			}
		}

		/// <summary>Gets or sets <see cref="_displayInclusives"/>.</summary>
		protected bool DisplayInclusives
		{
			get { return _displayInclusives; }
			set
			{
				_displayInclusives = value;
				ChangeDisplayedCards();
			}
		}

		#endregion Protected Properties

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

		#endregion Protected Methods

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
			// Filters by colors
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

			// Filters by rarities
			CollectionView = new Dictionary<string, (Card card, int nbCard)>(CollectionView.Where(
				value => (CssClassCommon.Contains("rarityIconSelected") && value.Value.card.Rarity.Equals(ECardRarity.COMMON))
							|| (CssClassUncommon.Contains("rarityIconSelected") && value.Value.card.Rarity.Equals(ECardRarity.UNCOMMON))
							|| (CssClassRare.Contains("rarityIconSelected") && value.Value.card.Rarity.Equals(ECardRarity.RARE))
							|| (CssClassMythic.Contains("rarityIconSelected") && value.Value.card.Rarity.Equals(ECardRarity.MYTHIC))
					)
				);

			// Filters by types
			if (DisplayInclusives)
			{
				CollectionView = new Dictionary<string, (Card card, int nb)>(CollectionView.Where(
					value => (_displayCreatures && value.Value.card.Types.Contains(ECardType.CREATURE))
					|| (_displaySorcery && value.Value.card.Types.Contains(ECardType.SORCERY))
					|| (_displayEnchantments && value.Value.card.Types.Contains(ECardType.ENCHANTMENT))
					|| (_displayInstants && value.Value.card.Types.Contains(ECardType.INSTANT))
					|| (_displayArtifacts && value.Value.card.Types.Contains(ECardType.ARTIFACT))
					|| (_displaySieges && value.Value.card.Types.Contains(ECardType.SIEGE))
					|| (_displayPlaneswalkers && value.Value.card.Types.Contains(ECardType.PLANESWALKER))
					|| (_displayLands && value.Value.card.Types.Contains(ECardType.LAND))
					|| (_displayLegendaries && value.Value.card.Types.Contains(ECardType.LEGENDARY))
					));
			}
			else
			{
				var all = new Dictionary<string, (Card card, int nb)>(CollectionView.Where(
					value => (_displayCreatures && value.Value.card.Types.Contains(ECardType.CREATURE))
					|| (_displaySorcery && value.Value.card.Types.Contains(ECardType.SORCERY))
					|| (_displayEnchantments && value.Value.card.Types.Contains(ECardType.ENCHANTMENT))
					|| (_displayInstants && value.Value.card.Types.Contains(ECardType.INSTANT))
					|| (_displayArtifacts && value.Value.card.Types.Contains(ECardType.ARTIFACT))
					|| (_displaySieges && value.Value.card.Types.Contains(ECardType.SIEGE))
					|| (_displayPlaneswalkers && value.Value.card.Types.Contains(ECardType.PLANESWALKER))
					|| (_displayLands && value.Value.card.Types.Contains(ECardType.LAND))
					|| (_displayLegendaries && value.Value.card.Types.Contains(ECardType.LEGENDARY))
					));

				var result = all;

				var typesToDisplay = new List<ECardType>();
				if (_displayCreatures)
					typesToDisplay.Add(ECardType.CREATURE);
				if (_displaySorcery)
					typesToDisplay.Add(ECardType.SORCERY);
				if (_displayEnchantments)
					typesToDisplay.Add(ECardType.ENCHANTMENT);
				if (_displayInstants)
					typesToDisplay.Add(ECardType.INSTANT);
				if (_displayArtifacts)
					typesToDisplay.Add(ECardType.ARTIFACT);
				if (_displaySieges)
					typesToDisplay.Add(ECardType.SIEGE);
				if (_displayPlaneswalkers)
					typesToDisplay.Add(ECardType.PLANESWALKER);
				if (_displayLands)
					typesToDisplay.Add(ECardType.LAND);
				if (_displayLegendaries)
					typesToDisplay.Add(ECardType.LEGENDARY);

				foreach (var item in all)
				{
					foreach (var type in typesToDisplay)
					{
						if (!item.Value.card.Types.Contains(type))
							result.Remove(item.Key);
					}
				}

				CollectionView = result;
			}

			NbDisplayedCards = CollectionView.Count();
		}

		#endregion Private Methods
	}
}
