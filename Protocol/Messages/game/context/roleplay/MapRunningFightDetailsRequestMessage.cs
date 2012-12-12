

// Generated on 12/11/2012 19:44:18
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class MapRunningFightDetailsRequestMessage : NetworkMessage
    {
        public const uint Id = 5750;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int fightId;
        
        public MapRunningFightDetailsRequestMessage()
        {
        }
        
        public MapRunningFightDetailsRequestMessage(int fightId)
        {
            this.fightId = fightId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(fightId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadInt();
            if (fightId < 0)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
        }
        
    }
    
}