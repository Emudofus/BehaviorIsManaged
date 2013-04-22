

// Generated on 04/17/2013 22:29:41
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameContextDestroyMessage : NetworkMessage
    {
        public const uint Id = 201;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GameContextDestroyMessage()
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