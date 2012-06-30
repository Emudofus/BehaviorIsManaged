using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BiM.Protocol.Types;

namespace BiM.Game.Stats
{
    public class Stats : INotifyPropertyChanged
    {
        public Stats()
        {
            Fields = new Dictionary<PlayerField, StatsRow>();
            SpellsModifications = new ObservableCollection<SpellModification>();
        }

        public Stats(CharacterCharacteristicsInformations stats)
        {
            if (stats == null) throw new ArgumentNullException("stats");
            Fields = new Dictionary<PlayerField, StatsRow>();
            SpellsModifications = new ObservableCollection<SpellModification>();
            Update(stats);
        }

        public ObservableCollection<SpellModification> SpellsModifications
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

        public short CurrentAP
        {
            get;
            set;
        }

        public short CurrentMP
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

        public void Update(CharacterCharacteristicsInformations stats)
        {
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

            this[PlayerField.Initiative] = new StatsRow(stats.initiative, PlayerField.Initiative);
            this[PlayerField.Prospecting] = new StatsRow(stats.prospecting, PlayerField.Prospecting);
            this[PlayerField.AP] = new StatsRow(stats.actionPoints, PlayerField.AP);
            this[PlayerField.MP] = new StatsRow(stats.movementPoints, PlayerField.MP);
            this[PlayerField.Strength] = new StatsRow(stats.strength, PlayerField.Strength);
            this[PlayerField.Vitality] = new StatsRow(stats.vitality, PlayerField.Vitality);
            this[PlayerField.Wisdom] = new StatsRow(stats.wisdom, PlayerField.Wisdom);
            this[PlayerField.Chance] = new StatsRow(stats.chance, PlayerField.Chance);
            this[PlayerField.Agility] = new StatsRow(stats.agility, PlayerField.Agility);
            this[PlayerField.Intelligence] = new StatsRow(stats.intelligence, PlayerField.Intelligence);
            this[PlayerField.Range] = new StatsRow(stats.range, PlayerField.Range);
            this[PlayerField.SummonLimit] = new StatsRow(stats.summonableCreaturesBoost, PlayerField.SummonLimit);
            this[PlayerField.DamageReflection] = new StatsRow(stats.reflect, PlayerField.DamageReflection);
            this[PlayerField.CriticalHit] = new StatsRow(stats.criticalHit, PlayerField.CriticalHit);
            this[PlayerField.CriticalMiss] = new StatsRow(stats.criticalMiss, PlayerField.CriticalMiss);
            this[PlayerField.HealBonus] = new StatsRow(stats.healBonus, PlayerField.HealBonus);
            this[PlayerField.DamageBonus] = new StatsRow(stats.allDamagesBonus, PlayerField.DamageBonus);
            this[PlayerField.WeaponDamageBonusPercent] = new StatsRow(stats.weaponDamagesBonusPercent, PlayerField.WeaponDamageBonusPercent);
            this[PlayerField.DamageBonusPercent] = new StatsRow(stats.damagesBonusPercent, PlayerField.DamageBonusPercent);
            this[PlayerField.TrapBonus] = new StatsRow(stats.trapBonus, PlayerField.TrapBonus);
            this[PlayerField.TrapBonusPercent] = new StatsRow(stats.trapBonusPercent, PlayerField.TrapBonusPercent);
            this[PlayerField.PermanentDamagePercent] = new StatsRow(stats.permanentDamagePercent, PlayerField.PermanentDamagePercent);
            this[PlayerField.TackleBlock] = new StatsRow(stats.tackleBlock, PlayerField.TackleBlock);
            this[PlayerField.TackleEvade] = new StatsRow(stats.tackleEvade, PlayerField.TackleEvade);
            this[PlayerField.APAttack] = new StatsRow(stats.PAAttack, PlayerField.APAttack);
            this[PlayerField.MPAttack] = new StatsRow(stats.PMAttack, PlayerField.MPAttack);
            this[PlayerField.PushDamageBonus] = new StatsRow(stats.pushDamageBonus, PlayerField.PushDamageBonus);
            this[PlayerField.CriticalDamageBonus] = new StatsRow(stats.criticalDamageBonus, PlayerField.CriticalDamageBonus);
            this[PlayerField.NeutralDamageBonus] = new StatsRow(stats.neutralDamageBonus, PlayerField.NeutralDamageBonus);
            this[PlayerField.EarthDamageBonus] = new StatsRow(stats.earthDamageBonus, PlayerField.EarthDamageBonus);
            this[PlayerField.WaterDamageBonus] = new StatsRow(stats.waterDamageBonus, PlayerField.WaterDamageBonus);
            this[PlayerField.AirDamageBonus] = new StatsRow(stats.airDamageBonus, PlayerField.AirDamageBonus);
            this[PlayerField.FireDamageBonus] = new StatsRow(stats.fireDamageBonus, PlayerField.FireDamageBonus);
            this[PlayerField.DodgeAPProbability] = new StatsRow(stats.dodgePALostProbability, PlayerField.DodgeAPProbability);
            this[PlayerField.DodgeMPProbability] = new StatsRow(stats.dodgePMLostProbability, PlayerField.DodgeMPProbability);
            this[PlayerField.NeutralResistPercent] = new StatsRow(stats.neutralElementResistPercent, PlayerField.NeutralResistPercent);
            this[PlayerField.EarthResistPercent] = new StatsRow(stats.earthElementResistPercent, PlayerField.EarthResistPercent);
            this[PlayerField.WaterResistPercent] = new StatsRow(stats.waterElementResistPercent, PlayerField.WaterResistPercent);
            this[PlayerField.AirResistPercent] = new StatsRow(stats.airElementResistPercent, PlayerField.AirResistPercent);
            this[PlayerField.FireResistPercent] = new StatsRow(stats.fireElementResistPercent, PlayerField.FireResistPercent);
            this[PlayerField.NeutralElementReduction] = new StatsRow(stats.neutralElementReduction, PlayerField.NeutralElementReduction);
            this[PlayerField.EarthElementReduction] = new StatsRow(stats.earthElementReduction, PlayerField.EarthElementReduction);
            this[PlayerField.WaterElementReduction] = new StatsRow(stats.waterElementReduction, PlayerField.WaterElementReduction);
            this[PlayerField.AirElementReduction] = new StatsRow(stats.airElementReduction, PlayerField.AirElementReduction);
            this[PlayerField.FireElementReduction] = new StatsRow(stats.fireElementReduction, PlayerField.FireElementReduction);
            this[PlayerField.PushDamageReduction] = new StatsRow(stats.pushDamageReduction, PlayerField.PushDamageReduction);
            this[PlayerField.CriticalDamageReduction] = new StatsRow(stats.criticalDamageReduction, PlayerField.CriticalDamageReduction);
            this[PlayerField.PvpNeutralResistPercent] = new StatsRow(stats.pvpNeutralElementResistPercent, PlayerField.PvpNeutralResistPercent);
            this[PlayerField.PvpEarthResistPercent] = new StatsRow(stats.pvpEarthElementResistPercent, PlayerField.PvpEarthResistPercent);
            this[PlayerField.PvpWaterResistPercent] = new StatsRow(stats.pvpWaterElementResistPercent, PlayerField.PvpWaterResistPercent);
            this[PlayerField.PvpAirResistPercent] = new StatsRow(stats.pvpAirElementResistPercent, PlayerField.PvpAirResistPercent);
            this[PlayerField.PvpFireResistPercent] = new StatsRow(stats.pvpFireElementResistPercent, PlayerField.PvpFireResistPercent);
            this[PlayerField.PvpNeutralElementReduction] = new StatsRow(stats.pvpNeutralElementReduction, PlayerField.PvpNeutralElementReduction);
            this[PlayerField.PvpEarthElementReduction] = new StatsRow(stats.pvpEarthElementReduction, PlayerField.PvpEarthElementReduction);
            this[PlayerField.PvpWaterElementReduction] = new StatsRow(stats.pvpWaterElementReduction, PlayerField.PvpWaterElementReduction);
            this[PlayerField.AirElementReduction] = new StatsRow(stats.pvpAirElementReduction, PlayerField.AirElementReduction);
            this[PlayerField.PvpFireElementReduction] = new StatsRow(stats.pvpFireElementReduction, PlayerField.PvpFireElementReduction);
            
            SpellsModifications = new ObservableCollection<SpellModification>(stats.spellModifications.Select(entry => new SpellModification(entry)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}