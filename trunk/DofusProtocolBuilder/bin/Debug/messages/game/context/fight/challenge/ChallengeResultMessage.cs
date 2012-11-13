

// Generated on 10/25/2012 10:42:38
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ChallengeResultMessage : NetworkMessage
    {
        public const uint Id = 6019;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte challengeId;
        public bool success;
        
        public ChallengeResultMessage()
        {
        }
        
        public ChallengeResultMessage(sbyte challengeId, bool success)
        {
            this.challengeId = challengeId;
            this.success = success;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(challengeId);
            writer.WriteBoolean(success);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            challengeId = reader.ReadSByte();
            if (challengeId < 0)
                throw new Exception("Forbidden value on challengeId = " + challengeId + ", it doesn't respect the following condition : challengeId < 0");
            success = reader.ReadBoolean();
        }
        
    }
    
}