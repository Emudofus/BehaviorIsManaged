

// Generated on 04/17/2013 22:29:38
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class CharactersListErrorMessage : NetworkMessage
    {
        public const uint Id = 5545;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public CharactersListErrorMessage()
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