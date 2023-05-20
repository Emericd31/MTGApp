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

		#endregion

		#region Protected Methods

		/// <summary>Method called when page is initialized.</summary>
		protected override async Task OnInitializedAsync()
		{
			DataService.Instance.MyCollection.Cards.CollectionChanged += RefreshCollection;
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

		#endregion
	}
}
