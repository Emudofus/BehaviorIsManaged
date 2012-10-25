

// Generated on 10/25/2012 10:42:36
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class CharacterCreationResultMessage : NetworkMessage
    {
        public const uint Id = 161;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte result;
        
        public CharacterCreationResultMessage()
        {
        }
        
        public CharacterCreationResultMessage(sbyte result)
        {
            this.result = result;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(result);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            result = reader.ReadSByte();
            if (result < 0)
                throw new Exception("Forbidden value on result = " + result + ", it doesn't respect the following condition : result < 0");
        }
        
    }
    
}