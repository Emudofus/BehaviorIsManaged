

// Generated on 12/11/2012 19:44:15
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameContextRemoveElementMessage : NetworkMessage
    {
        public const uint Id = 251;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int id;
        
        public GameContextRemoveElementMessage()
        {
        }
        
        public GameContextRemoveElementMessage(int id)
        {
            this.id = id;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(id);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            id = reader.ReadInt();
        }
        
    }
    
}