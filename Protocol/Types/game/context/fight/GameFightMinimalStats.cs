

// Generated on 04/17/2013 22:30:07
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class GameFightMinimalStats
    {
        public const short Id = 31;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int lifePoints;
        public int maxLifePoints;
        public int baseMaxLifePoints;
        public int permanentDamagePercent;
        public int shieldPoints;
        public short actionPoints;
        public short maxActionPoints;
        public short movementPoints;
        public short maxMovementPoints;
        public int summoner;
        public bool summoned;
        public short neutralElementResistPercent;
        public short earthElementResistPercent;
        public short waterElementResistPercent;
        public short airElementResistPercent;
        public short fireElementResistPercent;
        public short neutralElementReduction;
        public short earthElementReduction;
        public short waterElementReduction;
        public short airElementReduction;
        public short fireElementReduction;
        public short criticalDamageFixedResist;
        public short pushDamageFixedResist;
        public short dodgePALostProbability;
        public short dodgePMLostProbability;
        public short tackleBlock;
        public short tackleEvade;
        public sbyte invisibilityState;
        
        public GameFightMinimalStats()
        {
        }
        
        public GameFightMinimalStats(int lifePoints, int maxLifePoints, int baseMaxLifePoints, int permanentDamagePercent, int shieldPoints, short actionPoints, short maxActionPoints, short movementPoints, short maxMovementPoints, int summoner, bool summoned, short neutralElementResistPercent, short earthElementResistPercent, short waterElementResistPercent, short airElementResistPercent, short fireElementResistPercent, short neutralElementReduction, short earthElementReduction, short waterElementReduction, short airElementReduction, short fireElementReduction, short criticalDamageFixedResist, short pushDamageFixedResist, short dodgePALostProbability, short dodgePMLostProbability, short tackleBlock, short tackleEvade, sbyte invisibilityState)
        {
            this.lifePoints = lifePoints;
            this.maxLifePoints = maxLifePoints;
            this.baseMaxLifePoints = baseMaxLifePoints;
            this.permanentDamagePercent = permanentDamagePercent;
            this.shieldPoints = shieldPoints;
            this.actionPoints = actionPoints;
            this.maxActionPoints = maxActionPoints;
            this.movementPoints = movementPoints;
            this.maxMovementPoints = maxMovementPoints;
            this.summoner = summoner;
            this.summoned = summoned;
            this.neutralElementResistPercent = neutralElementResistPercent;
            this.earthElementResistPercent = earthElementResistPercent;
            this.waterElementResistPercent = waterElementResistPercent;
            this.airElementResistPercent = airElementResistPercent;
            this.fireElementResistPercent = fireElementResistPercent;
            this.neutralElementReduction = neutralElementReduction;
            this.earthElementReduction = earthElementReduction;
            this.waterElementReduction = waterElementReduction;
            this.airElementReduction = airElementReduction;
            this.fireElementReduction = fireElementReduction;
            this.criticalDamageFixedResist = criticalDamageFixedResist;
            this.pushDamageFixedResist = pushDamageFixedResist;
            this.dodgePALostProbability = dodgePALostProbability;
            this.dodgePMLostProbability = dodgePMLostProbability;
            this.tackleBlock = tackleBlock;
            this.tackleEvade = tackleEvade;
            this.invisibilityState = invisibilityState;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(lifePoints);
            writer.WriteInt(maxLifePoints);
            writer.WriteInt(baseMaxLifePoints);
            writer.WriteInt(permanentDamagePercent);
            writer.WriteInt(shieldPoints);
            writer.WriteShort(actionPoints);
            writer.WriteShort(maxActionPoints);
            writer.WriteShort(movementPoints);
            writer.WriteShort(maxMovementPoints);
            writer.WriteInt(summoner);
            writer.WriteBoolean(summoned);
            writer.WriteShort(neutralElementResistPercent);
            writer.WriteShort(earthElementResistPercent);
            writer.WriteShort(waterElementResistPercent);
            writer.WriteShort(airElementResistPercent);
            writer.WriteShort(fireElementResistPercent);
            writer.WriteShort(neutralElementReduction);
            writer.WriteShort(earthElementReduction);
            writer.WriteShort(waterElementReduction);
            writer.WriteShort(airElementReduction);
            writer.WriteShort(fireElementReduction);
            writer.WriteShort(criticalDamageFixedResist);
            writer.WriteShort(pushDamageFixedResist);
            writer.WriteShort(dodgePALostProbability);
            writer.WriteShort(dodgePMLostProbability);
            writer.WriteShort(tackleBlock);
            writer.WriteShort(tackleEvade);
            writer.WriteSByte(invisibilityState);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            lifePoints = reader.ReadInt();
            if (lifePoints < 0)
                throw new Exception("Forbidden value on lifePoints = " + lifePoints + ", it doesn't respect the following condition : lifePoints < 0");
            maxLifePoints = reader.ReadInt();
            if (maxLifePoints < 0)
                throw new Exception("Forbidden value on maxLifePoints = " + maxLifePoints + ", it doesn't respect the following condition : maxLifePoints < 0");
            baseMaxLifePoints = reader.ReadInt();
            if (baseMaxLifePoints < 0)
                throw new Exception("Forbidden value on baseMaxLifePoints = " + baseMaxLifePoints + ", it doesn't respect the following condition : baseMaxLifePoints < 0");
            permanentDamagePercent = reader.ReadInt();
            if (permanentDamagePercent < 0)
                throw new Exception("Forbidden value on permanentDamagePercent = " + permanentDamagePercent + ", it doesn't respect the following condition : permanentDamagePercent < 0");
            shieldPoints = reader.ReadInt();
            if (shieldPoints < 0)
                throw new Exception("Forbidden value on shieldPoints = " + shieldPoints + ", it doesn't respect the following condition : shieldPoints < 0");
            actionPoints = reader.ReadShort();
            maxActionPoints = reader.ReadShort();
            movementPoints = reader.ReadShort();
            maxMovementPoints = reader.ReadShort();
            summoner = reader.ReadInt();
            summoned = reader.ReadBoolean();
            neutralElementResistPercent = reader.ReadShort();
            earthElementResistPercent = reader.ReadShort();
            waterElementResistPercent = reader.ReadShort();
            airElementResistPercent = reader.ReadShort();
            fireElementResistPercent = reader.ReadShort();
            neutralElementReduction = reader.ReadShort();
            earthElementReduction = reader.ReadShort();
            waterElementReduction = reader.ReadShort();
            airElementReduction = reader.ReadShort();
            fireElementReduction = reader.ReadShort();
            criticalDamageFixedResist = reader.ReadShort();
            pushDamageFixedResist = reader.ReadShort();
            dodgePALostProbability = reader.ReadShort();
            if (dodgePALostProbability < 0)
                throw new Exception("Forbidden value on dodgePALostProbability = " + dodgePALostProbability + ", it doesn't respect the following condition : dodgePALostProbability < 0");
            dodgePMLostProbability = reader.ReadShort();
            if (dodgePMLostProbability < 0)
                throw new Exception("Forbidden value on dodgePMLostProbability = " + dodgePMLostProbability + ", it doesn't respect the following condition : dodgePMLostProbability < 0");
            tackleBlock = reader.ReadShort();
            tackleEvade = reader.ReadShort();
            invisibilityState = reader.ReadSByte();
        }
        
    }
    
}