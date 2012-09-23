

// Generated on 09/23/2012 22:26:49
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameContextRefreshEntityLookMessage : NetworkMessage
    {
        public const uint Id = 5637;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int id;
        public Types.EntityLook look;
        
        public GameContextRefreshEntityLookMessage()
        {
        }
        
        public GameContextRefreshEntityLookMessage(int id, Types.EntityLook look)
        {
            this.id = id;
            this.look = look;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(id);
            look.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            id = reader.ReadInt();
            look = new Types.EntityLook();
            look.Deserialize(reader);
        }
        
    }
    
}