

// Generated on 04/17/2013 22:29:45
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class EmoteListMessage : NetworkMessage
    {
        public const uint Id = 5689;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte[] emoteIds;
        
        public EmoteListMessage()
        {
        }
        
        public EmoteListMessage(sbyte[] emoteIds)
        {
            this.emoteIds = emoteIds;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)emoteIds.Length);
            foreach (var entry in emoteIds)
            {
                 writer.WriteSByte(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            emoteIds = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 emoteIds[i] = reader.ReadSByte();
            }
        }
        
    }
    
}