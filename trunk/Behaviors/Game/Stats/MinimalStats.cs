using System;
using System.Collections.Generic;
using BiM.Protocol.Enums;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Stats
{
    /// <summary>
    /// Stats fields used in fight
    /// </summary>
    public class MinimalStats
    {
        public MinimalStats(GameFightMinimalStats stats)
        {
            Update(stats);
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

        public int MaxAP
        {
            get { return this[PlayerField.AP]; }
        }

        public int MaxMP
        {
            get { return this[PlayerField.MP]; }
        }

        public GameActionFightInvisibilityStateEnum InvisibilityState
        {
            get;
            set;
        }

        public Dictionary<PlayerField, int> Fields
        {
            get;
            set;
        }

        public int this[PlayerField name]
        {
            get
            {
                int value;
                return Fields.TryGetValue(name, out value) ? value : default(int);
            }
            set
            {
                if (!Fields.ContainsKey(name))
                    Fields.Add(name, value);
                else
                    Fields[name] = value;
            }
        }


        public void Update(GameFightMinimalStats stats)
        {
            if (stats == null) throw new ArgumentNullException("stats");

            Health = stats.lifePoints;
            MaxHealth = stats.maxLifePoints;
            CurrentAP = stats.actionPoints;
            CurrentMP = stats.movementPoints;

            this[PlayerField.AP] = stats.maxActionPoints;
            this[PlayerField.MP] = stats.maxMovementPoints;
            this[PlayerField.PermanentDamagePercent] = stats.permanentDamagePercent;
            this[PlayerField.TackleBlock] = stats.tackleBlock;
            this[PlayerField.TackleEvade] = stats.tackleEvade;
            this[PlayerField.DodgeAPProbability] = stats.dodgePALostProbability;
            this[PlayerField.DodgeMPProbability] = stats.dodgePMLostProbability;
            this[PlayerField.NeutralResistPercent] = stats.neutralElementResistPercent;
            this[PlayerField.EarthResistPercent] = stats.earthElementResistPercent;
            this[PlayerField.WaterResistPercent] = stats.waterElementResistPercent;
            this[PlayerField.AirResistPercent] = stats.airElementResistPercent;
            this[PlayerField.FireResistPercent] = stats.fireElementResistPercent;
            this[PlayerField.NeutralElementReduction] = stats.neutralElementReduction;
            this[PlayerField.EarthElementReduction] = stats.earthElementReduction;
            this[PlayerField.WaterElementReduction] = stats.waterElementReduction;
            this[PlayerField.AirElementReduction] = stats.airElementReduction;
            this[PlayerField.FireElementReduction] = stats.fireElementReduction;

            InvisibilityState = (GameActionFightInvisibilityStateEnum) stats.invisibilityState;
        }
    }
}