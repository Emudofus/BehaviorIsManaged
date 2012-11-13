

// Generated on 10/25/2012 10:42:42
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class LockableShowCodeDialogMessage : NetworkMessage
    {
        public const uint Id = 5740;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool changeOrUse;
        public sbyte codeSize;
        
        public LockableShowCodeDialogMessage()
        {
        }
        
        public LockableShowCodeDialogMessage(bool changeOrUse, sbyte codeSize)
        {
            this.changeOrUse = changeOrUse;
            this.codeSize = codeSize;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(changeOrUse);
            writer.WriteSByte(codeSize);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            changeOrUse = reader.ReadBoolean();
            codeSize = reader.ReadSByte();
            if (codeSize < 0)
                throw new Exception("Forbidden value on codeSize = " + codeSize + ", it doesn't respect the following condition : codeSize < 0");
        }
        
    }
    
}