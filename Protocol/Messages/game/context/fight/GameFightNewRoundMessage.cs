

// Generated on 12/11/2012 19:44:16
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameFightNewRoundMessage : NetworkMessage
    {
        public const uint Id = 6239;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int roundNumber;
        
        public GameFightNewRoundMessage()
        {
        }
        
        public GameFightNewRoundMessage(int roundNumber)
        {
            this.roundNumber = roundNumber;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(roundNumber);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            roundNumber = reader.ReadInt();
            if (roundNumber < 0)
                throw new Exception("Forbidden value on roundNumber = " + roundNumber + ", it doesn't respect the following condition : roundNumber < 0");
        }
        
    }
    
}