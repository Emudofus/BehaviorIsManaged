using System.Linq;

namespace BiM.Behaviors.Game.Items
{
    public partial class Inventory
    {
        public bool AutomaticallyDestroyItemsOnOverload
        {
            get;
            set;
        }

        public void FixInventoryOverloadIfNeeded()
        {
            int weight = Weight;
            if (weight > WeightMax && AutomaticallyDestroyItemsOnOverload)
                foreach (Item item in Items.Where(item => item.IsAutomaticallyDeletable).OrderBy(item => item.Template.price / item.UnityWeight))
                {
                    if (weight <= WeightMax) return; // Inventory fixed
                    int tooMuch = weight - WeightMax;
                    if (item.TotalWeight < tooMuch) // Completely delete this item stack
                    {
                        Delete(item);
                        weight -= (int)item.TotalWeight;
                    }
                    else
                    { // Only partially delete this stack
                        int nbItemsToDelete = (tooMuch + (int)item.UnityWeight - 1) / (int)item.UnityWeight;
                        if (nbItemsToDelete > item.Quantity)
                            nbItemsToDelete = item.Quantity;
                        Delete(item, nbItemsToDelete);
                        weight -= (int)item.UnityWeight * nbItemsToDelete;
                    }
                }
        }

        /// <summary>
        /// Returns true if a weapon (and NOT a tool) is allready equiped. 
        /// Otherwise, looks for the "best" weapon available, and equip it. 
        /// </summary>
        /// <returns></returns>
        public bool EquipBestWeaponIfNeeded()
        {
            BiM.Behaviors.Game.Items.Item equipped = GetEquippedItem(BiM.Protocol.Enums.CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON);
            if (equipped != null && equipped.IsWeapon) return true; // A weapon is allready equipped => No change

            foreach (BiM.Behaviors.Game.Items.Item item in GetItems(ItemSuperTypeEnum.SUPERTYPE_WEAPON).Union(GetItems(ItemSuperTypeEnum.SUPERTYPE_WEAPON_7)).OrderByDescending(item => item.Level * 10 + item.PowerRate))
            {
                if (!item.IsWeapon) continue;

                Protocol.Data.Item wpn = item.Template;
                if (wpn.cursed || wpn.etheral) continue; // Better don't equip this

                if (!item.CheckCriteria(Owner)) continue; // Not the stats to use it
                if (Owner.Level < wpn.level) continue; // not the level to use it
                if (Equip(item))
                    return true;
            }
            return false; // No weapon found
        }

    }
}
