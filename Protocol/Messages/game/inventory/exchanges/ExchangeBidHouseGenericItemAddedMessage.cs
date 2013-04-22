

// Generated on 04/17/2013 22:29:56
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeBidHouseGenericItemAddedMessage : NetworkMessage
    {
        public const uint Id = 5947;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int objGenericId;
        
        public ExchangeBidHouseGenericItemAddedMessage()
        {
        }
        
        public ExchangeBidHouseGenericItemAddedMessage(int objGenericId)
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