

// Generated on 12/11/2012 19:44:16
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameFightStartingMessage : NetworkMessage
    {
        public const uint Id = 700;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte fightType;
        
        public GameFightStartingMessage()
        {
        }
        
        public GameFightStartingMessage(sbyte fightType)
        {
            this.fightType = fightType;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(fightType);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightType = reader.ReadSByte();
            if (fightType < 0)
                throw new Exception("Forbidden value on fightType = " + fightType + ", it doesn't respect the following condition : fightType < 0");
        }
        
    }
    
}