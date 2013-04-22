

// Generated on 04/17/2013 22:29:41
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameContextKickMessage : NetworkMessage
    {
        public const uint Id = 6081;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int targetId;
        
        public GameContextKickMessage()
        {
        }
        
        public GameContextKickMessage(int targetId)
        {
            this.targetId = targetId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(targetId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            targetId = reader.ReadInt();
        }
        
    }
    
}