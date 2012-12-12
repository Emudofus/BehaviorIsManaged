

// Generated on 12/11/2012 19:44:16
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameFightTurnResumeMessage : GameFightTurnStartMessage
    {
        public const uint Id = 6307;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GameFightTurnResumeMessage()
        {
        }
        
        public GameFightTurnResumeMessage(int id, int waitTime)
         : base(id, waitTime)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
    }
    
}