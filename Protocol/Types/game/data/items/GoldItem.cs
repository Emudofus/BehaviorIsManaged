

// Generated on 04/17/2013 22:30:08
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class GoldItem : Item
    {
        public const short Id = 123;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int sum;
        
        public GoldItem()
        {
        }
        
        public GoldItem(int sum)
        {
            this.sum = sum;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(sum);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            sum = reader.ReadInt();
            if (sum < 0)
                throw new Exception("Forbidden value on sum = " + sum + ", it doesn't respect the following condition : sum < 0");
        }
        
    }
    
}