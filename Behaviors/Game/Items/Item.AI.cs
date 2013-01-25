using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Protocol.Data;

namespace BiM.Behaviors.Game.Items
{
    public partial class Item
    {
        public bool CheckCriteria(PlayedCharacter pc)
        {
            if (string.IsNullOrEmpty(this.Template.criteria)) return true;
            return pc.CheckCriteria(this.Template.criteria);
        }

        public bool IsAutomaticallyDeletable
        {
            get
            {
                return !IsEquipped &&
                        !IsUsable &&
                        SuperType != ItemSuperTypeEnum.SUPERTYPE_QUEST &&
                        SuperType != ItemSuperTypeEnum.SUPERTYPE_DOFUS &&
                        Quantity > 1 &&
                        Template.price > 0;
            }
        }

        public bool IsWeapon
        {
            get
            {
                if (SuperType != ItemSuperTypeEnum.SUPERTYPE_WEAPON && SuperType != ItemSuperTypeEnum.SUPERTYPE_WEAPON_7) return false;
                if (Template.typeId == 19 || Template.typeId == 20 || Template.typeId == 21 || Template.typeId == 22 || // Tool
                        Template.typeId == 99 || // filet de capture
                        Template.typeId == 83 // pierre d'âme
                      ) return false;
                return Template is Weapon;
            }
        }

    }
}
