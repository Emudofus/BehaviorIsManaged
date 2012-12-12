

// Generated on 12/11/2012 19:44:28
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeMultiCraftCrafterCanUseHisRessourcesMessage : NetworkMessage
    {
        public const uint Id = 6020;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool allowed;
        
        public ExchangeMultiCraftCrafterCanUseHisRessourcesMessage()
        {
        }
        
        public ExchangeMultiCraftCrafterCanUseHisRessourcesMessage(bool allowed)
        {
            this.allowed = allowed;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(allowed);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            allowed = reader.ReadBoolean();
        }
        
    }
    
}