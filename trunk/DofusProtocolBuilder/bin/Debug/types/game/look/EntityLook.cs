

// Generated on 10/25/2012 10:42:58
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class EntityLook
    {
        public const short Id = 55;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short bonesId;
        public short[] skins;
        public int[] indexedColors;
        public short[] scales;
        public Types.SubEntity[] subentities;
        
        public EntityLook()
        {
        }
        
        public EntityLook(short bonesId, short[] skins, int[] indexedColors, short[] scales, Types.SubEntity[] subentities)
        {
            this.bonesId = bonesId;
            this.skins = skins;
            this.indexedColors = indexedColors;
            this.scales = scales;
            this.subentities = subentities;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteShort(bonesId);
            writer.WriteUShort((ushort)skins.Length);
            foreach (var entry in skins)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)indexedColors.Length);
            foreach (var entry in indexedColors)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)scales.Length);
            foreach (var entry in scales)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)subentities.Length);
            foreach (var entry in subentities)
            {
                 entry.Serialize(writer);
            }
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            bonesId = reader.ReadShort();
            if (bonesId < 0)
                throw new Exception("Forbidden value on bonesId = " + bonesId + ", it doesn't respect the following condition : bonesId < 0");
            var limit = reader.ReadUShort();
            skins = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 skins[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            indexedColors = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 indexedColors[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            scales = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 scales[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            subentities = new Types.SubEntity[limit];
            for (int i = 0; i < limit; i++)
            {
                 subentities[i] = new Types.SubEntity();
                 subentities[i].Deserialize(reader);
            }
        }
        
    }
    
}