

// Generated on 12/11/2012 19:44:12
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class AlmanachCalendarDateMessage : NetworkMessage
    {
        public const uint Id = 6341;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int date;
        
        public AlmanachCalendarDateMessage()
        {
        }
        
        public AlmanachCalendarDateMessage(int date)
        {
            this.date = date;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(date);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            date = reader.ReadInt();
        }
        
    }
    
}