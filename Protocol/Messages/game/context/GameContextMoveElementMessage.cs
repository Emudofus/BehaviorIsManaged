

// Generated on 04/17/2013 22:29:41
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameContextMoveElementMessage : NetworkMessage
    {
        public const uint Id = 253;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.EntityMovementInformations movement;
        
        public GameContextMoveElementMessage()
        {
        }
        
        public GameContextMoveElementMessage(Types.EntityMovementInformations movement)
        {
            this.movement = movement;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            movement.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            movement = new Types.EntityMovementInformations();
            movement.Deserialize(reader);
        }
        
    }
    
}