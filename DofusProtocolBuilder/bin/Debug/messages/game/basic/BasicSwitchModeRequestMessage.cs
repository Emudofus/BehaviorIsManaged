

// Generated on 10/25/2012 10:42:35
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class BasicSwitchModeRequestMessage : NetworkMessage
    {
        public const uint Id = 6101;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte mode;
        
        public BasicSwitchModeRequestMessage()
        {
        }
        
        public BasicSwitchModeRequestMessage(sbyte mode)
        {
            this.mode = mode;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(mode);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            mode = reader.ReadSByte();
        }
        
    }
    
}