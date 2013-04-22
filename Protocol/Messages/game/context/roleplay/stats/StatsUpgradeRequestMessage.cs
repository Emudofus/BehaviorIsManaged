

// Generated on 04/17/2013 22:29:52
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class StatsUpgradeRequestMessage : NetworkMessage
    {
        public const uint Id = 5610;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte statId;
        public short boostPoint;
        
        public StatsUpgradeRequestMessage()
        {
        }
        
        public StatsUpgradeRequestMessage(sbyte statId, short boostPoint)
        {
            this.statId = statId;
            this.boostPoint = boostPoint;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(statId);
            writer.WriteShort(boostPoint);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            statId = reader.ReadSByte();
            if (statId < 0)
                throw new Exception("Forbidden value on statId = " + statId + ", it doesn't respect the following condition : statId < 0");
            boostPoint = reader.ReadShort();
            if (boostPoint < 0)
                throw new Exception("Forbidden value on boostPoint = " + boostPoint + ", it doesn't respect the following condition : boostPoint < 0");
        }
        
    }
    
}