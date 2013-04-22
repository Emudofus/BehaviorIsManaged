

// Generated on 04/17/2013 22:30:02
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PackRestrictedSubAreaMessage : NetworkMessage
    {
        public const uint Id = 6186;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int subAreaId;
        
        public PackRestrictedSubAreaMessage()
        {
        }
        
        public PackRestrictedSubAreaMessage(int subAreaId)
        {
            this.subAreaId = subAreaId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(subAreaId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            subAreaId = reader.ReadInt();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
        }
        
    }
    
}