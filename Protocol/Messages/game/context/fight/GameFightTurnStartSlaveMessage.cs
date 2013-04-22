

// Generated on 04/17/2013 22:29:43
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameFightTurnStartSlaveMessage : GameFightTurnStartMessage
    {
        public const uint Id = 6213;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int idSummoner;
        
        public GameFightTurnStartSlaveMessage()
        {
        }
        
        public GameFightTurnStartSlaveMessage(int id, int waitTime, int idSummoner)
         : base(id, waitTime)
        {
            this.idSummoner = idSummoner;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(idSummoner);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            idSummoner = reader.ReadInt();
        }
        
    }
    
}