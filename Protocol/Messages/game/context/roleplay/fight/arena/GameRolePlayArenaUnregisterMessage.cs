

// Generated on 12/11/2012 19:44:18
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameRolePlayArenaUnregisterMessage : NetworkMessage
    {
        public const uint Id = 6282;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GameRolePlayArenaUnregisterMessage()
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