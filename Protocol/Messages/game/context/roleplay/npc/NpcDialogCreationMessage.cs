

// Generated on 04/17/2013 22:29:48
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class NpcDialogCreationMessage : NetworkMessage
    {
        public const uint Id = 5618;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int mapId;
        public int npcId;
        
        public NpcDialogCreationMessage()
        {
        }
        
        public NpcDialogCreationMessage(int mapId, int npcId)
        {
            this.mapId = mapId;
            this.npcId = npcId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(mapId);
            writer.WriteInt(npcId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            mapId = reader.ReadInt();
            npcId = reader.ReadInt();
        }
        
    }
    
}