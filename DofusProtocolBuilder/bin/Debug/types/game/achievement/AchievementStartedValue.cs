

// Generated on 10/25/2012 10:42:55
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class AchievementStartedValue : Achievement
    {
        public const short Id = 361;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short value;
        public short maxValue;
        
        public AchievementStartedValue()
        {
        }
        
        public AchievementStartedValue(short id, short value, short maxValue)
         : base(id)
        {
            this.value = value;
            this.maxValue = maxValue;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(value);
            writer.WriteShort(maxValue);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            value = reader.ReadShort();
            if (value < 0)
                throw new Exception("Forbidden value on value = " + value + ", it doesn't respect the following condition : value < 0");
            maxValue = reader.ReadShort();
            if (maxValue < 0)
                throw new Exception("Forbidden value on maxValue = " + maxValue + ", it doesn't respect the following condition : maxValue < 0");
        }
        
    }
    
}