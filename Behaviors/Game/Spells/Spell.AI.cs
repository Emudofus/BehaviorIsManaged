#region License GNU GPL
// Spell.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiM.Protocol.Data;
using BiM.Behaviors.Game.Effects;

namespace BiM.Behaviors.Game.Spells
{
    public partial class Spell
    {
        #region Categories
        [Flags]
        public enum SpellCategory
        {
            Healing = 0x0001,
            Teleport = 0x0002,
            Invocation = 0x0004,
            Buff = 0x0008,
            DamagesWater = 0x0010,
            DamagesEarth = 0x0020,
            DamagesAir = 0x0040,
            DamagesFire = 0x0080,
            DamagesNeutral = 0x0100,
        }


        private SpellCategory GetEffectCategories(EffectInstanceDice effect)
        {
            SpellCategory categories = 0;
            switch ((EffectsEnum)effect.effectId)
            {
                case EffectsEnum.Effect_StealHPAir:
                    return SpellCategory.DamagesAir | SpellCategory.Healing;
                case EffectsEnum.Effect_StealHPWater:
                    return SpellCategory.DamagesWater | SpellCategory.Healing;
                case EffectsEnum.Effect_StealHPFire:
                    return SpellCategory.DamagesFire | SpellCategory.Healing;
                case EffectsEnum.Effect_StealHPEarth:
                    return SpellCategory.DamagesEarth | SpellCategory.Healing;
                case EffectsEnum.Effect_StealHPNeutral:
                    return SpellCategory.DamagesNeutral | SpellCategory.Healing;
                case EffectsEnum.Effect_DamageFire:
                    return SpellCategory.DamagesFire;
                case EffectsEnum.Effect_DamageWater:
                    return SpellCategory.DamagesWater;
                case EffectsEnum.Effect_DamageAir:
                    return SpellCategory.DamagesAir;
                case EffectsEnum.Effect_DamageNeutral:
                case EffectsEnum.Effect_Punishment_Damage:
                    return SpellCategory.DamagesNeutral;
                case EffectsEnum.Effect_DamageEarth:
                    return SpellCategory.DamagesEarth;
                case EffectsEnum.Effect_HealHP_108:
                case EffectsEnum.Effect_HealHP_143:
                case EffectsEnum.Effect_HealHP_81:
                    return SpellCategory.Healing;
                case EffectsEnum.Effect_Summon:
                case EffectsEnum.Effect_Double:
                    return SpellCategory.Invocation;
                case EffectsEnum.Effect_AddArmorDamageReduction:
                case EffectsEnum.Effect_AddAirResistPercent:
                case EffectsEnum.Effect_AddFireResistPercent:
                case EffectsEnum.Effect_AddEarthResistPercent:
                case EffectsEnum.Effect_AddWaterResistPercent:
                case EffectsEnum.Effect_AddNeutralResistPercent:
                case EffectsEnum.Effect_AddAirElementReduction:
                case EffectsEnum.Effect_AddFireElementReduction:
                case EffectsEnum.Effect_AddEarthElementReduction:
                case EffectsEnum.Effect_AddWaterElementReduction:
                case EffectsEnum.Effect_AddNeutralElementReduction:
                case EffectsEnum.Effect_AddAgility:
                case EffectsEnum.Effect_AddStrength:
                case EffectsEnum.Effect_AddIntelligence:
                case EffectsEnum.Effect_AddHealth:
                case EffectsEnum.Effect_AddChance:
                case EffectsEnum.Effect_AddCriticalHit:
                case EffectsEnum.Effect_AddCriticalDamageBonus:
                case EffectsEnum.Effect_AddCriticalDamageReduction:
                case EffectsEnum.Effect_AddDamageBonus:
                case EffectsEnum.Effect_AddDamageBonusPercent:
                case EffectsEnum.Effect_AddDamageBonus_121:
                case EffectsEnum.Effect_AddFireDamageBonus:
                case EffectsEnum.Effect_AddAirDamageBonus:
                case EffectsEnum.Effect_AddWaterDamageBonus:
                case EffectsEnum.Effect_AddEarthDamageBonus:
                case EffectsEnum.Effect_AddNeutralDamageBonus:
                case EffectsEnum.Effect_AddDamageMultiplicator:
                case EffectsEnum.Effect_AddDamageReflection:
                case EffectsEnum.Effect_AddGlobalDamageReduction:
                case EffectsEnum.Effect_AddGlobalDamageReduction_105:
                case EffectsEnum.Effect_AddAP_111:
                case EffectsEnum.Effect_AddHealBonus:
                case EffectsEnum.Effect_AddWisdom:
                case EffectsEnum.Effect_AddProspecting:
                case EffectsEnum.Effect_AddMP:
                case EffectsEnum.Effect_AddMP_128:
                case EffectsEnum.Effect_AddPhysicalDamage_137:
                case EffectsEnum.Effect_AddPhysicalDamage_142:
                case EffectsEnum.Effect_AddPhysicalDamageReduction:
                case EffectsEnum.Effect_AddPushDamageReduction:
                case EffectsEnum.Effect_AddPushDamageBonus:
                case EffectsEnum.Effect_AddRange:
                case EffectsEnum.Effect_AddRange_136:
                case EffectsEnum.Effect_AddSummonLimit:
                case EffectsEnum.Effect_AddVitality:
                case EffectsEnum.Effect_AddVitalityPercent:
                case EffectsEnum.Effect_Dodge:
                case EffectsEnum.Effect_IncreaseAPAvoid:
                case EffectsEnum.Effect_IncreaseMPAvoid:
                case EffectsEnum.Effect_Invisibility:
                case EffectsEnum.Effect_ReflectSpell:
                case EffectsEnum.Effect_RegainAP:
                    return SpellCategory.Buff;
                case EffectsEnum.Effect_Teleport:
                    return SpellCategory.Teleport;
            }
            return categories;

        }

        public SpellCategory Categories
        {
            get
            {
                SpellCategory categories = 0;
                foreach (var eff in LevelTemplate.effects)
                    categories |= GetEffectCategories(eff);
                return categories;
            }
        }

        public bool IsAttack
        {
            get
            {
                return (Categories & (SpellCategory.DamagesWater | SpellCategory.DamagesEarth | SpellCategory.DamagesFire | SpellCategory.DamagesAir | SpellCategory.DamagesNeutral)) > 0;
            }
        }

        public bool IsSelfHealing
        {
            get
            {
                return (Categories & (SpellCategory.Healing)) > 0 && LevelTemplate.range == 0;
            }
        }

        public bool IsFriendHealing
        {
            get
            {
                return (Categories & (SpellCategory.Healing)) > 0 && LevelTemplate.range > 0;
            }
        }
        #endregion Categories

    }
}
