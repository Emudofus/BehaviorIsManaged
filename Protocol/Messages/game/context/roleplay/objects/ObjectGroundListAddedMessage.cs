

// Generated on 04/17/2013 22:29:48
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ObjectGroundListAddedMessage : NetworkMessage
    {
        public const uint Id = 5925;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short[] cells;
        public int[] referenceIds;
        
        public ObjectGroundListAddedMessage()
        {
        }
        
        public ObjectGroundListAddedMessage(short[] cells, int[] referenceIds)
        {
            this.cells = cells;
            this.referenceIds = referenceIds;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)cells.Length);
            foreach (var entry in cells)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)referenceIds.Length);
            foreach (var entry in referenceIds)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            cells = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 cells[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            referenceIds = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 referenceIds[i] = reader.ReadInt();
            }
        }
        
    }
    
}