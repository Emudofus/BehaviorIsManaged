

// Generated on 12/11/2012 19:44:10
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class DebugHighlightCellsMessage : NetworkMessage
    {
        public const uint Id = 2001;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int color;
        public short[] cells;
        
        public DebugHighlightCellsMessage()
        {
        }
        
        public DebugHighlightCellsMessage(int color, short[] cells)
        {
            this.color = color;
            this.cells = cells;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(color);
            writer.WriteUShort((ushort)cells.Length);
            foreach (var entry in cells)
            {
                 writer.WriteShort(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            color = reader.ReadInt();
            var limit = reader.ReadUShort();
            cells = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 cells[i] = reader.ReadShort();
            }
        }
        
    }
    
}