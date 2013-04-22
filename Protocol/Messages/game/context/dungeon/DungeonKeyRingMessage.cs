

// Generated on 04/17/2013 22:29:42
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class DungeonKeyRingMessage : NetworkMessage
    {
        public const uint Id = 6299;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short[] availables;
        public short[] unavailables;
        
        public DungeonKeyRingMessage()
        {
        }
        
        public DungeonKeyRingMessage(short[] availables, short[] unavailables)
        {
            this.availables = availables;
            this.unavailables = unavailables;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)availables.Length);
            foreach (var entry in availables)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)unavailables.Length);
            foreach (var entry in unavailables)
            {
                 writer.WriteShort(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            availables = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 availables[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            unavailables = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 unavailables[i] = reader.ReadShort();
            }
        }
        
    }
    
}