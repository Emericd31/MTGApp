using BlazorApp.API;

namespace BlazorApp.Data
{
	/// <summary>Class that handles application's shared data.</summary>
	public sealed class DataService
	{
		#region Private Properties

		/// <summary>Unique instance of the class.</summary>
		private static DataService? _instance;

		#endregion

		#region Public Properties

		/// <summary>Gets the unique instance of the class.</summary>
		public static DataService Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new DataService();
				}
				return _instance;
			}
		}

		/// <summary>Boolean indicating if application's data has been initialized.</summary>
		public bool HasBeenInitialized { get; set; }

		/// <summary>Collection of cards.</summary>
		public Collection MyCollection { get; set; }

		/// <summary>List of decks.</summary>
		public List<Collection> MyDecks { get; set; }

		/// <summary>List of existing Magic expansions.</summary>
		public List<Set> Sets { get; set; } = new List<Set>();

		/// <summary>Folder containing external application's data.</summary>
		public string? ApplicationDataFolder { get; set; }

		/// <summary>Dictionary containing card colors and their associated artworkds.</summary>
		public Dictionary<ECardColor, List<Artwork>> Artworks { get; set; }

		#endregion

		#region Constructor

		/// <summary>Constructor.</summary>
		private DataService()
		{
			MyCollection = new Collection("MyCollection", "Description");
			MyDecks = new List<Collection>();
			Artworks = new Dictionary<ECardColor, List<Artwork>>();
		}

		#endregion

		#region Public Methods

		/// <summary>Initializes application's data.</summary>
		public async Task InitializeData()
		{
			Instance.Sets = await SetAPI.GetSets().ConfigureAwait(false);
			InitializeArtworks();
		}

		#endregion

		#region Private Methods

		/// <summary>Initializes artworks dictionary.</summary>
		private void InitializeArtworks()
		{
			InitializeBlackArtworks();
			InitializeBlueArtworks();
			InitializeGreenArtworks();
			InitializeRedArtworks();
			InitializeWhiteArtworks();
		}

		/// <summary>Initializes black artworks.</summary>
		private void InitializeBlackArtworks()
		{
			List<Artwork> artworks = new List<Artwork>()
			{
				new Artwork("Artworks/Black/art-1.jpg", ECardColor.BLACK, -8),
				new Artwork("Artworks/Black/art-2.jpg", ECardColor.BLACK, -10),
				new Artwork("Artworks/Black/art-3.jpg", ECardColor.BLACK, -12),
				new Artwork("Artworks/Black/art-4.jpg", ECardColor.BLACK, -15),
				new Artwork("Artworks/Black/art-5.jpg", ECardColor.BLACK, -15)
			};

			if (Artworks.ContainsKey(ECardColor.BLACK))
				Artworks[ECardColor.BLACK] = artworks;
			else
				Artworks.Add(ECardColor.BLACK, artworks);
		}

		/// <summary>Initializes blue artworks.</summary>
		private void InitializeBlueArtworks()
		{
			List<Artwork> artworks = new List<Artwork>()
			{
				new Artwork("Artworks/Blue/art-1.jpg", ECardColor.BLUE, -14),
				new Artwork("Artworks/Blue/art-2.jpg", ECardColor.BLUE, -14),
				new Artwork("Artworks/Blue/art-3.jpg", ECardColor.BLUE, -14),
				new Artwork("Artworks/Blue/art-4.jpg", ECardColor.BLUE, -10)
			};

			if (Artworks.ContainsKey(ECardColor.BLUE))
				Artworks[ECardColor.BLUE] = artworks;
			else
				Artworks.Add(ECardColor.BLUE, artworks);
		}

		/// <summary>Initializes green artworks.</summary>
		private void InitializeGreenArtworks()
		{
			List<Artwork> artworks = new List<Artwork>()
			{
				new Artwork("Artworks/Green/art-1.jpg", ECardColor.GREEN, -20),
				new Artwork("Artworks/Green/art-2.jpg", ECardColor.GREEN, -20),
				new Artwork("Artworks/Green/art-3.jpg", ECardColor.GREEN, -30),
				new Artwork("Artworks/Green/art-4.jpg", ECardColor.GREEN, -30),
				new Artwork("Artworks/Green/art-5.jpg", ECardColor.GREEN, -30),
				new Artwork("Artworks/Green/art-6.jpg", ECardColor.GREEN, -20),
				new Artwork("Artworks/Green/art-7.jpg", ECardColor.GREEN, -10),
				new Artwork("Artworks/Green/art-8.jpg", ECardColor.GREEN, -20)
			};

			if (Artworks.ContainsKey(ECardColor.GREEN))
				Artworks[ECardColor.GREEN] = artworks;
			else
				Artworks.Add(ECardColor.GREEN, artworks);
		}

		/// <summary>Initializes red artworks.</summary>
		private void InitializeRedArtworks()
		{
			List<Artwork> artworks = new List<Artwork>()
			{
				new Artwork("Artworks/Red/art-1.jpg", ECardColor.RED, 0),
				new Artwork("Artworks/Red/art-2.jpg", ECardColor.RED, -10),
				new Artwork("Artworks/Red/art-3.jpg", ECardColor.RED, -10),
				new Artwork("Artworks/Red/art-4.jpg", ECardColor.RED, -20),
				new Artwork("Artworks/Red/art-5.jpg", ECardColor.RED, 0),
				new Artwork("Artworks/Red/art-6.jpg", ECardColor.RED, 0)
			};

			if (Artworks.ContainsKey(ECardColor.RED))
				Artworks[ECardColor.RED] = artworks;
			else
				Artworks.Add(ECardColor.RED, artworks);
		}


		/// <summary>Initializes white artworks.</summary>
		private void InitializeWhiteArtworks()
		{
			List<Artwork> artworks = new List<Artwork>()
			{
				new Artwork("Artworks/White/art-1.jpg", ECardColor.WHITE, -20),
				new Artwork("Artworks/White/art-2.jpg", ECardColor.WHITE, -15),
				new Artwork("Artworks/White/art-3.jpg", ECardColor.WHITE, 0),
				new Artwork("Artworks/White/art-4.jpg", ECardColor.WHITE, -8),
				new Artwork("Artworks/White/art-5.jpg", ECardColor.WHITE, -10),
				new Artwork("Artworks/White/art-6.jpg", ECardColor.WHITE, -10),
				new Artwork("Artworks/White/art-7.jpg", ECardColor.WHITE, -10),
				new Artwork("Artworks/White/art-8.jpg", ECardColor.WHITE, -3),
				new Artwork("Artworks/White/art-9.jpg", ECardColor.WHITE, 0),
				new Artwork("Artworks/White/art-10.jpg", ECardColor.WHITE, -10)
			};

			if (Artworks.ContainsKey(ECardColor.WHITE))
				Artworks[ECardColor.WHITE] = artworks;
			else
				Artworks.Add(ECardColor.WHITE, artworks);
		}

		#endregion
	}
}
