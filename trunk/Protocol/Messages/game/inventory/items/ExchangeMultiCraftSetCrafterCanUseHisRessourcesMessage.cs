

// Generated on 10/25/2012 10:42:51
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeMultiCraftSetCrafterCanUseHisRessourcesMessage : NetworkMessage
    {
        public const uint Id = 6021;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool allow;
        
        public ExchangeMultiCraftSetCrafterCanUseHisRessourcesMessage()
        {
        }
        
        public ExchangeMultiCraftSetCrafterCanUseHisRessourcesMessage(bool allow)
        {
            this.allow = allow;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(allow);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            allow = reader.ReadBoolean();
        }
        
    }
    
}