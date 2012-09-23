

// Generated on 09/23/2012 22:26:59
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeBidHouseBuyResultMessage : NetworkMessage
    {
        public const uint Id = 6272;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int uid;
        public bool bought;
        
        public ExchangeBidHouseBuyResultMessage()
        {
        }
        
        public ExchangeBidHouseBuyResultMessage(int uid, bool bought)
        {
            this.uid = uid;
            this.bought = bought;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(uid);
            writer.WriteBoolean(bought);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            uid = reader.ReadInt();
            if (uid < 0)
                throw new Exception("Forbidden value on uid = " + uid + ", it doesn't respect the following condition : uid < 0");
            bought = reader.ReadBoolean();
        }
        
    }
    
}