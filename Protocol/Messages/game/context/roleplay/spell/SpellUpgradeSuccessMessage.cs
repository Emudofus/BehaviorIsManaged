

// Generated on 12/11/2012 19:44:22
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class SpellUpgradeSuccessMessage : NetworkMessage
    {
        public const uint Id = 1201;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int spellId;
        public sbyte spellLevel;
        
        public SpellUpgradeSuccessMessage()
        {
        }
        
        public SpellUpgradeSuccessMessage(int spellId, sbyte spellLevel)
        {
            this.spellId = spellId;
            this.spellLevel = spellLevel;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(spellId);
            writer.WriteSByte(spellLevel);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            spellId = reader.ReadInt();
            spellLevel = reader.ReadSByte();
            if (spellLevel < 1 || spellLevel > 6)
                throw new Exception("Forbidden value on spellLevel = " + spellLevel + ", it doesn't respect the following condition : spellLevel < 1 || spellLevel > 6");
        }
        
    }
    
}