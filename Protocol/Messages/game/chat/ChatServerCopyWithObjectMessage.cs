

// Generated on 04/17/2013 22:29:40
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ChatServerCopyWithObjectMessage : ChatServerCopyMessage
    {
        public const uint Id = 884;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ObjectItem[] objects;
        
        public ChatServerCopyWithObjectMessage()
        {
        }
        
        public ChatServerCopyWithObjectMessage(sbyte channel, string content, int timestamp, string fingerprint, int receiverId, string receiverName, Types.ObjectItem[] objects)
         : base(channel, content, timestamp, fingerprint, receiverId, receiverName)
        {
            this.objects = objects;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)objects.Length);
            foreach (var entry in objects)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            objects = new Types.ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 objects[i] = new Types.ObjectItem();
                 objects[i].Deserialize(reader);
            }
        }
        
    }
    
}