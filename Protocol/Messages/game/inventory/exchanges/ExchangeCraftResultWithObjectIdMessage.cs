

// Generated on 12/11/2012 19:44:26
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeCraftResultWithObjectIdMessage : ExchangeCraftResultMessage
    {
        public const uint Id = 6000;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int objectGenericId;
        
        public ExchangeCraftResultWithObjectIdMessage()
        {
        }
        
        public ExchangeCraftResultWithObjectIdMessage(sbyte craftResult, int objectGenericId)
         : base(craftResult)
        {
            this.objectGenericId = objectGenericId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(objectGenericId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            objectGenericId = reader.ReadInt();
            if (objectGenericId < 0)
                throw new Exception("Forbidden value on objectGenericId = " + objectGenericId + ", it doesn't respect the following condition : objectGenericId < 0");
        }
        
    }
    
}