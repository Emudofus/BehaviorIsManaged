

// Generated on 10/25/2012 10:42:31
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class AcquaintanceSearchMessage : NetworkMessage
    {
        public const uint Id = 6144;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string nickname;
        
        public AcquaintanceSearchMessage()
        {
        }
        
        public AcquaintanceSearchMessage(string nickname)
        {
            this.nickname = nickname;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(nickname);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            nickname = reader.ReadUTF();
        }
        
    }
    
}