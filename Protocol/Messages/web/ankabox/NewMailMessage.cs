

// Generated on 04/17/2013 22:30:04
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class NewMailMessage : MailStatusMessage
    {
        public const uint Id = 6292;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int[] sendersAccountId;
        
        public NewMailMessage()
        {
        }
        
        public NewMailMessage(short unread, short total, int[] sendersAccountId)
         : base(unread, total)
        {
            this.sendersAccountId = sendersAccountId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)sendersAccountId.Length);
            foreach (var entry in sendersAccountId)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            sendersAccountId = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 sendersAccountId[i] = reader.ReadInt();
            }
        }
        
    }
    
}