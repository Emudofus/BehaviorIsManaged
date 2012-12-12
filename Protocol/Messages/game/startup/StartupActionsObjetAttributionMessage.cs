

// Generated on 12/11/2012 19:44:30
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class StartupActionsObjetAttributionMessage : NetworkMessage
    {
        public const uint Id = 1303;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int actionId;
        public int characterId;
        
        public StartupActionsObjetAttributionMessage()
        {
        }
        
        public StartupActionsObjetAttributionMessage(int actionId, int characterId)
        {
            this.actionId = actionId;
            this.characterId = characterId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(actionId);
            writer.WriteInt(characterId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            actionId = reader.ReadInt();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
            characterId = reader.ReadInt();
            if (characterId < 0)
                throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0");
        }
        
    }
    
}