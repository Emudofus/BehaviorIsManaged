

// Generated on 09/23/2012 22:27:10
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class ObjectItemMinimalInformation : Item
    {
        public const short Id = 124;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short objectGID;
        public short powerRate;
        public bool overMax;
        public Types.ObjectEffect[] effects;
        
        public ObjectItemMinimalInformation()
        {
        }
        
        public ObjectItemMinimalInformation(short objectGID, short powerRate, bool overMax, Types.ObjectEffect[] effects)
        {
            this.objectGID = objectGID;
            this.powerRate = powerRate;
            this.overMax = overMax;
            this.effects = effects;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(objectGID);
            writer.WriteShort(powerRate);
            writer.WriteBoolean(overMax);
            writer.WriteUShort((ushort)effects.Length);
            foreach (var entry in effects)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
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
        }
        
    }
    
}