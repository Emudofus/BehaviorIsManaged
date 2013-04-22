

// Generated on 04/17/2013 22:30:06
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class MapCoordinatesExtended : MapCoordinatesAndId
    {
        public const short Id = 176;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short subAreaId;
        
        public MapCoordinatesExtended()
        {
        }
        
        public MapCoordinatesExtended(short worldX, short worldY, int mapId, short subAreaId)
         : base(worldX, worldY, mapId)
        {
            this.subAreaId = subAreaId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(subAreaId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            subAreaId = reader.ReadShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
        }
        
    }
    
}