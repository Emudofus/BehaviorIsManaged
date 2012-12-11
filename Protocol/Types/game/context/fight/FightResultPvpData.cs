

// Generated on 12/11/2012 19:44:33
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class FightResultPvpData : FightResultAdditionalData
    {
        public const short Id = 190;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public byte grade;
        public ushort minHonorForGrade;
        public ushort maxHonorForGrade;
        public ushort honor;
        public short honorDelta;
        public ushort dishonor;
        public short dishonorDelta;
        
        public FightResultPvpData()
        {
        }
        
        public FightResultPvpData(byte grade, ushort minHonorForGrade, ushort maxHonorForGrade, ushort honor, short honorDelta, ushort dishonor, short dishonorDelta)
        {
            this.grade = grade;
            this.minHonorForGrade = minHonorForGrade;
            this.maxHonorForGrade = maxHonorForGrade;
            this.honor = honor;
            this.honorDelta = honorDelta;
            this.dishonor = dishonor;
            this.dishonorDelta = dishonorDelta;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteByte(grade);
            writer.WriteUShort(minHonorForGrade);
            writer.WriteUShort(maxHonorForGrade);
            writer.WriteUShort(honor);
            writer.WriteShort(honorDelta);
            writer.WriteUShort(dishonor);
            writer.WriteShort(dishonorDelta);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            grade = reader.ReadByte();
            if (grade < 0 || grade > 255)
                throw new Exception("Forbidden value on grade = " + grade + ", it doesn't respect the following condition : grade < 0 || grade > 255");
            minHonorForGrade = reader.ReadUShort();
            if (minHonorForGrade < 0 || minHonorForGrade > 20000)
                throw new Exception("Forbidden value on minHonorForGrade = " + minHonorForGrade + ", it doesn't respect the following condition : minHonorForGrade < 0 || minHonorForGrade > 20000");
            maxHonorForGrade = reader.ReadUShort();
            if (maxHonorForGrade < 0 || maxHonorForGrade > 20000)
                throw new Exception("Forbidden value on maxHonorForGrade = " + maxHonorForGrade + ", it doesn't respect the following condition : maxHonorForGrade < 0 || maxHonorForGrade > 20000");
            honor = reader.ReadUShort();
            if (honor < 0 || honor > 20000)
                throw new Exception("Forbidden value on honor = " + honor + ", it doesn't respect the following condition : honor < 0 || honor > 20000");
            honorDelta = reader.ReadShort();
            dishonor = reader.ReadUShort();
            if (dishonor < 0 || dishonor > 500)
                throw new Exception("Forbidden value on dishonor = " + dishonor + ", it doesn't respect the following condition : dishonor < 0 || dishonor > 500");
            dishonorDelta = reader.ReadShort();
        }
        
    }
    
}