

// Generated on 12/11/2012 19:44:27
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangePlayerRequestMessage : ExchangeRequestMessage
    {
        public const uint Id = 5773;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int target;
        
        public ExchangePlayerRequestMessage()
        {
        }
        
        public ExchangePlayerRequestMessage(sbyte exchangeType, int target)
         : base(exchangeType)
        {
            this.target = target;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(target);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            target = reader.ReadInt();
            if (target < 0)
                throw new Exception("Forbidden value on target = " + target + ", it doesn't respect the following condition : target < 0");
        }
        
    }
    
}