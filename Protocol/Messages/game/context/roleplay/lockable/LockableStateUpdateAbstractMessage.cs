

// Generated on 12/11/2012 19:44:19
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class LockableStateUpdateAbstractMessage : NetworkMessage
    {
        public const uint Id = 5671;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool locked;
        
        public LockableStateUpdateAbstractMessage()
        {
        }
        
        public LockableStateUpdateAbstractMessage(bool locked)
        {
            this.locked = locked;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(locked);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            locked = reader.ReadBoolean();
        }
        
    }
    
}