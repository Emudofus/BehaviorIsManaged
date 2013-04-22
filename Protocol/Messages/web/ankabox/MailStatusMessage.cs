

// Generated on 04/17/2013 22:30:04
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class MailStatusMessage : NetworkMessage
    {
        public const uint Id = 6275;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short unread;
        public short total;
        
        public MailStatusMessage()
        {
        }
        
        public MailStatusMessage(short unread, short total)
        {
            this.unread = unread;
            this.total = total;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(unread);
            writer.WriteShort(total);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            unread = reader.ReadShort();
            if (unread < 0)
                throw new Exception("Forbidden value on unread = " + unread + ", it doesn't respect the following condition : unread < 0");
            total = reader.ReadShort();
            if (total < 0)
                throw new Exception("Forbidden value on total = " + total + ", it doesn't respect the following condition : total < 0");
        }
        
    }
    
}