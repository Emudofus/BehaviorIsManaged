

// Generated on 09/23/2012 22:27:02
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeShowVendorTaxMessage : NetworkMessage
    {
        public const uint Id = 5783;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public ExchangeShowVendorTaxMessage()
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