

// Generated on 04/17/2013 22:30:04
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class TitleSelectedMessage : NetworkMessage
    {
        public const uint Id = 6366;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short titleId;
        
        public TitleSelectedMessage()
        {
        }
        
        public TitleSelectedMessage(short titleId)
        {
            this.titleId = titleId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(titleId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            titleId = reader.ReadShort();
            if (titleId < 0)
                throw new Exception("Forbidden value on titleId = " + titleId + ", it doesn't respect the following condition : titleId < 0");
        }
        
    }
    
}