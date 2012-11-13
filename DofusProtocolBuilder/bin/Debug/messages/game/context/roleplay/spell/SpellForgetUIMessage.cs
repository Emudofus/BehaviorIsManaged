

// Generated on 10/25/2012 10:42:44
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class SpellForgetUIMessage : NetworkMessage
    {
        public const uint Id = 5565;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool open;
        
        public SpellForgetUIMessage()
        {
        }
        
        public SpellForgetUIMessage(bool open)
        {
            this.open = open;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(open);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            open = reader.ReadBoolean();
        }
        
    }
    
}