

// Generated on 09/23/2012 22:26:58
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GuildSpellUpgradeRequestMessage : NetworkMessage
    {
        public const uint Id = 5699;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int spellId;
        
        public GuildSpellUpgradeRequestMessage()
        {
        }
        
        public GuildSpellUpgradeRequestMessage(int spellId)
        {
            this.spellId = spellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(spellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            spellId = reader.ReadInt();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
        }
        
    }
    
}