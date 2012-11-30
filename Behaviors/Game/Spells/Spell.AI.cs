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
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.Effects;
using BiM.Protocol.Data;
using DamageType = System.Double;
using BiM.Behaviors.Game.Stats; // One can either try as double or uint. Results will sightly differ. 

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
            Damages = DamagesNeutral | DamagesFire | DamagesAir | DamagesEarth | DamagesWater,
            None = 0,
            All = 0x01FF,
        }


        private void InitAI()
        {
            Categories = 0;
            foreach (var eff in LevelTemplate.effects)
                Categories |= GetEffectCategories(eff);
        }

        public SpellCategory Categories
        {
            get;
            private set;
        }

        public bool HasCategory(SpellCategory? category)
        {
            if (category == null) return true;
            return (category & Categories) > 0;
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

        #region Effects
        private static SpellCategory GetEffectCategories(EffectInstanceDice effect)
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

        public IEnumerable<EffectBase> GetEffects(SpellCategory? category = null)
        {
            foreach (EffectInstanceDice effect in LevelTemplate.effects)
                if (category == null || (GetEffectCategories(effect) & category) > 0)
                    yield return new EffectBase(effect);
        }

        public class Damages
        {
            public DamageType MinFire, MaxFire,
                 MinWater, MaxWater,
                 MinEarth, MaxEarth,
                 MinAir, MaxAir,
                 MinNeutral, MaxNeutral;

            public DamageType Fire { get { return (MinFire + MaxFire) / 2; } }
            public DamageType Air { get { return (MinAir + MaxAir) / 2; } }
            public DamageType Earth { get { return (MinEarth + MaxEarth) / 2; } }
            public DamageType Water { get { return (MinWater + MaxWater) / 2; } }
            public DamageType Neutral { get { return (MinEarth + MaxEarth) / 2; } }

            // Min total damage            
            public DamageType MinDamage { get { return MinFire + MinAir + MinEarth + MinWater + MinNeutral; } }

            // Max total damage            
            public DamageType MaxDamage { get { return MaxFire + MaxAir + MaxEarth + MaxWater + MaxNeutral; } }

            // Average total damage
            public DamageType Damage { get { return (MinDamage + MaxDamage) / 2; } }

            public void Add(Damages dmg)
            {
                MinFire += dmg.MinFire;
                MaxFire += dmg.MaxFire;
                MinWater += dmg.MinWater;
                MaxWater += dmg.MaxWater;
                MinEarth += dmg.MinEarth;
                MaxEarth += dmg.MaxEarth;
                MinAir += dmg.MinAir;
                MaxAir += dmg.MaxAir;
                MinNeutral += dmg.MinNeutral;
                MaxNeutral += dmg.MaxNeutral;
            }
        }

        private static void AdjustDamage(Damages damages, uint damage1, uint damage2, SpellCategory category, double chanceToHappen, int addDamage, int addDamagePercent, int reduceDamage, int reduceDamagePercent)
        {
            DamageType minDamage = damage1 >= damage2 ? damage2 : damage1;
            DamageType maxDamage = damage1 >= damage2 ? damage1 : damage2;
            if (reduceDamagePercent >= 100)
                return; // No damage
            minDamage = (DamageType)(((minDamage * (1 + (addDamagePercent / 100.0)) + addDamagePercent) - reduceDamage) * (1 - (reduceDamagePercent / 100.0)) * chanceToHappen);
            maxDamage = (DamageType)(((maxDamage * (1 + (addDamagePercent / 100.0)) + addDamagePercent) - reduceDamage) * (1 - (reduceDamagePercent / 100.0)) * chanceToHappen);
            switch (category)
            {
                case SpellCategory.DamagesNeutral:
                    damages.MinNeutral += minDamage;
                    damages.MaxNeutral += maxDamage;
                    break;
                case SpellCategory.DamagesFire:
                    damages.MinFire += minDamage;
                    damages.MaxAir += maxDamage;
                    break;
                case SpellCategory.DamagesAir:
                    damages.MinAir += minDamage;
                    damages.MaxAir += maxDamage;
                    break;
                case SpellCategory.DamagesWater:
                    damages.MinWater += minDamage;
                    damages.MaxWater += maxDamage;
                    break;
                case SpellCategory.DamagesEarth:
                    damages.MinEarth += minDamage;
                    damages.MaxEarth += maxDamage;
                    break;
            }
        }
        private static int GetSafetotal(PlayedCharacter caster, Stats.PlayerField field)
        {
            StatsRow row = caster.Stats[field];
            if (row == null) return 0;
            return row.Total;
        }

        /// <summary>
        /// Add damages for a given effect, taking into account caster bonus and target resistance. 
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="damages"></param>
        /// <param name="caster"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Damages CumulDamages(EffectInstanceDice effect, Damages damages, PlayedCharacter caster, Fighter target)
        {
            Damages result = damages;
            if (damages == null) result = new Damages();
            SpellTargetType targetType = (SpellTargetType)effect.targetId;

            //if ((targetType & SpellTargetType.ENEMIES) == 0) return damages; // No enemy can be targeted

            SpellCategory category = GetEffectCategories(effect);

            double chanceToHappen = 1.0; // 

            // When chances to happen is under 100%, then we reduce damages accordingly, for simplicity, but after having apply damage bonus & reduction. 
            // So average damage should remain exact even if Min and Max are not. 
            if (effect.random > 0)
                chanceToHappen = effect.random / 100.0;

            if ((category & SpellCategory.DamagesNeutral) > 0)
                AdjustDamage(damages, effect.diceNum, effect.diceSide, SpellCategory.DamagesNeutral, chanceToHappen,
                    caster == null ? 0 : GetSafetotal(caster, Stats.PlayerField.NeutralDamageBonus) + GetSafetotal(caster, Stats.PlayerField.DamageBonus) + GetSafetotal(caster, Stats.PlayerField.PhysicalDamage),
                    caster == null ? 0 : GetSafetotal(caster, Stats.PlayerField.DamageBonusPercent),
                    target == null ? 0 : target.Stats.NeutralElementReduction,
                    target == null ? 0 : target.Stats.NeutralResistPercent);

            if ((category & SpellCategory.DamagesFire) > 0)
                AdjustDamage(damages, effect.diceNum, effect.diceSide, SpellCategory.DamagesFire, chanceToHappen,
                    caster == null ? 0 : GetSafetotal(caster, Stats.PlayerField.FireDamageBonus) + GetSafetotal(caster, Stats.PlayerField.DamageBonus) + GetSafetotal(caster, Stats.PlayerField.MagicDamage),
                    caster == null ? 0 : GetSafetotal(caster, Stats.PlayerField.DamageBonusPercent),
                    target == null ? 0 : target.Stats.FireElementReduction,
                    target == null ? 0 : target.Stats.FireResistPercent);

            if ((category & SpellCategory.DamagesAir) > 0)
                AdjustDamage(damages, effect.diceNum, effect.diceSide, SpellCategory.DamagesAir, chanceToHappen,
                    caster == null ? 0 : GetSafetotal(caster, Stats.PlayerField.AirDamageBonus) + GetSafetotal(caster, Stats.PlayerField.DamageBonus) + GetSafetotal(caster, Stats.PlayerField.MagicDamage),
                    caster == null ? 0 : GetSafetotal(caster, Stats.PlayerField.DamageBonusPercent),
                    target == null ? 0 : target.Stats.AirElementReduction,
                    target == null ? 0 : target.Stats.AirResistPercent);

            if ((category & SpellCategory.DamagesWater) > 0)
                AdjustDamage(damages, effect.diceNum, effect.diceSide, SpellCategory.DamagesWater, chanceToHappen,
                    caster == null ? 0 : GetSafetotal(caster, Stats.PlayerField.WaterDamageBonus) + GetSafetotal(caster, Stats.PlayerField.DamageBonus) + GetSafetotal(caster, Stats.PlayerField.MagicDamage),
                    caster == null ? 0 : GetSafetotal(caster, Stats.PlayerField.DamageBonusPercent),
                    target == null ? 0 : target.Stats.WaterElementReduction,
                    target == null ? 0 : target.Stats.WaterResistPercent);

            if ((category & SpellCategory.DamagesEarth) > 0)
                AdjustDamage(damages, effect.diceNum, effect.diceSide, SpellCategory.DamagesEarth, chanceToHappen,
                    caster == null ? 0 : GetSafetotal(caster, Stats.PlayerField.EarthDamageBonus) + GetSafetotal(caster, Stats.PlayerField.DamageBonus) + GetSafetotal(caster, Stats.PlayerField.MagicDamage),
                    caster == null ? 0 : GetSafetotal(caster, Stats.PlayerField.DamageBonusPercent),
                    target == null ? 0 : target.Stats.EarthElementReduction,
                    target == null ? 0 : target.Stats.EarthResistPercent);

            return result;
        }

        #endregion Effects

        public Damages GetSpellDamages(PlayedCharacter caster, Fighter target)
        {
            Damages damages = new Damages();
            foreach (var effect in LevelTemplate.effects)
            {
                CumulDamages(effect, damages, caster, target);
            }
            return damages;
        }

    }
}
