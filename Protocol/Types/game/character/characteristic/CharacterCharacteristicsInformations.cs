

// Generated on 12/11/2012 19:44:32
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class CharacterCharacteristicsInformations
    {
        public const short Id = 8;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public double experience;
        public double experienceLevelFloor;
        public double experienceNextLevelFloor;
        public int kamas;
        public int statsPoints;
        public int spellsPoints;
        public Types.ActorExtendedAlignmentInformations alignmentInfos;
        public int lifePoints;
        public int maxLifePoints;
        public short energyPoints;
        public short maxEnergyPoints;
        public short actionPointsCurrent;
        public short movementPointsCurrent;
        public Types.CharacterBaseCharacteristic initiative;
        public Types.CharacterBaseCharacteristic prospecting;
        public Types.CharacterBaseCharacteristic actionPoints;
        public Types.CharacterBaseCharacteristic movementPoints;
        public Types.CharacterBaseCharacteristic strength;
        public Types.CharacterBaseCharacteristic vitality;
        public Types.CharacterBaseCharacteristic wisdom;
        public Types.CharacterBaseCharacteristic chance;
        public Types.CharacterBaseCharacteristic agility;
        public Types.CharacterBaseCharacteristic intelligence;
        public Types.CharacterBaseCharacteristic range;
        public Types.CharacterBaseCharacteristic summonableCreaturesBoost;
        public Types.CharacterBaseCharacteristic reflect;
        public Types.CharacterBaseCharacteristic criticalHit;
        public short criticalHitWeapon;
        public Types.CharacterBaseCharacteristic criticalMiss;
        public Types.CharacterBaseCharacteristic healBonus;
        public Types.CharacterBaseCharacteristic allDamagesBonus;
        public Types.CharacterBaseCharacteristic weaponDamagesBonusPercent;
        public Types.CharacterBaseCharacteristic damagesBonusPercent;
        public Types.CharacterBaseCharacteristic trapBonus;
        public Types.CharacterBaseCharacteristic trapBonusPercent;
        public Types.CharacterBaseCharacteristic permanentDamagePercent;
        public Types.CharacterBaseCharacteristic tackleBlock;
        public Types.CharacterBaseCharacteristic tackleEvade;
        public Types.CharacterBaseCharacteristic PAAttack;
        public Types.CharacterBaseCharacteristic PMAttack;
        public Types.CharacterBaseCharacteristic pushDamageBonus;
        public Types.CharacterBaseCharacteristic criticalDamageBonus;
        public Types.CharacterBaseCharacteristic neutralDamageBonus;
        public Types.CharacterBaseCharacteristic earthDamageBonus;
        public Types.CharacterBaseCharacteristic waterDamageBonus;
        public Types.CharacterBaseCharacteristic airDamageBonus;
        public Types.CharacterBaseCharacteristic fireDamageBonus;
        public Types.CharacterBaseCharacteristic dodgePALostProbability;
        public Types.CharacterBaseCharacteristic dodgePMLostProbability;
        public Types.CharacterBaseCharacteristic neutralElementResistPercent;
        public Types.CharacterBaseCharacteristic earthElementResistPercent;
        public Types.CharacterBaseCharacteristic waterElementResistPercent;
        public Types.CharacterBaseCharacteristic airElementResistPercent;
        public Types.CharacterBaseCharacteristic fireElementResistPercent;
        public Types.CharacterBaseCharacteristic neutralElementReduction;
        public Types.CharacterBaseCharacteristic earthElementReduction;
        public Types.CharacterBaseCharacteristic waterElementReduction;
        public Types.CharacterBaseCharacteristic airElementReduction;
        public Types.CharacterBaseCharacteristic fireElementReduction;
        public Types.CharacterBaseCharacteristic pushDamageReduction;
        public Types.CharacterBaseCharacteristic criticalDamageReduction;
        public Types.CharacterBaseCharacteristic pvpNeutralElementResistPercent;
        public Types.CharacterBaseCharacteristic pvpEarthElementResistPercent;
        public Types.CharacterBaseCharacteristic pvpWaterElementResistPercent;
        public Types.CharacterBaseCharacteristic pvpAirElementResistPercent;
        public Types.CharacterBaseCharacteristic pvpFireElementResistPercent;
        public Types.CharacterBaseCharacteristic pvpNeutralElementReduction;
        public Types.CharacterBaseCharacteristic pvpEarthElementReduction;
        public Types.CharacterBaseCharacteristic pvpWaterElementReduction;
        public Types.CharacterBaseCharacteristic pvpAirElementReduction;
        public Types.CharacterBaseCharacteristic pvpFireElementReduction;
        public Types.CharacterSpellModification[] spellModifications;
        
        public CharacterCharacteristicsInformations()
        {
        }
        
        public CharacterCharacteristicsInformations(double experience, double experienceLevelFloor, double experienceNextLevelFloor, int kamas, int statsPoints, int spellsPoints, Types.ActorExtendedAlignmentInformations alignmentInfos, int lifePoints, int maxLifePoints, short energyPoints, short maxEnergyPoints, short actionPointsCurrent, short movementPointsCurrent, Types.CharacterBaseCharacteristic initiative, Types.CharacterBaseCharacteristic prospecting, Types.CharacterBaseCharacteristic actionPoints, Types.CharacterBaseCharacteristic movementPoints, Types.CharacterBaseCharacteristic strength, Types.CharacterBaseCharacteristic vitality, Types.CharacterBaseCharacteristic wisdom, Types.CharacterBaseCharacteristic chance, Types.CharacterBaseCharacteristic agility, Types.CharacterBaseCharacteristic intelligence, Types.CharacterBaseCharacteristic range, Types.CharacterBaseCharacteristic summonableCreaturesBoost, Types.CharacterBaseCharacteristic reflect, Types.CharacterBaseCharacteristic criticalHit, short criticalHitWeapon, Types.CharacterBaseCharacteristic criticalMiss, Types.CharacterBaseCharacteristic healBonus, Types.CharacterBaseCharacteristic allDamagesBonus, Types.CharacterBaseCharacteristic weaponDamagesBonusPercent, Types.CharacterBaseCharacteristic damagesBonusPercent, Types.CharacterBaseCharacteristic trapBonus, Types.CharacterBaseCharacteristic trapBonusPercent, Types.CharacterBaseCharacteristic permanentDamagePercent, Types.CharacterBaseCharacteristic tackleBlock, Types.CharacterBaseCharacteristic tackleEvade, Types.CharacterBaseCharacteristic PAAttack, Types.CharacterBaseCharacteristic PMAttack, Types.CharacterBaseCharacteristic pushDamageBonus, Types.CharacterBaseCharacteristic criticalDamageBonus, Types.CharacterBaseCharacteristic neutralDamageBonus, Types.CharacterBaseCharacteristic earthDamageBonus, Types.CharacterBaseCharacteristic waterDamageBonus, Types.CharacterBaseCharacteristic airDamageBonus, Types.CharacterBaseCharacteristic fireDamageBonus, Types.CharacterBaseCharacteristic dodgePALostProbability, Types.CharacterBaseCharacteristic dodgePMLostProbability, Types.CharacterBaseCharacteristic neutralElementResistPercent, Types.CharacterBaseCharacteristic earthElementResistPercent, Types.CharacterBaseCharacteristic waterElementResistPercent, Types.CharacterBaseCharacteristic airElementResistPercent, Types.CharacterBaseCharacteristic fireElementResistPercent, Types.CharacterBaseCharacteristic neutralElementReduction, Types.CharacterBaseCharacteristic earthElementReduction, Types.CharacterBaseCharacteristic waterElementReduction, Types.CharacterBaseCharacteristic airElementReduction, Types.CharacterBaseCharacteristic fireElementReduction, Types.CharacterBaseCharacteristic pushDamageReduction, Types.CharacterBaseCharacteristic criticalDamageReduction, Types.CharacterBaseCharacteristic pvpNeutralElementResistPercent, Types.CharacterBaseCharacteristic pvpEarthElementResistPercent, Types.CharacterBaseCharacteristic pvpWaterElementResistPercent, Types.CharacterBaseCharacteristic pvpAirElementResistPercent, Types.CharacterBaseCharacteristic pvpFireElementResistPercent, Types.CharacterBaseCharacteristic pvpNeutralElementReduction, Types.CharacterBaseCharacteristic pvpEarthElementReduction, Types.CharacterBaseCharacteristic pvpWaterElementReduction, Types.CharacterBaseCharacteristic pvpAirElementReduction, Types.CharacterBaseCharacteristic pvpFireElementReduction, Types.CharacterSpellModification[] spellModifications)
        {
            this.experience = experience;
            this.experienceLevelFloor = experienceLevelFloor;
            this.experienceNextLevelFloor = experienceNextLevelFloor;
            this.kamas = kamas;
            this.statsPoints = statsPoints;
            this.spellsPoints = spellsPoints;
            this.alignmentInfos = alignmentInfos;
            this.lifePoints = lifePoints;
            this.maxLifePoints = maxLifePoints;
            this.energyPoints = energyPoints;
            this.maxEnergyPoints = maxEnergyPoints;
            this.actionPointsCurrent = actionPointsCurrent;
            this.movementPointsCurrent = movementPointsCurrent;
            this.initiative = initiative;
            this.prospecting = prospecting;
            this.actionPoints = actionPoints;
            this.movementPoints = movementPoints;
            this.strength = strength;
            this.vitality = vitality;
            this.wisdom = wisdom;
            this.chance = chance;
            this.agility = agility;
            this.intelligence = intelligence;
            this.range = range;
            this.summonableCreaturesBoost = summonableCreaturesBoost;
            this.reflect = reflect;
            this.criticalHit = criticalHit;
            this.criticalHitWeapon = criticalHitWeapon;
            this.criticalMiss = criticalMiss;
            this.healBonus = healBonus;
            this.allDamagesBonus = allDamagesBonus;
            this.weaponDamagesBonusPercent = weaponDamagesBonusPercent;
            this.damagesBonusPercent = damagesBonusPercent;
            this.trapBonus = trapBonus;
            this.trapBonusPercent = trapBonusPercent;
            this.permanentDamagePercent = permanentDamagePercent;
            this.tackleBlock = tackleBlock;
            this.tackleEvade = tackleEvade;
            this.PAAttack = PAAttack;
            this.PMAttack = PMAttack;
            this.pushDamageBonus = pushDamageBonus;
            this.criticalDamageBonus = criticalDamageBonus;
            this.neutralDamageBonus = neutralDamageBonus;
            this.earthDamageBonus = earthDamageBonus;
            this.waterDamageBonus = waterDamageBonus;
            this.airDamageBonus = airDamageBonus;
            this.fireDamageBonus = fireDamageBonus;
            this.dodgePALostProbability = dodgePALostProbability;
            this.dodgePMLostProbability = dodgePMLostProbability;
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
            this.pushDamageReduction = pushDamageReduction;
            this.criticalDamageReduction = criticalDamageReduction;
            this.pvpNeutralElementResistPercent = pvpNeutralElementResistPercent;
            this.pvpEarthElementResistPercent = pvpEarthElementResistPercent;
            this.pvpWaterElementResistPercent = pvpWaterElementResistPercent;
            this.pvpAirElementResistPercent = pvpAirElementResistPercent;
            this.pvpFireElementResistPercent = pvpFireElementResistPercent;
            this.pvpNeutralElementReduction = pvpNeutralElementReduction;
            this.pvpEarthElementReduction = pvpEarthElementReduction;
            this.pvpWaterElementReduction = pvpWaterElementReduction;
            this.pvpAirElementReduction = pvpAirElementReduction;
            this.pvpFireElementReduction = pvpFireElementReduction;
            this.spellModifications = spellModifications;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(experience);
            writer.WriteDouble(experienceLevelFloor);
            writer.WriteDouble(experienceNextLevelFloor);
            writer.WriteInt(kamas);
            writer.WriteInt(statsPoints);
            writer.WriteInt(spellsPoints);
            alignmentInfos.Serialize(writer);
            writer.WriteInt(lifePoints);
            writer.WriteInt(maxLifePoints);
            writer.WriteShort(energyPoints);
            writer.WriteShort(maxEnergyPoints);
            writer.WriteShort(actionPointsCurrent);
            writer.WriteShort(movementPointsCurrent);
            initiative.Serialize(writer);
            prospecting.Serialize(writer);
            actionPoints.Serialize(writer);
            movementPoints.Serialize(writer);
            strength.Serialize(writer);
            vitality.Serialize(writer);
            wisdom.Serialize(writer);
            chance.Serialize(writer);
            agility.Serialize(writer);
            intelligence.Serialize(writer);
            range.Serialize(writer);
            summonableCreaturesBoost.Serialize(writer);
            reflect.Serialize(writer);
            criticalHit.Serialize(writer);
            writer.WriteShort(criticalHitWeapon);
            criticalMiss.Serialize(writer);
            healBonus.Serialize(writer);
            allDamagesBonus.Serialize(writer);
            weaponDamagesBonusPercent.Serialize(writer);
            damagesBonusPercent.Serialize(writer);
            trapBonus.Serialize(writer);
            trapBonusPercent.Serialize(writer);
            permanentDamagePercent.Serialize(writer);
            tackleBlock.Serialize(writer);
            tackleEvade.Serialize(writer);
            PAAttack.Serialize(writer);
            PMAttack.Serialize(writer);
            pushDamageBonus.Serialize(writer);
            criticalDamageBonus.Serialize(writer);
            neutralDamageBonus.Serialize(writer);
            earthDamageBonus.Serialize(writer);
            waterDamageBonus.Serialize(writer);
            airDamageBonus.Serialize(writer);
            fireDamageBonus.Serialize(writer);
            dodgePALostProbability.Serialize(writer);
            dodgePMLostProbability.Serialize(writer);
            neutralElementResistPercent.Serialize(writer);
            earthElementResistPercent.Serialize(writer);
            waterElementResistPercent.Serialize(writer);
            airElementResistPercent.Serialize(writer);
            fireElementResistPercent.Serialize(writer);
            neutralElementReduction.Serialize(writer);
            earthElementReduction.Serialize(writer);
            waterElementReduction.Serialize(writer);
            airElementReduction.Serialize(writer);
            fireElementReduction.Serialize(writer);
            pushDamageReduction.Serialize(writer);
            criticalDamageReduction.Serialize(writer);
            pvpNeutralElementResistPercent.Serialize(writer);
            pvpEarthElementResistPercent.Serialize(writer);
            pvpWaterElementResistPercent.Serialize(writer);
            pvpAirElementResistPercent.Serialize(writer);
            pvpFireElementResistPercent.Serialize(writer);
            pvpNeutralElementReduction.Serialize(writer);
            pvpEarthElementReduction.Serialize(writer);
            pvpWaterElementReduction.Serialize(writer);
            pvpAirElementReduction.Serialize(writer);
            pvpFireElementReduction.Serialize(writer);
            writer.WriteUShort((ushort)spellModifications.Length);
            foreach (var entry in spellModifications)
            {
                 entry.Serialize(writer);
            }
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            experience = reader.ReadDouble();
            if (experience < 0)
                throw new Exception("Forbidden value on experience = " + experience + ", it doesn't respect the following condition : experience < 0");
            experienceLevelFloor = reader.ReadDouble();
            if (experienceLevelFloor < 0)
                throw new Exception("Forbidden value on experienceLevelFloor = " + experienceLevelFloor + ", it doesn't respect the following condition : experienceLevelFloor < 0");
            experienceNextLevelFloor = reader.ReadDouble();
            if (experienceNextLevelFloor < 0)
                throw new Exception("Forbidden value on experienceNextLevelFloor = " + experienceNextLevelFloor + ", it doesn't respect the following condition : experienceNextLevelFloor < 0");
            kamas = reader.ReadInt();
            if (kamas < 0)
                throw new Exception("Forbidden value on kamas = " + kamas + ", it doesn't respect the following condition : kamas < 0");
            statsPoints = reader.ReadInt();
            if (statsPoints < 0)
                throw new Exception("Forbidden value on statsPoints = " + statsPoints + ", it doesn't respect the following condition : statsPoints < 0");
            spellsPoints = reader.ReadInt();
            if (spellsPoints < 0)
                throw new Exception("Forbidden value on spellsPoints = " + spellsPoints + ", it doesn't respect the following condition : spellsPoints < 0");
            alignmentInfos = new Types.ActorExtendedAlignmentInformations();
            alignmentInfos.Deserialize(reader);
            lifePoints = reader.ReadInt();
            if (lifePoints < 0)
                throw new Exception("Forbidden value on lifePoints = " + lifePoints + ", it doesn't respect the following condition : lifePoints < 0");
            maxLifePoints = reader.ReadInt();
            if (maxLifePoints < 0)
                throw new Exception("Forbidden value on maxLifePoints = " + maxLifePoints + ", it doesn't respect the following condition : maxLifePoints < 0");
            energyPoints = reader.ReadShort();
            if (energyPoints < 0)
                throw new Exception("Forbidden value on energyPoints = " + energyPoints + ", it doesn't respect the following condition : energyPoints < 0");
            maxEnergyPoints = reader.ReadShort();
            if (maxEnergyPoints < 0)
                throw new Exception("Forbidden value on maxEnergyPoints = " + maxEnergyPoints + ", it doesn't respect the following condition : maxEnergyPoints < 0");
            actionPointsCurrent = reader.ReadShort();
            movementPointsCurrent = reader.ReadShort();
            initiative = new Types.CharacterBaseCharacteristic();
            initiative.Deserialize(reader);
            prospecting = new Types.CharacterBaseCharacteristic();
            prospecting.Deserialize(reader);
            actionPoints = new Types.CharacterBaseCharacteristic();
            actionPoints.Deserialize(reader);
            movementPoints = new Types.CharacterBaseCharacteristic();
            movementPoints.Deserialize(reader);
            strength = new Types.CharacterBaseCharacteristic();
            strength.Deserialize(reader);
            vitality = new Types.CharacterBaseCharacteristic();
            vitality.Deserialize(reader);
            wisdom = new Types.CharacterBaseCharacteristic();
            wisdom.Deserialize(reader);
            chance = new Types.CharacterBaseCharacteristic();
            chance.Deserialize(reader);
            agility = new Types.CharacterBaseCharacteristic();
            agility.Deserialize(reader);
            intelligence = new Types.CharacterBaseCharacteristic();
            intelligence.Deserialize(reader);
            range = new Types.CharacterBaseCharacteristic();
            range.Deserialize(reader);
            summonableCreaturesBoost = new Types.CharacterBaseCharacteristic();
            summonableCreaturesBoost.Deserialize(reader);
            reflect = new Types.CharacterBaseCharacteristic();
            reflect.Deserialize(reader);
            criticalHit = new Types.CharacterBaseCharacteristic();
            criticalHit.Deserialize(reader);
            criticalHitWeapon = reader.ReadShort();
            if (criticalHitWeapon < 0)
                throw new Exception("Forbidden value on criticalHitWeapon = " + criticalHitWeapon + ", it doesn't respect the following condition : criticalHitWeapon < 0");
            criticalMiss = new Types.CharacterBaseCharacteristic();
            criticalMiss.Deserialize(reader);
            healBonus = new Types.CharacterBaseCharacteristic();
            healBonus.Deserialize(reader);
            allDamagesBonus = new Types.CharacterBaseCharacteristic();
            allDamagesBonus.Deserialize(reader);
            weaponDamagesBonusPercent = new Types.CharacterBaseCharacteristic();
            weaponDamagesBonusPercent.Deserialize(reader);
            damagesBonusPercent = new Types.CharacterBaseCharacteristic();
            damagesBonusPercent.Deserialize(reader);
            trapBonus = new Types.CharacterBaseCharacteristic();
            trapBonus.Deserialize(reader);
            trapBonusPercent = new Types.CharacterBaseCharacteristic();
            trapBonusPercent.Deserialize(reader);
            permanentDamagePercent = new Types.CharacterBaseCharacteristic();
            permanentDamagePercent.Deserialize(reader);
            tackleBlock = new Types.CharacterBaseCharacteristic();
            tackleBlock.Deserialize(reader);
            tackleEvade = new Types.CharacterBaseCharacteristic();
            tackleEvade.Deserialize(reader);
            PAAttack = new Types.CharacterBaseCharacteristic();
            PAAttack.Deserialize(reader);
            PMAttack = new Types.CharacterBaseCharacteristic();
            PMAttack.Deserialize(reader);
            pushDamageBonus = new Types.CharacterBaseCharacteristic();
            pushDamageBonus.Deserialize(reader);
            criticalDamageBonus = new Types.CharacterBaseCharacteristic();
            criticalDamageBonus.Deserialize(reader);
            neutralDamageBonus = new Types.CharacterBaseCharacteristic();
            neutralDamageBonus.Deserialize(reader);
            earthDamageBonus = new Types.CharacterBaseCharacteristic();
            earthDamageBonus.Deserialize(reader);
            waterDamageBonus = new Types.CharacterBaseCharacteristic();
            waterDamageBonus.Deserialize(reader);
            airDamageBonus = new Types.CharacterBaseCharacteristic();
            airDamageBonus.Deserialize(reader);
            fireDamageBonus = new Types.CharacterBaseCharacteristic();
            fireDamageBonus.Deserialize(reader);
            dodgePALostProbability = new Types.CharacterBaseCharacteristic();
            dodgePALostProbability.Deserialize(reader);
            dodgePMLostProbability = new Types.CharacterBaseCharacteristic();
            dodgePMLostProbability.Deserialize(reader);
            neutralElementResistPercent = new Types.CharacterBaseCharacteristic();
            neutralElementResistPercent.Deserialize(reader);
            earthElementResistPercent = new Types.CharacterBaseCharacteristic();
            earthElementResistPercent.Deserialize(reader);
            waterElementResistPercent = new Types.CharacterBaseCharacteristic();
            waterElementResistPercent.Deserialize(reader);
            airElementResistPercent = new Types.CharacterBaseCharacteristic();
            airElementResistPercent.Deserialize(reader);
            fireElementResistPercent = new Types.CharacterBaseCharacteristic();
            fireElementResistPercent.Deserialize(reader);
            neutralElementReduction = new Types.CharacterBaseCharacteristic();
            neutralElementReduction.Deserialize(reader);
            earthElementReduction = new Types.CharacterBaseCharacteristic();
            earthElementReduction.Deserialize(reader);
            waterElementReduction = new Types.CharacterBaseCharacteristic();
            waterElementReduction.Deserialize(reader);
            airElementReduction = new Types.CharacterBaseCharacteristic();
            airElementReduction.Deserialize(reader);
            fireElementReduction = new Types.CharacterBaseCharacteristic();
            fireElementReduction.Deserialize(reader);
            pushDamageReduction = new Types.CharacterBaseCharacteristic();
            pushDamageReduction.Deserialize(reader);
            criticalDamageReduction = new Types.CharacterBaseCharacteristic();
            criticalDamageReduction.Deserialize(reader);
            pvpNeutralElementResistPercent = new Types.CharacterBaseCharacteristic();
            pvpNeutralElementResistPercent.Deserialize(reader);
            pvpEarthElementResistPercent = new Types.CharacterBaseCharacteristic();
            pvpEarthElementResistPercent.Deserialize(reader);
            pvpWaterElementResistPercent = new Types.CharacterBaseCharacteristic();
            pvpWaterElementResistPercent.Deserialize(reader);
            pvpAirElementResistPercent = new Types.CharacterBaseCharacteristic();
            pvpAirElementResistPercent.Deserialize(reader);
            pvpFireElementResistPercent = new Types.CharacterBaseCharacteristic();
            pvpFireElementResistPercent.Deserialize(reader);
            pvpNeutralElementReduction = new Types.CharacterBaseCharacteristic();
            pvpNeutralElementReduction.Deserialize(reader);
            pvpEarthElementReduction = new Types.CharacterBaseCharacteristic();
            pvpEarthElementReduction.Deserialize(reader);
            pvpWaterElementReduction = new Types.CharacterBaseCharacteristic();
            pvpWaterElementReduction.Deserialize(reader);
            pvpAirElementReduction = new Types.CharacterBaseCharacteristic();
            pvpAirElementReduction.Deserialize(reader);
            pvpFireElementReduction = new Types.CharacterBaseCharacteristic();
            pvpFireElementReduction.Deserialize(reader);
            var limit = reader.ReadUShort();
            spellModifications = new Types.CharacterSpellModification[limit];
            for (int i = 0; i < limit; i++)
            {
                 spellModifications[i] = new Types.CharacterSpellModification();
                 spellModifications[i].Deserialize(reader);
            }
        }
        
    }
    
}