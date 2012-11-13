

// Generated on 10/25/2012 10:42:42
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class DungeonPartyFinderListenRequestMessage : NetworkMessage
    {
        public const uint Id = 6246;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short dungeonId;
        
        public DungeonPartyFinderListenRequestMessage()
        {
        }
        
        public DungeonPartyFinderListenRequestMessage(short dungeonId)
        {
            this.dungeonId = dungeonId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(dungeonId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            dungeonId = reader.ReadShort();
            if (dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
        }
        
    }
    
}