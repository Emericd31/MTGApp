﻿using BlazorApp.Data;
using Microsoft.AspNetCore.Components;
using BlazorApp.API;

namespace BlazorApp.Pages
{
	/// <summary>Class that handles data and treatment for Search card page.</summary>
	public class SearchCardBase : ComponentBase
	{
		#region Protected Properties

		/// <summary>Search input value.</summary>
		protected string? SearchInput { get; set; }

		/// <summary>Search card code.</summary>
		protected string? SearchCardCode { get; set; }

		/// <summary>Search three letters set code.</summary>
		protected string? SearchSetCode { get; set; }

		/// <summary>Number of cards found.</summary>
		protected long NbCards { get; set; }

		/// <summary>List of cards found.</summary>
		protected List<Card>? Cards { get; set; } = null;

		#endregion

		#region Protected Methods

		/// <summary>Searches cards</summary>
		/// <remarks>Cards containing seach input value will be found.</remarks>
		protected async Task Search()
		{
			if (!string.IsNullOrEmpty(SearchInput))
			{
				var result = await CardAPI.SearchCards(SearchInput, limit: 200);
				Cards = result.cards;
				NbCards = result.totalCards;
				StateHasChanged();
			}
		}

		/// <summary>Searches cards</summary>
		/// <remarks>Cards corresponding to card code and set code will be found.</remarks>
		protected async Task SearchByCardCodeAndSetCode()
		{
			if (!string.IsNullOrEmpty(SearchCardCode) && !string.IsNullOrEmpty(SearchSetCode))
			{
				var result = await CardAPI.SearchCards(SearchCardCode, SearchSetCode);
				Cards = result.cards;
				NbCards = result.totalCards;
				StateHasChanged();
			}
		}

		#endregion
	}
}
