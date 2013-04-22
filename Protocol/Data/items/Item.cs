

// Generated on 04/17/2013 22:30:13
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Items")]
    public class Item : IDataObject
    {
        public const String MODULE = "Items";
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
        public String criteriaTarget;
        public Boolean hideEffects;
        public Boolean enhanceable;
        public Boolean nonUsableOnAnother;
        public uint appearanceId;
        public Boolean secretRecipe;
        public List<uint> dropMonsterIds;
        public uint recipeSlots;
        public List<uint> recipeIds;
        public Boolean bonusIsSecret;
        public List<EffectInstance> possibleEffects;
        public List<uint> favoriteSubAreas;
        public uint favoriteSubAreasBonus;
        public ItemType type;
        public uint weight;
    }
}