

// Generated on 04/17/2013 22:29:55
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class StatedMapUpdateMessage : NetworkMessage
    {
        public const uint Id = 5716;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.StatedElement[] statedElements;
        
        public StatedMapUpdateMessage()
        {
        }
        
        public StatedMapUpdateMessage(Types.StatedElement[] statedElements)
        {
            this.statedElements = statedElements;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)statedElements.Length);
            foreach (var entry in statedElements)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            statedElements = new Types.StatedElement[limit];
            for (int i = 0; i < limit; i++)
            {
                 statedElements[i] = new Types.StatedElement();
                 statedElements[i].Deserialize(reader);
            }
        }
        
    }
    
}