

// Generated on 10/25/2012 10:42:58
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class HouseInformationsForSell
    {
        public const short Id = 221;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int modelId;
        public string ownerName;
        public bool ownerConnected;
        public short worldX;
        public short worldY;
        public short subAreaId;
        public sbyte nbRoom;
        public sbyte nbChest;
        public int[] skillListIds;
        public bool isLocked;
        public int price;
        
        public HouseInformationsForSell()
        {
        }
        
        public HouseInformationsForSell(int modelId, string ownerName, bool ownerConnected, short worldX, short worldY, short subAreaId, sbyte nbRoom, sbyte nbChest, int[] skillListIds, bool isLocked, int price)
        {
            this.modelId = modelId;
            this.ownerName = ownerName;
            this.ownerConnected = ownerConnected;
            this.worldX = worldX;
            this.worldY = worldY;
            this.subAreaId = subAreaId;
            this.nbRoom = nbRoom;
            this.nbChest = nbChest;
            this.skillListIds = skillListIds;
            this.isLocked = isLocked;
            this.price = price;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(modelId);
            writer.WriteUTF(ownerName);
            writer.WriteBoolean(ownerConnected);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
            writer.WriteShort(subAreaId);
            writer.WriteSByte(nbRoom);
            writer.WriteSByte(nbChest);
            writer.WriteUShort((ushort)skillListIds.Length);
            foreach (var entry in skillListIds)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteBoolean(isLocked);
            writer.WriteInt(price);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            modelId = reader.ReadInt();
            if (modelId < 0)
                throw new Exception("Forbidden value on modelId = " + modelId + ", it doesn't respect the following condition : modelId < 0");
            ownerName = reader.ReadUTF();
            ownerConnected = reader.ReadBoolean();
            worldX = reader.ReadShort();
            if (worldX < -255 || worldX > 255)
                throw new Exception("Forbidden value on worldX = " + worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            worldY = reader.ReadShort();
            if (worldY < -255 || worldY > 255)
                throw new Exception("Forbidden value on worldY = " + worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
            subAreaId = reader.ReadShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            nbRoom = reader.ReadSByte();
            nbChest = reader.ReadSByte();
            var limit = reader.ReadUShort();
            skillListIds = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 skillListIds[i] = reader.ReadInt();
            }
            isLocked = reader.ReadBoolean();
            price = reader.ReadInt();
            if (price < 0)
                throw new Exception("Forbidden value on price = " + price + ", it doesn't respect the following condition : price < 0");
        }
        
    }
    
}