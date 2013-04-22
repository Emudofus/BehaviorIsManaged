

// Generated on 04/17/2013 22:30:07
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class GameFightFighterLightInformations
    {
        public const short Id = 413;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public bool sex;
        public bool alive;
        public int id;
        public string name;
        public short level;
        public sbyte breed;
        
        public GameFightFighterLightInformations()
        {
        }
        
        public GameFightFighterLightInformations(bool sex, bool alive, int id, string name, short level, sbyte breed)
        {
            this.sex = sex;
            this.alive = alive;
            this.id = id;
            this.name = name;
            this.level = level;
            this.breed = breed;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, sex);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, alive);
            writer.WriteByte(flag1);
            writer.WriteInt(id);
            writer.WriteUTF(name);
            writer.WriteShort(level);
            writer.WriteSByte(breed);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            byte flag1 = reader.ReadByte();
            sex = BooleanByteWrapper.GetFlag(flag1, 0);
            alive = BooleanByteWrapper.GetFlag(flag1, 1);
            id = reader.ReadInt();
            name = reader.ReadUTF();
            level = reader.ReadShort();
            if (level < 0)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0");
            breed = reader.ReadSByte();
        }
        
    }
    
}