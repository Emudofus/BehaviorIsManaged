

// Generated on 09/23/2012 22:26:51
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class MapObstacleUpdateMessage : NetworkMessage
    {
        public const uint Id = 6051;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.MapObstacle[] obstacles;
        
        public MapObstacleUpdateMessage()
        {
        }
        
        public MapObstacleUpdateMessage(Types.MapObstacle[] obstacles)
        {
            this.obstacles = obstacles;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)obstacles.Length);
            foreach (var entry in obstacles)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            obstacles = new Types.MapObstacle[limit];
            for (int i = 0; i < limit; i++)
            {
                 obstacles[i] = new Types.MapObstacle();
                 obstacles[i].Deserialize(reader);
            }
        }
        
    }
    
}