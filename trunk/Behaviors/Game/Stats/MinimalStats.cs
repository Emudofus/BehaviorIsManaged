#region License GNU GPL
// MinimalStats.cs
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