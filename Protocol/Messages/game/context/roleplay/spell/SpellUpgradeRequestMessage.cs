

// Generated on 12/11/2012 19:44:22
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class SpellUpgradeRequestMessage : NetworkMessage
    {
        public const uint Id = 5608;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short spellId;
        
        public SpellUpgradeRequestMessage()
        {
        }
        
        public SpellUpgradeRequestMessage(short spellId)
        {
            this.spellId = spellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(spellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            spellId = reader.ReadShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
        }
        
    }
    
}