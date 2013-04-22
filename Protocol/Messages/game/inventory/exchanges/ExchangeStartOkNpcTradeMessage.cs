

// Generated on 04/17/2013 22:29:59
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeStartOkNpcTradeMessage : NetworkMessage
    {
        public const uint Id = 5785;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int npcId;
        
        public ExchangeStartOkNpcTradeMessage()
        {
        }
        
        public ExchangeStartOkNpcTradeMessage(int npcId)
        {
            this.npcId = npcId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(npcId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            npcId = reader.ReadInt();
        }
        
    }
    
}