

// Generated on 09/23/2012 22:26:59
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class SetCharacterRestrictionsMessage : NetworkMessage
    {
        public const uint Id = 170;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ActorRestrictionsInformations restrictions;
        
        public SetCharacterRestrictionsMessage()
        {
        }
        
        public SetCharacterRestrictionsMessage(Types.ActorRestrictionsInformations restrictions)
        {
            this.restrictions = restrictions;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            restrictions.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            restrictions = new Types.ActorRestrictionsInformations();
            restrictions.Deserialize(reader);
        }
        
    }
    
}