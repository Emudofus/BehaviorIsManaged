

// Generated on 12/11/2012 19:44:30
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ShortcutBarAddErrorMessage : NetworkMessage
    {
        public const uint Id = 6227;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte error;
        
        public ShortcutBarAddErrorMessage()
        {
        }
        
        public ShortcutBarAddErrorMessage(sbyte error)
        {
            this.error = error;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(error);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            error = reader.ReadSByte();
            if (error < 0)
                throw new Exception("Forbidden value on error = " + error + ", it doesn't respect the following condition : error < 0");
        }
        
    }
    
}