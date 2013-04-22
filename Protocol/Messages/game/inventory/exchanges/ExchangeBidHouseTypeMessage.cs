

// Generated on 04/17/2013 22:29:56
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeBidHouseTypeMessage : NetworkMessage
    {
        public const uint Id = 5803;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int type;
        
        public ExchangeBidHouseTypeMessage()
        {
        }
        
        public ExchangeBidHouseTypeMessage(int type)
        {
            this.type = type;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(type);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            type = reader.ReadInt();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
        }
        
    }
    
}