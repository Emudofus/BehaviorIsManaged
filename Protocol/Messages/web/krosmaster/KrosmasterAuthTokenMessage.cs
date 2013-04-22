

// Generated on 04/17/2013 22:30:04
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class KrosmasterAuthTokenMessage : NetworkMessage
    {
        public const uint Id = 6351;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string token;
        
        public KrosmasterAuthTokenMessage()
        {
        }
        
        public KrosmasterAuthTokenMessage(string token)
        {
            this.token = token;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(token);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            token = reader.ReadUTF();
        }
        
    }
    
}