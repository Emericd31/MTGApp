using BlazorApp.API;
using BlazorApp.Data;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Pages
{
	/// <summary>Class that handles data and treatment for Cards page.</summary>
	public class CardsBase : ComponentBase
	{
		#region Protected Properties

		/// <summary>Page title.</summary>
		protected string PageTitle { get; set; } = "Liste de cartes : ";

		/// <summary>String corresponding to total cards.</summary>
		protected string TotalCards { get; set; } = "0";

		/// <summary>List of cards.</summary>
		protected List<Card>? Cards { get; set; } = null;

		#endregion

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
			if (firstRender)
			{
				var result = await CardAPI.GetCards(SetCode).ConfigureAwait(false);
				Cards = result.cards;
				TotalCards = result.totalCards.ToString();
				StateHasChanged();
			}
		}

		#endregion
	}
}
