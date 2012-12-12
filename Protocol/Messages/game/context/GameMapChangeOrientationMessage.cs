

// Generated on 12/11/2012 19:44:15
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameMapChangeOrientationMessage : NetworkMessage
    {
        public const uint Id = 946;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ActorOrientation orientation;
        
        public GameMapChangeOrientationMessage()
        {
        }
        
        public GameMapChangeOrientationMessage(Types.ActorOrientation orientation)
        {
            this.orientation = orientation;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            orientation.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            orientation = new Types.ActorOrientation();
            orientation.Deserialize(reader);
        }
        
    }
    
}