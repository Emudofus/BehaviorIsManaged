

// Generated on 12/11/2012 19:44:14
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ChatServerMessage : ChatAbstractServerMessage
    {
        public const uint Id = 881;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int senderId;
        public string senderName;
        public int senderAccountId;
        
        public ChatServerMessage()
        {
        }
        
        public ChatServerMessage(sbyte channel, string content, int timestamp, string fingerprint, int senderId, string senderName, int senderAccountId)
         : base(channel, content, timestamp, fingerprint)
        {
            this.senderId = senderId;
            this.senderName = senderName;
            this.senderAccountId = senderAccountId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(senderId);
            writer.WriteUTF(senderName);
            writer.WriteInt(senderAccountId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            senderId = reader.ReadInt();
            senderName = reader.ReadUTF();
            senderAccountId = reader.ReadInt();
        }
        
    }
    
}