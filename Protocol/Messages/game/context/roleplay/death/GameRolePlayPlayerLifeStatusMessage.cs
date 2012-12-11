

// Generated on 12/11/2012 19:44:18
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameRolePlayPlayerLifeStatusMessage : NetworkMessage
    {
        public const uint Id = 5996;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte state;
        
        public GameRolePlayPlayerLifeStatusMessage()
        {
        }
        
        public GameRolePlayPlayerLifeStatusMessage(sbyte state)
        {
            this.state = state;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(state);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            state = reader.ReadSByte();
            if (state < 0)
                throw new Exception("Forbidden value on state = " + state + ", it doesn't respect the following condition : state < 0");
        }
        
    }
    
}