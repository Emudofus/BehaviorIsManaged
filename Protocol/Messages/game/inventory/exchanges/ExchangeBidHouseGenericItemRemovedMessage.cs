

// Generated on 12/11/2012 19:44:25
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeBidHouseGenericItemRemovedMessage : NetworkMessage
    {
        public const uint Id = 5948;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int objGenericId;
        
        public ExchangeBidHouseGenericItemRemovedMessage()
        {
        }
        
        public ExchangeBidHouseGenericItemRemovedMessage(int objGenericId)
        {
            this.objGenericId = objGenericId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(objGenericId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            objGenericId = reader.ReadInt();
        }
        
    }
    
}