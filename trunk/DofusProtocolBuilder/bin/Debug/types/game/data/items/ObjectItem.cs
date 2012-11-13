

// Generated on 10/25/2012 10:42:57
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class ObjectItem : Item
    {
        public const short Id = 37;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public byte position;
        public short objectGID;
        public short powerRate;
        public bool overMax;
        public Types.ObjectEffect[] effects;
        public int objectUID;
        public int quantity;
        
        public ObjectItem()
        {
        }
        
        public ObjectItem(byte position, short objectGID, short powerRate, bool overMax, Types.ObjectEffect[] effects, int objectUID, int quantity)
        {
            this.position = position;
            this.objectGID = objectGID;
            this.powerRate = powerRate;
            this.overMax = overMax;
            this.effects = effects;
            this.objectUID = objectUID;
            this.quantity = quantity;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteByte(position);
            writer.WriteShort(objectGID);
            writer.WriteShort(powerRate);
            writer.WriteBoolean(overMax);
            writer.WriteUShort((ushort)effects.Length);
            foreach (var entry in effects)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            writer.WriteInt(objectUID);
            writer.WriteInt(quantity);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            position = reader.ReadByte();
            if (position < 0 || position > 255)
                throw new Exception("Forbidden value on position = " + position + ", it doesn't respect the following condition : position < 0 || position > 255");
            objectGID = reader.ReadShort();
            if (objectGID < 0)
                throw new Exception("Forbidden value on objectGID = " + objectGID + ", it doesn't respect the following condition : objectGID < 0");
            powerRate = reader.ReadShort();
            overMax = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            effects = new Types.ObjectEffect[limit];
            for (int i = 0; i < limit; i++)
            {
                 effects[i] = Types.ProtocolTypeManager.GetInstance<Types.ObjectEffect>(reader.ReadShort());
                 effects[i].Deserialize(reader);
            }
            objectUID = reader.ReadInt();
            if (objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
            quantity = reader.ReadInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
        }
        
    }
    
}