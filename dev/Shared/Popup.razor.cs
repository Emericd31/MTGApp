using Microsoft.AspNetCore.Components;

namespace BlazorApp.Shared
{
	/// <summary>Class that handles data and treatment for Popup component.</summary>
	public class PopupBase : ComponentBase
	{
		/// <summary>Boolean indicating if the popup should be displayed.</summary>
		[Parameter]
		public bool ShowPopup { get; set; }

		/// <summary>Popup title.</summary>
		[Parameter]
		public string? Title { get; set; }

		/// <summary>Popup content.</summary>
		[Parameter]
		public RenderFragment? Content { get; set; }

		/// <summary>Callback method to close the popup.</summary>
		[Parameter]
		public EventCallback ClosePopup { get; set; }
	}
}
