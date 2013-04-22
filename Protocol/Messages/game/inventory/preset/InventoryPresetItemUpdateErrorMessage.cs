

// Generated on 04/17/2013 22:30:01
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class InventoryPresetItemUpdateErrorMessage : NetworkMessage
    {
        public const uint Id = 6211;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte code;
        
        public InventoryPresetItemUpdateErrorMessage()
        {
        }
        
        public InventoryPresetItemUpdateErrorMessage(sbyte code)
        {
            this.code = code;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(code);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            code = reader.ReadSByte();
            if (code < 0)
                throw new Exception("Forbidden value on code = " + code + ", it doesn't respect the following condition : code < 0");
        }
        
    }
    
}