

// Generated on 12/11/2012 19:44:24
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class TaxCollectorAttackedMessage : NetworkMessage
    {
        public const uint Id = 5918;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short firstNameId;
        public short lastNameId;
        public short worldX;
        public short worldY;
        public int mapId;
        public short subAreaId;
        
        public TaxCollectorAttackedMessage()
        {
        }
        
        public TaxCollectorAttackedMessage(short firstNameId, short lastNameId, short worldX, short worldY, int mapId, short subAreaId)
        {
            this.firstNameId = firstNameId;
            this.lastNameId = lastNameId;
            this.worldX = worldX;
            this.worldY = worldY;
            this.mapId = mapId;
            this.subAreaId = subAreaId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(firstNameId);
            writer.WriteShort(lastNameId);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
            writer.WriteInt(mapId);
            writer.WriteShort(subAreaId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            firstNameId = reader.ReadShort();
            if (firstNameId < 0)
                throw new Exception("Forbidden value on firstNameId = " + firstNameId + ", it doesn't respect the following condition : firstNameId < 0");
            lastNameId = reader.ReadShort();
            if (lastNameId < 0)
                throw new Exception("Forbidden value on lastNameId = " + lastNameId + ", it doesn't respect the following condition : lastNameId < 0");
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
        }
        
    }
    
}