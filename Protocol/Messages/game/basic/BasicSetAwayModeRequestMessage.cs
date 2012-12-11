

// Generated on 12/11/2012 19:44:13
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class BasicSetAwayModeRequestMessage : NetworkMessage
    {
        public const uint Id = 5665;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool enable;
        public bool invisible;
        
        public BasicSetAwayModeRequestMessage()
        {
        }
        
        public BasicSetAwayModeRequestMessage(bool enable, bool invisible)
        {
            this.enable = enable;
            this.invisible = invisible;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, enable);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, invisible);
            writer.WriteByte(flag1);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            byte flag1 = reader.ReadByte();
            enable = BooleanByteWrapper.GetFlag(flag1, 0);
            invisible = BooleanByteWrapper.GetFlag(flag1, 1);
        }
        
    }
    
}