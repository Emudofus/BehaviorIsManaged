

// Generated on 04/17/2013 22:29:33
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class NicknameRegistrationMessage : NetworkMessage
    {
        public const uint Id = 5640;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public NicknameRegistrationMessage()
        {
        }
        
        
        public override void Serialize(IDataWriter writer)
        {
        }
        
        public override void Deserialize(IDataReader reader)
        {
        }
        
    }
    
}