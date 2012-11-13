#region License GNU GPL
// PlayerStats.cs
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
#endregion
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Core.Collections;
using BiM.Protocol.Enums;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Stats
{
    public class PlayerStats : INotifyPropertyChanged, IMinimalStats
    {
        public PlayerStats()
        {
            Fields = new Dictionary<PlayerField, StatsRow>
                         {
                             {PlayerField.Initiative, new StatsRow(PlayerField.Initiative, x => OnPropertyChanged("Initiative"))},
                             {PlayerField.Prospecting, new StatsRow(PlayerField.Prospecting)},
                             {
                                 PlayerField.AP, new StatsRow(PlayerField.AP, x =>
                                                                                  {
                                                                                      OnPropertyChanged("MaxAP");
                                                                                      OnPropertyChanged("AP");
                                                                                  })
                                 },
                             {
                                 PlayerField.MP, new StatsRow(PlayerField.MP, x =>
                                                                                  {
                                                                                      OnPropertyChanged("MaxMP");
                                                                                      OnPropertyChanged("MP");
                                                                                  })
                                 },
                             {PlayerField.Strength, new StatsRow(PlayerField.Strength, x => OnPropertyChanged("Strength"))},
                             {PlayerField.Vitality, new StatsRow(PlayerField.Vitality, x => OnPropertyChanged("Vitality"))},
                             {PlayerField.Wisdom, new StatsRow(PlayerField.Wisdom, x => OnPropertyChanged("Wisdom"))},
                             {PlayerField.Chance, new StatsRow(PlayerField.Chance, x => OnPropertyChanged("Chance"))},
                             {PlayerField.Agility, new StatsRow(PlayerField.Agility, x => OnPropertyChanged("Agility"))},
                             {PlayerField.Intelligence, new StatsRow(PlayerField.Intelligence, x => OnPropertyChanged("Intelligence"))},
                             {PlayerField.Range, new StatsRow(PlayerField.Range, x => OnPropertyChanged("Range"))},
                             {PlayerField.SummonLimit, new StatsRow(PlayerField.SummonLimit)},
                             {PlayerField.DamageReflection, new StatsRow(PlayerField.DamageReflection)},
                             {PlayerField.CriticalHit, new StatsRow(PlayerField.CriticalHit)},
                             {PlayerField.CriticalMiss, new StatsRow(PlayerField.CriticalMiss)},
                             {PlayerField.HealBonus, new StatsRow(PlayerField.HealBonus)},
                             {PlayerField.DamageBonus, new StatsRow(PlayerField.DamageBonus)},
                             {PlayerField.WeaponDamageBonusPercent, new StatsRow(PlayerField.WeaponDamageBonusPercent)},
                             {PlayerField.DamageBonusPercent, new StatsRow(PlayerField.DamageBonusPercent)},
                             {PlayerField.TrapBonus, new StatsRow(PlayerField.TrapBonus)},
                             {PlayerField.TrapBonusPercent, new StatsRow(PlayerField.TrapBonusPercent)},
                             {PlayerField.PermanentDamagePercent, new StatsRow(PlayerField.PermanentDamagePercent, x => OnPropertyChanged("PermanentDamagePercent"))},
                             {PlayerField.TackleBlock, new StatsRow(PlayerField.TackleBlock, x => OnPropertyChanged("TackleBlock"))},
                             {PlayerField.TackleEvade, new StatsRow(PlayerField.TackleEvade, x => OnPropertyChanged("TackleEvade"))},
                             {PlayerField.APAttack, new StatsRow(PlayerField.APAttack)},
                             {PlayerField.MPAttack, new StatsRow(PlayerField.MPAttack)},
                             {PlayerField.PushDamageBonus, new StatsRow(PlayerField.PushDamageBonus)},
                             {PlayerField.CriticalDamageBonus, new StatsRow(PlayerField.CriticalDamageBonus)},
                             {PlayerField.NeutralDamageBonus, new StatsRow(PlayerField.NeutralDamageBonus)},
                             {PlayerField.EarthDamageBonus, new StatsRow(PlayerField.EarthDamageBonus)},
                             {PlayerField.WaterDamageBonus, new StatsRow(PlayerField.WaterDamageBonus)},
                             {PlayerField.AirDamageBonus, new StatsRow(PlayerField.AirDamageBonus)},
                             {PlayerField.FireDamageBonus, new StatsRow(PlayerField.FireDamageBonus)},
                             {PlayerField.DodgeAPProbability, new StatsRow(PlayerField.DodgeAPProbability, x => OnPropertyChanged("DodgeAPProbability"))},
                             {PlayerField.DodgeMPProbability, new StatsRow(PlayerField.DodgeMPProbability, x => OnPropertyChanged("DodgeMPProbability"))},
                             {PlayerField.NeutralResistPercent, new StatsRow(PlayerField.NeutralResistPercent, x => OnPropertyChanged("NeutralResistPercent"))},
                             {PlayerField.EarthResistPercent, new StatsRow(PlayerField.EarthResistPercent, x => OnPropertyChanged("EarthResistPercent"))},
                             {PlayerField.WaterResistPercent, new StatsRow(PlayerField.WaterResistPercent, x => OnPropertyChanged("WaterResistPercent"))},
                             {PlayerField.AirResistPercent, new StatsRow(PlayerField.AirResistPercent, x => OnPropertyChanged("AirResistPercent"))},
                             {PlayerField.FireResistPercent, new StatsRow(PlayerField.FireResistPercent, x => OnPropertyChanged("FireResistPercent"))},
                             {PlayerField.NeutralElementReduction, new StatsRow(PlayerField.NeutralElementReduction, x => OnPropertyChanged("NeutralElementReduction"))},
                             {PlayerField.EarthElementReduction, new StatsRow(PlayerField.EarthElementReduction, x => OnPropertyChanged("EarthElementReduction"))},
                             {PlayerField.WaterElementReduction, new StatsRow(PlayerField.WaterElementReduction, x => OnPropertyChanged("WaterElementReduction"))},
                             {PlayerField.AirElementReduction, new StatsRow(PlayerField.AirElementReduction, x => OnPropertyChanged("AirElementReduction"))},
                             {PlayerField.FireElementReduction, new StatsRow(PlayerField.FireElementReduction, x => OnPropertyChanged("FireElementReduction"))},
                             {PlayerField.PushDamageReduction, new StatsRow(PlayerField.PushDamageReduction)},
                             {PlayerField.CriticalDamageReduction, new StatsRow(PlayerField.CriticalDamageReduction)},
                             {PlayerField.PvpNeutralResistPercent, new StatsRow(PlayerField.PvpNeutralResistPercent, x => OnPropertyChanged("NeutralResistPercent"))},
                             {PlayerField.PvpEarthResistPercent, new StatsRow(PlayerField.PvpEarthResistPercent, x => OnPropertyChanged("EarthResistPercent"))},
                             {PlayerField.PvpWaterResistPercent, new StatsRow(PlayerField.PvpWaterResistPercent, x => OnPropertyChanged("WaterResistPercent"))},
                             {PlayerField.PvpAirResistPercent, new StatsRow(PlayerField.PvpAirResistPercent, x => OnPropertyChanged("AirResistPercent"))},
                             {PlayerField.PvpFireResistPercent, new StatsRow(PlayerField.PvpFireResistPercent, x => OnPropertyChanged("FireResistPercent"))},
                             {PlayerField.PvpNeutralElementReduction, new StatsRow(PlayerField.PvpNeutralElementReduction, x => OnPropertyChanged("NeutralElementReduction"))},
                             {PlayerField.PvpEarthElementReduction, new StatsRow(PlayerField.PvpEarthElementReduction, x => OnPropertyChanged("EarthElementReduction"))},
                             {PlayerField.PvpWaterElementReduction, new StatsRow(PlayerField.PvpWaterElementReduction, x => OnPropertyChanged("WaterElementReduction"))},
                             {PlayerField.PvpAirElementReduction, new StatsRow(PlayerField.PvpAirElementReduction, x => OnPropertyChanged("AirElementReduction"))},
                             {PlayerField.PvpFireElementReduction, new StatsRow(PlayerField.PvpFireElementReduction, x => OnPropertyChanged("FireElementReduction"))},
                         };
            SpellsModifications = new ObservableCollectionMT<SpellModification>();
            InvisibilityState = GameActionFightInvisibilityStateEnum.VISIBLE;
        }

        public PlayerStats(PlayedCharacter owner, CharacterCharacteristicsInformations stats)
            : this()
        {
            if (stats == null) throw new ArgumentNullException("stats");
            Update(stats);
        }

        public PlayerStats(PlayedCharacter owner)
            : this()
        {
            if (owner == null) throw new ArgumentNullException("owner");
            Owner = owner;
        }

        public bool PvP
        {
            get;
            set;
        }

        public PlayedCharacter Owner
        {
            get;
            set;
        }

        public ObservableCollectionMT<SpellModification> SpellsModifications
        {
            get;
            set;
        }

        public Dictionary<PlayerField, StatsRow> Fields
        {
            get;
            set;
        }

        public double Experience
        {
            get;
            set;
        }

        public double ExperienceLevelFloor
        {
            get;
            set;
        }

        public double ExperienceNextLevelFloor
        {
            get;
            set;
        }

        public int SpellsPoints
        {
            get;
            set;
        }

        public int StatsPoints
        {
            get;
            set;
        }

        public int Kamas
        {
            get;
            set;
        }

        public short EnergyPoints
        {
            get;
            set;
        }

        public short MaxEnergyPoints
        {
            get;
            set;
        }

        public StatsRow AP
        {
            get { return this[PlayerField.AP]; }
        }

        public StatsRow MP
        {
            get { return this[PlayerField.MP]; }
        }

        public StatsRow Vitality
        {
            get { return this[PlayerField.Vitality]; }
        }

        public StatsRow Strength
        {
            get { return this[PlayerField.Strength]; }
        }

        public StatsRow Wisdom
        {
            get { return this[PlayerField.Wisdom]; }
        }

        public StatsRow Chance
        {
            get { return this[PlayerField.Chance]; }
        }

        public StatsRow Agility
        {
            get { return this[PlayerField.Agility]; }
        }

        public StatsRow Intelligence
        {
            get { return this[PlayerField.Intelligence]; }
        }

        public StatsRow this[PlayerField name]
        {
            get
            {
                StatsRow value;
                return Fields.TryGetValue(name, out value) ? value : null;
            }
            set
            {
                if (!Fields.ContainsKey(name))
                    Fields.Add(name, value);
                else
                    Fields[name] = value;
            }
        }

        public GameActionFightInvisibilityStateEnum InvisibilityState
        {
            get;
            set;
        }

        #region IMinimalStats Members

        public int Initiative
        {
            get { return this[PlayerField.Initiative].Total; }
        }

        public int Health
        {
            get;
            set;
        }

        public int MaxHealth
        {
            get;
            set;
        }

        public int MaxHealthBase
        {
            get;
            set;
        }

        public int MaxAP
        {
            get { return this[PlayerField.AP].Total; }
        }

        public int MaxMP
        {
            get { return this[PlayerField.MP].Total; }
        }

        public int Range
        {
            get { return this[PlayerField.Range].Total; }
        }

        public int PermanentDamagePercent
        {
            get { return this[PlayerField.PermanentDamagePercent].Total; }
        }

        public int TackleBlock
        {
            get { return this[PlayerField.TackleBlock].Total; }
        }

        public int TackleEvade
        {
            get { return this[PlayerField.TackleEvade].Total; }
        }

        public int DodgeAPProbability
        {
            get { return this[PlayerField.DodgeAPProbability].Total; }
        }

        public int DodgeMPProbability
        {
            get { return this[PlayerField.DodgeMPProbability].Total; }
        }

        public int NeutralResistPercent
        {
            get { return this[PvP ? PlayerField.PvpNeutralResistPercent : PlayerField.NeutralResistPercent].Total; }
        }

        public int EarthResistPercent
        {
            get { return this[PvP ? PlayerField.PvpEarthResistPercent : PlayerField.EarthResistPercent].Total; }
        }

        public int WaterResistPercent
        {
            get { return this[PvP ? PlayerField.PvpWaterResistPercent : PlayerField.WaterResistPercent].Total; }
        }

        public int AirResistPercent
        {
            get { return this[PvP ? PlayerField.PvpAirResistPercent : PlayerField.AirResistPercent].Total; }
        }

        public int FireResistPercent
        {
            get { return this[PvP ? PlayerField.PvpFireResistPercent : PlayerField.FireResistPercent].Total; }
        }

        public int NeutralElementReduction
        {
            get { return this[PvP ? PlayerField.PvpNeutralElementReduction : PlayerField.NeutralElementReduction].Total; }
        }

        public int EarthElementReduction
        {
            get { return this[PvP ? PlayerField.PvpEarthElementReduction : PlayerField.EarthElementReduction].Total; }
        }

        public int WaterElementReduction
        {
            get { return this[PvP ? PlayerField.PvpWaterElementReduction : PlayerField.WaterElementReduction].Total; }
        }

        public int AirElementReduction
        {
            get { return this[PvP ? PlayerField.PvpAirElementReduction : PlayerField.AirElementReduction].Total; }
        }

        public int FireElementReduction
        {
            get { return this[PvP ? PlayerField.PvpFireElementReduction : PlayerField.FireElementReduction].Total; }
        }


        public int CurrentAP
        {
            get;
            set;
        }

        public int CurrentMP
        {
            get;
            set;
        }

        public void Update(GameFightMinimalStats stats)
        {
            if (stats == null) throw new ArgumentNullException("stats");

            Health = stats.lifePoints;
            MaxHealth = stats.maxLifePoints;
            MaxHealthBase = stats.baseMaxLifePoints;
            InvisibilityState = (GameActionFightInvisibilityStateEnum) stats.invisibilityState;


            this[PlayerField.AP].Update(stats.actionPoints);
            this[PlayerField.MP].Update(stats.movementPoints);

            this[PlayerField.PermanentDamagePercent].Update(stats.permanentDamagePercent);
            this[PlayerField.TackleBlock].Update(stats.tackleBlock);
            this[PlayerField.TackleEvade].Update(stats.tackleEvade);
            this[PlayerField.DodgeAPProbability].Update(stats.dodgePALostProbability);
            this[PlayerField.DodgeMPProbability].Update(stats.dodgePMLostProbability);
            this[PlayerField.NeutralResistPercent].Update(stats.neutralElementResistPercent);
            this[PlayerField.EarthResistPercent].Update(stats.earthElementResistPercent);
            this[PlayerField.WaterResistPercent].Update(stats.waterElementResistPercent);
            this[PlayerField.AirResistPercent].Update(stats.airElementResistPercent);
            this[PlayerField.FireResistPercent].Update(stats.fireElementResistPercent);
            this[PlayerField.NeutralElementReduction].Update(stats.neutralElementReduction);
            this[PlayerField.EarthElementReduction].Update(stats.earthElementReduction);
            this[PlayerField.WaterElementReduction].Update(stats.waterElementReduction);
            this[PlayerField.AirElementReduction].Update(stats.airElementReduction);
            this[PlayerField.FireElementReduction].Update(stats.fireElementReduction);

            OnPropertyChanged(Binding.IndexerName);
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void Update(CharacterCharacteristicsInformations stats)
        {
            if (stats == null) throw new ArgumentNullException("stats");
            Experience = stats.experience;
            ExperienceLevelFloor = stats.experienceLevelFloor;
            ExperienceNextLevelFloor = stats.experienceNextLevelFloor;

            Kamas = stats.kamas;
            StatsPoints = stats.statsPoints;
            SpellsPoints = stats.spellsPoints;
            Health = stats.lifePoints;
            MaxHealth = stats.maxLifePoints;

            EnergyPoints = stats.energyPoints;
            MaxEnergyPoints = stats.maxEnergyPoints;
            CurrentAP = stats.actionPointsCurrent;
            CurrentMP = stats.movementPointsCurrent;

            this[PlayerField.Initiative].Update(stats.initiative);
            this[PlayerField.Prospecting].Update(stats.prospecting);
            this[PlayerField.AP].Update(stats.actionPoints);
            this[PlayerField.MP].Update(stats.movementPoints);
            this[PlayerField.Strength].Update(stats.strength);
            this[PlayerField.Vitality].Update(stats.vitality);
            this[PlayerField.Wisdom].Update(stats.wisdom);
            this[PlayerField.Chance].Update(stats.chance);
            this[PlayerField.Agility].Update(stats.agility);
            this[PlayerField.Intelligence].Update(stats.intelligence);
            this[PlayerField.Range].Update(stats.range);
            this[PlayerField.SummonLimit].Update(stats.summonableCreaturesBoost);
            this[PlayerField.DamageReflection].Update(stats.reflect);
            this[PlayerField.CriticalHit].Update(stats.criticalHit);
            this[PlayerField.CriticalMiss].Update(stats.criticalMiss);
            this[PlayerField.HealBonus].Update(stats.healBonus);
            this[PlayerField.DamageBonus].Update(stats.allDamagesBonus);
            this[PlayerField.WeaponDamageBonusPercent].Update(stats.weaponDamagesBonusPercent);
            this[PlayerField.DamageBonusPercent].Update(stats.damagesBonusPercent);
            this[PlayerField.TrapBonus].Update(stats.trapBonus);
            this[PlayerField.TrapBonusPercent].Update(stats.trapBonusPercent);
            this[PlayerField.PermanentDamagePercent].Update(stats.permanentDamagePercent);
            this[PlayerField.TackleBlock].Update(stats.tackleBlock);
            this[PlayerField.TackleEvade].Update(stats.tackleEvade);
            this[PlayerField.APAttack].Update(stats.PAAttack);
            this[PlayerField.MPAttack].Update(stats.PMAttack);
            this[PlayerField.PushDamageBonus].Update(stats.pushDamageBonus);
            this[PlayerField.CriticalDamageBonus].Update(stats.criticalDamageBonus);
            this[PlayerField.NeutralDamageBonus].Update(stats.neutralDamageBonus);
            this[PlayerField.EarthDamageBonus].Update(stats.earthDamageBonus);
            this[PlayerField.WaterDamageBonus].Update(stats.waterDamageBonus);
            this[PlayerField.AirDamageBonus].Update(stats.airDamageBonus);
            this[PlayerField.FireDamageBonus].Update(stats.fireDamageBonus);
            this[PlayerField.DodgeAPProbability].Update(stats.dodgePALostProbability);
            this[PlayerField.DodgeMPProbability].Update(stats.dodgePMLostProbability);
            this[PlayerField.NeutralResistPercent].Update(stats.neutralElementResistPercent);
            this[PlayerField.EarthResistPercent].Update(stats.earthElementResistPercent);
            this[PlayerField.WaterResistPercent].Update(stats.waterElementResistPercent);
            this[PlayerField.AirResistPercent].Update(stats.airElementResistPercent);
            this[PlayerField.FireResistPercent].Update(stats.fireElementResistPercent);
            this[PlayerField.NeutralElementReduction].Update(stats.neutralElementReduction);
            this[PlayerField.EarthElementReduction].Update(stats.earthElementReduction);
            this[PlayerField.WaterElementReduction].Update(stats.waterElementReduction);
            this[PlayerField.AirElementReduction].Update(stats.airElementReduction);
            this[PlayerField.FireElementReduction].Update(stats.fireElementReduction);
            this[PlayerField.PushDamageReduction].Update(stats.pushDamageReduction);
            this[PlayerField.CriticalDamageReduction].Update(stats.criticalDamageReduction);
            this[PlayerField.PvpNeutralResistPercent].Update(stats.pvpNeutralElementResistPercent);
            this[PlayerField.PvpEarthResistPercent].Update(stats.pvpEarthElementResistPercent);
            this[PlayerField.PvpWaterResistPercent].Update(stats.pvpWaterElementResistPercent);
            this[PlayerField.PvpAirResistPercent].Update(stats.pvpAirElementResistPercent);
            this[PlayerField.PvpFireResistPercent].Update(stats.pvpFireElementResistPercent);
            this[PlayerField.PvpNeutralElementReduction].Update(stats.pvpNeutralElementReduction);
            this[PlayerField.PvpEarthElementReduction].Update(stats.pvpEarthElementReduction);
            this[PlayerField.PvpWaterElementReduction].Update(stats.pvpWaterElementReduction);
            this[PlayerField.AirElementReduction].Update(stats.pvpAirElementReduction);
            this[PlayerField.PvpFireElementReduction].Update(stats.pvpFireElementReduction);

            SpellsModifications.Clear();
            foreach (SpellModification spell in stats.spellModifications.Select(entry => new SpellModification(entry)))
            {
                SpellsModifications.Add(spell);
            }

            OnPropertyChanged(Binding.IndexerName);
        }


        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
        }
    }
}