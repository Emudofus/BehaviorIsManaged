

// Generated on 04/17/2013 22:29:57
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeLeaveMessage : LeaveDialogMessage
    {
        public const uint Id = 5628;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool success;
        
        public ExchangeLeaveMessage()
        {
        }
        
        public ExchangeLeaveMessage(sbyte dialogType, bool success)
         : base(dialogType)
        {
            this.success = success;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean(success);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            success = reader.ReadBoolean();
        }
        
    }
    
}