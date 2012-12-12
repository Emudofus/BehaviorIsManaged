

// Generated on 12/11/2012 19:44:23
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class IgnoredAddedMessage : NetworkMessage
    {
        public const uint Id = 5678;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.IgnoredInformations ignoreAdded;
        public bool session;
        
        public IgnoredAddedMessage()
        {
        }
        
        public IgnoredAddedMessage(Types.IgnoredInformations ignoreAdded, bool session)
        {
            this.ignoreAdded = ignoreAdded;
            this.session = session;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(ignoreAdded.TypeId);
            ignoreAdded.Serialize(writer);
            writer.WriteBoolean(session);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            ignoreAdded = Types.ProtocolTypeManager.GetInstance<Types.IgnoredInformations>(reader.ReadShort());
            ignoreAdded.Deserialize(reader);
            session = reader.ReadBoolean();
        }
        
    }
    
}