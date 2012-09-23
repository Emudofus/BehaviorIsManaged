

// Generated on 09/23/2012 22:27:04
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class InventoryPresetUseMessage : NetworkMessage
    {
        public const uint Id = 6167;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte presetId;
        
        public InventoryPresetUseMessage()
        {
        }
        
        public InventoryPresetUseMessage(sbyte presetId)
        {
            this.presetId = presetId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(presetId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            presetId = reader.ReadSByte();
            if (presetId < 0)
                throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
        }
        
    }
    
}