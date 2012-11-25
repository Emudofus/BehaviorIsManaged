using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BiM.Behaviors.Game.Actors.Fighters
{
    public partial class PlayedFighter
    {
        public IEnumerable<Spells.Spell> GetOrderListOfSimpleAttackSpells(Fighter target, bool NoRangeCheck = false)
        {
            return Character.SpellsBook.GetOrderedAttackSpells(Character, target, Spells.Spell.SpellCategory.Damages).
                Where(spell => CanCastSpell(spell, target, NoRangeCheck) && !spell.LevelTemplate.needFreeCell && !spell.LevelTemplate.needFreeCell);
        }
    }
}
