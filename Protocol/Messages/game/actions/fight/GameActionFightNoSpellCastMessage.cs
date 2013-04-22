

// Generated on 04/17/2013 22:29:36
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameActionFightNoSpellCastMessage : NetworkMessage
    {
        public const uint Id = 6132;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int spellLevelId;
        
        public GameActionFightNoSpellCastMessage()
        {
        }
        
        public GameActionFightNoSpellCastMessage(int spellLevelId)
        {
            this.spellLevelId = spellLevelId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(spellLevelId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            spellLevelId = reader.ReadInt();
            if (spellLevelId < 0)
                throw new Exception("Forbidden value on spellLevelId = " + spellLevelId + ", it doesn't respect the following condition : spellLevelId < 0");
        }
        
    }
    
}