

// Generated on 04/17/2013 22:29:56
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeCraftResultWithObjectDescMessage : ExchangeCraftResultMessage
    {
        public const uint Id = 5999;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ObjectItemNotInContainer objectInfo;
        
        public ExchangeCraftResultWithObjectDescMessage()
        {
        }
        
        public ExchangeCraftResultWithObjectDescMessage(sbyte craftResult, Types.ObjectItemNotInContainer objectInfo)
         : base(craftResult)
        {
            this.objectInfo = objectInfo;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            objectInfo.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            objectInfo = new Types.ObjectItemNotInContainer();
            objectInfo.Deserialize(reader);
        }
        
    }
    
}