

// Generated on 12/11/2012 19:44:25
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class KamasUpdateMessage : NetworkMessage
    {
        public const uint Id = 5537;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int kamasTotal;
        
        public KamasUpdateMessage()
        {
        }
        
        public KamasUpdateMessage(int kamasTotal)
        {
            this.kamasTotal = kamasTotal;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(kamasTotal);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            kamasTotal = reader.ReadInt();
        }
        
    }
    
}