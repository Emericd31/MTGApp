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

		/// <summary>List of existing Magic expansions.</summary>
		public List<Set> Sets { get; set; } = new List<Set>();

		/// <summary>Folder containing external application's data.</summary>
		public string ApplicationDataFolder { get; set; }

		#endregion

		#region Constructor

		/// <summary>Constructor.</summary>
		private DataService()
		{
			MyCollection = new Collection("MyCollection", "Description");
		}

		#endregion

		#region Public Methods

		/// <summary>Initializes application's data.</summary>
		/// <returns></returns>
		public async Task InitializeData()
		{
			Instance.Sets = await SetAPI.GetSets().ConfigureAwait(false);
		}

		#endregion
	}
}
