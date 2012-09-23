

// Generated on 09/23/2012 22:27:05
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class AlignmentSubAreaUpdateExtendedMessage : AlignmentSubAreaUpdateMessage
    {
        public const uint Id = 6319;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short worldX;
        public short worldY;
        public int mapId;
        public sbyte eventType;
        
        public AlignmentSubAreaUpdateExtendedMessage()
        {
        }
        
        public AlignmentSubAreaUpdateExtendedMessage(short subAreaId, sbyte side, bool quiet, short worldX, short worldY, int mapId, sbyte eventType)
         : base(subAreaId, side, quiet)
        {
            this.worldX = worldX;
            this.worldY = worldY;
            this.mapId = mapId;
            this.eventType = eventType;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
            writer.WriteInt(mapId);
            writer.WriteSByte(eventType);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            worldX = reader.ReadShort();
            if (worldX < -255 || worldX > 255)
                throw new Exception("Forbidden value on worldX = " + worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            worldY = reader.ReadShort();
            if (worldY < -255 || worldY > 255)
                throw new Exception("Forbidden value on worldY = " + worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
            mapId = reader.ReadInt();
            eventType = reader.ReadSByte();
        }
        
    }
    
}