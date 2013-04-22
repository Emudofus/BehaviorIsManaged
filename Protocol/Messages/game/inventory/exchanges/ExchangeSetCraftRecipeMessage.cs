

// Generated on 04/17/2013 22:29:58
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeSetCraftRecipeMessage : NetworkMessage
    {
        public const uint Id = 6389;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short objectGID;
        
        public ExchangeSetCraftRecipeMessage()
        {
        }
        
        public ExchangeSetCraftRecipeMessage(short objectGID)
        {
            this.objectGID = objectGID;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(objectGID);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            objectGID = reader.ReadShort();
            if (objectGID < 0)
                throw new Exception("Forbidden value on objectGID = " + objectGID + ", it doesn't respect the following condition : objectGID < 0");
        }
        
    }
    
}