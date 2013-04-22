

// Generated on 04/17/2013 22:29:42
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class DungeonEnteredMessage : NetworkMessage
    {
        public const uint Id = 6152;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int dungeonId;
        
        public DungeonEnteredMessage()
        {
        }
        
        public DungeonEnteredMessage(int dungeonId)
        {
            this.dungeonId = dungeonId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(dungeonId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            dungeonId = reader.ReadInt();
            if (dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
        }
        
    }
    
}