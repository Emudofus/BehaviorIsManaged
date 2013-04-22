

// Generated on 04/17/2013 22:29:47
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class LockableUseCodeMessage : NetworkMessage
    {
        public const uint Id = 5667;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string code;
        
        public LockableUseCodeMessage()
        {
        }
        
        public LockableUseCodeMessage(string code)
        {
            this.code = code;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(code);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            code = reader.ReadUTF();
        }
        
    }
    
}