

// Generated on 12/11/2012 19:44:34
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class MonsterInGroupLightInformations
    {
        public const short Id = 395;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int creatureGenericId;
        public sbyte grade;
        
        public MonsterInGroupLightInformations()
        {
        }
        
        public MonsterInGroupLightInformations(int creatureGenericId, sbyte grade)
        {
            this.creatureGenericId = creatureGenericId;
            this.grade = grade;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(creatureGenericId);
            writer.WriteSByte(grade);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            creatureGenericId = reader.ReadInt();
            grade = reader.ReadSByte();
            if (grade < 0)
                throw new Exception("Forbidden value on grade = " + grade + ", it doesn't respect the following condition : grade < 0");
        }
        
    }
    
}