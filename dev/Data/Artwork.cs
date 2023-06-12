namespace BlazorApp.Data
{
	/// <summary>Artwork object.</summary>
	public class Artwork
	{
		#region Public Properties

		/// <summary>Artwork URL.</summary>
		public string Name { get; set; }

		/// <summary>Artwork color.</summary>
		public ECardColor Color { get; set; }

		/// <summary>Artwork margin top.</summary>
		public int DisplayDeckMargin { get; set; }

		#endregion

		#region Constructor

		/// <summary>Constructor.</summary>
		/// <param name="name">Artwork URL.</param>
		/// <param name="color">Artwork color.</param>
		/// <param name="displayDeckMargin">Artwork margin top.</param>
		public Artwork(string name, ECardColor color, int displayDeckMargin)
		{
			Name = name;
			Color = color;
			DisplayDeckMargin = displayDeckMargin;
		}

		#endregion
	}
}
