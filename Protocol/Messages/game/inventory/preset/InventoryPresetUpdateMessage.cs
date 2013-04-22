

// Generated on 04/17/2013 22:30:01
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class InventoryPresetUpdateMessage : NetworkMessage
    {
        public const uint Id = 6171;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.Preset preset;
        
        public InventoryPresetUpdateMessage()
        {
        }
        
        public InventoryPresetUpdateMessage(Types.Preset preset)
        {
            this.preset = preset;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            preset.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            preset = new Types.Preset();
            preset.Deserialize(reader);
        }
        
    }
    
}