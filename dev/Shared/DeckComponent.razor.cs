using BlazorApp.Data;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Shared
{
	/// <summary>Class that handles data and treatment for Deck component.</summary>
	public class DeckComponentBase : ComponentBase
	{
		#region Public Properties

		/// <summary>Deck parameter.</summary>
		[Parameter]
		public Collection? Deck { get; set; }

		#endregion

		#region Protected Properties

		/// <summary>String representing price of the deck (in euros).</summary>
		protected string? EURPrice { get; set; }

		/// <summary>String representing price of the deck (in dollars).</summary>
		protected string? USDPrice { get; set; }

		/// <summary>Boolean indicating if the deck contains exactly one color.</summary>
		protected bool OneColor { get; set; }

		/// <summary>Boolean indicating if the deck contains exactly two colors.</summary>
		protected bool TwoColor { get; set; }

		/// <summary>Boolean indicating if the deck contains exactly three colors.</summary>
		protected bool ThreeColor { get; set; }

		/// <summary>Boolean indicating if the deck contains exactly four colors.</summary>
		protected bool FourColor { get; set; }

		/// <summary>Boolean indicating if the deck contains exactly fives colors.</summary>
		protected bool FiveColor { get; set; }

		/// <summary>First color string.</summary>
		protected string FirstColor { get; set; } = "";

		/// <summary>Second color string.</summary>
		protected string SecondColor { get; set; } = "";

		/// <summary>Third color string.</summary>
		protected string ThirdColor { get; set; } = "";

		/// <summary>Fourth color string.</summary>
		protected string FourthColor { get; set; } = "";

		/// <summary>Fifth color string.</summary>
		protected string FifthColor { get; set; } = "";

		#endregion

		#region Protected Methods

		/// <summary>Method called when page is initialized.</summary>
		protected override Task OnInitializedAsync()
		{
			if (Deck != null)
			{
				EURPrice = Math.Abs(Deck.EURPrice).ToString("0.##");
				USDPrice = Math.Abs(Deck.USDPrice).ToString("0.##");
				OneColor = Deck.Colors.Count == 1;
				TwoColor = Deck.Colors.Count == 2;
				ThreeColor = Deck.Colors.Count == 3;
				FourColor = Deck.Colors.Count == 4;
				FiveColor = Deck.Colors.Count == 5;

				var remainingColors = new List<ECardColor>(Deck.Colors);

				if (Deck.Colors.Count > 0)
					FirstColor = AddColor(ref remainingColors);
				if (Deck.Colors.Count > 1)
					SecondColor = AddColor(ref remainingColors);
				if (Deck.Colors.Count > 2)
					ThirdColor = AddColor(ref remainingColors);
				if (Deck.Colors.Count > 3)
					FourthColor = AddColor(ref remainingColors);
				if (Deck.Colors.Count > 4)
					FifthColor = AddColor(ref remainingColors);
			}
			return base.OnInitializedAsync();
		}

		#endregion

		#region Private Methods

		/// <summary>Gets color value in the right order given a color list.</summary>
		/// <param name="remainingColors">Remaining colors in the list.</param>
		/// <returns>Color value.</returns>
		private string AddColor(ref List<ECardColor> remainingColors)
		{
			if (remainingColors.Contains(ECardColor.GREEN))
			{
				remainingColors.Remove(ECardColor.GREEN);
				return "green";
			}
			else if (remainingColors.Contains(ECardColor.BLUE))
			{
				remainingColors.Remove(ECardColor.BLUE);
				return "#005ddd";
			}
			else if (remainingColors.Contains(ECardColor.RED))
			{
				remainingColors.Remove(ECardColor.RED);
				return "red";
			}
			else if (remainingColors.Contains(ECardColor.WHITE))
			{
				remainingColors.Remove(ECardColor.WHITE);
				return "#c4c69e";
			}
			else if (remainingColors.Contains(ECardColor.BLACK))
			{
				remainingColors.Remove(ECardColor.BLACK);
				return "black";
			}

			return "";
		}

		#endregion
	}
}
