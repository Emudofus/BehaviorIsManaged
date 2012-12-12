

// Generated on 12/11/2012 19:44:33
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class GameFightMinimalStatsPreparation : GameFightMinimalStats
    {
        public const short Id = 360;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int initiative;
        
        public GameFightMinimalStatsPreparation()
        {
        }
        
        public GameFightMinimalStatsPreparation(int lifePoints, int maxLifePoints, int baseMaxLifePoints, int permanentDamagePercent, int shieldPoints, short actionPoints, short maxActionPoints, short movementPoints, short maxMovementPoints, int summoner, bool summoned, short neutralElementResistPercent, short earthElementResistPercent, short waterElementResistPercent, short airElementResistPercent, short fireElementResistPercent, short neutralElementReduction, short earthElementReduction, short waterElementReduction, short airElementReduction, short fireElementReduction, short criticalDamageFixedResist, short pushDamageFixedResist, short dodgePALostProbability, short dodgePMLostProbability, short tackleBlock, short tackleEvade, sbyte invisibilityState, int initiative)
         : base(lifePoints, maxLifePoints, baseMaxLifePoints, permanentDamagePercent, shieldPoints, actionPoints, maxActionPoints, movementPoints, maxMovementPoints, summoner, summoned, neutralElementResistPercent, earthElementResistPercent, waterElementResistPercent, airElementResistPercent, fireElementResistPercent, neutralElementReduction, earthElementReduction, waterElementReduction, airElementReduction, fireElementReduction, criticalDamageFixedResist, pushDamageFixedResist, dodgePALostProbability, dodgePMLostProbability, tackleBlock, tackleEvade, invisibilityState)
        {
            this.initiative = initiative;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(initiative);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            initiative = reader.ReadInt();
            if (initiative < 0)
                throw new Exception("Forbidden value on initiative = " + initiative + ", it doesn't respect the following condition : initiative < 0");
        }
        
    }
    
}