using System;
using System.Collections.Generic;
using System.ComponentModel;
using BiM.Protocol.Enums;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Stats
{
    /// <summary>
    /// Stats fields used in fight
    /// </summary>
    public class MinimalStats : IMinimalStats, INotifyPropertyChanged
    {
        public MinimalStats(GameFightMinimalStats stats)
        {
            Update(stats);
        }

        public int Initiative
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

        public int MaxHealthBase
        {
            get;
            set;
        }

        public int Range
        {
            get;
            set;
        }

        public int PermanentDamagePercent
        {
            get;
            set;
        }

        public int TackleBlock
        {
            get;
            set;
        }

        public int TackleEvade
        {
            get;
            set;
        }

        public int DodgeAPProbability
        {
            get;
            set;
        }

        public int DodgeMPProbability
        {
            get;
            set;
        }

        public int NeutralResistPercent
        {
            get;
            set;
        }

        public int EarthResistPercent
        {
            get;
            set;
        }

        public int WaterResistPercent
        {
            get;
            set;
        }

        public int AirResistPercent
        {
            get;
            set;
        }

        public int FireResistPercent
        {
            get;
            set;
        }

        public int NeutralElementReduction
        {
            get;
            set;
        }

        public int EarthElementReduction
        {
            get;
            set;
        }

        public int WaterElementReduction
        {
            get;
            set;
        }

        public int AirElementReduction
        {
            get;
            set;
        }

        public int FireElementReduction
        {
            get;
            set;
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

        public int MaxAP
        {
            get;
            set;
        }

        public int MaxMP
        {
            get;
            set;
        }

        public GameActionFightInvisibilityStateEnum InvisibilityState
        {
            get;
            set;
        }

        public void Update(GameFightMinimalStats stats)
        {
            if (stats == null) throw new ArgumentNullException("stats");

            Health = stats.lifePoints;
            MaxHealth = stats.maxLifePoints;
            CurrentAP = stats.actionPoints;
            CurrentMP = stats.movementPoints;
            MaxAP = stats.maxActionPoints;
            MaxMP = stats.maxMovementPoints;
            PermanentDamagePercent = stats.permanentDamagePercent;
            TackleBlock = stats.tackleBlock;
            TackleEvade = stats.tackleEvade;
            DodgeAPProbability = stats.dodgePALostProbability;
            DodgeMPProbability = stats.dodgePMLostProbability;
            NeutralResistPercent = stats.neutralElementResistPercent;
            EarthResistPercent = stats.earthElementResistPercent;
            WaterResistPercent = stats.waterElementResistPercent;
            AirResistPercent = stats.airElementResistPercent;
            FireResistPercent = stats.fireElementResistPercent;
            NeutralElementReduction = stats.neutralElementReduction;
            EarthElementReduction = stats.earthElementReduction;
            WaterElementReduction = stats.waterElementReduction;
            AirElementReduction = stats.airElementReduction;
            FireElementReduction = stats.fireElementReduction;

            InvisibilityState = (GameActionFightInvisibilityStateEnum) stats.invisibilityState;
        }

        public void Update(GameFightMinimalStatsPreparation stats)
        {
            Update((GameFightMinimalStats)stats);
            Initiative = stats.initiative;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}