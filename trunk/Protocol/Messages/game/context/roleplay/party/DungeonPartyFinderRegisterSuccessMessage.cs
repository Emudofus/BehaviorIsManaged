

// Generated on 09/23/2012 22:26:54
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class DungeonPartyFinderRegisterSuccessMessage : NetworkMessage
    {
        public const uint Id = 6241;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short[] dungeonIds;
        
        public DungeonPartyFinderRegisterSuccessMessage()
        {
        }
        
        public DungeonPartyFinderRegisterSuccessMessage(short[] dungeonIds)
        {
            this.dungeonIds = dungeonIds;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)dungeonIds.Length);
            foreach (var entry in dungeonIds)
            {
                 writer.WriteShort(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            dungeonIds = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 dungeonIds[i] = reader.ReadShort();
            }
        }
        
    }
    
}