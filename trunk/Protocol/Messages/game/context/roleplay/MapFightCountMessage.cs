

// Generated on 10/25/2012 10:42:40
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class MapFightCountMessage : NetworkMessage
    {
        public const uint Id = 210;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short fightCount;
        
        public MapFightCountMessage()
        {
        }
        
        public MapFightCountMessage(short fightCount)
        {
            this.fightCount = fightCount;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(fightCount);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightCount = reader.ReadShort();
            if (fightCount < 0)
                throw new Exception("Forbidden value on fightCount = " + fightCount + ", it doesn't respect the following condition : fightCount < 0");
        }
        
    }
    
}