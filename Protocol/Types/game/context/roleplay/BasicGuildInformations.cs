

// Generated on 04/17/2013 22:30:07
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class BasicGuildInformations
    {
        public const short Id = 365;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int guildId;
        public string guildName;
        
        public BasicGuildInformations()
        {
        }
        
        public BasicGuildInformations(int guildId, string guildName)
        {
            this.guildId = guildId;
            this.guildName = guildName;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(guildId);
            writer.WriteUTF(guildName);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            guildId = reader.ReadInt();
            if (guildId < 0)
                throw new Exception("Forbidden value on guildId = " + guildId + ", it doesn't respect the following condition : guildId < 0");
            guildName = reader.ReadUTF();
        }
        
    }
    
}