

// Generated on 04/17/2013 22:30:02
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class AlignmentAreaUpdateMessage : NetworkMessage
    {
        public const uint Id = 6060;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short areaId;
        public sbyte side;
        
        public AlignmentAreaUpdateMessage()
        {
        }
        
        public AlignmentAreaUpdateMessage(short areaId, sbyte side)
        {
            this.areaId = areaId;
            this.side = side;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(areaId);
            writer.WriteSByte(side);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            areaId = reader.ReadShort();
            if (areaId < 0)
                throw new Exception("Forbidden value on areaId = " + areaId + ", it doesn't respect the following condition : areaId < 0");
            side = reader.ReadSByte();
        }
        
    }
    
}