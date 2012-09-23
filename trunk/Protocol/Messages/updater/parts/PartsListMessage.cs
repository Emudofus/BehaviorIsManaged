

// Generated on 09/23/2012 22:27:06
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PartsListMessage : NetworkMessage
    {
        public const uint Id = 1502;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ContentPart[] parts;
        
        public PartsListMessage()
        {
        }
        
        public PartsListMessage(Types.ContentPart[] parts)
        {
            this.parts = parts;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)parts.Length);
            foreach (var entry in parts)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            parts = new Types.ContentPart[limit];
            for (int i = 0; i < limit; i++)
            {
                 parts[i] = new Types.ContentPart();
                 parts[i].Deserialize(reader);
            }
        }
        
    }
    
}