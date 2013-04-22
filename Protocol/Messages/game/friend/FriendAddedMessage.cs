

// Generated on 04/17/2013 22:29:52
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class FriendAddedMessage : NetworkMessage
    {
        public const uint Id = 5599;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.FriendInformations friendAdded;
        
        public FriendAddedMessage()
        {
        }
        
        public FriendAddedMessage(Types.FriendInformations friendAdded)
        {
            this.friendAdded = friendAdded;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(friendAdded.TypeId);
            friendAdded.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            friendAdded = Types.ProtocolTypeManager.GetInstance<Types.FriendInformations>(reader.ReadShort());
            friendAdded.Deserialize(reader);
        }
        
    }
    
}