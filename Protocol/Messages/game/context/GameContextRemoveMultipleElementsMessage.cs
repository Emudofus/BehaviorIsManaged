

// Generated on 04/17/2013 22:29:41
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameContextRemoveMultipleElementsMessage : NetworkMessage
    {
        public const uint Id = 252;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int[] id;
        
        public GameContextRemoveMultipleElementsMessage()
        {
        }
        
        public GameContextRemoveMultipleElementsMessage(int[] id)
        {
            this.id = id;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)id.Length);
            foreach (var entry in id)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            id = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 id[i] = reader.ReadInt();
            }
        }
        
    }
    
}