

// Generated on 12/11/2012 19:44:20
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PaddockToSellFilterMessage : NetworkMessage
    {
        public const uint Id = 6161;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int areaId;
        public sbyte atLeastNbMount;
        public sbyte atLeastNbMachine;
        public int maxPrice;
        
        public PaddockToSellFilterMessage()
        {
        }
        
        public PaddockToSellFilterMessage(int areaId, sbyte atLeastNbMount, sbyte atLeastNbMachine, int maxPrice)
        {
            this.areaId = areaId;
            this.atLeastNbMount = atLeastNbMount;
            this.atLeastNbMachine = atLeastNbMachine;
            this.maxPrice = maxPrice;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(areaId);
            writer.WriteSByte(atLeastNbMount);
            writer.WriteSByte(atLeastNbMachine);
            writer.WriteInt(maxPrice);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            areaId = reader.ReadInt();
            atLeastNbMount = reader.ReadSByte();
            atLeastNbMachine = reader.ReadSByte();
            maxPrice = reader.ReadInt();
            if (maxPrice < 0)
                throw new Exception("Forbidden value on maxPrice = " + maxPrice + ", it doesn't respect the following condition : maxPrice < 0");
        }
        
    }
    
}