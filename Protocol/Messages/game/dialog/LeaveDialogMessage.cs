

// Generated on 04/17/2013 22:29:52
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class LeaveDialogMessage : NetworkMessage
    {
        public const uint Id = 5502;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte dialogType;
        
        public LeaveDialogMessage()
        {
        }
        
        public LeaveDialogMessage(sbyte dialogType)
        {
            this.dialogType = dialogType;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(dialogType);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            dialogType = reader.ReadSByte();
            if (dialogType < 0)
                throw new Exception("Forbidden value on dialogType = " + dialogType + ", it doesn't respect the following condition : dialogType < 0");
        }
        
    }
    
}