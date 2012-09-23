

// Generated on 09/23/2012 22:27:00
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeCraftResultMessage : NetworkMessage
    {
        public const uint Id = 5790;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte craftResult;
        
        public ExchangeCraftResultMessage()
        {
        }
        
        public ExchangeCraftResultMessage(sbyte craftResult)
        {
            this.craftResult = craftResult;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(craftResult);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            craftResult = reader.ReadSByte();
            if (craftResult < 0)
                throw new Exception("Forbidden value on craftResult = " + craftResult + ", it doesn't respect the following condition : craftResult < 0");
        }
        
    }
    
}