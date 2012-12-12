

// Generated on 12/11/2012 19:44:15
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameContextReadyMessage : NetworkMessage
    {
        public const uint Id = 6071;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int mapId;
        
        public GameContextReadyMessage()
        {
        }
        
        public GameContextReadyMessage(int mapId)
        {
            this.mapId = mapId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(mapId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            mapId = reader.ReadInt();
            if (mapId < 0)
                throw new Exception("Forbidden value on mapId = " + mapId + ", it doesn't respect the following condition : mapId < 0");
        }
        
    }
    
}