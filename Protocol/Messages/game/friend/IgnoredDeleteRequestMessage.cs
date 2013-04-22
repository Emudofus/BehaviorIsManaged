

// Generated on 04/17/2013 22:29:53
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class IgnoredDeleteRequestMessage : NetworkMessage
    {
        public const uint Id = 5680;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int accountId;
        public bool session;
        
        public IgnoredDeleteRequestMessage()
        {
        }
        
        public IgnoredDeleteRequestMessage(int accountId, bool session)
        {
            this.accountId = accountId;
            this.session = session;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(accountId);
            writer.WriteBoolean(session);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            accountId = reader.ReadInt();
            if (accountId < 0)
                throw new Exception("Forbidden value on accountId = " + accountId + ", it doesn't respect the following condition : accountId < 0");
            session = reader.ReadBoolean();
        }
        
    }
    
}