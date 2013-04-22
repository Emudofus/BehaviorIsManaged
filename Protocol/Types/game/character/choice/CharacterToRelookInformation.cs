

// Generated on 04/17/2013 22:30:06
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class CharacterToRelookInformation : AbstractCharacterInformation
    {
        public const short Id = 399;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int cosmeticId;
        
        public CharacterToRelookInformation()
        {
        }
        
        public CharacterToRelookInformation(int id, int cosmeticId)
         : base(id)
        {
            this.cosmeticId = cosmeticId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(cosmeticId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            cosmeticId = reader.ReadInt();
            if (cosmeticId < 0)
                throw new Exception("Forbidden value on cosmeticId = " + cosmeticId + ", it doesn't respect the following condition : cosmeticId < 0");
        }
        
    }
    
}