

// Generated on 12/11/2012 19:44:30
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ShortcutBarRemovedMessage : NetworkMessage
    {
        public const uint Id = 6224;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte barType;
        public int slot;
        
        public ShortcutBarRemovedMessage()
        {
        }
        
        public ShortcutBarRemovedMessage(sbyte barType, int slot)
        {
            this.barType = barType;
            this.slot = slot;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(barType);
            writer.WriteInt(slot);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            barType = reader.ReadSByte();
            if (barType < 0)
                throw new Exception("Forbidden value on barType = " + barType + ", it doesn't respect the following condition : barType < 0");
            slot = reader.ReadInt();
            if (slot < 0 || slot > 99)
                throw new Exception("Forbidden value on slot = " + slot + ", it doesn't respect the following condition : slot < 0 || slot > 99");
        }
        
    }
    
}