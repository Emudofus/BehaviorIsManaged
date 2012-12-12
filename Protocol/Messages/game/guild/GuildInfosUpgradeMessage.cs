

// Generated on 12/11/2012 19:44:23
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GuildInfosUpgradeMessage : NetworkMessage
    {
        public const uint Id = 5636;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte maxTaxCollectorsCount;
        public sbyte taxCollectorsCount;
        public short taxCollectorLifePoints;
        public short taxCollectorDamagesBonuses;
        public short taxCollectorPods;
        public short taxCollectorProspecting;
        public short taxCollectorWisdom;
        public short boostPoints;
        public short[] spellId;
        public sbyte[] spellLevel;
        
        public GuildInfosUpgradeMessage()
        {
        }
        
        public GuildInfosUpgradeMessage(sbyte maxTaxCollectorsCount, sbyte taxCollectorsCount, short taxCollectorLifePoints, short taxCollectorDamagesBonuses, short taxCollectorPods, short taxCollectorProspecting, short taxCollectorWisdom, short boostPoints, short[] spellId, sbyte[] spellLevel)
        {
            this.maxTaxCollectorsCount = maxTaxCollectorsCount;
            this.taxCollectorsCount = taxCollectorsCount;
            this.taxCollectorLifePoints = taxCollectorLifePoints;
            this.taxCollectorDamagesBonuses = taxCollectorDamagesBonuses;
            this.taxCollectorPods = taxCollectorPods;
            this.taxCollectorProspecting = taxCollectorProspecting;
            this.taxCollectorWisdom = taxCollectorWisdom;
            this.boostPoints = boostPoints;
            this.spellId = spellId;
            this.spellLevel = spellLevel;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(maxTaxCollectorsCount);
            writer.WriteSByte(taxCollectorsCount);
            writer.WriteShort(taxCollectorLifePoints);
            writer.WriteShort(taxCollectorDamagesBonuses);
            writer.WriteShort(taxCollectorPods);
            writer.WriteShort(taxCollectorProspecting);
            writer.WriteShort(taxCollectorWisdom);
            writer.WriteShort(boostPoints);
            writer.WriteUShort((ushort)spellId.Length);
            foreach (var entry in spellId)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)spellLevel.Length);
            foreach (var entry in spellLevel)
            {
                 writer.WriteSByte(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            maxTaxCollectorsCount = reader.ReadSByte();
            if (maxTaxCollectorsCount < 0)
                throw new Exception("Forbidden value on maxTaxCollectorsCount = " + maxTaxCollectorsCount + ", it doesn't respect the following condition : maxTaxCollectorsCount < 0");
            taxCollectorsCount = reader.ReadSByte();
            if (taxCollectorsCount < 0)
                throw new Exception("Forbidden value on taxCollectorsCount = " + taxCollectorsCount + ", it doesn't respect the following condition : taxCollectorsCount < 0");
            taxCollectorLifePoints = reader.ReadShort();
            if (taxCollectorLifePoints < 0)
                throw new Exception("Forbidden value on taxCollectorLifePoints = " + taxCollectorLifePoints + ", it doesn't respect the following condition : taxCollectorLifePoints < 0");
            taxCollectorDamagesBonuses = reader.ReadShort();
            if (taxCollectorDamagesBonuses < 0)
                throw new Exception("Forbidden value on taxCollectorDamagesBonuses = " + taxCollectorDamagesBonuses + ", it doesn't respect the following condition : taxCollectorDamagesBonuses < 0");
            taxCollectorPods = reader.ReadShort();
            if (taxCollectorPods < 0)
                throw new Exception("Forbidden value on taxCollectorPods = " + taxCollectorPods + ", it doesn't respect the following condition : taxCollectorPods < 0");
            taxCollectorProspecting = reader.ReadShort();
            if (taxCollectorProspecting < 0)
                throw new Exception("Forbidden value on taxCollectorProspecting = " + taxCollectorProspecting + ", it doesn't respect the following condition : taxCollectorProspecting < 0");
            taxCollectorWisdom = reader.ReadShort();
            if (taxCollectorWisdom < 0)
                throw new Exception("Forbidden value on taxCollectorWisdom = " + taxCollectorWisdom + ", it doesn't respect the following condition : taxCollectorWisdom < 0");
            boostPoints = reader.ReadShort();
            if (boostPoints < 0)
                throw new Exception("Forbidden value on boostPoints = " + boostPoints + ", it doesn't respect the following condition : boostPoints < 0");
            var limit = reader.ReadUShort();
            spellId = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 spellId[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            spellLevel = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 spellLevel[i] = reader.ReadSByte();
            }
        }
        
    }
    
}