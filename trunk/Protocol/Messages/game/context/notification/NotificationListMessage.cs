

// Generated on 10/25/2012 10:42:39
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class NotificationListMessage : NetworkMessage
    {
        public const uint Id = 6087;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int[] flags;
        
        public NotificationListMessage()
        {
        }
        
        public NotificationListMessage(int[] flags)
        {
            this.flags = flags;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)flags.Length);
            foreach (var entry in flags)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            flags = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 flags[i] = reader.ReadInt();
            }
        }
        
    }
    
}