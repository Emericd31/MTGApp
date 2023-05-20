using BlazorApp.Data;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Shared
{
	/// <summary>Class that handles data and treatment for Extension component.</summary>
	public class ExtensionComponentBase : ComponentBase
	{
		#region Public Properties

		[Parameter]
		/// <summary>Set object.</summary>
		public Set? Set { get; set; }

		#endregion
	}
}
