

// Generated on 12/11/2012 19:44:30
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PrismFightAttackedMessage : NetworkMessage
    {
        public const uint Id = 5894;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short worldX;
        public short worldY;
        public int mapId;
        public short subAreaId;
        public sbyte prismSide;
        
        public PrismFightAttackedMessage()
        {
        }
        
        public PrismFightAttackedMessage(short worldX, short worldY, int mapId, short subAreaId, sbyte prismSide)
        {
            this.worldX = worldX;
            this.worldY = worldY;
            this.mapId = mapId;
            this.subAreaId = subAreaId;
            this.prismSide = prismSide;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
            writer.WriteInt(mapId);
            writer.WriteShort(subAreaId);
            writer.WriteSByte(prismSide);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            worldX = reader.ReadShort();
            if (worldX < -255 || worldX > 255)
                throw new Exception("Forbidden value on worldX = " + worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            worldY = reader.ReadShort();
            if (worldY < -255 || worldY > 255)
                throw new Exception("Forbidden value on worldY = " + worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
            mapId = reader.ReadInt();
            subAreaId = reader.ReadShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            prismSide = reader.ReadSByte();
        }
        
    }
    
}