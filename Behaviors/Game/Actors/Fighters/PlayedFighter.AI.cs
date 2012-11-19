using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BiM.Behaviors.Game.Actors.Fighters
{
  public partial class PlayedFighter
  {
    public IEnumerable<Spells.Spell> GetOrderListOfSimpleAttackSpells(Fighter target)
    {
      foreach (Spells.Spell spell in Character.SpellsBook.GetOrderedAttackSpells(Character, target, null))
      {
        if (CanCastSpell(spell, target) && !spell.LevelTemplate.needFreeCell && !spell.LevelTemplate.needFreeCell)
          yield return spell;
      }
    }
  }
}
