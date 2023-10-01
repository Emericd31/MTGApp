namespace BlazorApp.Data
{
	/// <summary>Possible values of set type.</summary>
	public enum ESetType
	{
		EXPANSION,
		PROMO,
		COMMANDER,
		FUNNY,
		TOKEN,
		OTHERS
	}

	/// <summary>Possible values of collection action.</summary>
	public enum ECollectionAction
	{
		ADD,
		REMOVE
	}

	/// <summary>Possible values of card color.</summary>
	public enum ECardColor
	{
		GREEN,
		BLUE,
		RED,
		WHITE,
		BLACK,
		ARTIFACT,
		BICOLOR
	}

	/// <summary>Possible values of card rarity.</summary>
	public enum ECardRarity
	{
		UNKNWOWN,
		COMMON,
		UNCOMMON,
		RARE,
		SPECIAL,
		MYTHIC,
		BONUS
	}

	/// <summary>Possible values of card type.</summary>
	public enum ECardType
	{
		UNKNOWN,
		CREATURE,
		SORCERY,
		ENCHANTMENT,
		INSTANT,
		ARTIFACT,
		SIEGE,
		PLANESWALKER,
		LAND,
		LEGENDARY
	}
}
