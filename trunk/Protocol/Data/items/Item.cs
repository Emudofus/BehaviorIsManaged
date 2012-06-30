using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;
namespace BiM.Protocol.Data
{
	[D2OClass("Items")]
	[Serializable]
	public class Item : IDataObject
	{
		private const String MODULE = "Items";
		public const uint EQUIPEMENT_CATEGORY = 0;
		public const uint CONSUMABLES_CATEGORY = 1;
		public const uint RESSOURCES_CATEGORY = 2;
		public const uint QUEST_CATEGORY = 3;
		public const uint OTHER_CATEGORY = 4;
		public int id;
		public uint nameId;
		public uint typeId;
		public uint descriptionId;
		public int iconId;
		public uint level;
		public uint realWeight;
		public Boolean cursed;
		public int useAnimationId;
		public Boolean usable;
		public Boolean targetable;
		public float price;
		public Boolean twoHanded;
		public Boolean etheral;
		public int itemSetId;
		public String criteria;
		public Boolean hideEffects;
		public Boolean enhanceable;
		public uint appearanceId;
		public List<uint> recipeIds;
		public Boolean bonusIsSecret;
		public List<EffectInstance> possibleEffects;
		public List<uint> favoriteSubAreas;
		public uint favoriteSubAreasBonus;
		public ItemType type;
		public uint weight;
	}
}
