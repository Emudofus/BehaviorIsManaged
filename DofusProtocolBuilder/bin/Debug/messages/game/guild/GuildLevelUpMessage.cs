

// Generated on 10/25/2012 10:42:46
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GuildLevelUpMessage : NetworkMessage
    {
        public const uint Id = 6062;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public byte newLevel;
        
        public GuildLevelUpMessage()
        {
        }
        
        public GuildLevelUpMessage(byte newLevel)
        {
            this.newLevel = newLevel;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteByte(newLevel);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            newLevel = reader.ReadByte();
            if (newLevel < 2 || newLevel > 200)
                throw new Exception("Forbidden value on newLevel = " + newLevel + ", it doesn't respect the following condition : newLevel < 2 || newLevel > 200");
        }
        
    }
    
}