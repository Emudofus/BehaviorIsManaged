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
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.Effects;
using BiM.Behaviors.Game.Stats;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Data;
using DamageType = System.Double;
using BiM.Behaviors.Game.Spells.Shapes;
using BiM.Protocol.Enums;
// One can either try as double or uint. Results will sightly differ. 

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
            Curse = 0x0200,
            Damages = DamagesNeutral | DamagesFire | DamagesAir | DamagesEarth | DamagesWater,
            None = 0,
            All = 0x01FF,
        }

        private void InitAI()
        {
            Categories = 0;
            areaDependsOnDirection = false;
            foreach (var eff in LevelTemplate.effects)
            {
                Categories |= GetEffectCategories(eff.effectId, LevelTemplate.id);
                if (eff.zoneShape == (uint)SpellShapeEnum.L || eff.zoneShape == (uint)SpellShapeEnum.T || eff.zoneShape == (uint)SpellShapeEnum.D || eff.zoneShape == (uint)SpellShapeEnum.V || eff.zoneShape == (uint)SpellShapeEnum.slash || eff.zoneShape == (uint)SpellShapeEnum.U)
                    areaDependsOnDirection = true;
            }
        }

        public SpellCategory Categories
        {
            get;
            private set;
        }

        public bool areaDependsOnDirection
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


        #region Effects on the fighters
        DamageType GetDamageReflection(Fighter target)
        {
            DamageType reflect = 0;
            foreach (var effect in target.GetFightTriggeredEffects((short)EffectsEnum.Effect_AddDamageReflection))
                reflect += effect.arg3 + effect.arg1 * (1 + effect.arg2) / 2 ;
            return reflect * (1 + target.Level * 0.05);
        }
        #endregion

        #region Effects
        public class SpellException
        {
            public SpellException(SpellCategory category)
            {
                Category = category;
            }
            public SpellCategory Category { get; set; }
            public uint MinNeutral { get; set; }
            public uint MaxNeutral { get; set; }
            public uint MinEarth { get; set; }
            public uint MaxEarth { get; set; }
            public uint MinFire { get; set; }
            public uint MaxFire { get; set; }
            public uint MinAir { get; set; }
            public uint MaxAir { get; set; }
            public uint MinWater { get; set; }
            public uint MaxWater { get; set; }
            public bool Buf { get; set; }
            public bool Curse { get; set; }
        }

        static Dictionary<uint, SpellException> spellExceptions = new Dictionary<uint, SpellException>()
        {
          {46,    new SpellException(SpellCategory.DamagesFire) { MinFire = 22, MaxFire = 24}}, // Glyphe enflammée
          {47,    new SpellException(SpellCategory.DamagesFire) { MinFire = 24, MaxFire = 26}},
          {48,    new SpellException(SpellCategory.DamagesFire) { MinFire = 26, MaxFire = 28}},
          {49,    new SpellException(SpellCategory.DamagesFire) { MinFire = 28, MaxFire = 30}},
          {50,    new SpellException(SpellCategory.DamagesFire) { MinFire = 30, MaxFire = 32}},
          {10533, new SpellException(SpellCategory.DamagesFire) { MinFire = 30, MaxFire = 32}},

          {81,    new SpellException(SpellCategory.DamagesFire) { MinFire = 1, MaxFire = 3}}, //Glyphe agressif
          {82,    new SpellException(SpellCategory.DamagesFire) { MinFire = 2, MaxFire = 4}},
          {83,    new SpellException(SpellCategory.DamagesFire) { MinFire = 3, MaxFire = 5}},
          {84,    new SpellException(SpellCategory.DamagesFire) { MinFire = 5, MaxFire = 7}},
          {85,    new SpellException(SpellCategory.DamagesFire) { MinFire = 6, MaxFire = 8}},
          {3093,  new SpellException(SpellCategory.DamagesFire) { MinFire = 8, MaxFire = 10}},
        };

        public static SpellCategory GetEffectCategories(uint effectId, uint spellLvId)
        {
            if (spellExceptions.ContainsKey(spellLvId))
                return spellExceptions[spellLvId].Category;
            switch ((EffectsEnum)effectId)
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
                case EffectsEnum.Effect_185:
                case EffectsEnum.Effect_621:
                case EffectsEnum.Effect_623:
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
                case EffectsEnum.Effect_PushBack:
                case EffectsEnum.Effect_RemoveAP:
                case EffectsEnum.Effect_LostMP:
                case EffectsEnum.Effect_StealKamas:
                case EffectsEnum.Effect_LoseHPByUsingAP:
                case EffectsEnum.Effect_LosingAP:
                case EffectsEnum.Effect_LosingMP:
                case EffectsEnum.Effect_SubRange_135:
                case EffectsEnum.Effect_SkipTurn:
                case EffectsEnum.Effect_Kill:
                case EffectsEnum.Effect_SubDamageBonus:
                case EffectsEnum.Effect_SubChance:
                case EffectsEnum.Effect_SubVitality:
                case EffectsEnum.Effect_SubAgility:
                case EffectsEnum.Effect_SubIntelligence:
                case EffectsEnum.Effect_SubWisdom:
                case EffectsEnum.Effect_SubStrength:
                case EffectsEnum.Effect_SubDodgeAPProbability:
                case EffectsEnum.Effect_SubDodgeMPProbability:
                case EffectsEnum.Effect_SubAP:
                case EffectsEnum.Effect_SubMP:
                case EffectsEnum.Effect_SubCriticalHit:
                case EffectsEnum.Effect_SubMagicDamageReduction:
                case EffectsEnum.Effect_SubPhysicalDamageReduction:
                case EffectsEnum.Effect_SubInitiative:
                case EffectsEnum.Effect_SubProspecting:
                case EffectsEnum.Effect_SubHealBonus:
                case EffectsEnum.Effect_SubDamageBonusPercent:
                case EffectsEnum.Effect_197:
                case EffectsEnum.Effect_SubEarthResistPercent:
                case EffectsEnum.Effect_SubWaterResistPercent:
                case EffectsEnum.Effect_SubAirResistPercent:
                case EffectsEnum.Effect_SubFireResistPercent:
                case EffectsEnum.Effect_SubNeutralResistPercent:
                case EffectsEnum.Effect_SubEarthElementReduction:
                case EffectsEnum.Effect_SubWaterElementReduction:
                case EffectsEnum.Effect_SubAirElementReduction:
                case EffectsEnum.Effect_SubFireElementReduction:
                case EffectsEnum.Effect_SubNeutralElementReduction:
                case EffectsEnum.Effect_SubPvpEarthResistPercent:
                case EffectsEnum.Effect_SubPvpWaterResistPercent:
                case EffectsEnum.Effect_SubPvpAirResistPercent:
                case EffectsEnum.Effect_SubPvpFireResistPercent:
                case EffectsEnum.Effect_SubPvpNeutralResistPercent:
                case EffectsEnum.Effect_StealChance:
                case EffectsEnum.Effect_StealVitality:
                case EffectsEnum.Effect_StealAgility:
                case EffectsEnum.Effect_StealIntelligence:
                case EffectsEnum.Effect_StealWisdom:
                case EffectsEnum.Effect_StealStrength:
                case EffectsEnum.Effect_275:
                case EffectsEnum.Effect_276:
                case EffectsEnum.Effect_277:
                case EffectsEnum.Effect_278:
                case EffectsEnum.Effect_279:
                case EffectsEnum.Effect_411:
                case EffectsEnum.Effect_413:
                case EffectsEnum.Effect_SubCriticalDamageBonus:
                case EffectsEnum.Effect_SubPushDamageReduction:
                case EffectsEnum.Effect_SubCriticalDamageReduction:
                case EffectsEnum.Effect_SubEarthDamageBonus:
                case EffectsEnum.Effect_SubFireDamageBonus:
                case EffectsEnum.Effect_SubWaterDamageBonus:
                case EffectsEnum.Effect_SubAirDamageBonus:
                case EffectsEnum.Effect_SubNeutralDamageBonus:
                case EffectsEnum.Effect_StealAP_440:
                case EffectsEnum.Effect_StealMP_441:
                    return SpellCategory.Curse;
            }
            return SpellCategory.None;

        }

        public IEnumerable<EffectDice> GetEffects(SpellCategory? category = null)
        {
            foreach (EffectInstanceDice effect in LevelTemplate.effects)
                if (category == null || (GetEffectCategories(effect.effectId, LevelTemplate.id) & category) > 0)
                    yield return new EffectDice(effect);
        }

        public class SpellImpact
        {
            public DamageType MinFire, MaxFire,
                 MinWater, MaxWater,
                 MinEarth, MaxEarth,
                 MinAir, MaxAir,
                 MinNeutral, MaxNeutral,
                 MinHeal, MaxHeal;

            public DamageType Fire { get { return (MinFire + MaxFire) / 2; } }
            public DamageType Air { get { return (MinAir + MaxAir) / 2; } }
            public DamageType Earth { get { return (MinEarth + MaxEarth) / 2; } }
            public DamageType Water { get { return (MinWater + MaxWater) / 2; } }
            public DamageType Neutral { get { return (MinEarth + MaxEarth) / 2; } }
            public DamageType Heal { get { return (MinHeal + MaxHeal) / 2; } }
            public DamageType Curse { get; set; }
            public DamageType Boost { get; set; }

            //public string Comment { get; set; }
            // Min total damage            
            public DamageType MinDamage { get { return MinFire + MinAir + MinEarth + MinWater + MinNeutral - MaxHeal + Curse - Boost; } }

            // Max total damage            
            public DamageType MaxDamage { get { return MaxFire + MaxAir + MaxEarth + MaxWater + MaxNeutral - MinHeal + Curse - Boost; } }

            /// <summary>
            /// Return positive values for bad effects (curses and spellImpact) and négative values for good effects (heals and boosts)
            /// </summary>
            public DamageType Damage { get { return (MinDamage + MaxDamage) / 2; } }

            public void Add(SpellImpact dmg)
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
                MinHeal += dmg.MinHeal;
                MaxHeal += dmg.MaxHeal;
                Curse += dmg.Curse;
                Boost += dmg.Boost;
            }

            public void Multiply(double ratio)
            {
                MinFire *= ratio;
                MaxFire  *= ratio;            
                MinWater  *= ratio;           
                MaxWater  *= ratio;           
                MinEarth  *= ratio;           
                MaxEarth  *= ratio;           
                MinAir  *= ratio;             
                MaxAir  *= ratio;             
                MinNeutral  *= ratio;         
                MaxNeutral  *= ratio;         
                MinHeal  *= ratio;            
                MaxHeal  *= ratio;            
                Curse  *= ratio;              
                Boost  *= ratio;              
            }

        }

        private static void AdjustDamage(SpellImpact damages, uint damage1, uint damage2, SpellCategory category, double chanceToHappen, int addDamage, int addDamagePercent, int reduceDamage, int reduceDamagePercent, bool negativ)
        {
            DamageType minDamage = damage1;
            DamageType maxDamage = damage1 >= damage2 ? damage1 : damage2;
            if (reduceDamagePercent >= 100)
                return; // No damage
            minDamage = (DamageType)(((minDamage * (1 + (addDamagePercent / 100.0)) + addDamage) - reduceDamage) * (1 - (reduceDamagePercent / 100.0)) * chanceToHappen);
            maxDamage = (DamageType)(((maxDamage * (1 + (addDamagePercent / 100.0)) + addDamage) - reduceDamage) * (1 - (reduceDamagePercent / 100.0)) * chanceToHappen);
            
            if (minDamage < 0) minDamage = 0;
            if (maxDamage < 0) maxDamage = 0;
        
    
            if (negativ) // or IsFriend
            {
                minDamage *= -1.5; // High penalty for firing on friends
                maxDamage *= -1.5; // High penalty for firing on friends
            }
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
                case SpellCategory.Healing:
                    damages.MinHeal += minDamage;
                    damages.MaxHeal += maxDamage;
                    break;
            }
        }
        private static int GetSafetotal(PlayedFighter caster, Stats.PlayerField field)
        {
            if (caster == null) return 0;
            StatsRow row = caster.PCStats[field];
            if (row == null) return 0;
            return row.Total;
        }

        public bool IsMaitrise(uint? weaponType)
        {
            if (Template.typeId == 23) // Maîtrise
                return (weaponType == null || LevelTemplate.effects[0].diceNum == weaponType);
            return false;
        }

        /// <summary>
        /// Add spellImpact for a given effect, taking into account caster bonus and target resistance. 
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="spellImpact"></param>
        /// <param name="caster"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static DamageType CumulEffects(EffectInstanceDice effect, ref SpellImpact spellImpact, PlayedFighter caster, Fighter target/*, Spell.SpellCategory Categories*/, Spell spell)
        {
            bool isFriend = caster.Team.Id == target.Team.Id;
            SpellImpact result = new SpellImpact();

            SpellTargetType targetType = (SpellTargetType)effect.targetId;

                
            //if ((targetType & SpellTargetType.ENEMIES) == 0) return spellImpact; // No enemy can be targeted

            SpellCategory category = GetEffectCategories(effect.effectId, spell.LevelTemplate.id)/* & Categories*/;
            if (category == 0) return 0; // No category selected in this spell

            if (spell.Template.id == 0) // Weapon => ignore non heal or damage effects
                if ( (category & (SpellCategory.Damages | SpellCategory.Healing)) == 0) return 0;

            double chanceToHappen = 1.0; // 

            // When chances to happen is under 100%, then we reduce spellImpact accordingly, for simplicity, but after having apply damage bonus & reduction. 
            // So average damage should remain exact even if Min and Max are not. 
            if (effect.random > 0)
                chanceToHappen = effect.random / 100.0;

            if (target.Summoned && (caster.Breed.Id != (int)BreedEnum.Osamodas || target.Team.Id != caster.Team.Id))
                chanceToHappen /= 2; // It's much better to hit non-summoned foes => effect on summons (except allies summon for Osa) is divided by 2. 

            SpellException spellException = null;
            if (spellExceptions.ContainsKey(spell.LevelTemplate.id))
                spellException = spellExceptions[spell.LevelTemplate.id];
            if ((category & SpellCategory.DamagesNeutral) > 0)
                AdjustDamage(result, spellException != null ? spellException.MinNeutral : effect.diceNum, spellException != null ? spellException.MaxNeutral : effect.diceSide, SpellCategory.DamagesNeutral, chanceToHappen,
                    GetSafetotal(caster, Stats.PlayerField.NeutralDamageBonus) + GetSafetotal(caster, Stats.PlayerField.DamageBonus) + GetSafetotal(caster, Stats.PlayerField.PhysicalDamage),
                    GetSafetotal(caster, Stats.PlayerField.DamageBonusPercent) + GetSafetotal(caster, Stats.PlayerField.Strength),
                    target == null ? 0 : target.Stats.NeutralElementReduction,
                    target == null ? 0 : target.Stats.NeutralResistPercent, isFriend);

            if ((category & SpellCategory.DamagesFire) > 0)
                AdjustDamage(result, spellException != null ? spellException.MinFire : effect.diceNum, spellException != null ? spellException.MaxFire : effect.diceSide, SpellCategory.DamagesFire, chanceToHappen,
                    GetSafetotal(caster, Stats.PlayerField.FireDamageBonus) + GetSafetotal(caster, Stats.PlayerField.DamageBonus) + GetSafetotal(caster, Stats.PlayerField.MagicDamage),
                    GetSafetotal(caster, Stats.PlayerField.DamageBonusPercent) + GetSafetotal(caster, Stats.PlayerField.Intelligence),
                    target == null ? 0 : target.Stats.FireElementReduction,
                    target == null ? 0 : target.Stats.FireResistPercent, isFriend);

            if ((category & SpellCategory.DamagesAir) > 0)
                AdjustDamage(result, spellException != null ? spellException.MinAir : effect.diceNum, spellException != null ? spellException.MaxAir : effect.diceSide, SpellCategory.DamagesAir, chanceToHappen,
                    GetSafetotal(caster, Stats.PlayerField.AirDamageBonus) + GetSafetotal(caster, Stats.PlayerField.DamageBonus) + GetSafetotal(caster, Stats.PlayerField.MagicDamage),
                    GetSafetotal(caster, Stats.PlayerField.DamageBonusPercent) + GetSafetotal(caster, Stats.PlayerField.Agility),
                    target == null ? 0 : target.Stats.AirElementReduction,
                    target == null ? 0 : target.Stats.AirResistPercent, isFriend);

            if ((category & SpellCategory.DamagesWater) > 0)
                AdjustDamage(result, spellException != null ? spellException.MinWater : effect.diceNum, spellException != null ? spellException.MaxWater : effect.diceSide, SpellCategory.DamagesWater, chanceToHappen,
                    GetSafetotal(caster, Stats.PlayerField.WaterDamageBonus) + GetSafetotal(caster, Stats.PlayerField.DamageBonus) + GetSafetotal(caster, Stats.PlayerField.MagicDamage),
                    GetSafetotal(caster, Stats.PlayerField.DamageBonusPercent) + GetSafetotal(caster, Stats.PlayerField.Chance),
                    target == null ? 0 : target.Stats.WaterElementReduction,
                    target == null ? 0 : target.Stats.WaterResistPercent, isFriend);

            if ((category & SpellCategory.DamagesEarth) > 0)
                AdjustDamage(result, spellException != null ? spellException.MinEarth : effect.diceNum, spellException != null ? spellException.MaxEarth : effect.diceSide, SpellCategory.DamagesEarth, chanceToHappen,
                    GetSafetotal(caster, Stats.PlayerField.EarthDamageBonus) + GetSafetotal(caster, Stats.PlayerField.DamageBonus) + GetSafetotal(caster, Stats.PlayerField.MagicDamage),
                    GetSafetotal(caster, Stats.PlayerField.DamageBonusPercent) + GetSafetotal(caster, Stats.PlayerField.Strength),
                    target == null ? 0 : target.Stats.EarthElementReduction,
                    target == null ? 0 : target.Stats.EarthResistPercent, isFriend);

            if ((category & SpellCategory.Healing) > 0)
            {
                bool steal = (category & SpellCategory.Damages) > 0;
                if (steal) target = caster; // Probably hp steal
                uint hptoHeal = (uint)(Math.Max(0, target.Stats.MaxHealth - target.Stats.Health)); // Can't heal over max
                if (steal)
                {
                    result.MinHeal = -Math.Min(hptoHeal, Math.Abs(result.MinDamage));
                    result.MaxHeal = -Math.Min(hptoHeal, Math.Abs(result.MaxDamage));
                }
                else
                {
                    bool skip = false;
                    if (spell.Template.id == 140) // Mot de reconstruction => do only use it on purpose
                    {
                        if (hptoHeal < target.Stats.Health || hptoHeal < 400) skip = true; // Only heal targets with under 50% of health and at least 400 hp to heal
                    }
                    if (!skip && hptoHeal > 0)
                    {
                        AdjustDamage(result, Math.Min(effect.diceNum, hptoHeal), Math.Min(effect.diceSide, hptoHeal), SpellCategory.Healing, chanceToHappen,
                             GetSafetotal(caster, Stats.PlayerField.HealBonus),
                             GetSafetotal(caster, Stats.PlayerField.Intelligence),
                             0,
                             0, isFriend);
                        if (result.Heal > hptoHeal)
                            if (isFriend)
                                result.MinHeal = result.MaxHeal = -hptoHeal;
                            else
                                result.MinHeal = result.MaxHeal = hptoHeal;
                    }
                }
            }
            if ((category & SpellCategory.Buff) > 0)
                if (isFriend)
                    result.Boost -= spell.Level * chanceToHappen;
                else
                    result.Boost += spell.Level * chanceToHappen;

            if ((category & SpellCategory.Curse) > 0)
            {
                DamageType ratio = spell.Level * chanceToHappen;

                if (effect.effectId == (int)EffectsEnum.Effect_SkipTurn) // Let say this effect counts as 2 damage per level of the target
                    ratio = target.Level * 2 * chanceToHappen;

                if (isFriend)
                    result.Curse -= 2 * ratio;
                else
                    result.Curse += ratio;
            }
            if (isFriend)
                result.Add(result); // amplify (double) effects on friends. 


            if (!isFriend && ((category & SpellCategory.Damages) > 0) && result.MinDamage > target.Stats.Health) // Enough damage to kill the target => affect an arbitrary 50% of max heal (with at least current health), so strong spells are not favored anymore. 
            {
                double ratio = Math.Max(target.Stats.MaxHealth / 2, target.Stats.Health) / result.MinDamage;
                result.Multiply(ratio);
            }

            if (spell.Template.id == 114) // Rekop
            {
                if (target.Stats.Health < 1000)
                    result.Multiply(0.1);
                else
                    if (target.Stats.Health < 2000)
                        result.Multiply(0.6);
            }

            // Damage reflection
            if (((category & SpellCategory.Damages) > 0) && result.Damage > 0 && !isFriend)
            {
                DamageType reflected = spell.GetDamageReflection(target);
                if (reflected > 0)
                {
                    if (reflected >= spellImpact.Damage) return 0; // Reflect all damages
                    result.MinHeal += reflected * 2;
                    result.MaxHeal += reflected * 2;
                }
            }

            if (spell.Template.id == 0 && (category & SpellCategory.Damages) > 0) // Weapon => consider effect of "maîtrise"
            {
                Weapon weapon = caster.Character.Inventory.GetEquippedWeapon();
                if (weapon != null)
                    foreach(var boost in caster.GetBoostWeaponDamagesEffects())
                        if (boost.weaponTypeId == weapon.typeId)
                            result.Multiply(1.0+boost.delta/100.0);
            }

            if (spellImpact != null)
                spellImpact.Add(result);
            else
                spellImpact = result;
            return result.Damage;
        }        
        #endregion Effects
        
        public SpellImpact GetSpellDamages(PlayedFighter caster, Fighter target/*, Spell.SpellCategory categories*/)
        {
            SpellImpact damages = null;
            foreach (var effect in LevelTemplate.effects)
                CumulEffects(effect, ref damages, caster, target/*, categories*/, this);
            return damages;
        }

        public DamageType GetTotalDamageOnAllEnemies(PlayedFighter caster)
        {
            DamageType damages = 0;
            foreach (Fighter enemy in caster.Team == null ? caster.Fight.AliveActors : caster.GetOpposedTeam().FightersAlive)
            {
                SpellImpact impact = null;
                foreach (var effect in LevelTemplate.effects)
                    damages += CumulEffects(effect, ref impact, caster, enemy/*, Spell.SpellCategory.Damages*/, this);
            }
            return damages;
        }

        // Fast function that says how big is the area covered by effects from a given category
        public uint GetArea(SpellCategory? category = SpellCategory.Damages)
        {
            uint area = 0;
            foreach (var effect in GetEffects())
                if ((Spell.GetEffectCategories((uint)effect.Id, LevelTemplate.id) & category) > 0)
                    area = Math.Max(area, effect.Surface);
            return area;
        }

        // Precise compute efficiency of a spell for a given category (beware to remove pc from friend and friendcells lists before calling this !)
        // Returns -1 if it would hit friends (todo : optimize if needed)
        public int GetFullAreaEffect(PlayedFighter pc, Cell source, Cell dest, IEnumerable<Fighter> actors, Spell.SpellCategory category, ref string comment)
        {
            SpellImpact spellImpact = new SpellImpact();

            foreach (EffectInstanceDice effect in LevelTemplate.effects)
                if ((Spell.GetEffectCategories((uint)effect.effectId, LevelTemplate.id) & category) > 0)
                {
                    comment += " Effect " + (EffectsEnum)(effect.effectId) + " : ";
                    EffectDice effectCl = new EffectDice(effect);
                    IEnumerable<Cell> cells = effectCl.GetArea(source, dest);
                    SpellTargetType targetType = (SpellTargetType)effect.targetId;
                    int nbAffectedTargets = 0;
                    if (EffectBase.canAffectTarget(effectCl, this, pc, pc) && cells.Contains(source))
                    {
                        // Caster would be affected
                        DamageType efficiency = Spell.CumulEffects(effect, ref spellImpact, pc, pc/*, category*/, this);
                        if (efficiency != 0)
                            comment += string.Format("{0} on {1} => {2}, ", (decimal)efficiency, pc, (decimal)spellImpact.Damage);
                        nbAffectedTargets++;
                        if (efficiency < 0) return 0; // The caster would be affected by a bad spell => give up     
                    }

                    foreach (var actor in actors.Where(act => cells.Contains(act.Cell))) // All actors within the area covered by the spell
                    {
                        if (!EffectBase.canAffectTarget(effectCl, this, pc, actor)) continue; // This actor is not affected by the spell
                        DamageType damage = Spell.CumulEffects(effect, ref spellImpact, pc, actor/*, category*/, this);
                        if (damage != 0)
                            comment += string.Format(" - {0} on {1} => {2}", (decimal)damage, actor, (decimal)spellImpact.Damage);

                        nbAffectedTargets++;
                        //if (damage > 0 && actor.Team == pc.Team) return 0; // Harmful on a friend => give up                        
                    }
                                        
                    //if (nbAffectedTargets > 1)
                    //{
                    //    pc.Character.SendWarning("Spell {0} : {1} targets affected for {2} damage - {3}", this, nbAffectedTargets, spellImpact.Damage, comment);
                    //}
                }

            if (Template.id == 139) // Mot d'altruisme : only use near end of fight or if lot of spellImpact to heal
            {
                int hpLeftOnFoes = actors.Where(actor => actor.Team.Id != pc.Team.Id).Sum(actor => actor.Stats.Health);
                comment += string.Format(" - special \"Mot d'altruisme\" processing : hpLeftOnFoes = {0}, efficiency = {1}", hpLeftOnFoes, (int)spellImpact.Damage);
                if (hpLeftOnFoes > 500) // Not the end of the fight
                    if (spellImpact.Damage < 300) return 0; // Do not cast it if less than 300 hp of healing
                    else
                        return (int)spellImpact.Damage / 3; // Otherwise, far from the end of the fight, divide efficiency by 3                
                // if close to the end of the fight, then returns full result. 
            }
            
            return (int)spellImpact.Damage;
        }

        // Find the best cell to cast a given spell, retrieve damage done and best cell (todo : optimize if needed)
        public SpellTarget FindBestTarget(PlayedFighter pc, Cell source, IEnumerable<Cell> destCells, IEnumerable<Fighter> actors, Spell.SpellCategory category)
        {
            SpellTarget result = null;
            foreach (Cell dest in destCells)
            {
                string comment = string.Empty;
                int efficientcy = 0;
                if (areaDependsOnDirection || !efficiencyCache.TryGetValue(dest.Id, out efficientcy))
                {
                    efficientcy = GetFullAreaEffect(pc, source, dest, actors, category, ref comment);
                    if (!areaDependsOnDirection) efficiencyCache[dest.Id] = efficientcy;
                }
                if (efficientcy > 0)
                {
                    if (result == null)
                    {
                        result = new SpellTarget(efficientcy, source, dest, this);
                        result.Comment = comment;
                    }
                    else
                        if (efficientcy > result.Efficiency)
                        {
                            result.Efficiency = efficientcy;
                            result.FromCell = source;
                            result.TargetCell = dest;
                            result.Spell = this;
                            result.Comment = comment;
                        }
                }
            }
            return result;
        }

        /// <summary>
        /// Find all cells where a spell can be cast, from casterCell position
        /// Todo : takes traps into account 
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="casterCell"></param>
        /// <param name="actors"></param>
        /// <returns></returns>
        private IEnumerable<Cell> GetCellsAtSpellRange(PlayedFighter caster, Cell casterCell, IEnumerable<Fighter> actors)
        {
            int maxRange = caster.GetRealSpellRange(LevelTemplate);
            if (LevelTemplate.castTestLos)
                _losMap.UpdateTargetCell(casterCell, true, false);

            Func<Cell, bool> filter = cell =>
            {
                if (LevelTemplate.castInLine || LevelTemplate.castInDiagonal)
                    if (!(LevelTemplate.castInLine && (casterCell.X == cell.X || casterCell.Y == cell.Y) || LevelTemplate.castInDiagonal && (Math.Abs(casterCell.X - cell.X) == Math.Abs(casterCell.Y - cell.Y)))) return false;
                if (LevelTemplate.castTestLos)
                    if (!_losMap[cell]) return false;
                if (LevelTemplate.needFreeCell)
                    if (!(cell.Walkable && !cell.NonWalkableDuringFight && cell != casterCell && !actors.Any(actor => actor.Cell != null && actor.Cell.Id == cell.Id))) return false;
                if (LevelTemplate.needTakenCell)
                    if (!(cell == casterCell || actors.Any(actor => actor.Cell != null && actor.Cell.Id == cell.Id))) return false;
                if (LevelTemplate.needFreeTrapCell)
                    return false; // Do not play traps yet   
                int? targetId = caster.Fight.GetActorsOnCell(cell).Select(actor => (int?)actor.Id).FirstOrDefault();
                if (targetId != null)
                    if (!IsAvailable(targetId)) return false;
                return true;
            };
            return casterCell.GetAllCellsInRange((int)LevelTemplate.minRange, maxRange, false, filter);
        }

        private LOSMap _losMap;
        private Dictionary<short, int> efficiencyCache = null;
            
        // Find where the PC should come to cast the spell, and the best target there (todo : optimize if needed)
        public SpellTarget FindBestTarget(PlayedFighter pc, IEnumerable<Cell> sourceCells, IEnumerable<Fighter> actors, Spell.SpellCategory category)
        {
            if (LevelTemplate.statesForbidden != null)
                if (LevelTemplate.statesForbidden.Any(state => pc.HasState(state)))
                {
                    pc.Character.SendWarning("Spell {0} skipped : statesForbidden {1}", this, string.Join(",", LevelTemplate.statesForbidden));
                    return null; // Can't cast this : all the required states are not on the caster
                }
            if (LevelTemplate.statesRequired != null)
                if (!LevelTemplate.statesRequired.All(state => pc.HasState(state)))
                {
                    pc.Character.SendWarning("Spell {0} skipped : statesRequired {1}", this, string.Join(",", LevelTemplate.statesForbidden));
                    return null; // Can't cast this : at least one required state is not on the caster
                }            
            if (IsMaitrise(null)) // If the spell is a maitrise, then ignore it if not of the proper type for equipped weapon. 
            {
                Weapon weapon = pc.Character.Inventory.GetEquippedWeapon();
                if (weapon == null) return null;
                if (!IsMaitrise(weapon.typeId))
                    return null;
                }
            _losMap = new LOSMap(pc.Fight);
            SpellTarget bestResult = null;
            #region optimisations
            IEnumerable<Fighter> enemies = pc.GetOpposedTeam().FightersAlive;
            IEnumerable<Fighter> friends = pc.Team.FightersAlive;
            
            bool goodSpell = (Categories & (SpellCategory.Buff | SpellCategory.Healing)) != 0;
            bool badSpell = (Categories & (SpellCategory.Damages | SpellCategory.Curse)) != 0;
            IEnumerable<Fighter> targets = null;
            int targetsCount = 0;
            if (goodSpell && badSpell)
            {
                goodSpell = badSpell = false;
            }
            else
            {
                targets = goodSpell ? friends : enemies;
                targetsCount = targets.Count();
            }
            uint surface = this.GetArea(category);
            efficiencyCache = null;
            if (!areaDependsOnDirection) efficiencyCache = new Dictionary<short, int>();
            #endregion
            if (surface == 1 && LevelTemplate.range == 0) // Hack fast Cure and protect self
            {
                var res = GetSpellDamages(pc, pc);
                if (res.Damage > 0)
                    bestResult = new SpellTarget(res.Damage, pc.Cell, pc.Cell, this);
            }
            else                
                foreach (Cell source in sourceCells)
                {
                    IEnumerable<Cell> destCells = GetCellsAtSpellRange(pc, source, actors);
                    if (goodSpell || badSpell)
                        if (surface <= 1 && LevelTemplate.range > 0)
                            destCells = destCells.Intersect(targets.Select(fighter => fighter.Cell)); // for spells that have an area of effect of 1, just find enemies or friends as targets. No need to scan all the range.                    
                    if (surface >= 560 && destCells.Count() > 1) // For spells that cover the full map, use only the first cell
                        destCells = destCells.Take(1);
                    SpellTarget newResult = FindBestTarget(pc, source, destCells, actors, category);
                    if (newResult == null || newResult.Efficiency <= 0) continue;
                    if (bestResult == null || bestResult.Efficiency < newResult.Efficiency)
                    {
                        bestResult = newResult;
                        if (surface >= 560) break; // if spell covers all map, and we have some hit, then no need to continue (first source cells are nearest)
                        if (targetsCount == 1 && surface == 1) break; // only one target and 1 cell area spell => no need to loop further
                    }
                }
            if (bestResult != null)
            {
                bestResult.Efficiency *= (pc.Stats.CurrentAP / LevelTemplate.apCost);
                bestResult.cast = false;
            }

            return bestResult;
        }


    }
}
