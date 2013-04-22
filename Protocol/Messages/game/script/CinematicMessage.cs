

// Generated on 04/17/2013 22:30:03
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class CinematicMessage : NetworkMessage
    {
        public const uint Id = 6053;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short cinematicId;
        
        public CinematicMessage()
        {
        }
        
        public CinematicMessage(short cinematicId)
        {
            this.cinematicId = cinematicId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(cinematicId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            cinematicId = reader.ReadShort();
            if (cinematicId < 0)
                throw new Exception("Forbidden value on cinematicId = " + cinematicId + ", it doesn't respect the following condition : cinematicId < 0");
        }
        
    }
    
}