

// Generated on 12/11/2012 19:44:19
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class HouseSellFromInsideRequestMessage : HouseSellRequestMessage
    {
        public const uint Id = 5884;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public HouseSellFromInsideRequestMessage()
        {
        }
        
        public HouseSellFromInsideRequestMessage(int amount)
         : base(amount)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
    }
    
}