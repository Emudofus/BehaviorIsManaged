

// Generated on 04/17/2013 22:30:00
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeObjectRemovedMessage : ExchangeObjectMessage
    {
        public const uint Id = 5517;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int objectUID;
        
        public ExchangeObjectRemovedMessage()
        {
        }
        
        public ExchangeObjectRemovedMessage(bool remote, int objectUID)
         : base(remote)
        {
            this.objectUID = objectUID;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(objectUID);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            objectUID = reader.ReadInt();
            if (objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
        }
        
    }
    
}