

// Generated on 04/17/2013 22:30:06
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class CharacterHardcoreInformations : CharacterBaseInformations
    {
        public const short Id = 86;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public sbyte deathState;
        public short deathCount;
        public byte deathMaxLevel;
        
        public CharacterHardcoreInformations()
        {
        }
        
        public CharacterHardcoreInformations(int id, byte level, string name, Types.EntityLook entityLook, sbyte breed, bool sex, sbyte deathState, short deathCount, byte deathMaxLevel)
         : base(id, level, name, entityLook, breed, sex)
        {
            this.deathState = deathState;
            this.deathCount = deathCount;
            this.deathMaxLevel = deathMaxLevel;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(deathState);
            writer.WriteShort(deathCount);
            writer.WriteByte(deathMaxLevel);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            deathState = reader.ReadSByte();
            if (deathState < 0)
                throw new Exception("Forbidden value on deathState = " + deathState + ", it doesn't respect the following condition : deathState < 0");
            deathCount = reader.ReadShort();
            if (deathCount < 0)
                throw new Exception("Forbidden value on deathCount = " + deathCount + ", it doesn't respect the following condition : deathCount < 0");
            deathMaxLevel = reader.ReadByte();
            if (deathMaxLevel < 1 || deathMaxLevel > 200)
                throw new Exception("Forbidden value on deathMaxLevel = " + deathMaxLevel + ", it doesn't respect the following condition : deathMaxLevel < 1 || deathMaxLevel > 200");
        }
        
    }
    
}