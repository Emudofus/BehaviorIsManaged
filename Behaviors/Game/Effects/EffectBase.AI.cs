#region License GNU GPL
// EffectBase.AI.cs
// 
// Copyright (C) 2012, 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

// Author : FastFrench - antispam@laposte.net
#endregion
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.Spells;
using BiM.Protocol.Enums;

namespace BiM.Behaviors.Game.Effects
{
    public partial class EffectBase
    {
        /// <summary>
        /// Warning : this method says if this affect may affect this target. But NOT if the target can be the target of the spell 
        /// (cf épée divine, where you cast it on yourself despite it effects only enemies around you)
        /// </summary>
        /// <param name="spellEffect"></param>
        /// <param name="spell"></param>
        /// <param name="caster"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool canAffectTarget(EffectDice spellEffect, Spell spell, PlayedFighter caster, Fighter target)
        {
            if (spell.LevelTemplate.spellBreed == (uint)BreedEnum.Eniripsa && spell.Categories == Spell.SpellCategory.Healing && caster.HasState(76)) return false;
            //if (!spell.IsAvailable(target == null ? null : (int?)target.Id)) return false;
            //BiM.Behaviors.Game.Spells.Spell.SpellCategory categories = 0;
            uint surface = spellEffect.Surface;
            //categories = BiM.Behaviors.Game.Spells.Spell.GetEffectCategories((uint)spellEffect.Id, spell.LevelTemplate.id);
            if (spellEffect.Targets == SpellTargetType.NONE) spellEffect.Targets = SpellTargetType.ALL;
            //if (target == null) return !spell.LevelTemplate.needTakenCell;

            if (caster == target) // Self
                return ((spellEffect.Targets & (SpellTargetType.ONLY_SELF | SpellTargetType.SELF)) != 0);

           
            if (caster.Team == target.Team) // Ally
                if (target.Summoned)
                    return ((spellEffect.Targets & SpellTargetType.ALLIES_SUMMON) != 0);
                else
                    return ((spellEffect.Targets & SpellTargetType.ALLIES_NON_SUMMON) != 0);

            if (target.Summoned) // Enemies
                return ((spellEffect.Targets & SpellTargetType.ENEMIES_SUMMON) != 0);
            else
                return ((spellEffect.Targets & SpellTargetType.ENEMIES_NON_SUMMON) != 0);
        }
    }
}
