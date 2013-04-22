

// Generated on 04/17/2013 22:30:10
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class UpdateMountIntBoost : UpdateMountBoost
    {
        public const short Id = 357;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int value;
        
        public UpdateMountIntBoost()
        {
        }
        
        public UpdateMountIntBoost(sbyte type, int value)
         : base(type)
        {
            this.value = value;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(value);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            value = reader.ReadInt();
        }
        
    }
    
}