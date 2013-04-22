

// Generated on 04/17/2013 22:30:08
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class HumanOptionOrnament : HumanOption
    {
        public const short Id = 411;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short ornamentId;
        
        public HumanOptionOrnament()
        {
        }
        
        public HumanOptionOrnament(short ornamentId)
        {
            this.ornamentId = ornamentId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(ornamentId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            ornamentId = reader.ReadShort();
            if (ornamentId < 0)
                throw new Exception("Forbidden value on ornamentId = " + ornamentId + ", it doesn't respect the following condition : ornamentId < 0");
        }
        
    }
    
}