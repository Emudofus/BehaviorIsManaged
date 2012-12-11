

// Generated on 12/11/2012 19:44:30
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ShortcutBarSwapRequestMessage : NetworkMessage
    {
        public const uint Id = 6230;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte barType;
        public int firstSlot;
        public int secondSlot;
        
        public ShortcutBarSwapRequestMessage()
        {
        }
        
        public ShortcutBarSwapRequestMessage(sbyte barType, int firstSlot, int secondSlot)
        {
            this.barType = barType;
            this.firstSlot = firstSlot;
            this.secondSlot = secondSlot;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(barType);
            writer.WriteInt(firstSlot);
            writer.WriteInt(secondSlot);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            barType = reader.ReadSByte();
            if (barType < 0)
                throw new Exception("Forbidden value on barType = " + barType + ", it doesn't respect the following condition : barType < 0");
            firstSlot = reader.ReadInt();
            if (firstSlot < 0 || firstSlot > 99)
                throw new Exception("Forbidden value on firstSlot = " + firstSlot + ", it doesn't respect the following condition : firstSlot < 0 || firstSlot > 99");
            secondSlot = reader.ReadInt();
            if (secondSlot < 0 || secondSlot > 99)
                throw new Exception("Forbidden value on secondSlot = " + secondSlot + ", it doesn't respect the following condition : secondSlot < 0 || secondSlot > 99");
        }
        
    }
    
}