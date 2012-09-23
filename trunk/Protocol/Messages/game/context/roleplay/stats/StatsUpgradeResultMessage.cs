

// Generated on 09/23/2012 22:26:56
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class StatsUpgradeResultMessage : NetworkMessage
    {
        public const uint Id = 5609;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short nbCharacBoost;
        
        public StatsUpgradeResultMessage()
        {
        }
        
        public StatsUpgradeResultMessage(short nbCharacBoost)
        {
            this.nbCharacBoost = nbCharacBoost;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(nbCharacBoost);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            nbCharacBoost = reader.ReadShort();
            if (nbCharacBoost < 0)
                throw new Exception("Forbidden value on nbCharacBoost = " + nbCharacBoost + ", it doesn't respect the following condition : nbCharacBoost < 0");
        }
        
    }
    
}