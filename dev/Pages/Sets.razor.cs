using System.Collections.ObjectModel;
using BlazorApp.Data;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Pages
{
	/// <summary>Class that handles data and treatment for Expansion page.</summary>
	public class SetsBase : ComponentBase
	{
		#region Privates Properties

		/// <summary>Boolean indicating if classic expansions are displayed.</summary>
		private bool _displayExtension { get; set; }

		/// <summary>Boolean indicating if promo expansions are displayed.</summary>
		private bool _displayPromo { get; set; }

		/// <summary>Boolean indicating if commander expansions are displayed.</summary>
		private bool _displayCommander { get; set; }

		/// <summary>Boolean indicating if funny expansions are displayed.</summary>
		private bool _displayFunny { get; set; }

		/// <summary>Boolean indicating if funny expansions are displayed.</summary>
		private bool _displayToken { get; set; }

		/// <summary>Boolean indicating if others expansions are displayed.</summary>
		private bool _displayOthers { get; set; }

		/// <summary>Search input value.</summary>
		private string _searchValue { get; set; } = "";

		#endregion

		#region Protected Properties

		/// <summary>Page title.</summary>
		protected string PageTitle { get; set; } = "Liste des extensions Magic";

		/// <summary>Gets or sets <see cref="_searchValue"/>.</summary>
		protected string SearchInput
		{
			get { return _searchValue; }
			set
			{
				_searchValue = value;
				if (_searchValue == "")
				{
					var setsToDisplay = DataService.Instance.Sets.Where(s => (_displayExtension && s.SetType == ESetType.EXPANSION)
						|| (_displayFunny && s.SetType == ESetType.FUNNY)
						|| (_displayPromo && s.SetType == ESetType.PROMO)
						|| (_displayCommander && s.SetType == ESetType.COMMANDER)
						|| (_displayToken && s.SetType == ESetType.TOKEN)
						|| (_displayOthers && s.SetType == ESetType.OTHERS)
						).OrderByDescending(e => e.ReleaseDate).ToList();
					ObservableSets = new Collection<Set>(setsToDisplay);
				}
				else
				{
					var items = DataService.Instance.Sets.Where(set => set.Name.ToLower().Contains(SearchInput.ToLower())).ToList();
					ObservableSets = new Collection<Set>(items);
				}
			}
		}

		/// <summary>Gets or sets <see cref="_displayExtension"/>.</summary>
		protected bool DisplayExtension
		{
			get { return _displayExtension; }
			set
			{
				_displayExtension = value;
				ChangeDisplay(value, ESetType.EXPANSION);
			}
		}

		/// <summary>Gets or sets <see cref="_displayPromo"/>.</summary>
		protected bool DisplayPromo
		{
			get { return _displayPromo; }
			set
			{
				_displayPromo = value;
				ChangeDisplay(value, ESetType.PROMO);
			}
		}

		/// <summary>Gets or sets <see cref="_displayCommander"/>.</summary>
		protected bool DisplayCommander
		{
			get { return _displayCommander; }
			set
			{
				_displayCommander = value;
				ChangeDisplay(value, ESetType.COMMANDER);
			}
		}

		/// <summary>Gets or sets <see cref="_displayFunny"/>.</summary>
		protected bool DisplayFunny
		{
			get { return _displayFunny; }
			set
			{
				_displayFunny = value;
				ChangeDisplay(value, ESetType.FUNNY);
			}
		}

		/// <summary>Gets or sets <see cref="_displayToken"/>.</summary>
		protected bool DisplayToken
		{
			get { return _displayToken; }
			set
			{
				_displayToken = value;
				ChangeDisplay(value, ESetType.TOKEN);
			}
		}

		/// <summary>Gets or sets <see cref="_displayOthers"/>.</summary>
		protected bool DisplayOthers
		{
			get { return _displayOthers; }
			set
			{
				_displayOthers = value;
				ChangeDisplay(value, ESetType.OTHERS);
			}
		}

		/// <summary>List of displayed expansions.</summary>

		protected Collection<Set> ObservableSets = new Collection<Set>();

		/// <summary>Boolean indicating if a search action is in progress.</summary>
		protected bool IsLoading { get; set; } = true;

		#endregion

		#region Protected Methods

		/// <summary>Method called when page is initialized.</summary>
		protected override void OnInitialized()
		{
			_displayExtension = true;
			_displayPromo = false;
			_displayCommander = false;
			_displayFunny = false;
			_displayToken = false;
			_displayOthers = false;
			var setsToDisplay = DataService.Instance.Sets.Where(s => (_displayExtension && s.SetType == ESetType.EXPANSION)
			|| (_displayFunny && s.SetType == ESetType.FUNNY)
			|| (_displayPromo && s.SetType == ESetType.PROMO)
			|| (_displayCommander && s.SetType == ESetType.COMMANDER)
			|| (_displayToken && s.SetType == ESetType.TOKEN)
			|| (_displayOthers && s.SetType == ESetType.OTHERS)
			).OrderByDescending(e => e.ReleaseDate).ToList();
			ObservableSets = new ObservableCollection<Set>(setsToDisplay);
			base.OnInitialized();
		}

		/// <summary>Method called after each rendering.</summary>
		/// <param name="firstRender">Boolean indicating if it's the first rendering.</param>
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender && DataService.Instance.Sets.Count == 0)
			{
				var setsToDisplay = DataService.Instance.Sets.Where(s => (_displayExtension && s.SetType == ESetType.EXPANSION)
				|| (_displayFunny && s.SetType == ESetType.FUNNY)
				|| (_displayPromo && s.SetType == ESetType.PROMO)
				|| (_displayCommander && s.SetType == ESetType.COMMANDER)
				|| (_displayToken && s.SetType == ESetType.TOKEN)
				|| (_displayOthers && s.SetType == ESetType.OTHERS)
				).OrderByDescending(e => e.ReleaseDate).ToList();
				ObservableSets = new Collection<Set>(setsToDisplay);
				StateHasChanged();
			}
		}

		#endregion

		#region Private Methods

		/// <summary>Changes displayed expansions.</summary>
		/// <param name="display">Boolean indicating if expansion type should be displayed.</param>
		/// <param name="setType">Expansion type.</param>
		private void ChangeDisplay(bool display, ESetType setType)
		{
			var items = DataService.Instance.Sets.Where(s => s.SetType == setType).ToList();
			var current = ObservableSets;
			if (display)
				foreach (var item in items)
					current.Add(item);
			else
				foreach (var item in items)
					current.Remove(item);
			ObservableSets = new ObservableCollection<Set>(current.OrderByDescending(set => set.ReleaseDate));
		}

		#endregion
	}
}