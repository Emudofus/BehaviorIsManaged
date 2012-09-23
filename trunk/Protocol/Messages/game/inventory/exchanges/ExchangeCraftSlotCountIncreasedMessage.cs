

// Generated on 09/23/2012 22:27:00
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeCraftSlotCountIncreasedMessage : NetworkMessage
    {
        public const uint Id = 6125;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte newMaxSlot;
        
        public ExchangeCraftSlotCountIncreasedMessage()
        {
        }
        
        public ExchangeCraftSlotCountIncreasedMessage(sbyte newMaxSlot)
        {
            this.newMaxSlot = newMaxSlot;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(newMaxSlot);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            newMaxSlot = reader.ReadSByte();
            if (newMaxSlot < 0)
                throw new Exception("Forbidden value on newMaxSlot = " + newMaxSlot + ", it doesn't respect the following condition : newMaxSlot < 0");
        }
        
    }
    
}