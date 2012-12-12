

// Generated on 12/11/2012 19:44:16
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ChallengeTargetsListRequestMessage : NetworkMessage
    {
        public const uint Id = 5614;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short challengeId;
        
        public ChallengeTargetsListRequestMessage()
        {
        }
        
        public ChallengeTargetsListRequestMessage(short challengeId)
        {
            this.challengeId = challengeId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(challengeId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            challengeId = reader.ReadShort();
            if (challengeId < 0)
                throw new Exception("Forbidden value on challengeId = " + challengeId + ", it doesn't respect the following condition : challengeId < 0");
        }
        
    }
    
}