

// Generated on 10/25/2012 10:42:35
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class CharacterSelectedForceReadyMessage : NetworkMessage
    {
        public const uint Id = 6072;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public CharacterSelectedForceReadyMessage()
        {
        }
        
        
        public override void Serialize(IDataWriter writer)
        {
        }
        
        public override void Deserialize(IDataReader reader)
        {
        }
        
    }
    
}